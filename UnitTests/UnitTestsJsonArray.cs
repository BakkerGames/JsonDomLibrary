using JsonDomLibrary;
using Xunit;

namespace JsonDomLibraryTests;

public class UnitTestsJsonArray
{
    [Fact]
    public void Test_JsonArray_CountZero()
    {
        JsonArray ja = new();
        Assert.Equal(0, ja.Count());
    }

    [Fact]
    public void Test_JsonArray_CountThree()
    {
        JsonArray ja = new();
        ja.Add(1);
        ja.Add(2);
        ja.Add(3);
        Assert.Equal(3, ja.Count());
    }

    [Fact]
    public void Test_JsonArray_ValueMatch()
    {
        JsonArray ja = new();
        ja.Add(1);
        var expected = 1;
        var actual = ja[0];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonArray_ToString_Empty()
    {
        JsonArray ja = new();
        var expected = "[]";
        var actual = ja.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonArray_ToString_Numbers()
    {
        JsonArray ja = new();
        ja.Add(1);
        ja.Add(2);
        ja.Add(3);
        var expected = "[1,2,3]";
        var actual = ja.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonArray_ToString_Strings()
    {
        JsonArray ja = new();
        ja.Add("abc");
        ja.Add("DEF");
        ja.Add("ghi");
        var expected = "[\"abc\",\"DEF\",\"ghi\"]";
        var actual = ja.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonArray_ToString_Unicode()
    {
        JsonArray ja = new();
        ja.Add("\u263A");
        var expected = "[\"\\u263a\"]";
        var actual = ja.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonArray_ToString_AllTypes()
    {
        JsonArray ja = new();
        ja.Add(null);
        ja.Add(true);
        ja.Add(false);
        ja.Add(123);
        ja.Add("abc");
        var expected = "[null,true,false,123,\"abc\"]";
        var actual = ja.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonArray_ToString_AllTypes_Format()
    {
        JsonArray ja = new();
        ja.Add(null);
        ja.Add(true);
        ja.Add(false);
        ja.Add(123);
        ja.Add("abc");
        var expected = "[\r\n  null,\r\n  true,\r\n  false,\r\n  123,\r\n  \"abc\"\r\n]";
        var actual = ja.ToString(true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonArray_InsertAt()
    {
        JsonArray ja = new();
        ja.Add(1);
        ja.Add(3);
        ja.InsertAt(1, 2);
        var expected = "[1,2,3]";
        var actual = ja.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonArray_RemoveAt()
    {
        JsonArray ja = new();
        ja.Add(1);
        ja.Add(2);
        ja.Add(3);
        ja.RemoveAt(1);
        var expected = "[1,3]";
        var actual = ja.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonArray_Clear()
    {
        JsonArray ja = new();
        ja.Add(1);
        ja.Add(2);
        ja.Add(3);
        ja.Clear();
        var expected = "[]";
        var actual = ja.ToString();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonArray_ForEach()
    {
        JsonArray ja = new();
        ja.Add(1);
        ja.Add(2);
        ja.Add(3);
        var expected = "1+2+3";
        var actual = "";
        foreach (var item in ja)
        {
            if (actual.Length > 0)
                actual += "+";
            actual += item.ToString();
        }
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Test_JsonArray_Parse()
    {
        var expected = "[null,true,false,123,\"abc\"]";
        JsonArray ja = JsonArray.Parse(expected);
        var actual = ja.ToString();
        Assert.Equal(expected, actual);
    }
}
