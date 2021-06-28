using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB11._2
{
    public class Material
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string property { get; set; }
        public float cost { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }
        public Material()
        {
            Items = new List<Item>();
            Suppliers = new List<Supplier>();
        }
        public bool checkcost()
        {
            return this.cost > 0;
        }
    }
}
