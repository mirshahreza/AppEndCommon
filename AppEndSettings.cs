﻿using System.Text.Json;
using System.Text.Json.Nodes;

namespace AppEnd
{
	public static class AppEndSettings
    {

		public const string ConfigSectionName = "AppEnd";
		public static List<string> ReservedFolders = ["..lib", "..templates", "appendstudio", ".DbComponents", ".PublicComponents", ".SharedComponents"];

		private static JsonArray? _dbServers;
        public static JsonArray DbServers
        {
            get
            {
                if (_dbServers is null)
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
                        _appsettings = null;
                    }
                    _dbServers = AppSettings[ConfigSectionName]?[nameof(DbServers)]?.AsArray();
                }
                return _dbServers;
            }
        }
        public static string WorkspacePath => "workspace";

        public static string ServerObjectsPath => $"{WorkspacePath}/server";

        public static string ApiCallsPath => $"{WorkspacePath}/apicalls";

        public static string ClientObjectsPath => $"{WorkspacePath}/client";

        public static string LoginDbConfName => AppSettings[ConfigSectionName]?[nameof(LoginDbConfName)]?.ToString() ?? "DefaultRepo";

        public static string LogDbConfName => AppSettings[ConfigSectionName]?[nameof(LogDbConfName)]?.ToString() ?? "DefaultRepo";

        public static int LogWriterQueueCap => AppSettings[ConfigSectionName]?[nameof(LogWriterQueueCap)]?.ToIntSafe() ?? 0;

        public static string TalkPoint => AppSettings[ConfigSectionName]?[nameof(TalkPoint)]?.ToString() ?? "talk-to-me";

        public static string PublicKeyRole => AppSettings[ConfigSectionName]?[nameof(PublicKeyRole)]?.ToString() ?? "";

        public static string DefaultSuccessLoggerMethod => AppSettings[ConfigSectionName]?[nameof(DefaultSuccessLoggerMethod)]?.ToString() ?? "";

        public static string DefaultErrorLoggerMethod => AppSettings[ConfigSectionName]?[nameof(DefaultErrorLoggerMethod)]?.ToString() ?? "";

        public static string PublicKeyUser => AppSettings[ConfigSectionName]?[nameof(PublicKeyUser)]?.ToString() ?? "";

        public static string[]? PublicMethods => AppSettings[ConfigSectionName]?[nameof(PublicMethods)]?.ToString().DeserializeAsStringArray();

        public static string Secret => AppSettings[ConfigSectionName]?[nameof(Secret)]?.ToString() ?? ConfigSectionName;


        private static JsonNode? _appsettings;
        public static JsonNode AppSettings
        {
            get
            {
                if (!File.Exists("appsettings.json")) throw new AppEndException("AppSettingsFileIsNotExist")
                    .AddParam("Site", $"{System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.Name}, {System.Reflection.MethodBase.GetCurrentMethod()?.Name}")
                            ;

                _appsettings ??= JsonNode.Parse(File.ReadAllText("appsettings.json"));

				if (_appsettings is null) throw new AppEndException("AppSettingsFileIsNotExist")
					.AddParam("Site", $"{System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.Name}, {System.Reflection.MethodBase.GetCurrentMethod()?.Name}")
							;
				return _appsettings;
            }
        }

        public static void RefereshSettings()
        {
            _appsettings = null;
        }

    }
}
