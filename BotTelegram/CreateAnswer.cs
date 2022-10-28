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
    internal class CreateAnswer
    {
        static public InlineKeyboardButton[][] СreatingButtons(List<СomponentsDataBase> componentsToButton)
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


        async static public void SearchByPhoto(Message message, string text, List<string> notFoundComponents, List<СomponentsDataBase> components, Dictionary<string, List<СomponentsDataBase>>? userComponents, Dictionary<string, InlineKeyboardMarkup>? userButtons, CancellationToken token, ITelegramBotClient botClient)
        {
            components.Clear();
            notFoundComponents.Clear();
            text = WordProcessing.TextPreparation(text);
            string[] words = text.Split(new char[] { ' ', ',', '.', '-', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            DataBase.FindInBase(message, words, notFoundComponents, components, userComponents);
            InlineKeyboardButton[][] arrayButton = CreateAnswer.СreatingButtons(components);
            InlineKeyboardMarkup inlineKeyboard = new(arrayButton);

            userButtons.Remove(message.Chat.FirstName); //удаляет по ключу элемент из словаря
            userButtons.Add(message.Chat.FirstName, inlineKeyboard);

            await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: $"Найдены компоненты состава.{Environment.NewLine}Для просмотра подробной информации по компоненту нажми на его название ниже.",
                            replyMarkup: inlineKeyboard,
                            cancellationToken: token);
        }


        async static public void SearchByText(Message message, string text, List<string> notFoundComponents, List<СomponentsDataBase> components, Dictionary<string, List<СomponentsDataBase>>? userComponents, Dictionary<string, InlineKeyboardMarkup>? userButtons, CancellationToken token, ITelegramBotClient botClient)
        {
            components.Clear();
            notFoundComponents.Clear();
            text = WordProcessing.TextPreparation(text);
            string[] words = words = text.Split(new char[] { ',', 'e', 'е' }, StringSplitOptions.RemoveEmptyEntries);
            string textMessageWithNotFoundComponents = DataBase.FindInBase(message, words, notFoundComponents, components, userComponents);

            InlineKeyboardButton[][] arrayButton = CreateAnswer.СreatingButtons(components);
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
}
