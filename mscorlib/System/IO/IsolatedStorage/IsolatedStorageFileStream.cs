using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.IO.IsolatedStorage
{
	// Token: 0x02000B76 RID: 2934
	[ComVisible(true)]
	public class IsolatedStorageFileStream : FileStream
	{
		// Token: 0x06006AD5 RID: 27349 RVA: 0x0016DE68 File Offset: 0x0016C068
		[ReflectionPermission(SecurityAction.Assert, TypeInformation = true)]
		private static string CreateIsolatedPath(IsolatedStorageFile isf, string path, FileMode mode)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (!Enum.IsDefined(typeof(FileMode), mode))
			{
				throw new ArgumentException("mode");
			}
			if (isf == null)
			{
				isf = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, IsolatedStorageFile.GetDomainIdentityFromEvidence(AppDomain.CurrentDomain.Evidence), IsolatedStorageFile.GetAssemblyIdentityFromEvidence(new StackFrame(3).GetMethod().ReflectedType.Assembly.UnprotectedGetEvidence()));
			}
			if (isf.IsDisposed)
			{
				throw new ObjectDisposedException("IsolatedStorageFile");
			}
			if (isf.IsClosed)
			{
				throw new InvalidOperationException("Storage needs to be open for this operation.");
			}
			FileInfo fileInfo = new FileInfo(isf.Root);
			if (!fileInfo.Directory.Exists)
			{
				fileInfo.Directory.Create();
			}
			if (Path.IsPathRooted(path))
			{
				string pathRoot = Path.GetPathRoot(path);
				path = path.Remove(0, pathRoot.Length);
			}
			string text = Path.Combine(isf.Root, path);
			Path.GetFullPath(text);
			if (!Path.GetFullPath(text).StartsWith(isf.Root))
			{
				throw new IsolatedStorageException();
			}
			fileInfo = new FileInfo(text);
			if (!fileInfo.Directory.Exists)
			{
				throw new DirectoryNotFoundException(string.Format(Locale.GetText("Could not find a part of the path \"{0}\"."), path));
			}
			return text;
		}

		// Token: 0x06006AD6 RID: 27350 RVA: 0x0016DF9C File Offset: 0x0016C19C
		public IsolatedStorageFileStream(string path, FileMode mode) : this(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.Read, 4096, null)
		{
		}

		// Token: 0x06006AD7 RID: 27351 RVA: 0x0016DFB5 File Offset: 0x0016C1B5
		public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access) : this(path, mode, access, (access == FileAccess.Write) ? FileShare.None : FileShare.Read, 4096, null)
		{
		}

		// Token: 0x06006AD8 RID: 27352 RVA: 0x0016DFCE File Offset: 0x0016C1CE
		public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share) : this(path, mode, access, share, 4096, null)
		{
		}

		// Token: 0x06006AD9 RID: 27353 RVA: 0x0016DFE1 File Offset: 0x0016C1E1
		public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize) : this(path, mode, access, share, bufferSize, null)
		{
		}

		// Token: 0x06006ADA RID: 27354 RVA: 0x0016DFF1 File Offset: 0x0016C1F1
		[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
		public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, IsolatedStorageFile isf) : base(IsolatedStorageFileStream.CreateIsolatedPath(isf, path, mode), mode, access, share, bufferSize, false, true)
		{
		}

		// Token: 0x06006ADB RID: 27355 RVA: 0x0016E00A File Offset: 0x0016C20A
		public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share, IsolatedStorageFile isf) : this(path, mode, access, share, 4096, isf)
		{
		}

		// Token: 0x06006ADC RID: 27356 RVA: 0x0016E01E File Offset: 0x0016C21E
		public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, IsolatedStorageFile isf) : this(path, mode, access, (access == FileAccess.Write) ? FileShare.None : FileShare.Read, 4096, isf)
		{
		}

		// Token: 0x06006ADD RID: 27357 RVA: 0x0016E038 File Offset: 0x0016C238
		public IsolatedStorageFileStream(string path, FileMode mode, IsolatedStorageFile isf) : this(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.Read, 4096, isf)
		{
		}

		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x06006ADE RID: 27358 RVA: 0x0016E051 File Offset: 0x0016C251
		public override bool CanRead
		{
			get
			{
				return base.CanRead;
			}
		}

		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x06006ADF RID: 27359 RVA: 0x0016E059 File Offset: 0x0016C259
		public override bool CanSeek
		{
			get
			{
				return base.CanSeek;
			}
		}

		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x06006AE0 RID: 27360 RVA: 0x0016E061 File Offset: 0x0016C261
		public override bool CanWrite
		{
			get
			{
				return base.CanWrite;
			}
		}

		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x06006AE1 RID: 27361 RVA: 0x0016E069 File Offset: 0x0016C269
		public override SafeFileHandle SafeFileHandle
		{
			[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
			get
			{
				throw new IsolatedStorageException(Locale.GetText("Information is restricted"));
			}
		}

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x06006AE2 RID: 27362 RVA: 0x0016E069 File Offset: 0x0016C269
		[Obsolete("Use SafeFileHandle - once available")]
		public override IntPtr Handle
		{
			[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
			get
			{
				throw new IsolatedStorageException(Locale.GetText("Information is restricted"));
			}
		}

		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x06006AE3 RID: 27363 RVA: 0x0016E07A File Offset: 0x0016C27A
		public override bool IsAsync
		{
			get
			{
				return base.IsAsync;
			}
		}

		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x06006AE4 RID: 27364 RVA: 0x0016E082 File Offset: 0x0016C282
		public override long Length
		{
			get
			{
				return base.Length;
			}
		}

		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x06006AE5 RID: 27365 RVA: 0x0016E08A File Offset: 0x0016C28A
		// (set) Token: 0x06006AE6 RID: 27366 RVA: 0x0016E092 File Offset: 0x0016C292
		public override long Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;
			}
		}

		// Token: 0x06006AE7 RID: 27367 RVA: 0x0016E09B File Offset: 0x0016C29B
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			return base.BeginRead(buffer, offset, numBytes, userCallback, stateObject);
		}

		// Token: 0x06006AE8 RID: 27368 RVA: 0x0016E0AA File Offset: 0x0016C2AA
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			return base.BeginWrite(buffer, offset, numBytes, userCallback, stateObject);
		}

		// Token: 0x06006AE9 RID: 27369 RVA: 0x0016E0B9 File Offset: 0x0016C2B9
		public override int EndRead(IAsyncResult asyncResult)
		{
			return base.EndRead(asyncResult);
		}

		// Token: 0x06006AEA RID: 27370 RVA: 0x0016E0C2 File Offset: 0x0016C2C2
		public override void EndWrite(IAsyncResult asyncResult)
		{
			base.EndWrite(asyncResult);
		}

		// Token: 0x06006AEB RID: 27371 RVA: 0x0016E0CB File Offset: 0x0016C2CB
		public override void Flush()
		{
			base.Flush();
		}

		// Token: 0x06006AEC RID: 27372 RVA: 0x0016E0D3 File Offset: 0x0016C2D3
		public override void Flush(bool flushToDisk)
		{
			base.Flush(flushToDisk);
		}

		// Token: 0x06006AED RID: 27373 RVA: 0x0016E0DC File Offset: 0x0016C2DC
		public override int Read(byte[] buffer, int offset, int count)
		{
			return base.Read(buffer, offset, count);
		}

		// Token: 0x06006AEE RID: 27374 RVA: 0x0016E0E7 File Offset: 0x0016C2E7
		public override int ReadByte()
		{
			return base.ReadByte();
		}

		// Token: 0x06006AEF RID: 27375 RVA: 0x0016E0EF File Offset: 0x0016C2EF
		public override long Seek(long offset, SeekOrigin origin)
		{
			return base.Seek(offset, origin);
		}

		// Token: 0x06006AF0 RID: 27376 RVA: 0x0016E0F9 File Offset: 0x0016C2F9
		public override void SetLength(long value)
		{
			base.SetLength(value);
		}

		// Token: 0x06006AF1 RID: 27377 RVA: 0x0016E102 File Offset: 0x0016C302
		public override void Write(byte[] buffer, int offset, int count)
		{
			base.Write(buffer, offset, count);
		}

		// Token: 0x06006AF2 RID: 27378 RVA: 0x0016E10D File Offset: 0x0016C30D
		public override void WriteByte(byte value)
		{
			base.WriteByte(value);
		}

		// Token: 0x06006AF3 RID: 27379 RVA: 0x0016E116 File Offset: 0x0016C316
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}
