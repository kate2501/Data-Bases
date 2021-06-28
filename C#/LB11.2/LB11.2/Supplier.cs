using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB11._2
{
    public class Supplier
    {
        public int Id { get; set; }
        public string name{ get; set; }
        public string manager { get; set; }
        public string address { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public ICollection<Material> Materials { get; set; }
        public Supplier()
        {
            Materials = new List<Material>();           
        }

        public int check()
        {
            return DateTime.Compare(this.end_date, this.start_date);
        }
    }
}
