using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RepositoryLib;
using SharedResources;
using SharedResources.Interfaces;

namespace RepositoryTest
{
    [TestFixture]
    public class RepositoryTests
    {
        private IRepository testRepo = null;

        [SetUp]
        public void SetUp()
        {
            testRepo = new Repository();
        }

        [TearDown]
        public void TearDown()
        {
            testRepo = null;
        }

        [Test]
        [TestCase("SELECT * FROM resource;")]
        public void Repo_SelectAll_Good(string query)
        {
            IResponse response = testRepo.DoQuery(query);

            Assert.AreEqual("SUCCESS", response.Status);
            Assert.AreEqual(StatusCode.SUCCESS_CODE, response.StatusCode);
            Assert.AreEqual("", response.Payload.ErrorMessage);
            Assert.AreEqual(4, response.Payload.Resource.Count); // ovde treba paziti
        }

        [Test]
        [TestCase("SELECT * FROM resource WHERE id=1;")]
        [TestCase("SELECT * FROM resource WHERE id=3;")]
        [TestCase("SELECT * FROM resource WHERE id=2;")]
        [TestCase("SELECT * FROM resource WHERE connectedTo=2;")]
        [TestCase("SELECT * FROM resource WHERE connectedType=5;")]
        [TestCase("SELECT * FROM resource WHERE type=1;")]
        [TestCase("SELECT id, type FROM resource WHERE type=2;")]
        [TestCase("SELECT id, name FROM resource WHERE id=4;")]
        [TestCase("SELECT id, connectedTo FROM resource WHERE connectedType=5;")]
        [TestCase("SELECT id, connectedType FROM resource WHERE connectedTo=2;")]
        public void Repo_SelectOne_Good(string query)
        {
            IResponse response = testRepo.DoQuery(query);

            Assert.AreEqual("SUCCESS", response.Status);
            Assert.AreEqual(StatusCode.SUCCESS_CODE, response.StatusCode);
            Assert.AreEqual("", response.Payload.ErrorMessage);
            Assert.AreEqual(1, response.Payload.Resource.Count);
        }

        [Test]
        [TestCase("SELECT * FROM resource WHERE id=105;")]
        [TestCase("SELECT * FROM resource WHERE type=23;")]
        [TestCase("SELECT * FROM notable WHERE id=2;")]
        [TestCase("SELECT doesntexist FROM resource WHERE type=1;")]
        public void Repo_Select_Bad(string query)
        {
            IResponse response = testRepo.DoQuery(query);

            Assert.AreEqual("REJECTED", response.Status);
            Assert.AreEqual(StatusCode.REJECTED_CODE, response.StatusCode);
        }


        [Test]
        [TestCase("INSERT INTO resource (name, title) VALUES ('mladjo', 'spawn');")]
        [TestCase("INSERT INTO resource (name, title, description) VALUES ('Something', 'Somewhat', 'test desc');")]
        [TestCase("INSERT INTO resource (name, connectedTo, type) VALUES ('MrRobot', 2, 1);")]
        [TestCase("INSERT INTO connection (one, two) VALUES (2, 3);")]
        [TestCase("INSERT INTO connection (one, two) VALUES (1, 4);")]
        public void Repo_Insert_Good(string query)
        {
            IResponse response = testRepo.DoQuery(query);

            Assert.AreEqual("SUCCESS", response.Status);
            Assert.AreEqual(StatusCode.SUCCESS_CODE, response.StatusCode);
            Assert.NotNull(response.Payload);
            Assert.AreEqual("", response.Payload.ErrorMessage);
            Assert.AreEqual(1, response.Payload.Resource.Count);
        }

        [Test]
        [TestCase("INSERT INTO notable (name, title) VALUES ('mladjo', 'spawn');")]
        [TestCase("INSERT INTO person (name, title, description) VALUES ('Something', 'Somewhat', 'test desc');")]
        [TestCase("INSERT INTO resource (car, connectedTo, type) VALUES ('mercedes', 2, 1);")]
        [TestCase("INSERT INTO resource (name, connectedTo, type) VALUES (655, 2, 105);")]
        public void Repo_Insert_Bad(string query)
        {
            IResponse response = testRepo.DoQuery(query);

            Assert.AreEqual("REJECTED", response.Status);
            Assert.AreEqual(StatusCode.REJECTED_CODE, response.StatusCode);           
        }

        [Test]
        [TestCase("UPDATE resource SET name='Senu' WHERE id=2;")]
        [TestCase("UPDATE resource SET name='Phoenix', type=2 WHERE id=2;")]
        [TestCase("UPDATE resource SET description='test description' WHERE id=2;")]
        [TestCase("UPDATE resource SET title='King', connectedTo=2, connectedType=1 WHERE id=2;")]
        public void Repo_Update_Good(string query)
        {
            IResponse response = testRepo.DoQuery(query);

            Assert.AreEqual("SUCCESS", response.Status);
            Assert.AreEqual(StatusCode.SUCCESS_CODE, response.StatusCode);
            Assert.NotNull(response.Payload);
            Assert.AreEqual("", response.Payload.ErrorMessage);
            Assert.AreEqual(1, response.Payload.Resource.Count);
        }

        [Test]
        [TestCase("UPDATE notable SET name='Senu' WHERE id=2;")]
        [TestCase("UPDATE resource SET some='Phoenix', type=2 WHERE id=2;")]
        [TestCase("UPDATE resource SET description='test description' WHERE id=88;")]
        public void Repo_Update_Bad(string query)
        {
            IResponse response = testRepo.DoQuery(query);

            Assert.AreEqual("REJECTED", response.Status);
            Assert.AreEqual(StatusCode.REJECTED_CODE, response.StatusCode);
        }

        [Test]
        [TestCase("DELETE FROM resource WHERE id=1;")]
        [TestCase("DELETE FROM resource WHERE id=2;")]
        [TestCase("DELETE FROM resource WHERE id=3;")]
        public void Repo_Delete_Good(string query)
        {
            IResponse response = testRepo.DoQuery(query);

            Assert.AreEqual("SUCCESS", response.Status);
            Assert.AreEqual(StatusCode.SUCCESS_CODE, response.StatusCode);
        }

        [Test]
        [TestCase("DELETE FROM notable WHERE id=1;")]
        [TestCase("DELETE FROM resource WHERE column=22;")]
        public void Repo_Delete_Bad(string query)
        {
            IResponse response = testRepo.DoQuery(query);

            Assert.AreEqual("REJECTED", response.Status);
            Assert.AreEqual(StatusCode.REJECTED_CODE, response.StatusCode);
        }


    }
}
