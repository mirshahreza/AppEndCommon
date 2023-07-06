namespace AppEnd
{
    public static class ExtensionsForString
    {
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
                    .AddParam("ParamName", paramName);
        }

    }
}