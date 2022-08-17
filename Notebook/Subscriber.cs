using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Notebook
{
    internal class Subscriber
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == null) name = value;
            }
        }

        private string phoneNumber;

        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                if (phoneNumber == null) phoneNumber = value;
            }
        }


        public Subscriber()
        { }

        /// <summary>
        /// Конструктор класса Subscriber
        /// </summary>
        /// <param name="name">Имя контакта в формате строки</param>
        /// <param name="phoneNumber">Номер телефона контакта в формате строки</param>
        public Subscriber(string name, string phoneNumber)
        {
            this.Name = name;
            this.PhoneNumber = phoneNumber;
        }

        /// <summary>
        /// Конструктор класса Subscriber
        /// </summary>
        /// <param name="data">Массив типа стринг, содержащий данные контактов в формате: "имя"+"пробел"+"номер телефона"[Конец строки]</param>
        public Subscriber(string[] data)
        {
            this.Name = data[0];
            this.PhoneNumber = data[1];
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Subscriber sub = (Subscriber)obj;
                return (this.Name == sub.Name) && (this.PhoneNumber == sub.PhoneNumber);
            }
        }

    }
}
