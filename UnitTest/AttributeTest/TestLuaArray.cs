using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lua;

namespace UnitTestProject1.AttributeTest
{
    [TestClass]
    public class TestLuaArray : TestBase
    {
        class DefaultIndexClass
        {
            [LuaArray(IndexMode = ArrayIndexMode.Default)]
            public int[] Array { get; set; }
            [LuaArray(IndexMode = ArrayIndexMode.Default)]
            public List<int> List { get; set; }
            [LuaArray(IndexMode = ArrayIndexMode.Default)]
            public IEnumerable<int> Enumerable { get; set; }
        }

        class ZeroBasedIndexClass
        {
            [LuaArray(IndexMode = ArrayIndexMode.ZeroBased)]
            public int[] Array { get; set; }
            [LuaArray(IndexMode = ArrayIndexMode.ZeroBased)]
            public List<int> List { get; set; }
            [LuaArray(IndexMode = ArrayIndexMode.ZeroBased)]
            public IEnumerable<int> Enumerable { get; set; }
        }

        class OneBasedIndexClass
        {
            [LuaArray(IndexMode = ArrayIndexMode.OneBased)]
            public int[] Array { get; set; }
            [LuaArray(IndexMode = ArrayIndexMode.OneBased)]
            public List<int> List { get; set; }
            [LuaArray(IndexMode = ArrayIndexMode.OneBased)]
            public IEnumerable<int> Enumerable { get; set; }
        }

        class SkipNullClass
        {
            [LuaArray(SkipNull = true)]
            public int?[] Array { get; set; }
            [LuaArray(SkipNull = true)]
            public List<int?> List { get; set; }
            [LuaArray(SkipNull = true)]
            public IEnumerable<int?> Enumerable { get; set; }
        }

        [TestMethod]
        public void DefaultIndexFromZero()
        {
            string lua = GetLua();
            DefaultIndexClass result = Serializer.Deserialize<DefaultIndexClass>(lua);
            Assert.AreEqual(3, result.Array.Length);
            Assert.AreEqual(0, result.Array[0]);
            Assert.AreEqual(1, result.Array[1]);
            Assert.AreEqual(2, result.Array[2]);

            Assert.AreEqual(3, result.List.Count);
            Assert.AreEqual(0, result.List[0]);
            Assert.AreEqual(1, result.List[1]);
            Assert.AreEqual(2, result.List[2]);

            Assert.AreEqual(3, result.Enumerable.Count());
            Assert.AreEqual(0, result.Enumerable.ElementAt(0));
            Assert.AreEqual(1, result.Enumerable.ElementAt(1));
            Assert.AreEqual(2, result.Enumerable.ElementAt(2));
        }

        [TestMethod]
        public void DefaultIndexFromOne()
        {
            string lua = GetLua();
            DefaultIndexClass result = Serializer.Deserialize<DefaultIndexClass>(lua);
            Assert.AreEqual(3, result.Array.Length);
            Assert.AreEqual(0, result.Array[0]);
            Assert.AreEqual(1, result.Array[1]);
            Assert.AreEqual(2, result.Array[2]);

            Assert.AreEqual(3, result.List.Count);
            Assert.AreEqual(0, result.List[0]);
            Assert.AreEqual(1, result.List[1]);
            Assert.AreEqual(2, result.List[2]);

            Assert.AreEqual(3, result.Enumerable.Count());
            Assert.AreEqual(0, result.Enumerable.ElementAt(0));
            Assert.AreEqual(1, result.Enumerable.ElementAt(1));
            Assert.AreEqual(2, result.Enumerable.ElementAt(2));
        }

        [TestMethod]
        public void DefaultIndexFromX()
        {
            string lua = GetLua();
            DefaultIndexClass result = Serializer.Deserialize<DefaultIndexClass>(lua);
            Assert.AreEqual(10, result.Array.Length);
            Assert.AreEqual(0, result.Array[9]);
            Assert.AreEqual(1, result.Array[1]);
            Assert.AreEqual(2, result.Array[2]);

            Assert.AreEqual(3, result.List.Count);
            Assert.AreEqual(0, result.List[0]);
            Assert.AreEqual(1, result.List[1]);
            Assert.AreEqual(2, result.List[2]);

            Assert.AreEqual(3, result.Enumerable.Count());
            Assert.AreEqual(0, result.Enumerable.ElementAt(0));
            Assert.AreEqual(1, result.Enumerable.ElementAt(1));
            Assert.AreEqual(2, result.Enumerable.ElementAt(2));
        }

