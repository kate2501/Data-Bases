using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;



namespace DB1
{
    class DAL
    {
        private MySqlConnection connection = null;

        public void OpenConnection(string conStr)
        {
            connection = new MySqlConnection(conStr);
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }
        public void givePr()
        {
            Console.WriteLine("\nAll programs\n");
            MySqlCommand cmd = new MySqlCommand("select pr_id, pr_date, pr_len, c_city, p_name from program " +
            "inner join city on city_id = c_id " +
            "left join project on project.p_id = program.p_id", connection);
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.HasRows)
                {
                    Console.WriteLine("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}\t{4,-20}", dr.GetName(0), dr.GetName(1), dr.GetName(2), dr.GetName(3), dr.GetName(4));

                    while (dr.Read())
                    {
                        Console.WriteLine("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}\t{4,-20}", dr.GetValue(0), dr.GetValue(1), dr.GetValue(2), dr.GetValue(3), dr.GetValue(4));
                    }
                }
            }
        }
        public void giveHost()
        {
            Console.WriteLine("\nAll hosts\n");
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM host", connection);
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.HasRows)
                {
                    Console.WriteLine("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}\t{4,-20}", dr.GetName(0), dr.GetName(1), dr.GetName(2), dr.GetName(3), dr.GetName(4));

                    while (dr.Read())
                    {
                        Console.WriteLine("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}\t{4,-20}", dr.GetValue(0), dr.GetValue(1), dr.GetValue(2), dr.GetValue(3), dr.GetValue(4));
                    }
                }
            }
        }

        public DataTable giveHosts(int id)
        {
            MySqlParameter p1 = new MySqlParameter("@id", id);
            MySqlCommand cmd = new MySqlCommand("select host.* from host_pr "+
            "inner join program on host_pr.pr_id = program.pr_id "+
            "inner join host on host_pr.h_id = host.h_id where program.pr_id = @id", connection);
            cmd.Parameters.AddRange(new MySqlParameter[] { p1});
            DataTable restable = new DataTable();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                restable.Load(dr);
            }
            return restable;
        }
        public void giveSp()
        {
            Console.WriteLine("\nAll sponsors\n");
            MySqlCommand cmd = new MySqlCommand(" select program.pr_id, pr_date, sp_comp, " +
                "sp_product from program inner join sp_program on program.pr_id = sp_program.pr_id " +
                "inner join sponsor on sponsor.sp_id = sp_program.sp_id", connection);
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.HasRows)
                {
                    Console.WriteLine("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}", dr.GetName(0), dr.GetName(1), dr.GetName(2), dr.GetName(3));

                    while (dr.Read())
                    {
                        Console.WriteLine("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}", dr.GetValue(0), dr.GetValue(1), dr.GetValue(2), dr.GetValue(3));
                    }
                }
            }
        }
        public void giveC()
        {
            Console.WriteLine("\nAll costs\n");
            MySqlCommand cmd = new MySqlCommand("select * from costs", connection);
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.HasRows)
                {
                    Console.WriteLine("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}\t{4,-20}", dr.GetName(0), dr.GetName(1), dr.GetName(2), dr.GetName(3), dr.GetName(4));

                    while (dr.Read())
                    {
                        Console.WriteLine("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}\t{4,-20}", dr.GetValue(0), dr.GetValue(1), dr.GetValue(2), dr.GetValue(3), dr.GetValue(4));
                    }
                }
            }
        }

        public void cost()
        {
            Console.WriteLine("Enter tr id:");
            int value = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("The full cost of trip {0}: ", value);
            MySqlCommand procCommand = new MySqlCommand("cost", connection);
            procCommand.CommandType = System.Data.CommandType.StoredProcedure;
            MySqlParameter p1 = new MySqlParameter("tr", value);
            procCommand.Parameters.Add(p1);
            using (MySqlDataReader dr = procCommand.ExecuteReader())
            {
                dr.Read();
                Console.WriteLine("{0} -> {1}", dr.GetName(0), dr.GetValue(0));
            }
        }
        public void autohost()
        {
            Console.WriteLine("Enter pr id:");
            int value = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("Choosing random hosts for pr. {0}", value);
            MySqlCommand procCommand = new MySqlCommand("autohost", connection);
            procCommand.CommandType = System.Data.CommandType.StoredProcedure;
            MySqlParameter p1 = new MySqlParameter("pr", value);
            procCommand.Parameters.Add(p1);
            MySqlCommand cmd = new MySqlCommand("select * from host_pr", connection);
            using (MySqlDataReader dr = procCommand.ExecuteReader()) { }
            using (MySqlDataReader dr1 = cmd.ExecuteReader())
            {
                if (dr1.HasRows)
                {
                    Console.WriteLine("{0,-20}\t{1,-20}", dr1.GetName(0), dr1.GetName(1));

                    while (dr1.Read())
                    {
                        Console.WriteLine("{0,-20}\t{1,-20}", dr1.GetValue(0), dr1.GetValue(1));
                    }
                }
            }
        }
        public void mostpop()
        {
            Console.WriteLine("The most popular destination");
            MySqlCommand procCommand = new MySqlCommand("mostpopular", connection);
            procCommand.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader dr = procCommand.ExecuteReader())
            {
                dr.Read();
                Console.WriteLine("{0}", dr.GetValue(0));
            }
        }
        public void sale(int c, double per)
        {
            //Console.WriteLine("Enter number of cost:");
            //int c = Convert.ToInt16(Console.ReadLine());
            //Console.WriteLine("Enter percentage:");
            //double per = Convert.ToDouble(Console.ReadLine());
            //Console.WriteLine("Sale for defined cost");
            MySqlCommand procCommand = new MySqlCommand("sale", connection);
            procCommand.CommandType = System.Data.CommandType.StoredProcedure;
            List<MySqlParameter> prm = new List<MySqlParameter>()
            {
            new MySqlParameter("c", MySqlDbType.Int16) {Value = c},
            new MySqlParameter("per", MySqlDbType.Double) {Value = per}
            };
            procCommand.Parameters.AddRange(prm.ToArray());
            using (MySqlDataReader dr = procCommand.ExecuteReader()) { }
            
        }

        public DataTable givePrAsDataTable()
        {
            MySqlCommand cmd = new MySqlCommand("select pr_id, pr_date, cast(pr_len as char) as pr_len, c_city, p_name from program " +
            "inner join city on city_id = c_id " +
            "left join project on project.p_id = program.p_id", connection);
            DataTable restable = new DataTable();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                restable.Load(dr);
            }
            return restable;
        }

        public DataTable giveHostAsDataTable()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM host", connection);
            DataTable restable = new DataTable();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                restable.Load(dr);
            }
            return restable;
        }
        public DataTable giveCAsDataTable()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM costs", connection);
            DataTable restable = new DataTable();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                restable.Load(dr);
            }
            return restable;
        }

        public DataTable giveSpAsDataTable()
        {
            MySqlCommand cmd = new MySqlCommand(" select program.pr_id, pr_date, sp_comp, " +
               "sp_product from program inner join sp_program on program.pr_id = sp_program.pr_id " +
               "inner join sponsor on sponsor.sp_id = sp_program.sp_id", connection);
            DataTable restable = new DataTable();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                restable.Load(dr);
            }
            return restable;
        }
    }
}
