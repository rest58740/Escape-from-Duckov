using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	// Token: 0x02000B31 RID: 2865
	public static class File
	{
		// Token: 0x06006752 RID: 26450 RVA: 0x001600AC File Offset: 0x0015E2AC
		public static StreamReader OpenText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return new StreamReader(path);
		}

		// Token: 0x06006753 RID: 26451 RVA: 0x001600C2 File Offset: 0x0015E2C2
		public static StreamWriter CreateText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return new StreamWriter(path, false);
		}

		// Token: 0x06006754 RID: 26452 RVA: 0x001600D9 File Offset: 0x0015E2D9
		public static StreamWriter AppendText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return new StreamWriter(path, true);
		}

		// Token: 0x06006755 RID: 26453 RVA: 0x001600F0 File Offset: 0x0015E2F0
		public static void Copy(string sourceFileName, string destFileName)
		{
			File.Copy(sourceFileName, destFileName, false);
		}

		// Token: 0x06006756 RID: 26454 RVA: 0x001600FC File Offset: 0x0015E2FC
		public static void Copy(string sourceFileName, string destFileName, bool overwrite)
		{
			if (sourceFileName == null)
			{
				throw new ArgumentNullException("sourceFileName", "File name cannot be null.");
			}
			if (destFileName == null)
			{
				throw new ArgumentNullException("destFileName", "File name cannot be null.");
			}
			if (sourceFileName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "sourceFileName");
			}
			if (destFileName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "destFileName");
			}
			FileSystem.CopyFile(Path.GetFullPath(sourceFileName), Path.GetFullPath(destFileName), overwrite);
		}

		// Token: 0x06006757 RID: 26455 RVA: 0x00160171 File Offset: 0x0015E371
		public static FileStream Create(string path)
		{
			return File.Create(path, 4096);
		}

		// Token: 0x06006758 RID: 26456 RVA: 0x0016017E File Offset: 0x0015E37E
		public static FileStream Create(string path, int bufferSize)
		{
			return new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize);
		}

		// Token: 0x06006759 RID: 26457 RVA: 0x0016018A File Offset: 0x0015E38A
		public static FileStream Create(string path, int bufferSize, FileOptions options)
		{
			return new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, options);
		}

		// Token: 0x0600675A RID: 26458 RVA: 0x00160197 File Offset: 0x0015E397
		public static void Delete(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			FileSystem.DeleteFile(Path.GetFullPath(path));
		}

		// Token: 0x0600675B RID: 26459 RVA: 0x001601B4 File Offset: 0x0015E3B4
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
				path = Path.GetFullPath(path);
				if (path.Length > 0 && PathInternal.IsDirectorySeparator(path[path.Length - 1]))
				{
					return false;
				}
				return FileSystem.FileExists(path);
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

		// Token: 0x0600675C RID: 26460 RVA: 0x0016023C File Offset: 0x0015E43C
		public static FileStream Open(string path, FileMode mode)
		{
			return File.Open(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);
		}

		// Token: 0x0600675D RID: 26461 RVA: 0x0016024E File Offset: 0x0015E44E
		public static FileStream Open(string path, FileMode mode, FileAccess access)
		{
			return File.Open(path, mode, access, FileShare.None);
		}

		// Token: 0x0600675E RID: 26462 RVA: 0x00160259 File Offset: 0x0015E459
		public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
		{
			return new FileStream(path, mode, access, share);
		}

		// Token: 0x0600675F RID: 26463 RVA: 0x00160264 File Offset: 0x0015E464
		internal static DateTimeOffset GetUtcDateTimeOffset(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Unspecified)
			{
				return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
			}
			return dateTime.ToUniversalTime();
		}

		// Token: 0x06006760 RID: 26464 RVA: 0x00160288 File Offset: 0x0015E488
		public static void SetCreationTime(string path, DateTime creationTime)
		{
			FileSystem.SetCreationTime(Path.GetFullPath(path), creationTime, false);
		}

		// Token: 0x06006761 RID: 26465 RVA: 0x0016029C File Offset: 0x0015E49C
		public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
		{
			FileSystem.SetCreationTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(creationTimeUtc), false);
		}

		// Token: 0x06006762 RID: 26466 RVA: 0x001602B0 File Offset: 0x0015E4B0
		public static DateTime GetCreationTime(string path)
		{
			return FileSystem.GetCreationTime(Path.GetFullPath(path)).LocalDateTime;
		}

		// Token: 0x06006763 RID: 26467 RVA: 0x001602D0 File Offset: 0x0015E4D0
		public static DateTime GetCreationTimeUtc(string path)
		{
			return FileSystem.GetCreationTime(Path.GetFullPath(path)).UtcDateTime;
		}

		// Token: 0x06006764 RID: 26468 RVA: 0x001602F0 File Offset: 0x0015E4F0
		public static void SetLastAccessTime(string path, DateTime lastAccessTime)
		{
			FileSystem.SetLastAccessTime(Path.GetFullPath(path), lastAccessTime, false);
		}

		// Token: 0x06006765 RID: 26469 RVA: 0x00160304 File Offset: 0x0015E504
		public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
		{
			FileSystem.SetLastAccessTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(lastAccessTimeUtc), false);
		}

		// Token: 0x06006766 RID: 26470 RVA: 0x00160318 File Offset: 0x0015E518
		public static DateTime GetLastAccessTime(string path)
		{
			return FileSystem.GetLastAccessTime(Path.GetFullPath(path)).LocalDateTime;
		}

		// Token: 0x06006767 RID: 26471 RVA: 0x00160338 File Offset: 0x0015E538
		public static DateTime GetLastAccessTimeUtc(string path)
		{
			return FileSystem.GetLastAccessTime(Path.GetFullPath(path)).UtcDateTime;
		}

		// Token: 0x06006768 RID: 26472 RVA: 0x00160358 File Offset: 0x0015E558
		public static void SetLastWriteTime(string path, DateTime lastWriteTime)
		{
			FileSystem.SetLastWriteTime(Path.GetFullPath(path), lastWriteTime, false);
		}

		// Token: 0x06006769 RID: 26473 RVA: 0x0016036C File Offset: 0x0015E56C
		public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
		{
			FileSystem.SetLastWriteTime(Path.GetFullPath(path), File.GetUtcDateTimeOffset(lastWriteTimeUtc), false);
		}

		// Token: 0x0600676A RID: 26474 RVA: 0x00160380 File Offset: 0x0015E580
		public static DateTime GetLastWriteTime(string path)
		{
			return FileSystem.GetLastWriteTime(Path.GetFullPath(path)).LocalDateTime;
		}

		// Token: 0x0600676B RID: 26475 RVA: 0x001603A0 File Offset: 0x0015E5A0
		public static DateTime GetLastWriteTimeUtc(string path)
		{
			return FileSystem.GetLastWriteTime(Path.GetFullPath(path)).UtcDateTime;
		}

		// Token: 0x0600676C RID: 26476 RVA: 0x001603C0 File Offset: 0x0015E5C0
		public static FileAttributes GetAttributes(string path)
		{
			return FileSystem.GetAttributes(Path.GetFullPath(path));
		}

		// Token: 0x0600676D RID: 26477 RVA: 0x001603D0 File Offset: 0x0015E5D0
		public static void SetAttributes(string path, FileAttributes fileAttributes)
		{
			if ((fileAttributes & (FileAttributes)(-2147483648)) == (FileAttributes)0)
			{
				FileSystem.SetAttributes(Path.GetFullPath(path), fileAttributes);
				return;
			}
			Path.Validate(path);
			MonoIOError error;
			if (!MonoIO.SetFileAttributes(path, fileAttributes, out error))
			{
				throw MonoIO.GetException(path, error);
			}
		}

		// Token: 0x0600676E RID: 26478 RVA: 0x0016040C File Offset: 0x0015E60C
		public static FileStream OpenRead(string path)
		{
			return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		// Token: 0x0600676F RID: 26479 RVA: 0x00160417 File Offset: 0x0015E617
		public static FileStream OpenWrite(string path)
		{
			return new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}

		// Token: 0x06006770 RID: 26480 RVA: 0x00160422 File Offset: 0x0015E622
		public static string ReadAllText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			return File.InternalReadAllText(path, Encoding.UTF8);
		}

		// Token: 0x06006771 RID: 26481 RVA: 0x00160455 File Offset: 0x0015E655
		public static string ReadAllText(string path, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			return File.InternalReadAllText(path, encoding);
		}

		// Token: 0x06006772 RID: 26482 RVA: 0x00160494 File Offset: 0x0015E694
		private static string InternalReadAllText(string path, Encoding encoding)
		{
			string result;
			using (StreamReader streamReader = new StreamReader(path, encoding, true))
			{
				result = streamReader.ReadToEnd();
			}
			return result;
		}

		// Token: 0x06006773 RID: 26483 RVA: 0x001604D0 File Offset: 0x0015E6D0
		public static void WriteAllText(string path, string contents)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				streamWriter.Write(contents);
			}
		}

		// Token: 0x06006774 RID: 26484 RVA: 0x00160530 File Offset: 0x0015E730
		public static void WriteAllText(string path, string contents, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			using (StreamWriter streamWriter = new StreamWriter(path, false, encoding))
			{
				streamWriter.Write(contents);
			}
		}

		// Token: 0x06006775 RID: 26485 RVA: 0x001605A0 File Offset: 0x0015E7A0
		public static byte[] ReadAllBytes(string path)
		{
			byte[] result;
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1))
			{
				long length = fileStream.Length;
				if (length > 2147483647L)
				{
					throw new IOException("The file is too long. This operation is currently limited to supporting files less than 2 gigabytes in size.");
				}
				if (length == 0L)
				{
					result = File.ReadAllBytesUnknownLength(fileStream);
				}
				else
				{
					int num = 0;
					int i = (int)length;
					byte[] array = new byte[i];
					while (i > 0)
					{
						int num2 = fileStream.Read(array, num, i);
						if (num2 == 0)
						{
							throw Error.GetEndOfFile();
						}
						num += num2;
						i -= num2;
					}
					result = array;
				}
			}
			return result;
		}

		// Token: 0x06006776 RID: 26486 RVA: 0x00160638 File Offset: 0x0015E838
		private unsafe static byte[] ReadAllBytesUnknownLength(FileStream fs)
		{
			byte[] array = null;
			Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)512], 512);
			byte[] result;
			try
			{
				int num = 0;
				for (;;)
				{
					if (num == span.Length)
					{
						uint num2 = (uint)(span.Length * 2);
						if (num2 > 2147483591U)
						{
							num2 = (uint)Math.Max(2147483591, span.Length + 1);
						}
						byte[] array2 = ArrayPool<byte>.Shared.Rent((int)num2);
						span.CopyTo(array2);
						if (array != null)
						{
							ArrayPool<byte>.Shared.Return(array, false);
						}
						span = (array = array2);
					}
					int num3 = fs.Read(span.Slice(num));
					if (num3 == 0)
					{
						break;
					}
					num += num3;
				}
				result = span.Slice(0, num).ToArray();
			}
			finally
			{
				if (array != null)
				{
					ArrayPool<byte>.Shared.Return(array, false);
				}
			}
			return result;
		}

		// Token: 0x06006777 RID: 26487 RVA: 0x00160718 File Offset: 0x0015E918
		public static void WriteAllBytes(string path, byte[] bytes)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path", "Path cannot be null.");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			File.InternalWriteAllBytes(path, bytes);
		}

		// Token: 0x06006778 RID: 26488 RVA: 0x00160768 File Offset: 0x0015E968
		private static void InternalWriteAllBytes(string path, byte[] bytes)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				fileStream.Write(bytes, 0, bytes.Length);
			}
		}

		// Token: 0x06006779 RID: 26489 RVA: 0x001607A8 File Offset: 0x0015E9A8
		public static string[] ReadAllLines(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			return File.InternalReadAllLines(path, Encoding.UTF8);
		}

		// Token: 0x0600677A RID: 26490 RVA: 0x001607DB File Offset: 0x0015E9DB
		public static string[] ReadAllLines(string path, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			return File.InternalReadAllLines(path, encoding);
		}

		// Token: 0x0600677B RID: 26491 RVA: 0x00160818 File Offset: 0x0015EA18
		private static string[] InternalReadAllLines(string path, Encoding encoding)
		{
			List<string> list = new List<string>();
			using (StreamReader streamReader = new StreamReader(path, encoding))
			{
				string item;
				while ((item = streamReader.ReadLine()) != null)
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600677C RID: 26492 RVA: 0x00160868 File Offset: 0x0015EA68
		public static IEnumerable<string> ReadLines(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			return ReadLinesIterator.CreateIterator(path, Encoding.UTF8);
		}

		// Token: 0x0600677D RID: 26493 RVA: 0x0016089B File Offset: 0x0015EA9B
		public static IEnumerable<string> ReadLines(string path, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			return ReadLinesIterator.CreateIterator(path, encoding);
		}

		// Token: 0x0600677E RID: 26494 RVA: 0x001608D8 File Offset: 0x0015EAD8
		public static void WriteAllLines(string path, string[] contents)
		{
			File.WriteAllLines(path, contents);
		}

		// Token: 0x0600677F RID: 26495 RVA: 0x001608E4 File Offset: 0x0015EAE4
		public static void WriteAllLines(string path, IEnumerable<string> contents)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			File.InternalWriteAllLines(new StreamWriter(path), contents);
		}

		// Token: 0x06006780 RID: 26496 RVA: 0x00160931 File Offset: 0x0015EB31
		public static void WriteAllLines(string path, string[] contents, Encoding encoding)
		{
			File.WriteAllLines(path, contents, encoding);
		}

		// Token: 0x06006781 RID: 26497 RVA: 0x0016093C File Offset: 0x0015EB3C
		public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			File.InternalWriteAllLines(new StreamWriter(path, false, encoding), contents);
		}

		// Token: 0x06006782 RID: 26498 RVA: 0x0016099C File Offset: 0x0015EB9C
		private static void InternalWriteAllLines(TextWriter writer, IEnumerable<string> contents)
		{
			try
			{
				foreach (string value in contents)
				{
					writer.WriteLine(value);
				}
			}
			finally
			{
				if (writer != null)
				{
					((IDisposable)writer).Dispose();
				}
			}
		}

		// Token: 0x06006783 RID: 26499 RVA: 0x001609FC File Offset: 0x0015EBFC
		public static void AppendAllText(string path, string contents)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			using (StreamWriter streamWriter = new StreamWriter(path, true))
			{
				streamWriter.Write(contents);
			}
		}

		// Token: 0x06006784 RID: 26500 RVA: 0x00160A5C File Offset: 0x0015EC5C
		public static void AppendAllText(string path, string contents, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			using (StreamWriter streamWriter = new StreamWriter(path, true, encoding))
			{
				streamWriter.Write(contents);
			}
		}

		// Token: 0x06006785 RID: 26501 RVA: 0x00160ACC File Offset: 0x0015ECCC
		public static void AppendAllLines(string path, IEnumerable<string> contents)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			File.InternalWriteAllLines(new StreamWriter(path, true), contents);
		}

		// Token: 0x06006786 RID: 26502 RVA: 0x00160B1C File Offset: 0x0015ED1C
		public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			File.InternalWriteAllLines(new StreamWriter(path, true, encoding), contents);
		}

		// Token: 0x06006787 RID: 26503 RVA: 0x00160B79 File Offset: 0x0015ED79
		public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
		{
			File.Replace(sourceFileName, destinationFileName, destinationBackupFileName, false);
		}

		// Token: 0x06006788 RID: 26504 RVA: 0x00160B84 File Offset: 0x0015ED84
		public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
		{
			if (sourceFileName == null)
			{
				throw new ArgumentNullException("sourceFileName");
			}
			if (destinationFileName == null)
			{
				throw new ArgumentNullException("destinationFileName");
			}
			FileSystem.ReplaceFile(Path.GetFullPath(sourceFileName), Path.GetFullPath(destinationFileName), (destinationBackupFileName != null) ? Path.GetFullPath(destinationBackupFileName) : null, ignoreMetadataErrors);
		}

		// Token: 0x06006789 RID: 26505 RVA: 0x00160BC0 File Offset: 0x0015EDC0
		public static void Move(string sourceFileName, string destFileName)
		{
			if (sourceFileName == null)
			{
				throw new ArgumentNullException("sourceFileName", "File name cannot be null.");
			}
			if (destFileName == null)
			{
				throw new ArgumentNullException("destFileName", "File name cannot be null.");
			}
			if (sourceFileName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "sourceFileName");
			}
			if (destFileName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "destFileName");
			}
			string fullPath = Path.GetFullPath(sourceFileName);
			string fullPath2 = Path.GetFullPath(destFileName);
			if (!FileSystem.FileExists(fullPath))
			{
				throw new FileNotFoundException(SR.Format("Could not find file '{0}'.", fullPath), fullPath);
			}
			FileSystem.MoveFile(fullPath, fullPath2);
		}

		// Token: 0x0600678A RID: 26506 RVA: 0x00160C52 File Offset: 0x0015EE52
		public static void Encrypt(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			throw new PlatformNotSupportedException("File encryption is not supported on this platform.");
		}

		// Token: 0x0600678B RID: 26507 RVA: 0x00160C52 File Offset: 0x0015EE52
		public static void Decrypt(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			throw new PlatformNotSupportedException("File encryption is not supported on this platform.");
		}

		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x0600678C RID: 26508 RVA: 0x00160C6C File Offset: 0x0015EE6C
		private static Encoding UTF8NoBOM
		{
			get
			{
				Encoding result;
				if ((result = File.s_UTF8NoBOM) == null)
				{
					result = (File.s_UTF8NoBOM = new UTF8Encoding(false, true));
				}
				return result;
			}
		}

		// Token: 0x0600678D RID: 26509 RVA: 0x00160C84 File Offset: 0x0015EE84
		private static StreamReader AsyncStreamReader(string path, Encoding encoding)
		{
			return new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan), encoding, true);
		}

		// Token: 0x0600678E RID: 26510 RVA: 0x00160CA0 File Offset: 0x0015EEA0
		private static StreamWriter AsyncStreamWriter(string path, Encoding encoding, bool append)
		{
			return new StreamWriter(new FileStream(path, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan), encoding);
		}

		// Token: 0x0600678F RID: 26511 RVA: 0x00160CC1 File Offset: 0x0015EEC1
		public static Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
		{
			return File.ReadAllTextAsync(path, Encoding.UTF8, cancellationToken);
		}

		// Token: 0x06006790 RID: 26512 RVA: 0x00160CD0 File Offset: 0x0015EED0
		public static Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				return File.InternalReadAllTextAsync(path, encoding, cancellationToken);
			}
			return Task.FromCanceled<string>(cancellationToken);
		}

		// Token: 0x06006791 RID: 26513 RVA: 0x00160D2C File Offset: 0x0015EF2C
		private static Task<string> InternalReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken)
		{
			File.<InternalReadAllTextAsync>d__67 <InternalReadAllTextAsync>d__;
			<InternalReadAllTextAsync>d__.path = path;
			<InternalReadAllTextAsync>d__.encoding = encoding;
			<InternalReadAllTextAsync>d__.cancellationToken = cancellationToken;
			<InternalReadAllTextAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<InternalReadAllTextAsync>d__.<>1__state = -1;
			<InternalReadAllTextAsync>d__.<>t__builder.Start<File.<InternalReadAllTextAsync>d__67>(ref <InternalReadAllTextAsync>d__);
			return <InternalReadAllTextAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06006792 RID: 26514 RVA: 0x00160D7F File Offset: 0x0015EF7F
		public static Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default(CancellationToken))
		{
			return File.WriteAllTextAsync(path, contents, File.UTF8NoBOM, cancellationToken);
		}

		// Token: 0x06006793 RID: 26515 RVA: 0x00160D90 File Offset: 0x0015EF90
		public static Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			if (string.IsNullOrEmpty(contents))
			{
				new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read).Dispose();
				return Task.CompletedTask;
			}
			return File.InternalWriteAllTextAsync(File.AsyncStreamWriter(path, encoding, false), contents, cancellationToken);
		}

		// Token: 0x06006794 RID: 26516 RVA: 0x00160E0C File Offset: 0x0015F00C
		public static Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<byte[]>(cancellationToken);
			}
			FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1, FileOptions.Asynchronous | FileOptions.SequentialScan);
			bool flag = false;
			Task<byte[]> result;
			try
			{
				long length = fileStream.Length;
				if (length > 2147483647L)
				{
					result = Task.FromException<byte[]>(new IOException("The file is too long. This operation is currently limited to supporting files less than 2 gigabytes in size."));
				}
				else
				{
					flag = true;
					result = ((length > 0L) ? File.InternalReadAllBytesAsync(fileStream, (int)length, cancellationToken) : File.InternalReadAllBytesUnknownLengthAsync(fileStream, cancellationToken));
				}
			}
			finally
			{
				if (!flag)
				{
					fileStream.Dispose();
				}
			}
			return result;
		}

		// Token: 0x06006795 RID: 26517 RVA: 0x00160E94 File Offset: 0x0015F094
		private static Task<byte[]> InternalReadAllBytesAsync(FileStream fs, int count, CancellationToken cancellationToken)
		{
			File.<InternalReadAllBytesAsync>d__71 <InternalReadAllBytesAsync>d__;
			<InternalReadAllBytesAsync>d__.fs = fs;
			<InternalReadAllBytesAsync>d__.count = count;
			<InternalReadAllBytesAsync>d__.cancellationToken = cancellationToken;
			<InternalReadAllBytesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<byte[]>.Create();
			<InternalReadAllBytesAsync>d__.<>1__state = -1;
			<InternalReadAllBytesAsync>d__.<>t__builder.Start<File.<InternalReadAllBytesAsync>d__71>(ref <InternalReadAllBytesAsync>d__);
			return <InternalReadAllBytesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06006796 RID: 26518 RVA: 0x00160EE8 File Offset: 0x0015F0E8
		private static Task<byte[]> InternalReadAllBytesUnknownLengthAsync(FileStream fs, CancellationToken cancellationToken)
		{
			File.<InternalReadAllBytesUnknownLengthAsync>d__72 <InternalReadAllBytesUnknownLengthAsync>d__;
			<InternalReadAllBytesUnknownLengthAsync>d__.fs = fs;
			<InternalReadAllBytesUnknownLengthAsync>d__.cancellationToken = cancellationToken;
			<InternalReadAllBytesUnknownLengthAsync>d__.<>t__builder = AsyncTaskMethodBuilder<byte[]>.Create();
			<InternalReadAllBytesUnknownLengthAsync>d__.<>1__state = -1;
			<InternalReadAllBytesUnknownLengthAsync>d__.<>t__builder.Start<File.<InternalReadAllBytesUnknownLengthAsync>d__72>(ref <InternalReadAllBytesUnknownLengthAsync>d__);
			return <InternalReadAllBytesUnknownLengthAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06006797 RID: 26519 RVA: 0x00160F34 File Offset: 0x0015F134
		public static Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path", "Path cannot be null.");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				return File.InternalWriteAllBytesAsync(path, bytes, cancellationToken);
			}
			return Task.FromCanceled(cancellationToken);
		}

		// Token: 0x06006798 RID: 26520 RVA: 0x00160F94 File Offset: 0x0015F194
		private static Task InternalWriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
		{
			File.<InternalWriteAllBytesAsync>d__74 <InternalWriteAllBytesAsync>d__;
			<InternalWriteAllBytesAsync>d__.path = path;
			<InternalWriteAllBytesAsync>d__.bytes = bytes;
			<InternalWriteAllBytesAsync>d__.cancellationToken = cancellationToken;
			<InternalWriteAllBytesAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InternalWriteAllBytesAsync>d__.<>1__state = -1;
			<InternalWriteAllBytesAsync>d__.<>t__builder.Start<File.<InternalWriteAllBytesAsync>d__74>(ref <InternalWriteAllBytesAsync>d__);
			return <InternalWriteAllBytesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06006799 RID: 26521 RVA: 0x00160FE7 File Offset: 0x0015F1E7
		public static Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
		{
			return File.ReadAllLinesAsync(path, Encoding.UTF8, cancellationToken);
		}

		// Token: 0x0600679A RID: 26522 RVA: 0x00160FF8 File Offset: 0x0015F1F8
		public static Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				return File.InternalReadAllLinesAsync(path, encoding, cancellationToken);
			}
			return Task.FromCanceled<string[]>(cancellationToken);
		}

		// Token: 0x0600679B RID: 26523 RVA: 0x00161054 File Offset: 0x0015F254
		private static Task<string[]> InternalReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken)
		{
			File.<InternalReadAllLinesAsync>d__77 <InternalReadAllLinesAsync>d__;
			<InternalReadAllLinesAsync>d__.path = path;
			<InternalReadAllLinesAsync>d__.encoding = encoding;
			<InternalReadAllLinesAsync>d__.cancellationToken = cancellationToken;
			<InternalReadAllLinesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string[]>.Create();
			<InternalReadAllLinesAsync>d__.<>1__state = -1;
			<InternalReadAllLinesAsync>d__.<>t__builder.Start<File.<InternalReadAllLinesAsync>d__77>(ref <InternalReadAllLinesAsync>d__);
			return <InternalReadAllLinesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600679C RID: 26524 RVA: 0x001610A7 File Offset: 0x0015F2A7
		public static Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default(CancellationToken))
		{
			return File.WriteAllLinesAsync(path, contents, File.UTF8NoBOM, cancellationToken);
		}

		// Token: 0x0600679D RID: 26525 RVA: 0x001610B8 File Offset: 0x0015F2B8
		public static Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				return File.InternalWriteAllLinesAsync(File.AsyncStreamWriter(path, encoding, false), contents, cancellationToken);
			}
			return Task.FromCanceled(cancellationToken);
		}

		// Token: 0x0600679E RID: 26526 RVA: 0x00161128 File Offset: 0x0015F328
		private static Task InternalWriteAllLinesAsync(TextWriter writer, IEnumerable<string> contents, CancellationToken cancellationToken)
		{
			File.<InternalWriteAllLinesAsync>d__80 <InternalWriteAllLinesAsync>d__;
			<InternalWriteAllLinesAsync>d__.writer = writer;
			<InternalWriteAllLinesAsync>d__.contents = contents;
			<InternalWriteAllLinesAsync>d__.cancellationToken = cancellationToken;
			<InternalWriteAllLinesAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InternalWriteAllLinesAsync>d__.<>1__state = -1;
			<InternalWriteAllLinesAsync>d__.<>t__builder.Start<File.<InternalWriteAllLinesAsync>d__80>(ref <InternalWriteAllLinesAsync>d__);
			return <InternalWriteAllLinesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600679F RID: 26527 RVA: 0x0016117C File Offset: 0x0015F37C
		private static Task InternalWriteAllTextAsync(StreamWriter sw, string contents, CancellationToken cancellationToken)
		{
			File.<InternalWriteAllTextAsync>d__81 <InternalWriteAllTextAsync>d__;
			<InternalWriteAllTextAsync>d__.sw = sw;
			<InternalWriteAllTextAsync>d__.contents = contents;
			<InternalWriteAllTextAsync>d__.cancellationToken = cancellationToken;
			<InternalWriteAllTextAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InternalWriteAllTextAsync>d__.<>1__state = -1;
			<InternalWriteAllTextAsync>d__.<>t__builder.Start<File.<InternalWriteAllTextAsync>d__81>(ref <InternalWriteAllTextAsync>d__);
			return <InternalWriteAllTextAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060067A0 RID: 26528 RVA: 0x001611CF File Offset: 0x0015F3CF
		public static Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default(CancellationToken))
		{
			return File.AppendAllTextAsync(path, contents, File.UTF8NoBOM, cancellationToken);
		}

		// Token: 0x060067A1 RID: 26529 RVA: 0x001611E0 File Offset: 0x0015F3E0
		public static Task AppendAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			if (string.IsNullOrEmpty(contents))
			{
				new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read).Dispose();
				return Task.CompletedTask;
			}
			return File.InternalWriteAllTextAsync(File.AsyncStreamWriter(path, encoding, true), contents, cancellationToken);
		}

		// Token: 0x060067A2 RID: 26530 RVA: 0x0016125C File Offset: 0x0015F45C
		public static Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default(CancellationToken))
		{
			return File.AppendAllLinesAsync(path, contents, File.UTF8NoBOM, cancellationToken);
		}

		// Token: 0x060067A3 RID: 26531 RVA: 0x0016126C File Offset: 0x0015F46C
		public static Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.", "path");
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				return File.InternalWriteAllLinesAsync(File.AsyncStreamWriter(path, encoding, true), contents, cancellationToken);
			}
			return Task.FromCanceled(cancellationToken);
		}

		// Token: 0x060067A4 RID: 26532 RVA: 0x0016018A File Offset: 0x0015E38A
		public static FileStream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
		{
			return new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, options);
		}

		// Token: 0x060067A5 RID: 26533 RVA: 0x001612DA File Offset: 0x0015F4DA
		public static FileSecurity GetAccessControl(string path)
		{
			return File.GetAccessControl(path, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x060067A6 RID: 26534 RVA: 0x001612E4 File Offset: 0x0015F4E4
		public static FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
		{
			return new FileSecurity(path, includeSections);
		}

		// Token: 0x060067A7 RID: 26535 RVA: 0x001612ED File Offset: 0x0015F4ED
		public static void SetAccessControl(string path, FileSecurity fileSecurity)
		{
			if (fileSecurity == null)
			{
				throw new ArgumentNullException("fileSecurity");
			}
			fileSecurity.PersistModifications(path);
		}

		// Token: 0x04003C30 RID: 15408
		private const int MaxByteArrayLength = 2147483591;

		// Token: 0x04003C31 RID: 15409
		private static Encoding s_UTF8NoBOM;

		// Token: 0x04003C32 RID: 15410
		internal const int DefaultBufferSize = 4096;
	}
}
