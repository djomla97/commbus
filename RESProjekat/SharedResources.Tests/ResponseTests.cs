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
    public class ResponseTests
    {
        [Test]
        public void Response_EmptyConstructor_Good()
        {
            Response testResponse = new Response();
            Assert.NotNull(testResponse);
        }

        [Test]
        public void Response_EmptyConstructorPayloadNotNull_Good()
        {
            Response testResponse = new Response();
            testResponse.Payload = new Payload();
            Assert.NotNull(testResponse.Payload);
        }

        [Test]
        public void Response_EmptyConstructorStatusesNotNull_Good()
        {
            Response testResponse = new Response();
            testResponse.Status = Status.SUCCESS.ToString();
            testResponse.StatusCode = StatusCode.SUCCESS_CODE;

            Assert.NotNull(testResponse.Status);
            Assert.NotNull(testResponse.StatusCode);
        }

        [Test]
        public void Response_ConstructorParametersNotNull_Good()
        {
            Response testResponse = new Response(new Payload(), Status.BAD_FORMAT, StatusCode.BAD_FORMAT_CODE);

            Assert.NotNull(testResponse.Payload);
            Assert.NotNull(testResponse.Status);
            Assert.NotNull(testResponse.StatusCode);
        }

        [Test]
        public void Response_ConstructorPropertiesAreNull_Bad()
        {
            Response testResponse = new Response();

            Assert.IsNull(testResponse.Payload);
            Assert.IsNull(testResponse.Status);
        }

    }
}
