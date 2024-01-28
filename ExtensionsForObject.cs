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

		public static bool ToBooleanSafe(this object? o, bool ifHasProblem = false)
		{
			string i = o.ToStringEmpty();
			if (i.IsNullOrEmpty()) return ifHasProblem;
			bool ii;
			if (bool.TryParse(i, out ii))
			{
				return ii;
			}
			return ifHasProblem;
		}

		public static object FixNull(this object? o, object ifNull)
        {
            if (o is null && ifNull is null) throw new ArgumentNullException(nameof(ifNull));
            if (o is null) return ifNull;
            else return o;
        }



    }
}