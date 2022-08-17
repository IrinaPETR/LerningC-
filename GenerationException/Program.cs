namespace GenerationException
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool repit = true;
            while (repit)
            {
                try
                {
                    Console.WriteLine($"{Environment.NewLine}Если хочешь вызвать ошибку, то нажми сразу Enter. Если хочешь НИЧЕГО, то введи что угодно)");
                    string? str = Console.ReadLine();
                    if (str == "") throw new Exception("Была введена пустая строчка");
                    else Console.WriteLine($"{Environment.NewLine}Отработало без ошибки. Печалька((");
                }

                catch (Exception exc)
                {
                    Console.WriteLine($"{Environment.NewLine}Ура! Ошибка:{Environment.NewLine}{exc.Message}");
                }

                finally
                {
                    Console.WriteLine($"{Environment.NewLine}Давай попробуем ещё раз! Хочешь повторить?{Environment.NewLine}Если да, то нажми пробел, если хочешь выйти, то - любую другую клавишу");
                    var consoleAction = Console.ReadKey();
                    string key;
                    if (consoleAction.KeyChar.ToString() != " ") repit = false;
                }
            }
            //
        }
    }
}