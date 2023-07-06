using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEndCommon
{
    public class AppEndClass
    {
        private string CSharpClassBody => @"
using System;
using System.Text.Encodings;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using AppEndDbIO;

namespace $Namespace$
{
    public static class $ClassName$
    {
$Methods$
    }
}
";

        private string CSharpMethodBody => @"
        public static object? $MethodName$(JsonElement ClientQueryJE)
        {
            return AppEndDbIO.ClientQuery.GetInstanceByQueryJson(ClientQueryJE).Exec();
        }
";

        private string tempBody = "";

        public List<string> Methods { get; set; } = new List<string>();

        public AppEndClass(string className,string namespaceName) 
        {
            tempBody = CSharpClassBody
                .Replace("$Namespace$", namespaceName)
                .Replace("$ClassName$", className);
        }

        public string ToCode()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var method in Methods)
            {
                sb.Append(CSharpMethodBody.Replace("$MethodName$", method));
            }
            return tempBody.Replace("$Methods$", sb.ToString());
        }

        
    }
}
