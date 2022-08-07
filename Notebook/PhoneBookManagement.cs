namespace Notebook
{
    internal class PhoneBookManagement
    {

        public static List<Subscriber> ReadBook()
        {
            using (StreamReader sr = File.OpenText(SingleFile.Instance))
            {
                List<Subscriber> sub = new List<Subscriber>();
                while (sr.EndOfStream == false)
                {
                    var subscriberLine = sr.ReadLine();
                    var nameAndPhone = subscriberLine.Split(' ');
                    var subscriber = new Subscriber(nameAndPhone[0], nameAndPhone[1]);
                    sub.Add(subscriber);
                }
                return sub;
            }

        }

        public static void WriteInBook(List<Subscriber> sub)
        {
            File.WriteAllText(SingleFile.Instance, string.Empty);
            using (StreamWriter fileWrite = File.AppendText(SingleFile.Instance))
            {
                for (int i = 0; i < sub.Count; i++)
                {
                    string writeLine = sub[i].Name + " " + sub[i].PhoneNumber;
                    fileWrite.WriteLine(writeLine);
                }
            }
        }

        public static List<Subscriber> RecordNewSubscriber(List<Subscriber> sub)
        {
            Console.WriteLine($"{Environment.NewLine}Имя нового контакат:");
            string nameSubscriber = Console.ReadLine();

            Console.WriteLine($"{Environment.NewLine}Номер телефона нового контакат:");
            string phoneNumberSubscriber = Console.ReadLine();

             return sub = SubscriberBook.AddSubscriber(sub ,nameSubscriber, phoneNumberSubscriber);
            Console.WriteLine($"{Environment.NewLine}Контакт записан.");
        }

        public static void SearchPhoneNumberSubscriber(List<Subscriber> sub)
        {
            Console.WriteLine($"{Environment.NewLine}Введите ИМЯ КОНТАКТА, который нужно найти:");
            string nameSubscriber = Console.ReadLine();
            Console.WriteLine($"{Environment.NewLine}Номер телефона контакта {nameSubscriber}:");
           Console.WriteLine($"{Environment.NewLine}" + SubscriberBook.SearchNumber(sub, nameSubscriber));
        }

        public static void SearchNameSubscriber(List<Subscriber> sub)
        {
            Console.WriteLine($"{Environment.NewLine}Введите НОМЕР ТЕЛЕФОНА контакта, который нужно найти:");
            string phoneNumberSubscriber = Console.ReadLine();
            Console.WriteLine($"{Environment.NewLine}Имя контака:");

           Console.WriteLine(SubscriberBook.SearchName(sub, phoneNumberSubscriber));
        }

        public static void DeleteSubscriber(List<Subscriber> sub)
        {
            Console.WriteLine($"{Environment.NewLine}Введите ИМЯ КОНТАКТА, который нужно УДАЛИТЬ:");
            string nameSubscriber = Console.ReadLine();

            SubscriberBook.DeleteSubscriber(sub, nameSubscriber);
        }

        
        public static void ShowAllPhoneBook(List<Subscriber> sub)
        {
            Console.WriteLine($"{Environment.NewLine}Твоя книга КОНТАКТОВ:");
            SubscriberBook.ShowAllSubscriber(sub);
        }

        public static List<Subscriber> RewritePhoneNumberSubscriber(List<Subscriber> sub)
        {
            Console.WriteLine($"{Environment.NewLine}Введите ИМЯ КОНТАКТА, который нужно перезаписать:");
            string nameSubscriber = Console.ReadLine();
            Console.WriteLine($"{Environment.NewLine}НОВЫЙ номер телефона контакта {nameSubscriber}:");
            string phoneNumberSubscriber = Console.ReadLine();
            sub = SubscriberBook.RewriteSubscriber(sub, nameSubscriber, phoneNumberSubscriber);
            return sub;
        }

    }


}
