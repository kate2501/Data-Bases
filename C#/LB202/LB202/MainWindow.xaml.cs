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
using System.Text.RegularExpressions;
using System.Collections;
using System.Transactions;

namespace LB202
{

    public partial class MainWindow : Window
    {

        public DataSet shopDs = new DataSet("Shop");
        private string conStr = string.Empty;
        private MySqlDataAdapter collAdapter;
        private MySqlDataAdapter materialAdapter;
        private MySqlDataAdapter stmatAdapter;
        private MySqlDataAdapter stockAdapter;
        private MySqlDataAdapter supmatAdpater;
        private MySqlDataAdapter supAdpater;

        private MySqlCommandBuilder collCommands;
        private MySqlCommandBuilder materialCommands;
        private MySqlCommandBuilder stmatCommands;
        private MySqlCommandBuilder stockCommands;
        private MySqlCommandBuilder supmatCommands;
        private MySqlCommandBuilder supCommands;

        private Stack myStack = new Stack();
        List<DataRow> rows = new List<DataRow>();



        public MainWindow()
        {
            InitializeComponent();
            conStr = ConfigurationManager.ConnectionStrings["shopConnROOT"].ConnectionString;
            collAdapter = new MySqlDataAdapter("select * from collection", conStr);
            materialAdapter = new MySqlDataAdapter("select * from material", conStr);
            stmatAdapter = new MySqlDataAdapter("select * from st_mat", conStr);
            stockAdapter = new MySqlDataAdapter("select * from stock", conStr);
            supmatAdpater = new MySqlDataAdapter("select * from sup_mat", conStr);
            supAdpater = new MySqlDataAdapter("select * from supplier", conStr);



            collCommands = new MySqlCommandBuilder(collAdapter);
            materialCommands = new MySqlCommandBuilder(materialAdapter);
            stmatCommands = new MySqlCommandBuilder(stmatAdapter);
            stockCommands = new MySqlCommandBuilder(stockAdapter);
            supmatCommands = new MySqlCommandBuilder(supmatAdpater);
            supCommands = new MySqlCommandBuilder(supAdpater);

            collAdapter.Fill(shopDs, "collection");
            materialAdapter.Fill(shopDs, "material");
            stmatAdapter.Fill(shopDs, "st_mat");
            stockAdapter.Fill(shopDs, "stock");
            supmatAdpater.Fill(shopDs, "sup_mat");
            supAdpater.Fill(shopDs, "supplier");

            stockAdapter.InsertCommand = new MySqlCommand("createRow");
            stockAdapter.InsertCommand.CommandType = CommandType.StoredProcedure;
            stockAdapter.InsertCommand.Parameters.Add(new MySqlParameter("cost", MySqlDbType.Float, 0, "cost"));
            stockAdapter.InsertCommand.Parameters.Add(new MySqlParameter("stype", MySqlDbType.VarChar, 20, "s_type"));
            stockAdapter.InsertCommand.Parameters.Add(new MySqlParameter("syear", MySqlDbType.Year, 0, "s_year"));
            stockAdapter.InsertCommand.Parameters.Add(new MySqlParameter("sdesc", MySqlDbType.VarChar, 50, "s_desc"));
            stockAdapter.InsertCommand.Parameters.Add(new MySqlParameter("ref", MySqlDbType.Int16, 0, "ref_coll"));
            stockAdapter.InsertCommand.Parameters.Add(new MySqlParameter("d", MySqlDbType.Date, 0, "s_date"));
            MySqlParameter param = new MySqlParameter("id", MySqlDbType.Int32, 0, "id");
            param.Direction = ParameterDirection.Output;
            stockAdapter.InsertCommand.Parameters.Add(param);

            materialAdapter.DeleteCommand = new MySqlCommand("DeleteRow");
            materialAdapter.DeleteCommand.CommandType = CommandType.StoredProcedure;
            materialAdapter.DeleteCommand.Parameters.Add(new MySqlParameter("idd", MySqlDbType.Int16, 0, "id")).SourceVersion = DataRowVersion.Original; ;


            combo.Items.Add("Show materials");
            combo.Items.Add("Show suppliers");
            combo.Items.Add("Show collection");
            combo.Items.Add("Predict cost");
            combo.Items.Add("Similar suppliers");
            combo.Items.Add("Similar collections");
            combo.Items.Add("Most popular supplier");
            combo.Items.Add("Most expensive item");
            combo.Items.Add("Sort materials by cost");
            combo.Items.Add("Min in mat. ucost");
            combo.Items.Add("Max in mat. ucost");
            combo.Items.Add("Items by year");

            cancel.Items.Add("Cancel all changes");
            cancel.Items.Add("Cancel del.rows");
            cancel.Items.Add("Cancel row modifing");
            cancel.Items.Add("Cancel modifing last row");

            List<DataRow> logrow = new List<DataRow>();
            

            foreach (DataColumn column in shopDs.Tables["stock"].Columns)
            {
                prop.Items.Add(column.ColumnName);
            }

            shopDs.Tables["stock"].PrimaryKey = new DataColumn[] { shopDs.Tables["stock"].Columns["id"] };
            shopDs.Tables["material"].PrimaryKey = new DataColumn[] { shopDs.Tables["material"].Columns["id"] };
            shopDs.Tables["collection"].PrimaryKey = new DataColumn[] { shopDs.Tables["collection"].Columns["id"] };
            shopDs.Tables["supplier"].PrimaryKey = new DataColumn[] { shopDs.Tables["supplier"].Columns["id"] };
            shopDs.Tables["st_mat"].PrimaryKey = new DataColumn[] { shopDs.Tables["st_mat"].Columns["st_id"], shopDs.Tables["st_mat"].Columns["mat_id"] };
            shopDs.Tables["sup_mat"].PrimaryKey = new DataColumn[] { shopDs.Tables["sup_mat"].Columns["sup_id"], shopDs.Tables["sup_mat"].Columns["mat_id"], shopDs.Tables["sup_mat"].Columns["deliver"] };

            setautoincr("stock");
            setautoincr("material");
            setautoincr("collection");
            setautoincr("supplier");

            DataRelation collToStock = new DataRelation("collToStock", shopDs.Tables["collection"].Columns["id"], shopDs.Tables["stock"].Columns["ref_coll"]);
            shopDs.Relations.Add(collToStock);
            collToStock.ChildKeyConstraint.DeleteRule = Rule.Cascade;

            DataRelation matToSt_mat = new DataRelation("matToSt_mat", shopDs.Tables["material"].Columns["id"], shopDs.Tables["st_mat"].Columns["mat_id"]);
            shopDs.Relations.Add(matToSt_mat);
            matToSt_mat.ChildKeyConstraint.DeleteRule = Rule.Cascade;

            DataRelation stToSt_mat = new DataRelation("stToSt_mat", shopDs.Tables["stock"].Columns["id"], shopDs.Tables["st_mat"].Columns["st_id"]);
            shopDs.Relations.Add(stToSt_mat);
            stToSt_mat.ChildKeyConstraint.DeleteRule = Rule.Cascade;

            DataRelation matToSup_mat = new DataRelation("matToSup_mat", shopDs.Tables["material"].Columns["id"], shopDs.Tables["sup_mat"].Columns["mat_id"]);
            shopDs.Relations.Add(matToSup_mat);
            matToSup_mat.ChildKeyConstraint.DeleteRule = Rule.Cascade;

            DataRelation supToSup_mat = new DataRelation("supToSup_mat", shopDs.Tables["supplier"].Columns["id"], shopDs.Tables["sup_mat"].Columns["sup_id"]);
            shopDs.Relations.Add(supToSup_mat);
            supToSup_mat.ChildKeyConstraint.DeleteRule = Rule.Cascade;

        }

