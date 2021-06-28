using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace LB11._2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ShopContext curCont;
        public MainWindow()
        {
            InitializeComponent();

            using (ShopContext db = new ShopContext())
            {
                //db.Suppliers.Clear();
                //db.Materials.Clear();
                //db.Items.Clear();
                //db.Collections.Clear();

                db.SaveChanges();
                Collection c1 = new Collection { name = "Barbara", c_class = "Premium", st_date = DateTime.Now, end_date = DateTime.Now.AddHours(500) };
                Collection c2 = new Collection { name = "Monica", c_class = "Usual", st_date = DateTime.Now, end_date = DateTime.Now.AddHours(600) };
                db.Collections.AddRange(new List<Collection>() { c1, c2 });
                Item it1 = new Item { cost = 500, s_type = "Coach", s_desc = "Very comfy", Collection = c1 };
                Item it2 = new Item { cost = 300, s_type = "Table", s_desc = "Very green", Collection = c1 };
                Item it3 = new Item { cost = 1000, s_type = "Bed", s_desc = "New and good", Collection = c2 };
                db.Items.AddRange(new List<Item>() { it1, it2, it3 });
                Material m1 = new Material { name = "wood", property = "good", cost = 100 };
                Material m2 = new Material { name = "leather", property = "black", cost = 200 };
                Material m3 = new Material { name = "leather", property = "white", cost = 210 };
                m1.Items.Add(it1);
                m2.Items.Add(it1);
                m3.Items.Add(it1);
                m1.Items.Add(it2);
                m1.Items.Add(it3);
                m2.Items.Add(it3);
                db.Materials.AddRange(new List<Material> { m1, m2, m3 });
                db.SaveChanges();
                Supplier s1 = new Supplier { name = "woodplant", manager = "Ivanov", address = "Pinsk", start_date = DateTime.Now, end_date = DateTime.Now.AddYears(3) };
                Supplier s2 = new Supplier { name = "leatherplant", manager = "Petrov", address = "Minsk", start_date = DateTime.Now, end_date = DateTime.Now.AddYears(2) };
                s1.Materials.Add(m1);
                s2.Materials.Add(m2);
                s2.Materials.Add(m3);
                db.Suppliers.AddRange(new List<Supplier> { s1, s2 });
                db.SaveChanges();
                
            }

            combo.Items.Add("delete linq");
            combo.Items.Add("update linq");
            combo1.Items.Add("Avg mat cost for item");
            combo1.Items.Add("Most popular supplier");
            combo1.Items.Add("Most popular collection");
            combo1.Items.Add("Quantity of mat usage");
            combo1.Items.Add("Show suppliers for item");
            combo1.Items.Add("Avg quant of mats for item");
            combo1.Items.Add("The longest contract");
            combo1.Items.Add("Avg cost of col item");
            
        }


        private void mainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            curCont = new ShopContext();
            curCont.Collections.Load();
            mainGrid.ItemsSource = curCont.Collections.Local.ToBindingList();
            //mainGrid.Columns[0].Visibility = Visibility.Hidden;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            curCont.Dispose();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainGrid.SelectedItems != null)
                {
                    for (int i = 0; i < mainGrid.SelectedItems.Count; i++)
                    {
                        Collection curCol = mainGrid.SelectedItems[i] as Collection;
                        if (curCol != null)
                        {
                            //var part = curCont.Items.Include(c => c.Collection).SingleOrDefault(p => p.CollectionId == curCol.Id);
                            //curCont.Items.Remove(part);
                            //curCont.Items.RemoveRange(curCont.Items.Where(x => x.Collection.Id == curCol.Id));
                            var range = curCont.Items.Include(c => c.Collection).Where(x => x.CollectionId == curCol.Id);
                            foreach (Item it in range)
                            {
                                //it.Collection = null;
                                MessageBox.Show(it.s_type);
                            }
                            curCont.Collections.Remove(curCol);
                        }


                       
                    }
                }
                curCont.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void OnKeyDownHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (curCont != null)
            {

                if (e.Command == DataGrid.DeleteCommand)
                {
                    try
                    {

                        if (mainGrid.SelectedItems != null)
                        {
                            for (int i = 0; i < mainGrid.SelectedItems.Count; i++)
                            {
                                Collection curCol = mainGrid.SelectedItems[i] as Collection;
                                if (curCol != null)
                                {
                                    curCont.Items.RemoveRange(curCont.Items.Where(x => x.Collection.Id == curCol.Id));
                                    curCont.Collections.Remove(curCol);
                                }
                            }
                        }
                        curCont.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {

                    curCont.SaveChanges();
                }   
            }
        }

        private void Items_Click(object sender, RoutedEventArgs e)
        {
            curCont.Items.Load();
            result.ItemsSource = curCont.Items.Local.ToBindingList();
        }

        private void Mat_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (result.SelectedItems != null)
                {
                    Item curCol = result.SelectedItems[0] as Item;
                    //    if (curCol != null)
                    //    {

                    //        var range = curCont.Items.Where(x => x.Id == curCol.Id).Include(p=>p.Materials).First();
                    //        result.ItemsSource =range.Materials.ToList() ;
                    //    }   
                    if (curCol != null)
                    {
                        var range = curCont.Items.Where(x => x.Id == curCol.Id).First();
                        curCont.Entry(range).Collection("Materials").Load();
                        result.ItemsSource = range.Materials.ToList();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string str = combo.SelectedItem.ToString();
            switch (str)
            {
                case "delete linq":
                        var query = curCont.Collections.Where(x => x.Id == 1).First();
                        curCont.Items.RemoveRange(curCont.Items.Where(x => x.Collection.Id == 1));
                        curCont.Collections.Remove(query);
                        curCont.SaveChanges();
                    break;
                case "update linq":
                    if (mainGrid.SelectedItems != null)
                    {
                        Collection cRow = mainGrid.SelectedItems[0] as Collection;
                        int m = cRow.Id;
                        Collection col = curCont.Collections.Where(x => x.Id == m).Single();
                        col.name = cRow.name;
                        col.c_class = cRow.c_class;
                        col.st_date = cRow.st_date;
                        col.end_date = cRow.end_date;
                        curCont.SaveChanges();
                    }
                    break;
                default:
                    MessageBox.Show("Something went wrong!");
                    break;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   
            string str = combo1.SelectedItem.ToString();
            switch(str)
            {
                case "Avg mat cost for item":
                    var res1 = curCont.Items.Select(g => new { item = g.s_type, cost = g.Materials.Average(c => c.cost) }).ToList();                        
                    result.ItemsSource = res1;
                    break;
                case "Most popular supplier":
                    var res2 = curCont.Suppliers.Select(g => new { name = g.name, count = g.Materials.Count() }).OrderByDescending(s=>s.count).First();
                    MessageBox.Show(res2.name);
                    break;
                case "Most popular collection":
                    var res3 = curCont.Collections.Select(g => new { name = g.name, count = g.Items.Count() }).OrderByDescending(s => s.count).First();
                    MessageBox.Show(res3.name);
                    break;
                case "Avg cost of col item":
                    var res4 = curCont.Items.GroupBy(c => c.CollectionId).Select(g =>
                        new { ID = g.Key, count=g.Average(c=>c.cost) })
                    .Join(curCont.Collections,
                    a => a.ID, b => b.Id,
                    (a, b) => new
                    {
                        name = b.name,
                        avg = a.count
                    }
                    ).ToList();
                    result.ItemsSource = res4;
                    break;
                case "Quantity of mat usage":
                    var res5 = curCont.Materials.Select(g => new { mat = g.name, count = g.Items.Count() }).ToList();
                    result.ItemsSource = res5;
                    break;
                case "Show suppliers for item":
                    if (result.SelectedItems != null)
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
                case "The longest contract":
                    var res8 = curCont.Suppliers.OrderByDescending(g=>DbFunctions.DiffDays(g.start_date,g.end_date)).First();
                    MessageBox.Show(res8.name);
                    break;
                default:
                    MessageBox.Show("Something went wrong!");
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

                var curC = curCont.Collections.Include(p => p.Items).ToList();
                string result = "";
                foreach(var c in curC)
                {
                    result += c.name + " Items: ";
                    foreach(var i in c.Items)
                    {
                        result += i.s_type + " ";
                    }
                    result += " \n";
                }
            MessageBox.Show(result);     
            
        }
    }
   
}
// протестировать modelfirst
