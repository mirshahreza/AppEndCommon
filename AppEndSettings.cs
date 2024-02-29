using System.Text.Json;
using System.Text.Json.Nodes;

namespace AppEnd
{
	public static class AppEndSettings
    {

		public const string ConfigSectionName = "AppEnd";
		public static List<string> ReservedFolders = ["..lib", "..templates", "appendstudio", ".DbComponents", ".PublicComponents", ".SharedComponents"];

		private static JsonArray? dbServers;
        public static JsonArray DbServers
        {
            get
            {
                if (dbServers == null)
                {
                    if (AppSettings[ConfigSectionName] == null) AppSettings[ConfigSectionName] = JsonNode.Parse("{}")?.AsObject();
                    if (AppSettings[ConfigSectionName]?[nameof(DbServers)] == null)
                    {
						if (AppSettings[ConfigSectionName] == null)
						{
							throw new AppEndException("AppSettingsFileMustContains")
								.AddParam("Section", "AppEnd:ServerObjectsPath")
								.AddParam("Site", $"{System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.Name}, {System.Reflection.MethodBase.GetCurrentMethod()?.Name}")
								;
						}
						AppSettings[ConfigSectionName][nameof(DbServers)] = JsonNode.Parse("[]")?.AsArray();
                        string s = JsonSerializer.Serialize(AppSettings, options: new()
                        {
                            WriteIndented = true
                        });
                        File.WriteAllText("appsettings.json", s);
                        appsettings = null;
                    }
                    dbServers = AppSettings[ConfigSectionName]?[nameof(DbServers)]?.AsArray();
                }
                return dbServers;
            }
        }
        public static string WorkspacePath
        {
            get
            {
                return "workspace";
            }
        }
		public static string ServerObjectsPath
		{
			get
			{
				return $"{WorkspacePath}/server";
			}
		}
		public static string ApiCallsPath
		{
			get
			{
				return $"{WorkspacePath}/apicalls";
			}
		}
		public static string ClientObjectsPath
        {
            get
            {
                return $"{WorkspacePath}/client";
            }
        }

		public static string LogDbConfName
		{
			get
			{
				return AppEndSettings.AppSettings[ConfigSectionName]?[nameof(LogDbConfName)]?.ToString() ?? "DefaultRepo";
			}
		}
		public static int LogWriterQueueCap
		{
			get
			{
				return AppEndSettings.AppSettings[ConfigSectionName]?[nameof(LogWriterQueueCap)]?.ToIntSafe() ?? 0;
			}
		}
		public static string UsersListMethod
		{
			get
			{
				return AppEndSettings.AppSettings[ConfigSectionName]?[nameof(UsersListMethod)]?.ToString() ?? "DefaultRepo.AAA_Users.ReadListForLoginUse";
			}
		}
		public static string TalkPoint
		{
			get
			{
				return AppEndSettings.AppSettings[ConfigSectionName]?[nameof(TalkPoint)]?.ToString() ?? "talk-to-me";
			}
		}
		public static string PublicKeyRole
		{
			get
			{
				return AppEndSettings.AppSettings[ConfigSectionName]?[nameof(PublicKeyRole)]?.ToString() ?? "";
			}
		}
		public static string DefaultSuccessLoggerMethod
		{
			get
			{
				return AppEndSettings.AppSettings[ConfigSectionName]?[nameof(DefaultSuccessLoggerMethod)]?.ToString() ?? "";
			}
		}
		public static string DefaultErrorLoggerMethod
		{
			get
			{
				return AppEndSettings.AppSettings[ConfigSectionName]?[nameof(DefaultErrorLoggerMethod)]?.ToString() ?? "";
			}
		}
		public static string PublicKeyUser
		{
			get
			{
				return AppEndSettings.AppSettings[ConfigSectionName]?[nameof(PublicKeyUser)]?.ToString() ?? "";
			}
		}
		public static string[]? PublicMethods
		{
			get
			{
				return AppEndSettings.AppSettings[ConfigSectionName]?[nameof(PublicMethods)]?.ToString().DeserializeAsStringArray();
			}
		}

		public static string Secret
		{
			get
			{
				return AppEndSettings.AppSettings[ConfigSectionName]?[nameof(Secret)]?.ToString() ?? ConfigSectionName;
			}
		}


		private static JsonNode? appsettings;
        public static JsonNode AppSettings
        {
            get
            {
                if (!File.Exists("appsettings.json")) throw new AppEndException("AppSettingsFileIsNotExist")
                    .AddParam("Site", $"{System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.Name}, {System.Reflection.MethodBase.GetCurrentMethod()?.Name}")
                            ;

                appsettings ??= JsonNode.Parse(File.ReadAllText("appsettings.json"));

				if (appsettings is null) throw new AppEndException("AppSettingsFileIsNotExist")
					.AddParam("Site", $"{System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.Name}, {System.Reflection.MethodBase.GetCurrentMethod()?.Name}")
							;
				return appsettings;
            }
        }

        public static void RefereshSettings()
        {
            appsettings = null;
        }

    }
}
