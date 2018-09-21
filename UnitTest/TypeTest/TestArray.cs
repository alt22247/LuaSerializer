using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.TypeTest
{
    [TestClass]
    public class TestArray : TestBase
    {
        [TestMethod]
        public void ArrayBasic()
        {
            string content = GetLua();

            int[] result = Serializer.Deserialize<int[]>(content);
            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(2, result[2]);
        }

        [TestMethod]
        public void ArrayNull()
        {
            int[] result = Serializer.Deserialize<int[]>(string.Empty);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void ArrayEmpty()
        {
            int[] result = Serializer.Deserialize<int[]>("{}");
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void ArrayZeroBased()
        {
            string content = GetLua();
            int[] result = Serializer.Deserialize<int[]>(content);
            Assert.AreEqual(6, result.Length);
            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(0, result[1]);
            Assert.AreEqual(1, result[2]);
            Assert.AreEqual(0, result[3]);
            Assert.AreEqual(0, result[4]);
            Assert.AreEqual(2, result[5]);
        }

        [TestMethod]
        public void ArrayOneBased()
        {
            string content = GetLua();
            int[] result = Serializer.Deserialize<int[]>(content);
            Assert.AreEqual(5, result.Length);
            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(0, result[2]);
            Assert.AreEqual(0, result[3]);
            Assert.AreEqual(2, result[4]);
        }

        [TestMethod]
        public void ArrayNullableType()
        {
            string content = GetLua();
            int?[] result = Serializer.Deserialize<int?[]>(content);
            Assert.AreEqual(5, result.Length);
            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(null, result[2]);
            Assert.AreEqual(null, result[3]);
            Assert.AreEqual(2, result[4]);
        }

        [TestMethod]
        [ExpectedParseException(3, 14)]
        public void ArrayInvalidComma()
        {
            string content = GetLua();
            Serializer.Deserialize<int[]>(content);
        }

        [TestMethod]
        [ExpectedParseException(3, 12)]
        public void ArrayInvalidType()
        {
            string content = GetLua();
            Serializer.Deserialize<int[]>(content);
        }

        //------------------------Serialize--------------------------
        [TestMethod]
        public void SerializeArrayBasic()
        {
            int[] array = new int[] { 0, 1, 2 };

            string result = Serializer.Serialize(array);
            Assert.AreEqual("{[1]=0,[2]=1,[3]=2,}", result);
        }

        [TestMethod]
        public void SerializeArrayEmpty()
        {
            int[] array = new int[0];
            string result = Serializer.Serialize(array);
            Assert.AreEqual("{}", result);
        }

        [TestMethod]
        public void SerializeArrayNullableType()
        {
            int?[] array = new int?[5];
            array[0] = 0;
            array[1] = 1;
            array[4] = 2;

            string result = Serializer.Serialize(array);
            Assert.AreEqual("{[1]=0,[2]=1,[5]=2,}", result);
        }
    }
}
