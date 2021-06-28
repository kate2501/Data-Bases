using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
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

namespace laba12
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DlContext curCont;


        public MainWindow()
        {
            InitializeComponent();
            combo1.Items.Add("Avg mat cost for item");
            combo1.Items.Add("Most popular supplier");
            combo1.Items.Add("Most popular collection");
            combo1.Items.Add("Quantity of mat usage");
            //combo1.Items.Add("Show suppliers for item");
            combo1.Items.Add("Avg quant of mats for item");
            //combo1.Items.Add("The longest contract");
            combo1.Items.Add("Avg cost of col item");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            curCont = new DlContext();
            curCont.Items.Load();
            mainGrid.ItemsSource = curCont.Items.Local.ToBindingList();
            curCont.Collections.Load();
            collGrid.ItemsSource = curCont.Collections.Local.ToBindingList();
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            curCont.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (mainGrid.SelectedItems != null)
            {
                using (DlContext db = new DlContext())
                {
                    Item curCol = mainGrid.SelectedItems[0] as Item;
                    int id = curCol.id;
                    //System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@id", id);
                    //System.Data.SqlClient.SqlParameter param1 = new System.Data.SqlClient.SqlParameter("@res", res);
                    var result = db.Database.SqlQuery<float>("CALL predict({0})", id).FirstOrDefault();
                    MessageBox.Show(result.ToString());
                }
            }
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = combo1.SelectedItem.ToString();
            switch (str)
            {

                case "Avg mat cost for item":

                    var res1 = curCont.Items.Select(g => new { item = g.s_type, cost = g.Materials.Average(c => c.u_cost) }).ToList();
                    collGrid.ItemsSource = res1;
                    break;
                case "Most popular supplier":
                    var res2 = curCont.Suppliers.SelectMany(m => m.Materials, (sup, mat) => new { sup, mat }).GroupBy(p => p.sup).Select(g => new
                    {
                        name = g.Key,
                        count = g.Count()
                    }).OrderByDescending(s => s.count);

                    collGrid.ItemsSource = res2;
                    break;
                case "Most popular collection":
                    var res3 = curCont.Collections.Select(g => new { name = g.name, count = g.Items.Count() }).OrderByDescending(s => s.count).First();
                    MessageBox.Show(res3.name);
                    break;
                case "Avg cost of col item":
                    var res4 = curCont.Items.GroupBy(c => c.CollectionId).Select(g =>
                        new { ID = g.Key, count = g.Average(c => c.cost) })
                    .Join(curCont.Collections,
                    a => a.ID, b => b.id,
                    (a, b) => new
                    {
                        name = b.name,
                        avg = a.count
                    }
                    ).ToList();
                    collGrid.ItemsSource = res4;
                    break;
                case "Quantity of mat usage":
                    var res5 = curCont.Materials.Select(g => new { mat = g.name, count = g.Items.Count() }).ToList();
                    collGrid.ItemsSource = res5;
                    break;
                case "Show suppliers for item":
                    if (collGrid.SelectedItems != null)
                    {
                        //Item it = result.SelectedItems[0] as Item;
                        //int id = it.Id;
                        //var res6 = curCont.Items.Where(x => x.Id == id).Include(p => p.Materials).First().Materials.Select(g=>new { sups=g.Select(x)});
                        //result.ItemsSource = res6.ToList();
                    }
                    break;
                case "Avg quant of mats for item":
                    var res7 = curCont.Items.Select(g => new { count = g.Materials.Count() }).Average(c => c.count).ToString();
                    MessageBox.Show(res7);
                    break;
                //case "The longest contract":
                //    var res8 = curCont.Suppliers.OrderByDescending(g => DbFunctions.DiffDays(g.start_date, g.end_date)).First();
                //    MessageBox.Show(res8.name);
                //    break;
                default:
                    MessageBox.Show("Something went wrong!");
                    break;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (DlContext db = new DlContext())
            {
                foreach (var entry in curCont.ChangeTracker.Entries())
                {
                    if (entry.State == EntityState.Modified)
                    {
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        mainGrid.Items.Refresh();
                    }
                    else if (entry.State == EntityState.Deleted)
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    else if (entry.State == EntityState.Added)
                    {
                        entry.State = EntityState.Detached;
                    }
                }
            }
            mainGrid.Items.Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (DlContext context = new DlContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Item i1 = context.Items.Find(1);
                        Item i2 = context.Items.Find(2);
                        int delta = 200;
                        i1.cost = i1.cost + delta;
                        if (context.ChangeTracker.HasChanges() == true)
                        {
                            i2.cost = i2.cost - delta;
                            if (i2.cost <= 0)
                            {
                                transaction.Rollback();
                            }
                            else
                            {
                                context.SaveChanges();
                                transaction.Commit();
                                this.Window_Loaded(null, null);
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var entry = curCont.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).Last();
            MessageBox.Show(entry.State.ToString());
            if (entry.State == EntityState.Modified)
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
                mainGrid.Items.Refresh();
            }
            else if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Unchanged;
            }
            else if (entry.State == EntityState.Added)
            {
                entry.State = EntityState.Detached;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            using (DlContext context = new DlContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Item i1 = context.Items.Find(5);
                        Item i2 = context.Items.Find(6);
                        int delta = 200;
                        i1.cost = i1.cost + delta;
                        if (context.ChangeTracker.HasChanges() == true)
                        {
                            i2.cost = i2.cost - delta;
                            if (i2.cost <= 0)
                            {
                                transaction.Rollback();
                            }
                            else
                            {
                                context.SaveChanges();
                                transaction.Commit();
                                this.Window_Loaded(null, null);
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (curCont != null)
            {
                curCont.SaveChanges();
                
                mainGrid.Items.Refresh();
            }

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

            collGrid.ItemsSource = curCont.AuditLogs.Local.ToBindingList();
        }

    }
    
}
