namespace JsonDomLibrary;

public class JsonObject : JsonBaseClass, IEnumerable
{
    private readonly Dictionary<string, object?> _data;

    public JsonObject(bool caseInsensitive = false)
    {
        if (!caseInsensitive)
            _data = new Dictionary<string, object?>();
        else
            _data = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerator GetEnumerator()
    {
        return _data.Keys.GetEnumerator();
    }

    public object? this[string key]
    {
        get
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (key.StartsWith("$."))
                return GetFromPath(key);
            if (_data.ContainsKey(key))
                return _data[key];
            return null;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (key.StartsWith("$."))
                SetFromPath(key, value);
            else if (_data.ContainsKey(key))
                _data[key] = value;
            else
                _data.Add(key, value);
        }
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

    internal string ToString(bool format, int indent)
    {
        StringBuilder result = new();
        result.Append('{');
        if (format)
        {
            indent++;
            result.AppendLine();
            result.Append(new string(' ', indent * 2));
        }
        bool comma = false;
        foreach (var kv in _data)
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
            result.Append(ValueToString(kv.Key));
            result.Append(':');
            if (format)
                result.Append(' ');
            result.Append(ValueToString(kv.Value, format, indent));
        }
        if (format)
        {
            indent--;
            result.AppendLine();
            result.Append(new string(' ', indent * 2));
        }
        result.Append('}');
        return result.ToString();
    }

    #endregion

    #region Parse

    public static JsonObject Parse(string data)
    {
        int pos = 0;
        return GetValueObject(data, ref pos);
    }

    #endregion

    #region private routines

    private object? GetFromPath(string path)
    {
        try
        {
            var keys = SplitPath(path);
            JsonObject? currJO = this;
            for (int i = 0; i < keys.Length; i++)
            {
                if (string.IsNullOrEmpty(keys[i]))
                    throw new Exception();
                if (currJO == null)
                    throw new Exception();
                if (i < keys.Length - 1)
                {
                    if (currJO[keys[i]] == null)
                        return null;
                    currJO = (JsonObject?)currJO[keys[i]];
                }
                else
                    return currJO[keys[i]];
            }
            throw new Exception();
        }
        catch (Exception)
        {
            throw new ArgumentException($"Invalid path: {path}");
        }
    }

    private void SetFromPath(string path, object? value)
    {
        try
        {
            var keys = SplitPath(path);
            JsonObject? currJO = this;
            for (int i = 0; i < keys.Length; i++)
            {
                if (string.IsNullOrEmpty(keys[i]))
                    throw new Exception();
                if (currJO == null)
                    throw new Exception();
                if (i < keys.Length - 1)
                {
                    if (currJO[keys[i]] == null)
                        currJO[keys[i]] = new JsonObject();
                    currJO = (JsonObject?)currJO[keys[i]];
                }
                else
                    currJO[keys[i]] = value;
            }
        }
        catch (Exception)
        {
            throw new ArgumentException($"Invalid path: {path}");
        }
    }

    private string[] SplitPath(string path)
    {
        List<string> result = new();
        bool inQuote = false;
        bool escape = false;
        StringBuilder currKey = new();
        try
        {
            foreach (char c in path[2..]) // skip "$."
            {
                switch (c)
                {
                    case '"':
                        if (escape)
                        {
                            currKey.Append('"');
                            escape = false;
                        }
                        else if (currKey.Length == 0)
                            inQuote = true;
                        else
                            inQuote = false;
                        break;
                    case '\\':
                        if (escape)
                        {
                            currKey.Append('\\');
                            escape = false;
                        }
                        else if (!inQuote)
                            throw new Exception();
                        else
                            escape = true;
                        break;
                    case '.':
                        if (inQuote)
                        {
                            currKey.Append('.');
                            escape = false;
                        }
                        else if (escape)
                            throw new Exception();
                        else
                        {
                            if (currKey.Length == 0 || currKey.ToString() == "$")
                                throw new Exception();
                            result.Add(currKey.ToString());
                            currKey.Clear();
                        }
                        break;
                    default:
                        currKey.Append(c);
                        escape = false;
                        break;
                }
            }
            if (currKey.Length == 0 || currKey.ToString() == "$")
                throw new Exception();
            result.Add(currKey.ToString());
            return result.ToArray();
        }
        catch (Exception)
        {
            throw new ArgumentException($"Invalid path: {path}");
        }
    }

    #endregion
}
