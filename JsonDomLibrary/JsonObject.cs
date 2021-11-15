using System.Text;

namespace JsonDomLibrary
{
    public class JsonObject
    {
        private readonly Dictionary<string, object?> _data;

        public JsonObject()
        {
            _data = new Dictionary<string, object?>();
        }

        public object? this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException(nameof(key));
                if (key.StartsWith("$."))
                    return GetFromPath(key);
                if (_data.ContainsKey(key))
                    return _data[key];
                return null;
            }
            set
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException(nameof(key));
                if (key.StartsWith("$."))
                    SetFromPath(key, value);
                else if (_data.ContainsKey(key))
                    _data[key] = value;
                else
                    _data.Add(key, value);
            }
        }

        public override string ToString()
        {
            StringBuilder result = new();
            result.Append('{');
            foreach (var kv in _data)
            {
                if (result.Length > 1)
                    result.Append(',');
                result.Append(Routines.ValueToString(kv.Key));
                result.Append(':');
                result.Append(Routines.ValueToString(kv.Value));
            }
            result.Append('}');
            return result.ToString();
        }

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
}