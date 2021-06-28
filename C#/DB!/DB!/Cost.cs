using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_
{
    class Cost
    {
        public int c_id;
        public int tr_id;
        public string c_name;
        public string c_type;
        public double c_price;

        public Cost(int id, int t_id, string name, string type, double price)
        {
            c_id = id;
            tr_id = t_id;
            c_name = name;
            c_type = type;
            c_price = price;
        }

        public void change(double per)
        {
            c_price = (1 - per / 100) * c_price;
        }
    }
}
