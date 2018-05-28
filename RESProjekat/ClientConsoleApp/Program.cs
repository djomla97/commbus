using System;
using WebClientLib;

namespace ClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebClient webClient = new WebClient();

            string request = string.Empty;
            while(true)
            {
                Console.WriteLine("Enter request: ");
                request = Console.ReadLine();

                if (request.Equals("exit"))
                    break;

                Console.WriteLine(webClient.SendRequest(request));
            }

            // string jsonQuery = "{ \" name\" = \" pera \", \" type\" = \" 1 \", \" connectedTo\" = \" 3, 5 \",  \" connectedType\" = \" 3, 4, 5 \", \" fields\" = \" id, title, name \"}";

            //Console.WriteLine(webClient.SendRequest("GET /resource" + " " + jsonQuery));

            Console.ReadKey();
        }
    }
}
