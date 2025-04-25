﻿using System.Text.Json;

namespace AppEndCommon
{
    public static class ExtSystem
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

		public static Dictionary<string, Dictionary<string, string>> GetFilesRecursiveWithInfo(this DirectoryInfo directory, string? searchPattern = null)
		{
			Dictionary<string, Dictionary<string, string>> files = [];
			string[] arr = GetFilesRecursive(directory, searchPattern).ToArray();
			foreach(string filePath in arr)
			{
				Dictionary<string, string> fileAttrs = new();
				FileInfo fileInfo = new(filePath);
				fileAttrs.Add("FilePath", filePath);
				fileAttrs.Add("FileName", fileInfo.Name);
				fileAttrs.Add("LastWrite", fileInfo.LastWriteTime.ToString());
				files.Add(filePath, fileAttrs);
			}
			return files;
		}

		public static  void Delete(this DirectoryInfo directory, string? searchPattern = null)
		{
			FileInfo[] files = directory.GetFiles(searchPattern ?? "");
			foreach(FileInfo file in files) File.Delete(file.FullName);
		}

		public static void ValidateIfExist(this FileInfo fileInfo)
        {
            if (File.Exists(fileInfo.FullName)) new ExtException($"FileAlreadyExist", System.Reflection.MethodBase.GetCurrentMethod())
                    .AddParam("Path", fileInfo.FullName)
                    .GetEx()
                    ;
        }

        public static void ValidateIfNotExist(this FileInfo fileInfo)
        {
            if (!File.Exists(fileInfo.FullName)) new ExtException("FileDoesNotExist", System.Reflection.MethodBase.GetCurrentMethod())
                    .AddParam("Path", fileInfo.FullName)
                    .GetEx();
        }

        public static string GetCacheKeyForFiles(this FileInfo fileInfo)
        {
            return $"file::{fileInfo.FullName}";
        }

        public static bool IsFolder(string path)
        {
            if (Directory.Exists(path)) return true;
            return false;
        }
        public static bool IsFile(string path)
        {
            if (File.Exists(path)) return true;
            return false;
        }

    }
}