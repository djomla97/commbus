using Newtonsoft.Json;
using SharedResources;
using SharedResources.Interfaces;
using System;
using System.Linq;

namespace WebClientLib
{
    public class WebClient : IWebClient
    {
        // TODO: implementirati i za Fields jos

        public WebClient()
        {

        }

        /// <summary>
        ///     Sends a request to a repository and retrieves a JSON response
        /// </summary>
        /// <param name="request">user defined request</param>
        /// <returns></returns>
        public string SendRequest(string request)
        {
            // GET /resource/1
            // ili GET /resource { "name"="pera", "type"="2", connectedTo="4" }

            IRequest requestParsed = ParseRequest(request);
            IResponse responseObject = new Response();

            // provere da li je dobro unesen zahtev

            if (requestParsed.Noun == null)
            {
                responseObject.Payload = new Payload();
                responseObject.Payload.Resource = null;
                responseObject.Payload.ErrorMessage = "Noun is not valid.";
                responseObject.Status = Status.BAD_FORMAT.ToString();
                responseObject.StatusCode = StatusCode.BAD_FORMAT_CODE;
            }
            else
            {

                if (requestParsed.Noun.Contains("connection"))
                {
                    if (requestParsed.Verb != "POST" && requestParsed.Verb != "DELETE")
                    {
                        responseObject.Payload = new Payload();
                        responseObject.Payload.Resource = null;
                        responseObject.Payload.ErrorMessage = "Method not supported. Must be POST or DELETE";
                        responseObject.Status = Status.BAD_FORMAT.ToString();
                        responseObject.StatusCode = StatusCode.BAD_FORMAT_CODE;
                    }
                }
                else if (requestParsed.Noun.Contains("resource"))
                {
                    if (requestParsed.Verb != "POST" && requestParsed.Verb != "DELETE" && requestParsed.Verb != "GET" && requestParsed.Verb != "PATCH")
                    {
                        responseObject.Payload = new Payload();
                        responseObject.Payload.Resource = null;
                        responseObject.Payload.ErrorMessage = "Method not supported. Must be GET, POST, PATCH or DELETE";
                        responseObject.Status = Status.BAD_FORMAT.ToString();
                        responseObject.StatusCode = StatusCode.BAD_FORMAT_CODE;
                    }
                }
            }

            string jsonFormat = JsonConvert.SerializeObject(requestParsed); // ovde vraca Request jer testiramo jos

            return jsonFormat;
        }


        // helper za parsiranje zahteva u konacan JSON oblik za slanje na CommunicationBus
        private IRequest ParseRequest(string requestToParse)
        {
            Request parsedRequest = new Request();

            // razdvojimo request tako da dobijemo odvojeno GET, pa onda /resource/{id}
            // a zatim u JSON obliku ostatak filtera, ukoliko ID nije naveden
            string[] requestSplit = requestToParse.Split(' ');

            // uzmemo metodu i ime tabele
            string method = requestSplit[0];
            string table = requestSplit[1];

            // postavimo metodu u request objekat
            parsedRequest.Verb = method;
            parsedRequest.Noun = table;

            // ukoliko imamo ID onda nemamo dodatne filtere
            if (table.Any(char.IsDigit))
            {
                parsedRequest.Query = null;
                parsedRequest.Fields = null;
            }
            else
            {
                try
                {
                    // moramo spojiti sve razmake posle
                    string jsonQuery = "";

                    for (int i = 2; i < requestSplit.Length; i++)
                    {
                        jsonQuery += requestSplit[i];
                    }

                    jsonQuery = jsonQuery.Remove(0, 1); // uklonimo { 
                    jsonQuery = jsonQuery.Remove(jsonQuery.Length - 1, 1); // uklonimo }
                    jsonQuery = jsonQuery.Replace(',', ';'); // sa specifikacije projekta

                    // sada uzmemo svaki query i formatiramo tako da bude kao sa specifikacije projekta
                    string[] queries = jsonQuery.Split(';');
                    string[] parsedQueries = new string[queries.Length];

                    for (int i = 0; i < queries.Length; i++)
                    {
                        if (queries[i].Any(char.IsDigit))
                        {
                            // "type" = "1" -> type=1
                            string[] temp = queries[i].Split('=');
                            temp[0] = temp[0].Replace("\"", "");
                            temp[1] = temp[1].Replace("\"", "");

                            parsedQueries[i] = temp[0] + "=" + temp[1];
                        }
                        else
                        {
                            // "name" = "pera" -> name='pera'
                            string[] temp = queries[i].Split('=');
                            temp[0] = temp[0].Replace("\"", "");
                            temp[1] = temp[1].Replace('"', '\'');

                            parsedQueries[i] = temp[0] + "=" + temp[1];
                        }
                    }

                    // ako uspesno parsira onda sacuvaj u Query
                    // isto kao sa specifikacije odradjeno, nema ; na kraju
                    for(int i = 0; i < parsedQueries.Length; i++)
                    {
                        if(i != parsedQueries.Length-1)
                        {
                            parsedRequest.Query += parsedQueries[i] + ";";
                        }
                        else
                        {
                            parsedRequest.Query += parsedQueries[i];
                        }
                    }

                }
                catch (Exception)
                {
                    parsedRequest.Noun = null;
                    parsedRequest.Query = null;
                    parsedRequest.Fields = null;
                }

                
            }

            return parsedRequest;

        }

    }
}
