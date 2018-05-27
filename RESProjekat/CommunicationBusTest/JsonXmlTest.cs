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

        private static string OcekivaniXml = "<request>    <verb> GET </verb>    <noun>/ resurs / 1 </noun>    <query> name = 'pera'; type=1</query>    <fields>id; name; surname</fields> </request>";
        private static string OcekivaniJson = "{     \"verb\": \"GET\",     \"noun\": \"/resurs/1\",     \"query\": \"name='pera'; type=1\",     \"fields\": \"id; name; surname\" }";

        [Test]
        [TestCase("<request>    <verb>GET</verb>    <noun>/resurs/1</noun>    <query>name='pera';type=1</query>    <fields>id; name; surname</fields> </request> ")]
        public void CovenrtFromXmlDobar(string xml)
        {
            JsonXmlConverter jsonXml = new JsonXmlConverter();
            string json = jsonXml.ConvertFromXml(xml);

            Assert.AreEqual(json.Trim(), OcekivaniJson.Trim());
        }


        [Test]
        [TestCase("{     \"verb\": \"GET\",     \"noun\": \"/resurs/1\",     \"query\": \"name='pera'; type=1\",     \"fields\": \"id; name; surname\" }")]
        public void CovenrtToXmlDobar(string json)
        {
            JsonXmlConverter xmlConverter = new JsonXmlConverter();
            string xml = xmlConverter.ConvertToXml(json);

            Assert.AreEqual(xml.Trim(), OcekivaniXml.Trim());
        }
    }
}
