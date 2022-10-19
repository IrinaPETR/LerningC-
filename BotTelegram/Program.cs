using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static BotTelegram.SearchText;
using System.Configuration;
using Telegram.Bot.Types.Enums;

using Emgu;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.OCR;

namespace BotTelegram
{
    internal class Program
    {
        static List<СomponentsDataBase> components = new List<СomponentsDataBase>();
        static List<string> notFoundComponents = new List<string>();
        static void Main(string[] args)
        {
            //Лучше тоже как обработчик события "Старт программы"
            DataBase.TestConnection();
            var client = new TelegramBotClient(ConfigurationManager.ConnectionStrings["TelegramBot"].ConnectionString);
            client.StartReceiving(MyUpdate, Error, null);

            Console.ReadLine();
        }


        async static Task MyUpdate(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        var message = update.Message;
                        if (message?.Text != null)
                        {
                            string text = message.Text.ToLower();
                            Console.WriteLine($"{message.Chat.FirstName} -> {message.Text}");
                            if (text.StartsWith("привет"))
                            {
                                await botClient.SendTextMessageAsync(message.Chat.Id, $"Приветик! Начни своё сообщение со слова \"найти\" для поиска ингридиента");
                            }
                            if (text.StartsWith("найти"))
                            {
                                components.Clear();
                                notFoundComponents.Clear();

                                text = text.Replace("найти", "");
                                text = text.Replace("е-", "");
                                text = text.Replace("ё", "е");
                                text = text.Trim();
                                string[] words = text.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries);

                                
                                foreach (string whoComponent in words)
                                {
                                    var withoutSpaceComponent = whoComponent.Trim();
                                    var foundСomponentList = DataBase.FindComponentsInBase(withoutSpaceComponent);
                                    if (foundСomponentList.Count == 0) notFoundComponents.Add(whoComponent);
                                    components.AddRange(foundСomponentList);
                                }

                                foreach (СomponentsDataBase com in components)
                                {
                                    if(com.LastName != "") 
                                        await botClient.SendTextMessageAsync(message.Chat.Id, $"E-{com.Key} или {com.Name} или {com.LastName}{Environment.NewLine}☠️Опасность: {com.Danger}{Environment.NewLine}🍉Влияние на продукт: {com.ActionOnTheProduct}{Environment.NewLine}🧔‍♀️Действие на человека: {com.InfluenceOnPerson}");
                                    else await botClient.SendTextMessageAsync(message.Chat.Id, $"E-{com.Key} или {com.Name}{Environment.NewLine}☠️Опасность: {com.Danger}{Environment.NewLine}🍉Влияние на продукт: {com.ActionOnTheProduct}{Environment.NewLine}🧔‍♀️Действие на человека: {com.InfluenceOnPerson}");
                                }
                                if (notFoundComponents.Count != 0)
                                {
                                    string textAnswer = null;
                                    foreach (string notFound in notFoundComponents)
                                    {
                                        textAnswer = textAnswer + Environment.NewLine + "❌" + notFound;
                                    }
                                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Не найдены компоненты: {textAnswer}");

                                }    
                                
                            }

                        }
                            break;
                    }
                //case UpdateType.CallbackQuery:
                //    код,выполняемый если выражение имеет значение1
                //    break;
                
                //default:
                //    код, выполняемый если выражение не имеет ни одно из выше указанных значений
                //    break;
            }
        }




        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new Exception("Это ошибка");
        }


    }
}