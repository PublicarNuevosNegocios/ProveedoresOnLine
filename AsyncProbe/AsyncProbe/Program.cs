using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProbe
{
    class Program
    {
        private static string result;
        static void Main(string[] args)
        {
            Task wait = asyncTask();
            Console.WriteLine(result);
            wait.Wait();
        }
        static async Task asyncTask()
        {
            result = "Hello	World!";
            DateTime start = DateTime.Now;
            Console.WriteLine("async:	Starting");
            Thread.Sleep(1000);
            Console.WriteLine("async:	Running	for	{0}	seconds",
            DateTime.Now.Subtract(start).TotalSeconds);
            Console.WriteLine("async:	Done");            
        }
    }
}
