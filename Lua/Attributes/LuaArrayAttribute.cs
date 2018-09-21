using System;

namespace Lua
{
    public enum ArrayIndexMode
    {
        Default,
        ZeroBased,
        OneBased
    }

    public sealed class LuaArrayAttribute : Attribute
    {
        public ArrayIndexMode IndexMode { get; set; }
        public bool SkipNull { get; set; }
        public LuaArrayAttribute()
        {
            IndexMode = ArrayIndexMode.Default;
        }
    }
}
