using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RepositoryLib;
using SharedResources;
using SharedResources.Interfaces;

namespace RepositoryTest
{
    [TestFixture]
    public class RepositoryTests
    {

        [Test]
        public void TestSelect()
        {
            Repository repository = new Repository();

            IResponse response = repository.DoQuery("SELECT * FROM resource");

            Assert.AreEqual("Pera", response.Payload.Resource[0].Name);
            Assert.AreEqual("Snoopy", response.Payload.Resource[1].Name);
        }


        [Test]
        public void TestInsert()
        {
            Repository repository = new Repository();

            IResponse response = repository.DoQuery("INSERT INTO resource (name, type) VALUES ('Biba', 1);");

            Assert.AreEqual("Biba", response.Payload.Resource[0].Name);

        }

        [Test]
        public void TestUpdate()
        {
            Repository repository = new Repository();

            IResponse response = repository.DoQuery("UPDATE resource SET name='mladjo', type=2 WHERE id=2;");

            Assert.AreEqual("mladjo", response.Payload.Resource[0].Name);

        }

        [Test]
        public void TestDelete()
        {
            Repository repository = new Repository();

            IResponse response = repository.DoQuery("DELETE FROM resource WHERE id=14;");

            Assert.AreEqual("SUCCESS", response.Status);
            Assert.AreEqual(StatusCode.SUCCESS_CODE, response.StatusCode);

        }
    }
}
