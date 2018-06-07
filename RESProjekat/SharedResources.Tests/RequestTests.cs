using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedResources;
using NUnit.Framework;

namespace SharedResources.Tests
{
    [TestFixture]
    public class RequestTests
    {
        [Test]
        public void Request_EmptyConstructor_Good()
        {
            Request testRequest = new Request();
            Assert.NotNull(testRequest);
        }

        [Test]
        public void Request_EmptyConstructorProperties_Good()
        {
            Request testRequest = new Request();
            testRequest.Verb = "GET";
            testRequest.Noun = "/resource";
            testRequest.Query = "name='pera';type=2";
            testRequest.Fields = "id;name;description";

            Assert.AreEqual("GET", testRequest.Verb);
            Assert.AreEqual("/resource", testRequest.Noun);
            Assert.AreEqual("name='pera';type=2", testRequest.Query);
            Assert.AreEqual("id;name;description", testRequest.Fields);
        }

        [Test]
        public void Request_ConstructorParameters_Good()
        {
            Request testRequest = new Request("GET", "/resource", "name='pera';type=2", "id;name;description");

            Assert.AreEqual("GET", testRequest.Verb);
            Assert.AreEqual("/resource", testRequest.Noun);
            Assert.AreEqual("name='pera';type=2", testRequest.Query);
            Assert.AreEqual("id;name;description", testRequest.Fields);
        }

        [Test]
        public void Request_PropertiesAreNull_Good()
        {
            Request testRequest = new Request();
            Assert.IsNull(testRequest.Verb);
            Assert.IsNull(testRequest.Noun);
            Assert.IsNull(testRequest.Query);
            Assert.IsNull(testRequest.Fields);
        }

    }
}
