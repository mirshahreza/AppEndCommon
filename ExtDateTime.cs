namespace AppEndCommon
{
    public static class ExtDateTime
	{
        public static string ToAppEndStandard(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:MM:ss t" + "M");
		}
    }
}