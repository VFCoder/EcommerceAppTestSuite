using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework
{
    public static class GlobalObjects
    {
        public static string ResponseContent { get; set; }
        public static string AccessTokenResponseContent { get; set; }
        public static string AccessToken { get; set; }

        private static readonly Dictionary<string, string> _globalDictionary = new Dictionary<string, string>();

        public static void AddOrUpdate(string key, string value)
        {
            if (_globalDictionary.ContainsKey(key))
            {
                _globalDictionary[key] = value;
            }
            else
            {
                _globalDictionary.Add(key, value);
            }
        }

        public static string Get(string key)
        {
            if (_globalDictionary.ContainsKey(key))
            {
                return _globalDictionary[key];
            }
            return null;
        }
    }
}
