using Xunit;
using JsonClassLibrary;

namespace UnitTests;

public class UnitTestJsonObject
{
    [Fact]
    public void Test_JsonObject_AnyNull()
    {
        JsonObject jo = new JsonObject();
        Assert.NotNull(jo);
        Assert.IsType<JsonObject>(jo);
        Assert.Null(jo["abc"]);
    }
}
