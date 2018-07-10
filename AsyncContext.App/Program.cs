using System;
using System.Threading.Tasks;

namespace AsyncContext.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Task.Delay(500);
            Console.WriteLine("Hello World!");
        }
    }
}
