using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEnd
{
    public class AppEndClass
    {
//        private string CSharpClassBody => @"
//using System;
//using System.Text.Encodings;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using AppEndDbIO;
//using AppEnd;

//namespace $Namespace$
//{
//    public static class $ClassName$
//    {
//$Methods$
//    }
//}
//";

//        private string CSharpMethodBody => @"
//        public static object? $MethodName$(JsonElement ClientQueryJE, AppEndUser? Actor)
//        {
//            return AppEndDbIO.ClientQuery.GetInstanceByQueryJson(ClientQueryJE, Actor?.ContextInfo).Exec();
//        }
//";

        private string tempBody = "";

        public List<string> Methods { get; set; } = new List<string>();

        public AppEndClass(string className,string namespaceName) 
        {
            tempBody = CSharpImpBodies.ClassImp
				.Replace("$Namespace$", namespaceName)
                .Replace("$ClassName$", className);
        }

        public string ToCode()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var method in Methods)
            {
                sb.Append(CSharpImpBodies.MethodImp.Replace("$MethodName$", method));
            }
            return tempBody.Replace("$Methods$", sb.ToString());
        }

        
    }

    public class AppEndMethod
    {
        private string methodName = "";
        public string MethodImplementation 
        {
            get
            {
                return CSharpImpBodies.MethodImp.Replace("$MethodName$", methodName);
			}
        }
        public AppEndMethod(string methodName)
        {
            this.methodName = methodName;
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
