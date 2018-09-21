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
    public class TestDictionary : TestBase
    {
        [TestMethod]
        public void DictionaryBasic()
        {
            string content = GetLua();

            Dictionary<string, object> result = Serializer.Deserialize<Dictionary<string, object>>(content);
            Assert.AreEqual(0, result["0"]);
            Assert.AreEqual(1, result["1"]);
            Assert.AreEqual(2, result["2"]);
        }

        [TestMethod]
        public void DictionaryNull()
        {
            Dictionary<string, object> result = Serializer.Deserialize<Dictionary<string, object>>(string.Empty);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void DictionaryEmpty()
        {
            Dictionary<string, object> result = Serializer.Deserialize<Dictionary<string, object>>("{}");
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void DictionaryNullable()
        {
            Dictionary<int?, int?> result = Serializer.Deserialize<Dictionary<int?, int?>>("{[0] = 1}");
            Assert.AreEqual(1, result[0]);
        }

        [TestMethod]
        public void DictionaryNested()
        {
            string content = GetLua();
            Dictionary<string, object> result = Serializer.Deserialize<Dictionary<string, object>>(content);


            Dictionary<string, object> one = (Dictionary<string, object>)result["1"];
            Assert.AreEqual("1.1", one["1.1"]);
            Assert.AreEqual("1.2", one["1.2"]);
            Dictionary<string, object> oneThree = (Dictionary<string, object>)one["1.3"];
            Assert.AreEqual("1.3.1", oneThree["1.3.1"]);
            Assert.AreEqual("1.3.2", oneThree["1.3.2"]);
            Dictionary<string, object> oneThreeThree = (Dictionary<string, object>)oneThree["1.3.3"];
            Assert.AreEqual("1.3.3.1", oneThreeThree["1.3.3.1"]);

            Assert.AreEqual("1.4", one["1.4"]);


            Assert.AreEqual("2", result["2"]);
        }

        [TestMethod]
        public void DictionaryDifferentType()
        {
            string content = GetLua();
            Dictionary<object, object> result = Serializer.Deserialize<Dictionary<object, object>>(content);

            Assert.AreEqual(1, result["1"]);
            Assert.AreEqual("2", result["2"]);
            Assert.AreEqual(3.14, result[3]);
            Assert.AreNotEqual(null, result[4]);
            Assert.AreEqual(true, result[true]);
            Assert.AreEqual(false, result[false]);
        }

        [TestMethod]
        public void DictionaryStrongTyped()
        {
            string content = GetLua();
            Dictionary<int, int> result = Serializer.Deserialize<Dictionary<int, int>>(content);
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(2, result[2]);
            Assert.AreEqual(3, result[3]);
            Assert.AreEqual(4, result[4]);
        }

        [TestMethod]
        public void DictionaryDefaultType()
        {
            string content = "{}";
            object result = Serializer.Deserialize<object>(content);
            Assert.IsInstanceOfType(result, typeof(Dictionary<string, object>));
        }

        [TestMethod]
        [ExpectedParseException(2, 11)]
        public void DictionaryWrongKeyType()
        {
            string content = GetLua();
            Dictionary<int, int> result = Serializer.Deserialize<Dictionary<int, int>>(content);
        }

        [TestMethod]
        [ExpectedParseException(2, 20)]
        public void DictionaryWrongValueType()
        {
            string content = GetLua();
            Dictionary<string, int> result = Serializer.Deserialize<Dictionary<string, int>>(content);
        }

        [TestMethod]
        [ExpectedParseException(5, 1)]
        public void DictionaryNoCloseBracket()
        {
            string content = GetLua();
            var a = Serializer.Deserialize<Dictionary<string, int>>(content);
        }

        [TestMethod]
        [ExpectedParseException(4, 12)]
        public void DictionaryDuplicateKey()
        {
            string content = GetLua();
            var a = Serializer.Deserialize<Dictionary<string, int>>(content);
        }

        //------------------------Serialize--------------------------
        [TestMethod]
        public void SerializeDictionaryBasic()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict["0"] = 0;
            dict["1"] = 1;
            dict["2"] = 2;

            string result = Serializer.Serialize(dict);
            Assert.AreEqual("{[\"0\"]=0,[\"1\"]=1,[\"2\"]=2,}", result);
        }

        [TestMethod]
        public void SerializeDictionaryNullable()
        {
            Dictionary<int?, int?> dict = new Dictionary<int?, int?>();
            dict[1] = 1;
            dict[2] = null;
            dict[3] = 3;

            string result = Serializer.Serialize(dict);
            Assert.AreEqual("{[1]=1,[3]=3,}", result);
        }
    }
}
