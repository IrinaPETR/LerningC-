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
            SingleFile instance = SingleFile.Instance;
            using (StreamReader sr = new StreamReader(SingleFile.Instance.myFileStream))
            {
                string readLine = sr.ReadToEnd();
                readLine = readLine.Replace("\n", "");
               List<Subscriber> sub = new List<Subscriber>(readLine.Length);
                char[] delimiterChars = { ' ', '\n', '\r' };
                string[] units = readLine.Split(delimiterChars);
                for (int i = 0; i < units.Length; i=i+2)
                {
                    var subscriber = new Subscriber(units[i], units[i+1]);
                    sub.Add(subscriber);
                }
                //sr.Close();
                //SingleFile.Instance.myFileStream.Close();
                return sub;
            }


        }

        public static void WriteInBook(List<Subscriber> sub)
        {
            //SingleFile instance = SingleFile.Instance;
            using (StreamWriter fileWrite = new StreamWriter(SingleFile.Instance.myFileStream, System.Text.Encoding.Default))
            {
                for (int i = 0; i < sub.Count; i++)
                {
                    string writeLine = sub[i].Name + " " + sub[i].PhoneNumber;
                    fileWrite.WriteLine(writeLine);
                }
            }
        } 


    }


}
