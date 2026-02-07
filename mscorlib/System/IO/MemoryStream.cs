using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	// Token: 0x02000B0C RID: 2828
	[Serializable]
	public class MemoryStream : Stream
	{
		// Token: 0x060064E3 RID: 25827 RVA: 0x00156C17 File Offset: 0x00154E17
		public MemoryStream() : this(0)
		{
		}

		// Token: 0x060064E4 RID: 25828 RVA: 0x00156C20 File Offset: 0x00154E20
		public MemoryStream(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", "Capacity must be positive.");
			}
			this._buffer = ((capacity != 0) ? new byte[capacity] : Array.Empty<byte>());
			this._capacity = capacity;
			this._expandable = true;
			this._writable = true;
			this._exposable = true;
			this._origin = 0;
			this._isOpen = true;
		}

		// Token: 0x060064E5 RID: 25829 RVA: 0x00156C87 File Offset: 0x00154E87
		public MemoryStream(byte[] buffer) : this(buffer, true)
		{
		}

		// Token: 0x060064E6 RID: 25830 RVA: 0x00156C94 File Offset: 0x00154E94
		public MemoryStream(byte[] buffer, bool writable)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			this._buffer = buffer;
			this._length = (this._capacity = buffer.Length);
			this._writable = writable;
			this._exposable = false;
			this._origin = 0;
			this._isOpen = true;
		}

		// Token: 0x060064E7 RID: 25831 RVA: 0x00156CEF File Offset: 0x00154EEF
		public MemoryStream(byte[] buffer, int index, int count) : this(buffer, index, count, true, false)
		{
		}

		// Token: 0x060064E8 RID: 25832 RVA: 0x00156CFC File Offset: 0x00154EFC
		public MemoryStream(byte[] buffer, int index, int count, bool writable) : this(buffer, index, count, writable, false)
		{
		}

		// Token: 0x060064E9 RID: 25833 RVA: 0x00156D0C File Offset: 0x00154F0C
		public MemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			this._buffer = buffer;
			this._position = index;
			this._origin = index;
			this._length = (this._capacity = index + count);
			this._writable = writable;
			this._exposable = publiclyVisible;
			this._expandable = false;
			this._isOpen = true;
		}

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x060064EA RID: 25834 RVA: 0x00156DB4 File Offset: 0x00154FB4
		public override bool CanRead
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x060064EB RID: 25835 RVA: 0x00156DB4 File Offset: 0x00154FB4
		public override bool CanSeek
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x060064EC RID: 25836 RVA: 0x00156DBC File Offset: 0x00154FBC
		public override bool CanWrite
		{
			get
			{
				return this._writable;
			}
		}

		// Token: 0x060064ED RID: 25837 RVA: 0x00156DC4 File Offset: 0x00154FC4
		private void EnsureNotClosed()
		{
			if (!this._isOpen)
			{
				throw Error.GetStreamIsClosed();
			}
		}

		// Token: 0x060064EE RID: 25838 RVA: 0x00156DD4 File Offset: 0x00154FD4
		private void EnsureWriteable()
		{
			if (!this.CanWrite)
			{
				throw Error.GetWriteNotSupported();
			}
		}

		// Token: 0x060064EF RID: 25839 RVA: 0x00156DE4 File Offset: 0x00154FE4
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this._isOpen = false;
					this._writable = false;
					this._expandable = false;
					this._lastReadTask = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060064F0 RID: 25840 RVA: 0x00156E2C File Offset: 0x0015502C
		private bool EnsureCapacity(int value)
		{
			if (value < 0)
			{
				throw new IOException("Stream was too long.");
			}
			if (value > this._capacity)
			{
				int num = value;
				if (num < 256)
				{
					num = 256;
				}
				if (num < this._capacity * 2)
				{
					num = this._capacity * 2;
				}
				if (this._capacity * 2 > 2147483591)
				{
					num = ((value > 2147483591) ? value : 2147483591);
				}
				this.Capacity = num;
				return true;
			}
			return false;
		}

		// Token: 0x060064F1 RID: 25841 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public override void Flush()
		{
		}

		// Token: 0x060064F2 RID: 25842 RVA: 0x00156EA0 File Offset: 0x001550A0
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

		// Token: 0x060064F3 RID: 25843 RVA: 0x00156EE8 File Offset: 0x001550E8
		public virtual byte[] GetBuffer()
		{
			if (!this._exposable)
			{
				throw new UnauthorizedAccessException("MemoryStream's internal buffer cannot be accessed.");
			}
			return this._buffer;
		}

		// Token: 0x060064F4 RID: 25844 RVA: 0x00156F03 File Offset: 0x00155103
		public virtual bool TryGetBuffer(out ArraySegment<byte> buffer)
		{
			if (!this._exposable)
			{
				buffer = default(ArraySegment<byte>);
				return false;
			}
			buffer = new ArraySegment<byte>(this._buffer, this._origin, this._length - this._origin);
			return true;
		}

		// Token: 0x060064F5 RID: 25845 RVA: 0x00156F3B File Offset: 0x0015513B
		internal byte[] InternalGetBuffer()
		{
			return this._buffer;
		}

		// Token: 0x060064F6 RID: 25846 RVA: 0x00156F43 File Offset: 0x00155143
		internal void InternalGetOriginAndLength(out int origin, out int length)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			origin = this._origin;
			length = this._length;
		}

		// Token: 0x060064F7 RID: 25847 RVA: 0x00156F62 File Offset: 0x00155162
		internal int InternalGetPosition()
		{
			return this._position;
		}

		// Token: 0x060064F8 RID: 25848 RVA: 0x00156F6C File Offset: 0x0015516C
		internal int InternalReadInt32()
		{
			this.EnsureNotClosed();
			int num = this._position += 4;
			if (num > this._length)
			{
				this._position = this._length;
				throw Error.GetEndOfFile();
			}
			return (int)this._buffer[num - 4] | (int)this._buffer[num - 3] << 8 | (int)this._buffer[num - 2] << 16 | (int)this._buffer[num - 1] << 24;
		}

		// Token: 0x060064F9 RID: 25849 RVA: 0x00156FE0 File Offset: 0x001551E0
		internal int InternalEmulateRead(int count)
		{
			this.EnsureNotClosed();
			int num = this._length - this._position;
			if (num > count)
			{
				num = count;
			}
			if (num < 0)
			{
				num = 0;
			}
			this._position += num;
			return num;
		}

		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x060064FA RID: 25850 RVA: 0x0015701C File Offset: 0x0015521C
		// (set) Token: 0x060064FB RID: 25851 RVA: 0x00157034 File Offset: 0x00155234
		public virtual int Capacity
		{
			get
			{
				this.EnsureNotClosed();
				return this._capacity - this._origin;
			}
			set
			{
				if ((long)value < this.Length)
				{
					throw new ArgumentOutOfRangeException("value", "capacity was less than the current size.");
				}
				this.EnsureNotClosed();
				if (!this._expandable && value != this.Capacity)
				{
					throw new NotSupportedException("Memory stream is not expandable.");
				}
				if (this._expandable && value != this._capacity)
				{
					if (value > 0)
					{
						byte[] array = new byte[value];
						if (this._length > 0)
						{
							Buffer.BlockCopy(this._buffer, 0, array, 0, this._length);
						}
						this._buffer = array;
					}
					else
					{
						this._buffer = null;
					}
					this._capacity = value;
				}
			}
		}

		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x060064FC RID: 25852 RVA: 0x001570CD File Offset: 0x001552CD
		public override long Length
		{
			get
			{
				this.EnsureNotClosed();
				return (long)(this._length - this._origin);
			}
		}

		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x060064FD RID: 25853 RVA: 0x001570E3 File Offset: 0x001552E3
		// (set) Token: 0x060064FE RID: 25854 RVA: 0x001570FC File Offset: 0x001552FC
		public override long Position
		{
			get
			{
				this.EnsureNotClosed();
				return (long)(this._position - this._origin);
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", "Non-negative number required.");
				}
				this.EnsureNotClosed();
				if (value > 2147483647L)
				{
					throw new ArgumentOutOfRangeException("value", "Stream length must be non-negative and less than 2^31 - 1 - origin.");
				}
				this._position = this._origin + (int)value;
			}
		}

		// Token: 0x060064FF RID: 25855 RVA: 0x0015714C File Offset: 0x0015534C
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
			this.EnsureNotClosed();
			int num = this._length - this._position;
			if (num > count)
			{
				num = count;
			}
			if (num <= 0)
			{
				return 0;
			}
			if (num <= 8)
			{
				int num2 = num;
				while (--num2 >= 0)
				{
					buffer[offset + num2] = this._buffer[this._position + num2];
				}
			}
			else
			{
				Buffer.BlockCopy(this._buffer, this._position, buffer, offset, num);
			}
			this._position += num;
			return num;
		}

		// Token: 0x06006500 RID: 25856 RVA: 0x00157210 File Offset: 0x00155410
		public override int Read(Span<byte> buffer)
		{
			if (base.GetType() != typeof(MemoryStream))
			{
				return base.Read(buffer);
			}
			this.EnsureNotClosed();
			int num = Math.Min(this._length - this._position, buffer.Length);
			if (num <= 0)
			{
				return 0;
			}
			new Span<byte>(this._buffer, this._position, num).CopyTo(buffer);
			this._position += num;
			return num;
		}

		// Token: 0x06006501 RID: 25857 RVA: 0x0015728C File Offset: 0x0015548C
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
			catch (OperationCanceledException exception)
			{
				task = Task.FromCancellation<int>(exception);
			}
			catch (Exception exception2)
			{
				task = Task.FromException<int>(exception2);
			}
			return task;
		}

		// Token: 0x06006502 RID: 25858 RVA: 0x00157358 File Offset: 0x00155558
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
			catch (OperationCanceledException exception)
			{
				result = new ValueTask<int>(Task.FromCancellation<int>(exception));
			}
			catch (Exception exception2)
			{
				result = new ValueTask<int>(Task.FromException<int>(exception2));
			}
			return result;
		}

		// Token: 0x06006503 RID: 25859 RVA: 0x001573F4 File Offset: 0x001555F4
		public override int ReadByte()
		{
			this.EnsureNotClosed();
			if (this._position >= this._length)
			{
				return -1;
			}
			byte[] buffer = this._buffer;
			int position = this._position;
			this._position = position + 1;
			return buffer[position];
		}

		// Token: 0x06006504 RID: 25860 RVA: 0x00157430 File Offset: 0x00155630
		public override void CopyTo(Stream destination, int bufferSize)
		{
			StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
			if (base.GetType() != typeof(MemoryStream))
			{
				base.CopyTo(destination, bufferSize);
				return;
			}
			int position = this._position;
			int num = this.InternalEmulateRead(this._length - position);
			if (num > 0)
			{
				destination.Write(this._buffer, position, num);
			}
		}

		// Token: 0x06006505 RID: 25861 RVA: 0x00157490 File Offset: 0x00155690
		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
			if (base.GetType() != typeof(MemoryStream))
			{
				return base.CopyToAsync(destination, bufferSize, cancellationToken);
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			int position = this._position;
			int num = this.InternalEmulateRead(this._length - this._position);
			if (num == 0)
			{
				return Task.CompletedTask;
			}
			MemoryStream memoryStream = destination as MemoryStream;
			if (memoryStream == null)
			{
				return destination.WriteAsync(this._buffer, position, num, cancellationToken);
			}
			Task result;
			try
			{
				memoryStream.Write(this._buffer, position, num);
				result = Task.CompletedTask;
			}
			catch (Exception exception)
			{
				result = Task.FromException(exception);
			}
			return result;
		}

		// Token: 0x06006506 RID: 25862 RVA: 0x00157544 File Offset: 0x00155744
		public override long Seek(long offset, SeekOrigin loc)
		{
			this.EnsureNotClosed();
			if (offset > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("offset", "Stream length must be non-negative and less than 2^31 - 1 - origin.");
			}
			switch (loc)
			{
			case SeekOrigin.Begin:
			{
				int num = this._origin + (int)offset;
				if (offset < 0L || num < this._origin)
				{
					throw new IOException("An attempt was made to move the position before the beginning of the stream.");
				}
				this._position = num;
				break;
			}
			case SeekOrigin.Current:
			{
				int num2 = this._position + (int)offset;
				if ((long)this._position + offset < (long)this._origin || num2 < this._origin)
				{
					throw new IOException("An attempt was made to move the position before the beginning of the stream.");
				}
				this._position = num2;
				break;
			}
			case SeekOrigin.End:
			{
				int num3 = this._length + (int)offset;
				if ((long)this._length + offset < (long)this._origin || num3 < this._origin)
				{
					throw new IOException("An attempt was made to move the position before the beginning of the stream.");
				}
				this._position = num3;
				break;
			}
			default:
				throw new ArgumentException("Invalid seek origin.");
			}
			return (long)this._position;
		}

		// Token: 0x06006507 RID: 25863 RVA: 0x00157638 File Offset: 0x00155838
		public override void SetLength(long value)
		{
			if (value < 0L || value > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("value", "Stream length must be non-negative and less than 2^31 - 1 - origin.");
			}
			this.EnsureWriteable();
			if (value > (long)(2147483647 - this._origin))
			{
				throw new ArgumentOutOfRangeException("value", "Stream length must be non-negative and less than 2^31 - 1 - origin.");
			}
			int num = this._origin + (int)value;
			if (!this.EnsureCapacity(num) && num > this._length)
			{
				Array.Clear(this._buffer, this._length, num - this._length);
			}
			this._length = num;
			if (this._position > num)
			{
				this._position = num;
			}
		}

		// Token: 0x06006508 RID: 25864 RVA: 0x001576D8 File Offset: 0x001558D8
		public virtual byte[] ToArray()
		{
			int num = this._length - this._origin;
			if (num == 0)
			{
				return Array.Empty<byte>();
			}
			byte[] array = new byte[num];
			Buffer.BlockCopy(this._buffer, this._origin, array, 0, num);
			return array;
		}

		// Token: 0x06006509 RID: 25865 RVA: 0x00157718 File Offset: 0x00155918
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
			this.EnsureNotClosed();
			this.EnsureWriteable();
			int num = this._position + count;
			if (num < 0)
			{
				throw new IOException("Stream was too long.");
			}
			if (num > this._length)
			{
				bool flag = this._position > this._length;
				if (num > this._capacity && this.EnsureCapacity(num))
				{
					flag = false;
				}
				if (flag)
				{
					Array.Clear(this._buffer, this._length, num - this._length);
				}
				this._length = num;
			}
			if (count <= 8 && buffer != this._buffer)
			{
				int num2 = count;
				while (--num2 >= 0)
				{
					this._buffer[this._position + num2] = buffer[offset + num2];
				}
			}
			else
			{
				Buffer.BlockCopy(buffer, offset, this._buffer, this._position, count);
			}
			this._position = num;
		}

		// Token: 0x0600650A RID: 25866 RVA: 0x00157830 File Offset: 0x00155A30
		public override void Write(ReadOnlySpan<byte> buffer)
		{
			if (base.GetType() != typeof(MemoryStream))
			{
				base.Write(buffer);
				return;
			}
			this.EnsureNotClosed();
			this.EnsureWriteable();
			int num = this._position + buffer.Length;
			if (num < 0)
			{
				throw new IOException("Stream was too long.");
			}
			if (num > this._length)
			{
				bool flag = this._position > this._length;
				if (num > this._capacity && this.EnsureCapacity(num))
				{
					flag = false;
				}
				if (flag)
				{
					Array.Clear(this._buffer, this._length, num - this._length);
				}
				this._length = num;
			}
			buffer.CopyTo(new Span<byte>(this._buffer, this._position, buffer.Length));
			this._position = num;
		}

		// Token: 0x0600650B RID: 25867 RVA: 0x001578FC File Offset: 0x00155AFC
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
			catch (OperationCanceledException exception)
			{
				result = Task.FromCancellation<VoidTaskResult>(exception);
			}
			catch (Exception exception2)
			{
				result = Task.FromException(exception2);
			}
			return result;
		}

		// Token: 0x0600650C RID: 25868 RVA: 0x001579A8 File Offset: 0x00155BA8
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
			catch (OperationCanceledException exception)
			{
				valueTask = new ValueTask(Task.FromCancellation<VoidTaskResult>(exception));
			}
			catch (Exception exception2)
			{
				valueTask = new ValueTask(Task.FromException(exception2));
			}
			return valueTask;
		}

		// Token: 0x0600650D RID: 25869 RVA: 0x00157A44 File Offset: 0x00155C44
		public override void WriteByte(byte value)
		{
			this.EnsureNotClosed();
			this.EnsureWriteable();
			if (this._position >= this._length)
			{
				int num = this._position + 1;
				bool flag = this._position > this._length;
				if (num >= this._capacity && this.EnsureCapacity(num))
				{
					flag = false;
				}
				if (flag)
				{
					Array.Clear(this._buffer, this._length, this._position - this._length);
				}
				this._length = num;
			}
			byte[] buffer = this._buffer;
			int position = this._position;
			this._position = position + 1;
			buffer[position] = value;
		}

		// Token: 0x0600650E RID: 25870 RVA: 0x00157AD8 File Offset: 0x00155CD8
		public virtual void WriteTo(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream", "Stream cannot be null.");
			}
			this.EnsureNotClosed();
			stream.Write(this._buffer, this._origin, this._length - this._origin);
		}

		// Token: 0x04003B4B RID: 15179
		private byte[] _buffer;

		// Token: 0x04003B4C RID: 15180
		private int _origin;

		// Token: 0x04003B4D RID: 15181
		private int _position;

		// Token: 0x04003B4E RID: 15182
		private int _length;

		// Token: 0x04003B4F RID: 15183
		private int _capacity;

		// Token: 0x04003B50 RID: 15184
		private bool _expandable;

		// Token: 0x04003B51 RID: 15185
		private bool _writable;

		// Token: 0x04003B52 RID: 15186
		private bool _exposable;

		// Token: 0x04003B53 RID: 15187
		private bool _isOpen;

		// Token: 0x04003B54 RID: 15188
		[NonSerialized]
		private Task<int> _lastReadTask;

		// Token: 0x04003B55 RID: 15189
		private const int MemStreamMaxLength = 2147483647;
	}
}
