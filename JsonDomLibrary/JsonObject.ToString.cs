namespace JsonDomLibrary;

public partial class JsonObject
{
    public override string ToString()
    {
        return ToString(false, 0);
    }

    public string ToString(bool format)
    {
        return ToString(format, false, 0);
    }

    public string ToString(bool format, bool unquoted)
    {
        return ToString(format, unquoted, 0);
    }

    public string ToString(bool format, int indent)
    {
        return ToString(format, false, indent);
    }

    public string ToString(bool format, bool unquoted, int indent)
    {
        StringBuilder result = new();
        result.Append('{');
        if (Count > 0)
        {
            if (format)
            {
                indent++;
                result.AppendLine();
                result.Append(new string(' ', indent * 2));
            }
            bool addComma = false;
            foreach (var kv in this)
            {
                if (addComma)
                {
                    result.Append(',');
                    if (format)
                    {
                        result.AppendLine();
                        result.Append(new string(' ', indent * 2));
                    }
                }
                else
                {
                    addComma = true;
                }
                result.Append(JsonRoutines.ValueToString(kv.Key, format, unquoted, indent));
                result.Append(':');
                if (format)
                    result.Append(' ');
                result.Append(JsonRoutines.ValueToString(kv.Value, format, unquoted, indent));
            }
            if (format)
            {
                indent--;
                result.AppendLine();
                result.Append(new string(' ', indent * 2));
            }
        }
        result.Append('}');
        return result.ToString();
    }
}
