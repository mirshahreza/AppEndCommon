using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEnd
{
    public class AppEndUser
    {
        public string UserName { get; set; } = "";
        public string[] Roles { set; get; } = new string[] { };
        public object? ContextInfo { set; get; }
    }
}
