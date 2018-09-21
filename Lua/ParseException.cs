using System;

namespace Lua
{
    public class ParseException : Exception
    {
        public int LineNumber { get; internal set; }
        public int LinePosition { get; internal set; }

        public ParseException(string msg)
            : base(msg)
        {
        }

        public ParseException(Exception innerException)
            : this($"Error parsing content", innerException)
        {
        }

        public ParseException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }

        public override string Message
        {
            get
            {
                return base.Message + $"\nat line {LineNumber} position {LinePosition}";
            }
        }
    }
}
