namespace JsonDomLibrary;

public static partial class JsonRoutines
{
    public static string? ValueToString(object? value)
    {
        return ValueToString(value, false, false, 0);
    }

    public static string? ValueToString(object? value, bool format, bool unquoted, int indent)
    {
        if (value == null)
            return "null";
        Type type = value.GetType();
        if (type == typeof(bool))
            return value.ToString()!.ToLower();
        if (type == typeof(string))
        {
            if (unquoted && IsValidUnquotedString((string)value))
            {
                return (string)value;
            }
            return $"\"{ToJsonString((string)value)}\"";
        }
        // handle JsonArray, JsonObject, and any derived types
        if (value is IJsonClass @class)
        {
            return @class.ToString(format, indent);
        }
        // check for dictionary
        if (value is IDictionary dictionary)
        {
            JsonObject jo = new();
            foreach (DictionaryEntry kv in dictionary)
            {
                // the key has to be converted to a string
                jo.Add(kv.Key.ToString(), kv.Value);
            }
            return jo.ToString(format, unquoted, indent);
        }
        // check for any type of list
        if (value is IEnumerable enumerable)
        {
            JsonArray ja = new();
            foreach (object? o in enumerable)
            {
                ja.Add(o);
            }
            return ja.ToString(format, unquoted, indent);
        }
        // number or some unknown type
        return value.ToString();
    }

    private static bool IsValidUnquotedString(string value)
    {
        // these always have to be quoted, ignore case
        if (value.ToLower() == "null" ||
            value.ToLower() == "true" ||
            value.ToLower() == "false")
            return false;
        // look for a string which can be unquoted
        bool first = true;
        foreach (char c in value)
        {
            if (first)
            {
                if (!char.IsLetter(c) && c != '_')
                    return false;
                first = false;
            }
            else if (!char.IsLetterOrDigit(c) && c != '_')
                return false;
        }
        return true;
    }

    #region private

    internal static string ToJsonString(string s)
    {
        StringBuilder sb = new();
        foreach (char c in s)
        {
            if (c == 8 || c == 9 || c == 10 || c == 12 || c == 13 || c == '"' || c == '\\')
            {
                switch (c)
                {
                    case (char)8:
                        sb.Append("\\b");
                        break;
                    case (char)9:
                        sb.Append("\\t");
                        break;
                    case (char)10:
                        sb.Append("\\n");
                        break;
                    case (char)12:
                        sb.Append("\\f");
                        break;
                    case (char)13:
                        sb.Append("\\r");
                        break;
                    default:
                        sb.Append($"\\{c}");
                        break;
                }
            }
            else if (c < 32 || c == 127 || c == 129 || c == 141 || c == 143 || c == 144 || c == 157 || c > 255)
            {
                sb.Append($"\\u{(int)c:x4}");
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    #endregion
}
