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
using System.Windows.Shapes;

namespace LB8T3
{
    /// <summary>
    /// Логика взаимодействия для Adding.xaml
    /// </summary>
    public partial class Adding : Window
    {
        DAL da = new DAL();

        public Adding()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Collection> itemlist = da.getcolllist();
            this.collGrid.ItemsSource = itemlist;
            collGrid.Columns[0].Visibility = Visibility.Hidden;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            float cost = txt1.Text == "" ? 0 : float.Parse(txt1.Text);
            string type = txt2.Text;
            int year = txt3.Text=="" ? 2020 : int.Parse(txt3.Text);
            string desc = txt4.Text;
            int i;
            if(collGrid.SelectedCells.Count!=0)
            {
                Collection dv = (Collection)collGrid.SelectedItems[0];
                i = dv.id;
                
            }
            else
            {
                i = 0;
            }
            da.adding(cost, type, year, desc, i);
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
