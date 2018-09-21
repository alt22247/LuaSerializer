using System.Diagnostics;
using System.IO;

namespace Lua
{
    internal class DataReader
    {
        const int _ll = 4;
        const int _bufferSize = 16 * 1024;
        TextReader _reader;
        char[] _buffer;

        bool _isEndOfStream;
        int _lastCharIndex;

        internal int LineNumber { get; private set; }
        internal int LinePosition { get; private set; }
        internal int Cursor { get; private set; }

        internal DataReader(TextReader reader)
        {
            _buffer = new char[_bufferSize + _ll];
            _lastCharIndex = _buffer.Length - 1;
            _reader = reader;
            Cursor = -1;
            LineNumber = 1;
            LinePosition = 1;
            ReadBuffer();
        }

        internal int Read()
        {
            if (!_isEndOfStream && _lastCharIndex - Cursor + 1 < 4)
                ReadBuffer();

            if (Cursor <= _lastCharIndex)
            {
                char current = _buffer[Cursor];
                Cursor++;
                LinePosition++;

                if (current == '\n' || (current == '\r' && Peek() != '\n'))
                {
                    LineNumber++;
                    LinePosition = 1;
                }

                if (current == '\t')
                    LinePosition += 3;

                return current;
            }

            return -1;
        }

        internal int Peek()
        {
            return Peek(0);
        }

        internal int Peek(int advance)
        {
            Debug.Assert(advance <= _ll);
            int target = Cursor + advance;
            if (target <= _lastCharIndex)
                return _buffer[target];

            return -1;
        }

        void ReadBuffer()
        {
            Debug.Assert(!_isEndOfStream);
            int startIndex = 0;
            if (Cursor != -1)
            {
                startIndex = _lastCharIndex - Cursor + 1;
                //Shifts remaining buffer to beginning
                for (int i = 0; i < startIndex; i++)
                    _buffer[i] = _buffer[Cursor + i];
            }
            Cursor = 0;

            int charsRead = _reader.Read(_buffer, startIndex, _bufferSize);
            _lastCharIndex = startIndex + charsRead - 1;
            _isEndOfStream = charsRead != _bufferSize;
        }
    }

}
