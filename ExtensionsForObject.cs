namespace AppEnd
{
    public static class ExtensionsForObject
    {
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
    }
}