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
    public class TestLuaMember : TestBase
    {
        class MemberBasicClass
        {
            [LuaMember(Name = "Prop1")]
            public int PropInt { get; set; }
#pragma warning disable 0649
            [LuaMember(Name = "Field1")]
            public int FieldInt;
#pragma warning restore 0649
        }

        [TestMethod]
        public void MemberName()
        {
            string lua = GetLua();
            MemberBasicClass result = Serializer.Deserialize<MemberBasicClass>(lua);
            Assert.AreEqual(result.PropInt, 3);
            Assert.AreEqual(result.FieldInt, 4);
        }

        abstract class MemberBaseClass
        {
            [LuaMember(Name = "V1")]
            public virtual int VirtualProp { get; set; }
            [LuaMember(Name = "V2")]
            public virtual int VirtualProp2 { get; set; }
            [LuaMember(Name = "A1")]
            public abstract int AbstractProp { get; set; }
            [LuaMember(Name = "A2")]
            public abstract int AbstractProp2 { get; set; }
        }

        class MemberInheritedClass : MemberBaseClass
        {
            public override int VirtualProp { get; set; }
            [LuaMember(Name = "V3")]
            public override int VirtualProp2 { get; set; }
            public override int AbstractProp { get; set; }
            [LuaMember(Name = "A3")]
            public override int AbstractProp2 { get; set; }
        }

        [TestMethod]
        public void MemberNameInherited()
        {
            string lua = GetLua();
            MemberInheritedClass result = Serializer.Deserialize<MemberInheritedClass>(lua);
            Assert.AreEqual(1, result.VirtualProp);
            Assert.AreEqual(3, result.VirtualProp2);
            Assert.AreEqual(4, result.AbstractProp);
            Assert.AreEqual(6, result.AbstractProp2);
        }

        class MemberNameDuplicateClass
        {
            [LuaMember(Name = "Prop")]
            public int Prop1 { get; set; }
            [LuaMember(Name = "Prop")]
            public int Prop2 { get; set; }
        }

        [TestMethod]
        [ExpectedParseException(1, 2)]
        public void MemberDuplicateName()
        {
            Serializer.Deserialize<MemberNameDuplicateClass>("{}");
        }
    }
}
