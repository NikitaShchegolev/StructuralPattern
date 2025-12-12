using Bridge.Solution;

namespace Bridge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var device = new Tv();
            var remote = new Remote(device);
            remote.TogglePower();
            var device2 = new Radio();
            remote = new Remote(device2);
            remote.TogglePower();
            Console.ReadKey();
        }
    }
}
