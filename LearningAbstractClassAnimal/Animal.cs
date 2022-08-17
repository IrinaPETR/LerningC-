using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningAbstractClassAnimal
{
    public abstract class Animal
    {
        public abstract double SpeadOfMs
        { get; protected set; }

        public abstract double AgeInYears { get; protected set; }

        public abstract double LenghtOfCm { get; set; }

        public abstract void Eat();
        public abstract void Move();

        public Animal(double speadOfMs, double ageInYears)
        {
            this.SpeadOfMs = speadOfMs;
            this.AgeInYears = ageInYears;
        }

    }
}
