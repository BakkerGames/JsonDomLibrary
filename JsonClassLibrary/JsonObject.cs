using System.Text;

namespace JsonClassLibrary
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
                    return GetPath(key);
                if (_data.ContainsKey(key))
                    return _data[key];
                return null;
            }
            set
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException(nameof(key));
                if (key.StartsWith("$."))
                    SetPath(key, value);
                else if (_data.ContainsKey(key))
                    _data[key] = value;
                else
                    _data.Add(key, value);
            }
        }

        private object? GetPath(string key)
        {
            try
            {
                string path = key[2..];
                if (string.IsNullOrEmpty(path) || path == "$")
                    throw new Exception();
                if (path.Contains('.'))
                {
                    string currKey = path[..path.IndexOf('.')];
                    string remainingPath = path[(path.IndexOf('.') + 1)..];
                    if (string.IsNullOrEmpty(currKey) ||
                        string.IsNullOrEmpty(remainingPath) ||
                        currKey == "$" ||
                        remainingPath == "$")
                        throw new Exception();
                    if (_data.ContainsKey(currKey))
                    {
                        JsonObject? jsonObject = (JsonObject?)_data[currKey];
                        if (jsonObject != null)
                            return jsonObject.GetPath($"$.{remainingPath}");
                    }
                    return null;
                }
                return this[path];
            }
            catch (Exception)
            {
                throw new ArgumentException($"Invalid path: {key}");
            }
        }

        private void SetPath(string key, object? value)
        {
            try
            {
                string path = key[2..];
                if (string.IsNullOrEmpty(path) || path == "$")
                    throw new Exception();
                if (path.Contains('.'))
                {
                    string currKey = path[..path.IndexOf('.')];
                    string remainingPath = path[(path.IndexOf('.') + 1)..];
                    if (string.IsNullOrEmpty(currKey) ||
                        string.IsNullOrEmpty(remainingPath) ||
                        currKey == "$" ||
                        remainingPath == "$")
                        throw new Exception();
                    if (_data.ContainsKey(currKey))
                    {
                        JsonObject? jsonObject = (JsonObject?)_data[currKey];
                        if (jsonObject != null)
                            jsonObject.SetPath($"$.{remainingPath}", value);
                    }
                    JsonObject? newJsonObject = new();
                    _data[currKey] = newJsonObject;
                    newJsonObject.SetPath($"$.{remainingPath}", value);
                }
                else
                {
                    this[path] = value;
                }
            }
            catch (Exception)
            {
                throw new ArgumentException($"Invalid path: {key}");
            }
        }

        private string[] SplitPath(string path)
        {
            List<string> result = new List<string>();
            bool inQuote = false;
            bool escape = false;
            StringBuilder currKey = new();
            foreach (char c in path)
            {
                if (c == '\\')
                {
                    if (escape)
                    {
                        currKey.Append(c);
                        escape = false;
                    }
                    else
                    {
                        escape = true;
                    }
                    continue;
                }
                if (c == '"')
                {
                    if (!inQuote)
                    {
                        inQuote = true;
                        continue;
                    }
                    else if (escape)
                    {
                        currKey.Append('"');
                        escape = false;
                    }
                }
                currKey.Append(c);
            }
            return result.ToArray();
        }
    }
}