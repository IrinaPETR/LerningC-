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
                            //await botClient.SendTextMessageAsync(message.Chat.Id, $"{text}");
                            text = text.ToLower();
                            SearchByPhoto(message, text);

                            return;
                        }



                        if (message?.Text != null)
                        {
                            string text = message.Text.ToLower();
                            Console.WriteLine($"{message.Chat.FirstName} -> {message.Text}");

                            if (text.StartsWith("хочу фото") || text.StartsWith("фото") || text.StartsWith("фотография"))
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
                                SearchByText(message, text);
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

                            InlineKeyboardButton[][] arrayButton = СreatingButtons(value);
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

                    //default:
                    //    break;
            }

            string TextPreparation(string text)
            {
                text = text.Replace("найти", "");
                text = text.Replace("е-", "");
                //text = text.Replace("е", "");
                text = text.Replace("e-", "");
                text = text.Replace("ё", "е");
                text = text.Replace(")", "");
                text = text.Replace("(", "");
                text = text.Trim();
                return text;
            }

            string FindInBase(Message message, string[] words)
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

            InlineKeyboardButton[][] СreatingButtons(List<СomponentsDataBase> componentsToButton)
            {

                InlineKeyboardButton[][] arrayButton = new InlineKeyboardButton[componentsToButton.Count + 1][];
                List<InlineKeyboardButton[]> massivButton = new List<InlineKeyboardButton[]>();

                for (int i = 0; i < componentsToButton.Count; i++)
                {
                    if (componentsToButton[i].LastName != "")
                        massivButton.Add(new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(text: $"E-{componentsToButton[i].Key} или {componentsToButton[i].Name} или {componentsToButton[i].LastName}", callbackData: $"{componentsToButton[i].Name}") });
                    else massivButton.Add(new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(text: $"E-{componentsToButton[i].Key} или {componentsToButton[i].Name}", callbackData: $"{componentsToButton[i].Name}") });
                    arrayButton[i] = massivButton[i];
                }

                massivButton.Add(new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(text: $"История моих поисков", callbackData: "история") });
                arrayButton[componentsToButton.Count] = massivButton[componentsToButton.Count];
                return arrayButton;
            }


            
            async void SearchByPhoto(Message message, string text)
            {
                components.Clear();
                notFoundComponents.Clear();
                text = TextPreparation(text);
                //text = text.Replace("е", "");
                string[] words = text.Split(new char[] { ' ', ',', '.', '-', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

                FindInBase(message, words);
                InlineKeyboardButton[][] arrayButton = СreatingButtons(components);
                InlineKeyboardMarkup inlineKeyboard = new(arrayButton);

                userButtons.Remove(message.Chat.FirstName); //удаляет по ключу элемент из словаря
                userButtons.Add(message.Chat.FirstName, inlineKeyboard);

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
                string[] words = words = text.Split(new char[] { ',', 'e', 'е'}, StringSplitOptions.RemoveEmptyEntries);
                string textMessageWithNotFoundComponents = FindInBase(message, words);

                InlineKeyboardButton[][] arrayButton = СreatingButtons(components);
                InlineKeyboardMarkup inlineKeyboard = new(arrayButton);

                userButtons.Remove(message.Chat.FirstName); //удаляет по ключу элемент из словаря
                userButtons.Add(message.Chat.FirstName, inlineKeyboard);

                if (components.Count == 0) await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: $"Не найдены компоненты: {textMessageWithNotFoundComponents}" + $"{Environment.NewLine} Давай ещё раз попробуем!",
                                parseMode: ParseMode.Html,
                                disableWebPagePreview: false,
                                replyMarkup: inlineKeyboard,
                                cancellationToken: token);
                else if (textMessageWithNotFoundComponents == null)
                {
                    await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: $"{Environment.NewLine}Я нашёл 😎" + $"{Environment.NewLine}Для просмотра подробной информации по компоненту нажми на него и я обновлю это сообщение))",
                                replyMarkup: inlineKeyboard,
                                cancellationToken: token);
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: $"Не найдены компоненты: {textMessageWithNotFoundComponents}" + $"{Environment.NewLine}Другие я нашел 😎" + $"{Environment.NewLine}Для просмотра подробной информации по компоненту нажми на него и я обновлю это сообщение))",
                                replyMarkup: inlineKeyboard,
                                cancellationToken: token);
                }
            }
        }


        static private Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new Exception("Это ошибка");
        }


    }
}