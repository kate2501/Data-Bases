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
using MySql.Data.MySqlClient;

namespace DB1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DAL da = new DAL();
        string str;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         
            if (rbutton1.IsChecked == true)
            {
                if (prgrid.SelectedItems != null)
                {
                    DataRowView dv = prgrid.SelectedItems[0] as DataRowView;
                    int id = Convert.ToInt16(dv.Row.ItemArray[0]);
                    da.OpenConnection(str);
                    hostgrid.ItemsSource = da.giveHosts(id).DefaultView;
                    hostgrid.Visibility = System.Windows.Visibility.Visible;
                    da.CloseConnection();
                    
                }
            }
            if (rbutton2.IsChecked == true)
            {
                DataRowView dv = prgrid.SelectedItems[0] as DataRowView;
                int id = Convert.ToInt16(dv.Row.ItemArray[0]);
                da.OpenConnection(str);
                da.sale(id, Int32.Parse(vbox.Text));
                DataTable dr = da.giveCAsDataTable();
                prgrid.ItemsSource = dr.DefaultView;
                da.CloseConnection();
                vbox.Text = "";
            }

        }
        private void Rbutton2_Checked(object sender, RoutedEventArgs e)
        {

                da.OpenConnection(str);
                button.Visibility = System.Windows.Visibility.Visible;
                vbox.Visibility = System.Windows.Visibility.Visible;
                prgrid.Visibility = System.Windows.Visibility.Visible;
                button.Content = "Low price";
                hostgrid.Visibility = System.Windows.Visibility.Hidden;
                DataTable dr = da.giveCAsDataTable();
                prgrid.ItemsSource = dr.DefaultView;
                da.CloseConnection();
        }

        private void Rbutton1_Checked(object sender, RoutedEventArgs e)
        {
            prgrid.Visibility = System.Windows.Visibility.Visible;
            button.Visibility = System.Windows.Visibility.Visible;
            vbox.Visibility = System.Windows.Visibility.Hidden;
            button.Content = "Show hosts for selected";
            da.OpenConnection(str);
            DataTable dr = da.givePrAsDataTable();
            prgrid.ItemsSource = dr.DefaultView;
            da.CloseConnection();
        }

        private void Rbutton3_Checked(object sender, RoutedEventArgs e)
        {
            button.Visibility = System.Windows.Visibility.Hidden;
            prgrid.Visibility = System.Windows.Visibility.Visible;
            vbox.Visibility = System.Windows.Visibility.Hidden;
            da.OpenConnection(str);
            DataTable dr = da.giveSpAsDataTable();
            prgrid.ItemsSource = dr.DefaultView;
            da.CloseConnection();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string user = userbox.Text;
            string pass = passwordbox.Password.ToString();
            prgrid.Visibility = System.Windows.Visibility.Hidden;
            hostgrid.Visibility = System.Windows.Visibility.Hidden;
            button.Visibility = System.Windows.Visibility.Hidden;
            vbox.Visibility = System.Windows.Visibility.Hidden;
            label1.Content = "";
            str = String.Format("server=localhost; user id={0}; password={1}; database=OryoliReshka", user, pass);
            try
            {
                da.OpenConnection(str);
                da.CloseConnection();
                switch(user)
                {
                    case "managers":
                        rbutton1.Visibility = System.Windows.Visibility.Visible;
                        rbutton2.Visibility = System.Windows.Visibility.Hidden;
                        rbutton3.Visibility = System.Windows.Visibility.Hidden;
                        break;
                    case "sponsor":
                        rbutton3.Visibility = System.Windows.Visibility.Visible;
                        rbutton2.Visibility = System.Windows.Visibility.Hidden;
                        rbutton1.Visibility = System.Windows.Visibility.Hidden;
                        break;
                    case "tripadmin":
                        rbutton2.Visibility = System.Windows.Visibility.Visible;
                        rbutton3.Visibility = System.Windows.Visibility.Hidden;
                        rbutton1.Visibility = System.Windows.Visibility.Hidden;
                        break;
                    default:
                        rbutton1.Visibility = System.Windows.Visibility.Visible;
                        rbutton2.Visibility = System.Windows.Visibility.Visible;
                        rbutton3.Visibility = System.Windows.Visibility.Visible;
                        break;
                }
                MessageBox.Show(String.Format("You are logged in as {0}", user));
                label1.Content = String.Format("username: {0}", user);
            }
            catch(Exception ex)
            {
                MessageBox.Show("No such user!");
            }
            userbox.Text = "";
            passwordbox.Clear();
        }
    }
}
