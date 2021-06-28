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

namespace LB903
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {

        private DataSet world = new DataSet("world");
        private string conStr = string.Empty;
        private MySqlDataAdapter cityAdapter;
        private MySqlDataAdapter countryAdapter;
        private MySqlDataAdapter langAdapter;

        private MySqlCommandBuilder cityCommands;
        private MySqlCommandBuilder countryCommands;
        private MySqlCommandBuilder langCommands;

        public MainWindow()
        {
            InitializeComponent();
            conStr = ConfigurationManager.ConnectionStrings["worldConnROOT"].ConnectionString;
            cityAdapter = new MySqlDataAdapter("select * from city", conStr);
            countryAdapter = new MySqlDataAdapter("select * from country", conStr);
            langAdapter = new MySqlDataAdapter("select * from countrylanguage", conStr);


            cityCommands = new MySqlCommandBuilder(cityAdapter);
            countryCommands = new MySqlCommandBuilder(countryAdapter);
            langCommands = new MySqlCommandBuilder(langAdapter);


            cityAdapter.Fill(world, "city");
            countryAdapter.Fill(world, "country");
            langAdapter.Fill(world, "countrylanguage");

            DataRelation countryToCity = new DataRelation("countryToCity", world.Tables["country"].Columns["Code"],
               world.Tables["city"].Columns["CountryCode"]);
            DataRelation countryToLang = new DataRelation("countryToCity", world.Tables["country"].Columns["Code"],
               world.Tables["countrylanguage"].Columns["CountryCode"]);


            world.Relations.Add(countryToCity);
           // world.Relations.Add(countryToLang);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           grid.ItemsSource = world.Tables["country"].DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var query = (from m in world.Tables["country"].AsEnumerable()
                         join t in world.Tables["city"].AsEnumerable()
                         on m["capital"] equals t["id"]
                         select new
                         {
                             name = m["name"],
                             capital = t["name"]
                         }).ToList();
            grid.ItemsSource = query;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int query1 = Convert.ToInt32((from m in world.Tables["countrylanguage"].AsEnumerable()
                          select m["CountryCode"]).Distinct().Count());
            float q = world.Tables["countrylanguage"].AsEnumerable().Where(row => row.Field<string>("Language") == "English").Sum(row => row.Field<float>("Percentage"));
            MessageBox.Show((q/ query1).ToString());
          
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            float query = world.Tables["country"].AsEnumerable().Where(row => row.Field<string>("Continent") == "Africa").Select(x => x.Field<float>("SurfaceArea")).Max();
            var query1 = (from m in world.Tables["country"].AsEnumerable()
                          where (float)m["SurfaceArea"] > query
                          select new
                          {
                              name = m["name"],
                              surface = m["SurfaceArea"]
                          }).ToList();
            grid.ItemsSource = query1;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var res = from m in world.Tables["city"].AsEnumerable()
                      where m["Name"].ToString().Length - m["Name"].ToString().Replace(" ", "").Length == 1 &&
                       m["Name"].ToString().Substring(0, m["Name"].ToString().IndexOf(' ')) != m["Name"].ToString().Substring(m["Name"].ToString().IndexOf(' ') + 1)
                      select new
                      {
                          Name = m["Name"],
                          CountryCode = m["CountryCode"]
                      };
            
            float gnp = world.Tables["country"].AsEnumerable().Select(x => x.Field<float>("GNP")).Average();
            var query = (from m in world.Tables["country"].AsEnumerable()
                         join t in res.AsEnumerable()
                         on m["Code"] equals t.CountryCode
                         where (float)m["GNP"] > gnp
                         select new
                         {
                             countryName = m["name"],
                             cityName = t.Name
                         }).ToList();
            grid.ItemsSource = query;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var tb1 = from m in world.Tables["city"].AsEnumerable()
                      group m by m.Field<string>("CountryCode") into mt
                      select new
                      {
                          mm = mt.Max(x => x.Field<int>("Population")),
                          cc = mt.Key
                      };


            var res2 = from m in world.Tables["city"].AsEnumerable()
                        join t in tb1.AsEnumerable()
                        on m["CountryCode"] equals t.cc
                        where Convert.ToInt32(m["Population"]) != Convert.ToInt32(t.mm)
                        group new {m,t} by new { k=m["CountryCode"],k1=m["Population"] } into mt
                        select new
                        {
                            k = mt.Average(x=>x.t.mm) + mt.Key.k1.,
                            cd = mt.Key.k
                        };

            


            var res = from m in world.Tables["countrylanguage"].AsEnumerable()
                      where m["IsOfficial"].ToString() == "T"
                      group m by m["CountryCode"] into mt
                      select new
                      {
                          c = mt.Key,
                          s = mt.Sum(x => x.Field<float>("Percentage"))
                      };
            var res1 = from m in world.Tables["country"].AsEnumerable()
                       from t in res.AsEnumerable()
                       where m["Code"].ToString()==t.c.ToString()
                       select new
                       {
                           cd = m["Code"],
                           nm = m["name"],
                           newpop = (int)m["Population"]* (float)t.s/100
                       };
            var query = (from m in res1.AsEnumerable()
                         join t in res2.AsEnumerable()
                         on m.cd.ToString() equals t.cd.ToString()
                         where (int)m.newpop > (int)t.k
                         select new
                         {
                             name = m.nm,
                             pop = m.newpop,
                             pop1 = t.k
                         }).ToList();
            grid.ItemsSource = res2.ToList();
        }
    }
}
