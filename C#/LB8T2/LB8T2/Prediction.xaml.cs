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

namespace LB8T2
{
    /// <summary>
    /// Логика взаимодействия для Prediction.xaml
    /// </summary>
    public partial class Prediction : Window
    {
        DAL da = new DAL();
        public Prediction()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int st_id = int.Parse(txt1.Text);
            int sub_id = int.Parse(txt2.Text);
            da.predict(st_id, sub_id);
        }
    }
}
