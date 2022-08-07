using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook
{
    internal class SubscriberBook
    {
        public static string SearchNumber(List<Subscriber> sub, string name)
        {
            for (int i = 0; i < sub.Count; i++)
            {
                if (sub[i].Name == name)
                    return sub[i].PhoneNumber;
            }

            return $"Контакт с именем {name} не найден";
        }

        public static string SearchName(List<Subscriber> sub, string number)
        {
            for (int i = 0; i < sub.Count; i++)
            {
                if (sub[i].PhoneNumber == number)
                    return sub[i].Name;
            }
            return $"Контакт с номером {number} не найден";
        }

        public static List<Subscriber> DeleteSubscriber(List<Subscriber> sub, string name)
        {
            for (int i = 0; i < sub.Count; i++)
            {
                if (sub[i].Name == name)
                {
                    sub.RemoveAt(i);
                    Console.WriteLine($"{Environment.NewLine}Контакт {name} УДАЛЁН!");
                    return sub;
                    break;
                }
            }
            Console.WriteLine($"{Environment.NewLine}Контакт с именем {name} не найден");
            return sub;

        }

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
            return sub;
        }


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


        public static void ShowAllSubscriber(List<Subscriber> sub)
        {
            foreach (var subscriber in sub)
            {
                Console.WriteLine(subscriber.Name + " " + subscriber.PhoneNumber);
            }

        }

    }
}
