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

namespace LB202
{
    /// <summary>
    /// Логика взаимодействия для Adding.xaml
    /// </summary>
    public partial class Adding : Window
    {

        private Button btnOk;
        public string cost { get { return t1.Text; }}
        public string type { get { return t2.Text; } }
        public int year { get { return Convert.ToInt32(t3.Text); } }
        public string desc { get { return t4.Text; } }

        public Adding()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Close();

        }
    }
}
