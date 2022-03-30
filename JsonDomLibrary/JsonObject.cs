﻿namespace JsonDomLibrary;

public class JsonObject : Dictionary<string, object?>
{
    public JsonObject()
    {
    }

    public new object? this[string key]
    {
        get
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (key.StartsWith("$."))
                return GetFromPath(key);
            if (ContainsKey(key))
                return base[key];
            return null;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (key.StartsWith("$."))
                SetFromPath(key, value);
            else if (ContainsKey(key))
                base[key] = value;
            else
                Add(key, value);
        }
    }

    public JsonArray GetJsonArray(string key)
    {
        return (JsonArray)(this[key] ?? new JsonArray());
    }

    public JsonObject GetJsonObject(string key)
    {
        return (JsonObject)(this[key] ?? new JsonObject());
    }

    public new bool Remove(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));
        if (key.StartsWith("$."))
            return RemoveFromPath(key);
        if (ContainsKey(key))
            return base.Remove(key);
        return false;
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
        foreach (var kv in this)
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
            result.Append(JsonBaseClass.ValueToString(kv.Key));
            result.Append(':');
            if (format)
                result.Append(' ');
            result.Append(JsonBaseClass.ValueToString(kv.Value, format, indent));
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
        return JsonBaseClass.GetValueObject(data, ref pos);
    }

    #endregion

    #region private routines

    private object? GetFromPath(string path)
    {
        try
        {
            var keys = SplitPath(path);
            object? obj = this;
            for (int i = 0; i < keys.Length; i++)
            {
                if (obj == null)
                    return null;
                if (obj.GetType() == typeof(JsonObject))
                {
                    obj = ((JsonObject)obj)[keys[i]];
                }
                else if (obj.GetType() == typeof(JsonArray))
                {
                    int index = int.Parse(keys[i]);
                    obj = ((JsonArray)obj)[index];
                }
                else throw new Exception();
            }
            return obj;
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
            object? obj = this;
            for (int i = 0; i < keys.Length - 1; i++)
            {
                if (obj?.GetType() == typeof(JsonObject))
                {
                    if (((JsonObject)obj)[keys[i]] == null)
                    {
                        ((JsonObject)obj)[keys[i]] = new JsonObject();
                    }
                    obj = ((JsonObject)obj)[keys[i]];
                }
                else if (obj?.GetType() == typeof(JsonArray))
                {
                    int index = int.Parse(keys[i]);
                    if (((JsonArray)obj)[index] == null)
                    {
                        ((JsonArray)obj)[index] = new JsonObject();
                    }
                    obj = ((JsonArray)obj)[index];
                }
                else throw new Exception();
            }
            if (obj?.GetType() == typeof(JsonObject))
            {
                ((JsonObject)obj)[keys[^1]] = value;
            }
            else if (obj?.GetType() == typeof(JsonArray))
            {
                ((JsonArray)obj)[int.Parse(keys[^1])] = value;
            }
            else throw new Exception();
        }
        catch (Exception)
        {
            throw new ArgumentException($"Invalid path: {path}");
        }
    }

    private bool RemoveFromPath(string path)
    {
        try
        {
            var keys = SplitPath(path);
            object? obj = this;
            for (int i = 0; i < keys.Length - 1; i++)
            {
                if (obj == null)
                    return false;
                if (obj.GetType() == typeof(JsonObject))
                {
                    obj = ((JsonObject)obj)[keys[i]];
                }
                else if (obj.GetType() == typeof(JsonArray))
                {
                    int index = int.Parse(keys[i]);
                    obj = ((JsonArray)obj)[index];
                }
                else throw new Exception();
            }
            if (obj != null)
                return ((JsonObject)obj).Remove(keys[^1]);
            return false;
        }
        catch (Exception)
        {
            throw new ArgumentException($"Invalid path: {path}");
        }
    }

    private static string[] SplitPath(string path)
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
                            var tempKey = currKey.ToString();
                            if (string.IsNullOrWhiteSpace(tempKey))
                                throw new Exception();
                            result.Add(tempKey);
                            currKey.Clear();
                        }
                        break;
                    default:
                        currKey.Append(c);
                        escape = false;
                        break;
                }
            }
            var tempKey2 = currKey.ToString();
            if (string.IsNullOrWhiteSpace(tempKey2))
                throw new Exception();
            result.Add(tempKey2);
            return result.ToArray();
        }
        catch (Exception)
        {
            throw new ArgumentException($"Invalid path: {path}");
        }
    }

    #endregion
}
