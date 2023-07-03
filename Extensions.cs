using System.Reflection;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using System.Security.Cryptography;
using JWT;
using JWT.Algorithms;

namespace AppEnd
{
    public static class Extensions
    {
        public static string ToStringEmpty(this JToken? jToken)
        {
            if (jToken == null) return "";
            return jToken.ToString();
        }
        public static string ToStringEmpty(this JValue? jValue)
        {
            if (jValue == null) return "";
            return jValue.ToString();
        }

        public static string ToStringEmpty(this object? o)
        {
            if (o is null) return "";
            return o.ToString();
        }
        public static int ToIntSafe(this object? o, int ifHasProblem = -1)
        {
            string i = o.ToStringEmpty();
            if (i.IsNullOrEmpty()) return ifHasProblem;
            int ii;
            if (int.TryParse(i, out ii))
            {
                return ii;
            }
            return ifHasProblem;
        }

        public static JArray ToJArray(this JToken? jToken)
        {
            if (jToken is null) return new();
            if (jToken is not JArray) StaticMethods.ThrowArgEx("Input parameter is not JArray");
            return (JArray)jToken;
        }

        public static JObject ToJObject(this JToken? jToken)
        {
            if (jToken == null) return new();
            if (jToken is not JObject) StaticMethods.ThrowArgEx("Input parameter is not JObject");
            return (JObject)jToken;
        }

        public static string FixNullToString(this JToken? jToken, string ifNull)
        {
            if (jToken == null) return ifNull;
            if (jToken is JValue && ((JValue)jToken).Value == null) return ifNull;
            return ((JValue)jToken).Value.ToStringEmpty();
        }
        public static string? FixNullToNullableString(this JToken? jToken, string? ifNull)
        {
            if (jToken == null) return ifNull;
            if (jToken is JValue && ((JValue)jToken).Value == null) return ifNull;
            return ((JValue)jToken).Value.ToStringEmpty();
        }
        public static int? FixNullToNullableInt(this JToken? jToken, int? ifNull)
        {
            if (jToken == null) return ifNull;
            if (jToken is JValue && ((JValue)jToken).Value == null) return ifNull;
            return int.Parse(((JValue)jToken).Value.ToStringEmpty());
        }
        public static int FixNullToInt(this JToken? jToken)
        {
            if (jToken is null) return -1;
            if (jToken is JValue && ((JValue)jToken).Value == null) return -1;
            return jToken.ToIntSafe();
        }

        public static bool FixNullToBool(this JToken? jToken, bool ifNull)
        {
            if (jToken == null) return ifNull;
            if (jToken is JValue && ((JValue)jToken).Value == null) return ifNull;
            return bool.Parse(((JValue)jToken).Value.ToStringEmpty());
        }

        public static Type[] GetTypesReal(this Assembly asm)
        {
            return asm.GetTypes().Where(i => !i.Name.Contains("EmbeddedAttribute") && !i.Name.Contains("RefSafetyRulesAttribute")).ToArray();
        }
        public static MethodInfo[] GetMethodsReal(this Type type)
        {
            return type.GetMethods().Where(m => !m.Name.Equals("GetType") && !m.Name.Equals("ToString") && !m.Name.Equals("Equals") && !m.Name.Equals("GetHashCode")).ToArray();
        }

        public static string FixNull(this string? s, string alternate)
        {
            if (s == null) return alternate;
            return s;
        }
        public static bool IsNullOrEmpty(this string? s)
        {
            if (s == null || s.Trim() == "") return true;
            return false;
        }
        public static string FixNullOrEmpty(this string? s, string alternate)
        {
            if (s == null || s.Trim() == "") return alternate;
            return s;
        }
        public static string RepeatN(this string s, int n)
        {
            string temp = "";
            for (int i = 0; i < n; i++)
            {
                temp += s;
            }
            return temp;
        }
        public static string ReplaceLastOccurrence(this string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);

            if (place == -1)
                return source;

            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        public static string RemoveUnNecessaryEmptyLines(this string s)
        {
            s = s.Replace("\r\n\r\n\r\n\r\n", Environment.NewLine);
            s = s.Replace("\r\n\r\n\r\n", Environment.NewLine);
            s = s.Replace("\r\n\r\n", Environment.NewLine);
            return s;
        }

