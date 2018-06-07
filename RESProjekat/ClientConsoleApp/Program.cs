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
            while (true)
            {
                Console.WriteLine("Request formats:");
                Console.WriteLine("\t GET /tableName/{id} {\"key\"=\"value\", \"connectedTo\"=\"values\", \"fields\"=\"values\"}");
                Console.WriteLine("\t ID is optional, but if you enter it, further queries will be ignored \n");

                Console.WriteLine("\t POST /tableName {\"key\"=\"value\", \"key\"=\"value\" ... }");
                Console.WriteLine("\t ID is incremental and unique, no need to pass it \n");

                Console.WriteLine("\t PATCH /tableName/id {\"key\"=\"newValue\", \"key\"=\"newValue\" ... }");
                Console.WriteLine("\t Update entity with id \n");

                Console.WriteLine("\t DELETE /tableName/id");
                Console.WriteLine("\t Deletes entity with id \n");

                Console.WriteLine("\t Tables: resource (GET, POST, PATCH, DELETE), connection (POST, DELETE) \n");

                Console.Write("Enter request (exit to quit): ");
                request = Console.ReadLine();

                if (request.Equals("exit"))
                    break;

                Console.WriteLine(Environment.NewLine + webClient.SendRequest(request));

                Console.WriteLine("\nPress any key to enter another request.");
                Console.ReadKey();
                Console.Clear();
            }

            Console.ReadKey();
        }
    }
}
