using System;

namespace Lua
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class LuaObjectAttribute : Attribute
    {
        public bool IsExplicit { get; set; }
        public bool IncludeNonPublic { get; set; }
    }
}
