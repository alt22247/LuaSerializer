using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lua;

namespace UnitTestProject1
{
    [TestClass]
    public class TestBase
    {
        string _luaFileFolderName;
        protected TestBase()
        {
            string parentNamespace = typeof(TestBase).Namespace;
            string classNamespace = GetType().Namespace;

            if (parentNamespace == classNamespace)
                _luaFileFolderName = string.Empty;
            else
                _luaFileFolderName = classNamespace.Substring(parentNamespace.Length + 1);
        }

        protected LuaSerializer Serializer;
        protected Stopwatch Stopwatch;
        [TestInitialize]
        public void InitializeSerializer()
        {
            Serializer = new LuaSerializer();
            Stopwatch = new Stopwatch();
        }

        protected string GetLua([CallerMemberName]string fileName = null)
        {
            return File.ReadAllText(Path.Combine("LuaFiles", _luaFileFolderName, fileName + ".lua"));
        }
    }
}
