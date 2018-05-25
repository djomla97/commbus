using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedResources;
using Newtonsoft.Json;
using SharedResources.Interfaces;

namespace WebClientLib
{
    public class WebClient
    {      

        public WebClient()
        {
            
        }

        public string SendRequest(string request)
        {
            IRequest requestObject = new Request();
            IResponse responseObject = new Response();

            // GET /resource/1
            // ili GET /resource { "name"="pera", "type"="2", connectedTo="4" }

            string[] requestParsed = request.Split(' ');

            // uzmemo ime tabele i metode
            string method = requestParsed[0];            
            string table = requestParsed[1].Split('/')[1];

            if(table == "resource")
            {
                if(method != "GET" && method != "POST" && method != "PATCH" && method != "DELETE")
                {
                    responseObject.Payload = new Payload();
                    responseObject.Payload.ErrorMessage = "Method not supported. Must be GET, POST, PATCH or DELETE.";
                    responseObject.Status = Status.BAD_FORMAT.ToString();
                    responseObject.StatusCode = StatusCode.BAD_FORMAT_CODE;
                }
            }

            string jsonFormat = JsonConvert.SerializeObject(responseObject);

            return jsonFormat;
        }

    }
}
