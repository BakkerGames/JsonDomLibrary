namespace JsonDomLibrary;

public partial class JsonArray : List<object?>, IJsonClass
{
    public bool? GetBool(params int[] indexes)
    {
        return (bool?)this[indexes];
    }

    public bool GetBoolOrDefault(params int[] indexes)
    {
        return (bool?)this[indexes] ?? false;
    }

    public string? GetString(params int[] indexes)
    {
        return (string?)this[indexes];
    }

    public string GetStringOrDefault(params int[] indexes)
    {
        return (string?)this[indexes] ?? "";
    }

    public int? GetInt(params int[] indexes)
    {
        var value = this[indexes];
        return value == null ? null : int.Parse(value.ToString());
    }

    public int GetIntOrDefault(params int[] indexes)
    {
        var value = this[indexes];
        return value == null ? 0 : int.Parse(value.ToString());
    }

    public long? GetLong(params int[] indexes)
    {
        var value = this[indexes];
        return value == null ? null : long.Parse(value.ToString());
    }

    public long GetLongOrDefault(params int[] indexes)
    {
        var value = this[indexes];
        return value == null ? 0L : long.Parse(value.ToString());
    }

    public double? GetDouble(params int[] indexes)
    {
        var value = this[indexes];
        return value == null ? null : double.Parse(value.ToString());
    }

    public double GetDoubleOrDefault(params int[] indexes)
    {
        var value = this[indexes];
        return value == null ? 0D : double.Parse(value.ToString());
    }

    public decimal? GetDecimal(params int[] indexes)
    {
        var value = this[indexes];
        return value == null ? null : decimal.Parse(value.ToString());
    }

    public decimal GetDecimalOrDefault(params int[] indexes)
    {
        var value = this[indexes];
        return value == null ? 0M : decimal.Parse(value.ToString());
    }

    public JsonArray? GetJsonArray(params int[] indexes)
    {
        return (JsonArray?)this[indexes];
    }

    public JsonArray GetJsonArrayOrDefault(params int[] indexes)
    {
        return (JsonArray?)this[indexes] ?? new JsonArray();
    }

    public JsonObject? GetJsonObject(params int[] indexes)
    {
        return (JsonObject?)this[indexes];
    }

    public JsonObject GetJsonObjectOrDefault(params int[] indexes)
    {
        return (JsonObject?)this[indexes] ?? new JsonObject();
    }
}