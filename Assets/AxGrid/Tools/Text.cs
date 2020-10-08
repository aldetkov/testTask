//#define develop

using System;
using System.Collections.Generic;
using System.IO;
using AxGrid.Utils;
using SmartFormat;
using SmartFormat.Core.Settings;
using UnityEngine;
using YamlDotNet.Serialization;


namespace AxGrid.Tools
{
    /// <summary>
    /// Тектовая утилита
    /// </summary>
    public static class Text
    {

        private static SmartFormatter sf;

        public static SmartFormatter Sf
        {
            get
            {
                if (sf == null){
                    Init(new []{"ru"});
                }
                return sf;
            }
        }

        private static Dictionary<string, object> result;
        public static Dictionary<string, object> Translations => result;

        public static bool HasKey(string key)
        {
            return result.ContainsKey(key);
        }

        
        
        public static void Init(IEnumerable<string> languageCodes)
        {
            result = new Dictionary<string, object>();
            foreach (var languageCode in languageCodes)
            {
                var file = $"Translations/{languageCode}_out.yml";
                var t = Resources.Load(file) as TextAsset;
                Log.Debug($"Load translations file {file}");
                var deserializer = new Deserializer();
                var obj = (Dictionary<string, object>) deserializer.Deserialize(
                    new StringReader(t.text),
                    typeof(Dictionary<string, object>)
                );
                result = StaticUtils.UnionDictionaries(result, obj);
            }

            sf = Smart.CreateDefaultSmartFormat();
            sf.Settings.FormatErrorAction = ErrorAction.ThrowError;
        }

        public static string GetFormat(string format)
        {
            if (!format.StartsWith("app."))
                return format;
            try
            {
                return Sf.Format("{" + format + "}", result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public static string Get(string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
                return format;
            return args.Length == 0 ? _Get(format) : Smart.Format(_Get(format), args);
        }

        private static string _Get(string var)
        {
            if (!var.StartsWith("app."))
                return var;
            try
            {
                return Sf.Format("{" + var + "}", result);
            }
            catch (Exception)
            {
                return var;
            }
        }

    }
}