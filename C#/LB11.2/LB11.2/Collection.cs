using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB11._2
{
    public class Collection
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public int Id { get; set; }
        public string name { get; set; }
        public string c_class { get; set; }
        public DateTime st_date { get; set; }
        public DateTime end_date { get; set; }
        public ICollection<Item> Items { get; set; }
        public Collection()
        {
            Items = new List<Item>();
        }
        public int check()
        {
            return DateTime.Compare(this.end_date, this.st_date);
        }
    }
}

