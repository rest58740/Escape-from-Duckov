using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics.Hashing;

namespace System
{
	// Token: 0x020000F7 RID: 247
	[Serializable]
	public readonly struct ArraySegment<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyList<T>, IReadOnlyCollection<T>
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0002126E File Offset: 0x0001F46E
		public static ArraySegment<T> Empty { get; } = new ArraySegment<T>(new T[0]);

		// Token: 0x0600071A RID: 1818 RVA: 0x00021275 File Offset: 0x0001F475
		public ArraySegment(T[] array)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			this._array = array;
			this._offset = 0;
			this._count = array.Length;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00021297 File Offset: 0x0001F497
		public ArraySegment(T[] array, int offset, int count)
		{
			if (array == null || offset > array.Length || count > array.Length - offset)
			{
				ThrowHelper.ThrowArraySegmentCtorValidationFailedExceptions(array, offset, count);
			}
			this._array = array;
			this._offset = offset;
			this._count = count;
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x000212C7 File Offset: 0x0001F4C7
		public T[] Array
		{
			get
			{
				return this._array;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x000212CF File Offset: 0x0001F4CF
		public int Offset
		{
			get
			{
				return this._offset;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x000212D7 File Offset: 0x0001F4D7
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x170000A3 RID: 163
		public T this[int index]
		{
			get
			{
				if (index >= this._count)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				return this._array[this._offset + index];
			}
			set
			{
				if (index >= this._count)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				this._array[this._offset + index] = value;
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00021326 File Offset: 0x0001F526
		public ArraySegment<T>.Enumerator GetEnumerator()
		{
			this.ThrowInvalidOperationIfDefault();
			return new ArraySegment<T>.Enumerator(this);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00021339 File Offset: 0x0001F539
		public override int GetHashCode()
		{
			if (this._array == null)
			{
				return 0;
			}
			return System.Numerics.Hashing.HashHelpers.Combine(System.Numerics.Hashing.HashHelpers.Combine(5381, this._offset), this._count) ^ this._array.GetHashCode();
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0002136C File Offset: 0x0001F56C
		public void CopyTo(T[] destination)
		{
			this.CopyTo(destination, 0);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00021376 File Offset: 0x0001F576
		public void CopyTo(T[] destination, int destinationIndex)
		{
			this.ThrowInvalidOperationIfDefault();
			System.Array.Copy(this._array, this._offset, destination, destinationIndex, this._count);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00021398 File Offset: 0x0001F598
		public void CopyTo(ArraySegment<T> destination)
		{
			this.ThrowInvalidOperationIfDefault();
			destination.ThrowInvalidOperationIfDefault();
			if (this._count > destination._count)
			{
				ThrowHelper.ThrowArgumentException_DestinationTooShort();
			}
			System.Array.Copy(this._array, this._offset, destination._array, destination._offset, this._count);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x000213E8 File Offset: 0x0001F5E8
		public override bool Equals(object obj)
		{
			return obj is ArraySegment<T> && this.Equals((ArraySegment<T>)obj);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00021400 File Offset: 0x0001F600
		public bool Equals(ArraySegment<T> obj)
		{
			return obj._array == this._array && obj._offset == this._offset && obj._count == this._count;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0002142E File Offset: 0x0001F62E
		public ArraySegment<T> Slice(int index)
		{
			this.ThrowInvalidOperationIfDefault();
			if (index > this._count)
			{
				ThrowHelper.ThrowArgumentOutOfRange_IndexException();
			}
			return new ArraySegment<T>(this._array, this._offset + index, this._count - index);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0002145F File Offset: 0x0001F65F
		public ArraySegment<T> Slice(int index, int count)
		{
			this.ThrowInvalidOperationIfDefault();
			if (index > this._count || count > this._count - index)
			{
				ThrowHelper.ThrowArgumentOutOfRange_IndexException();
			}
			return new ArraySegment<T>(this._array, this._offset + index, count);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00021494 File Offset: 0x0001F694
		public T[] ToArray()
		{
			this.ThrowInvalidOperationIfDefault();
			if (this._count == 0)
			{
				return ArraySegment<T>.Empty._array;
			}
			T[] array = new T[this._count];
			System.Array.Copy(this._array, this._offset, array, 0, this._count);
			return array;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x000214E0 File Offset: 0x0001F6E0
		public static bool operator ==(ArraySegment<T> a, ArraySegment<T> b)
		{
			return a.Equals(b);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x000214EA File Offset: 0x0001F6EA
		public static bool operator !=(ArraySegment<T> a, ArraySegment<T> b)
		{
			return !(a == b);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000214F8 File Offset: 0x0001F6F8
		public static implicit operator ArraySegment<T>(T[] array)
		{
			if (array == null)
			{
				return default(ArraySegment<T>);
			}
			return new ArraySegment<T>(array);
		}

		// Token: 0x170000A4 RID: 164
		T IList<!0>.this[int index]
		{
			get
			{
				this.ThrowInvalidOperationIfDefault();
				if (index < 0 || index >= this._count)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				return this._array[this._offset + index];
			}
			set
			{
				this.ThrowInvalidOperationIfDefault();
				if (index < 0 || index >= this._count)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				this._array[this._offset + index] = value;
			}
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00021574 File Offset: 0x0001F774
		int IList<!0>.IndexOf(T item)
		{
			this.ThrowInvalidOperationIfDefault();
			int num = System.Array.IndexOf<T>(this._array, item, this._offset, this._count);
			if (num < 0)
			{
				return -1;
			}
			return num - this._offset;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x000215AE File Offset: 0x0001F7AE
		void IList<!0>.Insert(int index, T item)
		{
			ThrowHelper.ThrowNotSupportedException();
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x000215AE File Offset: 0x0001F7AE
		void IList<!0>.RemoveAt(int index)
		{
			ThrowHelper.ThrowNotSupportedException();
		}

		// Token: 0x170000A5 RID: 165
		T IReadOnlyList<!0>.this[int index]
		{
			get
			{
				this.ThrowInvalidOperationIfDefault();
				if (index < 0 || index >= this._count)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				return this._array[this._offset + index];
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x000040F7 File Offset: 0x000022F7
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x000215AE File Offset: 0x0001F7AE
		void ICollection<!0>.Add(T item)
		{
			ThrowHelper.ThrowNotSupportedException();
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x000215AE File Offset: 0x0001F7AE
		void ICollection<!0>.Clear()
		{
			ThrowHelper.ThrowNotSupportedException();
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x000215B5 File Offset: 0x0001F7B5
		bool ICollection<!0>.Contains(T item)
		{
			this.ThrowInvalidOperationIfDefault();
			return System.Array.IndexOf<T>(this._array, item, this._offset, this._count) >= 0;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x000215DB File Offset: 0x0001F7DB
		bool ICollection<!0>.Remove(T item)
		{
			ThrowHelper.ThrowNotSupportedException();
			return false;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x000215E3 File Offset: 0x0001F7E3
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x000215E3 File Offset: 0x0001F7E3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000215F0 File Offset: 0x0001F7F0
		private void ThrowInvalidOperationIfDefault()
		{
			if (this._array == null)
			{
				ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_NullArray);
			}
		}

		// Token: 0x0400104B RID: 4171
		private readonly T[] _array;

		// Token: 0x0400104C RID: 4172
		private readonly int _offset;

		// Token: 0x0400104D RID: 4173
		private readonly int _count;

		// Token: 0x020000F8 RID: 248
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x0600073D RID: 1853 RVA: 0x00021613 File Offset: 0x0001F813
			internal Enumerator(ArraySegment<T> arraySegment)
			{
				this._array = arraySegment.Array;
				this._start = arraySegment.Offset;
				this._end = arraySegment.Offset + arraySegment.Count;
				this._current = arraySegment.Offset - 1;
			}

			// Token: 0x0600073E RID: 1854 RVA: 0x00021653 File Offset: 0x0001F853
			public bool MoveNext()
			{
				if (this._current < this._end)
				{
					this._current++;
					return this._current < this._end;
				}
				return false;
			}

			// Token: 0x170000A7 RID: 167
			// (get) Token: 0x0600073F RID: 1855 RVA: 0x00021681 File Offset: 0x0001F881
			public T Current
			{
				get
				{
					if (this._current < this._start)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumNotStarted();
					}
					if (this._current >= this._end)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumEnded();
					}
					return this._array[this._current];
				}
			}

			// Token: 0x170000A8 RID: 168
			// (get) Token: 0x06000740 RID: 1856 RVA: 0x000216BA File Offset: 0x0001F8BA
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000741 RID: 1857 RVA: 0x000216C7 File Offset: 0x0001F8C7
			void IEnumerator.Reset()
			{
				this._current = this._start - 1;
			}

			// Token: 0x06000742 RID: 1858 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public void Dispose()
			{
			}

			// Token: 0x0400104E RID: 4174
			private readonly T[] _array;

			// Token: 0x0400104F RID: 4175
			private readonly int _start;

			// Token: 0x04001050 RID: 4176
			private readonly int _end;

			// Token: 0x04001051 RID: 4177
			private int _current;
		}
	}
}
