using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.TypeTest
{
    [TestClass]
    public class TestNil : TestBase
    {
        [TestMethod]
        public void NilBasic()
        {
            object result = Serializer.Deserialize<object>("nil");
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        [ExpectedParseException(1, 2)]
        public void NilUpper()
        {
            Serializer.Deserialize<object>("NIL");
        }

        [TestMethod]
        public void NilInDict()
        {
            string content = @"
                             {
	                             [0] = 0,
	                             [1] = nil,
	                             [2] = 2
                             }";

            Dictionary<int, int?> result = Serializer.Deserialize<Dictionary<int, int?>>(content);
            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(null, result[1]);
            Assert.AreEqual(2, result[2]);
        }

        [TestMethod]
        public void NilNullable()
        {
            string content = "nil";
            int? result = Serializer.Deserialize<int?>(content);
            Assert.AreEqual(null, result);
        }

        //------------------------Serialize--------------------------
        [TestMethod]
        public void SerializeNil()
        {
            string result = Serializer.Serialize(new { Test = (object) null });
            Assert.AreEqual("{}", result);
        }

        [TestMethod]
        public void SerializeNilNullable()
        {
            string result = Serializer.Serialize(new { Test = (int?)null });
            Assert.AreEqual("{}", result);
        }
    }
}
