namespace JsonDomLibrary
{
    internal static class Routines
    {
        public static string? ValueToString(object? value)
        {
            if (value == null)
                return "null";
            Type type = value.GetType();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (type == typeof(bool))
                return value.ToString().ToLower();
            if (type == typeof(string))
            {
                if (((string)value).Contains('"'))
                    return $"\"{((string)value).Replace("\"", "\\\"")}\"";
                return $"\"{value}\"";
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return value.ToString();
        }
    }
}
