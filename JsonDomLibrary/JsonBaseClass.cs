namespace JsonDomLibrary;

public abstract class JsonBaseClass
{
    protected static string? ValueToString(object? value)
    {
        return ValueToString(value, false, 0);
    }

    protected static string? ValueToString(object? value, bool format, int indent)
    {
        if (value == null)
            return "null";
        Type type = value.GetType();
        if (type == typeof(bool))
            return value.ToString()!.ToLower();
        if (type == typeof(string))
        {
            return $"\"{ToJsonString((string)value)}\"";
        }
        if (type == typeof(JsonObject))
            return ((JsonObject)value).ToString(format, indent);
        if (type == typeof(JsonArray))
            return ((JsonArray)value).ToString(format, indent);
        return value.ToString();
    }

    private static string ToJsonString(string s)
    {
        StringBuilder sb = new StringBuilder();
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
}