        private void setautoincr(string name)
        {
            shopDs.Tables[name].Columns["id"].AutoIncrement = true;
            var lastId = (from m in shopDs.Tables[name].AsEnumerable()
                          select m["id"]).Max();
            shopDs.Tables[name].Columns["id"].AutoIncrementSeed = (int)lastId + 1;
            shopDs.Tables[name].Columns["id"].AutoIncrementStep = 1;
        }


        private bool check(int col, string str)
        {
            if (col == 2)
            {
                if (Int16.Parse(str) < 0)
                {
                    return false;
                }
                else return true;

            }
            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grid.ItemsSource = shopDs.Tables["stock"].DefaultView;
            matGrid.ItemsSource = shopDs.Tables["material"].DefaultView;
            grid.Columns[0].Visibility = Visibility.Hidden;
            grid.Columns[7].Visibility = Visibility.Hidden;
            //matGrid.Columns[0].Visibility = Visibility.Hidden;
            matGrid.Columns[4].Visibility = Visibility.Hidden;
            matGrid.Columns[5].Visibility = Visibility.Hidden;
            resGrid.ItemsSource = shopDs.Tables["collection"].DefaultView;

        }

        private void showProperty(string colname)
        {
            string colpr = "";

            colpr += "Name: " + shopDs.Tables["stock"].Columns[colname].ColumnName + "\n";
            string type = " " + shopDs.Tables["stock"].Columns[colname].DataType;
            if (type == " System.DateTime")
            {
                colpr += "Type: " + "Date" + "\n";
            }
            else
            {
                colpr += "Type: " + shopDs.Tables["stock"].Columns[colname].DataType + "\n";
            }
            colpr += "Allow null: " + shopDs.Tables["stock"].Columns[colname].AllowDBNull + "\n";
            colpr += "AutoIncrement: " + shopDs.Tables["stock"].Columns[colname].AutoIncrement + "\n";
            colpr += "Unique: " + shopDs.Tables["stock"].Columns[colname].Unique + "\n";
            colpr += "Number of primary keys: " + shopDs.Tables["stock"].PrimaryKey.Length + "\n";

            MessageBox.Show(colpr);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var path = @"C:\Users\diens\OneDrive\Рабочий стол\SQL\SQL\LB202\logs.txt";
            System.IO.File.WriteAllText(path, string.Empty);
            stockAdapter.Update(shopDs, "stock");

            materialAdapter.Update(shopDs, "material");
            collAdapter.Update(shopDs, "collection");
            shopDs.AcceptChanges();
        }


