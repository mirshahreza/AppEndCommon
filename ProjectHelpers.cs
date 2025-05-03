using System.Text.Json;
using System.Text.Json.Nodes;

namespace AppEndCommon
{
	public static class ProjectHelpers
	{
		public static List<Role> ApplicationRoles = [];

		public static string EncriptionSecret => AppSettings["AppEnd"]?[nameof(EncriptionSecret)]?.ToString() ?? nameof(EncriptionSecret);
		public static string RootUserName => AppSettings["AppEnd"]?[nameof(RootUserName)]?.ToString() ?? nameof(RootUserName);
		public static string RootRoleName => AppSettings["AppEnd"]?[nameof(RootRoleName)]?.ToString() ?? nameof(RootRoleName);
		public static string[] PublicMethods => AppSettings["AppEnd"]?[nameof(PublicMethods)]?.ToString().DeserializeAsStringArray() ?? [];
		public static JsonNode AppEndSection => AppSettings["AppEnd"];

		public static DirectoryInfo ProjectRoot => new(".");

		private static JsonNode? _appsettings;
        public static JsonNode AppSettings
        {
            get
            {
                if (!File.Exists("appsettings.json")) throw new ExtException("AppSettingsFileIsNotExist", System.Reflection.MethodBase.GetCurrentMethod()).GetEx();
                _appsettings ??= JsonNode.Parse(File.ReadAllText("appsettings.json"));
				if (_appsettings is null) throw new ExtException("AppSettingsFileIsNotExist", System.Reflection.MethodBase.GetCurrentMethod()).GetEx();
				return _appsettings;
            }
        }

		public static string GetAppSettings(string key)
		{
			if (AppSettings[key] is null) throw new ExtException("AppSettingsKeyIsNotExist", System.Reflection.MethodBase.GetCurrentMethod()).GetEx();
			return AppSettings[key]?.ToString() ?? string.Empty;
		}

		public static string GetConnectionStringByName(string connectionStringName)
		{
			if (AppSettings["ConnectionStrings"] is null) throw new ExtException("AppSettingsKeyIsNotExist", System.Reflection.MethodBase.GetCurrentMethod()).GetEx();
			if (AppSettings["ConnectionStrings"]?[connectionStringName] is null) throw new ExtException("AppSettingsKeyIsNotExist", System.Reflection.MethodBase.GetCurrentMethod()).GetEx();
			return AppSettings["ConnectionStrings"]?[connectionStringName]?.ToString() ?? string.Empty;
		}

		public static string GetConnectionStringDefault()
		{
			if (AppSettings["ConnectionStrings"] is null) throw new ExtException("AppSettingsKeyIsNotExist", System.Reflection.MethodBase.GetCurrentMethod()).GetEx();
			if (AppSettings["ConnectionStrings"]?["DefaultConnection"] is null) throw new ExtException("AppSettingsKeyIsNotExist", System.Reflection.MethodBase.GetCurrentMethod()).GetEx();
			return AppSettings["ConnectionStrings"]?["DefaultConnection"]?.ToString() ?? string.Empty;
		}

		public static string GetConnectionStringFirstOne()
		{
			if (AppSettings["ConnectionStrings"] is null) throw new ExtException("AppSettingsKeyIsNotExist", System.Reflection.MethodBase.GetCurrentMethod()).GetEx();
			if (AppSettings["ConnectionStrings"]?[0] is null) throw new ExtException("AppSettingsKeyIsNotExist", System.Reflection.MethodBase.GetCurrentMethod()).GetEx();
			return AppSettings["ConnectionStrings"]?[0]?.ToString() ?? string.Empty;
		}


		public static void Save()
        {
			string appSettingsText = JsonSerializer.Serialize(AppSettings, options: new()
			{
				WriteIndented = true
			});
			File.WriteAllText("appsettings.json", appSettingsText);
			RefereshSettings();
		}

        public static void RefereshSettings()
        {
            _appsettings = null;
        }

    }
}
