using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.TypeTest
{
    [TestClass]
    public class TestComment : TestBase
    {
        [TestMethod]
        public void CommentBasic()
        {
            string result = Serializer.Deserialize<string>("-- random comment");
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void CommentMultipleSingleLine()
        {
            string lineComment = GetLua();
            int result = Serializer.Deserialize<int>(lineComment);
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void CommentBlock()
        {
            string blockComment = GetLua();
            int result = Serializer.Deserialize<int>(blockComment);
            Assert.AreEqual(456, result);
        }

        [TestMethod]
        public void CommentInsideBlock()
        {
            string content = GetLua();

            int result = Serializer.Deserialize<int>(content);
            Assert.AreEqual(456, result);
        }

        [TestMethod]
        public void CommentInsideLineComment()
        {
            string content = GetLua();

            int result = Serializer.Deserialize<int>(content);
            Assert.AreEqual(456, result);
        }

        [TestMethod]
        [ExpectedParseException(6, 1)]
        public void CommentInvalidNested()
        {
            string content = GetLua();
            int result = Serializer.Deserialize<int>(content);
        }

        [TestMethod]
        [ExpectedParseException(2, 5)]
        public void CommentOneDash()
        {
            string content = GetLua();
            int result = Serializer.Deserialize<int>(content);
        }
    }
}