        private void Show_Click(object sender, RoutedEventArgs e)
        {
            string str = combo.SelectedItem.ToString();

            switch (str)
            {
                case "Show materials":
                    if (grid.SelectedItems != null)
                    {
                        DataRowView dataRow = (DataRowView)grid.SelectedItem;
                        int cellValue = Convert.ToInt32(dataRow.Row.ItemArray[0]);
                        MessageBox.Show(cellValue.ToString());
                        var query = (from t in shopDs.Tables["st_mat"].AsEnumerable()
                                     join m in shopDs.Tables["stock"].AsEnumerable()
                                                  on t["st_id"] equals m["id"]
                                     join p in shopDs.Tables["material"].AsEnumerable()
                                     on t["mat_id"] equals p["id"]
                                     where (int)m["id"] == cellValue
                                     group new { t, m, p } by p.Field<string>("name") into g
                                     select new
                                     {
                                         name = g.Key,
                                         quan = g.Sum(x => x.t.Field<float>("quantity"))
                                     }
                                     ).ToList();
                        resGrid.ItemsSource = query;
                    }
                    break;
                case "Show suppliers":
                    if (matGrid.SelectedItems != null)
                    {
                        DataRowView dataRow = (DataRowView)matGrid.SelectedItem;
                        int cellValue = Convert.ToInt32(dataRow.Row.ItemArray[0]);
                        MessageBox.Show(cellValue.ToString());
                        var query = (from t in shopDs.Tables["sup_mat"].AsEnumerable()
                                     join m in shopDs.Tables["material"].AsEnumerable()
                                                  on t["mat_id"] equals m["id"]
                                     join p in shopDs.Tables["supplier"].AsEnumerable()
                                     on t["sup_id"] equals p["id"]
                                     where (int)m["id"] == cellValue
                                     select new
                                     {
                                         name = p["name"],
                                         manager = p["manager"],
                                         address = p["address"],
                                         start_date = p["start_date"],
                                         end_date = p["end_date"]
                                     }

                                     ).Distinct().ToList();
                        resGrid.ItemsSource = query;
                    }
                    break;
                case "Show collection":
                    if (grid.SelectedItems != null)
                    {
                        DataRowView dataRow = (DataRowView)grid.SelectedItem;
                        int cellValue = Convert.ToInt32(dataRow.Row.ItemArray[0]);
                        MessageBox.Show(cellValue.ToString());
                        var query = (from m in shopDs.Tables["collection"].AsEnumerable()
                                     join t in shopDs.Tables["stock"].AsEnumerable()
                                     on m["id"] equals t["ref_coll"]
                                     where (int)t["id"] == cellValue
                                     select
                                     new
                                     {
                                         name = m["name"],
                                         s_class = m["s_class"],
                                         st_date = m["st_date"],
                                         end_date = m["end_date"]
                                     }
                                     ).ToList();
                        resGrid.ItemsSource = query;
                    }
                    break;
                case "Predict cost":
                    if (grid.SelectedItems != null)
                    {
                        DataRowView dataRow = (DataRowView)grid.SelectedItem;
                        int cellValue = Convert.ToInt32(dataRow.Row.ItemArray[0]);
                        MessageBox.Show(cellValue.ToString());
                        var query = from m in shopDs.Tables["st_mat"].AsEnumerable()
                                    join t in shopDs.Tables["material"].AsEnumerable()
                                    on m["mat_id"] equals t["id"]
                                    where (int)m["st_id"] == cellValue
                                    select new
                                    {
                                        id = cellValue,
                                        mm = m.Field<float>("quantity"),
                                        tt = t.Field<float>("u_cost")
                                    } into res
                                    group res by res.id into g
                                    select new
                                    {
                                        cost = g.Sum(x => x.mm * x.tt) * 1.2
                                    };
                        resGrid.ItemsSource = query;
                    }
                    break;
                case "Similar suppliers":
                    String text = sim.Text;
                    var query1 = (from p in shopDs.Tables["supplier"].AsEnumerable()
                                  where ComputeSimilarity.CalculateSimilarity((string)p["name"], text) > 0.5
                                  select new
                                  {
                                      name = p["name"],
                                      manager = p["manager"],
                                      address = p["address"],
                                      start_date = p["start_date"],
                                      end_date = p["end_date"]
                                  }
                                ).ToList();
                    resGrid.ItemsSource = query1;
                    break;
                case "Similar collections":
                    String text1 = sim.Text;
                    var query6 = (from m in shopDs.Tables["collection"].AsEnumerable()
                                  where ComputeSimilarity.CalculateSimilarity((string)m["name"], text1) > 0.5
                                  select
                                     new
                                     {
                                         name = m["name"],
                                         s_class = m["s_class"],
                                         st_date = m["st_date"],
                                         end_date = m["end_date"]
                                     }
                                ).ToList();
                    resGrid.ItemsSource = query6;
                    break;
                case "Most popular supplier":
                    var query2 = (from m in shopDs.Tables["sup_mat"].AsEnumerable()
                                  group m by m["sup_id"] into g
                                  select new
                                  {
                                      id = g.Key,
                                      quant = g.Count()
                                  } into res
                                  join p in shopDs.Tables["supplier"].AsEnumerable()
                                  on res.id equals p["id"]
                                  orderby res.quant descending
                                  select new
                                  {
                                      name = p["name"],
                                      manager = p["manager"],
                                      address = p["address"],
                                      purchase = res.quant
                                  }).ToList();
                    resGrid.ItemsSource = query2;

                    break;
                case "Most expensive item":
                    var query8 = (from m in shopDs.Tables["stock"].AsEnumerable()
                                  orderby m["cost"] descending
                                  select new
                                  {
                                      cost = m["cost"],
                                      s_type = m["s_type"],
                                      s_year = m["s_year"],
                                      s_desc = m["s_desc"]
                                  }
                                  ).ToList();
                    resGrid.ItemsSource = query8;
                    break;
                case "Sort materials by cost":
                    var query3 = shopDs.Tables["material"].AsEnumerable().OrderBy(x => x.Field<float>("u_cost")).Select(
                        m =>
                        new
                        {
                            name = m["name"],
                            property = m["property"],
                            u_cost = m["u_cost"]
                        }
                        ).ToList();
                    resGrid.ItemsSource = query3;
                    break;
                case "Min in mat. ucost":
                    var query4 = shopDs.Tables["material"].AsEnumerable().Min(x => x.Field<float>("u_cost"));
                    MessageBox.Show("Min mat. ucost=" + query4.ToString());
                    break;
                case "Max in mat. ucost":
                    var query5 = shopDs.Tables["material"].AsEnumerable().Max(x => x.Field<float>("u_cost"));
                    MessageBox.Show("Max mat. ucost=" + query5.ToString());
                    break;
                case "Items by year":
                    int text2 = Convert.ToInt32(sim.Text);
                    var query7 = (from m in shopDs.Tables["stock"].AsEnumerable()
                                  where Convert.ToInt32(m["s_year"]) == text2
                                  select new
                                  {
                                      cost = m["cost"],
                                      s_type = m["s_type"],
                                      s_year = m["s_year"],
                                      s_desc = m["s_desc"],

                                  }).ToList();
                    resGrid.ItemsSource = query7;
                    break;
                default:
                    MessageBox.Show("Something went wrong!");
                    break;
            }
        }

