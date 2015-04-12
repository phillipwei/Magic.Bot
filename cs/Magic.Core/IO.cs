using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Magic.Core
{
    public static class IO
    {
        private static Func<string, object> GetParser(Type type, char listSeperator, string emptyToken)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var itemType = type.GetGenericArguments()[0];
                var itemParser = GetParser(itemType, listSeperator, emptyToken);
                return (s) => {
                    var instance = Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
                    if(!s.Equals(emptyToken))
                    {
                        foreach (var itemString in s.Split(listSeperator))
                        {
                            var item = itemParser(itemString);
                            instance.GetType().GetMethod("Add").Invoke(instance, new object[] { item });
                        }
                    }
                    return instance;
                };
            }

            var explicitParser = type.GetMethod("Parse", new Type[] { typeof(string) });
            if(explicitParser != null)
            {
                return (s) => explicitParser.Invoke(null, new object[] { s });
            }
            else if (type.IsEnum)
            {
                return (s) => Enum.Parse(type, s);
            }
            else if (type.GetInterfaces().Contains(typeof(IConvertible)))
            {
                return (s) => Convert.ChangeType(s, type);
            }
            throw new InvalidCastException();
        }

        public static List<T> LoadFromFile<T>(string filePath, char seperator, char listSeperator, string emptyToken, params object[] args)
        {
            List<T> list = new List<T>();

            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            var headers = lines[0].Split(seperator);
            var setters = headers.Select(h => typeof(T).GetProperty(h, BindingFlags.Instance | BindingFlags.Public)).ToList();
            var parsers = setters.Select(s => GetParser(s.PropertyType, listSeperator, emptyToken)).ToList();

            for(int i=1; i<lines.Length; i++)
            {
                T obj = (T)Activator.CreateInstance(typeof(T), args);
                var columns = lines[i].Split(seperator);
                for (int j = 0; j < columns.Length; j++)
                {
                    var header = headers[j];
                    try
                    { 
                        setters[j].SetValue(obj, parsers[j](columns[j]), null);
                    }
                    catch(Exception e)
                    {
                        throw new FormatException(string.Format("Could not parse value '{0}' for type '{1}'", columns[j], headers[j]), e);
                    }
                }
                list.Add(obj);
            }

            return list;
        }

        private static Dictionary<string, string> _mapping = new Dictionary<string, string>() { 
            {"’", "'"},
            {"ʼ", "'"},
        };

        public static string Sanitize(string s)
        {
            foreach(var map in _mapping)
            {
                s = s.Replace(map.Key, map.Value);
            }
            return s;
        }
    }
}
