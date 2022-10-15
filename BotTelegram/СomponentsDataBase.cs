using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotTelegram
{
    internal class СomponentsDataBase
    {
        private string key;
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private string danger;

        public string Danger
        {
            get
            {
                return danger;
            }
            set
            {
                danger = value;
            }
        }


        private string influenceOnPerson;

        public string InfluenceOnPerson
        {
            get
            {
                return influenceOnPerson;
            }
            set
            {
                    influenceOnPerson = value;
            }
        }

        private string actionOnTheProduct;

        public string ActionOnTheProduct
        {
            get
            {
                return actionOnTheProduct;
            }
            set
            {
                    actionOnTheProduct = value;
            }
        }

        private string lastName;

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            { 
                    lastName = value;
            }
        }

        public СomponentsDataBase(string key, string name, string danger, string actionontheproduct, string influenceonperson, string lastname)
        {
            this.Key = key;
            this.Name = name;
            this.Danger = danger;
            this.ActionOnTheProduct = actionontheproduct;
            this.InfluenceOnPerson = influenceonperson;
            this.LastName = lastname;
        }

    }
}
