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
    public class TestNumber : TestBase
    {
        [TestMethod]
        public void NumberBasic()
        {
            int result = Serializer.Deserialize<int>("123");
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void NumberNull()
        {
            int result = Serializer.Deserialize<int>(string.Empty);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void NumberZero()
        {
            int result = Serializer.Deserialize<int>("0");
            Assert.AreEqual(0, result);

            result = Serializer.Deserialize<int>("-0");
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void NumberNegative()
        {
            int result = Serializer.Deserialize<int>("-123");
            Assert.AreEqual(-123, result);
        }

        [TestMethod]
        public void NumberFloatAsInt()
        {
            int result = Serializer.Deserialize<int>("12.456");
            Assert.AreEqual(12, result);
        }

        [TestMethod]
        public void NumberFloat()
        {
            float result = Serializer.Deserialize<float>("12.567");
            Assert.AreEqual(12.567f, result);
        }

        [TestMethod]
        public void NumberMax()
        {
            int resultInt = Serializer.Deserialize<int>(int.MaxValue.ToString());
            Assert.AreEqual(int.MaxValue, resultInt);

            float resultFloat = Serializer.Deserialize<float>("3.40282347E+38");
            Assert.AreEqual(float.MaxValue, resultFloat);

            double resultDouble = Serializer.Deserialize<double>("1.7976931348623157E+308");
            Assert.AreEqual(double.MaxValue, resultDouble);
        }

        [TestMethod]
        public void NumberMin()
        {
            int resultInt = Serializer.Deserialize<int>(int.MinValue.ToString());
            Assert.AreEqual(int.MinValue, resultInt);

            float resultFloat = Serializer.Deserialize<float>("-3.40282347E+38");
            Assert.AreEqual(float.MinValue, resultFloat);

            double resultDouble = Serializer.Deserialize<double>("-1.7976931348623157E+308");
            Assert.AreEqual(double.MinValue, resultDouble);
        }

        [TestMethod]
        public void NumberNaN()
        {
            double result = Serializer.Deserialize<double>("NaN");
            Assert.AreEqual(double.NaN, result);
        }

        [TestMethod]
        [ExpectedParseException(1, 4)]
        public void NumberNaNAsInt()
        {
            Serializer.Deserialize<int>("NaN");
        }

        [TestMethod]
        public void NumberNegativeFloat()
        {
            float result = Serializer.Deserialize<float>("-12.567");
            Assert.AreEqual(result, -12.567f);
        }

        [TestMethod]
        public void NumberLeadingZero()
        {
            float result = Serializer.Deserialize<float>("012.567");
            Assert.AreEqual(12.567f, result);
        }

        [TestMethod]
        public void NumberNegativeLeadingZero()
        {
            float result = Serializer.Deserialize<float>("-012.567");
            Assert.AreEqual(-12.567f, result);
        }

        [TestMethod]
        public void NumberHexUpper()
        {
            int result = Serializer.Deserialize<int>("0x3BF");
            Assert.AreEqual(959, result);
        }

        [TestMethod]
        public void NumberHexLower()
        {
            int result = Serializer.Deserialize<int>("0x3bf");
            Assert.AreEqual(959, result);
        }

        [TestMethod]
        public void NumberNullable()
        {
            int? result = Serializer.Deserialize<int?>("123");
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void NumberScientificNotion()
        {
            object result = Serializer.Deserialize<int>("6.1234e4");
            Assert.AreEqual(61234, result);

            result = Serializer.Deserialize<int>("6.1234E4");
            Assert.AreEqual(61234, result);

            result = Serializer.Deserialize<int>("6.1234E+4");
            Assert.AreEqual(61234, result);

            result = Serializer.Deserialize<int>("6.1234e+4");
            Assert.AreEqual(61234, result);

            result = Serializer.Deserialize<double>("6123.1E-4");
            Assert.AreEqual(0.61231d, result);

            result = Serializer.Deserialize<double>("6123.1E-4");
            Assert.AreEqual(0.61231d, result);
        }

        [TestMethod]
        public void NumberOverflow()
        {
            try
            {
                Serializer.Deserialize<int>(((long)int.MaxValue + 1).ToString());
            }
            catch (ParseException ex)
            {
                Assert.AreNotEqual(null, ex.InnerException);
                Assert.IsInstanceOfType(ex.InnerException, typeof(OverflowException));
            }
        }

        [TestMethod]
        [ExpectedParseException(1, 2)]
        public void NumberInvalidNegative()
        {
            Serializer.Deserialize<int>("-");
        }

        [TestMethod]
        [ExpectedParseException(1, 12)]
        public void NumberMultipleDecimalDot()
        {
            Serializer.Deserialize<int>("123.123.123");
        }

        //------------------------Serialize--------------------------
        [TestMethod]
        public void SerializeNumberBasic()
        {
            string result = Serializer.Serialize(123);
            Assert.AreEqual("123", result);
        }

        [TestMethod]
        public void SerializeNumberNegative()
        {
            string result = Serializer.Serialize(-123);
            Assert.AreEqual("-123", result);
        }

        [TestMethod]
        public void SerializeNumberFloat()
        {
            string result = Serializer.Serialize(12.567f);
            Assert.AreEqual("12.567", result);
        }

        [TestMethod]
        public void SerializeNumberNaN()
        {
            string result = Serializer.Serialize(double.NaN);
            Assert.AreEqual("NaN", result);
        }

        [TestMethod]
        public void SerializeNumberNegativeFloat()
        {
            string result = Serializer.Serialize(-12.567f);
            Assert.AreEqual(result, "-12.567");
        }

        [TestMethod]
        public void SerializeNumberNullable()
        {
            int? num = 123;
            string result = Serializer.Serialize(num);
            Assert.AreEqual("123", result);
        }
    }
}
