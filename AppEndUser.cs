namespace AppEnd
{
    public class AppEndUser
    {
        public string UserName { get; set; } = "";
        public string[] Roles { set; get; } = new string[] { };
        public object? ContextInfo { set; get; }
    }
}
