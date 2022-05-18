namespace JsonDomLibrary;

[Serializable()]
public class JsonArray : List<object?>, IJsonClass
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

    public bool? GetBool(int index)
    {
        return (bool?)this[index];
    }

    public string? GetString(int index)
    {
        return (string?)this[index];
    }

    public int? GetInt(int index)
    {
        return (int?)this[index];
    }

    public long? GetLong(int index)
    {
        return (long?)this[index];
    }

    public double? GetDouble(int index)
    {
        return (double?)this[index];
    }

    public decimal? GetDecimal(int index)
    {
        return (decimal?)this[index];
    }

    public static JsonArray FromList(IEnumerable list)
    {
        JsonArray result = new();
        foreach (object obj in list)
            result.Add(obj);
        return result;
    }

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
                result.Append(JsonRoutines.ValueToString(value, format, indent));
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

    public static JsonArray Parse(string data)
    {
        int pos = 0;
        return JsonRoutines.GetValueArray(data, ref pos);
    }
}
