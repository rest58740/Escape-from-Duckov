using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Security.AccessControl;

namespace System.IO
{
	// Token: 0x02000B2E RID: 2862
	public static class Directory
	{
		// Token: 0x060066DF RID: 26335 RVA: 0x0015F464 File Offset: 0x0015D664
		public static DirectoryInfo GetParent(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Path cannot be the empty string or all whitespace.", "path");
			}
			string directoryName = Path.GetDirectoryName(Path.GetFullPath(path));
			if (directoryName == null)
			{
				return null;
			}
			return new DirectoryInfo(directoryName);
		}

		// Token: 0x060066E0 RID: 26336 RVA: 0x0015F4AE File Offset: 0x0015D6AE
		public static DirectoryInfo CreateDirectory(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Path cannot be the empty string or all whitespace.", "path");
			}
			string fullPath = Path.GetFullPath(path);
			FileSystem.CreateDirectory(fullPath);
			return new DirectoryInfo(fullPath, null, null, false);
		}

		// Token: 0x060066E1 RID: 26337 RVA: 0x0015F4EC File Offset: 0x0015D6EC
		public static bool Exists(string path)
		{
			try
			{
				if (path == null)
				{
					return false;
				}
				if (path.Length == 0)
				{
					return false;
				}
				return FileSystem.DirectoryExists(Path.GetFullPath(path));
			}
			catch (ArgumentException)
			{
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			return false;
		}

		// Token: 0x060066E2 RID: 26338 RVA: 0x0015F550 File Offset: 0x0015D750
		public static void SetCreationTime(string path, DateTime creationTime)
		{
			FileSystem.SetCreationTime(Path.GetFullPath(path), creationTime, true);
		}

		// Token: 0x060066E3 RID: 26339 RVA: 0x0015F564 File Offset: 0x0015D764
		public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
		{
			FileSystem.SetCreationTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(creationTimeUtc), true);
		}

		// Token: 0x060066E4 RID: 26340 RVA: 0x0015F578 File Offset: 0x0015D778
		public static DateTime GetCreationTime(string path)
		{
			return File.GetCreationTime(path);
		}

		// Token: 0x060066E5 RID: 26341 RVA: 0x0015F580 File Offset: 0x0015D780
		public static DateTime GetCreationTimeUtc(string path)
		{
			return File.GetCreationTimeUtc(path);
		}

		// Token: 0x060066E6 RID: 26342 RVA: 0x0015F588 File Offset: 0x0015D788
		public static void SetLastWriteTime(string path, DateTime lastWriteTime)
		{
			FileSystem.SetLastWriteTime(Path.GetFullPath(path), lastWriteTime, true);
		}

		// Token: 0x060066E7 RID: 26343 RVA: 0x0015F59C File Offset: 0x0015D79C
		public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
		{
			FileSystem.SetLastWriteTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(lastWriteTimeUtc), true);
		}

		// Token: 0x060066E8 RID: 26344 RVA: 0x0015F5B0 File Offset: 0x0015D7B0
		public static DateTime GetLastWriteTime(string path)
		{
			return File.GetLastWriteTime(path);
		}

		// Token: 0x060066E9 RID: 26345 RVA: 0x0015F5B8 File Offset: 0x0015D7B8
		public static DateTime GetLastWriteTimeUtc(string path)
		{
			return File.GetLastWriteTimeUtc(path);
		}

		// Token: 0x060066EA RID: 26346 RVA: 0x0015F5C0 File Offset: 0x0015D7C0
		public static void SetLastAccessTime(string path, DateTime lastAccessTime)
		{
			FileSystem.SetLastAccessTime(Path.GetFullPath(path), lastAccessTime, true);
		}

		// Token: 0x060066EB RID: 26347 RVA: 0x0015F5D4 File Offset: 0x0015D7D4
		public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
		{
			FileSystem.SetLastAccessTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(lastAccessTimeUtc), true);
		}

		// Token: 0x060066EC RID: 26348 RVA: 0x0015F5E8 File Offset: 0x0015D7E8
		public static DateTime GetLastAccessTime(string path)
		{
			return File.GetLastAccessTime(path);
		}

		// Token: 0x060066ED RID: 26349 RVA: 0x0015F5F0 File Offset: 0x0015D7F0
		public static DateTime GetLastAccessTimeUtc(string path)
		{
			return File.GetLastAccessTimeUtc(path);
		}

		// Token: 0x060066EE RID: 26350 RVA: 0x0015F5F8 File Offset: 0x0015D7F8
		public static string[] GetFiles(string path)
		{
			return Directory.GetFiles(path, "*", EnumerationOptions.Compatible);
		}

		// Token: 0x060066EF RID: 26351 RVA: 0x0015F60A File Offset: 0x0015D80A
		public static string[] GetFiles(string path, string searchPattern)
		{
			return Directory.GetFiles(path, searchPattern, EnumerationOptions.Compatible);
		}

		// Token: 0x060066F0 RID: 26352 RVA: 0x0015F618 File Offset: 0x0015D818
		public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
		{
			return Directory.GetFiles(path, searchPattern, EnumerationOptions.FromSearchOption(searchOption));
		}

		// Token: 0x060066F1 RID: 26353 RVA: 0x0015F627 File Offset: 0x0015D827
		public static string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
		{
			return Directory.InternalEnumeratePaths(path, searchPattern, SearchTarget.Files, enumerationOptions).ToArray<string>();
		}

		// Token: 0x060066F2 RID: 26354 RVA: 0x0015F637 File Offset: 0x0015D837
		public static string[] GetDirectories(string path)
		{
			return Directory.GetDirectories(path, "*", EnumerationOptions.Compatible);
		}

		// Token: 0x060066F3 RID: 26355 RVA: 0x0015F649 File Offset: 0x0015D849
		public static string[] GetDirectories(string path, string searchPattern)
		{
			return Directory.GetDirectories(path, searchPattern, EnumerationOptions.Compatible);
		}

		// Token: 0x060066F4 RID: 26356 RVA: 0x0015F657 File Offset: 0x0015D857
		public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
		{
			return Directory.GetDirectories(path, searchPattern, EnumerationOptions.FromSearchOption(searchOption));
		}

		// Token: 0x060066F5 RID: 26357 RVA: 0x0015F666 File Offset: 0x0015D866
		public static string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
		{
			return Directory.InternalEnumeratePaths(path, searchPattern, SearchTarget.Directories, enumerationOptions).ToArray<string>();
		}

		// Token: 0x060066F6 RID: 26358 RVA: 0x0015F676 File Offset: 0x0015D876
		public static string[] GetFileSystemEntries(string path)
		{
			return Directory.GetFileSystemEntries(path, "*", EnumerationOptions.Compatible);
		}

		// Token: 0x060066F7 RID: 26359 RVA: 0x0015F688 File Offset: 0x0015D888
		public static string[] GetFileSystemEntries(string path, string searchPattern)
		{
			return Directory.GetFileSystemEntries(path, searchPattern, EnumerationOptions.Compatible);
		}

		// Token: 0x060066F8 RID: 26360 RVA: 0x0015F696 File Offset: 0x0015D896
		public static string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
		{
			return Directory.GetFileSystemEntries(path, searchPattern, EnumerationOptions.FromSearchOption(searchOption));
		}

		// Token: 0x060066F9 RID: 26361 RVA: 0x0015F6A5 File Offset: 0x0015D8A5
		public static string[] GetFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
		{
			return Directory.InternalEnumeratePaths(path, searchPattern, SearchTarget.Both, enumerationOptions).ToArray<string>();
		}

		// Token: 0x060066FA RID: 26362 RVA: 0x0015F6B8 File Offset: 0x0015D8B8
		internal static IEnumerable<string> InternalEnumeratePaths(string path, string searchPattern, SearchTarget searchTarget, EnumerationOptions options)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			FileSystemEnumerableFactory.NormalizeInputs(ref path, ref searchPattern, options);
			switch (searchTarget)
			{
			case SearchTarget.Files:
				return FileSystemEnumerableFactory.UserFiles(path, searchPattern, options);
			case SearchTarget.Directories:
				return FileSystemEnumerableFactory.UserDirectories(path, searchPattern, options);
			case SearchTarget.Both:
				return FileSystemEnumerableFactory.UserEntries(path, searchPattern, options);
			default:
				throw new ArgumentOutOfRangeException("searchTarget");
			}
		}

		// Token: 0x060066FB RID: 26363 RVA: 0x0015F726 File Offset: 0x0015D926
		public static IEnumerable<string> EnumerateDirectories(string path)
		{
			return Directory.EnumerateDirectories(path, "*", EnumerationOptions.Compatible);
		}

		// Token: 0x060066FC RID: 26364 RVA: 0x0015F738 File Offset: 0x0015D938
		public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
		{
			return Directory.EnumerateDirectories(path, searchPattern, EnumerationOptions.Compatible);
		}

		// Token: 0x060066FD RID: 26365 RVA: 0x0015F746 File Offset: 0x0015D946
		public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
		{
			return Directory.EnumerateDirectories(path, searchPattern, EnumerationOptions.FromSearchOption(searchOption));
		}

		// Token: 0x060066FE RID: 26366 RVA: 0x0015F755 File Offset: 0x0015D955
		public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
		{
			return Directory.InternalEnumeratePaths(path, searchPattern, SearchTarget.Directories, enumerationOptions);
		}

		// Token: 0x060066FF RID: 26367 RVA: 0x0015F760 File Offset: 0x0015D960
		public static IEnumerable<string> EnumerateFiles(string path)
		{
			return Directory.EnumerateFiles(path, "*", EnumerationOptions.Compatible);
		}

		// Token: 0x06006700 RID: 26368 RVA: 0x0015F772 File Offset: 0x0015D972
		public static IEnumerable<string> EnumerateFiles(string path, string searchPattern)
		{
			return Directory.EnumerateFiles(path, searchPattern, EnumerationOptions.Compatible);
		}

		// Token: 0x06006701 RID: 26369 RVA: 0x0015F780 File Offset: 0x0015D980
		public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
		{
			return Directory.EnumerateFiles(path, searchPattern, EnumerationOptions.FromSearchOption(searchOption));
		}

		// Token: 0x06006702 RID: 26370 RVA: 0x0015F78F File Offset: 0x0015D98F
		public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
		{
			return Directory.InternalEnumeratePaths(path, searchPattern, SearchTarget.Files, enumerationOptions);
		}

		// Token: 0x06006703 RID: 26371 RVA: 0x0015F79A File Offset: 0x0015D99A
		public static IEnumerable<string> EnumerateFileSystemEntries(string path)
		{
			return Directory.EnumerateFileSystemEntries(path, "*", EnumerationOptions.Compatible);
		}

		// Token: 0x06006704 RID: 26372 RVA: 0x0015F7AC File Offset: 0x0015D9AC
		public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
		{
			return Directory.EnumerateFileSystemEntries(path, searchPattern, EnumerationOptions.Compatible);
		}

		// Token: 0x06006705 RID: 26373 RVA: 0x0015F7BA File Offset: 0x0015D9BA
		public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
		{
			return Directory.EnumerateFileSystemEntries(path, searchPattern, EnumerationOptions.FromSearchOption(searchOption));
		}

		// Token: 0x06006706 RID: 26374 RVA: 0x0015F7C9 File Offset: 0x0015D9C9
		public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
		{
			return Directory.InternalEnumeratePaths(path, searchPattern, SearchTarget.Both, enumerationOptions);
		}

		// Token: 0x06006707 RID: 26375 RVA: 0x0015F7D4 File Offset: 0x0015D9D4
		public static string GetDirectoryRoot(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			string fullPath = Path.GetFullPath(path);
			return fullPath.Substring(0, PathInternal.GetRootLength(fullPath));
		}

		// Token: 0x06006708 RID: 26376 RVA: 0x0015F808 File Offset: 0x0015DA08
		internal static string InternalGetDirectoryRoot(string path)
		{
			if (path == null)
			{
				return null;
			}
			return path.Substring(0, PathInternal.GetRootLength(path));
		}

		// Token: 0x06006709 RID: 26377 RVA: 0x0015F821 File Offset: 0x0015DA21
		public static string GetCurrentDirectory()
		{
			return Environment.CurrentDirectory;
		}

		// Token: 0x0600670A RID: 26378 RVA: 0x0015F828 File Offset: 0x0015DA28
		public static void SetCurrentDirectory(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Path cannot be the empty string or all whitespace.", "path");
			}
			Environment.CurrentDirectory = Path.GetFullPath(path);
		}

		// Token: 0x0600670B RID: 26379 RVA: 0x0015F85C File Offset: 0x0015DA5C
		public static void Move(string sourceDirName, string destDirName)
		{
			if (sourceDirName == null)
			{
				throw new ArgumentNullException("sourceDirName");
			}
			if (sourceDirName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "sourceDirName");
			}
			if (destDirName == null)
			{
				throw new ArgumentNullException("destDirName");
			}
			if (destDirName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "destDirName");
			}
			string fullPath = Path.GetFullPath(sourceDirName);
			string text = PathInternal.EnsureTrailingSeparator(fullPath);
			string fullPath2 = Path.GetFullPath(destDirName);
			string text2 = PathInternal.EnsureTrailingSeparator(fullPath2);
			StringComparison stringComparison = PathInternal.StringComparison;
			if (string.Equals(text, text2, stringComparison))
			{
				throw new IOException("Source and destination path must be different.");
			}
			string pathRoot = Path.GetPathRoot(text);
			string pathRoot2 = Path.GetPathRoot(text2);
			if (!string.Equals(pathRoot, pathRoot2, stringComparison))
			{
				throw new IOException("Source and destination path must have identical roots. Move will not work across volumes.");
			}
			if (!FileSystem.DirectoryExists(fullPath) && !FileSystem.FileExists(fullPath))
			{
				throw new DirectoryNotFoundException(SR.Format("Could not find a part of the path '{0}'.", fullPath));
			}
			if (FileSystem.DirectoryExists(fullPath2))
			{
				throw new IOException(SR.Format("Cannot create '{0}' because a file or directory with the same name already exists.", fullPath2));
			}
			FileSystem.MoveDirectory(fullPath, fullPath2);
		}

		// Token: 0x0600670C RID: 26380 RVA: 0x0015F94E File Offset: 0x0015DB4E
		public static void Delete(string path)
		{
			FileSystem.RemoveDirectory(Path.GetFullPath(path), false);
		}

		// Token: 0x0600670D RID: 26381 RVA: 0x0015F95C File Offset: 0x0015DB5C
		public static void Delete(string path, bool recursive)
		{
			FileSystem.RemoveDirectory(Path.GetFullPath(path), recursive);
		}

		// Token: 0x0600670E RID: 26382 RVA: 0x0015F96A File Offset: 0x0015DB6A
		public static string[] GetLogicalDrives()
		{
			return FileSystem.GetLogicalDrives();
		}

		// Token: 0x0600670F RID: 26383 RVA: 0x0015F971 File Offset: 0x0015DB71
		public static DirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
		{
			return Directory.CreateDirectory(path);
		}

		// Token: 0x06006710 RID: 26384 RVA: 0x0015F979 File Offset: 0x0015DB79
		public static DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
		{
			return new DirectorySecurity(path, includeSections);
		}

		// Token: 0x06006711 RID: 26385 RVA: 0x0015F982 File Offset: 0x0015DB82
		public static DirectorySecurity GetAccessControl(string path)
		{
			return Directory.GetAccessControl(path, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x06006712 RID: 26386 RVA: 0x0015F98C File Offset: 0x0015DB8C
		public static void SetAccessControl(string path, DirectorySecurity directorySecurity)
		{
			if (directorySecurity == null)
			{
				throw new ArgumentNullException("directorySecurity");
			}
			string fullPath = Path.GetFullPath(path);
			directorySecurity.PersistModifications(fullPath);
		}

		// Token: 0x06006713 RID: 26387 RVA: 0x0015F9B8 File Offset: 0x0015DBB8
		internal static string InsecureGetCurrentDirectory()
		{
			MonoIOError monoIOError;
			string currentDirectory = MonoIO.GetCurrentDirectory(out monoIOError);
			if (monoIOError != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(monoIOError);
			}
			return currentDirectory;
		}

		// Token: 0x06006714 RID: 26388 RVA: 0x0015F9D8 File Offset: 0x0015DBD8
		internal static void InsecureSetCurrentDirectory(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Trim().Length == 0)
			{
				throw new ArgumentException("path string must not be an empty string or whitespace string");
			}
			if (!Directory.Exists(path))
			{
				throw new DirectoryNotFoundException("Directory \"" + path + "\" not found.");
			}
			MonoIOError monoIOError;
			MonoIO.SetCurrentDirectory(path, out monoIOError);
			if (monoIOError != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(path, monoIOError);
			}
		}
	}
}
