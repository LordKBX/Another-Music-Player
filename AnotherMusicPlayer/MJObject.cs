using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace App
{
    public class MJObject : JObject
    {
        public MJObject() : base() { }
        public MJObject(JObject ob) : base()
        {
            IEnumerable<JProperty> props = ob.Properties();
            foreach (JProperty prop in props) { this.Add(prop.Name, prop.Value); }
        }

        public T? GetValue<T>(string key, object? defaultValue = null) where T : IComparable
        {
            if (defaultValue != null && defaultValue.GetType().Name != typeof(T).Name) { throw new Exception("defaultValue Type do not match return type"); }
            JToken? jt = this.GetValue(key);
            if (jt != null) { return jt.Value<T>(); }
            if (defaultValue != null) { return (T)defaultValue; }
            return default(T);
        }

        public T GetValueStrict<T>(string key, object defaultValue) where T : IComparable
        {
            if (defaultValue == null) { throw new Exception("defaultValue is null"); }
            if (defaultValue.GetType().Name != typeof(T).Name) { throw new Exception("defaultValue Type do not match return type"); }
            JToken? jt = this.GetValue(key);
            if (jt != null)
            {
                T? v = jt.Value<T>();
                if (v != null) { return v; } else { return (T)defaultValue; }
            }
            return (T)defaultValue;
        }

        new public static MJObject Parse(string content)
        { try { return new MJObject(JObject.Parse(content)); } catch (Exception) { return new MJObject(); } }

        public static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            Error = (se, ev) => { ev.ErrorContext.Handled = true; },
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        public static string SerializeObject(object? value, Newtonsoft.Json.Formatting formating = Formatting.None)
        {
            if (value == null) { return ""; }
            return JsonConvert.SerializeObject(value, formating, jsonSerializerSettings);
        }
        public static T? DeserializeObject<T>(string value) { return (T?)JsonConvert.DeserializeObject(value, typeof(T), jsonSerializerSettings); }
        public static object? DeserializeObject(string value, Type? type) { return JsonConvert.DeserializeObject(value, type, jsonSerializerSettings); }

        public List<string> GetKeys() {
            List<string> keys = new List<string>();
            try { IEnumerable<JProperty> props = Properties(); foreach (JProperty prop in props) { keys.Add(prop.Name); } } catch (Exception) { }
            return keys;
        }
    }
}