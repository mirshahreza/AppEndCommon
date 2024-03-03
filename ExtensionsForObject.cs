﻿namespace AppEnd
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
			if (bool.TryParse(i, out bool ii))
			{
				return ii;
			}
			return ifHasProblem;
		}
		public static int To01Safe(this bool o)
		{
			if(o) return 1;
			return 0;
		}

		public static object FixNull(this object? o, object ifNull)
        {
            if (o is null && ifNull is null) throw new ArgumentNullException(nameof(ifNull));
            if (o is null) return ifNull;
            else return o;
        }



    }
}