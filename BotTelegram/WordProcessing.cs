using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotTelegram
{
    internal class WordProcessing
    {
        static public string TextPreparation(string text)
        {
            text = text.Replace("найти", "");
            text = text.Replace("е-", "");
            text = text.Replace("e-", "");
            text = text.Replace("ё", "е");
            text = text.Replace(")", "");
            text = text.Replace("(", "");
            text = text.Trim();
            return text;
        }
    }
}
