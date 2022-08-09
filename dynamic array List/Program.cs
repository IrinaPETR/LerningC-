namespace dynamic_array_List
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyList<int> myList = new MyList<int>();
            myList.Add(10);
            myList.Add(20);
            myList.Add(33);
            myList.Add(44);
            myList.Delete(0);
            myList.Add(5);
            myList.Add(6);
            myList.Add(7);
            myList.Add(8);
            //myList.Delete(0);
        }
    }
}