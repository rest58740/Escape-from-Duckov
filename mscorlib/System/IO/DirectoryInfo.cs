using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Runtime.Serialization;
using System.Security.AccessControl;

namespace System.IO
{
	// Token: 0x02000B2F RID: 2863
	[Serializable]
	public sealed class DirectoryInfo : FileSystemInfo
	{
		// Token: 0x06006715 RID: 26389 RVA: 0x0015FA3D File Offset: 0x0015DC3D
		public DirectoryInfo(string path)
		{
			this.Init(path, Path.GetFullPath(path), null, true);
		}

		// Token: 0x06006716 RID: 26390 RVA: 0x0015FA54 File Offset: 0x0015DC54
		internal DirectoryInfo(string originalPath, string fullPath = null, string fileName = null, bool isNormalized = false)
		{
			this.Init(originalPath, fullPath, fileName, isNormalized);
		}

		// Token: 0x06006717 RID: 26391 RVA: 0x0015FA68 File Offset: 0x0015DC68
		private void Init(string originalPath, string fullPath = null, string fileName = null, bool isNormalized = false)
		{
			if (originalPath == null)
			{
				throw new ArgumentNullException("path");
			}
			this.OriginalPath = originalPath;
			fullPath = (fullPath ?? originalPath);
			fullPath = (isNormalized ? fullPath : Path.GetFullPath(fullPath));
			this._name = (fileName ?? (PathInternal.IsRoot(fullPath) ? fullPath : Path.GetFileName(PathInternal.TrimEndingDirectorySeparator(fullPath.AsSpan()))).ToString());
			this.FullPath = fullPath;
		}

		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x06006718 RID: 26392 RVA: 0x0015FAE8 File Offset: 0x0015DCE8
		public DirectoryInfo Parent
		{
			get
			{
				string directoryName = Path.GetDirectoryName(PathInternal.IsRoot(this.FullPath) ? this.FullPath : PathInternal.TrimEndingDirectorySeparator(this.FullPath));
				if (directoryName == null)
				{
					return null;
				}
				return new DirectoryInfo(directoryName, null, null, false);
			}
		}

		// Token: 0x06006719 RID: 26393 RVA: 0x0015FB30 File Offset: 0x0015DD30
		public DirectoryInfo CreateSubdirectory(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (PathInternal.IsEffectivelyEmpty(path))
			{
				throw new ArgumentException("Path cannot be the empty string or all whitespace.", "path");
			}
			if (Path.IsPathRooted(path))
			{
				throw new ArgumentException("Second path fragment must not be a drive or UNC name.", "path");
			}
			string fullPath = Path.GetFullPath(Path.Combine(this.FullPath, path));
			ReadOnlySpan<char> span = PathInternal.TrimEndingDirectorySeparator(fullPath.AsSpan());
			ReadOnlySpan<char> value = PathInternal.TrimEndingDirectorySeparator(this.FullPath.AsSpan());
			if (span.StartsWith(value, PathInternal.StringComparison) && (span.Length == value.Length || PathInternal.IsDirectorySeparator(fullPath[value.Length])))
			{
				FileSystem.CreateDirectory(fullPath);
				return new DirectoryInfo(fullPath);
			}
			throw new ArgumentException(SR.Format("The directory specified, '{0}', is not a subdirectory of '{1}'.", path, this.FullPath), "path");
		}

		// Token: 0x0600671A RID: 26394 RVA: 0x0015FC09 File Offset: 0x0015DE09
		public void Create()
		{
			FileSystem.CreateDirectory(this.FullPath);
			base.Invalidate();
		}

		// Token: 0x0600671B RID: 26395 RVA: 0x0015FC1C File Offset: 0x0015DE1C
		public FileInfo[] GetFiles()
		{
			return this.GetFiles("*", EnumerationOptions.Compatible);
		}

		// Token: 0x0600671C RID: 26396 RVA: 0x0015FC2E File Offset: 0x0015DE2E
		public FileInfo[] GetFiles(string searchPattern)
		{
			return this.GetFiles(searchPattern, EnumerationOptions.Compatible);
		}

		// Token: 0x0600671D RID: 26397 RVA: 0x0015FC3C File Offset: 0x0015DE3C
		public FileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
		{
			return this.GetFiles(searchPattern, EnumerationOptions.FromSearchOption(searchOption));
		}

		// Token: 0x0600671E RID: 26398 RVA: 0x0015FC4B File Offset: 0x0015DE4B
		public FileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions)
		{
			return ((IEnumerable<FileInfo>)DirectoryInfo.InternalEnumerateInfos(this.FullPath, searchPattern, SearchTarget.Files, enumerationOptions)).ToArray<FileInfo>();
		}

		// Token: 0x0600671F RID: 26399 RVA: 0x0015FC65 File Offset: 0x0015DE65
		public FileSystemInfo[] GetFileSystemInfos()
		{
			return this.GetFileSystemInfos("*", EnumerationOptions.Compatible);
		}

