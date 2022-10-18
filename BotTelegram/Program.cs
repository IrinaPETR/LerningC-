using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static BotTelegram.SearchText;
using System.Configuration;

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
            var message = update.Message;
            InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Хочу найти описание ингридиента", callbackData: "поиск"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Хочу посмотреть весь список доступных ингридиентов", callbackData: "все"),
                    },
                });
            await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Выбери действие",
                            replyMarkup: inlineKeyboard,
                            cancellationToken: token);

            //Task findeComponentsOnName = new Task(async () =>
            //{
            //    await botClient.SendTextMessageAsync(message.Chat.Id, $"Отправь названия ингридиентов состава через запятую. Например: Куркумин, алканин");
            //    var messageComponents = update.Message;
            //    if (messageComponents != null)
            //    {
            //        components = DataBase.FindComponentsInBase(messageComponents.Text);
            //        botClient.SendTextMessageAsync(messageComponents.Chat.Id, $"{components[0].ActionOnTheProduct}");
            //        return;
            //    }
            //});



            //Task HandleCallbackQuery = MyUpdate.ContinueWith((ITelegramBotClient botClient, CallbackQuery callbackQuery) =>
            //{
            //    if (callbackQuery.Data.Equals("поиск"))
            //    {
            //        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text: "Отправь названия ингридиентов состава через запятую. Например: Куркумин, алканин");
            //    }
            //});

            if (message != null)
                {
                    Console.WriteLine($"{message.Chat.FirstName} -> {message.Text}");
                

                try
                    {
                    async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
                    {
                        if (callbackQuery.Data.Equals("поиск"))
                        {
                            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text: "Отправь названия ингридиентов состава через запятую. Например: Куркумин, алканин");
                        }
                    }

                    if (message.Text.ToLower().Contains("привет") || message.Text.ToLower().Contains("ghbdtn"))
                        {
                            //components = DataBase.SeeAll();
                            await botClient.SendTextMessageAsync(message.Chat.Id, $"Приветики");
                            return;
                        }

                        if (message.Text.ToLower().Contains("хочу найти описание ингридиента"))
                        {
                            message = null;
                        findeComponentsOnName.Start();  // запускаем задачу

                        findeComponentsOnName.Wait();
                        //int result = findeComponentsOnName.Result;

                        //    components = DataBase.FindComponentsInBase("куркумин");
                        //await botClient.SendTextMessageAsync(message.Chat.Id, $"{components[0].ActionOnTheProduct}");
                        //return;
                    }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
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
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"{text}");
                    return;
                }
        }




        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new Exception("Это ошибка");
        }


    }
}