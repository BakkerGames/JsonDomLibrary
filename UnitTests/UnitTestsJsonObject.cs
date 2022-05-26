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
    public void Test_JsonObject_GetPath_Null()
    {
        JsonObject jo = new();
        var value = jo["$.level1.level2.level3"];
        Assert.Null(value);
    }

    [Fact]
    public void Test_JsonObject_GetPath_JsonArray()
    {
        JsonObject jo1 = new();
        JsonArray ja2 = new();
        JsonObject jo3 = new();
        jo3["level3"] = 123;
        ja2.Add(null);
        ja2.Add(null);
        ja2.Add(jo3);
        jo1["level1"] = ja2;
        var value = jo1["$.level1.2.level3"];
        Assert.Equal(123, value);
    }

    [Fact]
    public void Test_JsonObject_SetPath_JsonArray()
    {
        JsonObject jo1 = new();
        JsonArray ja2 = new();
        JsonObject jo3 = new();
        jo3["level3"] = null;
        ja2.Add(null);
        ja2.Add(null);
        ja2.Add(jo3);
        jo1["level1"] = ja2;
        jo1["$.level1.2.level3"] = 123;
        var value = jo1["$.level1.2.level3"];
        Assert.Equal(123, value);
    }

    [Fact]
    public void Test_JsonObject_SetPathGetPathEscaped_Value()
    {
        JsonObject jo = new();
        var path = "$.\"level 1\".\"level.2\".\"  level3  \"";
        var origValue = "ABC";
        jo[path] = origValue;
        var newValue = jo[path];
        Assert.Equal(origValue, newValue);
    }

    [Fact]
    public void Test_JsonObject_SetPathGetPath_Value()
    {
        JsonObject jo = new();
        var path = "$.level1.level2.level3";
        var origValue = "ABC";
        jo[path] = origValue;
        var newValue = jo[path];
        Assert.Equal(origValue, newValue);
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
    public void Test_JsonObject_ToString_JsonObject_Path()
    {
        JsonObject jo = new();
        jo["$.abc.def.ghi"] = 123;
        var expected = "{\"abc\":{\"def\":{\"ghi\":123}}}";
        var actual = jo.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonObject_ToStringFormatted()
    {
        JsonObject jo = new();
        jo["$.abc.def.ghi"] = 123;
        jo["$.abc.def.xyz"] = new JsonObject();
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
    public void Test_JsonObject_Remove_JsonObject_Path()
    {
        JsonObject jo = new();
        jo["$.abc.def.ghi"] = 123;
        jo.Remove("$.abc.def.ghi");
        var expected = "{\"abc\":{\"def\":{}}}";
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
        jo["$.abc.def.ghi"] = 123;
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
        ((JsonArray?)jo["list"])?.Add("xyz");
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
    public void Test_JsonObject_Parse_ErrorExtraCommas()
    {
        var initial = "{\"key1\":1,,,\"key2\":2}";
        Assert.ThrowsAny<System.Exception>(() => { JsonObject.Parse(initial); });
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
}
