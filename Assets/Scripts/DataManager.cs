using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    static class DataManager
    {
        private const string FilePath = "gamedata.dat";
        private static readonly BinaryFormatter Formatter = new ();
        private static Dictionary<string, object> _dataDictionary;
        public static void SaveData(string key, object value)
        {
            Dictionary<string, object> dictionary = GetDictionary();
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key,value);
            }
            Debug.Log($"Data saving: {{{key}}} = {value}");
            using FileStream stream = new(FilePath, FileMode.OpenOrCreate);
            Formatter.Serialize(stream, _dataDictionary);

        }

        public static object GetData(string key)
        {
            
            if (!GetDictionary().ContainsKey(key)) return null;
            object data = GetDictionary()[key];
            Debug.Log($"Data retrieving: {{{key}}} = {data}");
            return data;
        }

        private static Dictionary<string, object> GetDictionary()
        {

            if (_dataDictionary != null) return _dataDictionary;
            using (FileStream stream = new(FilePath, FileMode.OpenOrCreate))
            {
                if (stream.Length == 0)
                    _dataDictionary = new Dictionary<string, object>();
                else
                    _dataDictionary = (Dictionary<string, object>)Formatter.Deserialize(stream);
            }
            return _dataDictionary;
        }
    }
}
