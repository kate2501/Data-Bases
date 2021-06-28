using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;



namespace LB9Ex
{

    public partial class MainWindow : Window
    {
        private DataSet shopDs = new DataSet("Shop");
        private string conStr = string.Empty;
        private MySqlDataAdapter manufAdapter;
        private MySqlDataAdapter productAdapter;
        private MySqlDataAdapter customerAdapter;
        private MySqlDataAdapter prcustAdapter;

        private MySqlCommandBuilder manufCommands;
        private MySqlCommandBuilder productCommands;
        private MySqlCommandBuilder customerCommands;
        private MySqlCommandBuilder prcustCommands;

        public MainWindow()
        {
            InitializeComponent();
            conStr = ConfigurationManager.ConnectionStrings["shopConnROOT"].ConnectionString;
            manufAdapter = new MySqlDataAdapter("select * from manufact", conStr);
            productAdapter = new MySqlDataAdapter("select * from product", conStr);
            customerAdapter = new MySqlDataAdapter("select * from customer", conStr);
            prcustAdapter = new MySqlDataAdapter("select * from pr_cust", conStr);


            manufCommands = new MySqlCommandBuilder(manufAdapter);
            productCommands = new MySqlCommandBuilder(productAdapter);
            customerCommands = new MySqlCommandBuilder(customerAdapter);
            prcustCommands = new MySqlCommandBuilder(prcustAdapter);


            manufAdapter.Fill(shopDs, "manufact");
            productAdapter.Fill(shopDs, "product");
            customerAdapter.Fill(shopDs, "customer");
            prcustAdapter.Fill(shopDs, "pr_cust");

            
           

            DataRelation manufToProduct = new DataRelation("ManufProduct", shopDs.Tables["manufact"].Columns["manuf_id"],
                shopDs.Tables["product"].Columns["prod_manuf_ref_id"]);
            DataRelation prcustToProduct = new DataRelation("PrCustProduct", shopDs.Tables["product"].Columns["prod_id"],
                shopDs.Tables["pr_cust"].Columns["prod_id"]);

            DataColumn p = shopDs.Tables["customer"].Columns["cust_id"];
            DataColumn c = shopDs.Tables["pr_cust"].Columns["cust_id"];
            DataRelation prcustToCust = new DataRelation("PrCustCust", p, c);
            //DataRelation prcustToCust = new DataRelation("PrCustCust", shopDs.Tables["customer"].Columns["cust_id"],
            //shopDs.Tables["pr_cast"].Columns["cust_id"]);

            shopDs.Relations.Add(manufToProduct);
            shopDs.Relations.Add(prcustToProduct);
            shopDs.Relations.Add(prcustToCust);

            var query = (from m in shopDs.Tables["manufact"].AsEnumerable()
                        select m["manuf_name"]).Distinct().ToList();
            foreach(var r in query)
            {
                comboBox1.Items.Add(r.ToString());
            }
            var query1 = (from m in shopDs.Tables["product"].AsEnumerable()
                         select m["prod_type"]).Distinct().ToList();
            foreach (var r in query1)
            {
                comboBox2.Items.Add(r.ToString());
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            manufGrid.ItemsSource = shopDs.Tables["manufact"].DefaultView;
        }

        private void RangeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double a = Convert.ToDouble(textBoxA.Text);
                double b =Convert.ToDouble(textBoxB.Text);
                if (b > a && a >= 0)
                {
                    var result = shopDs.Tables["product"].Select($"prod_price>{a} and prod_price<{b}");
                    //var query = (from m in shopDs.Tables["product"].AsEnumerable()
                    //             join t in shopDs.Tables["manufact"].AsEnumerable()
                    //             on m["prod_manuf_ref_id"] equals t["manuf_id"]
                    //             where (float)m["prod_price"] > a && (float)m["prod_price"] < b
                    //             select new
                    //             {
                    //                 Name = m["prod_name"],
                    //                 Manuf = t["manuf_name"],
                    //                 Price=m["prod_price"]
                    //             }
                    //       ).ToList();
                    resultGrid.Visibility = Visibility.Visible;
                    resultGrid.ItemsSource = result.CopyToDataTable().DefaultView;
                }
                else
                {
                    MessageBox.Show("Wrong input! Try again");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string str = comboBox1.SelectedItem.ToString();
                var query= (from m in shopDs.Tables["product"].AsEnumerable()
                            join t in shopDs.Tables["manufact"].AsEnumerable()
                            on m["prod_manuf_ref_id"] equals t["manuf_id"]
                            where (string)t["manuf_name"] ==str
                            select new
                            {
                                name=m["prod_name"],
                                type=m["prod_type"],
                                quantity=m["prod_quantity"],
                                price=m["prod_price"]
                            }
                           ).ToList();
                resultGrid.Visibility = Visibility.Visible;
                resultGrid.ItemsSource = query;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TypeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string str = comboBox2.SelectedItem.ToString();
                var query = (from m in shopDs.Tables["product"].AsEnumerable()
                             join t in shopDs.Tables["manufact"].AsEnumerable()
                             on m["prod_manuf_ref_id"] equals t["manuf_id"]
                             where (string)m["prod_type"] == str
                             select new
                             {
                                 manuf = t["manuf_name"],
                                 name = m["prod_name"],
                                 type = m["prod_type"],
                                 quantity = m["prod_quantity"],
                                 price = m["prod_price"]
                             }).ToList();
                resultGrid.Visibility = Visibility.Visible;
                resultGrid.ItemsSource = query;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String text = sim.Text;
            double percentage;
           
            try
            {
                var query = (from m in shopDs.Tables["product"].AsEnumerable()
                            select (string)m["prod_name"]).ToList();
                List<String> names = new List<String>();
                foreach (var r in query)
                {
                    percentage = ComputeSimilarity.CalculateSimilarity(r, text);
                    if(percentage>0.5)
                    {
                        names.Add(r);
                    }
                }
                if (!names.Any())
                {
                    MessageBox.Show("No similar names found");
                }
                else
                {
                    foreach (String r in names)
                    {
                        MessageBox.Show(r);
                    }
                }
            }
            
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Update_click_1(object sender, RoutedEventArgs e)
        {
            manufAdapter.Update(shopDs, "manufact");
        }

        private void Prod_Click(object sender, RoutedEventArgs e)
        {
            var query = (from m in shopDs.Tables["product"].AsEnumerable()
                         join t in shopDs.Tables["pr_cust"].AsEnumerable()
                         on m["prod_id"] equals t["prod_id"]
                         group new { m, t }
                         by m.Field<string>("prod_name") into mt
                         select new
                         {
                             prodName = mt.Key,
                             prodQuantity = mt.Sum(x => x.t.Field<int>("quantity"))
                         }).ToList();
            resultGrid.Visibility = Visibility.Visible;
            resultGrid.ItemsSource = query;

        }

        private void Cust_Click(object sender, RoutedEventArgs e)
        {
            var query = from k in (from m in shopDs.Tables["customer"].AsEnumerable()
                                   join t in shopDs.Tables["pr_cust"].AsEnumerable()
                                   on m["cust_id"] equals t["cust_id"]
                                   group new { m, t }
                                   by m.Field<string>("cust_name") into mt
                                   select new
                                   {
                                       custName = mt.Key,
                                       prodQuantity = mt.Sum(x => x.t.Field<int>("quantity"))
                                   }).ToList()
                        orderby k.prodQuantity
                        select k;

                        resultGrid.Visibility = Visibility.Visible;
                        resultGrid.ItemsSource = query;
        }
    }
}
