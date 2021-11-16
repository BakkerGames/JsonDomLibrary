namespace JsonDomLibrary;

public abstract class JsonBaseClass
{
    protected static string? ValueToString(object? value, bool format = false, int indent = 0)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        if (value == null)
            return "null";
        Type type = value.GetType();
        if (type == typeof(bool))
            return value.ToString().ToLower();
        if (type == typeof(string))
        {
            if (((string)value).Contains('"'))
                return $"\"{((string)value).Replace("\"", "\\\"")}\"";
            return $"\"{value}\"";
        }
        if (type == typeof(JsonObject))
            return ((JsonObject)value).ToString(format, indent);
        return value.ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}
