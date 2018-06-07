using RepositoryLib;
using SharedResources;
using SharedResources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationBusLib
{
    public class CommunicationBus
    {
        private static JsonXmlConverter jsonXmlConverter;
        private static DbXmlConverter dbXmlConverter;
        private static IResponse response;
        private static Repository repository = new Repository();

        public string SendCommand (string command)
        {
            jsonXmlConverter = new JsonXmlConverter();
            dbXmlConverter = new DbXmlConverter();

           

            string xml = jsonXmlConverter.ConvertToXml(command);
            string sql = dbXmlConverter.ConvertFromXml(xml);
            string json;

            //saljemo sql zahtev ka bazi podataka
           // response = repository.DoQuery(sql); //problem jer vracamo list<resposne>

            //pretvaramo response u Xml
            
            xml = dbXmlConverter.ConvertToXml(response);
            //pretvaramo u Json
            json = jsonXmlConverter.ConvertFromXml(xml);

            //
            return json;

        }

        
    }
}
