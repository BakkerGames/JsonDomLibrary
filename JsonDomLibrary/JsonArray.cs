namespace JsonDomLibrary;

public class JsonArray : JsonBaseClass, IEnumerable
{
    private readonly List<object> _data;

    public JsonArray()
    {
        _data = new List<object>();
    }

    public IEnumerator GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    public object? this[int index]
    {
        get
        {
            if (index < 0 || index >= _data.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _data[index];
        }
        set
        {
            if (index < 0 || index >= _data.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            _data[index] = value!;
        }
    }

    public int Count()
    {
        return _data.Count;
    }

    public void Clear()
    {
        _data.Clear();
    }

    public void Add(object? value)
    {
        _data.Add(value);
    }

    public void InsertAt(int index, object? value)
    {
        if (index < 0 || index >= Count())
            throw new ArgumentOutOfRangeException(nameof(index));
        _data.Insert(index, value);
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= Count())
            throw new ArgumentOutOfRangeException(nameof(index));
        _data.RemoveAt(index);
    }

    #region ToString

    public override string ToString()
    {
        return ToString(false, 0);
    }

    public string ToString(bool format)
    {
        return ToString(format, 0);
    }

    public string ToString(bool format, int indent)
    {
        StringBuilder result = new();
        result.Append('[');
        if (format)
        {
            indent++;
            result.AppendLine();
            result.Append(new string(' ', indent * 2));
        }
        bool comma = false;
        foreach (var value in _data)
        {
            if (comma)
            {
                result.Append(',');
                if (format)
                {
                    result.AppendLine();
                    result.Append(new string(' ', indent * 2));
                }
            }
            else
                comma = true;
            result.Append(ValueToString(value, format, indent));
        }
        if (format)
        {
            indent--;
            result.AppendLine();
            result.Append(new string(' ', indent * 2));
        }
        result.Append(']');
        return result.ToString();
    }

    #endregion

    #region Parse

    public static JsonArray Parse(string data)
    {
        int pos = 0;
        return GetValueArray(data, ref pos);
    }

    #endregion

}
