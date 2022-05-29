namespace JsonDomLibrary;

public partial class JsonObject : Dictionary<string, object?>, IJsonClass
{
    public object? this[params string[] keys]
    {
        get
        {
            if (keys.Length == 0)
                return null;
            if (string.IsNullOrEmpty(keys[0]))
                throw new ArgumentNullException(nameof(keys));
            if (keys.Length == 1)
            {
                if (ContainsKey(keys[0]))
                    return base[keys[0]];
                return null;
            }
            JsonObject? jo = (JsonObject?)base[keys[0]];
            if (jo == null) return null;
            List<string> newKeys = keys.ToList();
            newKeys.RemoveAt(0);
            return jo[newKeys.ToArray()];
        }
        set
        {
            if (keys.Length == 0)
                return;
            if (string.IsNullOrEmpty(keys[0]))
                throw new ArgumentNullException(nameof(keys));
            if (keys.Length == 1)
            {
                if (ContainsKey(keys[0]))
                    base[keys[0]] = value;
                else
                    Add(keys[0], value);
                return;
            }
            JsonObject? jo = (JsonObject?)base[keys[0]];
            if (jo == null) {
                jo = new();
                base[keys[0]] = jo;
            }
            List<string> newKeys = keys.ToList();
            newKeys.RemoveAt(0);
            jo[newKeys.ToArray()] = value;
        }
    }

    public bool? GetBool(params string[] keys)
    {
        return (bool?)this[keys];
    }

    public string? GetString(params string[] keys)
    {
        return (string?)this[keys];
    }

    public int? GetInt(params string[] keys)
    {
        return (int?)this[keys];
    }

    public long? GetLong(params string[] keys)
    {
        return (long?)this[keys];
    }

    public double? GetDouble(params string[] keys)
    {
        return (double?)this[keys];
    }

    public decimal? GetDecimal(params string[] keys)
    {
        return (decimal?)this[keys];
    }

    public JsonArray? GetJsonArray(params string[] keys)
    {
        return (JsonArray?)this[keys];
    }

    public JsonObject? GetJsonObject(params string[] keys)
    {
        return (JsonObject?)this[keys];
    }

    public static JsonObject Parse(string data)
    {
        int pos = 0;
        return JsonRoutines.GetValueObject(data, ref pos);
    }
}
