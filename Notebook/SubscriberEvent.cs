using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook
{
    internal class SubscriberEvent
    {
        public string Message { get;}

        //public Subscriber sub { get;}
        public SubscriberEvent(string message)
        {
            Message = message;
            //this.sub = sub;
        }
    }
}
