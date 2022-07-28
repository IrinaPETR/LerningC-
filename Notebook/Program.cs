using System.Reflection;

namespace Notebook
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Subscriber> sub = PhoneBookManagement.ReadBook();

            string number = SubscriberBook.SearchNumber(sub, "Ирина");
            Console.WriteLine(number);

            string name = SubscriberBook.SearchName(sub, "898211911641");
            Console.WriteLine(name);

            sub = SubscriberBook.AddSubscriber(sub, "Ira", "678911");

            SubscriberBook.ShowAllSubscriber(sub);
            sub = SubscriberBook.DeleteSubscriber(sub, "Ирина");
            Console.WriteLine("---------------------------");
            SubscriberBook.ShowAllSubscriber(sub);


            //StreamWriter str = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            //Console.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            //for (int i = 0; i < 256; i++)
            //{
            //    str.WriteLine(table[i]);
            //}
            //str.Close();


            //Subscriber[] Input = new Subscriber(File.ReadAllText(path));
            //Читаем из файла информацию
            //while ((Input = ReadFile.ReadLine()) != null)
            //{
            //    Console.WriteLine(Input);
            //}
        }
    }
}