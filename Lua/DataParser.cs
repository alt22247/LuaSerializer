using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lua
{
    internal class DataParser
    {
        static HashSet<char> _validNumberChars;
        static Dictionary<char, char> _escapeCharDict;
        DataReader _dataReader;

        static DataParser()
        {
            _validNumberChars = new HashSet<char> { '.', '+', '-', 'e', 'x' };
            for (char c = '0'; c <= '9'; c++)
                _validNumberChars.Add(c);

            for (char c = 'a'; c <= 'f'; c++)
                _validNumberChars.Add(c);

            for (char c = 'A'; c <= 'F'; c++)
                _validNumberChars.Add(c);

            _escapeCharDict = new Dictionary<char, char>();

            _escapeCharDict['a'] = '\a';
            _escapeCharDict['b'] = '\b';
            _escapeCharDict['f'] = '\f';
            _escapeCharDict['n'] = '\n';
            _escapeCharDict['r'] = '\r';
            _escapeCharDict['t'] = '\t';
            _escapeCharDict['v'] = '\v';
            _escapeCharDict['\\'] = '\\';
            _escapeCharDict['"'] = '"';
            _escapeCharDict['\''] = '\'';
            _escapeCharDict['['] = '[';
            _escapeCharDict[']'] = ']';
        }

        internal DataParser(DataReader dataReader)
        {
            _dataReader = dataReader;
        }

        void SkipSpaces()
        {
            SkipSpaces(false);
        }

        void SkipSpaces(bool skipNewLine)
        {
            int r;
            while ((r = _dataReader.Peek()) != -1)
            {
                char c = (char) r;
                if (c != ' ' && c != '\t')
                {
                    if (!skipNewLine)
                        return;

                    if (c != '\n' && c != '\r')
                        return;
                }

                _dataReader.Read();
            }
        }

        void SkipSpacesAndComment()
        {
            SkipSpaces(true);

            if (_dataReader.Peek() != '-' || _dataReader.Peek(1) != '-')
                return;

            _dataReader.Read();
            _dataReader.Read();

            bool isCommentBlock = _dataReader.Peek() == '[' && _dataReader.Peek(1) == '[';
            if (isCommentBlock)
            {
                _dataReader.Read();
                _dataReader.Read();
            }

            bool success = false;
            int r;
            while ((r = _dataReader.Read()) != -1)
            {
                char c = (char) r;
                if (isCommentBlock)
                {
                    if (c == '-' && _dataReader.Peek() == '-' &&
                        _dataReader.Peek(1) == ']' && _dataReader.Peek(2) == ']')
                    {
                        for (int i = 0; i < 4; i++)
                            _dataReader.Read();

                        success = true;
                        break;
                    }
                }
                else if (c == '\n' || c == '\r')
                {
                    success = true;
                    if (c == '\r' && _dataReader.Peek() == '\n')
                        _dataReader.Read();

                    break;
                }
            }

            if (isCommentBlock && !success)
                throw new ParseException("Closing tag for comment not found");

            SkipSpacesAndComment();
        }

        string ReadString()
        {
            int r = _dataReader.Read();
            char quote = (char) r;
            if (r != '"' && r != '\'')
                throw new ParseException($"String double quote expected but got {(char) r}");

            int startPosition = _dataReader.Cursor;
            bool isEscaped = false;

            StringBuilder sb = new StringBuilder();
            while ((r = _dataReader.Read()) != -1)
            {
                char c = (char) r;
                if (isEscaped)
                {
                    if (_escapeCharDict.ContainsKey(c))
                    {
                        sb.Append(_escapeCharDict[c]);
                    }
                    else if (char.IsDigit(c))
                    {
                        string asciiSequence = c.ToString();
                        for (int i = 0; i < 2; i++)
                        {
                            r = _dataReader.Peek();
                            if (!char.IsDigit((char) r))
                                break;

                            asciiSequence += (char) _dataReader.Read();
                        }

                        int ascii = int.Parse(asciiSequence);
                        sb.Append((char) ascii);
                    }
                    else
                    {
                        throw new ParseException($"Unreconized escape sequence {c}");
                    }
                    isEscaped = false;
                }
                else if (c == '\\')
                {
                    isEscaped = true;
                }
                else
                {
                    if (c == quote)
                        return sb.ToString();

                    if (c == '\r' || c == '\n')
                        break;

                    sb.Append(c);
                }
            }

            throw new ParseException($"Quote at {startPosition - 1} was not closed");
        }

        object ReadNumber()
        {
            int r;
            string numString = string.Empty;
            while ((r = _dataReader.Peek()) != -1)
            {
                char c = (char) r;
                if (!_validNumberChars.Contains(c))
                    break;

                numString += c;
                _dataReader.Read();
            }

            if (numString == string.Empty)
                throw new ParseException("No number read");

            if (numString.StartsWith("0x"))
                return Convert.ToInt64(numString, 16);

            string maxIntString = int.MaxValue.ToString();
            if (!numString.Contains(".") && numString.Length <= maxIntString.Length && numString.CompareTo(maxIntString) <= 0)
                return int.Parse(numString);

            return double.Parse(numString);
        }

        bool ReadBool()
        {
            char c = (char) _dataReader.Peek();
            char[] expectedChars;
            bool expectedResult;
            if (c == 't')
            {
                expectedChars = new char[] { 'r', 'u', 'e' };
                expectedResult = true;
            }
            else if (c == 'f')
            {
                expectedChars = new char[] { 'a', 'l', 's', 'e' };
                expectedResult = false;
            }
            else
            {
                throw new ParseException("Invalid Character");
            }

            _dataReader.Read();
            bool success = true;
            foreach (char expectedChar in expectedChars)
            {
                if (_dataReader.Peek() != expectedChar)
                {
                    success = false;
                    break;
                }

                _dataReader.Read();
            }

            if (!success)
                throw new ParseException("Error reading boolean");

            return expectedResult;
        }

        void ReadNil()
        {
            const string expectedString = "nil";
            for (int i = 0; i < 3; i++)
            {
                char c = (char) _dataReader.Peek();
                if (c != expectedString[i])
                    throw new ParseException("Failed to read nil");

                _dataReader.Read();
            }
        }

        void ReadNan()
        {
            const string expectedString = "NaN";
            for (int i = 0; i < 3; i++)
            {
                char c = (char) _dataReader.Peek();
                if (c != expectedString[i])
                    throw new ParseException("Failed to read NaN");

                _dataReader.Read();
            }
        }

        object ReadTableMemberKeyAssignment(Type keyType)
        {
            int startPosition = _dataReader.Cursor;
            int r = _dataReader.Read();
            if (r != '[')
                throw new ParseException($"Expects table member open bracket but got {(char) r}");

            object val = ReadKeyValue();
            if (keyType != null)
                val = ChangeType(val, keyType);

            SkipSpacesAndComment();
            r = _dataReader.Read();
            if (r != ']')
                throw new ParseException($"Table member bracket at {startPosition} is not closed");

            SkipSpacesAndComment();
            r = _dataReader.Read();
            if (r != '=')
                throw new ParseException($"= sign not found at {_dataReader.Cursor}");

            return val;
        }

        object ChangeType(object value, Type type)
        {
            if (type != null && type != typeof(object))
            {
                if (type.IsEnum)
                {
                    if (value is int)
                        return Enum.ToObject(type, (int) value);
                    else if (value is string)
                        return Enum.Parse(type, (string) value);
                    else
                        throw new ParseException("Unable to convert enum");
                }
                else
                {
                    return Convert.ChangeType(value, Nullable.GetUnderlyingType(type) ?? type);
                }
            }

            return value;
        }

        Dictionary<string, MemberInfo> GetMemberNameInfoDict(Type tableType)
        {
            LuaObjectAttribute luaObjectAttribute = tableType.GetCustomAttribute<LuaObjectAttribute>();
            bool isExplicit = false;
            bool includeNonPublic = false;

            Dictionary<string, MemberInfo> memberNameInfoDict = new Dictionary<string, MemberInfo>();

            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            if (luaObjectAttribute != null)
            {
                includeNonPublic = luaObjectAttribute.IncludeNonPublic;
                if (includeNonPublic)
                    bindingFlags = bindingFlags | BindingFlags.NonPublic;

                isExplicit = luaObjectAttribute.IsExplicit;
            }

            PropertyInfo[] pInfos = tableType.GetProperties(bindingFlags);
            FieldInfo[] fInfos = tableType.GetFields(bindingFlags);

            foreach (MemberInfo mi in pInfos.Union<MemberInfo>(fInfos))
            {
                CompilerGeneratedAttribute compilerGeneratedAttribute = mi.GetCustomAttribute<CompilerGeneratedAttribute>();
                if (compilerGeneratedAttribute != null)
                    continue;

                LuaMemberAttribute luaMemberAttribute = mi.GetCustomAttribute<LuaMemberAttribute>();
                if (luaMemberAttribute == null && isExplicit)
                    continue;

                string name = mi.Name;
                if (luaMemberAttribute != null && !string.IsNullOrEmpty(luaMemberAttribute.Name))
                    name = luaMemberAttribute.Name;

                if (mi is PropertyInfo)
                {
                    PropertyInfo pi = (PropertyInfo) mi;
                    if (pi.GetSetMethod(includeNonPublic) == null)
                        continue;
                }

                memberNameInfoDict.Add(name, mi);
            }

            return memberNameInfoDict;
        }

        object ReadTable(Type tableType, IEnumerable<Attribute> attributes)
        {
            int startPosition = _dataReader.Cursor;
            int r = _dataReader.Read();
            if (r != '{')
                throw new ParseException($"Expects table open bracket but got {(char) r}");

            Type genericTypeDefinition = null;
            if (tableType.IsGenericType)
                genericTypeDefinition = tableType.GetGenericTypeDefinition();
            bool isTableTypeDict = genericTypeDefinition == typeof(Dictionary<,>);

            Dictionary<string, MemberInfo> memberNameInfoDict = null;

            Type keyType = null;
            Type valueType = null;

            bool isFixedValueType = false;
            if (isTableTypeDict)
            {
                Type[] genericArguments = tableType.GetGenericArguments();
                keyType = genericArguments[0];
                valueType = genericArguments[1];
                isFixedValueType = true;
            }
            else if (tableType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(tableType))
            {
                valueType = tableType.GetGenericArguments()[0];
                isFixedValueType = true;
            }
            else if (tableType.IsArray)
            {
                keyType = typeof(int);
                valueType = tableType.GetElementType();
                isFixedValueType = true;
            }
            else
            {
                memberNameInfoDict = GetMemberNameInfoDict(tableType);
            }

            Dictionary<object, object> tableContent = new Dictionary<object, object>();
            bool isSuccess = false;

            while ((r = _dataReader.Peek()) != -1)
            {
                SkipSpacesAndComment();
                r = _dataReader.Peek();

                if (r == '}')
                {
                    _dataReader.Read();
                    isSuccess = true;
                    break;
                }

                object key = ReadTableMemberKeyAssignment(keyType);
                IEnumerable<Attribute> memberAttributes = null;
                if (!isFixedValueType)
                {
                    string keyString = key.ToString();
                    if (memberNameInfoDict.ContainsKey(keyString))
                    {
                        MemberInfo mi = memberNameInfoDict[keyString];
                        if (mi is PropertyInfo)
                            valueType = ((PropertyInfo) mi).PropertyType;
                        else
                            valueType = ((FieldInfo) mi).FieldType;

                        if (valueType.IsArray)
                            memberAttributes = mi.GetCustomAttributes();
                    }
                    else
                    {
                        valueType = null;
                    }
                }

                object value = ReadValue(valueType, memberAttributes);

                if (valueType != null)
                    tableContent.Add(key, value);

                SkipSpacesAndComment();
                r = _dataReader.Peek();

                if (r == ',')
                    _dataReader.Read();
                else if (r != '}')
                    throw new ParseException("Separation comma expected");
            }

            if (!isSuccess)
                throw new ParseException($"Open bracket at {startPosition} is not closed");

            object result = null;
            if (tableType.IsArray)
            {
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

                int size = 0;
                if (skipNull)
                {
                    size = tableContent.Keys.Count;
                }
                else if (tableContent.Keys.Count > 0)
                {
                    foreach (int key in tableContent.Keys.Select(x => Convert.ToInt32(x)))
                    {
                        size = Math.Max(size, key);
                        if (key == 0 && indexMode == ArrayIndexMode.Default)
                            indexMode = ArrayIndexMode.ZeroBased;
                    }

                    if (indexMode == ArrayIndexMode.ZeroBased)
                        size++;
                }

                Array array = Array.CreateInstance(valueType, size);
                int index = 0;
                foreach (object key in tableContent.Keys.OrderBy(x => Convert.ToInt32(x)))
                {
                    int targetIndex = index;
                    if (!skipNull)
                    {
                        targetIndex = Convert.ToInt32(key);
                        if (indexMode != ArrayIndexMode.ZeroBased)
                            targetIndex--;
                    }

                    if (targetIndex < 0)
                        throw new ParseException("Invalid Index");

                    array.SetValue(tableContent[key], targetIndex);
                    index++;
                }

                result = array;
            }
            else if (isTableTypeDict)
            {
                IDictionary dict = (IDictionary) Activator.CreateInstance(tableType);
                foreach (object key in tableContent.Keys)
                    dict.Add(key, tableContent[key]);

                result = dict;
            }
            else if (typeof(IList).IsAssignableFrom(tableType))
            {
                IList list = (IList) Activator.CreateInstance(tableType);
                foreach (object val in tableContent.Values)
                    list.Add(val);

                result = list;
            }
            else if (genericTypeDefinition == typeof(IEnumerable<>))
            {
                Type listType = typeof(List<>).MakeGenericType(new Type[] { valueType });
                IList list = (IList) Activator.CreateInstance(listType);

                foreach (object val in tableContent.Values)
                    list.Add(val);

                result = list;
            }
            else
            {
                object o = Activator.CreateInstance(tableType);

                foreach (object key in tableContent.Keys)
                {
                    string keyString = key.ToString();

                    MemberInfo mi = memberNameInfoDict[keyString];

                    PropertyInfo pInfo = mi as PropertyInfo;
                    FieldInfo fInfo = mi as FieldInfo;

                    if (pInfo != null)
                        pInfo.SetValue(o, tableContent[key]);
                    else if (fInfo != null)
                        fInfo.SetValue(o, tableContent[key]);
                }

                result = o;
            }

            return result;
        }

        object ReadKeyValue()
        {
            char c = (char) _dataReader.Peek();
            if (char.IsDigit(c) || c == '-') //number
                return ReadNumber();
            else if (c == '"' || c == '\'') //string
                return ReadString();
            else //better be bool
                return ReadBool();
        }

        object ReadValue(Type type)
        {
            return ReadValue(type, null);
        }

        object ReadValue(Type type, IEnumerable<Attribute> attributes)
        {
            SkipSpacesAndComment();
            char c = (char) _dataReader.Peek();

            if (c == '{') //table
            {
                if (type == null || type == typeof(object))
                    type = typeof(Dictionary<string, object>);

                return ReadTable(type, attributes);
            }
            else if (c == 'n') //nil
            {
                ReadNil();
                if (type.IsValueType)
                    return Activator.CreateInstance(type);
                return null;
            }
            else if (c == 'N') //NaN
            {
                ReadNan();
                return double.NaN;
            }
            else
            {
                object result = ReadKeyValue();
                return ChangeType(result, type);
            }
        }

        internal T Parse<T>()
        {
            try
            {
                SkipSpacesAndComment();
                if (_dataReader.Peek() == -1)
                    return default(T);

                T result = (T) ReadValue(typeof(T));
                SkipSpacesAndComment();
                if (_dataReader.Peek() != -1)
                    throw new ParseException("Extra tailing strings found");
                return result;
            }
            catch (ParseException ex)
            {
                ex.LineNumber = _dataReader.LineNumber;
                ex.LinePosition = _dataReader.LinePosition;
                throw ex;
            }
            catch (Exception ex)
            {
                ParseException e = new ParseException(ex);
                e.LineNumber = _dataReader.LineNumber;
                e.LinePosition = _dataReader.LinePosition;
                throw e;
            }
        }
    }

}
