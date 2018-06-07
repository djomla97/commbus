using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedResources;
using NUnit.Framework;

namespace SharedResources.Tests
{
    public class PayloadTests
    {

        [Test]
        public void Payload_EmptyConstructor_Good()
        {
            Payload testPayload = new Payload();
            Assert.NotNull(testPayload);
        }

        [Test]
        public void Payload_EmptyConstructorErrorMessage_Good()
        {
            Payload testPayload = new Payload();
            testPayload.ErrorMessage = "Some test message";
            Assert.AreEqual("Some test message", testPayload.ErrorMessage);
        }

        [Test]
        public void Payload_EmptyConstructorResource_Good()
        {
            Payload testPayload = new Payload();
            testPayload.Resource = new Resource();
            Assert.NotNull(testPayload.Resource);
        }

        [Test]
        public void Payload_EmptyConstructorType_Good()
        {
            Payload testPayload = new Payload();
            testPayload.Resource = new Resource();
            testPayload.Resource.Type = new ResourceType();
            Assert.NotNull(testPayload.Resource.Type);
        }

        [Test]
        public void Payload_EmptyConstructorResourceProperties_Good()
        {
            Payload testPayload = new Payload();
            testPayload.Resource = new Resource();
            testPayload.Resource.ID = 2;
            testPayload.Resource.Title = "Man";
            testPayload.Resource.Name = "Pera";
            testPayload.Resource.Description = "Test person";
            testPayload.Resource.Type = new ResourceType();
            testPayload.Resource.Type.ID = 1;
            testPayload.Resource.Type.Title = "Human";

            Assert.AreEqual(2, testPayload.Resource.ID);
            Assert.AreEqual("Man", testPayload.Resource.Title);
            Assert.AreEqual("Pera", testPayload.Resource.Name);
            Assert.AreEqual("Test person", testPayload.Resource.Description);
            Assert.AreEqual(1, testPayload.Resource.Type.ID);
            Assert.AreEqual("Human", testPayload.Resource.Type.Title);
        }

        [Test]
        public void Payload_ResourceIsNull_Bad()
        {
            Payload testPayload = new Payload();
            Assert.IsNull(testPayload.Resource);
        }

        [Test]
        public void Payload_TypeIsNull_Bad()
        {
            Payload testPayload = new Payload();
            testPayload.Resource = new Resource();
            Assert.IsNull(testPayload.Resource.Type);
        }

        [Test]
        public void Payload_ConstructorParameters_Good()
        {
            Payload testPayload = new Payload(new Resource(), "");
            Assert.NotNull(testPayload.Resource);
            Assert.AreEqual("", testPayload.ErrorMessage);
        }

    }
}
