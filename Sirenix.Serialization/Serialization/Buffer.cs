using System;
using System.Collections.Generic;

namespace Sirenix.Serialization
{
	// Token: 0x02000059 RID: 89
	public sealed class Buffer<T> : IDisposable
	{
		// Token: 0x0600031F RID: 799 RVA: 0x00016C08 File Offset: 0x00014E08
		private Buffer(int count)
		{
			this.array = new T[count];
			this.count = count;
			this.isFree = false;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000320 RID: 800 RVA: 0x00016C2C File Offset: 0x00014E2C
		public int Count
		{
			get
			{
				if (this.isFree)
				{
					throw new InvalidOperationException("Cannot access a buffer while it is freed.");
				}
				return this.count;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00016C49 File Offset: 0x00014E49
		public T[] Array
		{
			get
			{
				if (this.isFree)
				{
					throw new InvalidOperationException("Cannot access a buffer while it is freed.");
				}
				return this.array;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000322 RID: 802 RVA: 0x00016C66 File Offset: 0x00014E66
		public bool IsFree
		{
			get
			{
				return this.isFree;
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00016C70 File Offset: 0x00014E70
		public static Buffer<T> Claim(int minimumCapacity)
		{
			if (minimumCapacity < 0)
			{
				throw new ArgumentException("Requested size of buffer must be larger than or equal to 0.");
			}
			if (minimumCapacity < 256)
			{
				minimumCapacity = 256;
			}
			Buffer<T> buffer = null;
			object @lock = Buffer<T>.LOCK;
			lock (@lock)
			{
				for (int i = 0; i < Buffer<T>.FreeBuffers.Count; i++)
				{
					Buffer<T> buffer2 = Buffer<T>.FreeBuffers[i];
					if (buffer2 != null && buffer2.count >= minimumCapacity)
					{
						buffer = buffer2;
						buffer.isFree = false;
						Buffer<T>.FreeBuffers[i] = null;
						break;
					}
				}
			}
			if (buffer == null)
			{
				buffer = new Buffer<T>(Buffer<T>.NextPowerOfTwo(minimumCapacity));
			}
			return buffer;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00016D24 File Offset: 0x00014F24
		public static void Free(Buffer<T> buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (!buffer.isFree)
			{
				object @lock = Buffer<T>.LOCK;
				lock (@lock)
				{
					if (!buffer.isFree)
					{
						buffer.isFree = true;
						bool flag2 = false;
						for (int i = 0; i < Buffer<T>.FreeBuffers.Count; i++)
						{
							if (Buffer<T>.FreeBuffers[i] == null)
							{
								Buffer<T>.FreeBuffers[i] = buffer;
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							Buffer<T>.FreeBuffers.Add(buffer);
						}
					}
				}
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00016DCC File Offset: 0x00014FCC
		public void Free()
		{
			Buffer<T>.Free(this);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00016DCC File Offset: 0x00014FCC
		public void Dispose()
		{
			Buffer<T>.Free(this);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00016DD4 File Offset: 0x00014FD4
		private static int NextPowerOfTwo(int v)
		{
			v--;
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			v |= v >> 16;
			v++;
			return v;
		}

		// Token: 0x040000FA RID: 250
		private static readonly object LOCK = new object();

		// Token: 0x040000FB RID: 251
		private static readonly List<Buffer<T>> FreeBuffers = new List<Buffer<T>>();

		// Token: 0x040000FC RID: 252
		private int count;

		// Token: 0x040000FD RID: 253
		private T[] array;

		// Token: 0x040000FE RID: 254
		private volatile bool isFree;
	}
}
