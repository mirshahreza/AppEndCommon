﻿using System.Text;
using System.Text.RegularExpressions;

namespace AppEnd
{
    public static class ExtensionsForString
    {
        public static string BeginingCommonPart(this string? s1, string s2)
        {
            if (s1 is null || s2 is null) return "";
            char[] c1 = s1.ToCharArray();
            char[] c2 = s2.ToCharArray();
            List<char> result = new List<char>();

            int minLen = c1.Length < c2.Length ? c1.Length : c2.Length;

            int ind = 0;
            while (ind < minLen)
            {
                if (c1[ind] == c2[ind]) result.Add(c1[ind]);
                else break;
                ind++;
            }

            return string.Join("", result.ToArray());
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
        public static string ReplaceLastOccurrence(this string s, string find, string replace)
        {
            int place = s.LastIndexOf(find);
            if (place == -1) return s;
            string result = s.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        public static string RemoveUnNecessaryEmptyLines(this string s)
        {
            s = s.Replace(RepeatN(SV.NL, 4), SV.NL);
            s = s.Replace(RepeatN(SV.NL, 3), SV.NL);
            s = s.Replace(RepeatN(SV.NL, 2), SV.NL);
            return s;
        }

        public static void ValidateStringNotNullOrEmpty(this string s, string paramName)
        {
            if (s == null || s.Trim() == "") new AppEndException($"CanNotBeNullOrEmpty")
                    .AddParam("ParamName", paramName)
                    .AddParam("Site", $"{System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.Name}, {System.Reflection.MethodBase.GetCurrentMethod()?.Name}")
                    ;
        }

        public static string RemoveWhitelines(this string s)
        {
            return Regex.Replace(s, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
        }
    }
}