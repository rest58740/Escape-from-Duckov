using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	// Token: 0x02000B18 RID: 2840
	[Serializable]
	public class StreamWriter : TextWriter
	{
		// Token: 0x0600657A RID: 25978 RVA: 0x0015A576 File Offset: 0x00158776
		private void CheckAsyncTaskInProgress()
		{
			if (!this._asyncWriteTask.IsCompleted)
			{
				StreamWriter.ThrowAsyncIOInProgress();
			}
		}

		// Token: 0x0600657B RID: 25979 RVA: 0x0015866B File Offset: 0x0015686B
		private static void ThrowAsyncIOInProgress()
		{
			throw new InvalidOperationException("The stream is currently in use by a previous operation on the stream.");
		}

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x0600657C RID: 25980 RVA: 0x0015A58A File Offset: 0x0015878A
		private static Encoding UTF8NoBOM
		{
			get
			{
				return EncodingHelper.UTF8Unmarked;
			}
		}

		// Token: 0x0600657D RID: 25981 RVA: 0x0015A591 File Offset: 0x00158791
		internal StreamWriter() : base(null)
		{
		}

		// Token: 0x0600657E RID: 25982 RVA: 0x0015A5A5 File Offset: 0x001587A5
		public StreamWriter(Stream stream) : this(stream, StreamWriter.UTF8NoBOM, 1024, false)
		{
		}

		// Token: 0x0600657F RID: 25983 RVA: 0x0015A5B9 File Offset: 0x001587B9
		public StreamWriter(Stream stream, Encoding encoding) : this(stream, encoding, 1024, false)
		{
		}

		// Token: 0x06006580 RID: 25984 RVA: 0x0015A5C9 File Offset: 0x001587C9
		public StreamWriter(Stream stream, Encoding encoding, int bufferSize) : this(stream, encoding, bufferSize, false)
		{
		}

		// Token: 0x06006581 RID: 25985 RVA: 0x0015A5D8 File Offset: 0x001587D8
		public StreamWriter(Stream stream, Encoding encoding, int bufferSize, bool leaveOpen) : base(null)
		{
			if (stream == null || encoding == null)
			{
				throw new ArgumentNullException((stream == null) ? "stream" : "encoding");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException("Stream was not writable.");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "Positive number required.");
			}
			this.Init(stream, encoding, bufferSize, leaveOpen);
		}

		// Token: 0x06006582 RID: 25986 RVA: 0x0015A644 File Offset: 0x00158844
		public StreamWriter(string path) : this(path, false, StreamWriter.UTF8NoBOM, 1024)
		{
		}

		// Token: 0x06006583 RID: 25987 RVA: 0x0015A658 File Offset: 0x00158858
		public StreamWriter(string path, bool append) : this(path, append, StreamWriter.UTF8NoBOM, 1024)
		{
		}

		// Token: 0x06006584 RID: 25988 RVA: 0x0015A66C File Offset: 0x0015886C
		public StreamWriter(string path, bool append, Encoding encoding) : this(path, append, encoding, 1024)
		{
		}

		// Token: 0x06006585 RID: 25989 RVA: 0x0015A67C File Offset: 0x0015887C
		public StreamWriter(string path, bool append, Encoding encoding, int bufferSize)
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
				throw new ArgumentException("Empty path name is not legal.");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "Positive number required.");
			}
			Stream streamArg = new FileStream(path, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read, 4096, FileOptions.SequentialScan);
			this.Init(streamArg, encoding, bufferSize, false);
		}

		// Token: 0x06006586 RID: 25990 RVA: 0x0015A704 File Offset: 0x00158904
		private void Init(Stream streamArg, Encoding encodingArg, int bufferSize, bool shouldLeaveOpen)
		{
			this._stream = streamArg;
			this._encoding = encodingArg;
			this._encoder = this._encoding.GetEncoder();
			if (bufferSize < 128)
			{
				bufferSize = 128;
			}
			this._charBuffer = new char[bufferSize];
			this._byteBuffer = new byte[this._encoding.GetMaxByteCount(bufferSize)];
			this._charLen = bufferSize;
			if (this._stream.CanSeek && this._stream.Position > 0L)
			{
				this._haveWrittenPreamble = true;
			}
			this._closable = !shouldLeaveOpen;
		}

		// Token: 0x06006587 RID: 25991 RVA: 0x0015A797 File Offset: 0x00158997
		public override void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06006588 RID: 25992 RVA: 0x0015A7A8 File Offset: 0x001589A8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this._stream != null && disposing)
				{
					this.CheckAsyncTaskInProgress();
					this.Flush(true, true);
				}
			}
			finally
			{
				if (!this.LeaveOpen && this._stream != null)
				{
					try
					{
						if (disposing)
						{
							this._stream.Close();
						}
					}
					finally
					{
						this._stream = null;
						this._byteBuffer = null;
						this._charBuffer = null;
						this._encoding = null;
						this._encoder = null;
						this._charLen = 0;
						base.Dispose(disposing);
					}
				}
			}
		}

		// Token: 0x06006589 RID: 25993 RVA: 0x0015A840 File Offset: 0x00158A40
		public override ValueTask DisposeAsync()
		{
			if (!(base.GetType() != typeof(StreamWriter)))
			{
				return this.DisposeAsyncCore();
			}
			return base.DisposeAsync();
		}

		// Token: 0x0600658A RID: 25994 RVA: 0x0015A868 File Offset: 0x00158A68
		private ValueTask DisposeAsyncCore()
		{
			StreamWriter.<DisposeAsyncCore>d__33 <DisposeAsyncCore>d__;
			<DisposeAsyncCore>d__.<>4__this = this;
			<DisposeAsyncCore>d__.<>t__builder = AsyncValueTaskMethodBuilder.Create();
			<DisposeAsyncCore>d__.<>1__state = -1;
			<DisposeAsyncCore>d__.<>t__builder.Start<StreamWriter.<DisposeAsyncCore>d__33>(ref <DisposeAsyncCore>d__);
			return <DisposeAsyncCore>d__.<>t__builder.Task;
		}

		// Token: 0x0600658B RID: 25995 RVA: 0x0015A8AC File Offset: 0x00158AAC
		private void CloseStreamFromDispose(bool disposing)
		{
			if (!this.LeaveOpen && this._stream != null)
			{
				try
				{
					if (disposing)
					{
						this._stream.Close();
					}
				}
				finally
				{
					this._stream = null;
					this._byteBuffer = null;
					this._charBuffer = null;
					this._encoding = null;
					this._encoder = null;
					this._charLen = 0;
					base.Dispose(disposing);
				}
			}
		}

		// Token: 0x0600658C RID: 25996 RVA: 0x0015A91C File Offset: 0x00158B1C
		public override void Flush()
		{
			this.CheckAsyncTaskInProgress();
			this.Flush(true, true);
		}

		// Token: 0x0600658D RID: 25997 RVA: 0x0015A92C File Offset: 0x00158B2C
		private void Flush(bool flushStream, bool flushEncoder)
		{
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			if (this._charPos == 0 && !flushStream && !flushEncoder)
			{
				return;
			}
			if (!this._haveWrittenPreamble)
			{
				this._haveWrittenPreamble = true;
				ReadOnlySpan<byte> preamble = this._encoding.Preamble;
				if (preamble.Length > 0)
				{
					this._stream.Write(preamble);
				}
			}
			int bytes = this._encoder.GetBytes(this._charBuffer, 0, this._charPos, this._byteBuffer, 0, flushEncoder);
			this._charPos = 0;
			if (bytes > 0)
			{
				this._stream.Write(this._byteBuffer, 0, bytes);
			}
			if (flushStream)
			{
				this._stream.Flush();
			}
		}

		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x0600658E RID: 25998 RVA: 0x0015A9DA File Offset: 0x00158BDA
		// (set) Token: 0x0600658F RID: 25999 RVA: 0x0015A9E2 File Offset: 0x00158BE2
		public virtual bool AutoFlush
		{
			get
			{
				return this._autoFlush;
			}
			set
			{
				this.CheckAsyncTaskInProgress();
				this._autoFlush = value;
				if (value)
				{
					this.Flush(true, false);
				}
			}
		}

		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x06006590 RID: 26000 RVA: 0x0015A9FC File Offset: 0x00158BFC
		public virtual Stream BaseStream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x06006591 RID: 26001 RVA: 0x0015AA04 File Offset: 0x00158C04
		internal bool LeaveOpen
		{
			get
			{
				return !this._closable;
			}
		}

		// Token: 0x170011C3 RID: 4547
		// (set) Token: 0x06006592 RID: 26002 RVA: 0x0015AA0F File Offset: 0x00158C0F
		internal bool HaveWrittenPreamble
		{
			set
			{
				this._haveWrittenPreamble = value;
			}
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x06006593 RID: 26003 RVA: 0x0015AA18 File Offset: 0x00158C18
		public override Encoding Encoding
		{
			get
			{
				return this._encoding;
			}
		}

		// Token: 0x06006594 RID: 26004 RVA: 0x0015AA20 File Offset: 0x00158C20
		public override void Write(char value)
		{
			this.CheckAsyncTaskInProgress();
			if (this._charPos == this._charLen)
			{
				this.Flush(false, false);
			}
			this._charBuffer[this._charPos] = value;
			this._charPos++;
			if (this._autoFlush)
			{
				this.Flush(true, false);
			}
		}

		// Token: 0x06006595 RID: 26005 RVA: 0x0015AA75 File Offset: 0x00158C75
		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void Write(char[] buffer)
		{
			this.WriteSpan(buffer, false);
		}

		// Token: 0x06006596 RID: 26006 RVA: 0x0015AA84 File Offset: 0x00158C84
		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void Write(char[] buffer, int index, int count)
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
			this.WriteSpan(buffer.AsSpan(index, count), false);
		}

		// Token: 0x06006597 RID: 26007 RVA: 0x0015AAF3 File Offset: 0x00158CF3
		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void Write(ReadOnlySpan<char> buffer)
		{
			if (base.GetType() == typeof(StreamWriter))
			{
				this.WriteSpan(buffer, false);
				return;
			}
			base.Write(buffer);
		}

		// Token: 0x06006598 RID: 26008 RVA: 0x0015AB1C File Offset: 0x00158D1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe void WriteSpan(ReadOnlySpan<char> buffer, bool appendNewLine)
		{
			this.CheckAsyncTaskInProgress();
			if (buffer.Length <= 4 && buffer.Length <= this._charLen - this._charPos)
			{
				for (int i = 0; i < buffer.Length; i++)
				{
					char[] charBuffer = this._charBuffer;
					int charPos = this._charPos;
					this._charPos = charPos + 1;
					charBuffer[charPos] = *buffer[i];
				}
			}
			else
			{
				char[] charBuffer2 = this._charBuffer;
				if (charBuffer2 == null)
				{
					throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
				}
				fixed (char* reference = MemoryMarshal.GetReference<char>(buffer))
				{
					char* ptr = reference;
					fixed (char* ptr2 = &charBuffer2[0])
					{
						char* ptr3 = ptr2;
						char* ptr4 = ptr;
						int j = buffer.Length;
						int num = this._charPos;
						while (j > 0)
						{
							if (num == charBuffer2.Length)
							{
								this.Flush(false, false);
								num = 0;
							}
							int num2 = Math.Min(charBuffer2.Length - num, j);
							int num3 = num2 * 2;
							Buffer.MemoryCopy((void*)ptr4, (void*)(ptr3 + num), (long)num3, (long)num3);
							this._charPos += num2;
							num += num2;
							ptr4 += num2;
							j -= num2;
						}
					}
				}
			}
			if (appendNewLine)
			{
				char[] coreNewLine = this.CoreNewLine;
				for (int k = 0; k < coreNewLine.Length; k++)
				{
					if (this._charPos == this._charLen)
					{
						this.Flush(false, false);
					}
					this._charBuffer[this._charPos] = coreNewLine[k];
					this._charPos++;
				}
			}
			if (this._autoFlush)
			{
				this.Flush(true, false);
			}
		}

		// Token: 0x06006599 RID: 26009 RVA: 0x0015AC9C File Offset: 0x00158E9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void Write(string value)
		{
			this.WriteSpan(value, false);
		}

		// Token: 0x0600659A RID: 26010 RVA: 0x0015ACAB File Offset: 0x00158EAB
		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void WriteLine(string value)
		{
			this.CheckAsyncTaskInProgress();
			this.WriteSpan(value, true);
		}

		// Token: 0x0600659B RID: 26011 RVA: 0x0015ACC0 File Offset: 0x00158EC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void WriteLine(ReadOnlySpan<char> value)
		{
			if (base.GetType() == typeof(StreamWriter))
			{
				this.CheckAsyncTaskInProgress();
				this.WriteSpan(value, true);
				return;
			}
			base.WriteLine(value);
		}

		// Token: 0x0600659C RID: 26012 RVA: 0x0015ACF0 File Offset: 0x00158EF0
		public override Task WriteAsync(char value)
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteAsync(value);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, value, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, false);
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x0600659D RID: 26013 RVA: 0x0015AD68 File Offset: 0x00158F68
		private static Task WriteAsyncInternal(StreamWriter _this, char value, char[] charBuffer, int charPos, int charLen, char[] coreNewLine, bool autoFlush, bool appendNewLine)
		{
			StreamWriter.<WriteAsyncInternal>d__57 <WriteAsyncInternal>d__;
			<WriteAsyncInternal>d__._this = _this;
			<WriteAsyncInternal>d__.value = value;
			<WriteAsyncInternal>d__.charBuffer = charBuffer;
			<WriteAsyncInternal>d__.charPos = charPos;
			<WriteAsyncInternal>d__.charLen = charLen;
			<WriteAsyncInternal>d__.coreNewLine = coreNewLine;
			<WriteAsyncInternal>d__.autoFlush = autoFlush;
			<WriteAsyncInternal>d__.appendNewLine = appendNewLine;
			<WriteAsyncInternal>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteAsyncInternal>d__.<>1__state = -1;
			<WriteAsyncInternal>d__.<>t__builder.Start<StreamWriter.<WriteAsyncInternal>d__57>(ref <WriteAsyncInternal>d__);
			return <WriteAsyncInternal>d__.<>t__builder.Task;
		}

		// Token: 0x0600659E RID: 26014 RVA: 0x0015ADE8 File Offset: 0x00158FE8
		public override Task WriteAsync(string value)
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteAsync(value);
			}
			if (value == null)
			{
				return Task.CompletedTask;
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, value, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, false);
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x0600659F RID: 26015 RVA: 0x0015AE68 File Offset: 0x00159068
		private static Task WriteAsyncInternal(StreamWriter _this, string value, char[] charBuffer, int charPos, int charLen, char[] coreNewLine, bool autoFlush, bool appendNewLine)
		{
			StreamWriter.<WriteAsyncInternal>d__59 <WriteAsyncInternal>d__;
			<WriteAsyncInternal>d__._this = _this;
			<WriteAsyncInternal>d__.value = value;
			<WriteAsyncInternal>d__.charBuffer = charBuffer;
			<WriteAsyncInternal>d__.charPos = charPos;
			<WriteAsyncInternal>d__.charLen = charLen;
			<WriteAsyncInternal>d__.coreNewLine = coreNewLine;
			<WriteAsyncInternal>d__.autoFlush = autoFlush;
			<WriteAsyncInternal>d__.appendNewLine = appendNewLine;
			<WriteAsyncInternal>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteAsyncInternal>d__.<>1__state = -1;
			<WriteAsyncInternal>d__.<>t__builder.Start<StreamWriter.<WriteAsyncInternal>d__59>(ref <WriteAsyncInternal>d__);
			return <WriteAsyncInternal>d__.<>t__builder.Task;
		}

		// Token: 0x060065A0 RID: 26016 RVA: 0x0015AEE8 File Offset: 0x001590E8
		public override Task WriteAsync(char[] buffer, int index, int count)
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
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteAsync(buffer, index, count);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, new ReadOnlyMemory<char>(buffer, index, count), this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, false, default(CancellationToken));
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x060065A1 RID: 26017 RVA: 0x0015AFC0 File Offset: 0x001591C0
		public override Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteAsync(buffer, cancellationToken);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			Task task = StreamWriter.WriteAsyncInternal(this, buffer, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, false, cancellationToken);
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x060065A2 RID: 26018 RVA: 0x0015B048 File Offset: 0x00159248
		private static Task WriteAsyncInternal(StreamWriter _this, ReadOnlyMemory<char> source, char[] charBuffer, int charPos, int charLen, char[] coreNewLine, bool autoFlush, bool appendNewLine, CancellationToken cancellationToken)
		{
			StreamWriter.<WriteAsyncInternal>d__62 <WriteAsyncInternal>d__;
			<WriteAsyncInternal>d__._this = _this;
			<WriteAsyncInternal>d__.source = source;
			<WriteAsyncInternal>d__.charBuffer = charBuffer;
			<WriteAsyncInternal>d__.charPos = charPos;
			<WriteAsyncInternal>d__.charLen = charLen;
			<WriteAsyncInternal>d__.coreNewLine = coreNewLine;
			<WriteAsyncInternal>d__.autoFlush = autoFlush;
			<WriteAsyncInternal>d__.appendNewLine = appendNewLine;
			<WriteAsyncInternal>d__.cancellationToken = cancellationToken;
			<WriteAsyncInternal>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteAsyncInternal>d__.<>1__state = -1;
			<WriteAsyncInternal>d__.<>t__builder.Start<StreamWriter.<WriteAsyncInternal>d__62>(ref <WriteAsyncInternal>d__);
			return <WriteAsyncInternal>d__.<>t__builder.Task;
		}

		// Token: 0x060065A3 RID: 26019 RVA: 0x0015B0D0 File Offset: 0x001592D0
		public override Task WriteLineAsync()
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteLineAsync();
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, ReadOnlyMemory<char>.Empty, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, true, default(CancellationToken));
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x060065A4 RID: 26020 RVA: 0x0015B154 File Offset: 0x00159354
		public override Task WriteLineAsync(char value)
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteLineAsync(value);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, value, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, true);
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x060065A5 RID: 26021 RVA: 0x0015B1CC File Offset: 0x001593CC
		public override Task WriteLineAsync(string value)
		{
			if (value == null)
			{
				return this.WriteLineAsync();
			}
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteLineAsync(value);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, value, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, true);
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x060065A6 RID: 26022 RVA: 0x0015B24C File Offset: 0x0015944C
		public override Task WriteLineAsync(char[] buffer, int index, int count)
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
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteLineAsync(buffer, index, count);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, new ReadOnlyMemory<char>(buffer, index, count), this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, true, default(CancellationToken));
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x060065A7 RID: 26023 RVA: 0x0015B324 File Offset: 0x00159524
		public override Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteLineAsync(buffer, cancellationToken);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			Task task = StreamWriter.WriteAsyncInternal(this, buffer, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, true, cancellationToken);
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x060065A8 RID: 26024 RVA: 0x0015B3AC File Offset: 0x001595AC
		public override Task FlushAsync()
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.FlushAsync();
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = this.FlushAsyncInternal(true, true, this._charBuffer, this._charPos, default(CancellationToken));
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x170011C5 RID: 4549
		// (set) Token: 0x060065A9 RID: 26025 RVA: 0x0015B417 File Offset: 0x00159617
		private int CharPos_Prop
		{
			set
			{
				this._charPos = value;
			}
		}

		// Token: 0x170011C6 RID: 4550
		// (set) Token: 0x060065AA RID: 26026 RVA: 0x0015AA0F File Offset: 0x00158C0F
		private bool HaveWrittenPreamble_Prop
		{
			set
			{
				this._haveWrittenPreamble = value;
			}
		}

		// Token: 0x060065AB RID: 26027 RVA: 0x0015B420 File Offset: 0x00159620
		private Task FlushAsyncInternal(bool flushStream, bool flushEncoder, char[] sCharBuffer, int sCharPos, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			if (sCharPos == 0 && !flushStream && !flushEncoder)
			{
				return Task.CompletedTask;
			}
			Task result = StreamWriter.FlushAsyncInternal(this, flushStream, flushEncoder, sCharBuffer, sCharPos, this._haveWrittenPreamble, this._encoding, this._encoder, this._byteBuffer, this._stream, cancellationToken);
			this._charPos = 0;
			return result;
		}

		// Token: 0x060065AC RID: 26028 RVA: 0x0015B480 File Offset: 0x00159680
		private static Task FlushAsyncInternal(StreamWriter _this, bool flushStream, bool flushEncoder, char[] charBuffer, int charPos, bool haveWrittenPreamble, Encoding encoding, Encoder encoder, byte[] byteBuffer, Stream stream, CancellationToken cancellationToken)
		{
			StreamWriter.<FlushAsyncInternal>d__74 <FlushAsyncInternal>d__;
			<FlushAsyncInternal>d__._this = _this;
			<FlushAsyncInternal>d__.flushStream = flushStream;
			<FlushAsyncInternal>d__.flushEncoder = flushEncoder;
			<FlushAsyncInternal>d__.charBuffer = charBuffer;
			<FlushAsyncInternal>d__.charPos = charPos;
			<FlushAsyncInternal>d__.haveWrittenPreamble = haveWrittenPreamble;
			<FlushAsyncInternal>d__.encoding = encoding;
			<FlushAsyncInternal>d__.encoder = encoder;
			<FlushAsyncInternal>d__.byteBuffer = byteBuffer;
			<FlushAsyncInternal>d__.stream = stream;
			<FlushAsyncInternal>d__.cancellationToken = cancellationToken;
			<FlushAsyncInternal>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<FlushAsyncInternal>d__.<>1__state = -1;
			<FlushAsyncInternal>d__.<>t__builder.Start<StreamWriter.<FlushAsyncInternal>d__74>(ref <FlushAsyncInternal>d__);
			return <FlushAsyncInternal>d__.<>t__builder.Task;
		}

		// Token: 0x04003B9E RID: 15262
		internal const int DefaultBufferSize = 1024;

		// Token: 0x04003B9F RID: 15263
		private const int DefaultFileStreamBufferSize = 4096;

		// Token: 0x04003BA0 RID: 15264
		private const int MinBufferSize = 128;

		// Token: 0x04003BA1 RID: 15265
		private const int DontCopyOnWriteLineThreshold = 512;

		// Token: 0x04003BA2 RID: 15266
		public new static readonly StreamWriter Null = new StreamWriter(Stream.Null, StreamWriter.UTF8NoBOM, 128, true);

		// Token: 0x04003BA3 RID: 15267
		private Stream _stream;

		// Token: 0x04003BA4 RID: 15268
		private Encoding _encoding;

		// Token: 0x04003BA5 RID: 15269
		private Encoder _encoder;

		// Token: 0x04003BA6 RID: 15270
		private byte[] _byteBuffer;

		// Token: 0x04003BA7 RID: 15271
		private char[] _charBuffer;

		// Token: 0x04003BA8 RID: 15272
		private int _charPos;

		// Token: 0x04003BA9 RID: 15273
		private int _charLen;

		// Token: 0x04003BAA RID: 15274
		private bool _autoFlush;

		// Token: 0x04003BAB RID: 15275
		private bool _haveWrittenPreamble;

		// Token: 0x04003BAC RID: 15276
		private bool _closable;

		// Token: 0x04003BAD RID: 15277
		private Task _asyncWriteTask = Task.CompletedTask;
	}
}
