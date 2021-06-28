using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Configuration;

namespace LB8T3
{
    class DAL
    {
        private MySqlConnection _connection = null;
        private readonly string _connectionString;

        public DAL(string connString)
        {
            _connectionString = connString;
        }
        public DAL() : this(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString)
        {

        }

        public void OpenConnection()
        {
            try
            {
                _connection = new MySqlConnection(this._connectionString);
                _connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CloseConnection()
        {
            if (_connection?.State != ConnectionState.Closed)
            {
                _connection?.Close();
            }
        }

        public DataTable givestock()
        {
            this.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("select * from stock", _connection);
            DataTable restable = new DataTable();
            using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                restable.Load(dr);
            }
            return restable;
        }

        public List<Stock> getstocklist()
        {
            List<Stock> st = new List<Stock>();
            this.OpenConnection();
            MySqlCommand cmd= new MySqlCommand("select * from stock", _connection);
            using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while(dr.Read())
                {
                    st.Add(new Stock
                    {
                        id = (int)dr["id"],
                        cost= Convert.IsDBNull(dr["cost"]) ? 0 : (float)dr["cost"],
                        s_type=(string)dr["s_type"],
                        s_year=Convert.ToInt16(dr["s_year"]),
                        s_desc=(string)dr["s_desc"],
                        ref_coll= Convert.IsDBNull(dr["ref_coll"]) ? 0 : (int)dr["ref_coll"],

                    });
                }
            }
            return st;
        }

        public List<Collection> getcolllist()
        {
            List<Collection> col = new List<Collection>();
            this.OpenConnection();
            
            MySqlCommand cmd = new MySqlCommand("select * from collection", _connection);
            using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while(dr.Read())
                {
                    col.Add(new Collection
                    {
                        id = (int)dr["id"],
                        name = (string)dr["name"],
                        s_class = (string)dr["s_class"],
                        st_date = (DateTime)dr["st_date"],
                        end_date = Convert.IsDBNull(dr["end_date"]) ? DateTime.Today : (DateTime)dr["end_date"]
                    });
                }
            }
            return col;
        }

        public List<Material> getmatlist()
        {
            List<Material> col = new List<Material>();
            this.OpenConnection();

            MySqlCommand cmd = new MySqlCommand("select * from material", _connection);
            using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr.Read())
                {
                    col.Add(new Material
                    {
                        id = (int)dr["id"],
                        name = (string)dr["name"],
                        property = (string)dr["property"],
                        ucost = (float)dr["u_sost"]
                    });
                }
            }
            return col;
        }

        public List<Supplier> getsup(int id)
        {
            List<Supplier> col = new List<Supplier>();
            this.OpenConnection();
            string str = "select supplier.* from material inner join sup_mat on " +
            "material.id = sup_mat.mat_id inner join supplier on " +
            "supplier.id = sup_mat.sup_id where material.id =" + id;
            MySqlCommand cmd = new MySqlCommand(str, _connection);
            using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr.Read())
                {
                    col.Add(new Supplier
                    {
                        id = (int)dr["id"],
                        name = (string)dr["name"],
                        manager=(string)dr["manager"],
                        startdate = (DateTime)dr["start_date"],
                        enddate = (DateTime)dr["end_date"]
                    });
                }
            }
            return col;
        }
        public List<Sup_mat> show_sup_mat()
        {
            List<Sup_mat> col = new List<Sup_mat>();
            this.OpenConnection();
            string str = "select * from sup_mat";
            MySqlCommand cmd = new MySqlCommand(str, _connection);
            using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr.Read())
                {
                    col.Add(new Sup_mat
                    {
                        supid = (int)dr["sup_id"],
                        matid = (int)dr["mat_id"],
                        deliver=(DateTime)dr["deliver"],
                        cost=(float)dr["cost"],
                        quantity= Convert.IsDBNull(dr["quantity"]) ? 0 : (int)dr["quantity"]
                    });
                }
            }
            return col;
        }
        public void predict(Stock dv)
        {
            this.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("predict", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            MySqlParameter p = new MySqlParameter("id", dv.id);
            cmd.Parameters.Add(p);
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                dr.Read();
                string val = dr.GetValue(0).ToString();
                MessageBox.Show(val);
            }
        }

        public DataTable showmat(Stock dv)
        {
            this.OpenConnection();
            string str= "select material.* from stock inner join st_mat on "+
            "stock.id = st_mat.st_id inner join material on "+
            "material.id = st_mat.mat_id where stock.id ="+dv.id;
            MySqlCommand cmd = new MySqlCommand(str, _connection);
            DataTable restable = new DataTable();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                restable.Load(dr);
            }
            return restable;
        }

        public DataTable showsup(int id)
        {
            this.OpenConnection();
            string str = "select supplier.* from material inner join sup_mat on " +
            "material.id = sup_mat.mat_id inner join supplier on " +
            "supplier.id = sup_mat.sup_id where material.id =" + id;
            MySqlCommand cmd = new MySqlCommand(str, _connection);
            DataTable restable = new DataTable();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                restable.Load(dr);
            }
            return restable;
        }
        public DataTable sup_mat()
        {
            this.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("select * from sup_mat", _connection);
            DataTable restable = new DataTable();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                restable.Load(dr);
            }
            return restable;
        }

        public void deleterow(Stock st)
        {
            this.OpenConnection();
            string str = $"delete from stock where id={st.id}";
            MySqlCommand cmd = new MySqlCommand(str, _connection);
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                MessageBox.Show("The selected row was deleted");
            }
        }

        public void edition(Stock st, string ind, string ntext)
        {
            //this.OpenConnection();
            //string str = $"update stock set {ind}=\"{ntext}\" where id={st.id}";
            //MySqlCommand cmd = new MySqlCommand(str, _connection);
            //MySqlTransaction tr = _connection.BeginTransaction();
            //cmd.Transaction = tr;
            //try
            //{
            //    using (MySqlDataReader dr = cmd.ExecuteReader())
            //    {
            //        MessageBox.Show("The selected row was updated");
            //        tr.Commit();
            //    }
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    tr.Rollback();
            //}
            this.OpenConnection();
            MySqlTransaction transaction = _connection.BeginTransaction();
            MySqlCommand cmd = _connection.CreateCommand();
            cmd.Transaction = transaction;
            try
            {
                cmd.CommandText = $"update stock set {ind}=\"{ntext}\" where id={st.id}";
                int x = cmd.ExecuteNonQuery();
                transaction.Commit();
                MessageBox.Show("The selected row was updated");
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            this.CloseConnection();

        }
        public void adding(float cost, string type, int year, string desc, int i)
        {
            this.OpenConnection();
            string str;
            if (i != 0)
            {
                str = $"insert into stock (cost, s_type, s_year, s_desc, ref_coll) values ({cost},\"{type}\",{year},\"{desc}\", {i})";
            }
            else
            {
                str = $"insert into stock (cost, s_type, s_year, s_desc) values ({cost},\"{type}\",{year},\"{desc}\")";
            }
            MySqlCommand cmd = new MySqlCommand(str, _connection);
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                MessageBox.Show("New row was added!");
            }
        }
    }
}
