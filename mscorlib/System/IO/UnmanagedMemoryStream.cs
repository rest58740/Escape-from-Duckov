using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	// Token: 0x02000B29 RID: 2857
	public class UnmanagedMemoryStream : Stream
	{
		// Token: 0x06006695 RID: 26261 RVA: 0x0015E336 File Offset: 0x0015C536
		protected UnmanagedMemoryStream()
		{
			this._mem = null;
			this._isOpen = false;
		}

		// Token: 0x06006696 RID: 26262 RVA: 0x0015E34D File Offset: 0x0015C54D
		public UnmanagedMemoryStream(SafeBuffer buffer, long offset, long length)
		{
			this.Initialize(buffer, offset, length, FileAccess.Read);
		}

		// Token: 0x06006697 RID: 26263 RVA: 0x0015E35F File Offset: 0x0015C55F
		public UnmanagedMemoryStream(SafeBuffer buffer, long offset, long length, FileAccess access)
		{
			this.Initialize(buffer, offset, length, access);
		}

		// Token: 0x06006698 RID: 26264 RVA: 0x0015E374 File Offset: 0x0015C574
		protected unsafe void Initialize(SafeBuffer buffer, long offset, long length, FileAccess access)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0L)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (length < 0L)
			{
				throw new ArgumentOutOfRangeException("length", "Non-negative number required.");
			}
			if (buffer.ByteLength < (ulong)(offset + length))
			{
				throw new ArgumentException("Offset and length were greater than the size of the SafeBuffer.");
			}
			if (access < FileAccess.Read || access > FileAccess.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("access");
			}
			if (this._isOpen)
			{
				throw new InvalidOperationException("The method cannot be called twice on the same instance.");
			}
			byte* ptr = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				buffer.AcquirePointer(ref ptr);
				if (ptr + offset + length < ptr)
				{
					throw new ArgumentException("The UnmanagedMemoryStream capacity would wrap around the high end of the address space.");
				}
			}
			finally
			{
				if (ptr != null)
				{
					buffer.ReleasePointer();
				}
			}
			this._offset = offset;
			this._buffer = buffer;
			this._length = length;
			this._capacity = length;
			this._access = access;
			this._isOpen = true;
		}

		// Token: 0x06006699 RID: 26265 RVA: 0x0015E468 File Offset: 0x0015C668
		[CLSCompliant(false)]
		public unsafe UnmanagedMemoryStream(byte* pointer, long length)
		{
			this.Initialize(pointer, length, length, FileAccess.Read);
		}

		// Token: 0x0600669A RID: 26266 RVA: 0x0015E47A File Offset: 0x0015C67A
		[CLSCompliant(false)]
		public unsafe UnmanagedMemoryStream(byte* pointer, long length, long capacity, FileAccess access)
		{
			this.Initialize(pointer, length, capacity, access);
		}

		// Token: 0x0600669B RID: 26267 RVA: 0x0015E490 File Offset: 0x0015C690
		[CLSCompliant(false)]
		protected unsafe void Initialize(byte* pointer, long length, long capacity, FileAccess access)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			if (length < 0L || capacity < 0L)
			{
				throw new ArgumentOutOfRangeException((length < 0L) ? "length" : "capacity", "Non-negative number required.");
			}
			if (length > capacity)
			{
				throw new ArgumentOutOfRangeException("length", "The length cannot be greater than the capacity.");
			}
			if (pointer + capacity < pointer)
			{
				throw new ArgumentOutOfRangeException("capacity", "The UnmanagedMemoryStream capacity would wrap around the high end of the address space.");
			}
			if (access < FileAccess.Read || access > FileAccess.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("access", "Enum value was out of legal range.");
			}
			if (this._isOpen)
			{
				throw new InvalidOperationException("The method cannot be called twice on the same instance.");
			}
			this._mem = pointer;
			this._offset = 0L;
			this._length = length;
			this._capacity = capacity;
			this._access = access;
			this._isOpen = true;
		}

		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x0600669C RID: 26268 RVA: 0x0015E558 File Offset: 0x0015C758
		public override bool CanRead
		{
			get
			{
				return this._isOpen && (this._access & FileAccess.Read) > (FileAccess)0;
			}
		}

		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x0600669D RID: 26269 RVA: 0x0015E56F File Offset: 0x0015C76F
		public override bool CanSeek
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x0600669E RID: 26270 RVA: 0x0015E577 File Offset: 0x0015C777
		public override bool CanWrite
		{
			get
			{
				return this._isOpen && (this._access & FileAccess.Write) > (FileAccess)0;
			}
		}

		// Token: 0x0600669F RID: 26271 RVA: 0x0015E58E File Offset: 0x0015C78E
		protected override void Dispose(bool disposing)
		{
			this._isOpen = false;
			this._mem = null;
			base.Dispose(disposing);
		}

		// Token: 0x060066A0 RID: 26272 RVA: 0x0015E5A6 File Offset: 0x0015C7A6
		private void EnsureNotClosed()
		{
			if (!this._isOpen)
			{
				throw Error.GetStreamIsClosed();
			}
		}

		// Token: 0x060066A1 RID: 26273 RVA: 0x0015E5B6 File Offset: 0x0015C7B6
		private void EnsureReadable()
		{
			if (!this.CanRead)
			{
				throw Error.GetReadNotSupported();
			}
		}

		// Token: 0x060066A2 RID: 26274 RVA: 0x00156DD4 File Offset: 0x00154FD4
		private void EnsureWriteable()
		{
			if (!this.CanWrite)
			{
				throw Error.GetWriteNotSupported();
			}
		}

		// Token: 0x060066A3 RID: 26275 RVA: 0x0015E5C6 File Offset: 0x0015C7C6
		public override void Flush()
		{
			this.EnsureNotClosed();
		}

		// Token: 0x060066A4 RID: 26276 RVA: 0x0015E5D0 File Offset: 0x0015C7D0
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			Task result;
			try
			{
				this.Flush();
				result = Task.CompletedTask;
			}
			catch (Exception exception)
			{
				result = Task.FromException(exception);
			}
			return result;
		}

		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x060066A5 RID: 26277 RVA: 0x0015E618 File Offset: 0x0015C818
		public override long Length
		{
			get
			{
				this.EnsureNotClosed();
				return Interlocked.Read(ref this._length);
			}
		}

		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x060066A6 RID: 26278 RVA: 0x0015E62B File Offset: 0x0015C82B
		public long Capacity
		{
			get
			{
				this.EnsureNotClosed();
				return this._capacity;
			}
		}

		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x060066A7 RID: 26279 RVA: 0x0015E639 File Offset: 0x0015C839
		// (set) Token: 0x060066A8 RID: 26280 RVA: 0x0015E654 File Offset: 0x0015C854
		public override long Position
		{
			get
			{
				if (!this.CanSeek)
				{
					throw Error.GetStreamIsClosed();
				}
				return Interlocked.Read(ref this._position);
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", "Non-negative number required.");
				}
				if (!this.CanSeek)
				{
					throw Error.GetStreamIsClosed();
				}
				Interlocked.Exchange(ref this._position, value);
			}
		}

		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x060066A9 RID: 26281 RVA: 0x0015E688 File Offset: 0x0015C888
		// (set) Token: 0x060066AA RID: 26282 RVA: 0x0015E6D8 File Offset: 0x0015C8D8
		[CLSCompliant(false)]
		public unsafe byte* PositionPointer
		{
			get
			{
				if (this._buffer != null)
				{
					throw new NotSupportedException("This operation is not supported for an UnmanagedMemoryStream created from a SafeBuffer.");
				}
				this.EnsureNotClosed();
				long num = Interlocked.Read(ref this._position);
				if (num > this._capacity)
				{
					throw new IndexOutOfRangeException("Unmanaged memory stream position was beyond the capacity of the stream.");
				}
				return this._mem + num;
			}
			set
			{
				if (this._buffer != null)
				{
					throw new NotSupportedException("This operation is not supported for an UnmanagedMemoryStream created from a SafeBuffer.");
				}
				this.EnsureNotClosed();
				if (value < this._mem)
				{
					throw new IOException("An attempt was made to move the position before the beginning of the stream.");
				}
				long num = (long)(value - this._mem);
				if (num < 0L)
				{
					throw new ArgumentOutOfRangeException("offset", "UnmanagedMemoryStream length must be non-negative and less than 2^63 - 1 - baseAddress.");
				}
				Interlocked.Exchange(ref this._position, num);
			}
		}

		// Token: 0x060066AB RID: 26283 RVA: 0x0015E740 File Offset: 0x0015C940
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			return this.ReadCore(new Span<byte>(buffer, offset, count));
		}

		// Token: 0x060066AC RID: 26284 RVA: 0x0015E7A9 File Offset: 0x0015C9A9
		public override int Read(Span<byte> buffer)
		{
			if (base.GetType() == typeof(UnmanagedMemoryStream))
			{
				return this.ReadCore(buffer);
			}
			return base.Read(buffer);
		}

		// Token: 0x060066AD RID: 26285 RVA: 0x0015E7D4 File Offset: 0x0015C9D4
		internal unsafe int ReadCore(Span<byte> buffer)
		{
			this.EnsureNotClosed();
			this.EnsureReadable();
			long num = Interlocked.Read(ref this._position);
			long num2 = Math.Min(Interlocked.Read(ref this._length) - num, (long)buffer.Length);
			if (num2 <= 0L)
			{
				return 0;
			}
			int num3 = (int)num2;
			if (num3 < 0)
			{
				return 0;
			}
			fixed (byte* reference = MemoryMarshal.GetReference<byte>(buffer))
			{
				byte* dest = reference;
				if (this._buffer != null)
				{
					byte* ptr = null;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						this._buffer.AcquirePointer(ref ptr);
						Buffer.Memcpy(dest, ptr + num + this._offset, num3);
						goto IL_A5;
					}
					finally
					{
						if (ptr != null)
						{
							this._buffer.ReleasePointer();
						}
					}
				}
				Buffer.Memcpy(dest, this._mem + num, num3);
				IL_A5:;
			}
			Interlocked.Exchange(ref this._position, num + num2);
			return num3;
		}

		// Token: 0x060066AE RID: 26286 RVA: 0x0015E8AC File Offset: 0x0015CAAC
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<int>(cancellationToken);
			}
			Task<int> task;
			try
			{
				int num = this.Read(buffer, offset, count);
				Task<int> lastReadTask = this._lastReadTask;
				Task<int> task2;
				if (lastReadTask == null || lastReadTask.Result != num)
				{
					task = (this._lastReadTask = Task.FromResult<int>(num));
					task2 = task;
				}
				else
				{
					task2 = lastReadTask;
				}
				task = task2;
			}
			catch (Exception exception)
			{
				task = Task.FromException<int>(exception);
			}
			return task;
		}

		// Token: 0x060066AF RID: 26287 RVA: 0x0015E964 File Offset: 0x0015CB64
		public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
			}
			ValueTask<int> result;
			try
			{
				ArraySegment<byte> arraySegment;
				result = new ValueTask<int>(MemoryMarshal.TryGetArray<byte>(buffer, out arraySegment) ? this.Read(arraySegment.Array, arraySegment.Offset, arraySegment.Count) : this.Read(buffer.Span));
			}
			catch (Exception exception)
			{
				result = new ValueTask<int>(Task.FromException<int>(exception));
			}
			return result;
		}

		// Token: 0x060066B0 RID: 26288 RVA: 0x0015E9E8 File Offset: 0x0015CBE8
		public unsafe override int ReadByte()
		{
			this.EnsureNotClosed();
			this.EnsureReadable();
			long num = Interlocked.Read(ref this._position);
			long num2 = Interlocked.Read(ref this._length);
			if (num >= num2)
			{
				return -1;
			}
			Interlocked.Exchange(ref this._position, num + 1L);
			if (this._buffer != null)
			{
				byte* ptr = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					this._buffer.AcquirePointer(ref ptr);
					return (int)(ptr + num)[this._offset];
				}
				finally
				{
					if (ptr != null)
					{
						this._buffer.ReleasePointer();
					}
				}
			}
			return (int)this._mem[num];
		}

		// Token: 0x060066B1 RID: 26289 RVA: 0x0015EA8C File Offset: 0x0015CC8C
		public override long Seek(long offset, SeekOrigin loc)
		{
			this.EnsureNotClosed();
			switch (loc)
			{
			case SeekOrigin.Begin:
				if (offset < 0L)
				{
					throw new IOException("An attempt was made to move the position before the beginning of the stream.");
				}
				Interlocked.Exchange(ref this._position, offset);
				break;
			case SeekOrigin.Current:
			{
				long num = Interlocked.Read(ref this._position);
				if (offset + num < 0L)
				{
					throw new IOException("An attempt was made to move the position before the beginning of the stream.");
				}
				Interlocked.Exchange(ref this._position, offset + num);
				break;
			}
			case SeekOrigin.End:
			{
				long num2 = Interlocked.Read(ref this._length);
				if (num2 + offset < 0L)
				{
					throw new IOException("An attempt was made to move the position before the beginning of the stream.");
				}
				Interlocked.Exchange(ref this._position, num2 + offset);
				break;
			}
			default:
				throw new ArgumentException("Invalid seek origin.");
			}
			return Interlocked.Read(ref this._position);
		}

		// Token: 0x060066B2 RID: 26290 RVA: 0x0015EB48 File Offset: 0x0015CD48
		public override void SetLength(long value)
		{
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("value", "Non-negative number required.");
			}
			if (this._buffer != null)
			{
				throw new NotSupportedException("This operation is not supported for an UnmanagedMemoryStream created from a SafeBuffer.");
			}
			this.EnsureNotClosed();
			this.EnsureWriteable();
			if (value > this._capacity)
			{
				throw new IOException("Unable to expand length of this stream beyond its capacity.");
			}
			long num = Interlocked.Read(ref this._position);
			long num2 = Interlocked.Read(ref this._length);
			if (value > num2)
			{
				Buffer.ZeroMemory(this._mem + num2, value - num2);
			}
			Interlocked.Exchange(ref this._length, value);
			if (num > value)
			{
				Interlocked.Exchange(ref this._position, value);
			}
		}

		// Token: 0x060066B3 RID: 26291 RVA: 0x0015EBE8 File Offset: 0x0015CDE8
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			this.WriteCore(new Span<byte>(buffer, offset, count));
		}

		// Token: 0x060066B4 RID: 26292 RVA: 0x0015EC56 File Offset: 0x0015CE56
		public override void Write(ReadOnlySpan<byte> buffer)
		{
			if (base.GetType() == typeof(UnmanagedMemoryStream))
			{
				this.WriteCore(buffer);
				return;
			}
			base.Write(buffer);
		}

		// Token: 0x060066B5 RID: 26293 RVA: 0x0015EC80 File Offset: 0x0015CE80
		internal unsafe void WriteCore(ReadOnlySpan<byte> buffer)
		{
			this.EnsureNotClosed();
			this.EnsureWriteable();
			long num = Interlocked.Read(ref this._position);
			long num2 = Interlocked.Read(ref this._length);
			long num3 = num + (long)buffer.Length;
			if (num3 < 0L)
			{
				throw new IOException("Stream was too long.");
			}
			if (num3 > this._capacity)
			{
				throw new NotSupportedException("Unable to expand length of this stream beyond its capacity.");
			}
			if (this._buffer == null)
			{
				if (num > num2)
				{
					Buffer.ZeroMemory(this._mem + num2, num - num2);
				}
				if (num3 > num2)
				{
					Interlocked.Exchange(ref this._length, num3);
				}
			}
			fixed (byte* reference = MemoryMarshal.GetReference<byte>(buffer))
			{
				byte* src = reference;
				if (this._buffer != null)
				{
					if (this._capacity - num < (long)buffer.Length)
					{
						throw new ArgumentException("Not enough space available in the buffer.");
					}
					byte* ptr = null;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						this._buffer.AcquirePointer(ref ptr);
						Buffer.Memcpy(ptr + num + this._offset, src, buffer.Length);
						goto IL_10C;
					}
					finally
					{
						if (ptr != null)
						{
							this._buffer.ReleasePointer();
						}
					}
				}
				Buffer.Memcpy(this._mem + num, src, buffer.Length);
				IL_10C:;
			}
			Interlocked.Exchange(ref this._position, num3);
		}

		// Token: 0x060066B6 RID: 26294 RVA: 0x0015EDBC File Offset: 0x0015CFBC
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			Task result;
			try
			{
				this.Write(buffer, offset, count);
				result = Task.CompletedTask;
			}
			catch (Exception exception)
			{
				result = Task.FromException(exception);
			}
			return result;
		}

		// Token: 0x060066B7 RID: 26295 RVA: 0x0015EE54 File Offset: 0x0015D054
		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask(Task.FromCanceled(cancellationToken));
			}
			ValueTask valueTask;
			try
			{
				ArraySegment<byte> arraySegment;
				if (MemoryMarshal.TryGetArray<byte>(buffer, out arraySegment))
				{
					this.Write(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
				}
				else
				{
					this.Write(buffer.Span);
				}
				valueTask = default(ValueTask);
				valueTask = valueTask;
			}
			catch (Exception exception)
			{
				valueTask = new ValueTask(Task.FromException(exception));
			}
			return valueTask;
		}

		// Token: 0x060066B8 RID: 26296 RVA: 0x0015EED8 File Offset: 0x0015D0D8
		public unsafe override void WriteByte(byte value)
		{
			this.EnsureNotClosed();
			this.EnsureWriteable();
			long num = Interlocked.Read(ref this._position);
			long num2 = Interlocked.Read(ref this._length);
			long num3 = num + 1L;
			if (num >= num2)
			{
				if (num3 < 0L)
				{
					throw new IOException("Stream was too long.");
				}
				if (num3 > this._capacity)
				{
					throw new NotSupportedException("Unable to expand length of this stream beyond its capacity.");
				}
				if (this._buffer == null)
				{
					if (num > num2)
					{
						Buffer.ZeroMemory(this._mem + num2, num - num2);
					}
					Interlocked.Exchange(ref this._length, num3);
				}
			}
			if (this._buffer != null)
			{
				byte* ptr = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					this._buffer.AcquirePointer(ref ptr);
					(ptr + num)[this._offset] = value;
					goto IL_C4;
				}
				finally
				{
					if (ptr != null)
					{
						this._buffer.ReleasePointer();
					}
				}
			}
			this._mem[num] = value;
			IL_C4:
			Interlocked.Exchange(ref this._position, num3);
		}

		// Token: 0x04003C14 RID: 15380
		private SafeBuffer _buffer;

		// Token: 0x04003C15 RID: 15381
		private unsafe byte* _mem;

		// Token: 0x04003C16 RID: 15382
		private long _length;

		// Token: 0x04003C17 RID: 15383
		private long _capacity;

		// Token: 0x04003C18 RID: 15384
		private long _position;

		// Token: 0x04003C19 RID: 15385
		private long _offset;

		// Token: 0x04003C1A RID: 15386
		private FileAccess _access;

		// Token: 0x04003C1B RID: 15387
		internal bool _isOpen;

		// Token: 0x04003C1C RID: 15388
		private Task<int> _lastReadTask;
	}
}
