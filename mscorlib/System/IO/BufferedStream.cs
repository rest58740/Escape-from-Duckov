using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	// Token: 0x02000B41 RID: 2881
	public sealed class BufferedStream : Stream
	{
		// Token: 0x0600680A RID: 26634 RVA: 0x001629B3 File Offset: 0x00160BB3
		internal SemaphoreSlim LazyEnsureAsyncActiveSemaphoreInitialized()
		{
			return LazyInitializer.EnsureInitialized<SemaphoreSlim>(ref this._asyncActiveSemaphore, () => new SemaphoreSlim(1, 1));
		}

		// Token: 0x0600680B RID: 26635 RVA: 0x001629DF File Offset: 0x00160BDF
		public BufferedStream(Stream stream) : this(stream, 4096)
		{
		}

		// Token: 0x0600680C RID: 26636 RVA: 0x001629F0 File Offset: 0x00160BF0
		public BufferedStream(Stream stream, int bufferSize)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", SR.Format("'{0}' must be greater than zero.", "bufferSize"));
			}
			this._stream = stream;
			this._bufferSize = bufferSize;
			if (!this._stream.CanRead && !this._stream.CanWrite)
			{
				throw new ObjectDisposedException(null, "Cannot access a closed Stream.");
			}
		}

		// Token: 0x0600680D RID: 26637 RVA: 0x00162A63 File Offset: 0x00160C63
		private void EnsureNotClosed()
		{
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot access a closed Stream.");
			}
		}

		// Token: 0x0600680E RID: 26638 RVA: 0x00162A79 File Offset: 0x00160C79
		private void EnsureCanSeek()
		{
			if (!this._stream.CanSeek)
			{
				throw new NotSupportedException("Stream does not support seeking.");
			}
		}

		// Token: 0x0600680F RID: 26639 RVA: 0x00162A93 File Offset: 0x00160C93
		private void EnsureCanRead()
		{
			if (!this._stream.CanRead)
			{
				throw new NotSupportedException("Stream does not support reading.");
			}
		}

		// Token: 0x06006810 RID: 26640 RVA: 0x00162AAD File Offset: 0x00160CAD
		private void EnsureCanWrite()
		{
			if (!this._stream.CanWrite)
			{
				throw new NotSupportedException("Stream does not support writing.");
			}
		}

		// Token: 0x06006811 RID: 26641 RVA: 0x00162AC8 File Offset: 0x00160CC8
		private void EnsureShadowBufferAllocated()
		{
			if (this._buffer.Length != this._bufferSize || this._bufferSize >= 81920)
			{
				return;
			}
			byte[] array = new byte[Math.Min(this._bufferSize + this._bufferSize, 81920)];
			Buffer.BlockCopy(this._buffer, 0, array, 0, this._writePos);
			this._buffer = array;
		}

		// Token: 0x06006812 RID: 26642 RVA: 0x00162B2B File Offset: 0x00160D2B
		private void EnsureBufferAllocated()
		{
			if (this._buffer == null)
			{
				this._buffer = new byte[this._bufferSize];
			}
		}

		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x06006813 RID: 26643 RVA: 0x00162B46 File Offset: 0x00160D46
		public Stream UnderlyingStream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x06006814 RID: 26644 RVA: 0x00162B4E File Offset: 0x00160D4E
		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
		}

		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x06006815 RID: 26645 RVA: 0x00162B56 File Offset: 0x00160D56
		public override bool CanRead
		{
			get
			{
				return this._stream != null && this._stream.CanRead;
			}
		}

		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x06006816 RID: 26646 RVA: 0x00162B6D File Offset: 0x00160D6D
		public override bool CanWrite
		{
			get
			{
				return this._stream != null && this._stream.CanWrite;
			}
		}

		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x06006817 RID: 26647 RVA: 0x00162B84 File Offset: 0x00160D84
		public override bool CanSeek
		{
			get
			{
				return this._stream != null && this._stream.CanSeek;
			}
		}

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x06006818 RID: 26648 RVA: 0x00162B9B File Offset: 0x00160D9B
		public override long Length
		{
			get
			{
				this.EnsureNotClosed();
				if (this._writePos > 0)
				{
					this.FlushWrite();
				}
				return this._stream.Length;
			}
		}

		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x06006819 RID: 26649 RVA: 0x00162BBD File Offset: 0x00160DBD
		// (set) Token: 0x0600681A RID: 26650 RVA: 0x00162BEC File Offset: 0x00160DEC
		public override long Position
		{
			get
			{
				this.EnsureNotClosed();
				this.EnsureCanSeek();
				return this._stream.Position + (long)(this._readPos - this._readLen + this._writePos);
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", "Non-negative number required.");
				}
				this.EnsureNotClosed();
				this.EnsureCanSeek();
				if (this._writePos > 0)
				{
					this.FlushWrite();
				}
				this._readPos = 0;
				this._readLen = 0;
				this._stream.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x0600681B RID: 26651 RVA: 0x00162C48 File Offset: 0x00160E48
		public override ValueTask DisposeAsync()
		{
			BufferedStream.<DisposeAsync>d__34 <DisposeAsync>d__;
			<DisposeAsync>d__.<>4__this = this;
			<DisposeAsync>d__.<>t__builder = AsyncValueTaskMethodBuilder.Create();
			<DisposeAsync>d__.<>1__state = -1;
			<DisposeAsync>d__.<>t__builder.Start<BufferedStream.<DisposeAsync>d__34>(ref <DisposeAsync>d__);
			return <DisposeAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600681C RID: 26652 RVA: 0x00162C8C File Offset: 0x00160E8C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._stream != null)
				{
					try
					{
						this.Flush();
					}
					finally
					{
						this._stream.Dispose();
					}
				}
			}
			finally
			{
				this._stream = null;
				this._buffer = null;
				base.Dispose(disposing);
			}
		}

		// Token: 0x0600681D RID: 26653 RVA: 0x00162CEC File Offset: 0x00160EEC
		public override void Flush()
		{
			this.EnsureNotClosed();
			if (this._writePos > 0)
			{
				this.FlushWrite();
				return;
			}
			if (this._readPos < this._readLen)
			{
				if (this._stream.CanSeek)
				{
					this.FlushRead();
				}
				if (this._stream.CanWrite)
				{
					this._stream.Flush();
				}
				return;
			}
			if (this._stream.CanWrite)
			{
				this._stream.Flush();
			}
			this._writePos = (this._readPos = (this._readLen = 0));
		}

		// Token: 0x0600681E RID: 26654 RVA: 0x00162D7A File Offset: 0x00160F7A
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<int>(cancellationToken);
			}
			this.EnsureNotClosed();
			return this.FlushAsyncInternal(cancellationToken);
		}

		// Token: 0x0600681F RID: 26655 RVA: 0x00162D9C File Offset: 0x00160F9C
		private Task FlushAsyncInternal(CancellationToken cancellationToken)
		{
			BufferedStream.<FlushAsyncInternal>d__38 <FlushAsyncInternal>d__;
			<FlushAsyncInternal>d__.<>4__this = this;
			<FlushAsyncInternal>d__.cancellationToken = cancellationToken;
			<FlushAsyncInternal>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<FlushAsyncInternal>d__.<>1__state = -1;
			<FlushAsyncInternal>d__.<>t__builder.Start<BufferedStream.<FlushAsyncInternal>d__38>(ref <FlushAsyncInternal>d__);
			return <FlushAsyncInternal>d__.<>t__builder.Task;
		}

		// Token: 0x06006820 RID: 26656 RVA: 0x00162DE7 File Offset: 0x00160FE7
		private void FlushRead()
		{
			if (this._readPos - this._readLen != 0)
			{
				this._stream.Seek((long)(this._readPos - this._readLen), SeekOrigin.Current);
			}
			this._readPos = 0;
			this._readLen = 0;
		}

		// Token: 0x06006821 RID: 26657 RVA: 0x00162E24 File Offset: 0x00161024
		private void ClearReadBufferBeforeWrite()
		{
			if (this._readPos == this._readLen)
			{
				this._readPos = (this._readLen = 0);
				return;
			}
			if (!this._stream.CanSeek)
			{
				throw new NotSupportedException("Cannot write to a BufferedStream while the read buffer is not empty if the underlying stream is not seekable. Ensure that the stream underlying this BufferedStream can seek or avoid interleaving read and write operations on this BufferedStream.");
			}
			this.FlushRead();
		}

		// Token: 0x06006822 RID: 26658 RVA: 0x00162E6E File Offset: 0x0016106E
		private void FlushWrite()
		{
			this._stream.Write(this._buffer, 0, this._writePos);
			this._writePos = 0;
			this._stream.Flush();
		}

		// Token: 0x06006823 RID: 26659 RVA: 0x00162E9C File Offset: 0x0016109C
		private Task FlushWriteAsync(CancellationToken cancellationToken)
		{
			BufferedStream.<FlushWriteAsync>d__42 <FlushWriteAsync>d__;
			<FlushWriteAsync>d__.<>4__this = this;
			<FlushWriteAsync>d__.cancellationToken = cancellationToken;
			<FlushWriteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<FlushWriteAsync>d__.<>1__state = -1;
			<FlushWriteAsync>d__.<>t__builder.Start<BufferedStream.<FlushWriteAsync>d__42>(ref <FlushWriteAsync>d__);
			return <FlushWriteAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06006824 RID: 26660 RVA: 0x00162EE8 File Offset: 0x001610E8
		private int ReadFromBuffer(byte[] array, int offset, int count)
		{
			int num = this._readLen - this._readPos;
			if (num == 0)
			{
				return 0;
			}
			if (num > count)
			{
				num = count;
			}
			Buffer.BlockCopy(this._buffer, this._readPos, array, offset, num);
			this._readPos += num;
			return num;
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x00162F34 File Offset: 0x00161134
		private int ReadFromBuffer(Span<byte> destination)
		{
			int num = Math.Min(this._readLen - this._readPos, destination.Length);
			if (num > 0)
			{
				new ReadOnlySpan<byte>(this._buffer, this._readPos, num).CopyTo(destination);
				this._readPos += num;
			}
			return num;
		}

		// Token: 0x06006826 RID: 26662 RVA: 0x00162F8C File Offset: 0x0016118C
		private int ReadFromBuffer(byte[] array, int offset, int count, out Exception error)
		{
			int result;
			try
			{
				error = null;
				result = this.ReadFromBuffer(array, offset, count);
			}
			catch (Exception ex)
			{
				error = ex;
				result = 0;
			}
			return result;
		}

		// Token: 0x06006827 RID: 26663 RVA: 0x00162FC4 File Offset: 0x001611C4
		public override int Read(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			this.EnsureNotClosed();
			this.EnsureCanRead();
			int num = this.ReadFromBuffer(array, offset, count);
			if (num == count)
			{
				return num;
			}
			int num2 = num;
			if (num > 0)
			{
				count -= num;
				offset += num;
			}
			this._readPos = (this._readLen = 0);
			if (this._writePos > 0)
			{
				this.FlushWrite();
			}
			if (count >= this._bufferSize)
			{
				return this._stream.Read(array, offset, count) + num2;
			}
			this.EnsureBufferAllocated();
			this._readLen = this._stream.Read(this._buffer, 0, this._bufferSize);
			num = this.ReadFromBuffer(array, offset, count);
			return num + num2;
		}

		// Token: 0x06006828 RID: 26664 RVA: 0x001630B8 File Offset: 0x001612B8
		public override int Read(Span<byte> destination)
		{
			this.EnsureNotClosed();
			this.EnsureCanRead();
			int num = this.ReadFromBuffer(destination);
			if (num == destination.Length)
			{
				return num;
			}
			if (num > 0)
			{
				destination = destination.Slice(num);
			}
			this._readPos = (this._readLen = 0);
			if (this._writePos > 0)
			{
				this.FlushWrite();
			}
			if (destination.Length >= this._bufferSize)
			{
				return this._stream.Read(destination) + num;
			}
			this.EnsureBufferAllocated();
			this._readLen = this._stream.Read(this._buffer, 0, this._bufferSize);
			return this.ReadFromBuffer(destination) + num;
		}

		// Token: 0x06006829 RID: 26665 RVA: 0x00163160 File Offset: 0x00161360
		private Task<int> LastSyncCompletedReadTask(int val)
		{
			Task<int> task = this._lastSyncCompletedReadTask;
			if (task != null && task.Result == val)
			{
				return task;
			}
			task = Task.FromResult<int>(val);
			this._lastSyncCompletedReadTask = task;
			return task;
		}

		// Token: 0x0600682A RID: 26666 RVA: 0x00163194 File Offset: 0x00161394
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
			this.EnsureNotClosed();
			this.EnsureCanRead();
			int num = 0;
			SemaphoreSlim semaphoreSlim = this.LazyEnsureAsyncActiveSemaphoreInitialized();
			Task task = semaphoreSlim.WaitAsync();
			if (task.IsCompletedSuccessfully)
			{
				bool flag = true;
				try
				{
					Exception ex;
					num = this.ReadFromBuffer(buffer, offset, count, out ex);
					flag = (num == count || ex != null);
					if (flag)
					{
						return (ex == null) ? this.LastSyncCompletedReadTask(num) : Task.FromException<int>(ex);
					}
				}
				finally
				{
					if (flag)
					{
						semaphoreSlim.Release();
					}
				}
			}
			return this.ReadFromUnderlyingStreamAsync(new Memory<byte>(buffer, offset + num, count - num), cancellationToken, num, task).AsTask();
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x0016329C File Offset: 0x0016149C
		public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
			}
			this.EnsureNotClosed();
			this.EnsureCanRead();
			int num = 0;
			SemaphoreSlim semaphoreSlim = this.LazyEnsureAsyncActiveSemaphoreInitialized();
			Task task = semaphoreSlim.WaitAsync();
			if (task.IsCompletedSuccessfully)
			{
				bool flag = true;
				try
				{
					num = this.ReadFromBuffer(buffer.Span);
					flag = (num == buffer.Length);
					if (flag)
					{
						return new ValueTask<int>(num);
					}
				}
				finally
				{
					if (flag)
					{
						semaphoreSlim.Release();
					}
				}
			}
			return this.ReadFromUnderlyingStreamAsync(buffer.Slice(num), cancellationToken, num, task);
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x0016333C File Offset: 0x0016153C
		private ValueTask<int> ReadFromUnderlyingStreamAsync(Memory<byte> buffer, CancellationToken cancellationToken, int bytesAlreadySatisfied, Task semaphoreLockTask)
		{
			BufferedStream.<ReadFromUnderlyingStreamAsync>d__51 <ReadFromUnderlyingStreamAsync>d__;
			<ReadFromUnderlyingStreamAsync>d__.<>4__this = this;
			<ReadFromUnderlyingStreamAsync>d__.buffer = buffer;
			<ReadFromUnderlyingStreamAsync>d__.cancellationToken = cancellationToken;
			<ReadFromUnderlyingStreamAsync>d__.bytesAlreadySatisfied = bytesAlreadySatisfied;
			<ReadFromUnderlyingStreamAsync>d__.semaphoreLockTask = semaphoreLockTask;
			<ReadFromUnderlyingStreamAsync>d__.<>t__builder = AsyncValueTaskMethodBuilder<int>.Create();
			<ReadFromUnderlyingStreamAsync>d__.<>1__state = -1;
			<ReadFromUnderlyingStreamAsync>d__.<>t__builder.Start<BufferedStream.<ReadFromUnderlyingStreamAsync>d__51>(ref <ReadFromUnderlyingStreamAsync>d__);
			return <ReadFromUnderlyingStreamAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600682D RID: 26669 RVA: 0x000A438E File Offset: 0x000A258E
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return TaskToApm.Begin(this.ReadAsync(buffer, offset, count, CancellationToken.None), callback, state);
		}

		// Token: 0x0600682E RID: 26670 RVA: 0x000A43A7 File Offset: 0x000A25A7
		public override int EndRead(IAsyncResult asyncResult)
		{
			return TaskToApm.End<int>(asyncResult);
		}

		// Token: 0x0600682F RID: 26671 RVA: 0x001633A0 File Offset: 0x001615A0
		public override int ReadByte()
		{
			if (this._readPos == this._readLen)
			{
				return this.ReadByteSlow();
			}
			byte[] buffer = this._buffer;
			int readPos = this._readPos;
			this._readPos = readPos + 1;
			return buffer[readPos];
		}

		// Token: 0x06006830 RID: 26672 RVA: 0x001633DC File Offset: 0x001615DC
		private int ReadByteSlow()
		{
			this.EnsureNotClosed();
			this.EnsureCanRead();
			if (this._writePos > 0)
			{
				this.FlushWrite();
			}
			this.EnsureBufferAllocated();
			this._readLen = this._stream.Read(this._buffer, 0, this._bufferSize);
			this._readPos = 0;
			if (this._readLen == 0)
			{
				return -1;
			}
			byte[] buffer = this._buffer;
			int readPos = this._readPos;
			this._readPos = readPos + 1;
			return buffer[readPos];
		}

		// Token: 0x06006831 RID: 26673 RVA: 0x00163454 File Offset: 0x00161654
		private void WriteToBuffer(byte[] array, ref int offset, ref int count)
		{
			int num = Math.Min(this._bufferSize - this._writePos, count);
			if (num <= 0)
			{
				return;
			}
			this.EnsureBufferAllocated();
			Buffer.BlockCopy(array, offset, this._buffer, this._writePos, num);
			this._writePos += num;
			count -= num;
			offset += num;
		}

		// Token: 0x06006832 RID: 26674 RVA: 0x001634B0 File Offset: 0x001616B0
		private int WriteToBuffer(ReadOnlySpan<byte> buffer)
		{
			int num = Math.Min(this._bufferSize - this._writePos, buffer.Length);
			if (num > 0)
			{
				this.EnsureBufferAllocated();
				buffer.Slice(0, num).CopyTo(new Span<byte>(this._buffer, this._writePos, num));
				this._writePos += num;
			}
			return num;
		}

		// Token: 0x06006833 RID: 26675 RVA: 0x00163514 File Offset: 0x00161714
		private void WriteToBuffer(byte[] array, ref int offset, ref int count, out Exception error)
		{
			try
			{
				error = null;
				this.WriteToBuffer(array, ref offset, ref count);
			}
			catch (Exception ex)
			{
				error = ex;
			}
		}

		// Token: 0x06006834 RID: 26676 RVA: 0x00163548 File Offset: 0x00161748
		public override void Write(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			this.EnsureNotClosed();
			this.EnsureCanWrite();
			if (this._writePos == 0)
			{
				this.ClearReadBufferBeforeWrite();
			}
			int num = checked(this._writePos + count);
			if (checked(num + count >= this._bufferSize + this._bufferSize))
			{
				if (this._writePos > 0)
				{
					if (num <= this._bufferSize + this._bufferSize && num <= 81920)
					{
						this.EnsureShadowBufferAllocated();
						Buffer.BlockCopy(array, offset, this._buffer, this._writePos, count);
						this._stream.Write(this._buffer, 0, num);
						this._writePos = 0;
						return;
					}
					this._stream.Write(this._buffer, 0, this._writePos);
					this._writePos = 0;
				}
				this._stream.Write(array, offset, count);
				return;
			}
			this.WriteToBuffer(array, ref offset, ref count);
			if (this._writePos < this._bufferSize)
			{
				return;
			}
			this._stream.Write(this._buffer, 0, this._writePos);
			this._writePos = 0;
			this.WriteToBuffer(array, ref offset, ref count);
		}

		// Token: 0x06006835 RID: 26677 RVA: 0x001636A4 File Offset: 0x001618A4
		public override void Write(ReadOnlySpan<byte> buffer)
		{
			this.EnsureNotClosed();
			this.EnsureCanWrite();
			if (this._writePos == 0)
			{
				this.ClearReadBufferBeforeWrite();
			}
			int num = checked(this._writePos + buffer.Length);
			if (checked(num + buffer.Length >= this._bufferSize + this._bufferSize))
			{
				if (this._writePos > 0)
				{
					if (num <= this._bufferSize + this._bufferSize && num <= 81920)
					{
						this.EnsureShadowBufferAllocated();
						buffer.CopyTo(new Span<byte>(this._buffer, this._writePos, buffer.Length));
						this._stream.Write(this._buffer, 0, num);
						this._writePos = 0;
						return;
					}
					this._stream.Write(this._buffer, 0, this._writePos);
					this._writePos = 0;
				}
				this._stream.Write(buffer);
				return;
			}
			int start = this.WriteToBuffer(buffer);
			if (this._writePos < this._bufferSize)
			{
				return;
			}
			buffer = buffer.Slice(start);
			this._stream.Write(this._buffer, 0, this._writePos);
			this._writePos = 0;
			start = this.WriteToBuffer(buffer);
		}

		// Token: 0x06006836 RID: 26678 RVA: 0x001637CC File Offset: 0x001619CC
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
			return this.WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).AsTask();
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x00163840 File Offset: 0x00161A40
		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask(Task.FromCanceled<int>(cancellationToken));
			}
			this.EnsureNotClosed();
			this.EnsureCanWrite();
			SemaphoreSlim semaphoreSlim = this.LazyEnsureAsyncActiveSemaphoreInitialized();
			Task task = semaphoreSlim.WaitAsync();
			if (task.IsCompletedSuccessfully)
			{
				bool flag = true;
				try
				{
					if (this._writePos == 0)
					{
						this.ClearReadBufferBeforeWrite();
					}
					flag = (buffer.Length < this._bufferSize - this._writePos);
					if (flag)
					{
						this.WriteToBuffer(buffer.Span);
						return default(ValueTask);
					}
				}
				finally
				{
					if (flag)
					{
						semaphoreSlim.Release();
					}
				}
			}
			return new ValueTask(this.WriteToUnderlyingStreamAsync(buffer, cancellationToken, task));
		}

		// Token: 0x06006838 RID: 26680 RVA: 0x001638F8 File Offset: 0x00161AF8
		private Task WriteToUnderlyingStreamAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken, Task semaphoreLockTask)
		{
			BufferedStream.<WriteToUnderlyingStreamAsync>d__63 <WriteToUnderlyingStreamAsync>d__;
			<WriteToUnderlyingStreamAsync>d__.<>4__this = this;
			<WriteToUnderlyingStreamAsync>d__.buffer = buffer;
			<WriteToUnderlyingStreamAsync>d__.cancellationToken = cancellationToken;
			<WriteToUnderlyingStreamAsync>d__.semaphoreLockTask = semaphoreLockTask;
			<WriteToUnderlyingStreamAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteToUnderlyingStreamAsync>d__.<>1__state = -1;
			<WriteToUnderlyingStreamAsync>d__.<>t__builder.Start<BufferedStream.<WriteToUnderlyingStreamAsync>d__63>(ref <WriteToUnderlyingStreamAsync>d__);
			return <WriteToUnderlyingStreamAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06006839 RID: 26681 RVA: 0x000A45BB File Offset: 0x000A27BB
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return TaskToApm.Begin(this.WriteAsync(buffer, offset, count, CancellationToken.None), callback, state);
		}

		// Token: 0x0600683A RID: 26682 RVA: 0x000A45D4 File Offset: 0x000A27D4
		public override void EndWrite(IAsyncResult asyncResult)
		{
			TaskToApm.End(asyncResult);
		}

		// Token: 0x0600683B RID: 26683 RVA: 0x00163954 File Offset: 0x00161B54
		public override void WriteByte(byte value)
		{
			this.EnsureNotClosed();
			if (this._writePos == 0)
			{
				this.EnsureCanWrite();
				this.ClearReadBufferBeforeWrite();
				this.EnsureBufferAllocated();
			}
			if (this._writePos >= this._bufferSize - 1)
			{
				this.FlushWrite();
			}
			byte[] buffer = this._buffer;
			int writePos = this._writePos;
			this._writePos = writePos + 1;
			buffer[writePos] = value;
		}

		// Token: 0x0600683C RID: 26684 RVA: 0x001639B0 File Offset: 0x00161BB0
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.EnsureNotClosed();
			this.EnsureCanSeek();
			if (this._writePos > 0)
			{
				this.FlushWrite();
				return this._stream.Seek(offset, origin);
			}
			if (this._readLen - this._readPos > 0 && origin == SeekOrigin.Current)
			{
				offset -= (long)(this._readLen - this._readPos);
			}
			long position = this.Position;
			long num = this._stream.Seek(offset, origin);
			this._readPos = (int)(num - (position - (long)this._readPos));
			if (0 <= this._readPos && this._readPos < this._readLen)
			{
				this._stream.Seek((long)(this._readLen - this._readPos), SeekOrigin.Current);
			}
			else
			{
				this._readPos = (this._readLen = 0);
			}
			return num;
		}

		// Token: 0x0600683D RID: 26685 RVA: 0x00163A78 File Offset: 0x00161C78
		public override void SetLength(long value)
		{
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("value", "Non-negative number required.");
			}
			this.EnsureNotClosed();
			this.EnsureCanSeek();
			this.EnsureCanWrite();
			this.Flush();
			this._stream.SetLength(value);
		}

		// Token: 0x0600683E RID: 26686 RVA: 0x00163AB4 File Offset: 0x00161CB4
		public override void CopyTo(Stream destination, int bufferSize)
		{
			StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
			int num = this._readLen - this._readPos;
			if (num > 0)
			{
				destination.Write(this._buffer, this._readPos, num);
				this._readPos = (this._readLen = 0);
			}
			else if (this._writePos > 0)
			{
				this.FlushWrite();
			}
			this._stream.CopyTo(destination, bufferSize);
		}

		// Token: 0x0600683F RID: 26687 RVA: 0x00163B1C File Offset: 0x00161D1C
		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
			if (!cancellationToken.IsCancellationRequested)
			{
				return this.CopyToAsyncCore(destination, bufferSize, cancellationToken);
			}
			return Task.FromCanceled<int>(cancellationToken);
		}

		// Token: 0x06006840 RID: 26688 RVA: 0x00163B40 File Offset: 0x00161D40
		private Task CopyToAsyncCore(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			BufferedStream.<CopyToAsyncCore>d__71 <CopyToAsyncCore>d__;
			<CopyToAsyncCore>d__.<>4__this = this;
			<CopyToAsyncCore>d__.destination = destination;
			<CopyToAsyncCore>d__.bufferSize = bufferSize;
			<CopyToAsyncCore>d__.cancellationToken = cancellationToken;
			<CopyToAsyncCore>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CopyToAsyncCore>d__.<>1__state = -1;
			<CopyToAsyncCore>d__.<>t__builder.Start<BufferedStream.<CopyToAsyncCore>d__71>(ref <CopyToAsyncCore>d__);
			return <CopyToAsyncCore>d__.<>t__builder.Task;
		}

		// Token: 0x04003C87 RID: 15495
		private const int MaxShadowBufferSize = 81920;

		// Token: 0x04003C88 RID: 15496
		private const int DefaultBufferSize = 4096;

		// Token: 0x04003C89 RID: 15497
		private Stream _stream;

		// Token: 0x04003C8A RID: 15498
		private byte[] _buffer;

		// Token: 0x04003C8B RID: 15499
		private readonly int _bufferSize;

		// Token: 0x04003C8C RID: 15500
		private int _readPos;

		// Token: 0x04003C8D RID: 15501
		private int _readLen;

		// Token: 0x04003C8E RID: 15502
		private int _writePos;

		// Token: 0x04003C8F RID: 15503
		private Task<int> _lastSyncCompletedReadTask;

		// Token: 0x04003C90 RID: 15504
		private SemaphoreSlim _asyncActiveSemaphore;
	}
}
