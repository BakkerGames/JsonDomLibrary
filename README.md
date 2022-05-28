# Json Document Object Model Library

This library contains classes for in-memory representations of JsonObjects and JsonArrays.

Both JsonObject and JsonArray types can be read from string representations using Parse() or output using ToString().

Calling ToString(true) will return a formatted Json string using two-space indents and "\r\n" line endings.

JsonObjects use direct access: jo["key"] = value;

JsonObjects can also use path access: jo["$.key1.key2.\"key 3\".key4"] = value;

This implementation of Parse() allows and ignores extra/trailing commas, comments (// and /**/), and all char.IsWhiteSpace().

As an additional feature, string values which begin with a letter or underline and contain only letters, digits, and underlines do not need to be quoted. These will be handled automatically in Parse(). They can be output using ToString(*, true), where "*" is true/false for formatted/unformatted output. Letters are defined by char.IsLetter() and digits by char.IsDigit(). The string values "null", "true", and "false" must be quoted in order to be handled as strings.

JsonObject is based on Dictionary<string, object> and has all the properties and methods available.

JsonArray is based on List<object> and has all the properties and methods available.

Both have helper functions if the value's data type is known:
    public bool? GetBool()
    public string? GetString()
    public int? GetInt()
    public long? GetLong()
    public double? GetDouble()
    public decimal? GetDecimal()
    public JsonArray GetJsonArray()
    public JsonObject GetJsonObject()
