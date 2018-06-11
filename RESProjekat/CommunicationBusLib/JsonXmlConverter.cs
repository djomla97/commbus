using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CommunicationBusLib
{
    public class JsonXmlConverter
    {
        public JsonXmlConverter()
        {

        }

        public string ConvertFromXml(string xml)
        {
            string json = "";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            json = JsonConvert.SerializeXmlNode(xmlDocument);

            return json;
        }
        public string ConvertToXml(string json)
        {
          //  XmlDocument xmlDocument = (XmlDocument)JsonConvert.DeserializeXmlNode(json);
          //  string xml = xmlDocument.ToString();

            XNode xmlResponse = JsonConvert.DeserializeXNode(json, "request");

            return xmlResponse.ToString();

         //   return xml;
        }
    }
}