		// Token: 0x06006720 RID: 26400 RVA: 0x0015FC77 File Offset: 0x0015DE77
		public FileSystemInfo[] GetFileSystemInfos(string searchPattern)
		{
			return this.GetFileSystemInfos(searchPattern, EnumerationOptions.Compatible);
		}

		// Token: 0x06006721 RID: 26401 RVA: 0x0015FC85 File Offset: 0x0015DE85
		public FileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
		{
			return this.GetFileSystemInfos(searchPattern, EnumerationOptions.FromSearchOption(searchOption));
		}

		// Token: 0x06006722 RID: 26402 RVA: 0x0015FC94 File Offset: 0x0015DE94
		public FileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
		{
			return DirectoryInfo.InternalEnumerateInfos(this.FullPath, searchPattern, SearchTarget.Both, enumerationOptions).ToArray<FileSystemInfo>();
		}

		// Token: 0x06006723 RID: 26403 RVA: 0x0015FCA9 File Offset: 0x0015DEA9
		public DirectoryInfo[] GetDirectories()
		{
			return this.GetDirectories("*", EnumerationOptions.Compatible);
		}

		// Token: 0x06006724 RID: 26404 RVA: 0x0015FCBB File Offset: 0x0015DEBB
		public DirectoryInfo[] GetDirectories(string searchPattern)
		{
			return this.GetDirectories(searchPattern, EnumerationOptions.Compatible);
		}

		// Token: 0x06006725 RID: 26405 RVA: 0x0015FCC9 File Offset: 0x0015DEC9
		public DirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
		{
			return this.GetDirectories(searchPattern, EnumerationOptions.FromSearchOption(searchOption));
		}

