using CommunicationBusLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationBusTest
{
    [TestFixture]
    public class JsonXmlTest
    {

        private static string OcekivaniXml = "<request>\r\n  <verb>GET</verb>\r\n  <noun>/resurs/1</noun>\r\n  <query>name='pera';type=1</query>\r\n  <fields>id;name;surname</fields>\r\n</request>";
        private static string OcekivaniJson = "{\"request\":{\"verb\":\"GET\",\"noun\":\"/resurs/1\",\"query\":\"name='pera';type=1\",\"fields\":\"id;name;surname\"}}";

        private static string OcekivaniXml2 = "<request>\r\n  <verb>POST</verb>\r\n  <noun>/resurs</noun>\r\n  <query>name='pera';type=1</query>\r\n</request>";


        [Test]
        [TestCase("<request><verb>GET</verb><noun>/resurs/1</noun><query>name='pera';type=1</query><fields>id;name;surname</fields></request>")]
        public void ConvertFromXmlDobar(string xml)
        {
            JsonXmlConverter jsonXml = new JsonXmlConverter();
            string json = jsonXml.ConvertFromXml(xml);

            Assert.AreEqual(json, OcekivaniJson);
        }

        //dovrsiti
        [Test]
        [TestCase("{\"verb\":\"GET\",\"noun\":\"/resurs/1\",\"query\":\"name='pera';type=1\",\"fields\":\"id;name;surname\"}")]
        public void ConvertToXmlDobar(string json)
        {
            JsonXmlConverter xmlConverter = new JsonXmlConverter();
            string xml = xmlConverter.ConvertToXml(json);

            Assert.AreEqual(xml, OcekivaniXml);
        }


        [Test]
        [TestCase("{\"verb\":\"POST\",\"noun\":\"/resurs\",\"query\":\"name='pera';type=1\"}")]
        public void ConverttToXmlDobar2(string json)
        {
            JsonXmlConverter xmlConverter = new JsonXmlConverter();
            string xml = xmlConverter.ConvertToXml(json);

            Assert.AreEqual(xml, OcekivaniXml2);
        }
    }
}