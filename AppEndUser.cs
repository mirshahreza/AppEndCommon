using System.Collections;

namespace AppEnd
{
    public class AppEndUser
    {
		public int Id { get; set; } = 0;
		public string UserName { get; set; } = "";
		public string[] Roles { set; get; } = [];
        public Hashtable? ContextInfo { set; get; }
    }
}
