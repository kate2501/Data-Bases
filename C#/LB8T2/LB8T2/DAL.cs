using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Configuration;


namespace LB8T2
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
        public DataTable giveStudsAsDataTable()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT st_id, st_name, st_surname, st_speciality, st_form, scholarship, course FROM studs", _connection);
            DataTable restable = new DataTable();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                restable.Load(dr);
            }

            return restable;
        }

        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            string command = "SELECT st_id, st_name, st_surname, st_speciality, st_form, scholarship, course FROM studs";
            this.OpenConnection();
            MySqlCommand cmd = new MySqlCommand(command, _connection);
            using (MySqlDataReader curRead =
                cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while(curRead.Read())
                {
                    students.Add(new Student
                    {
                        _id=(int)curRead["st_id"],
                        name=(string)curRead["st_name"],
                        surname=(string)curRead["st_surname"],
                        speciality=(string)curRead["st_speciality"],
                        edu_form=(string)curRead["st_form"],
                        salary=(int)curRead["scholarship"],
                        course=(int)curRead["course"]
                    });
                }
            }
            return students;
        }
        public void updateSt(Student stud)
        {
            try
            {
                string command = $"update studs set scholarship={stud.salary + 20} where st_id={stud._id} and scholarship!=0";
                this.OpenConnection();
                MySqlCommand updateCommand = new MySqlCommand(command, _connection);
                int res = updateCommand.ExecuteNonQuery();
                this.CloseConnection();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public DataTable joinQuery(int i)
        {
            this.OpenConnection();
            string c = $"select studs.st_name, studs.st_surname," +
                "subjects.sub_name, exams.exam_date, exams.exam_mark from exams " +
                "inner join subjects on subjects.sub_id=exams.ref_sub_id inner join studs on studs.st_id=" +
                "exams.ref_st_id where studs.st_id="+i;
            MySqlCommand cmd = new MySqlCommand(c, _connection);
            DataTable restable = new DataTable();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                restable.Load(dr);
            }
            this.CloseConnection();
            return restable;
        }
        public void predict(int stid, int stsub)
        {
            this.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("predict", _connection);
            cmd.CommandType= System.Data.CommandType.StoredProcedure;
            List<MySqlParameter> prm = new List<MySqlParameter>()
            {
            new MySqlParameter("stid", MySqlDbType.Int16) {Value = stid},
            new MySqlParameter("stsub", MySqlDbType.Int16) {Value = stsub}
            };
            cmd.Parameters.AddRange(prm.ToArray());
            using (MySqlDataReader dr = cmd.ExecuteReader()) {
                    dr.Read();
                string val=dr.GetValue(0).ToString();
                MessageBox.Show(val);
            }
        }
    }
}

