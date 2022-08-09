namespace dynamic_array_List
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyList<int> myList = new MyList<int>();
            myList.Add(0);
            myList.Add(1);
            myList.Add(2);
            myList.Add(3);
            myList.Delete(0);
            myList.Add(4);
            int lenght = myList.Count;
            myList.Add(5);
            myList.Add(6);
            myList.Add(7);
        }
    }
}