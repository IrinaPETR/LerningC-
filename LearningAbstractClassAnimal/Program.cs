namespace LearningAbstractClassAnimal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Animal turtle = new Turtle(1, 0.5);
            turtle.Eat();

            turtle.LenghtOfCm = 1000;
            turtle.Eat();

            Console.WriteLine($"{Environment.NewLine}--------------------------------------------------{Environment.NewLine}");

            Panda panda = new Panda(0, 8);
            panda.Move();
            panda.Slipe();

            Console.ReadKey();
        }
    }
}