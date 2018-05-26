﻿using System;
using WebClientLib;

namespace ClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebClient webClient = new WebClient();

            string jsonQuery = "{ \" name\" = \" pera \", \" type\" = \" 1 \" }";

            Console.WriteLine(webClient.SendRequest("GET /resource" + " " + jsonQuery));

            Console.ReadKey();
        }
    }
}