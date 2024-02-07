using System.Text;

namespace AppEnd
{
	public class AppEndClass(string className, string namespaceName)
	{
        private readonly string tempBody = CSharpImpBodies.ClassImp.Replace("$Namespace$", namespaceName).Replace("$ClassName$", className);

        public List<string> Methods { get; set; } = [];

		public string ToCode()
        {
            StringBuilder sb = new();
            foreach (var method in Methods)
				sb.Append(CSharpImpBodies.MethodImp.Replace("$MethodName$", method));
			return tempBody.Replace("$Methods$", sb.ToString());
        }
    }

    public class AppEndMethod(string methodName)
	{
        private readonly string methodName = methodName;
        public string MethodImplementation 
        {
            get
            {
                return CSharpImpBodies.MethodImp.Replace("$MethodName$", methodName);
			}
        }
	}


	internal static class CSharpImpBodies
    {
        internal static string ClassImp => @"
using System;
using System.Text.Encodings;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using AppEndDbIO;
using AppEnd;

namespace $Namespace$
{
    public static class $ClassName$
    {
$Methods$
    }
}
";

		internal static string MethodImp => @"
        public static object? $MethodName$(JsonElement ClientQueryJE, AppEndUser? Actor)
        {
            return AppEndDbIO.ClientQuery.GetInstanceByQueryJson(ClientQueryJE, Actor?.ContextInfo).Exec();
        }
";

	}

}
