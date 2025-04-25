using System.Reflection;

namespace AppEndCommon
{
	public static class ExtMethodBase
	{
		public static string GetPlaceInfo(this MethodBase? methodBase)
		{
			if (methodBase == null) return "";
			return $"{methodBase?.DeclaringType?.Name}, {methodBase?.Name}";
		}
	}
}