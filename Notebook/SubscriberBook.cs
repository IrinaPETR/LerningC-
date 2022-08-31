using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook
{
    internal class SubscriberBook
    {
        //public delegate void SubscriberHandler(SubscriberEvent e);
        public delegate void SubscriberHandler(string message);
        public static event SubscriberHandler? Notify;

        /// <summary>
        /// Поиск номера телефона контакта по известному имени
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, в котором будет производиться поиск контакта</param>
        /// <param name="name">Имя контакта, номер телефона которого нужно найти</param>
        /// <returns>Строка, содержащая номер телефона искомого контакта</returns>
        public static string SearchNumber(List<Subscriber> sub, string name)
        {
            for (int i = 0; i < sub.Count; i++)
            {
                if (sub[i].Name == name)
                    return sub[i].PhoneNumber;
            }

            return $"Контакт с именем {name} не найден";
        }

        /// <summary>
        /// Поиск имени контакта по известному номеру телефона
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, в котором будет производиться поиск контакта</param>
        /// <param name="number">Номер телефона контакта, имя которого нужно найти</param>
        /// <returns>Строка, содержащая имя искомого контакта</returns>
        public static string SearchName(List<Subscriber> sub, string number)
        {
            for (int i = 0; i < sub.Count; i++)
            {
                if (sub[i].PhoneNumber == number)
                    return sub[i].Name;
            }
            return $"Контакт с номером {number} не найден";
        }

        /// <summary>
        /// Удаляет контакт в динамическом массиве типа Subscriber по известному имени
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, из которого будет удалён контакт</param>
        /// <param name="name">Имя контакта, который нужно удалить</param>
        /// <returns>Динамический массив типа Subscriber, после удаления контакта</returns>
        public static List<Subscriber> DeleteSubscriber(List<Subscriber> sub, string name)
        {
            for (int i = 0; i < sub.Count; i++)
            {
                if (sub[i].Name == name)
                {
                    sub.RemoveAt(i);
                    Notify?.Invoke($"{Environment.NewLine}Контакт {name} УДАЛЁН!");
                    return sub;
                }
            }
            Console.WriteLine($"{Environment.NewLine}Контакт с именем {name} не найден");
            return sub;

        }

        /// <summary>
        /// Добавляет новый контакт в динамический массив типа Subscriber
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, в котором будет добавлен новый контакт</param>
        /// <param name="name">Имя нового контакта</param>
        /// <param name="number">Номер телефона нового контакта</param>
        /// <returns>Динамический массив типа Subscriber после добавления нового контакта</returns>
        public static List<Subscriber> AddSubscriber(List<Subscriber> sub, string name, string number)
        {
            for (int i = 0; i < sub.Count; i++)
            {
                if (sub[i].Name == name)
                {
                    Console.WriteLine($"{Environment.NewLine}Контакт с таким именем уже существует. Введи уникальное имя для номера {number}");
                    name = Console.ReadLine();
                    i = 0;
                }

                if (sub[i].PhoneNumber == number)
                {
                    Console.WriteLine($"{Environment.NewLine}Контакт с таким номером уже существует. Имя контакта: {sub[i].Name}");
                    break;
                }

            }

            sub.Add(new Subscriber(name, number));

            Notify?.Invoke($"{Environment.NewLine}Был записан контакт: {Environment.NewLine} Имя - {name}. Номер - {number}");
            //Notify?.Invoke(new SubscriberEvent($"{Environment.NewLine}Был записан контакт: {Environment.NewLine} Имя - {name}. Номер - {number}"));
            return sub;
            Console.WriteLine("Строчка после return. ТЫ ЧТОООО СРАБОТАЛААА????");
        }

        /// <summary>
        /// Перезаписывает контакт в телефонной книге
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, в котором будет производиться перезапись контакта</param>
        /// <param name="name">Имя существующего контакта</param>
        /// <param name="number">Новый номер телефона для контакта</param>
        /// <returns>Динамический массив типа Subscriber после перезаписи контакта</returns>
        public static List<Subscriber> RewriteSubscriber(List<Subscriber> sub, string name, string number)
        {
            for (int i = 0; i < sub.Count; i++)
            {
                bool trySearchName = false;
                if (sub[i].Name == name)
                {
                    sub.RemoveAt(i);
                    sub.Insert(i, new Subscriber(name, number));
                    Console.WriteLine($"{Environment.NewLine}Контакт переписан!");
                    trySearchName = true;
                    break;
                }
                else if (i == sub.Count && trySearchName == false)
                {
                    Console.WriteLine($"{Environment.NewLine}Контакт с таким именем НЕ существует. Уточните имя контакта:");
                    name = Console.ReadLine();
                    i = 0;
                }
            }
            return sub;
        }

        /// <summary>
        /// Вывод всех контактов телефонной книги на экран
        /// </summary>
        /// <param name="sub">Динамический массив типа Subscriber, который нужно вывести на экран</param>
        public static void ShowAllSubscriber(List<Subscriber> sub)
        {
            foreach (var subscriber in sub)
            {
                Console.WriteLine(subscriber.Name + " " + subscriber.PhoneNumber);
            }

        }

    }
}
