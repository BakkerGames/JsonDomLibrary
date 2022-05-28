namespace JsonDomLibrary;

public static partial class JsonRoutines
{
    private const string INVALID_JSON = "Invalid json";

    internal static JsonObject GetValueObject(string data, ref int pos)
    {
        JsonObject result = new();
        SkipWhitespace(data, ref pos);
        if (data[pos] != '{')
            throw new ArgumentException($"{INVALID_JSON} - pos:{pos} - {data[pos]}");
        pos++; // skip initial {
        object? token = GetValue(data, ref pos);
        while (token?.ToString() == ",") // skip any leading commas
            token = GetValue(data, ref pos);
        while (token?.ToString() != "}")
        {
            if (GetValue(data, ref pos)?.ToString() != ":")
                throw new ArgumentException($"{INVALID_JSON} - pos:{pos} - {token}");
            object? value = GetValue(data, ref pos);
            result[token?.ToString()] = value;
            token = GetValue(data, ref pos);
            if (token?.ToString() != "," && token?.ToString() != "}")
                throw new ArgumentException($"{INVALID_JSON} - pos:{pos} - {token}");
            while (token?.ToString() == ",") // skip any extra/trailing commas
                token = GetValue(data, ref pos);
        }
        return result;
    }

    internal static JsonArray GetValueArray(string data, ref int pos)
    {
        JsonArray result = new();
        SkipWhitespace(data, ref pos);
        if (data[pos] != '[')
            throw new ArgumentException($"{INVALID_JSON} - pos:{pos} - {data[pos]}");
        pos++; // skip initial [
        object? token = GetValue(data, ref pos);
        while (token?.ToString() == ",") // skip any leading commas
            token = GetValue(data, ref pos);
        while (token?.ToString() != "]")
        {
            result.Add(token);
            token = GetValue(data, ref pos);
            if (token?.ToString() != "," && token?.ToString() != "]")
                throw new ArgumentException($"{INVALID_JSON} - pos:{pos} - {token}");
            while (token?.ToString() == ",") // skip any extra/trailing commas
                token = GetValue(data, ref pos);
        }
        return result;
    }

    private static object? GetValue(string data, ref int pos)
    {
        SkipWhitespace(data, ref pos);
        if (pos >= data.Length)
            throw new ArgumentException($"{INVALID_JSON} - pos:{pos}");
        char c = data[pos];
        if (c == '}' || c == ']' || c == ':' || c == ',')
        {
            pos++;
            return c.ToString();
        }
        if (c == '"')
            return GetValueString(data, ref pos);
        if ((c >= '0' && c <= '9') || c == '-' || c == '.')
            return GetValueNumber(data, ref pos);
        if (c == '{')
            return GetValueObject(data, ref pos);
        if (c == '[')
            return GetValueArray(data, ref pos);
        // look for any value starting with a letter or underline
        if (char.IsLetter(c) || c == '_')
        {
            string word = GetWord(data, ref pos);
            if (word == "null")
                return null;
            if (word == "true")
                return true;
            if (word == "false")
                return false;
            // return string value that didn't have quotes
            return word;
        }
        throw new ArgumentException($"{INVALID_JSON} - pos:{pos} - {c}");
    }

    private static string GetWord(string data, ref int pos)
    {
        StringBuilder sb = new();
        while (pos < data.Length)
        {
            char c = data[pos];
            if (char.IsLetterOrDigit(c) || c == '_')
            {
                sb.Append(c);
                pos++;
            }
            else break;
        }
        return sb.ToString();
    }

    private static void SkipWhitespace(string data, ref int pos)
    {
        bool lastSlash = false;
        bool lastAsterisk = false;
        bool inLineComment = false;
        bool inMultiComment = false;
        while (pos < data.Length)
        {
            char c = data[pos++];
            if (lastSlash)
            {
                lastSlash = false;
                if (c == '/')
                {
                    inLineComment = true;
                    continue;
                }
                if (c == '*')
                {
                    inMultiComment = true;
                    continue;
                }
                throw new ArgumentException($"{INVALID_JSON} - pos:{pos} - {c}");
            }
            if (inLineComment)
            {
                if (c == '\r' || c == '\n')
                {
                    inLineComment = false;
                }
                continue;
            }
            if (inMultiComment)
            {
                if (c == '*')
                {
                    lastAsterisk = true;
                    continue;
                }
                if (c == '/' && lastAsterisk)
                {
                    inMultiComment = false;
                }
                lastAsterisk = false;
                continue;
            }
            if (c == '/')
            {
                lastSlash = true;
                continue;
            }
            if (char.IsWhiteSpace(c))
            {
                continue;
            }
            pos--;
            return;
        }
    }

    private static string GetValueString(string data, ref int pos)
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
                    default:
                        // just append the character and don't throw an error
                        sb.Append(c);
                        break;
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

    private static object GetValueNumber(string data, ref int pos)
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
