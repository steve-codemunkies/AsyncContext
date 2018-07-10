using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncContext.App
{
    public class Program
    {
        protected static AsyncLocal<int> AsyncLocalInt { get; set; }
        protected static Random Random { get; set; }
        public static async Task Main(string[] args)
        {
            AsyncLocalInt = new AsyncLocal<int>();
            Random = new Random((int)DateTime.UtcNow.Ticks);

            var tasks = new List<Task>();

            for(var i = 0; i < 5; i++)
            {
                tasks.Add(OuterMethodAsync(i));
            }

            await Task.WhenAll(tasks);
        }

        private static async Task OuterMethodAsync(int i)
        {
            Console.WriteLine("[{0}] Entering OuterMethod Thread: {1}", i, Thread.CurrentThread.ManagedThreadId);
            AsyncLocalInt.Value = i;

            await InnerMethodAsync(Random.Next(200, 401));

            var j = AsyncLocalInt.Value;
            Console.WriteLine("[{0}] Exiting OuterMethod Thread: {1}; AsyncLocal retrieved: {2}", i, Thread.CurrentThread.ManagedThreadId, j);
        }

        private static async Task InnerMethodAsync(int waitTime)
        {
            Console.WriteLine("[{0}] Entering InnerMethod Thread: {1}; Wait time: {2}", AsyncLocalInt.Value, Thread.CurrentThread.ManagedThreadId, waitTime);

            await Task.Delay(waitTime);

            Console.WriteLine("[{0}] Exiting InnerMethod Thread: {1}; Wait time: {2}", AsyncLocalInt.Value, Thread.CurrentThread.ManagedThreadId, waitTime);
        }
    }
}
