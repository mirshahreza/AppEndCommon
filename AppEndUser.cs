using System.Collections;

namespace AppEnd
{
    public class AppEndUser
    {
        public string UserName { get; set; } = "";
        public string[] Roles { set; get; } = new string[] { };
        public Hashtable? ContextInfo { set; get; }
    }
}
