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
            return @class.ToString(format, indent);
        return value.ToString();
    }

    private static bool IsValidUnquotedString(string value)
    {
        // look for a string which can be unquoted
        char c = value[0];
        if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c == '_'))
        {
            foreach (char c1 in value)
            {
                if (c1 >= 'a' && c1 <= 'z') continue;
                if (c1 >= 'A' && c1 <= 'Z') continue;
                if (c1 >= '0' && c1 <= '9') continue;
                if (c1 == '_') continue;
                return false;
            }
            return true;
        }
        return false;
    }

    #region private

    internal static string ToJsonString(string s)
    {
        StringBuilder sb = new();
        bool lastBackslash = false;
        foreach (char c in s)
        {
            if (!lastBackslash && c == '\\')
            {
                lastBackslash = true;
                continue;
            }
            if (c == '\\' || c == '"' || c == 8 || c == 9 || c == 10 || c == 12 || c == 13)
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
            else if (c == '"')
            {
                sb.Append("\\\"");
            }
            else
            {
                sb.Append(c);
            }
            lastBackslash = false;
        }
        return sb.ToString();
    }

    #endregion
}
