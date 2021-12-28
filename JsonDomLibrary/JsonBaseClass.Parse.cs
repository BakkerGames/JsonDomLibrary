﻿namespace JsonDomLibrary;

public abstract partial class JsonBaseClass
{
    private const string INVALID_JSON = "Invalid json";

    internal static JsonObject GetValueObject(string data, ref int pos)
    {
        JsonObject result = new();
        SkipWhitespace(data, ref pos);
        pos++;
        object? token = GetValue(data, ref pos);
        while (token?.ToString() != "}")
        {
            if (GetValue(data, ref pos)?.ToString() != ":")
                throw new ArgumentException(INVALID_JSON);
            object? value = GetValue(data, ref pos);
            result[token?.ToString()] = value;
            token = GetValue(data, ref pos);
            if (token?.ToString() == ",")
                token = GetValue(data, ref pos);
            else if (token?.ToString() != "}")
                throw new ArgumentException(INVALID_JSON);
        }
        return result;
    }

    internal static JsonArray GetValueArray(string data, ref int pos)
    {
        JsonArray result = new();
        pos++;
        object? token = GetValue(data, ref pos);
        while (token?.ToString() != "]")
        {
            result.Add(token);
            token = GetValue(data, ref pos);
            if (token?.ToString() == ",")
                token = GetValue(data, ref pos);
            else if (token?.ToString() != "]")
                throw new ArgumentException(INVALID_JSON);
        }
        return result;
    }

    internal static object? GetValue(string data, ref int pos)
    {
        SkipWhitespace(data, ref pos);
        if (pos >= data.Length) throw new ArgumentException(INVALID_JSON);
        char c = data[pos];
        if (c == '}' || c == ']' || c == ':' || c == ',')
        {
            pos++;
            return c.ToString();
        }
        if (c == '"')
            return GetValueString(data, ref pos);
        if (c >= '0' && c <= '9')
            return GetValueNumber(data, ref pos);
        if (c == '{')
            return GetValueObject(data, ref pos);
        if (c == '[')
            return GetValueArray(data, ref pos);
        if (c == 'n' && GetWord(data, ref pos) == "null")
            return null;
        if (c == 't' && GetWord(data, ref pos) == "true")
            return true;
        if (c == 'f' && GetWord(data, ref pos) == "false")
            return false;
        throw new ArgumentException(INVALID_JSON);
    }

    private static string GetWord(string data, ref int pos)
    {
        StringBuilder sb = new();
        while (pos < data.Length)
        {
            char c = data[pos];
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
            {
                sb.Append(c);
                pos++;
            }
            else break;
        }
        return sb.ToString();
    }

    internal static void SkipWhitespace(string data, ref int pos)
    {
        while (pos < data.Length)
        {
            char c = data[pos];
            if (c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\f' || c == '\b')
                pos++;
            else return;
        }
    }

    internal static string GetValueString(string data, ref int pos)
    {
        bool lastSlash = false;
        pos++; // skip quote
        StringBuilder sb = new();
        while (pos < data.Length)
        {
            char c = data[pos++];
            if (lastSlash)
            {
                lastSlash = false;
                switch (c)
                {
                    case 'r': sb.Append('\r'); break;
                    case 'n': sb.Append('\n'); break;
                    case 't': sb.Append('\t'); break;
                    case 'f': sb.Append('\f'); break;
                    case 'b': sb.Append('\b'); break;
                    case 'u':
                        string value = $"{data[pos++]}{data[pos++]}{data[pos++]}{data[pos++]}";
                        sb.Append((char)uint.Parse(value, System.Globalization.NumberStyles.AllowHexSpecifier));
                        break;
                    default: sb.Append(c); break;
                }
            }
            else if (c == '\\')
                lastSlash = true;
            else if (c == '"')
                break;
            else sb.Append(c);
        }
        return sb.ToString();
    }

    internal static object GetValueNumber(string data, ref int pos)
    {
        StringBuilder sb = new();
        char c;
        while (pos < data.Length)
        {
            c = data[pos];
            if ((c >= '0' && c <= '9') || c == '.' || c == '-' || c == 'e' || c == 'E')
            {
                sb.Append(c);
                pos++;
            }
            else break;
        }
        string value = sb.ToString();
        if (value.Contains('e') || value.Contains('E'))
        {
            return double.Parse(value);
        }
        if (value.Contains('.'))
        {
            return decimal.Parse(value);
        }
        if (long.Parse(value) > int.MaxValue || long.Parse(value) < int.MinValue)
        {
            return long.Parse(value);
        }
        return int.Parse(value);
    }
}
