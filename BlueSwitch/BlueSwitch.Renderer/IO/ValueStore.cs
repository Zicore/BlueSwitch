using System;
using System.Collections.Generic;

namespace BlueSwitch.Base.IO
{
    public class ValueStore
    {
        public Dictionary<string, object> Values { get; } = new Dictionary<string, object>();

        public void Store(String key, Object value)
        {
            Values[key] = value;
        }

        public T Get<T>(String key)
        {
            return (T)Values[key];
        }

        public T GetOrDefault<T>(String key)
        {
            if (Values.ContainsKey(key))
            {
                return (T)Values[key];
            }
            return default(T);
        }
    }
}
