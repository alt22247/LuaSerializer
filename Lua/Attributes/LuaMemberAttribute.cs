using System;

namespace Lua
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class LuaMemberAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
