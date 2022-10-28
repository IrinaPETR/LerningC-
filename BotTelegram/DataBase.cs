using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static BotTelegram.SearchText;
using System.Configuration;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;

using Emgu;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.OCR;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.InputFiles;


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
                NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM componentsdatabase WHERE name Like \'{name}\' or key like \'{name}\' or lastname like \'{name}\'", con); 
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



        static public string FindInBase(Message message, string[] words, List<string> notFoundComponents, List<СomponentsDataBase>  components, Dictionary<string, List<СomponentsDataBase>>? userComponents)
        {
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].StartsWith("е"))
                {
                    words[i] = words[i].Replace("е", "");
                }

            }
            foreach (string whoComponent in words)
            {
                var withoutSpaceComponent = whoComponent.Trim();
                var foundСomponentList = DataBase.FindComponentsInBase(withoutSpaceComponent);
                if (foundСomponentList.Count == 0) notFoundComponents.Add(whoComponent);
                components.AddRange(foundСomponentList);
                List<СomponentsDataBase> value = new List<СomponentsDataBase>();
                bool flagDouble = false;
                if (!(userComponents.TryGetValue(message.Chat.FirstName, out value)) && foundСomponentList.Count != 0)
                {
                    userComponents.Add(message.Chat.FirstName, foundСomponentList);

                }
                else if (foundСomponentList.Count != 0)
                {
                    foreach (СomponentsDataBase userComponent in value)
                    {
                        if (foundСomponentList[0].Key == userComponent.Key)
                        {
                            flagDouble = true;
                        }

                    }
                    if (!flagDouble)
                    {
                        foundСomponentList.AddRange(value);
                        userComponents.Remove(message.Chat.FirstName); //удаляет по ключу элемент из словаря
                        userComponents.Add(message.Chat.FirstName, foundСomponentList);
                    }

                }

            }


            string textMessageWithNotFoundComponents = null;
            if (notFoundComponents.Count != 0)
            {
                foreach (string notFound in notFoundComponents)
                {
                    textMessageWithNotFoundComponents = textMessageWithNotFoundComponents + Environment.NewLine + "❌" + notFound;
                }
            }
            return textMessageWithNotFoundComponents;
        }


    }
}
