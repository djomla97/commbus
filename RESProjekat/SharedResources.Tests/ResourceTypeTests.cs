using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharedResources.Tests
{
    [TestFixture]
    public class ResourceTypeTests
    {
        [Test]
        public void ResourceType_Constructor_Good()
        {
            ResourceType testResourceType = new ResourceType();
            Assert.NotNull(testResourceType);
        }

        [Test]
        public void ResourceType_Declare_Null()
        {
            ResourceType testResourceType = null;
            Assert.IsNull(testResourceType);
        }

        [Test]
        public void ResourceType_Properties_Null()
        {
            ResourceType testResourceType = new ResourceType();

            Assert.IsNull(testResourceType.Title);
            Assert.AreEqual(0 ,testResourceType.ID);
        }

        [Test]
        public void ResourceType_Properties_NotNull()
        {
            ResourceType testResourceType = new ResourceType() { ID = 1, Title = "nesto" };
            Assert.NotNull(testResourceType);
            Assert.NotNull(testResourceType.ID);
            Assert.NotNull(testResourceType.Title);
        }

        [Test]
        public void ResourceType_Properties_Good()
        {
            ResourceType testResourceType = new ResourceType() { ID = 2, Title="test"};
            Assert.AreEqual(2, testResourceType.ID);
            Assert.AreEqual("test", testResourceType.Title);
        }

    }
}
