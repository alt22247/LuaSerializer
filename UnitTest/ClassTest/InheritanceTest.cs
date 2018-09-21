using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1.ClassTest
{
    [TestClass]
    public class InheritanceTest : TestBase
    {
        class BaseClass
        {
            public int BaseProp { get; set; }
#pragma warning disable 0649
            public int BaseField;
#pragma warning restore 0649
        }

        class InheritedClass : BaseClass
        {
            public int InheritedProp { get; set; }
#pragma warning disable 0649
            public int InheritedField;
#pragma warning restore 0649
        }

        [TestMethod]
        public void InheritBasic()
        {
            string lua = GetLua();
            InheritedClass result = Serializer.Deserialize<InheritedClass>(lua);
            Assert.AreEqual(1, result.BaseProp);
            Assert.AreEqual(2, result.BaseField);
            Assert.AreEqual(3, result.InheritedProp);
            Assert.AreEqual(4, result.InheritedField);

            string serializeString = Serializer.Serialize(result);
            Assert.AreEqual("{[\"InheritedProp\"]=3,[\"BaseProp\"]=1,[\"InheritedField\"]=4,[\"BaseField\"]=2,}", serializeString);
        }

        class InheritedClass2 : InheritedClass
        {
            public int Inherited2Prop { get; set; }
        }

        [TestMethod]
        public void InheritDepth()
        {
            string lua = GetLua();
            InheritedClass2 result = Serializer.Deserialize<InheritedClass2>(lua);
            Assert.AreEqual(1, result.BaseProp);
            Assert.AreEqual(2, result.BaseField);
            Assert.AreEqual(3, result.InheritedProp);
            Assert.AreEqual(4, result.InheritedField);
            Assert.AreEqual(5, result.Inherited2Prop);

            string serializeString = Serializer.Serialize(result);
            Assert.AreEqual("{[\"Inherited2Prop\"]=5,[\"InheritedProp\"]=3,[\"BaseProp\"]=1,[\"InheritedField\"]=4,[\"BaseField\"]=2,}", serializeString);
        }

        class VirtualBaseClass
        {
            public virtual int VirtualProp { get; set; }
#pragma warning disable 0649
            public virtual int VirtualProp2 { get; set; }
#pragma warning restore 0649
        }

        class InheritedVirtualClass : VirtualBaseClass
        {
            public override int VirtualProp { get; set; }
        }

        [TestMethod]
        public void InheritVirtual()
        {
            string lua = GetLua();
            InheritedVirtualClass result = Serializer.Deserialize<InheritedVirtualClass>(lua);
            Assert.AreEqual(1, result.VirtualProp);
            Assert.AreEqual(2, result.VirtualProp2);

            string serializeString = Serializer.Serialize(result);
            Assert.AreEqual("{[\"VirtualProp\"]=1,[\"VirtualProp2\"]=2,}", serializeString);
        }

        abstract class AbstractBaseClass
        {
            public abstract int Prop { get; set; }
        }

        class InheritedAbstractClass : AbstractBaseClass
        {
            public override int Prop { get; set; }
        }

        [TestMethod]
        public void InheritedAbstract()
        {
            string lua = GetLua();
            InheritedAbstractClass result = Serializer.Deserialize<InheritedAbstractClass>(lua);
            Assert.AreEqual(1, result.Prop);

            string serializeString = Serializer.Serialize(result);
            Assert.AreEqual("{[\"Prop\"]=1,}", serializeString);
        }
    }
}
