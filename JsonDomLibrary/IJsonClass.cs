namespace JsonDomLibrary;

public interface IJsonClass
{
    public string ToString(bool format);

    public string ToString(bool format, bool unquoted);

    public string ToString(bool format, int indent);

    public string ToString(bool format, bool unquoted, int indent);
}
