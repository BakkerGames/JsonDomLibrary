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
            if (((string)value).Contains('"'))
                return $"\"{((string)value).Replace("\"", "\\\"")}\"";
            return $"\"{value}\"";
        }
        if (type == typeof(JsonObject))
            return ((JsonObject)value).ToString(format, indent);
        if (type == typeof(JsonArray))
            return ((JsonArray)value).ToString(format, indent);
        return value.ToString();
    }
}
