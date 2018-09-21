using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.StressTest
{
    [TestClass]
    public class StressTestArray : TestBase
    {
        [TestMethod]
        public void ArraySize10()
        {
            ArraySizeTest(10);
        }

        [TestMethod]
        public void ArraySize10x2()
        {
            ArraySizeTest((int) Math.Pow(10, 2));
        }

        [TestMethod]
        public void ArraySize10x3()
        {
            ArraySizeTest((int)Math.Pow(10, 3));
        }

        [TestMethod]
        public void ArraySize10x4()
        {
            ArraySizeTest((int)Math.Pow(10, 4));
        }

        [TestMethod]
        public void ArraySize10x5()
        {
            ArraySizeTest((int)Math.Pow(10, 5));
        }

        [TestMethod]
        public void ArraySize10x6()
        {
            ArraySizeTest((int)Math.Pow(10, 6));
        }

        /*
        [TestMethod]
        public void ArraySize10x7()
        {
            ArraySizeTest((int)Math.Pow(10, 7));
        }
        */

        private void ArraySizeTest(int size)
        {
            int[] array = new int[size];
            Random rng = new Random();
            for (int i = 0; i < size; i++)
                array[i] = rng.Next(100);

            string serializedString = Serializer.Serialize(array);
            int[] newArray = Serializer.Deserialize<int[]>(serializedString);

            Assert.AreEqual(array.Length, newArray.Length);
            for (int i = 0; i < array.Length; i++)
                Assert.AreEqual(array[i], newArray[i]);
        }
    }
}