		// Token: 0x06006726 RID: 26406 RVA: 0x0015FCD8 File Offset: 0x0015DED8
		public DirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions)
		{
			return ((IEnumerable<DirectoryInfo>)DirectoryInfo.InternalEnumerateInfos(this.FullPath, searchPattern, SearchTarget.Directories, enumerationOptions)).ToArray<DirectoryInfo>();
		}

		// Token: 0x06006727 RID: 26407 RVA: 0x0015FCF2 File Offset: 0x0015DEF2
		public IEnumerable<DirectoryInfo> EnumerateDirectories()
		{
			return this.EnumerateDirectories("*", EnumerationOptions.Compatible);
		}

		// Token: 0x06006728 RID: 26408 RVA: 0x0015FD04 File Offset: 0x0015DF04
		public IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern)
		{
			return this.EnumerateDirectories(searchPattern, EnumerationOptions.Compatible);
		}

		// Token: 0x06006729 RID: 26409 RVA: 0x0015FD12 File Offset: 0x0015DF12
		public IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
		{
			return this.EnumerateDirectories(searchPattern, EnumerationOptions.FromSearchOption(searchOption));
		}

		// Token: 0x0600672A RID: 26410 RVA: 0x0015FD21 File Offset: 0x0015DF21
		public IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions)
		{
			return (IEnumerable<DirectoryInfo>)DirectoryInfo.InternalEnumerateInfos(this.FullPath, searchPattern, SearchTarget.Directories, enumerationOptions);
		}

		// Token: 0x0600672B RID: 26411 RVA: 0x0015FD36 File Offset: 0x0015DF36
		public IEnumerable<FileInfo> EnumerateFiles()
		{
			return this.EnumerateFiles("*", EnumerationOptions.Compatible);
		}

		// Token: 0x0600672C RID: 26412 RVA: 0x0015FD48 File Offset: 0x0015DF48
		public IEnumerable<FileInfo> EnumerateFiles(string searchPattern)
		{
			return this.EnumerateFiles(searchPattern, EnumerationOptions.Compatible);
		}

		// Token: 0x0600672D RID: 26413 RVA: 0x0015FD56 File Offset: 0x0015DF56
		public IEnumerable<FileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
		{
			return this.EnumerateFiles(searchPattern, EnumerationOptions.FromSearchOption(searchOption));
		}

		// Token: 0x0600672E RID: 26414 RVA: 0x0015FD65 File Offset: 0x0015DF65
		public IEnumerable<FileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions)
		{
			return (IEnumerable<FileInfo>)DirectoryInfo.InternalEnumerateInfos(this.FullPath, searchPattern, SearchTarget.Files, enumerationOptions);
		}

		// Token: 0x0600672F RID: 26415 RVA: 0x0015FD7A File Offset: 0x0015DF7A
		public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos()
		{
			return this.EnumerateFileSystemInfos("*", EnumerationOptions.Compatible);
		}

		// Token: 0x06006730 RID: 26416 RVA: 0x0015FD8C File Offset: 0x0015DF8C
		public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
		{
			return this.EnumerateFileSystemInfos(searchPattern, EnumerationOptions.Compatible);
		}

		// Token: 0x06006731 RID: 26417 RVA: 0x0015FD9A File Offset: 0x0015DF9A
		public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
		{
			return this.EnumerateFileSystemInfos(searchPattern, EnumerationOptions.FromSearchOption(searchOption));
		}

		// Token: 0x06006732 RID: 26418 RVA: 0x0015FDA9 File Offset: 0x0015DFA9
		public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
		{
			return DirectoryInfo.InternalEnumerateInfos(this.FullPath, searchPattern, SearchTarget.Both, enumerationOptions);
		}

		// Token: 0x06006733 RID: 26419 RVA: 0x0015FDBC File Offset: 0x0015DFBC
		internal static IEnumerable<FileSystemInfo> InternalEnumerateInfos(string path, string searchPattern, SearchTarget searchTarget, EnumerationOptions options)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			FileSystemEnumerableFactory.NormalizeInputs(ref path, ref searchPattern, options);
			switch (searchTarget)
			{
			case SearchTarget.Files:
				return FileSystemEnumerableFactory.FileInfos(path, searchPattern, options);
			case SearchTarget.Directories:
				return FileSystemEnumerableFactory.DirectoryInfos(path, searchPattern, options);
			case SearchTarget.Both:
				return FileSystemEnumerableFactory.FileSystemInfos(path, searchPattern, options);
			default:
				throw new ArgumentException("Enum value was out of legal range.", "searchTarget");
			}
		}

		// Token: 0x170011E0 RID: 4576
		// (get) Token: 0x06006734 RID: 26420 RVA: 0x0015FE21 File Offset: 0x0015E021
		public DirectoryInfo Root
		{
			get
			{
				return new DirectoryInfo(Path.GetPathRoot(this.FullPath));
			}
		}

		// Token: 0x06006735 RID: 26421 RVA: 0x0015FE34 File Offset: 0x0015E034
		public void MoveTo(string destDirName)
		{
			if (destDirName == null)
			{
				throw new ArgumentNullException("destDirName");
			}
			if (destDirName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "destDirName");
			}
			string fullPath = Path.GetFullPath(destDirName);
			string text = PathInternal.EnsureTrailingSeparator(fullPath);
			string text2 = PathInternal.EnsureTrailingSeparator(this.FullPath);
			if (string.Equals(text2, text, PathInternal.StringComparison))
			{
				throw new IOException("Source and destination path must be different.");
			}
			string pathRoot = Path.GetPathRoot(text2);
			string pathRoot2 = Path.GetPathRoot(text);
			if (!string.Equals(pathRoot, pathRoot2, PathInternal.StringComparison))
			{
				throw new IOException("Source and destination path must have identical roots. Move will not work across volumes.");
			}
			if (!this.Exists && !FileSystem.FileExists(this.FullPath))
			{
				throw new DirectoryNotFoundException(SR.Format("Could not find a part of the path '{0}'.", this.FullPath));
			}
			if (FileSystem.DirectoryExists(fullPath))
			{
				throw new IOException(SR.Format("Cannot create '{0}' because a file or directory with the same name already exists.", text));
			}
			FileSystem.MoveDirectory(this.FullPath, fullPath);
			this.Init(destDirName, text, null, true);
			base.Invalidate();
		}

		// Token: 0x06006736 RID: 26422 RVA: 0x0015FF1D File Offset: 0x0015E11D
		public override void Delete()
		{
			FileSystem.RemoveDirectory(this.FullPath, false);
		}

		// Token: 0x06006737 RID: 26423 RVA: 0x0015FF2B File Offset: 0x0015E12B
		public void Delete(bool recursive)
		{
			FileSystem.RemoveDirectory(this.FullPath, recursive);
		}

		// Token: 0x06006738 RID: 26424 RVA: 0x0015FF39 File Offset: 0x0015E139
		private DirectoryInfo(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06006739 RID: 26425 RVA: 0x0015FF43 File Offset: 0x0015E143
		public void Create(DirectorySecurity directorySecurity)
		{
			FileSystem.CreateDirectory(this.FullPath);
		}

		// Token: 0x0600673A RID: 26426 RVA: 0x0015FF50 File Offset: 0x0015E150
		public DirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity)
		{
			return this.CreateSubdirectory(path);
		}

		// Token: 0x0600673B RID: 26427 RVA: 0x0015FF59 File Offset: 0x0015E159
		public DirectorySecurity GetAccessControl()
		{
			return Directory.GetAccessControl(this.FullPath, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x0600673C RID: 26428 RVA: 0x0015FF68 File Offset: 0x0015E168
		public DirectorySecurity GetAccessControl(AccessControlSections includeSections)
		{
			return Directory.GetAccessControl(this.FullPath, includeSections);
		}

		// Token: 0x0600673D RID: 26429 RVA: 0x0015FF76 File Offset: 0x0015E176
		public void SetAccessControl(DirectorySecurity directorySecurity)
		{
			Directory.SetAccessControl(this.FullPath, directorySecurity);
		}
	}
}
