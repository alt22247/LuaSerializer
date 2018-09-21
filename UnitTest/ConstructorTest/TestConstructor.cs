using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1.ConstructorTest
{
    [TestClass]
    public class TestConstructor : TestBase
    {
        class PrivateConstructorClass
        {
            PrivateConstructorClass()
            {
            }
        }

        [TestMethod]
        [ExpectedParseException(1, 3)]
        public void PrivateConstructor()
        {
            Serializer.Deserialize<PrivateConstructorClass>("{}");
        }

        class ProtectedConstructorClass
        {
            ProtectedConstructorClass()
            {
            }
        }

        [TestMethod]
        [ExpectedParseException(1, 3)]
        public void ProtectedConstructor()
        {
            Serializer.Deserialize<ProtectedConstructorClass>("{}");
        }

        class InternalConstructorClass
        {
            InternalConstructorClass()
            {
            }
        }

        [TestMethod]
        [ExpectedParseException(1, 3)]
        public void InternalConstructor()
        {
            Serializer.Deserialize<InternalConstructorClass>("{}");
        }

        class PublicConstructorClass
        {
            public PublicConstructorClass()
            {
            }
        }

        [TestMethod]
        public void PublicConstructor()
        {
            PublicConstructorClass result = Serializer.Deserialize<PublicConstructorClass>("{}");
            Assert.IsNotNull(result);
        }

        class NoParameterlessConstructorClass
        {
            public NoParameterlessConstructorClass(int num)
            {
            }
        }

        [TestMethod]
        [ExpectedParseException(1, 3)]
        public void NoParameterlessConstructor()
        {
            Serializer.Deserialize<NoParameterlessConstructorClass>("{}");
        }
    }
}
