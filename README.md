#Json Document Object Model Library

This library contains classes for in-memory representations of JsonObjects and JsonArrays.

Both JsonObject and JsonArray types can be parsed from string representations or exported using ToString(). ToString(true) will export a formatted Json document.

JsonObjects can use direct access: jo["key"] = value

JsonObjects can also use path access: jo["$.key1.key2.key3"] = value