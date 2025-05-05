namespace AppEndCommon
{
	public static class ExtIntArray
	{
		public static bool HasIntersect(this int[] arr, int[]? testArr)
		{
			if (testArr is null || testArr.Length == 0) return false;
			foreach (int str in arr)
			{
				if (testArr.Contains(str)) return true;
			}
			return false;
		}

	}
}