namespace dynamic_array_List
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyList<string> myList = new MyList<string>();
            myList.Add("0");
            myList.Add("1");
            myList.Add("2");
            myList.Add("3");
            myList.DeleteByIndex(0);
            myList.Add("4");
            int lenght = myList.Count;
            myList.Add("5");
            myList.Add("6f");
            myList.Add("7");
            myList.DeleteIfExist("6f");
        }
    }
}