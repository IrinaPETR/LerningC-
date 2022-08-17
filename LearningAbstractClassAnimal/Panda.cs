using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningAbstractClassAnimal
{
    public class Panda : Animal
    {
        public override double SpeadOfMs { get; protected set; }
        public override double AgeInYears { get; protected set; }
        public override double LenghtOfCm { get; set; }

        public override void Eat()
        {
            Console.WriteLine("Я ем моллюсков и рыб.");
            if (this.LenghtOfCm != 0) Console.WriteLine($"Мне нужно ПРОКОРМИТЬ {this.LenghtOfCm} см своего тела))");
        }

        public override void Move()
        {
            Console.WriteLine($"Я очень медленный, я не хочу двигаться со скоростью {this.SpeadOfMs}");
        }

        public void Slipe()
        {
            Console.WriteLine("Я сплю. Меня не трогать)");
        }

        public Panda(double speadOfMs, double ageInYears) : base(speadOfMs, ageInYears) { }
    }
}
