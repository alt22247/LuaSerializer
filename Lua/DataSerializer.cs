using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lua
{
    internal class DataSerializer
    {
        static Dictionary<char, string> _escapeCharDict;
        DataWriter _writer;
        static DataSerializer()
        {
            _escapeCharDict = new Dictionary<char, string>();
            _escapeCharDict['\a'] = "\\a";
            _escapeCharDict['\b'] = "\\b";
            _escapeCharDict['\f'] = "\\f";
            _escapeCharDict['\n'] = "\\n";
            _escapeCharDict['\r'] = "\\r";
            _escapeCharDict['\t'] = "\\t";
            _escapeCharDict['\v'] = "\\v";
            _escapeCharDict['\\'] = "\\\\";
            _escapeCharDict['"'] = "\\\"";
        }

        internal DataSerializer(DataWriter writer)
        {
            _writer = writer;
        }

        string QuoteAndEscapeString(string s)
        {
            Debug.Assert(s != null);
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                if (_escapeCharDict.ContainsKey(c))
                    sb.Append(_escapeCharDict[c]);
                else
                    sb.Append(c);
            }
            sb.Append("\"");
            return sb.ToString();
        }

        void WriteTableMember(object key, object val, IEnumerable<Attribute> attributes)
        {
            if (val == null)
                return;

            LuaMemberAttribute luaMemberAttribute = null;
            if (attributes != null)
                luaMemberAttribute = attributes.OfType<LuaMemberAttribute>().FirstOrDefault();

            string keyName = null;

            if (luaMemberAttribute != null && !string.IsNullOrEmpty(luaMemberAttribute.Name))
                keyName = QuoteAndEscapeString(luaMemberAttribute.Name);
            else if (key is string)
                keyName = QuoteAndEscapeString((string) key);
            else
                keyName = key.ToString();

            _writer.Write($"[{keyName}]=");

            Serialize(val, attributes);
            _writer.Write(",");
        }

        internal void Serialize(object obj)
        {
            Serialize(obj, null);
        }

        void Serialize(object obj, IEnumerable<Attribute> attributes)
        {
            if (obj == null)
                return;

            Type type = obj.GetType();

            if (type.IsEnum)
            {
                _writer.Write(((int) obj).ToString());
            }
            else if (type.IsValueType)
            {
                if (type == typeof(DateTime))
                    _writer.Write(QuoteAndEscapeString(obj.ToString()));
                else if (type == typeof(bool))
                    _writer.Write(obj.ToString().ToLower());
                else
                    _writer.Write(obj.ToString());
            }
            else if (type == typeof(string))
            {
                _writer.Write(QuoteAndEscapeString((string) obj));
            }
            else if (typeof(IDictionary).IsAssignableFrom(type))
            {
                _writer.Write("{");
                IDictionary dict = (IDictionary) obj;
                foreach (object key in dict.Keys)
                    WriteTableMember(key, dict[key], null);
                _writer.Write("}");
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                IEnumerable list = (IEnumerable) obj;
                _writer.Write("{");

                bool skipNull = false;
                ArrayIndexMode indexMode = ArrayIndexMode.Default;
                LuaArrayAttribute arrayAttribute = null;
                if (attributes != null)
                    arrayAttribute = (LuaArrayAttribute) attributes.FirstOrDefault(x => x is LuaArrayAttribute);
                if (arrayAttribute != null)
                {
                    skipNull = arrayAttribute.SkipNull;
                    indexMode = arrayAttribute.IndexMode;
                }

                int index = indexMode == ArrayIndexMode.ZeroBased ? 0 : 1;

                foreach (object item in list)
                {
                    WriteTableMember(index, item, null);
                    index++;
                }
                _writer.Write("}");
            }
            else
            {
                _writer.Write("{");
                foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    WriteTableMember(pi.Name, pi.GetValue(obj), pi.GetCustomAttributes());

                foreach (FieldInfo fi in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    WriteTableMember(fi.Name, fi.GetValue(obj), fi.GetCustomAttributes());
                _writer.Write("}");
            }
        }
    }

}
