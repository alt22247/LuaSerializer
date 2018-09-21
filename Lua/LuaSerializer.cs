using System;
using System.IO;

namespace Lua
{

    public class LuaSerializer
    {
        public T Deserialize<T>(string data)
        {
            if (data == null)
                return default(T);

            using (StringReader sr = new StringReader(data))
                return Deserialize<T>(sr);
        }

        public T Deserialize<T>(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("Reader cannot be null");

            DataReader dataReader = new DataReader(reader);
            DataParser parser = new DataParser(dataReader);

            return parser.Parse<T>();
        }

        public string Serialize(object obj)
        {
            if (obj == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    Serialize(obj, sw);

                    ms.Seek(0, SeekOrigin.Begin);
                    using (StreamReader sr = new StreamReader(ms))
                        return sr.ReadToEnd();
                }
            }
        }

        public void Serialize(object obj, TextWriter writer)
        {
            if (obj == null)
                return;

            DataWriter dataWriter = new DataWriter(writer);
            DataSerializer serializer = new DataSerializer(dataWriter);
            serializer.Serialize(obj);
            dataWriter.Flush();
        }
    }
}
