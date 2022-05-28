# Json Document Object Model Library

This library contains classes for in-memory representations of JsonObjects and JsonArrays.

Both JsonObject and JsonArray types can be read from string representations using `.Parse()` or output using `.ToString()`.

Calling `.ToString(true)` will return a formatted Json string using two-space indents and "\r\n" line endings.

JsonObjects use direct access: `jo["key"] = value;` and `var value = jo["key"];`. This is a non-strict implementation, so an undefined key returns a null value. The keys are case-sensitive. Keys which are null or only whitespace are not allowed.

JsonObjects can also use path access: `jo["$.key1.key2.\"key 3\".key4"] = value;`. This is enabled with `jo.AllowPaths = true;` or `JsonObject jo = new() { AllowPaths = true };` for the root object.

This implementation of `.Parse()` allows and ignores extra/trailing commas, comments (// and /**/), and all of .NET's `char.IsWhiteSpace()`. It allows and ignores "\\" in strings in front of characters which don't need to be escaped.

JsonArray is based on `List<object>` and has all the properties and methods available.

JsonObject is based on `Dictionary<string, object>` and has all the properties and methods available. `.Remove()` is overloaded to handle paths if enabled, removing the last key/value on the path.

Both have helper functions if the value's data type is known: `GetBool()` `GetString()` `GetInt()` `GetLong()` `GetDouble()` `GetDecimal()` `GetJsonArray()` `GetJsonObject()`. Provide the key for JsonObject values or the index for JsonArray values.

Because any object type is allowed to be assigned as a value, that value's `.ToString()` is used to generate the output. This can cause issues when parsing it back in, as the type may not be preserved. Specifically, date/time types will cause parsing errors and should be stored as string values.

As an optional feature, string values which begin with a letter or underline and contain only letters, digits, and underlines do not need to be quoted. This will be handled automatically in `.Parse()`. They can be output using `.ToString(*, true)`, where "\*" is true/false for formatted/unformatted output. Letters are defined by .NET's `char.IsLetter()` and digits by `char.IsDigit()`. The reserved words "null", "true", and "false" must always be quoted to be handled as strings. This should only be used when the recipient is able to handle unquoted strings. Using this feature can greatly decrease the output length.