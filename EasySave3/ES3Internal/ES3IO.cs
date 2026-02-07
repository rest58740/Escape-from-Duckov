using System;
using System.IO;
using UnityEngine;

namespace ES3Internal
{
	// Token: 0x020000D3 RID: 211
	public static class ES3IO
	{
		// Token: 0x0600041B RID: 1051 RVA: 0x0001ABD4 File Offset: 0x00018DD4
		public static DateTime GetTimestamp(string filePath)
		{
			if (!ES3IO.FileExists(filePath))
			{
				return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			}
			return File.GetLastWriteTime(filePath).ToUniversalTime();
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001AC09 File Offset: 0x00018E09
		public static string GetExtension(string path)
		{
			return Path.GetExtension(path);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001AC11 File Offset: 0x00018E11
		public static void DeleteFile(string filePath)
		{
			if (ES3IO.FileExists(filePath))
			{
				File.Delete(filePath);
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001AC21 File Offset: 0x00018E21
		public static bool FileExists(string filePath)
		{
			return File.Exists(filePath);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001AC29 File Offset: 0x00018E29
		public static void MoveFile(string sourcePath, string destPath)
		{
			File.Move(sourcePath, destPath);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001AC32 File Offset: 0x00018E32
		public static void CopyFile(string sourcePath, string destPath)
		{
			File.Copy(sourcePath, destPath);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001AC3B File Offset: 0x00018E3B
		public static void MoveDirectory(string sourcePath, string destPath)
		{
			Directory.Move(sourcePath, destPath);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001AC44 File Offset: 0x00018E44
		public static void CreateDirectory(string directoryPath)
		{
			Directory.CreateDirectory(directoryPath);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001AC4D File Offset: 0x00018E4D
		public static bool DirectoryExists(string directoryPath)
		{
			return Directory.Exists(directoryPath);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001AC58 File Offset: 0x00018E58
		public static string GetDirectoryPath(string path, char seperator = '/')
		{
			char value = ES3IO.UsesForwardSlash(path) ? '/' : '\\';
			int num = path.LastIndexOf(value);
			if (num == path.Length - 1)
			{
				return path;
			}
			if (num == path.Length - 1)
			{
				num = path.Substring(0, num).LastIndexOf(value);
			}
			if (num == -1)
			{
				ES3Debug.LogError("Path provided is not a directory path as it contains no slashes.", null, 0);
			}
			return path.Substring(0, num);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001ACBB File Offset: 0x00018EBB
		public static bool UsesForwardSlash(string path)
		{
			return path.Contains("/");
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001ACCD File Offset: 0x00018ECD
		public static string CombinePathAndFilename(string directoryPath, string fileOrDirectoryName)
		{
			if (directoryPath[directoryPath.Length - 1] != '/' && directoryPath[directoryPath.Length - 1] != '\\')
			{
				directoryPath += "/";
			}
			return directoryPath + fileOrDirectoryName;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001AD08 File Offset: 0x00018F08
		public static string[] GetDirectories(string path, bool getFullPaths = true)
		{
			string[] directories = Directory.GetDirectories(path);
			for (int i = 0; i < directories.Length; i++)
			{
				if (!getFullPaths)
				{
					directories[i] = Path.GetFileName(directories[i]);
				}
				directories[i].Replace("\\", "/");
			}
			return directories;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001AD4C File Offset: 0x00018F4C
		public static void DeleteDirectory(string directoryPath)
		{
			if (ES3IO.DirectoryExists(directoryPath))
			{
				Directory.Delete(directoryPath, true);
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001AD60 File Offset: 0x00018F60
		public static string[] GetFiles(string path, bool getFullPaths = true)
		{
			string[] files = Directory.GetFiles(ES3IO.GetDirectoryPath(path, '/'));
			if (!getFullPaths)
			{
				for (int i = 0; i < files.Length; i++)
				{
					files[i] = Path.GetFileName(files[i]);
				}
			}
			return files;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001AD98 File Offset: 0x00018F98
		public static byte[] ReadAllBytes(string path)
		{
			return File.ReadAllBytes(path);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001ADA0 File Offset: 0x00018FA0
		public static void WriteAllBytes(string path, byte[] bytes)
		{
			File.WriteAllBytes(path, bytes);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001ADAC File Offset: 0x00018FAC
		public static void CommitBackup(ES3Settings settings)
		{
			ES3Debug.Log("Committing backup for " + settings.path + " to storage location " + settings.location.ToString(), null, 0);
			string text = settings.FullPath + ".tmp";
			if (settings.location != ES3.Location.File)
			{
				if (settings.location == ES3.Location.PlayerPrefs)
				{
					PlayerPrefs.SetString(settings.FullPath, PlayerPrefs.GetString(text));
					PlayerPrefs.DeleteKey(text);
					PlayerPrefs.Save();
				}
				return;
			}
			string text2 = settings.FullPath + ".tmp.bak";
			if (ES3IO.FileExists(settings.FullPath))
			{
				ES3IO.DeleteFile(text2);
				ES3IO.CopyFile(settings.FullPath, text2);
				try
				{
					ES3IO.DeleteFile(settings.FullPath);
					ES3IO.MoveFile(text, settings.FullPath);
				}
				catch (Exception ex)
				{
					try
					{
						ES3IO.DeleteFile(settings.FullPath);
					}
					catch
					{
					}
					ES3IO.MoveFile(text2, settings.FullPath);
					throw ex;
				}
				ES3IO.DeleteFile(text2);
				return;
			}
			ES3IO.MoveFile(text, settings.FullPath);
		}

		// Token: 0x0400011F RID: 287
		internal static readonly string persistentDataPath = Application.persistentDataPath;

		// Token: 0x04000120 RID: 288
		internal static readonly string dataPath = Application.dataPath;

		// Token: 0x04000121 RID: 289
		internal const string backupFileSuffix = ".bac";

		// Token: 0x04000122 RID: 290
		internal const string temporaryFileSuffix = ".tmp";

		// Token: 0x02000105 RID: 261
		public enum ES3FileMode
		{
			// Token: 0x040001EE RID: 494
			Read,
			// Token: 0x040001EF RID: 495
			Write,
			// Token: 0x040001F0 RID: 496
			Append
		}
	}
}
