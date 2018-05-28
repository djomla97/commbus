using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharedResources;
using CommunicationBusLib;

namespace CommunicationBusTest
{
    [TestFixture]
    public class DbXmlConverterTest
    {
        // klase
        private static DbXmlConverter dbXmlConverter = null;


        // test get
        private string testXmlOne = string.Empty;
        private string testXmlTwo = string.Empty;
        private string testXmlThree = string.Empty;
        private string testXmlFour = string.Empty;
        private string testXmlBreak = string.Empty;
        private Response testResponse = null;

        // test post
        private string testXmlPostOne = string.Empty;

        //test patch
        private string testXmlPatchOne = string.Empty;

        // test delete
        private string testXmlDeleteOne = string.Empty;
        private string testXmlDeleteTwo = string.Empty;

        // ---------------------------------------------------
        // ocekivani get
        private string expectedDbFromXmlOne = string.Empty;
        private string expectedDbFromXmlTwo = string.Empty;
        private string expectedDbFromXmlThree = string.Empty;
        private string expectedDbFromXmlFour = string.Empty;
        private string expectedXmlFromResponse = string.Empty;
        private string expectedXmlBreak = string.Empty;

        // ocekivani post
        private string expectedDbFromPostOne = string.Empty;

        //ocekivani patch
        private string expectedDbFromPatchOne = string.Empty;

        // ocekivani delete
        private string expectedDbFromDeleteOne = string.Empty;
        private string expectedDbFromDeleteTwo = string.Empty;


        [SetUp]
        public void SetUp()
        {
            // klase
            dbXmlConverter = new DbXmlConverter();

            // test get
            testXmlOne = "<request>\r\n  <verb>GET</verb>\r\n  <noun>/resource</noun>\r\n  <query>name='pera';type=1</query>\r\n  <fields>id;name;surname</fields>\r\n</request>";
            testXmlTwo = "<request>\r\n  <verb>GET</verb>\r\n  <noun>/resource/2</noun>\r\n   <fields>id;name</fields>\r\n</request>";
            testXmlThree = "<request>\r\n  <verb>GET</verb>\r\n  <noun>/resource/3</noun>\r\n</request>";
            testXmlFour = "<request>\r\n  <verb>GET</verb>\r\n  <noun>/resource</noun>\r\n  <query>name='nesto';type=2</query>\r\n</request>";
            testXmlBreak = "<request>r\n  <verb>BREAK</verb>\r\n  <noun>/resource/2</noun>\r\n</request>";

            // test post
            testXmlPostOne = "<request>\r\n  <verb>POST</verb>\r\n  <noun>/resource</noun>\r\n  <query>name='pera';type=1</query>\r\n</request>";

            // test patch
            testXmlPatchOne = "<request>\r\n  <verb>PATCH</verb>\r\n  <noun>/resource/3</noun>\r\n  <query>name='mladjo';type=2</query>\r\n</request>";

            // test delete
            testXmlDeleteOne = "<request>\r\n  <verb>DELETE</verb>\r\n  <noun>/resource/3</noun>\r\n</request>";
            testXmlDeleteTwo = "<request>\r\n  <verb>DELETE</verb>\r\n  <noun>/resource</noun>\r\n  <query>name='pera';type=2</query>\r\n</request>";

            // test response
            testResponse = new Response() { Status = Status.SUCCESS.ToString(), StatusCode = StatusCode.SUCCESS_CODE };
            testResponse.Payload = new Payload();
            testResponse.Payload.Resource = new Resource() { ID = 1, Title = "Osoba", Name = "Pera", Description = "Test osoba", Type = new ResourceType() { ID = 1, Title = "Osoba Tip" } };


            // ----------------------------------------------------
            // ocekivani
            expectedDbFromXmlOne = "SELECT id,name,surname FROM resource WHERE name='pera' AND type=1;";
            expectedDbFromXmlTwo = "SELECT id,name FROM resource WHERE id=2;";
            expectedDbFromXmlThree = "SELECT * FROM resource WHERE id=3;";
            expectedDbFromXmlFour = "SELECT * FROM resource WHERE name='nesto' AND type=2;";

            // ocekivani post
            expectedDbFromPostOne = "INSERT INTO resource (name, type) VALUES ('pera', 1);";

            // ocekivani patch
            expectedDbFromPatchOne = "UPDATE resource SET name='mladjo', type=2 WHERE id=3;";

            // ocekivani delete
            expectedDbFromDeleteOne = "DELETE FROM resource WHERE id=3;";
            expectedDbFromDeleteTwo = "DELETE FROM resource WHERE name='pera' AND type=2;";

            // ocekivani response
            expectedXmlFromResponse = "<Response>\r\n  <Payload>\r\n    <Resource>\r\n      <ID>1</ID>\r\n      <Title>Osoba</Title>\r\n      <Name>Pera</Name>\r\n      <Description>Test osoba</Description>\r\n      <Type>\r\n        <ID>1</ID>\r\n        <Title>Osoba Tip</Title>\r\n      </Type>\r\n    </Resource>\r\n    <ErrorMessage />\r\n  </Payload>\r\n  <Status>SUCCESS</Status>\r\n  <StatusCode>2000</StatusCode>\r\n</Response>";

        }

