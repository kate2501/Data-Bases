using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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

namespace LB8T2
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
            List<Student> itemlist = da.GetAllStudents();
            this.studsGrid.ItemsSource = itemlist;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(studsGrid.SelectedItems !=null)
            {
                Student dv = (Student)studsGrid.SelectedItems[0];
                da.updateSt(dv);
                Window_Loaded(null, null);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (studsGrid.SelectedItems != null)
            {
                Student dv = (Student)studsGrid.SelectedItems[0];
                int i = dv._id;
                DataTable dtExams = da.joinQuery(i);
                examsGrid.ItemsSource = dtExams.DefaultView;
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Prediction pr = new Prediction();
            pr.Show();
        
        }

    
    }
}
