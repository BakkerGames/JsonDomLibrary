namespace JsonDomLibrary;

public partial class JsonObjectExtended
{
    private JsonObject _data;
    private JsonObject _changes;
    private JsonArray _removed;

    public JsonObjectExtended()
    {
        _data = new();
        _changes = new();
        _removed = new();
    }

    public JsonObjectExtended(JsonObject value)
    {
        _data = value;
        _changes = new();
        _removed = new();
    }

    public JsonObjectExtended(JsonObjectExtended value)
    {
        _data = value._data;
        _changes = value._changes;
        _removed = value._removed;
    }

    public JsonObjectExtended(string value)
    {
        _data = JsonObject.Parse(value);
        _changes = new();
        _removed = new();
    }

    public object? this[params string[] keys]
    {
        get
        {
            if (keys.Length == 0)
                return null;
            if (string.IsNullOrEmpty(keys[0]))
                throw new ArgumentNullException(nameof(keys));
            if (keys.Length == 1)
            {
                if (_removed.Contains(keys[0]))
                    return null;
                if (_changes.ContainsKey(keys[0]))
                    return _changes[keys[0]];
                if (_data.ContainsKey(keys[0]))
                    return _data[keys[0]];
                return null;
            }
            JsonObjectExtended? jo = (JsonObjectExtended?)this[keys[0]]; // this[] can return null
            if (jo == null) return null;
            List<string> newKeys = keys.ToList();
            newKeys.RemoveAt(0);
            return jo[newKeys.ToArray()];
        }
        set
        {
            if (keys.Length == 0)
                return;
            if (string.IsNullOrEmpty(keys[0]))
                throw new ArgumentNullException(nameof(keys));
            if (keys.Length == 1)
            {
                if (_removed.Contains(keys[0]))
                {
                    _removed.Remove(keys[0]);
                }
                if (_data[keys[0]] == value)
                {
                    if (_changes.ContainsKey(keys[0]))
                    {
                        _changes.Remove(keys[0]);
                    }
                    return;
                }
                if (value is not null and JsonObject)
                {
                    _changes[keys[0]] = new JsonObjectExtended((JsonObject)value);
                }
                else
                {
                    _changes[keys[0]] = value;
                }
                return;
            }
            JsonObjectExtended? jo = (JsonObjectExtended?)this[keys[0]]; // this[] can return null
            if (jo == null)
            {
                jo = new();
                _changes[keys[0]] = jo;
            }
            List<string> newKeys = keys.ToList();
            newKeys.RemoveAt(0);
            jo[newKeys.ToArray()] = value;
        }
    }

    public List<string> Keys
    {
        get
        {
            List<string> result = _data.Keys.ToList();
            foreach (string key in _changes.Keys)
            {
                if (!result.Contains(key))
                    result.Add(key);
            }
            foreach (string key in _removed.Cast<string>())
            {
                if (key != null && result.Contains(key))
                    result.Remove(key);
            }
            return result;
        }
    }

    public int Count
    {
        get
        {
            return Keys.Count;
        }
    }

    public bool ContainsKey(string key)
    {
        if (_removed.Contains(key))
            return false;
        return _data.ContainsKey(key) || _changes.ContainsKey(key);
    }

    public void Add(string key, object? value)
    {
        if (_removed.Contains(key))
        {
            _removed.Remove(key);
        }
        _changes[key] = value;
    }

    public void Clear()
    {
        _data.Clear();
        _changes.Clear();
        _removed.Clear();
    }

    public void Remove(string key)
    {
        if (_changes.ContainsKey(key))
        {
            _changes.Remove(key);
        }
        if (!_removed.Contains(key))
        {
            _removed.Add(key);
        }
    }

    public void Rename(string fromKey, string toKey)
    {
        if (string.IsNullOrEmpty(fromKey))
        {
            throw new ArgumentNullException(nameof(fromKey));
        }
        if (string.IsNullOrEmpty(toKey))
        {
            throw new ArgumentNullException(nameof(toKey));
        }
        if (fromKey == toKey)
        {
            throw new ArgumentException("Keys cannot match");
        }
        if (!ContainsKey(fromKey))
        {
            throw new ArgumentException($"Key not found: {fromKey}");
        }
        if (ContainsKey(toKey))
        {
            throw new ArgumentException($"Key already exists: {toKey}");
        }
        var value = this[fromKey];
        this[toKey] = value;
        Remove(fromKey);
    }

    public void SetChanges(JsonObject value)
    {
        _changes = value.GetJsonObjectOrDefault(nameof(_changes));
        _removed = value.GetJsonArrayOrDefault(nameof(_removed));
    }

    public void Collapse()
    {
        _data = GetCollapsed();
        _changes.Clear();
        _removed.Clear();
    }

    public JsonObject GetCollapsed()
    {
        JsonObject result = _data;
        foreach (KeyValuePair<string, object?> kv in _changes)
        {
            result[kv.Key] = kv.Value;
        }
        foreach (string key in _removed.Cast<string>())
        {
            if (result.ContainsKey(key))
            {
                result.Remove(key);
            }
        }
        return result;
    }

    public static JsonObjectExtended Parse(string value)
    {
        JsonObjectExtended result = new(JsonObject.Parse(value));
        return result;
    }
}