        // GET testovi

        [Test]
        public void Xml_To_Db_Convert_Get_Good_One()
        {
            string result = dbXmlConverter.ConvertFromXml(testXmlOne);
            Assert.AreEqual(expectedDbFromXmlOne, result);
        }

        [Test]
        public void Xml_To_Db_Convert_Get_Good_Two()
        {
            string result = dbXmlConverter.ConvertFromXml(testXmlTwo);
            Assert.AreEqual(expectedDbFromXmlTwo, result);
        }

        [Test]
        public void Xml_To_Db_Convert_Get_Good_Three()
        {
            string result = dbXmlConverter.ConvertFromXml(testXmlThree);
            Assert.AreEqual(expectedDbFromXmlThree, result);
        }

        [Test]
        public void Xml_To_Db_Convert_Get_Good_Four()
        {
            string result = dbXmlConverter.ConvertFromXml(testXmlFour);
            Assert.AreEqual(expectedDbFromXmlFour, result);
        }

        [Test]
        public void Xml_To_Db_Convert_Break()
        {
            string result = dbXmlConverter.ConvertFromXml(testXmlBreak);
            Assert.AreEqual(expectedXmlBreak, result);
        }

        // POST testovi
        [Test]
        public void Xml_To_Db_Convert_Post_Good_One()
        {
            string result = dbXmlConverter.ConvertFromXml(testXmlPostOne);
            Assert.AreEqual(expectedDbFromPostOne, result);
        }

        // PATCH testovi
        [Test]
        public void Xml_To_Db_Convert_Patch_Good_One()
        {
            string result = dbXmlConverter.ConvertFromXml(testXmlPatchOne);
            Assert.AreEqual(expectedDbFromPatchOne, result);
        }

        // DELETE testovi
        [Test]
        public void Xml_To_Db_Convert_Delete_Good_One()
        {
            string result = dbXmlConverter.ConvertFromXml(testXmlDeleteOne);
            Assert.AreEqual(expectedDbFromDeleteOne, result);
        }

        [Test]
        public void Xml_To_Db_Convert_Delete_Good_Two()
        {
            string result = dbXmlConverter.ConvertFromXml(testXmlDeleteTwo);
            Assert.AreEqual(expectedDbFromDeleteTwo, result);
        }


        // JSON to XML test
        [Test]
        public void Response_To_Xml_Good()
        {
            string result = dbXmlConverter.ConvertToXml(testResponse);
            Assert.AreEqual(expectedXmlFromResponse, result);
        }

        [TearDown]
        public void TearDown()
        {
            // klase
            dbXmlConverter = null;

            // test get
            testXmlOne = string.Empty;
            testXmlTwo = string.Empty;
            testXmlThree = string.Empty;
            testXmlFour = string.Empty;
            testXmlBreak = string.Empty;
            testResponse = null;

            // test post
            testXmlPostOne = string.Empty;

            // ocekivani get
            expectedDbFromXmlOne = string.Empty;
            expectedDbFromXmlTwo = string.Empty;
            expectedDbFromXmlThree = string.Empty;
            expectedDbFromXmlFour = string.Empty;
            expectedXmlBreak = string.Empty;
            expectedXmlFromResponse = string.Empty;


            // ocekivani post
            expectedDbFromPostOne = string.Empty;

            // ocekivani patch
            expectedDbFromPatchOne = string.Empty;

            // ocekivani delete
            expectedDbFromDeleteOne = string.Empty;
            expectedDbFromDeleteTwo = string.Empty;
    }
    }
}
