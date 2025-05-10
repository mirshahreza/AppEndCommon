using System.Reflection;

namespace AppEndCommon
{
	public static class ExtType
	{
		public static MethodInfo[] GetMethodsReal(this Type type)
		{
			return type.GetMethods().Where(m => !m.Name.Equals("GetType") && !m.Name.Equals("ToString") && !m.Name.Equals("Equals") && !m.Name.Equals("GetHashCode")).ToArray();
		}
	}
}
