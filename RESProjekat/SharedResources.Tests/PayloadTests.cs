using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedResources;
using NUnit.Framework;
using SharedResources.Interfaces;

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
            testPayload.Resource = new List<IResource>();
            Assert.NotNull(testPayload.Resource);
        }

        [Test]
        public void Payload_EmptyConstructorType_Good()
        {
            Payload testPayload = new Payload();
            testPayload.Resource = new List<IResource>();
            testPayload.Resource.Add(new Resource());
            testPayload.Resource[0].Type = new ResourceType();
            Assert.NotNull(testPayload.Resource[0].Type);
        }

        [Test]
        public void Payload_EmptyConstructorResourceProperties_Good()
        {
            Payload testPayload = new Payload();
            testPayload.Resource = new List<IResource>();
            testPayload.Resource.Add(new Resource());
            testPayload.Resource[0].ID = 2;
            testPayload.Resource[0].Title = "Man";
            testPayload.Resource[0].Name = "Pera";
            testPayload.Resource[0].Description = "Test person";
            testPayload.Resource[0].Type = new ResourceType();
            testPayload.Resource[0].Type.ID = 1;
            testPayload.Resource[0].Type.Title = "Human";

            Assert.AreEqual(2, testPayload.Resource[0].ID);
            Assert.AreEqual("Man", testPayload.Resource[0].Title);
            Assert.AreEqual("Pera", testPayload.Resource[0].Name);
            Assert.AreEqual("Test person", testPayload.Resource[0].Description);
            Assert.AreEqual(1, testPayload.Resource[0].Type.ID);
            Assert.AreEqual("Human", testPayload.Resource[0].Type.Title);
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
            testPayload.Resource = new List<IResource>();
            testPayload.Resource.Add(new Resource());
            Assert.IsNull(testPayload.Resource[0].Type);
        }

        [Test]
        public void Payload_ConstructorParameters_Good()
        {
            Payload testPayload = new Payload(new List<IResource>(), "");
            Assert.NotNull(testPayload.Resource);
            Assert.AreEqual("", testPayload.ErrorMessage);
        }

    }
}
