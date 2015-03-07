using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageFeedback.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine("arg {0}", arg);
            }

            Console.WriteLine("Hello world");
        }
    }
}
