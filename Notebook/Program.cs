using System.Reflection;

namespace Notebook
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PhoneBookManagement.ManageNotify += Console.WriteLine;
            List<Subscriber> phoneBook1 = PhoneBookManagement.ReadBook();
            bool closePhoneBook = false;

            while (closePhoneBook == false)
            {
                string helloText = $"Привет, друг! Я - твоя личная записная книжка) {Environment.NewLine}Если ты хочешь:";
                string helloText1 = $"{Environment.NewLine}- Записать новый контакт, то нажми цифру '1';";
                string helloText2 = $"{Environment.NewLine}- Найти номер телефона по имени человека, то нажми цифру '2';";
                string helloText3 = $"{Environment.NewLine}- Определить имя человека по номеру телефона, то нажми цифру '3';";
                string helloText4 = $"{Environment.NewLine}- Удалить существующий контакт, то нажми цифру '4';";
                string helloText5 = $"{Environment.NewLine}- Перезаписать номер телефона у существующего контакта, то нажми цифру '5';";
                string helloText6 = $"{Environment.NewLine}- Вывести на экран полный список контактов, то нажми цифру '6';{Environment.NewLine}";
                

                Console.WriteLine(helloText + helloText1 + helloText2 + helloText3 + helloText4 + helloText5 + helloText6);
                var consoleAction = Console.ReadKey();
                int key;
                bool tryAction = int.TryParse(consoleAction.KeyChar.ToString(), out key);

                switch (key)
                {
                    case 1:
                        phoneBook1 = PhoneBookManagement.RecordNewSubscriber(phoneBook1);

                        break;
                    case 2:
                        PhoneBookManagement.SearchPhoneNumberSubscriber(phoneBook1);
                        break;
                    case 3:
                        PhoneBookManagement.SearchNameSubscriber(phoneBook1);
                        break;
                    case 4:
                        PhoneBookManagement.DeleteSubscriber(phoneBook1);
                        break;
                    case 5:
                        phoneBook1 = PhoneBookManagement.RewritePhoneNumberSubscriber(phoneBook1);
                        break;
                    case 6:
                        PhoneBookManagement.ShowAllPhoneBook(phoneBook1);
                        break;
                }

                var sub2 = new Subscriber("Ирина", "89821191164");
                Console.WriteLine(phoneBook1[10].Equals(sub2));
                
                
                Console.WriteLine($"{Environment.NewLine}Если хотите закрыть телефонну книгу и сохранить изменения, то нажмите - 9;{Environment.NewLine}");
                Console.WriteLine($"Если хотите продолжить работу с телефонной книгой, то нажмите - 0;{Environment.NewLine}");

                consoleAction = Console.ReadKey();
                tryAction = int.TryParse(consoleAction.KeyChar.ToString(), out key);
                if (key == 9) closePhoneBook = true;
                
                Console.Clear();
            }

            PhoneBookManagement.WriteInBook(phoneBook1);

        }
    }
}