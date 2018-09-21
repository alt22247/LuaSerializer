using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.TypeTest
{
    [TestClass]
    public class TestBool : TestBase
    {
        [TestMethod]
        public void BoolBasic()
        {
            Assert.IsTrue(Serializer.Deserialize<bool>("true"));
            Assert.IsFalse(Serializer.Deserialize<bool>("false"));
        }

        [TestMethod]
        public void BoolNull()
        {
            Assert.IsFalse(Serializer.Deserialize<bool>(string.Empty));
        }

        [TestMethod]
        public void BoolFromString()
        {
            Assert.IsTrue(Serializer.Deserialize<bool>("\"true\""));
            Assert.IsFalse(Serializer.Deserialize<bool>("\"false\""));
        }

        [TestMethod]
        public void BoolNullable()
        {
            Assert.IsTrue((bool)Serializer.Deserialize<bool?>("\"true\""));
            Assert.IsFalse((bool)Serializer.Deserialize<bool?>("\"false\""));
            Assert.IsNull(Serializer.Deserialize<bool?>("nil"));
        }

        [TestMethod]
        public void BoolFromInt()
        {
            Assert.IsTrue(Serializer.Deserialize<bool>("1"));
            Assert.IsTrue(Serializer.Deserialize<bool>("2"));
            Assert.IsFalse(Serializer.Deserialize<bool>("0"));
        }

        [TestMethod]
        [ExpectedParseException(1, 5)]
        public void BoolInvalidTrue()
        {
            Serializer.Deserialize<bool>("truee");
        }

        [TestMethod]
        [ExpectedParseException(1, 3)]
        public void BoolInvalidTrue2()
        {
            Serializer.Deserialize<bool>("trxe");
        }

        [TestMethod]
        [ExpectedParseException(1, 6)]
        public void BoolInvalidFalse()
        {
            Serializer.Deserialize<bool>("falsee");
        }

        [TestMethod]
        [ExpectedParseException(1, 3)]
        public void BoolInvalidFalse2()
        {
            Serializer.Deserialize<bool>("faxse");
        }

        //------------------------Serialize--------------------------
        [TestMethod]
        public void SerializeBoolBasic()
        {
            Assert.AreEqual("true", Serializer.Serialize(true));
            Assert.AreEqual("false", Serializer.Serialize(false));
        }

        [TestMethod]
        public void SerializeBoolNullable()
        {
            bool? input = true;
            string result = Serializer.Serialize(input);
            Assert.AreEqual("true", result);

            input = false;
            result = Serializer.Serialize(input);
            Assert.AreEqual("false", result);

            input = null;
            result = Serializer.Serialize(input);
            Assert.IsNull(result);
        }
    }
}
