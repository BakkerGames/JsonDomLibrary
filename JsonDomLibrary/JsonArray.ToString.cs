namespace JsonDomLibrary;

public partial class JsonArray
{
    public override string ToString()
    {
        return ToString(false, false, 0);
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
        result.Append('[');
        if (Count > 0)
        {
            if (format)
            {
                indent++;
                result.AppendLine();
                result.Append(new string(' ', indent * 2));
            }
            bool comma = false;
            foreach (var value in this)
            {
                if (comma)
                {
                    result.Append(',');
                    if (format)
                    {
                        result.AppendLine();
                        result.Append(new string(' ', indent * 2));
                    }
                }
                else
                    comma = true;
                result.Append(JsonRoutines.ValueToString(value, format, unquoted, indent));
            }
            if (format)
            {
                indent--;
                result.AppendLine();
                result.Append(new string(' ', indent * 2));
            }
        }
        result.Append(']');
        return result.ToString();
    }
}
