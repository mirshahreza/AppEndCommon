namespace AppEndCommon
{
	public static class ExtIntArray
	{
		public static bool HasIntersect(this int[]? l1, int[]? l2)
		{
			if (l2 is null || l2.Length == 0) return false;
			if (l1 is null || l1.Length == 0) return false;
			foreach (int i in l1) if (l2.Contains(i)) return true;
			return false;
		}

	}
}