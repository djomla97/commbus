using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharedResources.Tests
{
    [TestFixture]
    public class ResourceTests
    {
        [Test]
        public void Resource_Constructor_Good()
        {
            Resource testResource = new Resource();
            Assert.IsNotNull(testResource);
        }

        [Test]
        public void Resource_Declare_Null() {
            Resource testResource = null;
            Assert.IsNull(testResource);
        }

        [Test]
        public void Resource_ConstructorWithType_Good()
        {
            Resource testResource = new Resource(new ResourceType());
            Assert.NotNull(testResource.Type);
        }

        [Test]
        public void Resource_PropertyInit_NotNull()
        {
            Resource testResource = new Resource();
            testResource.ID = 1;
            testResource.Name = "pera";
            testResource.Title = "osoba";
            testResource.Type = new ResourceType();
            testResource.Description = "something";

            Assert.NotNull(testResource);
            Assert.NotNull(testResource.ID);
            Assert.NotNull(testResource.Name);
            Assert.NotNull(testResource.Title);
            Assert.NotNull(testResource.Type);
            Assert.NotNull(testResource.Description);
        }

        [Test]
        public void Resource_PropertyNotInit_Null()
        {
            Resource testResource = new Resource();

            Assert.NotNull(testResource);
            Assert.AreEqual(0, testResource.ID);
            Assert.IsNull(testResource.Name);
            Assert.IsNull(testResource.Title);
            Assert.IsNull(testResource.Description);
        }

        [Test]
        public void Resource_PropertyInit_Good()
        {
            Resource testResource = new Resource();
            testResource.ID = 1;
            testResource.Name = "pera";
            testResource.Title = "osoba";
            testResource.Type = new ResourceType();
            testResource.Type.ID = 1;
            testResource.Type.Title = "human";
            testResource.Description = "something";

            Assert.AreEqual(1, testResource.ID);
            Assert.AreEqual("pera", testResource.Name);
            Assert.AreEqual("osoba", testResource.Title);
            Assert.AreEqual(1, testResource.Type.ID);
            Assert.AreEqual("human", testResource.Type.Title);
            Assert.AreEqual("something", testResource.Description);
        }

    }
}
