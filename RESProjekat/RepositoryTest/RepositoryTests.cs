using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RepositoryLib;
using SharedResources;

namespace RepositoryTest
{
    [TestFixture]
    public class RepositoryTests
    {

        [Test]
        public void Test1()
        {
            Repository repository = new Repository();

            List<Response> response =  repository.DoQuery("SELECT * FROM resource");

            Assert.AreEqual("Pera",response[0].Payload.Resource.Name);
            Assert.AreEqual("Djoka", response[1].Payload.Resource.Name);
        }
    }
}
