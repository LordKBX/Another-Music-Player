using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnotherMusicPlayer
{
    public static class EnumHelper<T> where T : struct, Enum // This constraint requires C# 7.3 or later.
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        public static IList<T> GetLocalValues(Enum value)
        {
            var enumValues = new List<T>();
            string parsed = string.Join(",", GetDisplayValues(value));

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (parsed.Contains(fi.Name))
                {
                    enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
                }
            }
            return enumValues;
        }

        public static T AddValue(T value, T v)
        {
            var enumValues = new List<T>();
            string parsed = EnumHelper<T>.GetValuesString(value);
            string sv = EnumHelper<T>.GetValuesString(v);

            //Console.WriteLine(parsed);
            //Console.WriteLine(sv);
            try
            {
                if (!parsed.Contains(sv))
                {
                    //Console.WriteLine("f**k");
                    parsed += "," + sv;
                    return EnumHelper<T>.Parse(parsed);
                }
            }
            catch (Exception) { }
            return (T)value;
        }

        public static T DelValue(T value, T deletionItem, T defaultValue)
        {
            var enumValues = new List<T>();
            string parsed = EnumHelper<T>.GetValuesString(value);
            string sv = EnumHelper<T>.GetValuesString(deletionItem);

            //Debug.WriteLine(parsed);
            //Debug.WriteLine(sv);
            try
            {
                if (parsed.Contains(sv))
                {
                    //Debug.WriteLine("f**k");
                    parsed = parsed.Replace(sv, "").Replace(", ,", ",").Replace(",,", ",").Trim();
                    if (parsed.StartsWith(",")) { parsed = parsed.Substring(1); }
                    if (parsed.EndsWith(",")) { parsed = parsed.Substring(0, parsed.Length - 1); }
                    parsed = parsed.Trim();
                    //Debug.WriteLine(parsed);
                    return (parsed.Length > 0)?EnumHelper<T>.Parse(parsed):defaultValue;
                }
            }
            catch (Exception) { }
            return (T)value;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T? Parse2(string value)
        {
            try { return (T)Enum.Parse(typeof(T), value, true); }
            catch (Exception) { return null; }
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => {
                T? r = Parse(obj);
                if (r != null) { return GetDisplayValue((T)r); }
                else { return ""; }
            }).ToList();
        }

        private static string lookupResource(Type resourceManagerProvider, string resourceKey)
        {
            var resourceKeyProperty = resourceManagerProvider.GetProperty(resourceKey,
                BindingFlags.Static | BindingFlags.Public, null, typeof(string),
                new Type[0], null);
            if (resourceKeyProperty != null)
            {
                MethodInfo? mi = resourceKeyProperty.GetMethod;
                if (mi != null) { return "" + mi.Invoke(null, null); }
            }

            return resourceKey; // Fallback with the key name
        }

        public static string GetDisplayValue(T value)
        {
            FieldInfo? fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) { return ""; }

            DisplayAttribute[]? descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (descriptionAttributes == null) { return ""; }

            if (descriptionAttributes.Length > 0)
            {
                if (descriptionAttributes[0].ResourceType != null)
                { return lookupResource(descriptionAttributes[0].ResourceType, "" + descriptionAttributes[0].Name); }

                if (descriptionAttributes == null) return string.Empty;
                return (descriptionAttributes.Length > 0) ? "" + descriptionAttributes[0].Name : value.ToString();
            }

            return "" + Enum.GetName(typeof(T), value);
        }

        public static string GetValuesString(T value)
        {
            string st = "";
            T _Value = (T)value;
            foreach (var val in @EnumHelper<T>.GetValues(_Value))
            {
                if (_Value.HasFlag(val))
                {
                    var description = EnumHelper<T>.GetDisplayValue(val);
                    if (description.Length <= 0) { continue; }
                    if (st.Length > 0) { st += ", "; }
                    st += description;
                }
            }
            if (st.Length == 0) { return "None"; } else { return st; }
        }
    }
}