        #region JSON
        public static string ToJsonStringByNewtonsoft(this object? o, bool indented = true)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(o, indented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None);
        }
        public static JObject FromJsonToJObjectByNewtonsoft(this string s)
        {
            return JObject.Parse(s);
        }
        public static JArray FromJsonToJArrayByNewtonsoft(this string s)
        {
            return JArray.Parse(s);
        }
        public static string ToJsonStringByBuiltIn(this object? o, bool indented = true, bool includeFields = true, JsonIgnoreCondition ignorePolicy = JsonIgnoreCondition.WhenWritingDefault)
        {
            return JsonSerializer.Serialize(o, options: new()
            {
                IncludeFields = includeFields,
                WriteIndented = indented,
                DefaultIgnoreCondition = ignorePolicy,
                IgnoreReadOnlyProperties = true,
            });
        }
        public static string ToJsonStringByBuiltInAllDefaults(this object? o)
        {
            return JsonSerializer.Serialize(o);
        }
        public static JsonElement ToJsonElementByNewton(this object o)
        {
            return JsonSerializer.Deserialize<JsonElement>(o.ToJsonStringByNewtonsoft());
        }
        public static JsonElement ToJsonElementByBuiltIn(this object o)
        {
            return JsonSerializer.Deserialize<JsonElement>(o.ToJsonStringByBuiltIn(false));
        }

        public static string[]? DeserializeAsStringArray(this string? o)
        {
            if (o is null) return null;
            return JsonSerializer.Deserialize<string[]>(o);
        }

        #endregion


        #region Encription
        private static JWT.Serializers.JsonNetSerializer jsonNetSerializer = new JWT.Serializers.JsonNetSerializer();
        private static JwtBase64UrlEncoder jwtBase64UrlEncoder = new JwtBase64UrlEncoder();
        private static HMACSHA512Algorithm hMACSHA256Algorithm = new HMACSHA512Algorithm();
        private static UtcDateTimeProvider utcDateTimeProvider = new UtcDateTimeProvider();
        private static JwtValidator jwtValidator = new JwtValidator(jsonNetSerializer, utcDateTimeProvider);
        private static JwtEncoder enc = new JwtEncoder(hMACSHA256Algorithm, jsonNetSerializer, jwtBase64UrlEncoder);
        private static JwtDecoder dec = new JwtDecoder(jsonNetSerializer, jwtValidator, jwtBase64UrlEncoder, hMACSHA256Algorithm);

        public static string Encode(this object o, string secret)
        {
            return enc.Encode(null, o, Encoding.Unicode.GetBytes(secret));
        }

        public static string Decode(this string token, string secret)
        {
            if (token.FixNullOrEmpty("null").Equals("null")) StaticMethods.ThrowEx("Input object for encoding can not be null or empty");
            JwtParts jpS;
            jpS = new JwtParts(token);
            return dec.Decode(jpS, Encoding.Unicode.GetBytes(secret), true);
        }

        public static string GetMD5Hash(this string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public static string GetMD4Hash(this string input)
        {
            return StaticMethods.Md4Hash(input);
        }

        #endregion

        #region FileSystem
        public static void CopyFilesRecursively(this DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }

        #endregion


        public static object ToOrigType(this JsonElement s, ParameterInfo parameterInfo)
        {
            if (parameterInfo.ParameterType == typeof(int)) return int.Parse(s.ToString());
            if (parameterInfo.ParameterType == typeof(Int16)) return Int16.Parse(s.ToString());
            if (parameterInfo.ParameterType == typeof(Int32)) return Int32.Parse(s.ToString());
            if (parameterInfo.ParameterType == typeof(Int64)) return Int64.Parse(s.ToString());
            if (parameterInfo.ParameterType == typeof(bool)) return bool.Parse(s.ToString());
            if (parameterInfo.ParameterType == typeof(DateTime)) return DateTime.Parse(s.ToString());
            if (parameterInfo.ParameterType == typeof(Guid)) return Guid.Parse(s.ToString());
            if (parameterInfo.ParameterType == typeof(float)) return float.Parse(s.ToString());
            if (parameterInfo.ParameterType == typeof(Decimal)) return Decimal.Parse(s.ToString());
            if (parameterInfo.ParameterType == typeof(string)) return s.ToString();
            if (parameterInfo.ParameterType == typeof(byte[])) return Encoding.UTF8.GetBytes(s.ToString());
            return s;
        }
    }
}