        [TestMethod]
        public void ZeroBasedIndexFromZero()
        {
            string lua = GetLua();
            ZeroBasedIndexClass result = Serializer.Deserialize<ZeroBasedIndexClass>(lua);
            Assert.AreEqual(3, result.Array.Length);
            Assert.AreEqual(0, result.Array[0]);
            Assert.AreEqual(1, result.Array[1]);
            Assert.AreEqual(2, result.Array[2]);

            Assert.AreEqual(3, result.List.Count);
            Assert.AreEqual(0, result.List[0]);
            Assert.AreEqual(1, result.List[1]);
            Assert.AreEqual(2, result.List[2]);

            Assert.AreEqual(3, result.Enumerable.Count());
            Assert.AreEqual(0, result.Enumerable.ElementAt(0));
            Assert.AreEqual(1, result.Enumerable.ElementAt(1));
            Assert.AreEqual(2, result.Enumerable.ElementAt(2));
        }

        [TestMethod]
        public void ZeroBasedIndexFromOne()
        {
            string lua = GetLua();
            ZeroBasedIndexClass result = Serializer.Deserialize<ZeroBasedIndexClass>(lua);
            Assert.AreEqual(4, result.Array.Length);
            Assert.AreEqual(0, result.Array[0]);
            Assert.AreEqual(1, result.Array[1]);
            Assert.AreEqual(2, result.Array[2]);
            Assert.AreEqual(3, result.Array[3]);

            Assert.AreEqual(3, result.List.Count);
            Assert.AreEqual(1, result.List[0]);
            Assert.AreEqual(2, result.List[1]);
            Assert.AreEqual(3, result.List[2]);

            Assert.AreEqual(3, result.Enumerable.Count());
            Assert.AreEqual(1, result.Enumerable.ElementAt(0));
            Assert.AreEqual(2, result.Enumerable.ElementAt(1));
            Assert.AreEqual(3, result.Enumerable.ElementAt(2));
        }

        [TestMethod]
        public void OneBasedIndexFromOne()
        {
            string lua = GetLua();
            OneBasedIndexClass result = Serializer.Deserialize<OneBasedIndexClass>(lua);

            Assert.AreEqual(3, result.Array.Length);
            Assert.AreEqual(1, result.Array[0]);
            Assert.AreEqual(2, result.Array[1]);
            Assert.AreEqual(3, result.Array[2]);

            Assert.AreEqual(3, result.List.Count);
            Assert.AreEqual(1, result.List[0]);
            Assert.AreEqual(2, result.List[1]);
            Assert.AreEqual(3, result.List[2]);

            Assert.AreEqual(3, result.Enumerable.Count());
            Assert.AreEqual(1, result.Enumerable.ElementAt(0));
            Assert.AreEqual(2, result.Enumerable.ElementAt(1));
            Assert.AreEqual(3, result.Enumerable.ElementAt(2));
        }

        [TestMethod]
        public void SkipNull()
        {
            string lua = GetLua();
            SkipNullClass result = Serializer.Deserialize<SkipNullClass>(lua);
            Assert.AreEqual(3, result.Array.Length);
            Assert.AreEqual(1, result.Array[0]);
            Assert.AreEqual(3, result.Array[1]);
            Assert.AreEqual(5, result.Array[2]);

            Assert.AreEqual(3, result.List.Count);
            Assert.AreEqual(1, result.List[0]);
            Assert.AreEqual(3, result.List[1]);
            Assert.AreEqual(5, result.List[2]);

            Assert.AreEqual(3, result.Enumerable.Count());
            Assert.AreEqual(1, result.Enumerable.ElementAt(0));
            Assert.AreEqual(3, result.Enumerable.ElementAt(1));
            Assert.AreEqual(5, result.Enumerable.ElementAt(2));
        }

        //------------------------Serialize--------------------------
        [TestMethod]
        public void SerializeDefaultIndex()
        {
            string expectedLua = "{[\"Array\"]={[1]=0,[2]=1,[3]=2,},[\"List\"]={[1]=0,[2]=1,[3]=2,},[\"Enumerable\"]={[1]=0,[2]=1,[3]=2,},}";
            DefaultIndexClass input = new DefaultIndexClass();
            input.Array = new int[] { 0, 1, 2 };
            input.List = new List<int>() { 0, 1, 2 };
            input.Enumerable = Enumerable.Range(0, 3);
            string result = Serializer.Serialize(input);
            Assert.AreEqual(expectedLua, result);
        }

        [TestMethod]
        public void SerializeZeroBasedIndex()
        {
            string expectedLua = "{[\"Array\"]={[0]=0,[1]=1,[2]=2,},[\"List\"]={[0]=0,[1]=1,[2]=2,},[\"Enumerable\"]={[0]=0,[1]=1,[2]=2,},}";
            ZeroBasedIndexClass input = new ZeroBasedIndexClass();
            input.Array = new int[] { 0, 1, 2 };
            input.List = new List<int>() { 0, 1, 2 };
            input.Enumerable = Enumerable.Range(0, 3);
            string result = Serializer.Serialize(input);
            Assert.AreEqual(expectedLua, result);
        }
    }
}
