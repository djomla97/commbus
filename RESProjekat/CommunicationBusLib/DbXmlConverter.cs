using Newtonsoft.Json;
using SharedResources;
using System.Text;
using System.Xml;

namespace CommunicationBusLib
{
    public class DbXmlConverter
    {
        public string ConvertToXml(IResponse response)
        {
            string jsonResponse = JsonConvert.SerializeObject(response);
            XmlDocument xmlResponse = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonResponse);

            return xmlResponse.ToString();
        }

        public string ConvertFromXml(string request)
        {
            // kako izgleda GET /resource/1 ???
            
            /*
            StringBuilder sqlQuery = new StringBuilder();

            // METHOD
            int firstIndexOfVerb = request.IndexOf("<verb>");
            int lastIndexOfVerb = request.IndexOf("</verb>");

            string method = request.Substring(firstIndexOfVerb + 6, lastIndexOfVerb - firstIndexOfVerb);

            // WHERE
            int firstIndexOfNoun = request.IndexOf("<noun>");
            int lastIndexOfNoun = request.IndexOf("</noun>");

            string whereQuery = request.Substring(firstIndexOfNoun + 6, lastIndexOfNoun - firstIndexOfNoun);

            string[] whereQueryParsed = whereQuery.Split('/');
            
            
            switch (method)
            {
                case "GET":
                    sqlQuery.Append("SELECT ");
                    break;
                case "POST":
                    sqlQuery.Append("INSERT INTO resources");
                    break;
                case "PATCH":
                    sqlQuery.Append("UPDATE resources SET ");
                    break;
                case "DELETE":
                    sqlQuery.Append("DELETE FROM resources ");
                    break;
            }
            */

            return "";
        }
    }
}
