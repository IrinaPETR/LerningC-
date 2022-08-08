namespace Notebook
{
    internal class PhoneBookManagement
    {
        /// <summary>
        /// Чтение контактов телефонной книги из локального файла.
        /// </summary>
        /// <returns>Динамический массив типа Subscriber, который состоит из контактов телефоннйо книги</returns>
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
        /// <summary>
        /// Запись телефонной книги в локальный файл, с удалением предыдущих записей.
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, который будет записан в файл</param>
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
        /// <summary>
        /// Запись нового контакта типа Subscriber в динамический массив
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, в который нужно добавить новый контакт</param>
        /// <returns>Динамический массив типа Subscriber после добавления нового контакта</returns>
        public static List<Subscriber> RecordNewSubscriber(List<Subscriber> sub)
        {
            Console.WriteLine($"{Environment.NewLine}Имя нового контакат:");
            string nameSubscriber = Console.ReadLine();

            Console.WriteLine($"{Environment.NewLine}Номер телефона нового контакат:");
            string phoneNumberSubscriber = Console.ReadLine();

             return sub = SubscriberBook.AddSubscriber(sub ,nameSubscriber, phoneNumberSubscriber);
            Console.WriteLine($"{Environment.NewLine}Контакт записан.");
        }

        /// <summary>
        /// Поиск номера телефона контакта в записной книге по известному имени
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, в котором будет производиться поиск</param>
        public static void SearchPhoneNumberSubscriber(List<Subscriber> sub)
        {
            Console.WriteLine($"{Environment.NewLine}Введите ИМЯ КОНТАКТА, который нужно найти:");
            string nameSubscriber = Console.ReadLine();
            Console.WriteLine($"{Environment.NewLine}Номер телефона контакта {nameSubscriber}:");
           Console.WriteLine($"{Environment.NewLine}" + SubscriberBook.SearchNumber(sub, nameSubscriber));
        }

        /// <summary>
        /// Поиск имени контакта в записной книге по известному номеру телефона
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, в котором будет производиться поиск</param>
        public static void SearchNameSubscriber(List<Subscriber> sub)
        {
            Console.WriteLine($"{Environment.NewLine}Введите НОМЕР ТЕЛЕФОНА контакта, который нужно найти:");
            string phoneNumberSubscriber = Console.ReadLine();
            Console.WriteLine($"{Environment.NewLine}Имя контака:");

           Console.WriteLine(SubscriberBook.SearchName(sub, phoneNumberSubscriber));
        }

        /// <summary>
        /// Удаление контакта из телефонной книги по его имени 
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, из которого будет удалён контакт</param>
        public static void DeleteSubscriber(List<Subscriber> sub)
        {
            Console.WriteLine($"{Environment.NewLine}Введите ИМЯ КОНТАКТА, который нужно УДАЛИТЬ:");
            string nameSubscriber = Console.ReadLine();

            SubscriberBook.DeleteSubscriber(sub, nameSubscriber);
        }

        /// <summary>
        /// Выводит на экран все контакты телефонной книги
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, который будет выведен на экран</param>
        public static void ShowAllPhoneBook(List<Subscriber> sub)
        {
            Console.WriteLine($"{Environment.NewLine}Твоя книга КОНТАКТОВ:");
            SubscriberBook.ShowAllSubscriber(sub);
        }

        /// <summary>
        /// Перезаписывает номер телефона контакта в телефонной книге по известному имени контакта
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, в котором будет производиться перезапись контакта</param>
        /// <returns>Динамический массив типа Subscriber после перезаписи контакта</returns>
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
