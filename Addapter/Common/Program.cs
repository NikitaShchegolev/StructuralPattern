namespace Common
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string searchResult = "asdf";
            int tableCount;
            bool result = int.TryParse(searchResult.ToString(), out tableCount);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
