using System.IO;

namespace Lua
{
    internal class DataWriter
    {
        TextWriter _writer;
        const int _bufferSize = 1024 * 1024;
        int _currentBuffer = 0;
        internal DataWriter(TextWriter writer)
        {
            _writer = writer;
        }

        internal void Write(string s)
        {
            _writer.Write(s);
            _currentBuffer++;
            if (_currentBuffer > _bufferSize)
            {
                Flush();
                _currentBuffer = 0;
            }
        }

        internal void Flush()
        {
            _writer.Flush();
        }
    }

}
