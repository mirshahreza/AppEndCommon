using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEnd
{
    public static class SV
    {
        public static string NT => "\t";
        public static string NL => Environment.NewLine;
        public static string NL2x => Environment.NewLine + Environment.NewLine;

        public static string CSharpClassBody => @"
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
}";
        

        public static string CSharpMethodBody => @"
        public static object? $MethodName$(JsonElement ClientQueryJE)
        {
            return AppEndDbIO.ClientQuery.Instance(ClientQueryJE).Exec();
        }";
        

    }
}
