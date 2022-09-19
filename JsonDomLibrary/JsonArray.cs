namespace JsonDomLibrary;

[Serializable()]
public partial class JsonArray : List<object?>, IJsonClass
{
    public object? this[params int[] indexes]
    {
        get
        {
            if (indexes.Length == 0)
                return null;
            if (indexes[0] < 0)
                throw new ArgumentOutOfRangeException(nameof(indexes), indexes[0].ToString());
            if (indexes.Length == 1)
            {
                if (indexes[0] < Count)
                    return base[indexes[0]];
                return null;
            }
            JsonArray? ja = (JsonArray?)this[indexes[0]]; // this[] can return null
            if (ja == null) return null;
            List<int> newIndexes = indexes.ToList();
            newIndexes.RemoveAt(0);
            return ja[newIndexes.ToArray()];
        }
        set
        {
            if (indexes.Length == 0)
                return;
            if (indexes[0] < 0)
                throw new ArgumentOutOfRangeException(nameof(indexes), indexes[0].ToString());
            if (indexes.Length == 1)
            {
                if (indexes[0] < Count)
                    base[indexes[0]] = value;
                else
                {
                    while (Count < indexes[0])
                        Add(null);
                    Add(value);
                }
                return;
            }
            JsonArray? ja = (JsonArray?)this[indexes[0]]; // this[] can return null
            if (ja == null)
            {
                while (Count <= indexes[0])
                    Add(null);
                ja = new();
                base[indexes[0]] = ja;
            }
            List<int> newIndexes = indexes.ToList();
            newIndexes.RemoveAt(0);
            ja[newIndexes.ToArray()] = value;
        }
    }

    public bool? GetBool(params int[] indexes)
    {
        return (bool?)this[indexes];
    }

    public string? GetString(params int[] indexes)
    {
        return (string?)this[indexes];
    }

    public int? GetInt(params int[] indexes)
    {
        var value = this[indexes];
        if (value == null) return null;
        return int.Parse(value.ToString());
    }

    public long? GetLong(params int[] indexes)
    {
        var value = this[indexes];
        if (value == null) return null;
        return long.Parse(value.ToString());
    }

    public double? GetDouble(params int[] indexes)
    {
        var value = this[indexes];
        if (value == null) return null;
        return double.Parse(value.ToString());
    }

    public decimal? GetDecimal(params int[] indexes)
    {
        var value = this[indexes];
        if (value == null) return null;
        return decimal.Parse(value.ToString());
    }

    public JsonArray? GetJsonArray(params int[] indexes)
    {
        return (JsonArray?)this[indexes];
    }

    public JsonObject? GetJsonObject(params int[] indexes)
    {
        return (JsonObject?)this[indexes];
    }

    public static JsonArray FromList(IEnumerable list)
    {
        JsonArray result = new();
        foreach (object obj in list)
            result.Add(obj);
        return result;
    }

    public static JsonArray Parse(string data)
    {
        int pos = 0;
        return JsonRoutines.GetValueArray(data, ref pos);
    }
}
