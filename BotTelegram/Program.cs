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
    internal class Program
    {
        static List<СomponentsDataBase> components = new List<СomponentsDataBase>();
        static Dictionary<string, List<СomponentsDataBase>> ?userComponents = new Dictionary<string, List<СomponentsDataBase>>();
        static Dictionary<string, InlineKeyboardMarkup>? userButtons = new Dictionary<string, InlineKeyboardMarkup>();
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
                            text = text.ToLower();

                            CreateAnswer.SearchByPhoto(message, text, notFoundComponents, components, userComponents, userButtons, token, botClient);

                            return;
                        }



                        if (message?.Text != null)
                        {
                            string text = message.Text.ToLower();
                            Console.WriteLine($"{message.Chat.FirstName} -> {message.Text}");

                            if (text.StartsWith("алгоритм"))
                            {

                                for(int i=1; i<5; i++)
                                {
                                    string mypath = $@"D:\Бывший рабочий\1 юный программист\Академия Цифра\AcademyCifra\FinalProject\АЛГОРИТМ\Часть{i}.png";
                                    using (var fileStream = new FileStream(mypath, FileMode.Open, FileAccess.Read, FileShare.Read))
                                    {
                                        await botClient.SendPhotoAsync(
                                            chatId: message.Chat.Id,
                                            photo: new InputOnlineFile(fileStream)
                                        );
                                    }

                                }
                                
                            }

                            else if (text.StartsWith("хочу фото") || text.StartsWith("фото") || text.StartsWith("фотография"))
                            {

                                string mypath = @"D:\Бывший рабочий\1 юный программист\Академия Цифра\AcademyCifra\FinalProject\Photo.jfif";
                                using (var fileStream = new FileStream(mypath, FileMode.Open, FileAccess.Read, FileShare.Read))
                                {
                                    await botClient.SendPhotoAsync(
                                        chatId: message.Chat.Id,
                                        photo: new InputOnlineFile(fileStream),
                                        caption: "Это тебе для проверки 🥰"
                                    );
                                }

                                mypath = @"D:\Бывший рабочий\1 юный программист\Академия Цифра\AcademyCifra\FinalProject\photo2.png";
                                using (var fileStream = new FileStream(mypath, FileMode.Open, FileAccess.Read, FileShare.Read))
                                {
                                    await botClient.SendPhotoAsync(
                                        chatId: message.Chat.Id,
                                        photo: new InputOnlineFile(fileStream),
                                        caption: "Вторая на выбор)"
                                    );
                                }

                            }
                            else if (text.StartsWith("привет") || text.StartsWith("/start"))
                            {
                                await botClient.SendTextMessageAsync(message.Chat.Id, $"Приветик, {message.From.FirstName}! Я помогу тебе прочитать этикетку продукта 🥰 Отправь мне состав, а я выведу описание пищевых добавок.{Environment.NewLine}{Environment.NewLine}Начни своё сообщение со слова 💥\"НАЙТИ\"💥 и перечисли ингридиенты состава через запятую. {Environment.NewLine}⚡Например: \"Найти азорубин, E-124\" {Environment.NewLine}{Environment.NewLine}А ещё можешь отправить 📸фотографию, я скажу, что там увидел 😉");
                            }
                            else if (text.StartsWith("найти"))
                            {
                                CreateAnswer.SearchByText(message, text, notFoundComponents, components, userComponents, userButtons, token, botClient);
                            }
                            else await botClient.SendTextMessageAsync(message.Chat.Id, $"🧐ххххммммм....... Я такого не знаю) Давай попробуем ещё раз! {Environment.NewLine}{Environment.NewLine}Начни своё сообщение со слова 💥\"НАЙТИ\"💥 и перечисли ингридиенты состава через запятую. {Environment.NewLine}✅Например: \"Найти азорубин, E-124\"{Environment.NewLine}{Environment.NewLine}А ещё можешь отправить 📸фотографию, я скажу, что там увидел 😉"); 

                        }
                            break;
                    }
                case UpdateType.CallbackQuery:
                    {
                        CallbackQuery callbackQuery = update.CallbackQuery;
                        if (callbackQuery.Data == "история" && userComponents.ContainsKey(callbackQuery.Message.Chat.FirstName))
                        {

                            List<СomponentsDataBase> value = new List<СomponentsDataBase>();
                            userComponents.TryGetValue(callbackQuery.Message.Chat.FirstName, out value);

                            InlineKeyboardButton[][] arrayButton = CreateAnswer.СreatingButtons(value);
                            InlineKeyboardMarkup inlineKeyboard = new(arrayButton);

                            userButtons.Remove(callbackQuery.Message.Chat.FirstName); //удаляет по ключу элемент из словаря
                            userButtons.Add(callbackQuery.Message.Chat.FirstName, inlineKeyboard);

                            await botClient.SendTextMessageAsync(
                                            chatId: callbackQuery.Message.Chat.Id,
                                            //callbackQuery.Message.MessageId,                                            
                                            text: $"Найдены компоненты состава.{Environment.NewLine}Для просмотра подробной информации по компоненту нажми на его название ниже.",
                                            replyMarkup: inlineKeyboard,
                                            cancellationToken: token);

                        }
                        else
                        {
                            var callbackComponent = DataBase.FindComponentsInBase(callbackQuery.Data);
                            foreach (СomponentsDataBase com in callbackComponent)
                            {
                                if (callbackQuery.Data.StartsWith(com.Name))
                                {
                                    InlineKeyboardMarkup inlineKeyboard = null;
                                    userButtons.TryGetValue(callbackQuery.Message.Chat.FirstName, out inlineKeyboard);
                                    if (com.LastName != "")
                                    {
                                        await botClient.EditMessageTextAsync(
                                            chatId: callbackQuery.Message.Chat.Id,
                                            callbackQuery.Message.MessageId,
                                            $"Думаю",
                                            replyMarkup: inlineKeyboard,
                                            cancellationToken: token);
                                        await botClient.EditMessageTextAsync(
                                            chatId: callbackQuery.Message.Chat.Id,
                                            callbackQuery.Message.MessageId,
                                            $"E-{com.Key} или {com.Name} или {com.LastName}{Environment.NewLine}☠️Опасность: {com.Danger}{Environment.NewLine}🍉Влияние на продукт: {com.ActionOnTheProduct}{Environment.NewLine}🧔‍♀️Действие на человека: {com.InfluenceOnPerson}",
                                            replyMarkup: inlineKeyboard,
                                            cancellationToken: token);
                                    }
                                    
                                    else
                                    {
                                        await botClient.EditMessageTextAsync(
                                            chatId: callbackQuery.Message.Chat.Id,
                                            callbackQuery.Message.MessageId,
                                            $"Думаю",
                                            replyMarkup: inlineKeyboard,
                                            cancellationToken: token);
                                        await botClient.EditMessageTextAsync(
                                            chatId: callbackQuery.Message.Chat.Id,
                                            callbackQuery.Message.MessageId,
                                            $"E-{com.Key} или {com.Name}{Environment.NewLine}☠️Опасность: {com.Danger}{Environment.NewLine}🍉Влияние на продукт: {com.ActionOnTheProduct}{Environment.NewLine}🧔‍♀️Действие на человека: {com.InfluenceOnPerson}",
                                            replyMarkup: inlineKeyboard,
                                            cancellationToken: token);

                                    }
                                }
                            }
                        }

                    }
                    break; 
            }
        }


        static private Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new Exception("Это ошибка");
        }


    }
}