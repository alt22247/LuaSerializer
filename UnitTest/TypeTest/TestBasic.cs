using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lua;

namespace UnitTestProject1.TypeTest
{
    [TestClass]
    public class TestBasic : TestBase
    {
        [TestMethod]
        public void TabAndSpace()
        {
            int result = Serializer.Deserialize<int>("   \t \t 123 \t \t");
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void NewLine()
        {
            int result = Serializer.Deserialize<int>("   \n \n \n 123 \n");
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void EmptyObject()
        {
            object result = Serializer.Deserialize<object>("{}");
            Assert.AreNotEqual(null, result);
        }

        [TestMethod]
        public void Null()
        {
            object result = Serializer.Deserialize<object>("");
            Assert.AreEqual(null, result);

            result = Serializer.Deserialize<object>("   \t   \n    ");
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        [ExpectedParseException(1, 27)]
        public void InvalidTailingComma()
        {
            Serializer.Deserialize<int>("   \t \t 123 \t \t, ");
        }

        [TestMethod]
        [ExpectedParseException(1, 3)]
        public void InvalidLeadingComma()
        {
            Serializer.Deserialize<int>("  , \t \t 123 \t \t ");
        }

        [TestMethod]
        [ExpectedParseException(1, 1)]
        public void UnreconizedChar()
        {
            Serializer.Deserialize<string>("\a");
        }

        //------------------------Serialize--------------------------
        [TestMethod]
        public void SerializeSimpleString()
        {
            string result = Serializer.Serialize("string");
            Assert.AreEqual("\"string\"", result);
        }

        [TestMethod]
        public void SerializeNewLine()
        {
            string result = Serializer.Serialize("\n");
            Assert.AreEqual("\"\\n\"", result);
        }

        [TestMethod]
        public void SerializeQuote()
        {
            string result = Serializer.Serialize("\"");
            Assert.AreEqual("\"\\\"\"", result);
        }

        [TestMethod]
        public void SerializeNull()
        {
            string result = Serializer.Serialize(null);
            Assert.AreEqual(null, result);
        }
    }
}
