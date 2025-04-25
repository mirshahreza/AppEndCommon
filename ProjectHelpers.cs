using System.Text.Json;
using System.Text.Json.Nodes;

namespace AppEndCommon
{
	public static class ProjectHelpers
    {
		public static string EncriptionSecret => AppSettings["AppEnd"]?[nameof(EncriptionSecret)]?.ToString() ?? nameof(EncriptionSecret);
		public static string RootUserName => AppSettings["AppEnd"]?[nameof(RootUserName)]?.ToString() ?? nameof(RootUserName);
		public static string RootRoleName => AppSettings["AppEnd"]?[nameof(RootRoleName)]?.ToString() ?? nameof(RootRoleName);
		public static string[] PublicMethods => AppSettings["AppEnd"]?[nameof(PublicMethods)]?.ToString().DeserializeAsStringArray() ?? [];

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
