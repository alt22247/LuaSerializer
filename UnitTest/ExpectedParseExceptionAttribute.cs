using Lua;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject1
{
    public class ExpectedParseExceptionAttribute : ExpectedExceptionBaseAttribute
    {
        int _lineNumber;
        int _linePosition;
        public ExpectedParseExceptionAttribute(int lineNumber, int linePosition)
        {
            _lineNumber = lineNumber;
            _linePosition = linePosition;
        }

        protected override void Verify(Exception exception)
        {
            ParseException pe = exception as ParseException;
            if (pe == null)
                throw new Exception($"ParseException expected but got {exception.GetType().Name}", exception);

            if (pe.LineNumber != _lineNumber)
                throw new Exception($"Expects LineNumber {_lineNumber} but got {pe.LineNumber}");

            if (pe.LinePosition != _linePosition)
                throw new Exception($"Expects LinePosition {_linePosition} but got {pe.LinePosition}");

        }
    }
}
