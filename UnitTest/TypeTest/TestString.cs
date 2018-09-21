using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.TypeTest
{
    [TestClass]
    public class TestString : TestBase
    {
        [TestMethod]
        public void StringBasic()
        {
            string result = Serializer.Deserialize<string>("\"some random string\"");
            Assert.AreEqual("some random string", result);
        }

        [TestMethod]
        public void StringNull()
        {
            string result = Serializer.Deserialize<string>(string.Empty);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void StringWithQuote()
        {
            string result = Serializer.Deserialize<string>("\"String \\\"With\\\" Quote\"");
            Assert.AreEqual("String \"With\" Quote", result);
        }

        [TestMethod]
        public void StringFromInt()
        {
            string result = Serializer.Deserialize<string>("12345");
            Assert.AreEqual("12345", result);
        }

        [TestMethod]
        public void StringNewLine()
        {
            string result = Serializer.Deserialize<string>("\"\\r\"");
            Assert.AreEqual("\r", result);
        }

        [TestMethod]
        public void StringNewTab()
        {
            string result = Serializer.Deserialize<string>("\"\\t\\t\"");
            Assert.AreEqual("\t\t", result);
        }

        [TestMethod]
        public void StringSingleQuote()
        {
            string result = Serializer.Deserialize<string>("'\"'");
            Assert.AreEqual("\"", result);
        }

        [TestMethod]
        public void StringNestedQuote()
        {
            string result = Serializer.Deserialize<string>("'123\"\\'\\'\"123'");
            Assert.AreEqual("123\"''\"123", result);
        }

        [TestMethod]
        public void StringEscapeBeforeQuote()
        {
            string result = Serializer.Deserialize<string>("\"123\\\\\"");
            Assert.AreEqual("123\\", result);
        }

        [TestMethod]
        public void StringAsciiEscapeSequence()
        {
            string result = Serializer.Deserialize<string>("\"\\9A\"");
            Assert.AreEqual("\tA", result);

            result = Serializer.Deserialize<string>("\"\\10A\"");
            Assert.AreEqual("\nA", result);

            result = Serializer.Deserialize<string>("\"\\049A\"");
            Assert.AreEqual("1A", result);
        }

        [TestMethod]
        public void StringCEscapeChars()
        {
            string result = Serializer.Deserialize<string>(@"""\a\b\f\n\r\t\v\\\""\'\[\]""");
            Assert.AreEqual("\a\b\f\n\r\t\v\\\"'[]", result);
        }

        [TestMethod]
        [ExpectedParseException(1, 4)]
        public void StringExtraQuote()
        {
            Serializer.Deserialize<string>("\" \"\"");
        }

        [TestMethod]
        [ExpectedParseException(1, 6)]
        public void StringInvalidEndingQuote()
        {
            Serializer.Deserialize<string>("\"123'");
        }

        [TestMethod]
        [ExpectedParseException(1, 7)]
        public void StringInvalidNested()
        {
            Serializer.Deserialize<string>("\"'123\"'");
        }

        //------------------------Serialize--------------------------
        [TestMethod]
        public void SerializeStringBasic()
        {
            string result = Serializer.Serialize("some random string");
            Assert.AreEqual("\"some random string\"", result);
        }

        [TestMethod]
        public void SerializeStringEmpty()
        {
            string result = Serializer.Serialize(string.Empty);
            Assert.AreEqual("\"\"", result);
        }

        [TestMethod]
        public void SerializeStringWithQuote()
        {
            string result = Serializer.Serialize("String \"With\" Quote");
            Assert.AreEqual("\"String \\\"With\\\" Quote\"", result);
        }

        [TestMethod]
        public void SerializeStringNewLine()
        {
            string result = Serializer.Serialize("\r");
            Assert.AreEqual("\"\\r\"", result);
        }

        [TestMethod]
        public void SerializeStringNewTab()
        {
            string result = Serializer.Serialize("\t\t");
            Assert.AreEqual("\"\\t\\t\"", result);
        }

        [TestMethod]
        public void SerializeStringSingleQuote()
        {
            string result = Serializer.Serialize("''");
            Assert.AreEqual("\"''\"", result);
        }

        [TestMethod]
        public void SerializeStringEscapeBeforeQuote()
        {
            string result = Serializer.Serialize("123\\");
            Assert.AreEqual("\"123\\\\\"", result);
        }

        [TestMethod]
        public void SerializeStringCEscapeChars()
        {
            string result = Serializer.Serialize("\a\b\f\n\r\t\v\\\"");
            Assert.AreEqual(@"""\a\b\f\n\r\t\v\\\""""", result);
        }
    }
}