        private void matGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            rows.Clear();
            int row1 = e.Row.GetIndex();
            int col1 = e.Column.DisplayIndex;
            string edit = ((TextBox)e.EditingElement).Text;
            if (shopDs.Tables["material"].Rows.Count>row1)
            {
                shopDs.Tables["material"].Rows[row1][col1] = edit;
                DataRow row = shopDs.Tables["material"].Rows[row1];
                DataRow newrow = shopDs.Tables["material"].NewRow();
                newrow.ItemArray= (object[])row.ItemArray.Clone();
                rows.Add(newrow);
            }

        }


        private void DataGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                string edit = ((TextBox)e.EditingElement).Text;
                int col1 = e.Column.DisplayIndex;
                int row1 = e.Row.GetIndex();
                shopDs.Tables["stock"].AcceptChanges();
                var r = shopDs.Tables["stock"].Rows[row1].ItemArray;
                String[] row =
                {
                    r.GetValue(col1).ToString(), row1.ToString(),col1.ToString()
                };
                //MessageBox.Show(string.Join(" ", row));
                shopDs.Tables["stock"].Rows[row1][col1] = edit;
                grid.ItemsSource = shopDs.Tables["stock"].DefaultView;
                myStack.Push(row);
            }
        }


        private void Prop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showProperty(prop.SelectedItem.ToString());
        }

        private void form_open(object sender, KeyEventArgs e)
        {
            try
            {
                DataRowView dataRow = (DataRowView)resGrid.SelectedItem;
                int cellValue = Convert.ToInt32(dataRow.Row.ItemArray[0]);
                if (e.Key == Key.F1)
                {
                    Adding f = new Adding();
                    f.ShowDialog();
                    DataRow dr = shopDs.Tables["stock"].NewRow();
                    dr["cost"] = Convert.ToDouble(f.cost);
                    dr["s_type"] = f.type;
                    dr["s_year"] = Convert.ToInt32(f.year);
                    dr["s_desc"] = f.desc;
                    dr["ref_coll"] = cellValue;
                    MessageBox.Show(f.cost + " " + f.type + " " + f.year + " " + f.desc);
                    shopDs.Tables["stock"].Rows.Add(dr);
                    grid.ItemsSource = null;
                    grid.ItemsSource = shopDs.Tables["stock"].DefaultView;

                    grid.Columns[0].Visibility = Visibility.Hidden;
                    grid.Columns[7].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = grid.SelectedItems.Count - 1; i >= 0; i--)
                {
                    DataRowView drv = grid.SelectedItems[i] as DataRowView;
                    if (drv != null)
                    {
                        drv.Row.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                grid.SelectedItems.Clear();
                //stockAdapter.Update(shopDs, "stock");
            }
        }

        private void check()
        {

        }

        private void Cancel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = cancel.SelectedItem.ToString();
            switch (str)
            {
                case "Cancel all changes":
                    {
                        shopDs.RejectChanges();
                        break;
                    }
                case "Cancel del.rows":
                    {
                        int val;
                        foreach (DataRow r in shopDs.Tables["material"].Rows)
                        {
                            if (r.RowState.ToString() == "Deleted")
                            {
                                r.RejectChanges();
                                if (rows[0][0].ToString() == r.ItemArray[0].ToString())
                                {
                                    var res = (from p in shopDs.Tables["material"].AsEnumerable()
                                               where (p.Field<int>("id")).ToString() == r.ItemArray[0].ToString()
                                               select p).First() as DataRow;

                                    val = shopDs.Tables["material"].Rows.IndexOf(res);
                                    shopDs.Tables["material"].Rows[val].ItemArray = (object[])rows[0].ItemArray.Clone();

                                }

                            }
                        }

                        break;
                    }
                case "Cancel row modifing":
                    {
                        foreach (DataTable tb in shopDs.Tables)
                        {
                            foreach (DataRow r in tb.Rows)
                            {
                                if (r.RowState.ToString() == "Modified")
                                {
                                    r.RejectChanges();
                                }
                            }
                        }
                        break;
                    }
                case "Cancel modifing last row":
                    {

                        var row = ((IEnumerable)myStack.Peek()).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
                        //MessageBox.Show(row[0]+" "+row[1]+" "+row[2]);
                        shopDs.Tables["stock"].Rows[Convert.ToInt16(row[1])][Convert.ToInt16(row[2])] = row[0];
                        grid.ItemsSource = shopDs.Tables["stock"].DefaultView;
                        //lastRow.RejectChanges();
                        break;
                    }
            }
        }

        private void Edit_rows_Click(object sender, RoutedEventArgs e)
        {
            (from p in shopDs.Tables["stock"].AsEnumerable()
             where p.Field<int>("ref_coll") == 1
             select p).ToList<DataRow>().ForEach(row => { row["cost"] = 1.15 * Convert.ToInt32(row["cost"]); });
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            (from p in shopDs.Tables["stock"].AsEnumerable()
             where p.Field<int>("ref_coll") == 2
             select p).ToList<DataRow>().ForEach(row => { row.Delete(); });
        }


       



        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //using (TransactionScope proccess = new TransactionScope())
            //{
            //    (from p in shopDs.Tables["stock"].AsEnumerable()
            //     select p).ToList<DataRow>().ForEach(row => { row["cost"] = 1.15 * Convert.ToInt32(row["cost"]); });
            //    stockAdapter.Update(shopDs, "stock");
            //    proccess.Complete();

            //}
            DataView dataView = new DataView(shopDs.Tables["material"]);
            dataView.RowStateFilter = DataViewRowState.ModifiedCurrent;
            matGrid.ItemsSource = dataView;
           

        }

        private void MatGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }
    }
}

// удаление
// сконф delete команду