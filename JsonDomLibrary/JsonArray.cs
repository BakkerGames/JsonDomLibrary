namespace JsonDomLibrary;

public class JsonArray : List<object?>
{
    public JsonArray()
    {
    }

    public JsonArray GetJsonArray(int index)
    {
        return (JsonArray)(this[index] ?? new JsonArray());
    }

    public JsonObject GetJsonObject(int index)
    {
        return (JsonObject)(this[index] ?? new JsonObject());
    }

    #region ToString

    public override string ToString()
    {
        return ToString(false, 0);
    }

    public string ToString(bool format)
    {
        return ToString(format, 0);
    }

    public string ToString(bool format, int indent)
    {
        StringBuilder result = new();
        result.Append('[');
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
            result.Append(JsonBaseClass.ValueToString(value, format, indent));
        }
        if (format)
        {
            indent--;
            result.AppendLine();
            result.Append(new string(' ', indent * 2));
        }
        result.Append(']');
        return result.ToString();
    }

    #endregion

    #region Parse

    public static JsonArray Parse(string data)
    {
        int pos = 0;
        return JsonBaseClass.GetValueArray(data, ref pos);
    }

    #endregion

}
