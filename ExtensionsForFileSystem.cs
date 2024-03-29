﻿namespace AppEnd
{
    public static class ExtensionsForFileSystem
    {
        public static void CopyFilesRecursively(this DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories()) CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles()) file.CopyTo(Path.Combine(target.FullName, file.Name));
        }

        public static IEnumerable<string> GetFilesRecursive(this DirectoryInfo directory, string? searchPattern = null)
        {
            string path = directory.FullName;
            Queue<string> queue = new();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                string[]? files = null;
                try
                {
                    files = searchPattern is null || searchPattern == "" ? Directory.GetFiles(path) : Directory.GetFiles(path, searchPattern);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                if (files is not null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        yield return files[i];
                    }
                }
            }
        }

        public static void ValidateIfExist(this FileInfo fileInfo)
        {
            if (File.Exists(fileInfo.FullName)) new AppEndException($"FileAlreadyExist")
                    .AddParam("Path", fileInfo.FullName)
                    .AddParam("Site", $"{System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.Name}, {System.Reflection.MethodBase.GetCurrentMethod()?.Name}")
                    ;
        }

        public static void ValidateIfNotExist(this FileInfo fileInfo)
        {
            if (!File.Exists(fileInfo.FullName)) new AppEndException("FileDoesNotExist")
                    .AddParam("Path", fileInfo.FullName)
                    .AddParam("Site", $"{System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.Name}, {System.Reflection.MethodBase.GetCurrentMethod()?.Name}")
                    ;
        }

        public static string GetCacheKeyForFiles(this FileInfo fileInfo)
        {
            return $"file::{fileInfo.FullName}";
        }
    }
}