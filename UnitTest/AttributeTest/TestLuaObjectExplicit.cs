using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lua;
using UnitTestProject1.TypeTest;

namespace UnitTestProject1.AttributeTest
{
    [TestClass]
    public class TestLuaObjectExplicit : TestBase
    {
        [LuaObject(IsExplicit = true)]
        class ExplicitBasicClass1
        {
            public int PropInt { get; set; }
#pragma warning disable 0649
            public int FieldInt;
#pragma warning restore 0649
        }

        [TestMethod]
        public void ExplicitBasic1()
        {
            string lua = GetLua("ExplicitBasic");
            ExplicitBasicClass1 result = Serializer.Deserialize<ExplicitBasicClass1>(lua);
            Assert.AreEqual(0, result.PropInt);
            Assert.AreEqual(0, result.FieldInt);
        }

        [LuaObject(IsExplicit = true)]
        class ExplicitBasicClass2
        {
            [LuaMember]
            public int PropInt { get; set; }
#pragma warning disable 0649
            public int FieldInt;
#pragma warning restore 0649
        }

        [TestMethod]
        public void ExplicitBasic2()
        {
            string lua = GetLua("ExplicitBasic");
            ExplicitBasicClass2 result = Serializer.Deserialize<ExplicitBasicClass2>(lua);
            Assert.AreEqual(5, result.PropInt);
            Assert.AreEqual(0, result.FieldInt);
        }

        [LuaObject(IsExplicit = true)]
        class ExplicitBasicClass3
        {
            public int PropInt { get; set; }
#pragma warning disable 0649
            [LuaMember]
            public int FieldInt;
#pragma warning restore 0649
        }

        [TestMethod]
        public void ExplicitBasic3()
        {
            string lua = GetLua("ExplicitBasic");
            ExplicitBasicClass3 result = Serializer.Deserialize<ExplicitBasicClass3>(lua);
            Assert.AreEqual(0, result.PropInt);
            Assert.AreEqual(10, result.FieldInt);
        }

        [LuaObject(IsExplicit = true)]
        class ExplicitBasicClass4
        {
            [LuaMember]
            public int PropInt { get; set; }
#pragma warning disable 0649
            [LuaMember]
            public int FieldInt;
#pragma warning restore 0649
        }

        [TestMethod]
        public void ExplicitBasic4()
        {
            string lua = GetLua("ExplicitBasic");
            ExplicitBasicClass4 result = Serializer.Deserialize<ExplicitBasicClass4>(lua);
            Assert.AreEqual(5, result.PropInt);
            Assert.AreEqual(10, result.FieldInt);
        }

        [LuaObject(IsExplicit = true)]
        class ExplicitReadOnlyClass
        {
            [LuaMember]
            public string PropGetOnly
            {
                get
                {
                    return "Get Only";
                }
            }

            [LuaMember]
            public readonly string PropReadOnly = "Read Only";
        }

        [TestMethod]
        public void ExplicitReadOnly()
        {
            string lua = GetLua();
            ExplicitReadOnlyClass result = Serializer.Deserialize<ExplicitReadOnlyClass>(lua);
            Assert.AreEqual("Get Only", result.PropGetOnly);
            Assert.AreEqual("Updated", result.PropReadOnly);
        }

        [LuaObject(IsExplicit = true)]
        class SubClass
        {
            [LuaMember]
            public int MemberData { get; set; }
            public int NoneMemberData { get; set; }
        }

        [LuaObject(IsExplicit = true)]
        class ExplicitSubClassClass
        {
            [LuaMember]
            public SubClass SubProp { get; set; }
#pragma warning disable 0649
            [LuaMember]
            public SubClass SubField;
#pragma warning restore 0649
        }

        [TestMethod]
        public void ExplicitSubClass()
        {
            string lua = GetLua();
            ExplicitSubClassClass result = Serializer.Deserialize<ExplicitSubClassClass>(lua);
            Assert.AreNotEqual(null, result.SubProp);
            Assert.AreEqual(123, result.SubProp.MemberData);
            Assert.AreEqual(0, result.SubProp.NoneMemberData);

            Assert.AreNotEqual(null, result.SubField);
            Assert.AreEqual(123, result.SubField.MemberData);
            Assert.AreEqual(0, result.SubField.NoneMemberData);
        }
    }
}
