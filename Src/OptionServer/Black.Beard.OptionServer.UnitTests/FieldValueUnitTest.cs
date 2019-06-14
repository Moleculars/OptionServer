using Bb;
using Bb.OptionServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;
using System.Linq;

namespace Black.Beard.OptionServer.UnitTests
{

    [TestClass]
    public class FieldValueUnitTest
    {


        [TestMethod]
        public void TestField1()
        {

            FieldValue<Guid> v1 = new FieldValue<Guid>() { };

            Assert.AreEqual(v1.Equals(Guid.Empty), true);
            Assert.AreEqual(v1.Equals(Guid.NewGuid()), false);

            v1 = new FieldValue<Guid>() { Value = Guid.NewGuid() };

            Assert.AreEqual(v1.Equals(v1.Value), true);
            Assert.AreEqual(v1.Equals(Guid.NewGuid()), false);

        }

        [TestMethod]
        public void TestField2()
        {

            FieldValue<string> v1 = new FieldValue<string>() { };

            Assert.AreEqual(v1.Equals(string.Empty), false);
            Assert.AreEqual(v1.Equals(Guid.NewGuid()), false);
            Assert.AreEqual(v1.Equals(null), true);

            v1 = new FieldValue<string>() { Value = "test" };
            Assert.AreEqual(v1.Equals(string.Empty), false);
            Assert.AreEqual(v1.Equals(Guid.NewGuid()), false);
            Assert.AreEqual(v1.Equals("test"), true);

        }



    }

}
