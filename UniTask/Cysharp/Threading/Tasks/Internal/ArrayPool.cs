using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x02000106 RID: 262
	internal sealed class ArrayPool<T>
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x0000CFC0 File Offset: 0x0000B1C0
		private ArrayPool()
		{
			this.buckets = new MinimumQueue<T[]>[18];
			this.locks = new SpinLock[18];
			for (int i = 0; i < this.buckets.Length; i++)
			{
				this.buckets[i] = new MinimumQueue<T[]>(4);
				this.locks[i] = new SpinLock(false);
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0000D020 File Offset: 0x0000B220
		public T[] Rent(int minimumLength)
		{
			if (minimumLength < 0)
			{
				throw new ArgumentOutOfRangeException("minimumLength");
			}
			if (minimumLength == 0)
			{
				return ArrayPool<T>.EmptyArray;
			}
			int num = ArrayPool<T>.CalculateSize(minimumLength);
			int queueIndex = ArrayPool<T>.GetQueueIndex(num);
			if (queueIndex != -1)
			{
				MinimumQueue<T[]> minimumQueue = this.buckets[queueIndex];
				bool flag = false;
				try
				{
					this.locks[queueIndex].Enter(ref flag);
					if (minimumQueue.Count != 0)
					{
						return minimumQueue.Dequeue();
					}
				}
				finally
				{
					if (flag)
					{
						this.locks[queueIndex].Exit(false);
					}
				}
			}
			return new T[num];
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0000D0B8 File Offset: 0x0000B2B8
		public void Return(T[] array, bool clearArray = false)
		{
			if (array == null || array.Length == 0)
			{
				return;
			}
			int queueIndex = ArrayPool<T>.GetQueueIndex(array.Length);
			if (queueIndex != -1)
			{
				if (clearArray)
				{
					Array.Clear(array, 0, array.Length);
				}
				MinimumQueue<T[]> minimumQueue = this.buckets[queueIndex];
				bool flag = false;
				try
				{
					this.locks[queueIndex].Enter(ref flag);
					if (minimumQueue.Count <= 50)
					{
						minimumQueue.Enqueue(array);
					}
				}
				finally
				{
					if (flag)
					{
						this.locks[queueIndex].Exit(false);
					}
				}
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0000D140 File Offset: 0x0000B340
		private static int CalculateSize(int size)
		{
			size--;
			size |= size >> 1;
			size |= size >> 2;
			size |= size >> 4;
			size |= size >> 8;
			size |= size >> 16;
			size++;
			if (size < 8)
			{
				size = 8;
			}
			return size;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0000D178 File Offset: 0x0000B378
		private static int GetQueueIndex(int size)
		{
			if (size <= 2048)
			{
				if (size <= 64)
				{
					if (size <= 16)
					{
						if (size == 8)
						{
							return 0;
						}
						if (size == 16)
						{
							return 1;
						}
					}
					else
					{
						if (size == 32)
						{
							return 2;
						}
						if (size == 64)
						{
							return 3;
						}
					}
				}
				else if (size <= 256)
				{
					if (size == 128)
					{
						return 4;
					}
					if (size == 256)
					{
						return 5;
					}
				}
				else
				{
					if (size == 512)
					{
						return 6;
					}
					if (size == 1024)
					{
						return 7;
					}
					if (size == 2048)
					{
						return 8;
					}
				}
			}
			else if (size <= 32768)
			{
				if (size <= 8192)
				{
					if (size == 4096)
					{
						return 9;
					}
					if (size == 8192)
					{
						return 10;
					}
				}
				else
				{
					if (size == 16384)
					{
						return 11;
					}
					if (size == 32768)
					{
						return 12;
					}
				}
			}
			else if (size <= 131072)
			{
				if (size == 65536)
				{
					return 13;
				}
				if (size == 131072)
				{
					return 14;
				}
			}
			else
			{
				if (size == 262144)
				{
					return 15;
				}
				if (size == 524288)
				{
					return 16;
				}
				if (size == 1048576)
				{
					return 17;
				}
			}
			return -1;
		}

		// Token: 0x040000FC RID: 252
		private const int DefaultMaxNumberOfArraysPerBucket = 50;

		// Token: 0x040000FD RID: 253
		private static readonly T[] EmptyArray = new T[0];

		// Token: 0x040000FE RID: 254
		public static readonly ArrayPool<T> Shared = new ArrayPool<T>();

		// Token: 0x040000FF RID: 255
		private readonly MinimumQueue<T[]>[] buckets;

		// Token: 0x04000100 RID: 256
		private readonly SpinLock[] locks;
	}
}
