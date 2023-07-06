using System.Reflection;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace AppEnd
{
    public static class ExtensionsForTypeAndAssembly
    {
        public static Type[] GetTypesReal(this Assembly asm)
        {
            return asm.GetTypes().Where(i => !i.Name.Contains("EmbeddedAttribute") && !i.Name.Contains("RefSafetyRulesAttribute")).ToArray();
        }
        public static MethodInfo[] GetMethodsReal(this Type type)
        {
            return type.GetMethods().Where(m => !m.Name.Equals("GetType") && !m.Name.Equals("ToString") && !m.Name.Equals("Equals") && !m.Name.Equals("GetHashCode")).ToArray();
        }
    }
}