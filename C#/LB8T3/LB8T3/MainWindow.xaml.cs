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
using System.Data;


namespace LB8T3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DAL da = new DAL();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //DataTable dt = da.givestock();
            //stockGrid.ItemsSource = dt.DefaultView;
            List<Stock> itemlist = da.getstocklist();
            this.stockGrid.ItemsSource = itemlist;
            stockGrid.Columns[0].Visibility= Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (stockGrid.SelectedItems != null)
            {
                Stock dv = (Stock)stockGrid.SelectedItems[0];
                //int i = dv.id;
                da.predict(dv);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (stockGrid.SelectedItems != null)
            {
                Stock dv = (Stock)stockGrid.SelectedItems[0];
                //int i = dv.id;
                DataTable dt = da.showmat(dv);
                matGrid.ItemsSource = dt.DefaultView;
                
            }
        }



        private void OnKeyDownHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == DataGrid.DeleteCommand)
            {
                try
                {
                    Stock dv = (Stock)stockGrid.SelectedItems[0];
                    //int i = dv.id;
                    
                    da.deleterow(dv);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
               
            }
        }
        private void EditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Stock st = e.Row.Item as Stock;
            string ind = stockGrid.CurrentCell.Column.Header.ToString();
            TextBox t = e.EditingElement as TextBox;
            string ntext = t.Text.ToString();
            da.edition(st, ind, ntext);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Adding ad = new Adding();
            ad.Show();
            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            if (matGrid.SelectedItems != null)
            {
                int i = Convert.ToInt32(((DataRowView)matGrid.SelectedValue)["id"]);
                DataTable dt = da.showsup(i);
                supGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (matGrid.SelectedItems != null)
            {
                int i = Convert.ToInt32(((DataRowView)matGrid.SelectedValue)["id"]);
                List<Supplier> supl = da.getsup(i);
                List<Sup_mat> smat = da.show_sup_mat();
                //DataTable newdt = new DataTable();
                //DataColumn col;
                //DataRow row;
                //col = new DataColumn();
                //col.DataType=System.Type.GetType("System.DateTime");
                //col.ColumnName = "delivery";
                //newdt.Columns.Add(col);
                //col = new DataColumn();
                //col.DataType = System.Type.GetType("System.Int32");
                //col.ColumnName = "quantity";
                //newdt.Columns.Add(col);
                //col = new DataColumn();
                //col.DataType = System.Type.GetType("System.Single");
                //col.ColumnName = "cost";
                //newdt.Columns.Add(col);
                
                //var q = from c in dt.AsEnumerable() join
                //        s in supdt.AsEnumerable() on c.Field<int>("sup_id") equals s.Field<int>("id")
                //        where c.Field<int>("mat_id") == i
                //        select c;
                var q = from c in smat
                        join s in supl on c.supid equals s.id
                        where c.matid == i
                        select c;
                this.supGrid.ItemsSource = q.ToList();
                //foreach (DataRow dr in q)
                //{
                //    row = newdt.NewRow();
                //    row["delivery"] = dr[2];
                //    row["cost"] = dr[3];
                //    row["quantity"] = dr[4];
                //    newdt.Rows.Add(row);
                //}
                //supGrid.ItemsSource = newdt.DefaultView;
            }
        }
    }
}
 

