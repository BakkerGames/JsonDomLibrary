namespace JsonDomLibrary;

public interface IJsonClass
{
    public string ToString(bool format);

    public string ToString(bool format, int indent);
}
