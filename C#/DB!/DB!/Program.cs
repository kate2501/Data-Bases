using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;


namespace DB_
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("My DAL app");
                DAL myAccesLayer = new DAL();
                myAccesLayer.OpenConnection("server=localhost; user id=root;password=September3; database=OryoliReshka");
                myAccesLayer.givePr();
                Console.WriteLine("--------------------------------------------------");
                myAccesLayer.giveHost();
                Console.WriteLine("--------------------------------------------------");
                myAccesLayer.giveSp();
                Console.WriteLine("--------------------------------------------------");
                myAccesLayer.cost();
                Console.WriteLine("--------------------------------------------------");
                myAccesLayer.autohost();
                Console.WriteLine("--------------------------------------------------");
                myAccesLayer.mostpop();
                Console.WriteLine("--------------------------------------------------");
                myAccesLayer.giveC();
                Console.WriteLine("--------------------------------------------------");
                //myAccesLayer.sale();
                //DisplayTable(myAccesLayer.givePrAsDataTable());
                List<Programs> subList = myAccesLayer.getAllPrAsList();
                //Console.WriteLine();
                //Console.WriteLine(subList[1].c_city);
                List<Cost> subList1 = myAccesLayer.getAllCAsList();
                //Console.WriteLine();
                Console.WriteLine(subList1[0].c_price);
                subList1[0].change(50);
                Console.WriteLine(subList1[0].c_price);
                myAccesLayer.CloseConnection();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
        private static void DisplayTable(DataTable dt)
        {

            for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
            {
                Console.Write("{0,-25}", dt.Columns[curCol].ColumnName);
            }
            Console.WriteLine("\n  ");

            for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
            {
                for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
                {
                    Console.Write("{0,-25}", dt.Rows[curRow][curCol].ToString());
                }
                Console.WriteLine();
            }
        }
    }
}
