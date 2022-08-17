using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningAbstractClassAnimal
{
    public class Turtle : Animal
    {
        public override double SpeadOfMs { get; protected set; }
        public override double AgeInYears { get; protected set; }
        public override double LenghtOfCm { get; set; }

        public override void Eat()
        {
            Console.WriteLine("Я ем моллюсков и рыб.");
            if(this.LenghtOfCm != 0) Console.WriteLine($"Мне нужно ПРОКОРМИТЬ {this.LenghtOfCm} см своего тела))");
        }

        public override void Move()
        {
            Console.WriteLine($"Я ПЛЫВУ со скоростью {this.SpeadOfMs}");
        }

        public Turtle(double speadOfMs, double ageInYears) : base(speadOfMs, ageInYears) { }

        //public Turtle(double speadOfMs, double ageInYears, double lenghtOfCm)
        //{
        //    this.SpeadOfMs = speadOfMs;
        //    this.AgeInYears = ageInYears;
        //    this.LenghtOfCm = lenghtOfCm;
        //}
    }
}
