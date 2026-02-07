using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	// Token: 0x02000B1E RID: 2846
	[Serializable]
	public abstract class TextReader : MarshalByRefObject, IDisposable
	{
		// Token: 0x060065B9 RID: 26041 RVA: 0x0015C1EA File Offset: 0x0015A3EA
		public virtual void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060065BA RID: 26042 RVA: 0x0015C1EA File Offset: 0x0015A3EA
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060065BB RID: 26043 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x060065BC RID: 26044 RVA: 0x0012276A File Offset: 0x0012096A
		public virtual int Peek()
		{
			return -1;
		}

		// Token: 0x060065BD RID: 26045 RVA: 0x0012276A File Offset: 0x0012096A
		public virtual int Read()
		{
			return -1;
		}

		// Token: 0x060065BE RID: 26046 RVA: 0x0015C1FC File Offset: 0x0015A3FC
		public virtual int Read(char[] buffer, int index, int count)
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
			int i;
			for (i = 0; i < count; i++)
			{
				int num = this.Read();
				if (num == -1)
				{
					break;
				}
				buffer[index + i] = (char)num;
			}
			return i;
		}

		// Token: 0x060065BF RID: 26047 RVA: 0x0015C278 File Offset: 0x0015A478
		public virtual int Read(Span<char> buffer)
		{
			char[] array = ArrayPool<char>.Shared.Rent(buffer.Length);
			int result;
			try
			{
				int num = this.Read(array, 0, buffer.Length);
				if ((ulong)num > (ulong)((long)buffer.Length))
				{
					throw new IOException("The read operation returned an invalid length.");
				}
				new Span<char>(array, 0, num).CopyTo(buffer);
				result = num;
			}
			finally
			{
				ArrayPool<char>.Shared.Return(array, false);
			}
			return result;
		}

		// Token: 0x060065C0 RID: 26048 RVA: 0x0015C2F4 File Offset: 0x0015A4F4
		public virtual string ReadToEnd()
		{
			char[] array = new char[4096];
			StringBuilder stringBuilder = new StringBuilder(4096);
			int charCount;
			while ((charCount = this.Read(array, 0, array.Length)) != 0)
			{
				stringBuilder.Append(array, 0, charCount);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060065C1 RID: 26049 RVA: 0x0015C338 File Offset: 0x0015A538
		public virtual int ReadBlock(char[] buffer, int index, int count)
		{
			int num = 0;
			int num2;
			do
			{
				num += (num2 = this.Read(buffer, index + num, count - num));
			}
			while (num2 > 0 && num < count);
			return num;
		}

		// Token: 0x060065C2 RID: 26050 RVA: 0x0015C364 File Offset: 0x0015A564
		public virtual int ReadBlock(Span<char> buffer)
		{
			char[] array = ArrayPool<char>.Shared.Rent(buffer.Length);
			int result;
			try
			{
				int num = this.ReadBlock(array, 0, buffer.Length);
				if ((ulong)num > (ulong)((long)buffer.Length))
				{
					throw new IOException("The read operation returned an invalid length.");
				}
				new Span<char>(array, 0, num).CopyTo(buffer);
				result = num;
			}
			finally
			{
				ArrayPool<char>.Shared.Return(array, false);
			}
			return result;
		}

		// Token: 0x060065C3 RID: 26051 RVA: 0x0015C3E0 File Offset: 0x0015A5E0
		public virtual string ReadLine()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num;
			for (;;)
			{
				num = this.Read();
				if (num == -1)
				{
					goto IL_43;
				}
				if (num == 13 || num == 10)
				{
					break;
				}
				stringBuilder.Append((char)num);
			}
			if (num == 13 && this.Peek() == 10)
			{
				this.Read();
			}
			return stringBuilder.ToString();
			IL_43:
			if (stringBuilder.Length > 0)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x060065C4 RID: 26052 RVA: 0x0015C441 File Offset: 0x0015A641
		public virtual Task<string> ReadLineAsync()
		{
			return Task<string>.Factory.StartNew((object state) => ((TextReader)state).ReadLine(), this, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		// Token: 0x060065C5 RID: 26053 RVA: 0x0015C478 File Offset: 0x0015A678
		public virtual Task<string> ReadToEndAsync()
		{
			TextReader.<ReadToEndAsync>d__14 <ReadToEndAsync>d__;
			<ReadToEndAsync>d__.<>4__this = this;
			<ReadToEndAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<ReadToEndAsync>d__.<>1__state = -1;
			<ReadToEndAsync>d__.<>t__builder.Start<TextReader.<ReadToEndAsync>d__14>(ref <ReadToEndAsync>d__);
			return <ReadToEndAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060065C6 RID: 26054 RVA: 0x0015C4BC File Offset: 0x0015A6BC
		public virtual Task<int> ReadAsync(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Non-negative number required.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			return this.ReadAsyncInternal(new Memory<char>(buffer, index, count), default(CancellationToken)).AsTask();
		}

		// Token: 0x060065C7 RID: 26055 RVA: 0x0015C534 File Offset: 0x0015A734
		public virtual ValueTask<int> ReadAsync(Memory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			ArraySegment<char> arraySegment;
			Task<int> task;
			if (!MemoryMarshal.TryGetArray<char>(buffer, out arraySegment))
			{
				task = Task<int>.Factory.StartNew(delegate(object state)
				{
					Tuple<TextReader, Memory<char>> tuple = (Tuple<TextReader, Memory<char>>)state;
					return tuple.Item1.Read(tuple.Item2.Span);
				}, Tuple.Create<TextReader, Memory<char>>(this, buffer), cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
			}
			else
			{
				task = this.ReadAsync(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
			}
			return new ValueTask<int>(task);
		}

		// Token: 0x060065C8 RID: 26056 RVA: 0x0015C5AC File Offset: 0x0015A7AC
		internal virtual ValueTask<int> ReadAsyncInternal(Memory<char> buffer, CancellationToken cancellationToken)
		{
			Tuple<TextReader, Memory<char>> state2 = new Tuple<TextReader, Memory<char>>(this, buffer);
			return new ValueTask<int>(Task<int>.Factory.StartNew(delegate(object state)
			{
				Tuple<TextReader, Memory<char>> tuple = (Tuple<TextReader, Memory<char>>)state;
				return tuple.Item1.Read(tuple.Item2.Span);
			}, state2, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default));
		}

		// Token: 0x060065C9 RID: 26057 RVA: 0x0015C5F8 File Offset: 0x0015A7F8
		public virtual Task<int> ReadBlockAsync(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Non-negative number required.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			return this.ReadBlockAsyncInternal(new Memory<char>(buffer, index, count), default(CancellationToken)).AsTask();
		}

		// Token: 0x060065CA RID: 26058 RVA: 0x0015C670 File Offset: 0x0015A870
		public virtual ValueTask<int> ReadBlockAsync(Memory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			ArraySegment<char> arraySegment;
			Task<int> task;
			if (!MemoryMarshal.TryGetArray<char>(buffer, out arraySegment))
			{
				task = Task<int>.Factory.StartNew(delegate(object state)
				{
					Tuple<TextReader, Memory<char>> tuple = (Tuple<TextReader, Memory<char>>)state;
					return tuple.Item1.ReadBlock(tuple.Item2.Span);
				}, Tuple.Create<TextReader, Memory<char>>(this, buffer), cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
			}
			else
			{
				task = this.ReadBlockAsync(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
			}
			return new ValueTask<int>(task);
		}

		// Token: 0x060065CB RID: 26059 RVA: 0x0015C6E8 File Offset: 0x0015A8E8
		internal ValueTask<int> ReadBlockAsyncInternal(Memory<char> buffer, CancellationToken cancellationToken)
		{
			TextReader.<ReadBlockAsyncInternal>d__20 <ReadBlockAsyncInternal>d__;
			<ReadBlockAsyncInternal>d__.<>4__this = this;
			<ReadBlockAsyncInternal>d__.buffer = buffer;
			<ReadBlockAsyncInternal>d__.cancellationToken = cancellationToken;
			<ReadBlockAsyncInternal>d__.<>t__builder = AsyncValueTaskMethodBuilder<int>.Create();
			<ReadBlockAsyncInternal>d__.<>1__state = -1;
			<ReadBlockAsyncInternal>d__.<>t__builder.Start<TextReader.<ReadBlockAsyncInternal>d__20>(ref <ReadBlockAsyncInternal>d__);
			return <ReadBlockAsyncInternal>d__.<>t__builder.Task;
		}

		// Token: 0x060065CC RID: 26060 RVA: 0x0015C73B File Offset: 0x0015A93B
		public static TextReader Synchronized(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (!(reader is TextReader.SyncTextReader))
			{
				return new TextReader.SyncTextReader(reader);
			}
			return reader;
		}

		// Token: 0x04003BE9 RID: 15337
		public static readonly TextReader Null = new TextReader.NullTextReader();

		// Token: 0x02000B1F RID: 2847
		[Serializable]
		private sealed class NullTextReader : TextReader
		{
			// Token: 0x060065CF RID: 26063 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			public override int Read(char[] buffer, int index, int count)
			{
				return 0;
			}

			// Token: 0x060065D0 RID: 26064 RVA: 0x0000AF5E File Offset: 0x0000915E
			public override string ReadLine()
			{
				return null;
			}
		}

		// Token: 0x02000B20 RID: 2848
		[Serializable]
		internal sealed class SyncTextReader : TextReader
		{
			// Token: 0x060065D1 RID: 26065 RVA: 0x0015C76F File Offset: 0x0015A96F
			internal SyncTextReader(TextReader t)
			{
				this._in = t;
			}

			// Token: 0x060065D2 RID: 26066 RVA: 0x0015C77E File Offset: 0x0015A97E
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override void Close()
			{
				this._in.Close();
			}

			// Token: 0x060065D3 RID: 26067 RVA: 0x0015C78B File Offset: 0x0015A98B
			[MethodImpl(MethodImplOptions.Synchronized)]
			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					((IDisposable)this._in).Dispose();
				}
			}

			// Token: 0x060065D4 RID: 26068 RVA: 0x0015C79B File Offset: 0x0015A99B
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override int Peek()
			{
				return this._in.Peek();
			}

			// Token: 0x060065D5 RID: 26069 RVA: 0x0015C7A8 File Offset: 0x0015A9A8
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override int Read()
			{
				return this._in.Read();
			}

			// Token: 0x060065D6 RID: 26070 RVA: 0x0015C7B5 File Offset: 0x0015A9B5
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override int Read(char[] buffer, int index, int count)
			{
				return this._in.Read(buffer, index, count);
			}

			// Token: 0x060065D7 RID: 26071 RVA: 0x0015C7C5 File Offset: 0x0015A9C5
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override int ReadBlock(char[] buffer, int index, int count)
			{
				return this._in.ReadBlock(buffer, index, count);
			}

			// Token: 0x060065D8 RID: 26072 RVA: 0x0015C7D5 File Offset: 0x0015A9D5
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override string ReadLine()
			{
				return this._in.ReadLine();
			}

			// Token: 0x060065D9 RID: 26073 RVA: 0x0015C7E2 File Offset: 0x0015A9E2
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override string ReadToEnd()
			{
				return this._in.ReadToEnd();
			}

			// Token: 0x060065DA RID: 26074 RVA: 0x0015C7EF File Offset: 0x0015A9EF
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override Task<string> ReadLineAsync()
			{
				return Task.FromResult<string>(this.ReadLine());
			}

			// Token: 0x060065DB RID: 26075 RVA: 0x0015C7FC File Offset: 0x0015A9FC
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override Task<string> ReadToEndAsync()
			{
				return Task.FromResult<string>(this.ReadToEnd());
			}

			// Token: 0x060065DC RID: 26076 RVA: 0x0015C80C File Offset: 0x0015AA0C
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
			{
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer", "Buffer cannot be null.");
				}
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Non-negative number required.");
				}
				if (buffer.Length - index < count)
				{
					throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
				}
				return Task.FromResult<int>(this.ReadBlock(buffer, index, count));
			}

			// Token: 0x060065DD RID: 26077 RVA: 0x0015C870 File Offset: 0x0015AA70
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override Task<int> ReadAsync(char[] buffer, int index, int count)
			{
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer", "Buffer cannot be null.");
				}
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Non-negative number required.");
				}
				if (buffer.Length - index < count)
				{
					throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
				}
				return Task.FromResult<int>(this.Read(buffer, index, count));
			}

			// Token: 0x04003BEA RID: 15338
			internal readonly TextReader _in;
		}
	}
}
