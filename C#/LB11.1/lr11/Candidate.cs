using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace lr11
{
    class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public float Rating { get; set; }
        public ICollection<Confident> Confidents { get; set; }
        public ICollection<Promise> Promises { get; set; }
        public Candidate()
        {
            Confidents = new List<Confident>();
            Promises = new List<Promise>();
        }

        

        public void RatingChange(float rate)
        {
            if (rate <= 100 && rate >= 0)
            {
                this.Rating = rate;
            }
            else
            {
                MessageBox.Show("Incorect rate value!");
            }
        }
    }
}
