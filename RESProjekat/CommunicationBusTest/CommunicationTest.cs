using CommunicationBusLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using RepositoryLib;
using SharedResources.Interfaces;
using SharedResources;
using Newtonsoft.Json;

namespace CommunicationBusTest
{
    [TestFixture]
    public class CommunicationTest
    {
        private IResponse testResponse;
        private Mock<IRepository> repoMock;
        private DbXmlConverter dbXmlTest = new DbXmlConverter();
        private JsonXmlConverter jsonXmlTest = new JsonXmlConverter();

        [SetUp]
        public void SetUp()
        {
            testResponse = new Response(new Payload(), Status.SUCCESS, StatusCode.SUCCESS_CODE);
            testResponse.Payload.Resource = new List<IResource>();
            testResponse.Payload.Resource.Add(new Resource()
                { ID = 1,
                  Name = "TestName",
                  Title = "TestTitle",
                  Description = "test opis",
                  Type = new ResourceType() { ID = 1, Title = "TestType" }
            });
            testResponse.Payload.ErrorMessage = "";

            repoMock = new Mock<IRepository>();
            repoMock.Setup(f => f.DoQuery(It.IsAny<string>())).Returns(testResponse);
        }


        [Test]
        public void DobarKonstruktor()
        {          
            CommunicationBus communicationBus = new CommunicationBus(repoMock.Object);
            Assert.IsNotNull(communicationBus);
        }

        [Test]
        public void Test_SendCommand()
        {
            CommunicationBus communicationBus = new CommunicationBus(repoMock.Object);
            Request reqTest = new Request("GET", "/resource", "id=2", "name;title");

            string jsonFormat = JsonConvert.SerializeObject(reqTest);

            string sendResponse = communicationBus.SendCommand(jsonFormat);

            string xmlTest = dbXmlTest.ConvertToXml(testResponse);
            string jsonTest = jsonXmlTest.ConvertFromXml(xmlTest);

            Assert.AreEqual(jsonTest, sendResponse);

        }
    }
}
