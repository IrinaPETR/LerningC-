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
        public Subscriber(string name, string phoneNumber)
        {
            this.Name = name;
            this.PhoneNumber = phoneNumber;
        }

        public Subscriber(string[] data)
        {
            this.Name = data[0];
            this.PhoneNumber = data[1];
        }

    }
}
