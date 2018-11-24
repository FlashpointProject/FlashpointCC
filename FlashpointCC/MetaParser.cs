using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlashpointCurator
{
    class MetaParser
    {
        public static T Deserialize<T>(Stream stream) where T: class, new()
        {
            stream.Position = 0;
            var obj = new T();
            var dict = Parse(stream);
            foreach (var entry in GetKeys(typeof(T)))
            {
                if (dict.ContainsKey(entry.Value))
                {
                    entry.Key.SetValue(obj, dict[entry.Value]);
                }
            }
            return obj;
        }

        public static string Serialize(object obj)
        {
            var sb = new StringBuilder();
            foreach (var entry in GetKeys(obj.GetType()))
            {
                sb.AppendFormat("{0}: {1}", entry.Value, entry.Key.GetValue(obj).ToString()).AppendLine();
            }
            return sb.ToString();
        }

        private static Dictionary<PropertyInfo, string> GetKeys(Type type)
        {
            Dictionary<PropertyInfo, string> keys = new Dictionary<PropertyInfo, string>();
            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.GetCustomAttribute<MetaIgnore>() == null))
            {
                var metaElement = prop.GetCustomAttribute<MetaElement>();
                keys.Add(prop, metaElement == null ? prop.Name : metaElement.ElementName);
            }
            return keys;
        }

        private static Dictionary<string, string> Parse(Stream stream)
        {
            string line;
            var values = new Dictionary<string, string>();
            using (var reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(new char[] { ':' }, 2);
                    values.Add(split[0], split[1].TrimStart());
                }
            }
            return values;
        }

        public class MetaElement : Attribute
        {
            public string ElementName { get; set; }

            public MetaElement() { }

            public MetaElement(string elementName)
            {
                ElementName = elementName;
            }
        }

        public class MetaIgnore : Attribute { }
    }
}
