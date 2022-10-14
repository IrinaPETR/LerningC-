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

            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "One", "Two" },
                new KeyboardButton[] { "Three", "Four" },
            })
            {
                ResizeKeyboard = true
            };

            //Message sentMessage = await botClient.SendTextMessageAsync(
            //    chatId: message.Chat.Id,
            //    text: "Choose a response",
            //    replyMarkup: replyKeyboardMarkup);

            if (message != null)
            {
                Console.WriteLine($"{message.Chat.FirstName} -> {message.Text}");

                try
                {
                    if (message.Text.ToLower().Contains("привет") || message.Text.ToLower().Contains("ghbdtn"))
                    {
                        //DataBase.SeeAll();
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Приветики");
                        return;
                    }

                    if (message.Text.ToLower().Contains("two"))
                    {
                        
                        //await botClient.SendTextMessageAsync(message.Chat.Id, $"Приветики");
                        return;
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