using System.Reflection;

namespace AppEndCommon
{
	public static class ExtAssembly
    {
        public static Type[] GetTypesReal(this Assembly asm)
        {
            return asm.GetTypes().Where(i => !i.Name.ContainsIgnoreCase("EmbeddedAttribute") && !i.Name.ContainsIgnoreCase("RefSafetyRulesAttribute")).ToArray();
        }
        
    }
}