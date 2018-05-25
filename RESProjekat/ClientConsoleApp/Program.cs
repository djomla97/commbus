using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebClientLib;

namespace ClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient webClient = new WebClient();
            Console.WriteLine(webClient.SendRequest("NESTO /resource/1"));

            Console.ReadKey();
        }
    }
}
