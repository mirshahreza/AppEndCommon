﻿using AppEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AppEnd
{
    public class AppEndSettings
    {
        private static string? serverObjectsPath;
        private static string? clientObjectsPath;
        private static JsonArray? dbServers;
        public static JsonArray DbServers
        {
            get
            {
                if (dbServers == null)
                {
                    if (AppSettings["AppEnd"] == null)
                    {
                        AppSettings["AppEnd"] = JsonNode.Parse("{}")?.AsObject();
                    }
                    if (AppSettings["AppEnd"]?["DbServers"] == null)
                    {
                        AppSettings["AppEnd"]["DbServers"] = JsonNode.Parse("[]")?.AsArray();
                        string s = JsonSerializer.Serialize(AppSettings, options: new()
                        {
                            WriteIndented = true
                        });
                        File.WriteAllText("appsettings.json", s);
                        appsettings = null;
                    }
                    dbServers = AppSettings["AppEnd"]?["DbServers"]?.AsArray();
                }
                return dbServers;
            }
        }
        public static string WorkspacePath
        {
            get
            {
                return AppSettings["AppEnd"]?["WorkspacePath"]?.AsValue().ToString() ?? "workspace";
            }
        }
        public static string ServerObjectsPath
        {
            get
            {
                if (serverObjectsPath == null)
                {
                    if (AppSettings["AppEnd"] == null)
                    {
                        throw new AppEndException("AppSettingsFileMustContains")
                            .AddParam("Section", "AppEnd:ServerObjectsPath")
                    .AddParam("Site", $"{System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.Name}, {System.Reflection.MethodBase.GetCurrentMethod()?.Name}")
                            ;
                    }
                    serverObjectsPath = $"{WorkspacePath}/{(AppSettings["AppEnd"]?.AsObject()?["ServerObjectsPath"]?.ToString())}";
                }
                return serverObjectsPath;
            }
        }
        public static string ClientObjectsPath
        {
            get
            {
                if (clientObjectsPath == null)
                {
                    if (AppSettings["AppEnd"] == null)
                    {
                        throw new AppEndException("AppSettingsFileMustContains")
                            .AddParam("Section", "AppEnd:ClientObjectsPath")
                    .AddParam("Site", $"{System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.Name}, {System.Reflection.MethodBase.GetCurrentMethod()?.Name}")
                            ;
                    }
                    clientObjectsPath = $"{WorkspacePath}/{(AppSettings["AppEnd"]?.AsObject()?["ClientObjectsPath"]?.ToString())}";
                }
                return clientObjectsPath;
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
                if (appsettings is null) appsettings = JsonNode.Parse(File.ReadAllText("appsettings.json"));
                return appsettings;
            }
        }

        public static void RefereshSettings()
        {
            appsettings = null;
        }

    }
}
