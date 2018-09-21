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
    public class TestLuaObjectIncludeNonPublic : TestBase
    {
        [LuaObject(IncludeNonPublic = true)]
        class IncludeNonPublicBasicClass
        {
            private int _privatePropInt { get; set; }
            protected int _protectedPropInt { get; set; }
#pragma warning disable 0649
            private int _privateFieldInt;
            protected int _protectedFieldInt;
#pragma warning restore 0649

            public int PrivatePropInt
            {
                get
                {
                    return _privatePropInt;
                }
            }

            public int PrivateFieldInt
            {
                get
                {
                    return _privateFieldInt;
                }
            }

            public int ProtectedPropInt
            {
                get
                {
                    return _protectedPropInt;
                }
            }

            public int ProtectedFieldInt
            {
                get
                {
                    return _protectedFieldInt;
                }
            }
        }

        [TestMethod]
        public void IncludeNonPublicBasic()
        {
            string lua = GetLua();
            IncludeNonPublicBasicClass result = Serializer.Deserialize<IncludeNonPublicBasicClass>(lua);
            Assert.AreEqual(5, result.PrivatePropInt);
            Assert.AreEqual(10, result.PrivateFieldInt);
            Assert.AreEqual(5, result.ProtectedPropInt);
            Assert.AreEqual(10, result.ProtectedFieldInt);
        }

        class PrivateSetClass
        {
            public int Prop1 { get; private set; }
        }

        [TestMethod]
        public void PrivateSet()
        {
            string lua = GetLua();
            PrivateSetClass result = Serializer.Deserialize<PrivateSetClass>(lua);
            Assert.AreEqual(0, result.Prop1);
        }

        [LuaObject(IncludeNonPublic = true)]
        class IncludeNonPublicPrivateSetClass
        {
            public int Prop1 { get; private set; }
        }

        [TestMethod]
        public void IncludeNonPublicPrivateSet()
        {
            string lua = GetLua();
            IncludeNonPublicPrivateSetClass result = Serializer.Deserialize<IncludeNonPublicPrivateSetClass>(lua);
            Assert.AreEqual(5, result.Prop1);
        }

        [LuaObject(IncludeNonPublic = true, IsExplicit = true)]
        class IncludeNonPublicExplicitClass
        {
            [LuaMember]
            private int _propInt { get; set; }
            private int _propInt2 { get; set; }

#pragma warning disable 0649
            [LuaMember]
            private int _fieldInt;
            private int _fieldInt2;
#pragma warning restore 0649

            public int PropInt
            {
                get
                {
                    return _propInt;
                }
            }

            public int PropInt2
            {
                get
                {
                    return _propInt2;
                }
            }

            public int FieldInt
            {
                get
                {
                    return _fieldInt;
                }
            }

            public int FieldInt2
            {
                get
                {
                    return _fieldInt2;
                }
            }
        }

        [TestMethod]
        public void IncludeNonPublicExplicit()
        {
            string lua = GetLua();
            IncludeNonPublicExplicitClass result = Serializer.Deserialize<IncludeNonPublicExplicitClass>(lua);
            Assert.AreEqual(5, result.PropInt);
            Assert.AreEqual(10, result.FieldInt);
            Assert.AreEqual(0, result.PropInt2);
            Assert.AreEqual(0, result.FieldInt2);
        }
    }
}
