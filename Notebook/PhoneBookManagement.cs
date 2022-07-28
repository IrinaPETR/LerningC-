using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Notebook
{
    internal class PhoneBookManagement
    {
        
        public static List<Subscriber> ReadBook()
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string[] readLine = File.ReadAllLines(path + "/phonebook.txt");
            List<Subscriber> sub = new List<Subscriber>(readLine.Length);
            for (int i = 0; i < readLine.Length; i++)
            {
                string[] units = readLine[i].Split(' ');
                var subscriber = new Subscriber(units);
                sub.Add(subscriber);
            }
            return sub;
        }


    }
}
