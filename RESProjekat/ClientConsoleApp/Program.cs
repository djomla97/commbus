using System;
using WebClientLib;

namespace ClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebClient webClient = new WebClient();

            string jsonQuery = "{ \" name\" = \" pera \", \" type\" = \" 1 \", \" connectedTo\" = \" 3, 5 \",  \" connectedType\" = \" 3, 4, 5 \", \" fields\" = \" id, title, name \"}";

            Console.WriteLine(webClient.SendRequest("GET /resource" + " " + jsonQuery));

            Console.ReadKey();
        }
    }
}
