using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_
{
    class Programs
    {
        public int pr_id;
        public string pr_date;
        public string pr_len;
        public string c_city;

        public Programs(int id, string date, string len, string city)
        {
            pr_id = id;
            pr_date = date;
            pr_len = len;
            c_city = city;
        }
    }
}
