using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LB11._2
{
    public class Item
    {
        public int Id { get; set; }
        public float cost { get; set; }
        public string s_type { get; set; }
        public string s_desc { get; set; }
        public int? CollectionId { get; set; }
        public string op { get; set; }
        public Collection Collection { get; set; }
        public ICollection<Material> Materials { get; set; }
        public Item()
        {
            Materials = new List<Material>();
        }
        public bool checkcost()
        {
            return this.cost > 0;
        }
    }
}
