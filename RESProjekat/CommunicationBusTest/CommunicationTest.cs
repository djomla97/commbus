using CommunicationBusLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationBusTest
{
    [TestFixture]
    public class CommunicationTest
    {
        //private static string Json = "{\"verb\":\"GET\",\"noun\":\"/resurs/1\",\"query\":\"name='pera';type=1\",\"fields\":\"id;name;surname\"}";

        [Test]
        public void DobarKonstruktor()
        {
            CommunicationBus communicationBus = new CommunicationBus();
            Assert.IsNotNull(communicationBus);
        }
    }
}
