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
        static NpgsqlConnection con = null;
        public static void TestConnection()
        {
            con = GetConnection();
            con.Open();
            if(con.State == ConnectionState.Open)
            {
            Console.WriteLine("Подключение установлено!");
            }
            //SeeAll();
            
        }

        private static NpgsqlConnection GetConnection()
        {
            NpgsqlConnection baseConnections = new NpgsqlConnection(@$"{ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString}");
            return baseConnections;
        }

        static public List<СomponentsDataBase> SeeAll()
        {
            List<СomponentsDataBase> components = new List<СomponentsDataBase>();
            NpgsqlDataReader dataReader = null;

            try
            {
                //con.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM componentsdatabase WHERE Key LIKE '101'", con);
                dataReader = command.ExecuteReader();
                
                while (dataReader.Read())
                {
                    components.Add(ReadComponents(dataReader));
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
            return components;

        }




        static public List<СomponentsDataBase> FindComponentsInBase(string name)
        {
            List<СomponentsDataBase> components = new List<СomponentsDataBase>();
            NpgsqlDataReader dataReader = null;

            try
            {
                NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM componentsdatabase WHERE \"name\" LIKE \'{name}\'", con);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {                    
                    components.Add(ReadComponents(dataReader));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
            return components;

        }

        private static СomponentsDataBase ReadComponents(NpgsqlDataReader dataReader)
        {
            string st1 = Convert.ToString(dataReader["influenceonperson"]);
            string st2 = Convert.ToString(dataReader["lastname"]);
            var compNew = new СomponentsDataBase(
                Convert.ToString(dataReader["key"]),
                Convert.ToString(dataReader["name"]),
                Convert.ToString(dataReader["danger"]),
                Convert.ToString(dataReader["actionontheproduct"]),
                Convert.ToString(dataReader["influenceonperson"]),
                Convert.ToString(dataReader["lastname"]));
            return compNew;
        }
    }
}
