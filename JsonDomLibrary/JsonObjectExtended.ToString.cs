namespace JsonDomLibrary;

public partial class JsonObjectExtended
{
    public override string ToString()
    {
        return ToString(false, false, 0);
    }

    public string ToString(bool format)
    {
        return ToString(format, false, 0);
    }

    public string ToString(bool format, bool unquoted)
    {
        return ToString(format, unquoted, 0);
    }

    public string ToString(bool format, int indent)
    {
        return ToString(format, false, indent);
    }

    public string ToString(bool format, bool unquoted, int indent)
    {
        return GetCollapsed().ToString(format, unquoted, indent);
    }

    public string ToStringChanges()
    {
        return ToStringChanges(false, false, 0);
    }

    public string ToStringChanges(bool format)
    {
        return ToStringChanges(format, false, 0);
    }

    public string ToStringChanges(bool format, bool unquoted)
    {
        return ToStringChanges(format, unquoted, 0);
    }

    public string ToStringChanges(bool format, int indent)
    {
        return ToStringChanges(format, false, indent);
    }

    public string ToStringChanges(bool format, bool unquoted, int indent)
    {
        JsonObject result = new()
        {
            { nameof(_changes), _changes },
            { nameof(_removed), _removed }
        };
        return result.ToString(format, unquoted, indent);
    }
}