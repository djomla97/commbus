using RepositoryLib;
using SharedResources;
using SharedResources.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationBusLib
{
    public class CommunicationBus : ICommunicationBus
    {
        private static JsonXmlConverter jsonXmlConverter;
        private static DbXmlConverter dbXmlConverter;
        private static IResponse response;
        private IRepository repository;

        [ExcludeFromCodeCoverage]
        public CommunicationBus()
        {
            repository = new Repository();
        }

        public CommunicationBus(IRepository repo)
        {
            this.repository = repo;
        }

        public string SendCommand (string command)
        {
            jsonXmlConverter = new JsonXmlConverter();
            dbXmlConverter = new DbXmlConverter();

            string xml = jsonXmlConverter.ConvertToXml(command);
            string sql = dbXmlConverter.ConvertFromXml(xml);
            string json ="";

            //saljemo sql zahtev ka bazi podataka
            response = repository.DoQuery(sql);

            //pretvaramo response u Xml
            xml = dbXmlConverter.ConvertToXml(response);
            //pretvaramo u Json
            json = jsonXmlConverter.ConvertFromXml(xml);

            return json;
        }   
    }
}
