using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Notebook
{
    public class SingleFile
    {
        private static string instance;

        public static string Instance
        {
            get
            {
                if (instance == null)
                    instance = GetExecutionFolder();

                return instance;
            }
        }

        private static string GetExecutionFolder()
        {
            return (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/phonebook.txt");
        }
    }
}

