using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
	// Token: 0x02000B5E RID: 2910
	[ComVisible(true)]
	public class FileStream : Stream
	{
		// Token: 0x06006975 RID: 26997 RVA: 0x001682BB File Offset: 0x001664BB
		[Obsolete("Use FileStream(SafeFileHandle handle, FileAccess access) instead")]
		public FileStream(IntPtr handle, FileAccess access) : this(handle, access, true, 4096, false, false)
		{
		}

		// Token: 0x06006976 RID: 26998 RVA: 0x001682CD File Offset: 0x001664CD
		[Obsolete("Use FileStream(SafeFileHandle handle, FileAccess access) instead")]
		public FileStream(IntPtr handle, FileAccess access, bool ownsHandle) : this(handle, access, ownsHandle, 4096, false, false)
		{
		}

		// Token: 0x06006977 RID: 26999 RVA: 0x001682DF File Offset: 0x001664DF
		[Obsolete("Use FileStream(SafeFileHandle handle, FileAccess access, int bufferSize) instead")]
		public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize) : this(handle, access, ownsHandle, bufferSize, false, false)
		{
		}

		// Token: 0x06006978 RID: 27000 RVA: 0x001682EE File Offset: 0x001664EE
		[Obsolete("Use FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead")]
		public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync) : this(handle, access, ownsHandle, bufferSize, isAsync, false)
		{
		}

		// Token: 0x06006979 RID: 27001 RVA: 0x00168300 File Offset: 0x00166500
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		internal FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync, bool isConsoleWrapper)
		{
			this.name = "[Unknown]";
			base..ctor();
			if (handle == MonoIO.InvalidHandle)
			{
				throw new ArgumentException("handle", Locale.GetText("Invalid."));
			}
			this.Init(new SafeFileHandle(handle, false), access, ownsHandle, bufferSize, isAsync, isConsoleWrapper);
		}

		// Token: 0x0600697A RID: 27002 RVA: 0x00168355 File Offset: 0x00166555
		public FileStream(string path, FileMode mode) : this(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.Read, 4096, false, FileOptions.None)
		{
		}

		// Token: 0x0600697B RID: 27003 RVA: 0x0016836F File Offset: 0x0016656F
		public FileStream(string path, FileMode mode, FileAccess access) : this(path, mode, access, (access == FileAccess.Write) ? FileShare.None : FileShare.Read, 4096, false, false)
		{
		}

		// Token: 0x0600697C RID: 27004 RVA: 0x00168389 File Offset: 0x00166589
		public FileStream(string path, FileMode mode, FileAccess access, FileShare share) : this(path, mode, access, share, 4096, false, FileOptions.None)
		{
		}

		// Token: 0x0600697D RID: 27005 RVA: 0x0016839D File Offset: 0x0016659D
		public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize) : this(path, mode, access, share, bufferSize, false, FileOptions.None)
		{
		}

		// Token: 0x0600697E RID: 27006 RVA: 0x001683AE File Offset: 0x001665AE
		public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync) : this(path, mode, access, share, bufferSize, useAsync ? FileOptions.Asynchronous : FileOptions.None)
		{
		}

		// Token: 0x0600697F RID: 27007 RVA: 0x001683C9 File Offset: 0x001665C9
		public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options) : this(path, mode, access, share, bufferSize, false, options)
		{
		}

		// Token: 0x06006980 RID: 27008 RVA: 0x001683DB File Offset: 0x001665DB
		public FileStream(SafeFileHandle handle, FileAccess access) : this(handle, access, 4096, false)
		{
		}

		// Token: 0x06006981 RID: 27009 RVA: 0x001683EB File Offset: 0x001665EB
		public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize) : this(handle, access, bufferSize, false)
		{
		}

		// Token: 0x06006982 RID: 27010 RVA: 0x001683F7 File Offset: 0x001665F7
		public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
		{
			this.name = "[Unknown]";
			base..ctor();
			this.Init(handle, access, false, bufferSize, isAsync, false);
		}

		// Token: 0x06006983 RID: 27011 RVA: 0x00168417 File Offset: 0x00166617
		[MonoLimitation("This ignores the rights parameter")]
		public FileStream(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options) : this(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, share, bufferSize, false, options)
		{
		}

		// Token: 0x06006984 RID: 27012 RVA: 0x00168417 File Offset: 0x00166617
		[MonoLimitation("This ignores the rights and fileSecurity parameters")]
		public FileStream(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity fileSecurity) : this(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, share, bufferSize, false, options)
		{
		}

		// Token: 0x06006985 RID: 27013 RVA: 0x001683C9 File Offset: 0x001665C9
		internal FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options, string msgPath, bool bFromProxy, bool useLongPath = false, bool checkHost = false) : this(path, mode, access, share, bufferSize, false, options)
		{
		}

		// Token: 0x06006986 RID: 27014 RVA: 0x00168430 File Offset: 0x00166630
		internal FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool isAsync, bool anonymous) : this(path, mode, access, share, bufferSize, anonymous, isAsync ? FileOptions.Asynchronous : FileOptions.None)
		{
		}

		// Token: 0x06006987 RID: 27015 RVA: 0x00168450 File Offset: 0x00166650
		internal FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool anonymous, FileOptions options)
		{
			this.name = "[Unknown]";
			base..ctor();
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Path is empty");
			}
			this.anonymous = anonymous;
			share &= ~FileShare.Inheritable;
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "Positive number required.");
			}
			if (mode < FileMode.CreateNew || mode > FileMode.Append)
			{
				throw new ArgumentOutOfRangeException("mode", "Enum value was out of legal range.");
			}
			if (access < FileAccess.Read || access > FileAccess.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("access", "Enum value was out of legal range.");
			}
			if (share < FileShare.None || share > (FileShare.Read | FileShare.Write | FileShare.Delete))
			{
				throw new ArgumentOutOfRangeException("share", "Enum value was out of legal range.");
			}
			if (path.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				throw new ArgumentException("Name has invalid chars");
			}
			path = Path.InsecureGetFullPath(path);
			if (Directory.Exists(path))
			{
				throw new UnauthorizedAccessException(string.Format(Locale.GetText("Access to the path '{0}' is denied."), this.GetSecureFileName(path, false)));
			}
			if (mode == FileMode.Append && (access & FileAccess.Read) == FileAccess.Read)
			{
				throw new ArgumentException("Append access can be requested only in write-only mode.");
			}
			if ((access & FileAccess.Write) == (FileAccess)0 && mode != FileMode.Open && mode != FileMode.OpenOrCreate)
			{
				throw new ArgumentException(string.Format(Locale.GetText("Combining FileMode: {0} with FileAccess: {1} is invalid."), access, mode));
			}
			string directoryName = Path.GetDirectoryName(path);
			if (directoryName.Length > 0 && !Directory.Exists(Path.GetFullPath(directoryName)))
			{
				string text = Locale.GetText("Could not find a part of the path \"{0}\".");
				string arg = anonymous ? directoryName : Path.GetFullPath(path);
				throw new DirectoryNotFoundException(string.Format(text, arg));
			}
			if (!anonymous)
			{
				this.name = path;
			}
			MonoIOError error;
			IntPtr intPtr = MonoIO.Open(path, mode, access, share, options, out error);
			if (intPtr == MonoIO.InvalidHandle)
			{
				throw MonoIO.GetException(this.GetSecureFileName(path), error);
			}
			this.safeHandle = new SafeFileHandle(intPtr, false);
			this.access = access;
			this.owner = true;
			if (MonoIO.GetFileType(this.safeHandle, out error) == MonoFileType.Disk)
			{
				this.canseek = true;
				this.async = ((options & FileOptions.Asynchronous) > FileOptions.None);
			}
			else
			{
				this.canseek = false;
				this.async = false;
			}
			if (access == FileAccess.Read && this.canseek && bufferSize == 4096)
			{
				long length = this.Length;
				if ((long)bufferSize > length)
				{
					bufferSize = (int)((length < 1000L) ? 1000L : length);
				}
			}
			this.InitBuffer(bufferSize, false);
			if (mode == FileMode.Append)
			{
				this.Seek(0L, SeekOrigin.End);
				this.append_startpos = this.Position;
				return;
			}
			this.append_startpos = 0L;
		}

		// Token: 0x06006988 RID: 27016 RVA: 0x001686B8 File Offset: 0x001668B8
		private void Init(SafeFileHandle safeHandle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync, bool isConsoleWrapper)
		{
			if (!isConsoleWrapper && safeHandle.IsInvalid)
			{
				throw new ArgumentException(Environment.GetResourceString("Invalid handle."), "handle");
			}
			if (access < FileAccess.Read || access > FileAccess.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("access");
			}
			if (!isConsoleWrapper && bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("Positive number required."));
			}
			MonoIOError monoIOError;
			MonoFileType fileType = MonoIO.GetFileType(safeHandle, out monoIOError);
			if (monoIOError != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(this.name, monoIOError);
			}
			if (fileType == MonoFileType.Unknown)
			{
				throw new IOException("Invalid handle.");
			}
			if (fileType == MonoFileType.Disk)
			{
				this.canseek = true;
			}
			else
			{
				this.canseek = false;
			}
			this.safeHandle = safeHandle;
			this.ExposeHandle();
			this.access = access;
			this.owner = ownsHandle;
			this.async = isAsync;
			this.anonymous = false;
			if (this.canseek)
			{
				this.buf_start = MonoIO.Seek(safeHandle, 0L, SeekOrigin.Current, out monoIOError);
				if (monoIOError != MonoIOError.ERROR_SUCCESS)
				{
					throw MonoIO.GetException(this.name, monoIOError);
				}
			}
			this.append_startpos = 0L;
		}

		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06006989 RID: 27017 RVA: 0x001687AD File Offset: 0x001669AD
		public override bool CanRead
		{
			get
			{
				return this.access == FileAccess.Read || this.access == FileAccess.ReadWrite;
			}
		}

		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x0600698A RID: 27018 RVA: 0x001687C3 File Offset: 0x001669C3
		public override bool CanWrite
		{
			get
			{
				return this.access == FileAccess.Write || this.access == FileAccess.ReadWrite;
			}
		}

		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x0600698B RID: 27019 RVA: 0x001687D9 File Offset: 0x001669D9
		public override bool CanSeek
		{
			get
			{
				return this.canseek;
			}
		}

		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x0600698C RID: 27020 RVA: 0x001687E1 File Offset: 0x001669E1
		public virtual bool IsAsync
		{
			get
			{
				return this.async;
			}
		}

		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x0600698D RID: 27021 RVA: 0x001687E9 File Offset: 0x001669E9
		public virtual string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x0600698E RID: 27022 RVA: 0x001687F4 File Offset: 0x001669F4
		public override long Length
		{
			get
			{
				if (this.safeHandle.IsClosed)
				{
					throw new ObjectDisposedException("Stream has been closed");
				}
				if (!this.CanSeek)
				{
					throw new NotSupportedException("The stream does not support seeking");
				}
				this.FlushBufferIfDirty();
				MonoIOError monoIOError;
				long length = MonoIO.GetLength(this.safeHandle, out monoIOError);
				if (monoIOError != MonoIOError.ERROR_SUCCESS)
				{
					throw MonoIO.GetException(this.GetSecureFileName(this.name), monoIOError);
				}
				return length;
			}
		}

		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x0600698F RID: 27023 RVA: 0x00168858 File Offset: 0x00166A58
		// (set) Token: 0x06006990 RID: 27024 RVA: 0x001688CD File Offset: 0x00166ACD
		public override long Position
		{
			get
			{
				if (this.safeHandle.IsClosed)
				{
					throw new ObjectDisposedException("Stream has been closed");
				}
				if (!this.CanSeek)
				{
					throw new NotSupportedException("The stream does not support seeking");
				}
				if (!this.isExposed)
				{
					return this.buf_start + (long)this.buf_offset;
				}
				MonoIOError monoIOError;
				long result = MonoIO.Seek(this.safeHandle, 0L, SeekOrigin.Current, out monoIOError);
				if (monoIOError != MonoIOError.ERROR_SUCCESS)
				{
					throw MonoIO.GetException(this.GetSecureFileName(this.name), monoIOError);
				}
				return result;
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("Non-negative number required."));
				}
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x06006991 RID: 27025 RVA: 0x001688F2 File Offset: 0x00166AF2
		[Obsolete("Use SafeFileHandle instead")]
		public virtual IntPtr Handle
		{
			[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
			[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
			get
			{
				IntPtr result = this.safeHandle.DangerousGetHandle();
				if (!this.isExposed)
				{
					this.ExposeHandle();
				}
				return result;
			}
		}

		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x06006992 RID: 27026 RVA: 0x0016890D File Offset: 0x00166B0D
		public virtual SafeFileHandle SafeFileHandle
		{
			[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
			[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
			get
			{
				if (!this.isExposed)
				{
					this.ExposeHandle();
				}
				return this.safeHandle;
			}
		}

		// Token: 0x06006993 RID: 27027 RVA: 0x00168923 File Offset: 0x00166B23
		private void ExposeHandle()
		{
			this.isExposed = true;
			this.FlushBuffer();
			this.InitBuffer(0, true);
		}

		// Token: 0x06006994 RID: 27028 RVA: 0x0016893C File Offset: 0x00166B3C
		public override int ReadByte()
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			if (!this.CanRead)
			{
				throw new NotSupportedException("Stream does not support reading");
			}
			if (this.buf_size != 0)
			{
				if (this.buf_offset >= this.buf_length)
				{
					this.RefillBuffer();
					if (this.buf_length == 0)
					{
						return -1;
					}
				}
				byte[] array = this.buf;
				int num = this.buf_offset;
				this.buf_offset = num + 1;
				return array[num];
			}
			if (this.ReadData(this.safeHandle, this.buf, 0, 1) == 0)
			{
				return -1;
			}
			return (int)this.buf[0];
		}

		// Token: 0x06006995 RID: 27029 RVA: 0x001689D4 File Offset: 0x00166BD4
		public override void WriteByte(byte value)
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			if (!this.CanWrite)
			{
				throw new NotSupportedException("Stream does not support writing");
			}
			if (this.buf_offset == this.buf_size)
			{
				this.FlushBuffer();
			}
			if (this.buf_size == 0)
			{
				this.buf[0] = value;
				this.buf_dirty = true;
				this.buf_length = 1;
				this.FlushBuffer();
				return;
			}
			byte[] array = this.buf;
			int num = this.buf_offset;
			this.buf_offset = num + 1;
			array[num] = value;
			if (this.buf_offset > this.buf_length)
			{
				this.buf_length = this.buf_offset;
			}
			this.buf_dirty = true;
		}

		// Token: 0x06006996 RID: 27030 RVA: 0x00168A80 File Offset: 0x00166C80
		public override int Read([In] [Out] byte[] array, int offset, int count)
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (!this.CanRead)
			{
				throw new NotSupportedException("Stream does not support reading");
			}
			int num = array.Length;
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			if (offset > num)
			{
				throw new ArgumentException("destination offset is beyond array size");
			}
			if (offset > num - count)
			{
				throw new ArgumentException("Reading would overrun buffer");
			}
			if (this.async)
			{
				IAsyncResult asyncResult = this.BeginRead(array, offset, count, null, null);
				return this.EndRead(asyncResult);
			}
			return this.ReadInternal(array, offset, count);
		}

		// Token: 0x06006997 RID: 27031 RVA: 0x00168B38 File Offset: 0x00166D38
		private int ReadInternal(byte[] dest, int offset, int count)
		{
			int num = this.ReadSegment(dest, offset, count);
			if (num == count)
			{
				return count;
			}
			int num2 = num;
			count -= num;
			if (count > this.buf_size)
			{
				this.FlushBuffer();
				num = this.ReadData(this.safeHandle, dest, offset + num, count);
				this.buf_start += (long)num;
			}
			else
			{
				this.RefillBuffer();
				num = this.ReadSegment(dest, offset + num2, count);
			}
			return num2 + num;
		}

		// Token: 0x06006998 RID: 27032 RVA: 0x00168BA4 File Offset: 0x00166DA4
		public override IAsyncResult BeginRead(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			if (!this.CanRead)
			{
				throw new NotSupportedException("This stream does not support reading");
			}
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (numBytes < 0)
			{
				throw new ArgumentOutOfRangeException("numBytes", "Must be >= 0");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Must be >= 0");
			}
			if (numBytes > array.Length - offset)
			{
				throw new ArgumentException("Buffer too small. numBytes/offset wrong.");
			}
			if (!this.async)
			{
				return base.BeginRead(array, offset, numBytes, userCallback, stateObject);
			}
			return new FileStream.ReadDelegate(this.ReadInternal).BeginInvoke(array, offset, numBytes, userCallback, stateObject);
		}

		// Token: 0x06006999 RID: 27033 RVA: 0x00168C54 File Offset: 0x00166E54
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!this.async)
			{
				return base.EndRead(asyncResult);
			}
			AsyncResult asyncResult2 = asyncResult as AsyncResult;
			if (asyncResult2 == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			FileStream.ReadDelegate readDelegate = asyncResult2.AsyncDelegate as FileStream.ReadDelegate;
			if (readDelegate == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			return readDelegate.EndInvoke(asyncResult);
		}

		// Token: 0x0600699A RID: 27034 RVA: 0x00168CBC File Offset: 0x00166EBC
		public override void Write(byte[] array, int offset, int count)
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			if (offset > array.Length - count)
			{
				throw new ArgumentException("Reading would overrun buffer");
			}
			if (!this.CanWrite)
			{
				throw new NotSupportedException("Stream does not support writing");
			}
			if (this.async)
			{
				IAsyncResult asyncResult = this.BeginWrite(array, offset, count, null, null);
				this.EndWrite(asyncResult);
				return;
			}
			this.WriteInternal(array, offset, count);
		}

		// Token: 0x0600699B RID: 27035 RVA: 0x00168D64 File Offset: 0x00166F64
		private void WriteInternal(byte[] src, int offset, int count)
		{
			if (count > this.buf_size)
			{
				this.FlushBuffer();
				if (this.CanSeek && !this.isExposed)
				{
					MonoIOError monoIOError;
					MonoIO.Seek(this.safeHandle, this.buf_start, SeekOrigin.Begin, out monoIOError);
					if (monoIOError != MonoIOError.ERROR_SUCCESS)
					{
						throw MonoIO.GetException(this.GetSecureFileName(this.name), monoIOError);
					}
				}
				int i = count;
				while (i > 0)
				{
					MonoIOError monoIOError;
					int num = MonoIO.Write(this.safeHandle, src, offset, i, out monoIOError);
					if (monoIOError != MonoIOError.ERROR_SUCCESS)
					{
						throw MonoIO.GetException(this.GetSecureFileName(this.name), monoIOError);
					}
					i -= num;
					offset += num;
				}
				this.buf_start += (long)count;
				return;
			}
			int num2 = 0;
			while (count > 0)
			{
				int num3 = this.WriteSegment(src, offset + num2, count);
				num2 += num3;
				count -= num3;
				if (count == 0)
				{
					break;
				}
				this.FlushBuffer();
			}
		}

		// Token: 0x0600699C RID: 27036 RVA: 0x00168E30 File Offset: 0x00167030
		public override IAsyncResult BeginWrite(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			if (!this.CanWrite)
			{
				throw new NotSupportedException("This stream does not support writing");
			}
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (numBytes < 0)
			{
				throw new ArgumentOutOfRangeException("numBytes", "Must be >= 0");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Must be >= 0");
			}
			if (numBytes > array.Length - offset)
			{
				throw new ArgumentException("array too small. numBytes/offset wrong.");
			}
			if (!this.async)
			{
				return base.BeginWrite(array, offset, numBytes, userCallback, stateObject);
			}
			FileStreamAsyncResult fileStreamAsyncResult = new FileStreamAsyncResult(userCallback, stateObject);
			fileStreamAsyncResult.BytesRead = -1;
			fileStreamAsyncResult.Count = numBytes;
			fileStreamAsyncResult.OriginalCount = numBytes;
			return new FileStream.WriteDelegate(this.WriteInternal).BeginInvoke(array, offset, numBytes, userCallback, stateObject);
		}

		// Token: 0x0600699D RID: 27037 RVA: 0x00168EFC File Offset: 0x001670FC
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!this.async)
			{
				base.EndWrite(asyncResult);
				return;
			}
			AsyncResult asyncResult2 = asyncResult as AsyncResult;
			if (asyncResult2 == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			FileStream.WriteDelegate writeDelegate = asyncResult2.AsyncDelegate as FileStream.WriteDelegate;
			if (writeDelegate == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			writeDelegate.EndInvoke(asyncResult);
		}

		// Token: 0x0600699E RID: 27038 RVA: 0x00168F64 File Offset: 0x00167164
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			if (!this.CanSeek)
			{
				throw new NotSupportedException("The stream does not support seeking");
			}
			long num;
			switch (origin)
			{
			case SeekOrigin.Begin:
				num = offset;
				break;
			case SeekOrigin.Current:
				num = this.Position + offset;
				break;
			case SeekOrigin.End:
				num = this.Length + offset;
				break;
			default:
				throw new ArgumentException("origin", "Invalid SeekOrigin");
			}
			if (num < 0L)
			{
				throw new IOException("Attempted to Seek before the beginning of the stream");
			}
			if (num < this.append_startpos)
			{
				throw new IOException("Can't seek back over pre-existing data in append mode");
			}
			this.FlushBuffer();
			MonoIOError monoIOError;
			this.buf_start = MonoIO.Seek(this.safeHandle, num, SeekOrigin.Begin, out monoIOError);
			if (monoIOError != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(this.GetSecureFileName(this.name), monoIOError);
			}
			return this.buf_start;
		}

		// Token: 0x0600699F RID: 27039 RVA: 0x00169038 File Offset: 0x00167238
		public override void SetLength(long value)
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			if (!this.CanSeek)
			{
				throw new NotSupportedException("The stream does not support seeking");
			}
			if (!this.CanWrite)
			{
				throw new NotSupportedException("The stream does not support writing");
			}
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("value is less than 0");
			}
			this.FlushBuffer();
			MonoIOError monoIOError;
			MonoIO.SetLength(this.safeHandle, value, out monoIOError);
			if (monoIOError != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(this.GetSecureFileName(this.name), monoIOError);
			}
			if (this.Position > value)
			{
				this.Position = value;
			}
		}

		// Token: 0x060069A0 RID: 27040 RVA: 0x001690CE File Offset: 0x001672CE
		public override void Flush()
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			this.FlushBuffer();
		}

		// Token: 0x060069A1 RID: 27041 RVA: 0x001690F0 File Offset: 0x001672F0
		public virtual void Flush(bool flushToDisk)
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			this.FlushBuffer();
			if (flushToDisk)
			{
				MonoIOError monoIOError;
				MonoIO.Flush(this.safeHandle, out monoIOError);
			}
		}

		// Token: 0x060069A2 RID: 27042 RVA: 0x0016912C File Offset: 0x0016732C
		public virtual void Lock(long position, long length)
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			if (position < 0L)
			{
				throw new ArgumentOutOfRangeException("position must not be negative");
			}
			if (length < 0L)
			{
				throw new ArgumentOutOfRangeException("length must not be negative");
			}
			MonoIOError monoIOError;
			MonoIO.Lock(this.safeHandle, position, length, out monoIOError);
			if (monoIOError != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(this.GetSecureFileName(this.name), monoIOError);
			}
		}

		// Token: 0x060069A3 RID: 27043 RVA: 0x00169198 File Offset: 0x00167398
		public virtual void Unlock(long position, long length)
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			if (position < 0L)
			{
				throw new ArgumentOutOfRangeException("position must not be negative");
			}
			if (length < 0L)
			{
				throw new ArgumentOutOfRangeException("length must not be negative");
			}
			MonoIOError monoIOError;
			MonoIO.Unlock(this.safeHandle, position, length, out monoIOError);
			if (monoIOError != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(this.GetSecureFileName(this.name), monoIOError);
			}
		}

		// Token: 0x060069A4 RID: 27044 RVA: 0x00169204 File Offset: 0x00167404
		~FileStream()
		{
			this.Dispose(false);
		}

		// Token: 0x060069A5 RID: 27045 RVA: 0x00169234 File Offset: 0x00167434
		protected override void Dispose(bool disposing)
		{
			Exception ex = null;
			if (this.safeHandle != null && !this.safeHandle.IsClosed)
			{
				try
				{
					this.FlushBuffer();
				}
				catch (Exception ex)
				{
				}
				if (this.owner)
				{
					MonoIOError monoIOError;
					MonoIO.Close(this.safeHandle.DangerousGetHandle(), out monoIOError);
					if (monoIOError != MonoIOError.ERROR_SUCCESS)
					{
						throw MonoIO.GetException(this.GetSecureFileName(this.name), monoIOError);
					}
					this.safeHandle.DangerousRelease();
				}
			}
			this.canseek = false;
			this.access = (FileAccess)0;
			if (disposing && this.buf != null)
			{
				if (this.buf.Length == 4096 && FileStream.buf_recycle == null)
				{
					object obj = FileStream.buf_recycle_lock;
					lock (obj)
					{
						if (FileStream.buf_recycle == null)
						{
							FileStream.buf_recycle = this.buf;
						}
					}
				}
				this.buf = null;
				GC.SuppressFinalize(this);
			}
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x060069A6 RID: 27046 RVA: 0x0016932C File Offset: 0x0016752C
		public FileSecurity GetAccessControl()
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			return new FileSecurity(this.SafeFileHandle, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x060069A7 RID: 27047 RVA: 0x00169353 File Offset: 0x00167553
		public void SetAccessControl(FileSecurity fileSecurity)
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			if (fileSecurity == null)
			{
				throw new ArgumentNullException("fileSecurity");
			}
			fileSecurity.PersistModifications(this.SafeFileHandle);
		}

		// Token: 0x060069A8 RID: 27048 RVA: 0x00169387 File Offset: 0x00167587
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			if (this.safeHandle.IsClosed)
			{
				throw new ObjectDisposedException("Stream has been closed");
			}
			return base.FlushAsync(cancellationToken);
		}

		// Token: 0x060069A9 RID: 27049 RVA: 0x001693A8 File Offset: 0x001675A8
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return base.ReadAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x060069AA RID: 27050 RVA: 0x001693B5 File Offset: 0x001675B5
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return base.WriteAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x060069AB RID: 27051 RVA: 0x001693C2 File Offset: 0x001675C2
		private int ReadSegment(byte[] dest, int dest_offset, int count)
		{
			count = Math.Min(count, this.buf_length - this.buf_offset);
			if (count > 0)
			{
				Buffer.InternalBlockCopy(this.buf, this.buf_offset, dest, dest_offset, count);
				this.buf_offset += count;
			}
			return count;
		}

		// Token: 0x060069AC RID: 27052 RVA: 0x00169404 File Offset: 0x00167604
		private int WriteSegment(byte[] src, int src_offset, int count)
		{
			if (count > this.buf_size - this.buf_offset)
			{
				count = this.buf_size - this.buf_offset;
			}
			if (count > 0)
			{
				Buffer.BlockCopy(src, src_offset, this.buf, this.buf_offset, count);
				this.buf_offset += count;
				if (this.buf_offset > this.buf_length)
				{
					this.buf_length = this.buf_offset;
				}
				this.buf_dirty = true;
			}
			return count;
		}

		// Token: 0x060069AD RID: 27053 RVA: 0x00169478 File Offset: 0x00167678
		private void FlushBuffer()
		{
			if (this.buf_dirty)
			{
				if (this.CanSeek && !this.isExposed)
				{
					MonoIOError monoIOError;
					MonoIO.Seek(this.safeHandle, this.buf_start, SeekOrigin.Begin, out monoIOError);
					if (monoIOError != MonoIOError.ERROR_SUCCESS)
					{
						throw MonoIO.GetException(this.GetSecureFileName(this.name), monoIOError);
					}
				}
				int i = this.buf_length;
				int num = 0;
				while (i > 0)
				{
					MonoIOError monoIOError;
					int num2 = MonoIO.Write(this.safeHandle, this.buf, num, this.buf_length, out monoIOError);
					if (monoIOError != MonoIOError.ERROR_SUCCESS)
					{
						throw MonoIO.GetException(this.GetSecureFileName(this.name), monoIOError);
					}
					i -= num2;
					num += num2;
				}
			}
			this.buf_start += (long)this.buf_offset;
			this.buf_offset = (this.buf_length = 0);
			this.buf_dirty = false;
		}

		// Token: 0x060069AE RID: 27054 RVA: 0x00169540 File Offset: 0x00167740
		private void FlushBufferIfDirty()
		{
			if (this.buf_dirty)
			{
				this.FlushBuffer();
			}
		}

		// Token: 0x060069AF RID: 27055 RVA: 0x00169550 File Offset: 0x00167750
		private void RefillBuffer()
		{
			this.FlushBuffer();
			this.buf_length = this.ReadData(this.safeHandle, this.buf, 0, this.buf_size);
		}

		// Token: 0x060069B0 RID: 27056 RVA: 0x00169578 File Offset: 0x00167778
		private int ReadData(SafeHandle safeHandle, byte[] buf, int offset, int count)
		{
			MonoIOError monoIOError;
			int num = MonoIO.Read(safeHandle, buf, offset, count, out monoIOError);
			if (monoIOError == MonoIOError.ERROR_BROKEN_PIPE)
			{
				num = 0;
			}
			else if (monoIOError != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(this.GetSecureFileName(this.name), monoIOError);
			}
			if (num == -1)
			{
				throw new IOException();
			}
			return num;
		}

		// Token: 0x060069B1 RID: 27057 RVA: 0x001695C0 File Offset: 0x001677C0
		private void InitBuffer(int size, bool isZeroSize)
		{
			if (isZeroSize)
			{
				size = 0;
				this.buf = new byte[1];
			}
			else
			{
				if (size <= 0)
				{
					throw new ArgumentOutOfRangeException("bufferSize", "Positive number required.");
				}
				size = Math.Max(size, 8);
				if (size <= 4096 && FileStream.buf_recycle != null)
				{
					object obj = FileStream.buf_recycle_lock;
					lock (obj)
					{
						if (FileStream.buf_recycle != null)
						{
							this.buf = FileStream.buf_recycle;
							FileStream.buf_recycle = null;
						}
					}
				}
				if (this.buf == null)
				{
					this.buf = new byte[size];
				}
				else
				{
					Array.Clear(this.buf, 0, size);
				}
			}
			this.buf_size = size;
		}

		// Token: 0x060069B2 RID: 27058 RVA: 0x00169680 File Offset: 0x00167880
		private string GetSecureFileName(string filename)
		{
			if (!this.anonymous)
			{
				return Path.GetFullPath(filename);
			}
			return Path.GetFileName(filename);
		}

		// Token: 0x060069B3 RID: 27059 RVA: 0x00169697 File Offset: 0x00167897
		private string GetSecureFileName(string filename, bool full)
		{
			if (this.anonymous)
			{
				return Path.GetFileName(filename);
			}
			if (!full)
			{
				return filename;
			}
			return Path.GetFullPath(filename);
		}

		// Token: 0x04003D2E RID: 15662
		internal const int DefaultBufferSize = 4096;

		// Token: 0x04003D2F RID: 15663
		private static byte[] buf_recycle;

		// Token: 0x04003D30 RID: 15664
		private static readonly object buf_recycle_lock = new object();

		// Token: 0x04003D31 RID: 15665
		private byte[] buf;

		// Token: 0x04003D32 RID: 15666
		private string name;

		// Token: 0x04003D33 RID: 15667
		private SafeFileHandle safeHandle;

		// Token: 0x04003D34 RID: 15668
		private bool isExposed;

		// Token: 0x04003D35 RID: 15669
		private long append_startpos;

		// Token: 0x04003D36 RID: 15670
		private FileAccess access;

		// Token: 0x04003D37 RID: 15671
		private bool owner;

		// Token: 0x04003D38 RID: 15672
		private bool async;

		// Token: 0x04003D39 RID: 15673
		private bool canseek;

		// Token: 0x04003D3A RID: 15674
		private bool anonymous;

		// Token: 0x04003D3B RID: 15675
		private bool buf_dirty;

		// Token: 0x04003D3C RID: 15676
		private int buf_size;

		// Token: 0x04003D3D RID: 15677
		private int buf_length;

		// Token: 0x04003D3E RID: 15678
		private int buf_offset;

		// Token: 0x04003D3F RID: 15679
		private long buf_start;

		// Token: 0x02000B5F RID: 2911
		// (Invoke) Token: 0x060069B6 RID: 27062
		private delegate int ReadDelegate(byte[] buffer, int offset, int count);

		// Token: 0x02000B60 RID: 2912
		// (Invoke) Token: 0x060069BA RID: 27066
		private delegate void WriteDelegate(byte[] buffer, int offset, int count);
	}
}
