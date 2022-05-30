# Json Document Object Model Library

This library contains classes for in-memory representations of JsonObjects and JsonArrays. They are dynamic, editable entities which can be nested to any level.

Both JsonObject and JsonArray types can be read from string representations using `.Parse()` or output using `.ToString()`.

Calling `.ToString(true)` will return a formatted Json string using two-space indents and "\r\n" line endings.

JsonArrays use indexed access: `ja[3] = value;` and `var value = ja[3];`. Accessing an index beyond the current end returns null, or fills the previous undefined elements with null. JsonArray is based on `List<object>` and has all the properties and methods available.

JsonObjects use key access: `jo["key"] = value;` and `var value = jo["key"];`. This is a non-strict implementation, so an undefined key returns a null value. The keys are case-sensitive. Keys which are null or empty are not allowed, but with no other restrictions. Duplicate keys cannot exists, and will just overwrite values. JsonObject is based on `Dictionary<string, object>` and has all the properties and methods available.

This implementation of `.Parse()` is highly permissive. It allows and ignores extra/trailing commas, comments (`//` and `/**/`), and all of .NET's `char.IsWhiteSpace()`. It allows and ignores `\` in strings in front of characters which don't need to be escaped. Keys and string values with only letters, digits, and underlines are parsed properly even if not surrounded by quotes (see below).

## Additional features

Both JsonArrays and JsonObjects have casting functions if the value's data type is known: `GetBool()` `GetString()` `GetInt()` `GetLong()` `GetDouble()` `GetDecimal()` `GetJsonArray()` `GetJsonObject()`. Provide the key for JsonObject values or the index for JsonArray values. All return nullable values.

Both JsonArrays and JsonObjects may be sent multiple keys/indexes when accessing. This creates a nested structure in JsonObjects, or multi-dimentional arrays in JsonArrays. Examples are `jo["key1", "key2", "key3"] = value;` and `ja[4, 7] = value;`. Multiple keys/indexes can be used in all the casting functions too. Arrays of `string[]` or `int[]` can be used instead.

Simple keys and string values which begin with a letter or underline and contain only letters, digits, and underlines will be handled properly by `.Parse()` if not surrounded by quotes. The function `.ToString(*, true)` (where `*` is true/false for formatted/unformatted output) will not add quotes around any of these simple keys/strings in the output. Letters are defined by .NET's `char.IsLetter()` and digits by `char.IsDigit()`. The reserved words "null", "true", and "false" must always be quoted to be handled as strings. This should only be used when the recipient is able to handle unquoted strings, but using this feature can significantly decrease the output length.

## Notes

Because any .NET object type is allowed to be assigned as a value, an unknown type's `.ToString()` is used to generate the output. This can cause issues when parsing the output back in, as it may throw errors, or at the very least the object type can be lost. Specifically, date/time, Guids, and many others should be stored as strings. The receipient would have to reconstruct the original types as appropriate.