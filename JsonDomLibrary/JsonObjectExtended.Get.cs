namespace JsonDomLibrary;

public partial class JsonObjectExtended
{
    public bool? GetBool(params string[] keys)
    {
        return (bool?)this[keys];
    }

    public bool GetBoolOrDefault(params string[] keys)
    {
        return (bool?)this[keys] ?? false;
    }

    public string? GetString(params string[] keys)
    {
        return (string?)this[keys];
    }

    public string GetStringOrDefault(params string[] keys)
    {
        return (string?)this[keys] ?? "";
    }

    public int? GetInt(params string[] keys)
    {
        var value = this[keys];
        return value == null ? null : int.Parse(value.ToString());
    }

    public int GetIntOrDefault(params string[] keys)
    {
        var value = this[keys];
        return value == null ? 0 : int.Parse(value.ToString());
    }

    public long? GetLong(params string[] keys)
    {
        var value = this[keys];
        return value == null ? null : long.Parse(value.ToString());
    }

    public long GetLongOrDefault(params string[] keys)
    {
        var value = this[keys];
        return value == null ? 0L : long.Parse(value.ToString());
    }

    public double? GetDouble(params string[] keys)
    {
        var value = this[keys];
        return value == null ? null : double.Parse(value.ToString());
    }

    public double GetDoubleOrDefault(params string[] keys)
    {
        var value = this[keys];
        return value == null ? 0D : double.Parse(value.ToString());
    }

    public decimal? GetDecimal(params string[] keys)
    {
        var value = this[keys];
        return value == null ? null : decimal.Parse(value.ToString());
    }

    public decimal GetDecimalOrDefault(params string[] keys)
    {
        var value = this[keys];
        return value == null ? 0M : decimal.Parse(value.ToString());
    }

    public JsonArray? GetJsonArray(params string[] keys)
    {
        return (JsonArray?)this[keys];
    }

    public JsonArray GetJsonArrayOrDefault(params string[] keys)
    {
        return (JsonArray?)this[keys] ?? new JsonArray();
    }

    public JsonObject? GetJsonObject(params string[] keys)
    {
        return (JsonObject?)this[keys];
    }

    public JsonObject GetJsonObjectOrDefault(params string[] keys)
    {
        return (JsonObject?)this[keys] ?? new JsonObject();
    }
}