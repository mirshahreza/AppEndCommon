namespace AppEndCommon
{
	public static class ExtList
    {
		public static void AddIfNotContains(this List<string> list, string name)
		{
			if (!list.ContainsIgnoreCase(name)) list.Add(name);
		}
		public static void AddSafe(this List<string> list, string? name)
		{
			if (name is not null) list.Add(name);
		}

		public static bool ContainsIgnoreCase(this List<string>? list, string? testString)
		{
			if (list is null) return false;
			if (testString is null || testString == "") return false;
			foreach (string str in list) if (str.ToLower() == testString.ToLower()) return true;
			return false;
		}

		public static bool HasIntersect(this List<string>? l1, string[]? l2)
		{
			if (l2 is null || l2.Length == 0) return false;
			if (l1 is null || l2.Length == 0) return false;
			foreach (string str in l1) if (l2.ContainsIgnoreCase(str)) return true;
			return false;
		}

		public static bool HasIntersect(this List<int>? l1, int[]? l2)
		{
			if (l2 is null || l2.Length == 0) return false;
			if (l1 is null || l2.Length == 0) return false;
			foreach (int i in l1) if (l2.Contains(i)) return true;
			return false;
		}

		public static bool HasIntersect(this List<int>? l1, List<int>? l2)
		{
			if (l2 is null || l2.Count == 0) return false;
			if (l1 is null || l1.Count == 0) return false;
			foreach (int i in l1) if (l2.Contains(i)) return true;
			return false;
		}

	}
}