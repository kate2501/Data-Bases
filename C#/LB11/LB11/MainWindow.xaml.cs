using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace LB11
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ElectionContext curCont;
        public MainWindow()
        {
            InitializeComponent();
            using (ElectionContext db = new ElectionContext())
            {
                Candidate c1 = new Candidate { Name = "Alex", Surname = "Lukashenko", Rating = 35 };
                Candidate c2 = new Candidate { Name = "Zianon", Surname = "Pozniak", Rating = 65 };
                Candidate c3 = new Candidate { Name = "Maria", Surname = "Vasilevish", Rating = 0 };
                db.Candidates.AddRange(new List<Candidate>() { c1, c2, c3 });
                Confident conf1 = new Confident { FullName = "Alex Matskevich", Age = 40, PoliticalPreferences = "Neutral", Candidate = c1 };
                Confident conf2 = new Confident { FullName = "Karyna Kluchnik", Age = 18, PoliticalPreferences = "Monarch", Candidate = c2 };
                Confident conf3 = new Confident { FullName = "Tanya Verbovich", Age = 25, PoliticalPreferences = "Liberal", Candidate = c2 };
                db.Confidents.AddRange(new List<Confident>() { conf1, conf2, conf3 });
                Promise prom1 = new Promise { Text = "Stop War" };
                Promise prom2 = new Promise { Text = "Increase revenue" };
                prom1.Candidates.Add(c1);
                prom2.Candidates.Add(c1);
                prom2.Candidates.Add(c2);
                db.Promises.AddRange(new List<Promise> { prom1, prom2 });
                db.SaveChanges();
                CandidateProfile prof1 = new CandidateProfile { Id = c1.Id, Age = 50, Description = "Current president" };
                CandidateProfile prof2 = new CandidateProfile { Id = c2.Id, Age = 40, Description = "Legend" };
                db.CandidateProfiles.AddRange(new List<CandidateProfile> { prof1, prof2 });
                db.SaveChanges();
            }

        }

        private void mainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            curCont = new ElectionContext();
            curCont.Candidates.Load();
            mainGrid.ItemsSource = curCont.Candidates.Local.ToBindingList();
            this.Closing += MainWindow_Closing;
            var result1 = curCont.Candidates.Where(x => x.Rating > 10);
            getlen(result1);

        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            curCont.Dispose();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            curCont.SaveChanges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainGrid.SelectedItems != null)
                {
                    for (int i = 0; i < mainGrid.SelectedItems.Count; i++)
                    {
                        Candidate curCand = mainGrid.SelectedItems[i] as Candidate;
                        if (curCand != null)
                        {
                            curCont.Candidates.Remove(curCand);
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //var curCand = curCont.Candidates.Include(p => p.Confidents).ToList();
            //string result = "";
            //foreach (var c in curCand)
            //{
            //    result += c.Name + " " + c.Surname + " \n" + "Confidents: ";
            //    foreach (var conf in c.Confidents)
            //    {
            //       result += conf.FullName + " ";
            //    }
            //    result += "\n";
            //}
            //MessageBox.Show(result);

            //var curCand = curCont.Candidates.ToList();
            //string result = "";
            //foreach (var c in curCand)
            //{
            //    result += c.Name + " " + c.Surname + " \n" + "Confidents: ";
            //    foreach (var conf in c.Confidents)
            //    {
            //        result += conf.FullName + " ";
            //    }
            //    result += "\n";
            //}
            //MessageBox.Show(result);

            var curCand = curCont.Candidates.FirstOrDefault();
            curCont.Entry(curCand).Collection("Confidents").Load();
            string result = "";
            result += curCand.Name + " " + curCand.Surname + " \n" + "Confidents: ";
            foreach (var conf in curCand.Confidents)
            {
                result += conf.FullName + " ";
            }
            MessageBox.Show(result);
        }

        private void Rating_Click(object sender, RoutedEventArgs e)
        {
            //Candidate firstCandidate = curCont.Candidates.FirstOrDefault();
            //System.Random rnd = new System.Random();
            //int myInt = rnd.Next(1, 10000);
            //float myFloat = myInt / 100;
            //firstCandidate.RatingChange(myFloat);
            //Candidate secondCandidate = curCont.Candidates.OrderBy(x => x.Id).Skip(1).FirstOrDefault();
            //secondCandidate.RatingChange(100 - myFloat);
            //curCont.SaveChanges();
            //mainGrid.Items.Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Candidate curCand = mainGrid.SelectedItems[0] as Candidate;
            curCand.Surname = surnameBox.Text;
            curCont.SaveChanges();
            mainGrid.Items.Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (ElectionContext db = new ElectionContext())
            {
                var result1 = db.Candidates.Where(x => x.Rating > 10).ToList();
                resultGrid.ItemsSource = result1;
                var result2 = db.Candidates.Join(db.Confidents,
                p => p.Id,
                c => c.CandidateId,
                (p, c) => new
                {
                    Name = c.FullName,
                    CandidateSurname = p.Surname
                }
                ).ToList();

                var result3 = db.Confidents
                .GroupBy(c => c.CandidateId)
                .Select(g =>
                new { ID = g.FirstOrDefault().Id, AvgAge = g.Average(c => c.Age) })
                .Join(db.Candidates,
                a => a.ID,
                b => b.Id,
                (a, b) => new
                {
                    Name = b.Name,
                    Surname = b.Surname,
                    Age = a.AvgAge
                }
                ).ToList();
                resultGrid.ItemsSource = result3;
            }
        }

        private  void getlen(IQueryable p)
        {
            MessageBox.Show(p.ToString());

        }

    }
}
