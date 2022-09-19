using JsonDomLibrary;
using Xunit;

namespace JsonDomLibraryTests;

public class UnitTestJsonObject
{
    [Fact]
    public void Test_JsonObject_AnyNull()
    {
        JsonObject jo = new();
        Assert.NotNull(jo);
        Assert.IsType<JsonObject>(jo);
        Assert.Null(jo["abc"]);
    }

    [Fact]
    public void Test_JsonObject_GetKey_Null()
    {
        JsonObject jo = new();
        Assert.Null(jo["abc"]);
    }

    [Fact]
    public void Test_JsonObject_GetSet_Value()
    {
        JsonObject jo = new();
        var key = "abc";
        var expected = "123";
        jo[key] = expected;
        Assert.Equal(expected, jo[key]);
    }

    [Fact]
    public void Test_JsonObject_ToString_Null()
    {
        JsonObject jo = new();
        jo["abc"] = null;
        var expected = "{\"abc\":null}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToString_True()
    {
        JsonObject jo = new();
        jo["abc"] = true;
        var expected = "{\"abc\":true}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToString_False()
    {
        JsonObject jo = new();
        jo["abc"] = false;
        var expected = "{\"abc\":false}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToString_Number()
    {
        JsonObject jo = new();
        jo["abc"] = 123;
        var expected = "{\"abc\":123}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToString_Decimal()
    {
        JsonObject jo = new();
        jo["abc"] = 123.456;
        var expected = "{\"abc\":123.456}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToString_Decimal_NoLeadingZero()
    {
        JsonObject jo = new();
        jo["abc"] = .456;
        var expected = "{\"abc\":0.456}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToString_Decimal_NoLeadingZeroNegative()
    {
        JsonObject jo = new();
        jo["abc"] = -.456;
        var expected = "{\"abc\":-0.456}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_Parse_Decimal_NoLeadingZeroNegative()
    {
        var expected = "{\"abc\":-0.456}";
        JsonObject jo = JsonObject.Parse(expected);
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToString_String()
    {
        JsonObject jo = new();
        jo["abc"] = "123";
        var expected = "{\"abc\":\"123\"}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToString_String_Empty()
    {
        JsonObject jo = new();
        jo["abc"] = "";
        var expected = "{\"abc\":\"\"}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToString_String_Quotes()
    {
        JsonObject jo = new();
        jo["abc"] = "\"def\"";
        var expected = "{\"abc\":\"\\\"def\\\"\"}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToString_String_Unquoted()
    {
        JsonObject jo = new();
        jo["abc"] = "def";
        var expected = "{abc:def}";
        var actual = jo.ToString(false, true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToString_JsonObject_Empty()
    {
        JsonObject jo = new();
        jo["abc"] = new JsonObject();
        var expected = "{\"abc\":{}}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToString_JsonObject_Value()
    {
        JsonObject jo = new();
        JsonObject jo1 = new();
        jo1["def"] = 123;
        jo["abc"] = jo1;
        var expected = "{\"abc\":{\"def\":123}}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToStringFormatted()
    {
        JsonObject jo = new();
        jo["abc", "def", "ghi"] = 123;
        jo["abc", "def", "xyz"] = new JsonObject();
        var expected = "{\r\n  \"abc\": {\r\n    \"def\": {\r\n      \"ghi\": 123,\r\n      \"xyz\": {}\r\n    }\r\n  }\r\n}";
        var actual = jo.ToString(true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_Remove()
    {
        JsonObject jo = new();
        jo["abc"] = 123;
        jo["def"] = 456;
        jo["ghi"] = 789;
        jo.Remove("def");
        var expected = "{\"abc\":123,\"ghi\":789}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_Count()
    {
        JsonObject jo = new();
        jo["abc"] = 123;
        jo["def"] = 456;
        jo["ghi"] = 789;
        var expected = 3;
        var actual = jo.Count;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_CaseSensitive()
    {
        JsonObject jo = new();
        jo["abc"] = 123;
        var expected = 123;
        var actual = jo["ABC"];
        Assert.NotEqual(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_Format()
    {
        JsonObject jo = new();
        jo["abc", "def", "ghi"] = 123;
        var expected = "{\r\n  \"abc\": {\r\n    \"def\": {\r\n      \"ghi\": 123\r\n    }\r\n  }\r\n}";
        var actual = jo.ToString(true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_AddJsonArray()
    {
        JsonObject jo = new();
        jo["abc"] = 123;
        jo["list"] = new JsonArray();
        jo.GetJsonArray("list")?.Add("xyz");
        var expected = "{\"abc\":123,\"list\":[\"xyz\"]}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_Parse()
    {
        var expected = "{\"a\":123,\"b\":\"string\",\"c\":null,\"d\":true,\"e\":false,\"f\":{\"x\":999},\"g\":[3.14]}";
        JsonObject jo = JsonObject.Parse(expected);
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_Parse_Whitespace()
    {
        var initial = "  {" +
            "  \"a\"  :  123  ," +
            "  \"b\"  :  \"string\"  ," +
            "  \"c\"  :  null  ," +
            "  \"d\"  :  true  ," +
            "  \"e\"  :  false  ," +
            "  \"f\"  :  {  \"x\"  :  999  }  ," +
            "  \"g\"  :  [  3.14  ]  " +
            "  }  ";
        JsonObject jo = JsonObject.Parse(initial);
        var expected = "{\"a\":123,\"b\":\"string\",\"c\":null,\"d\":true,\"e\":false,\"f\":{\"x\":999},\"g\":[3.14]}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_Parse_Whitespace_Comments()
    {
        var initial = "/*beginning*/  {" +
            "  \"a\"  :  123  , //hello!\n" +
            "  \"b\"  :  \"string\" /* skip this */ ," +
            "  \"c\"  :  null  ," +
            "/* skip me too \r\n hi! */  \"d\"  :  true // skip me ,\r" +
            " , \"e\"  :  false  ," +
            "  \"f\"  :  {  \"x\"  :  999  }  ," +
            "  \"g\"  :  [  3.14  ]  " +
            "  }  // and check the end";
        JsonObject jo = JsonObject.Parse(initial);
        var expected = "{\"a\":123,\"b\":\"string\",\"c\":null,\"d\":true,\"e\":false,\"f\":{\"x\":999},\"g\":[3.14]}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_Parse_IgnoreExtraCommas()
    {
        var initial = "{,,\"key1\":1,,,\"key2\":2,,}";
        JsonObject jo = JsonObject.Parse(initial);
        var expected = "{\"key1\":1,\"key2\":2}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_Parse_ErrorUnterminated()
    {
        var initial = "{\"key1\":1,\"key2\":2";
        Assert.ThrowsAny<System.Exception>(() => { JsonObject.Parse(initial); });
    }

    [Fact]
    public void Test_JsonObject_Parse_ErrorNoValue()
    {
        var initial = "{\"key1\",\"key2\"}";
        Assert.ThrowsAny<System.Exception>(() => { JsonObject.Parse(initial); });
    }

    [Fact]
    public void Test_JsonObject_Params_Set()
    {
        JsonObject jo = new();
        jo["a", "b", "c"] = "abc";
        var expected = "{\"a\":{\"b\":{\"c\":\"abc\"}}}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_Params_Get()
    {
        JsonObject jo = new();
        var initial = "abc";
        jo["a", "b", "c"] = initial;
        var expected = initial;
        var actual = jo["a", "b", "c"];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_GetInt()
    {
        int? expected = 0;
        string initial = $"{{\"key\":{expected}}}";
        JsonObject jo = JsonObject.Parse(initial);
        var actual = jo.GetInt("key");
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_GetLong()
    {
        long? expected = 0;
        string initial = $"{{\"key\":{expected}}}";
        JsonObject jo = JsonObject.Parse(initial);
        var actual = jo.GetLong("key");
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_GetDouble()
    {
        double? expected = 0;
        string initial = $"{{\"key\":{expected}}}";
        JsonObject jo = JsonObject.Parse(initial);
        var actual = jo.GetDouble("key");
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_GetDecimal()
    {
        decimal? expected = 0;
        string initial = $"{{\"key\":{expected}}}";
        JsonObject jo = JsonObject.Parse(initial);
        var actual = jo.GetDecimal("key");
        Assert.Equal(expected, actual);
    }
}