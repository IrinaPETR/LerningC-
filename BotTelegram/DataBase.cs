using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace BotTelegram
{
    internal class DataBase
    {
        public static void TestConnection()
        {
            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();
                if(con.State == ConnectionState.Open)
                {
                    Console.WriteLine("Подключение установлено!");
                }
                SeeAll(con);
            }
        }
        //static NpgsqlConnection baseConnections;
        private static NpgsqlConnection GetConnection()
        {
            NpgsqlConnection baseConnections = new NpgsqlConnection(@$"{ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString}");
            return baseConnections;
        }

        static public void SeeAll(NpgsqlConnection con)
        {
            NpgsqlDataReader dataReader = null;

            try
            {
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM componentsdatabase WHERE Key LIKE '101'", con);
                dataReader = command.ExecuteReader();
                
                while (dataReader.Read())
                {
                    Console.WriteLine(Convert.ToString(dataReader["Name"]) + Convert.ToString(dataReader["actionontheproduct"]));
                     
                }
                

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally 
            {
                if(dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }

        }
    }
}
