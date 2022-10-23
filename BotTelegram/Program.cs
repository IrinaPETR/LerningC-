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

namespace BotTelegram
{
    internal class Program
    {
        static List<СomponentsDataBase> components = new List<СomponentsDataBase>();
        static Dictionary<string, List<СomponentsDataBase>> ?userComponents = new Dictionary<string, List<СomponentsDataBase>>();
        static List<string> notFoundComponents = new List<string>();
        static void Main(string[] args)
        {
            //Лучше тоже как обработчик события "Старт программы"
            DataBase.TestConnection();
            var client = new TelegramBotClient(ConfigurationManager.ConnectionStrings["TelegramBot"].ConnectionString);
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = {}
            };
            client.StartReceiving(MyUpdate, Error, receiverOptions);

            Console.ReadLine();
        }


        static async Task MyUpdate(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        var message = update.Message;


                        
                        if (message.Photo != null)
                        {
                            //СДЕЛАТЬ ЭТО КАК СОБЫТИЕ!!!!!
                            var fileId = update.Message.Photo.Last().FileId;
                            var fileInfo = await botClient.GetFileAsync(fileId);
                            var filePath = fileInfo.FilePath;

                            string destinationFilePath = $@"C:\Emgu\Photo\user\{message.From.FirstName}.png";
                            await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
                            await botClient.DownloadFileAsync(filePath, fileStream);
                            fileStream.Close();

                            Tesseract tess = new Tesseract(@"C:\Emgu\langu", "rus", OcrEngineMode.TesseractLstmCombined);
                            tess.SetImage(new Image<Bgr, byte>(destinationFilePath));
                            tess.Recognize();
                            string text = tess.GetUTF8Text();

                            tess.Dispose();
                            //await botClient.SendTextMessageAsync(message.Chat.Id, $"{text}");
                            text = text.ToLower();
                            SearchByPhoto(message, text);

                            return;
                        }



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
                                SearchByText(message, text);
                            }

                        }
                            break;
                    }
                case UpdateType.CallbackQuery:
                    {
                        CallbackQuery callbackQuery = update.CallbackQuery;
                        //if()
                        var callbackComponent = DataBase.FindComponentsInBase(callbackQuery.Data);
                        foreach (СomponentsDataBase com in callbackComponent)
                        {
                            if (callbackQuery.Data.StartsWith(com.Name))
                            {
                                if (com.LastName != "")
                                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"E-{com.Key} или {com.Name} или {com.LastName}{Environment.NewLine}☠️Опасность: {com.Danger}{Environment.NewLine}🍉Влияние на продукт: {com.ActionOnTheProduct}{Environment.NewLine}🧔‍♀️Действие на человека: {com.InfluenceOnPerson}");
                                else await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"E-{com.Key} или {com.Name}{Environment.NewLine}☠️Опасность: {com.Danger}{Environment.NewLine}🍉Влияние на продукт: {com.ActionOnTheProduct}{Environment.NewLine}🧔‍♀️Действие на человека: {com.InfluenceOnPerson}");
                            }
                        }

                    }
                    break; 

                    //default:
                    //    break;
            }

            string TextPreparation(string text)
            {
                text = text.Replace("найти", "");
                text = text.Replace("е-", "");
                text = text.Replace("е", "");
                text = text.Replace("ё", "е");
                text = text.Replace(")", "");
                text = text.Replace("(", "");
                text = text.Trim();
                return text;
            }

            string FindInBase(Message message, string[] words)
            {
                foreach (string whoComponent in words)
                {
                    var withoutSpaceComponent = whoComponent.Trim();
                    var foundСomponentList = DataBase.FindComponentsInBase(withoutSpaceComponent);
                    if (foundСomponentList.Count == 0) notFoundComponents.Add(whoComponent);
                    components.AddRange(foundСomponentList);
                    List<СomponentsDataBase> value = new List<СomponentsDataBase>();
                    if (userComponents.TryGetValue(message.From.FirstName, out value))
                    {
                        foundСomponentList.AddRange(value);
                        userComponents.Remove(message.From.FirstName); //удаляет по ключу элемент из словаря
                    }
                    userComponents.Add(message.From.FirstName, foundСomponentList);
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

            InlineKeyboardButton[][] СreatingButtons()
            {

                InlineKeyboardButton[][] arrayButton = new InlineKeyboardButton[components.Count + 1][];
                List<InlineKeyboardButton[]> massivButton = new List<InlineKeyboardButton[]>();

                for (int i = 0; i < components.Count; i++)
                {
                    if (components[i].LastName != "")
                        massivButton.Add(new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(text: $"E-{components[i].Key} или {components[i].Name} или {components[i].LastName}", callbackData: $"{components[i].Name}") });
                    else massivButton.Add(new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(text: $"E-{components[i].Key} или {components[i].Name}", callbackData: $"{components[i].Name}") });
                    arrayButton[i] = massivButton[i];
                }

                massivButton.Add(new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(text: $"История моих поисков", callbackData: $"история") });
                arrayButton[components.Count] = massivButton[components.Count];
                return arrayButton;
            }

            async void SearchByPhoto(Message message, string text)
            {
                components.Clear();
                notFoundComponents.Clear();
                text = TextPreparation(text);
                string[] words = text.Split(new char[] { ',', ' ', '.', '-', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                FindInBase(message, words);
                InlineKeyboardButton[][] arrayButton = СreatingButtons();
                InlineKeyboardMarkup inlineKeyboard = new(arrayButton);
                await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: $"Найдены компоненты состава.{Environment.NewLine}Для просмотра подробной информации по компоненту нажми на его название ниже.",
                                replyMarkup: inlineKeyboard,
                                cancellationToken: token);
            }
            async void SearchByText(Message message, string text)
            {
                components.Clear();
                notFoundComponents.Clear();
                text = TextPreparation(text);
                string[] words = words = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string textMessageWithNotFoundComponents = FindInBase(message, words);

                InlineKeyboardButton[][] arrayButton = СreatingButtons();
                InlineKeyboardMarkup inlineKeyboard = new(arrayButton);
                await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: $"Не найдены компоненты: {textMessageWithNotFoundComponents}" + $"{Environment.NewLine}Для просмотра подробной информации по компоненту нажми на его название ниже.",
                                replyMarkup: inlineKeyboard,
                                cancellationToken: token);
            }
        }


        static private Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new Exception("Это ошибка");
        }


    }
}