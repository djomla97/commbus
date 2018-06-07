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

            Response response =  repository.DoQuery("SELECT * FROM ResourcesTable") as Response;

            Assert.AreEqual("nekiname", response.Payload.Resource.Name);
        }
    }
}
