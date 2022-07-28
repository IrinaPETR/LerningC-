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
                if (sub[i].Name == name) sub.RemoveAt(i);
            }
            return sub;

        }

        public static List<Subscriber> AddSubscriber(List<Subscriber> sub, string name, string number)
        {
            sub.Add(new Subscriber(name, number));
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
