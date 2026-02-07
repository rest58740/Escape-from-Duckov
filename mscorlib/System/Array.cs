using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020001D1 RID: 465
	[Serializable]
	public abstract class Array : ICollection, IEnumerable, IList, IStructuralComparable, IStructuralEquatable, ICloneable
	{
		// Token: 0x060013DB RID: 5083 RVA: 0x0004EC90 File Offset: 0x0004CE90
		public static Array CreateInstance(Type elementType, params long[] lengths)
		{
			if (lengths == null)
			{
				throw new ArgumentNullException("lengths");
			}
			if (lengths.Length == 0)
			{
				throw new ArgumentException("Must provide at least one rank.");
			}
			int[] array = new int[lengths.Length];
			for (int i = 0; i < lengths.Length; i++)
			{
				long num = lengths[i];
				if (num > 2147483647L || num < -2147483648L)
				{
					throw new ArgumentOutOfRangeException("len", "Arrays larger than 2GB are not supported.");
				}
				array[i] = (int)num;
			}
			return Array.CreateInstance(elementType, array);
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0004ED03 File Offset: 0x0004CF03
		public static ReadOnlyCollection<T> AsReadOnly<T>(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return new ReadOnlyCollection<T>(array);
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0004ED1C File Offset: 0x0004CF1C
		public static void Resize<T>(ref T[] array, int newSize)
		{
			if (newSize < 0)
			{
				throw new ArgumentOutOfRangeException("newSize", "Non-negative number required.");
			}
			T[] array2 = array;
			if (array2 == null)
			{
				array = new T[newSize];
				return;
			}
			if (array2.Length != newSize)
			{
				T[] array3 = new T[newSize];
				Array.Copy(array2, 0, array3, 0, (array2.Length > newSize) ? newSize : array2.Length);
				array = array3;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x0004ED71 File Offset: 0x0004CF71
		int ICollection.Count
		{
			get
			{
				return this.Length;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060013DF RID: 5087 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E1 RID: 481
		object IList.this[int index]
		{
			get
			{
				return this.GetValue(index);
			}
			set
			{
				this.SetValue(value, index);
			}
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0004ED8C File Offset: 0x0004CF8C
		int IList.Add(object value)
		{
			throw new NotSupportedException("Collection was of a fixed size.");
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0004ED98 File Offset: 0x0004CF98
		bool IList.Contains(object value)
		{
			return Array.IndexOf(this, value) >= 0;
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0004EDA7 File Offset: 0x0004CFA7
		void IList.Clear()
		{
			Array.Clear(this, this.GetLowerBound(0), this.Length);
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0004EDBC File Offset: 0x0004CFBC
		int IList.IndexOf(object value)
		{
			return Array.IndexOf(this, value);
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0004ED8C File Offset: 0x0004CF8C
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException("Collection was of a fixed size.");
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0004ED8C File Offset: 0x0004CF8C
		void IList.Remove(object value)
		{
			throw new NotSupportedException("Collection was of a fixed size.");
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0004ED8C File Offset: 0x0004CF8C
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException("Collection was of a fixed size.");
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0004EDC5 File Offset: 0x0004CFC5
		public void CopyTo(Array array, int index)
		{
			if (array != null && array.Rank != 1)
			{
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
			}
			Array.Copy(this, this.GetLowerBound(0), array, index, this.Length);
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x000231D1 File Offset: 0x000213D1
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0004EDF4 File Offset: 0x0004CFF4
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Array array = other as Array;
			if (array == null || this.Length != array.Length)
			{
				throw new ArgumentException("Object is not a array with the same number of elements as the array to compare it to.", "other");
			}
			int num = 0;
			int num2 = 0;
			while (num < array.Length && num2 == 0)
			{
				object value = this.GetValue(num);
				object value2 = array.GetValue(num);
				num2 = comparer.Compare(value, value2);
				num++;
			}
			return num2;
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0004EE60 File Offset: 0x0004D060
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			Array array = other as Array;
			if (array == null || array.Length != this.Length)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				object value = this.GetValue(i);
				object value2 = array.GetValue(i);
				if (!comparer.Equals(value, value2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00033E4F File Offset: 0x0003204F
		internal static int CombineHashCodes(int h1, int h2)
		{
			return (h1 << 5) + h1 ^ h2;
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0004EEC0 File Offset: 0x0004D0C0
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			int num = 0;
			for (int i = (this.Length >= 8) ? (this.Length - 8) : 0; i < this.Length; i++)
			{
				num = Array.CombineHashCodes(num, comparer.GetHashCode(this.GetValue(i)));
			}
			return num;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0004EF16 File Offset: 0x0004D116
		public static int BinarySearch(Array array, object value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.BinarySearch(array, array.GetLowerBound(0), array.Length, value, null);
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0004EF3C File Offset: 0x0004D13C
		public static TOutput[] ConvertAll<TInput, TOutput>(TInput[] array, Converter<TInput, TOutput> converter)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}
			TOutput[] array2 = new TOutput[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = converter(array[i]);
			}
			return array2;
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0004EF91 File Offset: 0x0004D191
		public static void Copy(Array sourceArray, Array destinationArray, long length)
		{
			if (length > 2147483647L || length < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("length", "Arrays larger than 2GB are not supported.");
			}
			Array.Copy(sourceArray, destinationArray, (int)length);
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0004EFC0 File Offset: 0x0004D1C0
		public static void Copy(Array sourceArray, long sourceIndex, Array destinationArray, long destinationIndex, long length)
		{
			if (sourceIndex > 2147483647L || sourceIndex < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("sourceIndex", "Arrays larger than 2GB are not supported.");
			}
			if (destinationIndex > 2147483647L || destinationIndex < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("destinationIndex", "Arrays larger than 2GB are not supported.");
			}
			if (length > 2147483647L || length < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("length", "Arrays larger than 2GB are not supported.");
			}
			Array.Copy(sourceArray, (int)sourceIndex, destinationArray, (int)destinationIndex, (int)length);
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x0004F043 File Offset: 0x0004D243
		public void CopyTo(Array array, long index)
		{
			if (index > 2147483647L || index < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index", "Arrays larger than 2GB are not supported.");
			}
			this.CopyTo(array, (int)index);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0004F070 File Offset: 0x0004D270
		public static void ForEach<T>(T[] array, Action<T> action)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			for (int i = 0; i < array.Length; i++)
			{
				action(array[i]);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x0004F0B4 File Offset: 0x0004D2B4
		public long LongLength
		{
			get
			{
				long num = (long)this.GetLength(0);
				for (int i = 1; i < this.Rank; i++)
				{
					num *= (long)this.GetLength(i);
				}
				return num;
			}
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0004F0E7 File Offset: 0x0004D2E7
		public long GetLongLength(int dimension)
		{
			return (long)this.GetLength(dimension);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0004F0F1 File Offset: 0x0004D2F1
		public object GetValue(long index)
		{
			if (index > 2147483647L || index < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index", "Arrays larger than 2GB are not supported.");
			}
			return this.GetValue((int)index);
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0004F120 File Offset: 0x0004D320
		public object GetValue(long index1, long index2)
		{
			if (index1 > 2147483647L || index1 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index1", "Arrays larger than 2GB are not supported.");
			}
			if (index2 > 2147483647L || index2 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index2", "Arrays larger than 2GB are not supported.");
			}
			return this.GetValue((int)index1, (int)index2);
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0004F17C File Offset: 0x0004D37C
		public object GetValue(long index1, long index2, long index3)
		{
			if (index1 > 2147483647L || index1 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index1", "Arrays larger than 2GB are not supported.");
			}
			if (index2 > 2147483647L || index2 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index2", "Arrays larger than 2GB are not supported.");
			}
			if (index3 > 2147483647L || index3 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index3", "Arrays larger than 2GB are not supported.");
			}
			return this.GetValue((int)index1, (int)index2, (int)index3);
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0004F1FC File Offset: 0x0004D3FC
		public object GetValue(params long[] indices)
		{
			if (indices == null)
			{
				throw new ArgumentNullException("indices");
			}
			if (this.Rank != indices.Length)
			{
				throw new ArgumentException("Indices length does not match the array rank.");
			}
			int[] array = new int[indices.Length];
			for (int i = 0; i < indices.Length; i++)
			{
				long num = indices[i];
				if (num > 2147483647L || num < -2147483648L)
				{
					throw new ArgumentOutOfRangeException("index", "Arrays larger than 2GB are not supported.");
				}
				array[i] = (int)num;
			}
			return this.GetValue(array);
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x0000270D File Offset: 0x0000090D
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0004F276 File Offset: 0x0004D476
		public static int BinarySearch(Array array, int index, int length, object value)
		{
			return Array.BinarySearch(array, index, length, value, null);
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0004F282 File Offset: 0x0004D482
		public static int BinarySearch(Array array, object value, IComparer comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.BinarySearch(array, array.GetLowerBound(0), array.Length, value, comparer);
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0004F2A8 File Offset: 0x0004D4A8
		public static int BinarySearch(Array array, int index, int length, object value, IComparer comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || length < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "length", "Non-negative number required.");
			}
			if (array.Length - index < length)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (array.Rank != 1)
			{
				throw new RankException("Only single dimension arrays are supported here.");
			}
			if (comparer == null)
			{
				comparer = Comparer.Default;
			}
			int i = index;
			int num = index + length - 1;
			object[] array2 = array as object[];
			if (array2 != null)
			{
				while (i <= num)
				{
					int median = Array.GetMedian(i, num);
					int num2;
					try
					{
						num2 = comparer.Compare(array2[median], value);
					}
					catch (Exception innerException)
					{
						throw new InvalidOperationException("Failed to compare two elements in the array.", innerException);
					}
					if (num2 == 0)
					{
						return median;
					}
					if (num2 < 0)
					{
						i = median + 1;
					}
					else
					{
						num = median - 1;
					}
				}
			}
			else
			{
				while (i <= num)
				{
					int median2 = Array.GetMedian(i, num);
					int num3;
					try
					{
						num3 = comparer.Compare(array.GetValue(median2), value);
					}
					catch (Exception innerException2)
					{
						throw new InvalidOperationException("Failed to compare two elements in the array.", innerException2);
					}
					if (num3 == 0)
					{
						return median2;
					}
					if (num3 < 0)
					{
						i = median2 + 1;
					}
					else
					{
						num = median2 - 1;
					}
				}
			}
			return ~i;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0004F3DC File Offset: 0x0004D5DC
		private static int GetMedian(int low, int hi)
		{
			return low + (hi - low >> 1);
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0004F3E5 File Offset: 0x0004D5E5
		public static int BinarySearch<T>(T[] array, T value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.BinarySearch<T>(array, 0, array.Length, value, null);
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x0004F401 File Offset: 0x0004D601
		public static int BinarySearch<T>(T[] array, T value, IComparer<T> comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.BinarySearch<T>(array, 0, array.Length, value, comparer);
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0004F41D File Offset: 0x0004D61D
		public static int BinarySearch<T>(T[] array, int index, int length, T value)
		{
			return Array.BinarySearch<T>(array, index, length, value, null);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0004F42C File Offset: 0x0004D62C
		public static int BinarySearch<T>(T[] array, int index, int length, T value, IComparer<T> comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || length < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "length", "Non-negative number required.");
			}
			if (array.Length - index < length)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			return ArraySortHelper<T>.Default.BinarySearch(array, index, length, value, comparer);
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0004F48D File Offset: 0x0004D68D
		public static int IndexOf(Array array, object value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.IndexOf(array, value, array.GetLowerBound(0), array.Length);
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0004F4B4 File Offset: 0x0004D6B4
		public static int IndexOf(Array array, object value, int startIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int lowerBound = array.GetLowerBound(0);
			return Array.IndexOf(array, value, startIndex, array.Length - startIndex + lowerBound);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0004F4EC File Offset: 0x0004D6EC
		public static int IndexOf(Array array, object value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new RankException("Only single dimension arrays are supported here.");
			}
			int lowerBound = array.GetLowerBound(0);
			if (startIndex < lowerBound || startIndex > array.Length + lowerBound)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || count > array.Length - startIndex + lowerBound)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			object[] array2 = array as object[];
			int num = startIndex + count;
			if (array2 != null)
			{
				if (value == null)
				{
					for (int i = startIndex; i < num; i++)
					{
						if (array2[i] == null)
						{
							return i;
						}
					}
				}
				else
				{
					for (int j = startIndex; j < num; j++)
					{
						object obj = array2[j];
						if (obj != null && obj.Equals(value))
						{
							return j;
						}
					}
				}
			}
			else
			{
				for (int k = startIndex; k < num; k++)
				{
					object value2 = array.GetValue(k);
					if (value2 == null)
					{
						if (value == null)
						{
							return k;
						}
					}
					else if (value2.Equals(value))
					{
						return k;
					}
				}
			}
			return lowerBound - 1;
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0004F5E6 File Offset: 0x0004D7E6
		public static int IndexOf<T>(T[] array, T value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.IndexOfImpl<T>(array, value, 0, array.Length);
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0004F601 File Offset: 0x0004D801
		public static int IndexOf<T>(T[] array, T value, int startIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.IndexOf<T>(array, value, startIndex, array.Length - startIndex);
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0004F620 File Offset: 0x0004D820
		public static int IndexOf<T>(T[] array, T value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (startIndex < 0 || startIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || count > array.Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			return Array.IndexOfImpl<T>(array, value, startIndex, count);
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0004F67A File Offset: 0x0004D87A
		public static int LastIndexOf(Array array, object value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.LastIndexOf(array, value, array.Length - 1, array.Length);
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0004F69F File Offset: 0x0004D89F
		public static int LastIndexOf(Array array, object value, int startIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.LastIndexOf(array, value, startIndex, startIndex + 1);
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0004F6BC File Offset: 0x0004D8BC
		public static int LastIndexOf(Array array, object value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Length == 0)
			{
				return -1;
			}
			if (startIndex < 0 || startIndex >= array.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			if (count > startIndex + 1)
			{
				throw new ArgumentOutOfRangeException("endIndex", "endIndex cannot be greater than startIndex.");
			}
			if (array.Rank != 1)
			{
				throw new RankException("Only single dimension arrays are supported here.");
			}
			object[] array2 = array as object[];
			int num = startIndex - count + 1;
			if (array2 != null)
			{
				if (value == null)
				{
					for (int i = startIndex; i >= num; i--)
					{
						if (array2[i] == null)
						{
							return i;
						}
					}
				}
				else
				{
					for (int j = startIndex; j >= num; j--)
					{
						object obj = array2[j];
						if (obj != null && obj.Equals(value))
						{
							return j;
						}
					}
				}
			}
			else
			{
				for (int k = startIndex; k >= num; k--)
				{
					object value2 = array.GetValue(k);
					if (value2 == null)
					{
						if (value == null)
						{
							return k;
						}
					}
					else if (value2.Equals(value))
					{
						return k;
					}
				}
			}
			return -1;
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0004F7B9 File Offset: 0x0004D9B9
		public static int LastIndexOf<T>(T[] array, T value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.LastIndexOf<T>(array, value, array.Length - 1, array.Length);
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0004F7D8 File Offset: 0x0004D9D8
		public static int LastIndexOf<T>(T[] array, T value, int startIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.LastIndexOf<T>(array, value, startIndex, (array.Length == 0) ? 0 : (startIndex + 1));
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0004F7FC File Offset: 0x0004D9FC
		public static int LastIndexOf<T>(T[] array, T value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Length == 0)
			{
				if (startIndex != -1 && startIndex != 0)
				{
					throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (count != 0)
				{
					throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
				}
				return -1;
			}
			else
			{
				if (startIndex < 0 || startIndex >= array.Length)
				{
					throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (count < 0 || startIndex - count + 1 < 0)
				{
					throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
				}
				return Array.LastIndexOfImpl<T>(array, value, startIndex, count);
			}
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0004F886 File Offset: 0x0004DA86
		public static void Reverse(Array array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Reverse(array, array.GetLowerBound(0), array.Length);
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0004F8AC File Offset: 0x0004DAAC
		public static void Reverse(Array array, int index, int length)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int lowerBound = array.GetLowerBound(0);
			if (index < lowerBound || length < 0)
			{
				throw new ArgumentOutOfRangeException((index < lowerBound) ? "index" : "length", "Non-negative number required.");
			}
			if (array.Length - (index - lowerBound) < length)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (array.Rank != 1)
			{
				throw new RankException("Only single dimension arrays are supported here.");
			}
			object[] array2 = array as object[];
			if (array2 != null)
			{
				Array.Reverse<object>(array2, index, length);
				return;
			}
			int i = index;
			int num = index + length - 1;
			while (i < num)
			{
				object value = array.GetValue(i);
				array.SetValue(array.GetValue(num), i);
				array.SetValue(value, num);
				i++;
				num--;
			}
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0004F967 File Offset: 0x0004DB67
		public static void Reverse<T>(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Reverse<T>(array, 0, array.Length);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0004F984 File Offset: 0x0004DB84
		public static void Reverse<T>(T[] array, int index, int length)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || length < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "length", "Non-negative number required.");
			}
			if (array.Length - index < length)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (length <= 1)
			{
				return;
			}
			ref T ptr = ref Unsafe.Add<T>(Unsafe.As<byte, T>(array.GetRawSzArrayData()), index);
			ref T ptr2 = ref Unsafe.Add<T>(Unsafe.Add<T>(ref ptr, length), -1);
			do
			{
				T t = ptr;
				ptr = ptr2;
				ptr2 = t;
				ptr = Unsafe.Add<T>(ref ptr, 1);
				ptr2 = Unsafe.Add<T>(ref ptr2, -1);
			}
			while (Unsafe.IsAddressLessThan<T>(ref ptr, ref ptr2));
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0004FA2D File Offset: 0x0004DC2D
		public void SetValue(object value, long index)
		{
			if (index > 2147483647L || index < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index", "Arrays larger than 2GB are not supported.");
			}
			this.SetValue(value, (int)index);
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x0004FA5C File Offset: 0x0004DC5C
		public void SetValue(object value, long index1, long index2)
		{
			if (index1 > 2147483647L || index1 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index1", "Arrays larger than 2GB are not supported.");
			}
			if (index2 > 2147483647L || index2 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index2", "Arrays larger than 2GB are not supported.");
			}
			this.SetValue(value, (int)index1, (int)index2);
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0004FAB8 File Offset: 0x0004DCB8
		public void SetValue(object value, long index1, long index2, long index3)
		{
			if (index1 > 2147483647L || index1 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index1", "Arrays larger than 2GB are not supported.");
			}
			if (index2 > 2147483647L || index2 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index2", "Arrays larger than 2GB are not supported.");
			}
			if (index3 > 2147483647L || index3 < -2147483648L)
			{
				throw new ArgumentOutOfRangeException("index3", "Arrays larger than 2GB are not supported.");
			}
			this.SetValue(value, (int)index1, (int)index2, (int)index3);
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0004FB3C File Offset: 0x0004DD3C
		public void SetValue(object value, params long[] indices)
		{
			if (indices == null)
			{
				throw new ArgumentNullException("indices");
			}
			if (this.Rank != indices.Length)
			{
				throw new ArgumentException("Indices length does not match the array rank.");
			}
			int[] array = new int[indices.Length];
			for (int i = 0; i < indices.Length; i++)
			{
				long num = indices[i];
				if (num > 2147483647L || num < -2147483648L)
				{
					throw new ArgumentOutOfRangeException("index", "Arrays larger than 2GB are not supported.");
				}
				array[i] = (int)num;
			}
			this.SetValue(value, array);
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0004FBB7 File Offset: 0x0004DDB7
		public static void Sort(Array array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort(array, null, array.GetLowerBound(0), array.Length, null);
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0004FBDC File Offset: 0x0004DDDC
		public static void Sort(Array array, int index, int length)
		{
			Array.Sort(array, null, index, length, null);
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0004FBE8 File Offset: 0x0004DDE8
		public static void Sort(Array array, IComparer comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort(array, null, array.GetLowerBound(0), array.Length, comparer);
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0004FC0D File Offset: 0x0004DE0D
		public static void Sort(Array array, int index, int length, IComparer comparer)
		{
			Array.Sort(array, null, index, length, comparer);
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0004FC19 File Offset: 0x0004DE19
		public static void Sort(Array keys, Array items)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			Array.Sort(keys, items, keys.GetLowerBound(0), keys.Length, null);
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0004FC3E File Offset: 0x0004DE3E
		public static void Sort(Array keys, Array items, IComparer comparer)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			Array.Sort(keys, items, keys.GetLowerBound(0), keys.Length, comparer);
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0004FC63 File Offset: 0x0004DE63
		public static void Sort(Array keys, Array items, int index, int length)
		{
			Array.Sort(keys, items, index, length, null);
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0004FC70 File Offset: 0x0004DE70
		public static void Sort(Array keys, Array items, int index, int length, IComparer comparer)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			if (keys.Rank != 1 || (items != null && items.Rank != 1))
			{
				throw new RankException("Only single dimension arrays are supported here.");
			}
			int lowerBound = keys.GetLowerBound(0);
			if (items != null && lowerBound != items.GetLowerBound(0))
			{
				throw new ArgumentException("The arrays' lower bounds must be identical.");
			}
			if (index < lowerBound || length < 0)
			{
				throw new ArgumentOutOfRangeException((length < 0) ? "length" : "index", "Non-negative number required.");
			}
			if (keys.Length - (index - lowerBound) < length || (items != null && index - lowerBound > items.Length - length))
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (length > 1)
			{
				Array.SortImpl(keys, items, index, length, comparer);
			}
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0004FD25 File Offset: 0x0004DF25
		public static void Sort<T>(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort<T>(array, 0, array.Length, null);
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0004FD40 File Offset: 0x0004DF40
		public static void Sort<T>(T[] array, int index, int length)
		{
			Array.Sort<T>(array, index, length, null);
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0004FD4B File Offset: 0x0004DF4B
		public static void Sort<T>(T[] array, IComparer<T> comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort<T>(array, 0, array.Length, comparer);
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0004FD68 File Offset: 0x0004DF68
		public static void Sort<T>(T[] array, int index, int length, IComparer<T> comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || length < 0)
			{
				throw new ArgumentOutOfRangeException((length < 0) ? "length" : "index", "Non-negative number required.");
			}
			if (array.Length - index < length)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (length > 1)
			{
				ArraySortHelper<T>.Default.Sort(array, index, length, comparer);
			}
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0004FDCB File Offset: 0x0004DFCB
		public static void Sort<T>(T[] array, Comparison<T> comparison)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (comparison == null)
			{
				throw new ArgumentNullException("comparison");
			}
			ArraySortHelper<T>.Sort(array, 0, array.Length, comparison);
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0004FDF4 File Offset: 0x0004DFF4
		public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			Array.Sort<TKey, TValue>(keys, items, 0, keys.Length, null);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0004FE10 File Offset: 0x0004E010
		public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length)
		{
			Array.Sort<TKey, TValue>(keys, items, index, length, null);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0004FE1C File Offset: 0x0004E01C
		public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, IComparer<TKey> comparer)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			Array.Sort<TKey, TValue>(keys, items, 0, keys.Length, comparer);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0004FE38 File Offset: 0x0004E038
		public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length, IComparer<TKey> comparer)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			if (index < 0 || length < 0)
			{
				throw new ArgumentOutOfRangeException((length < 0) ? "length" : "index", "Non-negative number required.");
			}
			if (keys.Length - index < length || (items != null && index > items.Length - length))
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (length > 1)
			{
				if (items == null)
				{
					Array.Sort<TKey>(keys, index, length, comparer);
					return;
				}
				ArraySortHelper<TKey, TValue>.Default.Sort(keys, items, index, length, comparer);
			}
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0004FEB6 File Offset: 0x0004E0B6
		public static bool Exists<T>(T[] array, Predicate<T> match)
		{
			return Array.FindIndex<T>(array, match) != -1;
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0004FEC8 File Offset: 0x0004E0C8
		public static void Fill<T>(T[] array, T value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = value;
			}
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0004FEFC File Offset: 0x0004E0FC
		public static void Fill<T>(T[] array, T value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (startIndex < 0 || startIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || startIndex > array.Length - count)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			for (int i = startIndex; i < startIndex + count; i++)
			{
				array[i] = value;
			}
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0004FF64 File Offset: 0x0004E164
		public static T Find<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (match(array[i]))
				{
					return array[i];
				}
			}
			return default(T);
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0004FFBC File Offset: 0x0004E1BC
		public static T[] FindAll<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			int num = 0;
			T[] array2 = Array.Empty<T>();
			for (int i = 0; i < array.Length; i++)
			{
				if (match(array[i]))
				{
					if (num == array2.Length)
					{
						Array.Resize<T>(ref array2, Math.Min((num == 0) ? 4 : (num * 2), array.Length));
					}
					array2[num++] = array[i];
				}
			}
			if (num != array2.Length)
			{
				Array.Resize<T>(ref array2, num);
			}
			return array2;
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x00050049 File Offset: 0x0004E249
		public static int FindIndex<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.FindIndex<T>(array, 0, array.Length, match);
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x00050064 File Offset: 0x0004E264
		public static int FindIndex<T>(T[] array, int startIndex, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.FindIndex<T>(array, startIndex, array.Length - startIndex, match);
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x00050084 File Offset: 0x0004E284
		public static int FindIndex<T>(T[] array, int startIndex, int count, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (startIndex < 0 || startIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || startIndex > array.Length - count)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (match(array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x00050108 File Offset: 0x0004E308
		public static T FindLast<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			for (int i = array.Length - 1; i >= 0; i--)
			{
				if (match(array[i]))
				{
					return array[i];
				}
			}
			return default(T);
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x00050161 File Offset: 0x0004E361
		public static int FindLastIndex<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.FindLastIndex<T>(array, array.Length - 1, array.Length, match);
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x00050180 File Offset: 0x0004E380
		public static int FindLastIndex<T>(T[] array, int startIndex, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.FindLastIndex<T>(array, startIndex, startIndex + 1, match);
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0005019C File Offset: 0x0004E39C
		public static int FindLastIndex<T>(T[] array, int startIndex, int count, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			if (array.Length == 0)
			{
				if (startIndex != -1)
				{
					throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
			}
			else if (startIndex < 0 || startIndex >= array.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (count < 0 || startIndex - count + 1 < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Count must be positive and count must refer to a location within the string/array/collection.");
			}
			int num = startIndex - count;
			for (int i = startIndex; i > num; i--)
			{
				if (match(array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x00050238 File Offset: 0x0004E438
		public static bool TrueForAll<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!match(array[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x00050281 File Offset: 0x0004E481
		public IEnumerator GetEnumerator()
		{
			return new Array.ArrayEnumerator(this);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0000259F File Offset: 0x0000079F
		private Array()
		{
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0004ED71 File Offset: 0x0004CF71
		internal int InternalArray__ICollection_get_Count()
		{
			return this.Length;
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x000040F7 File Offset: 0x000022F7
		internal bool InternalArray__ICollection_get_IsReadOnly()
		{
			return true;
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x00050289 File Offset: 0x0004E489
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ref byte GetRawSzArrayData()
		{
			return ref Unsafe.As<Array.RawData>(this).Data;
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x00050296 File Offset: 0x0004E496
		internal IEnumerator<T> InternalArray__IEnumerable_GetEnumerator<T>()
		{
			if (this.Length == 0)
			{
				return Array.EmptyInternalEnumerator<T>.Value;
			}
			return new Array.InternalEnumerator<T>(this);
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x000502B1 File Offset: 0x0004E4B1
		internal void InternalArray__ICollection_Clear()
		{
			throw new NotSupportedException("Collection is read-only");
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x000502BD File Offset: 0x0004E4BD
		internal void InternalArray__ICollection_Add<T>(T item)
		{
			throw new NotSupportedException("Collection is of a fixed size");
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x000502BD File Offset: 0x0004E4BD
		internal bool InternalArray__ICollection_Remove<T>(T item)
		{
			throw new NotSupportedException("Collection is of a fixed size");
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x000502CC File Offset: 0x0004E4CC
		internal bool InternalArray__ICollection_Contains<T>(T item)
		{
			if (this.Rank > 1)
			{
				throw new RankException("Only single dimension arrays are supported.");
			}
			int length = this.Length;
			for (int i = 0; i < length; i++)
			{
				T t;
				this.GetGenericValueImpl<T>(i, out t);
				if (item == null)
				{
					if (t == null)
					{
						return true;
					}
				}
				else if (item.Equals(t))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00050333 File Offset: 0x0004E533
		internal void InternalArray__ICollection_CopyTo<T>(T[] array, int arrayIndex)
		{
			Array.Copy(this, this.GetLowerBound(0), array, arrayIndex, this.Length);
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0005034C File Offset: 0x0004E54C
		internal T InternalArray__IReadOnlyList_get_Item<T>(int index)
		{
			if (index >= this.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			T result;
			this.GetGenericValueImpl<T>(index, out result);
			return result;
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0004ED71 File Offset: 0x0004CF71
		internal int InternalArray__IReadOnlyCollection_get_Count()
		{
			return this.Length;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x000502BD File Offset: 0x0004E4BD
		internal void InternalArray__Insert<T>(int index, T item)
		{
			throw new NotSupportedException("Collection is of a fixed size");
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x000502BD File Offset: 0x0004E4BD
		internal void InternalArray__RemoveAt(int index)
		{
			throw new NotSupportedException("Collection is of a fixed size");
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x00050378 File Offset: 0x0004E578
		internal int InternalArray__IndexOf<T>(T item)
		{
			if (this.Rank > 1)
			{
				throw new RankException("Only single dimension arrays are supported.");
			}
			int length = this.Length;
			for (int i = 0; i < length; i++)
			{
				T t;
				this.GetGenericValueImpl<T>(i, out t);
				if (item == null)
				{
					if (t == null)
					{
						return i + this.GetLowerBound(0);
					}
				}
				else if (t.Equals(item))
				{
					return i + this.GetLowerBound(0);
				}
			}
			return this.GetLowerBound(0) - 1;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x000503F8 File Offset: 0x0004E5F8
		internal T InternalArray__get_Item<T>(int index)
		{
			if (index >= this.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			T result;
			this.GetGenericValueImpl<T>(index, out result);
			return result;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x00050424 File Offset: 0x0004E624
		internal void InternalArray__set_Item<T>(int index, T item)
		{
			if (index >= this.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			object[] array = this as object[];
			if (array != null)
			{
				array[index] = item;
				return;
			}
			this.SetGenericValueImpl<T>(index, ref item);
		}

		// Token: 0x0600144B RID: 5195
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetGenericValue_icall<T>(ref Array self, int pos, out T value);

		// Token: 0x0600144C RID: 5196
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGenericValue_icall<T>(ref Array self, int pos, ref T value);

		// Token: 0x0600144D RID: 5197 RVA: 0x00050464 File Offset: 0x0004E664
		internal void GetGenericValueImpl<T>(int pos, out T value)
		{
			Array array = this;
			Array.GetGenericValue_icall<T>(ref array, pos, out value);
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0005047C File Offset: 0x0004E67C
		internal void SetGenericValueImpl<T>(int pos, ref T value)
		{
			Array array = this;
			Array.SetGenericValue_icall<T>(ref array, pos, ref value);
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600144F RID: 5199 RVA: 0x00050494 File Offset: 0x0004E694
		public int Length
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				int num = this.GetLength(0);
				for (int i = 1; i < this.Rank; i++)
				{
					num *= this.GetLength(i);
				}
				return num;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x000504C5 File Offset: 0x0004E6C5
		public int Rank
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.GetRank();
			}
		}

		// Token: 0x06001451 RID: 5201
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetRank();

		// Token: 0x06001452 RID: 5202
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetLength(int dimension);

		// Token: 0x06001453 RID: 5203
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetLowerBound(int dimension);

		// Token: 0x06001454 RID: 5204
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object GetValue(params int[] indices);

		// Token: 0x06001455 RID: 5205
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(object value, params int[] indices);

		// Token: 0x06001456 RID: 5206
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object GetValueImpl(int pos);

		// Token: 0x06001457 RID: 5207
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetValueImpl(object value, int pos);

		// Token: 0x06001458 RID: 5208
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool FastCopy(Array source, int source_idx, Array dest, int dest_idx, int length);

		// Token: 0x06001459 RID: 5209
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Array CreateInstanceImpl(Type elementType, int[] lengths, int[] bounds);

		// Token: 0x0600145A RID: 5210 RVA: 0x000504CD File Offset: 0x0004E6CD
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int GetUpperBound(int dimension)
		{
			return this.GetLowerBound(dimension) + this.GetLength(dimension) - 1;
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x000504E0 File Offset: 0x0004E6E0
		public object GetValue(int index)
		{
			if (this.Rank != 1)
			{
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
			}
			int lowerBound = this.GetLowerBound(0);
			if (index < lowerBound || index > this.GetUpperBound(0))
			{
				throw new IndexOutOfRangeException("Index has to be between upper and lower bound of the array.");
			}
			if (base.GetType().GetElementType().IsPointer)
			{
				throw new NotSupportedException("Type is not supported.");
			}
			return this.GetValueImpl(index - lowerBound);
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00050548 File Offset: 0x0004E748
		public object GetValue(int index1, int index2)
		{
			int[] indices = new int[]
			{
				index1,
				index2
			};
			return this.GetValue(indices);
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0005056C File Offset: 0x0004E76C
		public object GetValue(int index1, int index2, int index3)
		{
			int[] indices = new int[]
			{
				index1,
				index2,
				index3
			};
			return this.GetValue(indices);
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x00050594 File Offset: 0x0004E794
		public void SetValue(object value, int index)
		{
			if (this.Rank != 1)
			{
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
			}
			int lowerBound = this.GetLowerBound(0);
			if (index < lowerBound || index > this.GetUpperBound(0))
			{
				throw new IndexOutOfRangeException("Index has to be >= lower bound and <= upper bound of the array.");
			}
			if (base.GetType().GetElementType().IsPointer)
			{
				throw new NotSupportedException("Type is not supported.");
			}
			this.SetValueImpl(value, index - lowerBound);
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x00050600 File Offset: 0x0004E800
		public void SetValue(object value, int index1, int index2)
		{
			int[] indices = new int[]
			{
				index1,
				index2
			};
			this.SetValue(value, indices);
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x00050624 File Offset: 0x0004E824
		public void SetValue(object value, int index1, int index2, int index3)
		{
			int[] indices = new int[]
			{
				index1,
				index2,
				index3
			};
			this.SetValue(value, indices);
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0005064D File Offset: 0x0004E84D
		internal static Array UnsafeCreateInstance(Type elementType, int[] lengths, int[] lowerBounds)
		{
			return Array.CreateInstance(elementType, lengths, lowerBounds);
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x00050657 File Offset: 0x0004E857
		internal static Array UnsafeCreateInstance(Type elementType, int length1, int length2)
		{
			return Array.CreateInstance(elementType, length1, length2);
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x00050661 File Offset: 0x0004E861
		internal static Array UnsafeCreateInstance(Type elementType, params int[] lengths)
		{
			return Array.CreateInstance(elementType, lengths);
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0005066C File Offset: 0x0004E86C
		public static Array CreateInstance(Type elementType, int length)
		{
			int[] lengths = new int[]
			{
				length
			};
			return Array.CreateInstance(elementType, lengths);
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0005068C File Offset: 0x0004E88C
		public static Array CreateInstance(Type elementType, int length1, int length2)
		{
			int[] lengths = new int[]
			{
				length1,
				length2
			};
			return Array.CreateInstance(elementType, lengths);
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x000506B0 File Offset: 0x0004E8B0
		public static Array CreateInstance(Type elementType, int length1, int length2, int length3)
		{
			int[] lengths = new int[]
			{
				length1,
				length2,
				length3
			};
			return Array.CreateInstance(elementType, lengths);
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x000506D8 File Offset: 0x0004E8D8
		public static Array CreateInstance(Type elementType, params int[] lengths)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			if (lengths == null)
			{
				throw new ArgumentNullException("lengths");
			}
			if (lengths.Length > 255)
			{
				throw new TypeLoadException();
			}
			int[] bounds = null;
			elementType = (elementType.UnderlyingSystemType as RuntimeType);
			if (elementType == null)
			{
				throw new ArgumentException("Type must be a type provided by the runtime.", "elementType");
			}
			if (elementType.Equals(typeof(void)))
			{
				throw new NotSupportedException("Array type can not be void");
			}
			if (elementType.ContainsGenericParameters)
			{
				throw new NotSupportedException("Array type can not be an open generic type");
			}
			return Array.CreateInstanceImpl(elementType, lengths, bounds);
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x00050778 File Offset: 0x0004E978
		public static Array CreateInstance(Type elementType, int[] lengths, int[] lowerBounds)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			if (lengths == null)
			{
				throw new ArgumentNullException("lengths");
			}
			if (lowerBounds == null)
			{
				throw new ArgumentNullException("lowerBounds");
			}
			elementType = (elementType.UnderlyingSystemType as RuntimeType);
			if (elementType == null)
			{
				throw new ArgumentException("Type must be a type provided by the runtime.", "elementType");
			}
			if (elementType.Equals(typeof(void)))
			{
				throw new NotSupportedException("Array type can not be void");
			}
			if (elementType.ContainsGenericParameters)
			{
				throw new NotSupportedException("Array type can not be an open generic type");
			}
			if (lengths.Length < 1)
			{
				throw new ArgumentException("Arrays must contain >= 1 elements.");
			}
			if (lengths.Length != lowerBounds.Length)
			{
				throw new ArgumentException("Arrays must be of same size.");
			}
			for (int i = 0; i < lowerBounds.Length; i++)
			{
				if (lengths[i] < 0)
				{
					throw new ArgumentOutOfRangeException("lengths", "Each value has to be >= 0.");
				}
				if ((long)lowerBounds[i] + (long)lengths[i] > 2147483647L)
				{
					throw new ArgumentOutOfRangeException("lengths", "Length + bound must not exceed Int32.MaxValue.");
				}
			}
			if (lengths.Length > 255)
			{
				throw new TypeLoadException();
			}
			return Array.CreateInstanceImpl(elementType, lengths, lowerBounds);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0005088C File Offset: 0x0004EA8C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void Clear(Array array, int index, int length)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (length < 0)
			{
				throw new IndexOutOfRangeException("length < 0");
			}
			int lowerBound = array.GetLowerBound(0);
			if (index < lowerBound)
			{
				throw new IndexOutOfRangeException("index < lower bound");
			}
			index -= lowerBound;
			if (index > array.Length - length)
			{
				throw new IndexOutOfRangeException("index + length > size");
			}
			Array.ClearInternal(array, index, length);
		}

		// Token: 0x0600146A RID: 5226
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearInternal(Array a, int index, int count);

		// Token: 0x0600146B RID: 5227 RVA: 0x000508F0 File Offset: 0x0004EAF0
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Copy(Array sourceArray, Array destinationArray, int length)
		{
			if (sourceArray == null)
			{
				throw new ArgumentNullException("sourceArray");
			}
			if (destinationArray == null)
			{
				throw new ArgumentNullException("destinationArray");
			}
			Array.Copy(sourceArray, sourceArray.GetLowerBound(0), destinationArray, destinationArray.GetLowerBound(0), length);
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x00050924 File Offset: 0x0004EB24
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
		{
			if (sourceArray == null)
			{
				throw new ArgumentNullException("sourceArray");
			}
			if (destinationArray == null)
			{
				throw new ArgumentNullException("destinationArray");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Value has to be >= 0.");
			}
			if (sourceArray.Rank != destinationArray.Rank)
			{
				throw new RankException("Only single dimension arrays are supported here.");
			}
			if (sourceIndex < 0)
			{
				throw new ArgumentOutOfRangeException("sourceIndex", "Value has to be >= 0.");
			}
			if (destinationIndex < 0)
			{
				throw new ArgumentOutOfRangeException("destinationIndex", "Value has to be >= 0.");
			}
			if (Array.FastCopy(sourceArray, sourceIndex, destinationArray, destinationIndex, length))
			{
				return;
			}
			int num = sourceIndex - sourceArray.GetLowerBound(0);
			int num2 = destinationIndex - destinationArray.GetLowerBound(0);
			if (num2 < 0)
			{
				throw new ArgumentOutOfRangeException("destinationIndex", "Index was less than the array's lower bound in the first dimension.");
			}
			if (num > sourceArray.Length - length)
			{
				throw new ArgumentException("length");
			}
			if (num2 > destinationArray.Length - length)
			{
				throw new ArgumentException("Destination array was not long enough. Check destIndex and length, and the array's lower bounds", "destinationArray");
			}
			Type elementType = sourceArray.GetType().GetElementType();
			Type elementType2 = destinationArray.GetType().GetElementType();
			bool isValueType = elementType2.IsValueType;
			if (sourceArray != destinationArray || num > num2)
			{
				for (int i = 0; i < length; i++)
				{
					object valueImpl = sourceArray.GetValueImpl(num + i);
					if (valueImpl == null && isValueType)
					{
						throw new InvalidCastException();
					}
					try
					{
						destinationArray.SetValueImpl(valueImpl, num2 + i);
					}
					catch (ArgumentException)
					{
						throw Array.CreateArrayTypeMismatchException();
					}
					catch (InvalidCastException)
					{
						if (Array.CanAssignArrayElement(elementType, elementType2))
						{
							throw;
						}
						throw Array.CreateArrayTypeMismatchException();
					}
				}
				return;
			}
			for (int j = length - 1; j >= 0; j--)
			{
				object valueImpl2 = sourceArray.GetValueImpl(num + j);
				try
				{
					destinationArray.SetValueImpl(valueImpl2, num2 + j);
				}
				catch (ArgumentException)
				{
					throw Array.CreateArrayTypeMismatchException();
				}
				catch
				{
					if (Array.CanAssignArrayElement(elementType, elementType2))
					{
						throw;
					}
					throw Array.CreateArrayTypeMismatchException();
				}
			}
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0004DBDB File Offset: 0x0004BDDB
		private static ArrayTypeMismatchException CreateArrayTypeMismatchException()
		{
			return new ArrayTypeMismatchException();
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x00050B04 File Offset: 0x0004ED04
		private static bool CanAssignArrayElement(Type source, Type target)
		{
			if (source.IsValueType)
			{
				return source.IsAssignableFrom(target);
			}
			if (source.IsInterface)
			{
				return !target.IsValueType;
			}
			if (target.IsInterface)
			{
				return !source.IsValueType;
			}
			return source.IsAssignableFrom(target) || target.IsAssignableFrom(source);
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x00050B57 File Offset: 0x0004ED57
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void ConstrainedCopy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
		{
			Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x00050B64 File Offset: 0x0004ED64
		public static T[] Empty<T>()
		{
			return EmptyArray<T>.Value;
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Initialize()
		{
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x00050B6B File Offset: 0x0004ED6B
		private static int IndexOfImpl<T>(T[] array, T value, int startIndex, int count)
		{
			return EqualityComparer<T>.Default.IndexOf(array, value, startIndex, count);
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x00050B7B File Offset: 0x0004ED7B
		private static int LastIndexOfImpl<T>(T[] array, T value, int startIndex, int count)
		{
			return EqualityComparer<T>.Default.LastIndexOf(array, value, startIndex, count);
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x00050B8C File Offset: 0x0004ED8C
		private static void SortImpl(Array keys, Array items, int index, int length, IComparer comparer)
		{
			object[] array = keys as object[];
			object[] array2 = null;
			if (array != null)
			{
				array2 = (items as object[]);
			}
			if (array != null && (items == null || array2 != null))
			{
				Array.SorterObjectArray sorterObjectArray = new Array.SorterObjectArray(array, array2, comparer);
				sorterObjectArray.Sort(index, length);
				return;
			}
			Array.SorterGenericArray sorterGenericArray = new Array.SorterGenericArray(keys, items, comparer);
			sorterGenericArray.Sort(index, length);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00050BDE File Offset: 0x0004EDDE
		internal static T UnsafeLoad<T>(T[] array, int index)
		{
			return array[index];
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x00050BE7 File Offset: 0x0004EDE7
		internal static void UnsafeStore<T>(T[] array, int index, T value)
		{
			array[index] = value;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00050BF1 File Offset: 0x0004EDF1
		internal static R UnsafeMov<S, R>(S instance)
		{
			return (R)((object)instance);
		}

		// Token: 0x020001D2 RID: 466
		private sealed class ArrayEnumerator : IEnumerator, ICloneable
		{
			// Token: 0x06001478 RID: 5240 RVA: 0x00050BFE File Offset: 0x0004EDFE
			internal ArrayEnumerator(Array array)
			{
				this._array = array;
				this._index = -1;
				this._endIndex = array.Length;
			}

			// Token: 0x06001479 RID: 5241 RVA: 0x00050C20 File Offset: 0x0004EE20
			public bool MoveNext()
			{
				if (this._index < this._endIndex)
				{
					this._index++;
					return this._index < this._endIndex;
				}
				return false;
			}

			// Token: 0x0600147A RID: 5242 RVA: 0x00050C4E File Offset: 0x0004EE4E
			public void Reset()
			{
				this._index = -1;
			}

			// Token: 0x0600147B RID: 5243 RVA: 0x000231D1 File Offset: 0x000213D1
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x170001E9 RID: 489
			// (get) Token: 0x0600147C RID: 5244 RVA: 0x00050C58 File Offset: 0x0004EE58
			public object Current
			{
				get
				{
					if (this._index < 0)
					{
						throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
					}
					if (this._index >= this._endIndex)
					{
						throw new InvalidOperationException("Enumeration already finished.");
					}
					if (this._index == 0 && this._array.GetType().GetElementType().IsPointer)
					{
						throw new NotSupportedException("Type is not supported.");
					}
					return this._array.GetValueImpl(this._index);
				}
			}

			// Token: 0x0400145C RID: 5212
			private Array _array;

			// Token: 0x0400145D RID: 5213
			private int _index;

			// Token: 0x0400145E RID: 5214
			private int _endIndex;
		}

		// Token: 0x020001D3 RID: 467
		[StructLayout(LayoutKind.Sequential)]
		private class RawData
		{
			// Token: 0x0400145F RID: 5215
			public IntPtr Bounds;

			// Token: 0x04001460 RID: 5216
			public IntPtr Count;

			// Token: 0x04001461 RID: 5217
			public byte Data;
		}

		// Token: 0x020001D4 RID: 468
		internal struct InternalEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x0600147E RID: 5246 RVA: 0x00050CCD File Offset: 0x0004EECD
			internal InternalEnumerator(Array array)
			{
				this.array = array;
				this.idx = -2;
			}

			// Token: 0x0600147F RID: 5247 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public void Dispose()
			{
			}

			// Token: 0x06001480 RID: 5248 RVA: 0x00050CE0 File Offset: 0x0004EEE0
			public bool MoveNext()
			{
				if (this.idx == -2)
				{
					this.idx = this.array.Length;
				}
				if (this.idx != -1)
				{
					int num = this.idx - 1;
					this.idx = num;
					return num != -1;
				}
				return false;
			}

			// Token: 0x170001EA RID: 490
			// (get) Token: 0x06001481 RID: 5249 RVA: 0x00050D2C File Offset: 0x0004EF2C
			public T Current
			{
				get
				{
					if (this.idx == -2)
					{
						throw new InvalidOperationException("Enumeration has not started. Call MoveNext");
					}
					if (this.idx == -1)
					{
						throw new InvalidOperationException("Enumeration already finished");
					}
					return this.array.InternalArray__get_Item<T>(this.array.Length - 1 - this.idx);
				}
			}

			// Token: 0x06001482 RID: 5250 RVA: 0x00050D81 File Offset: 0x0004EF81
			void IEnumerator.Reset()
			{
				this.idx = -2;
			}

			// Token: 0x170001EB RID: 491
			// (get) Token: 0x06001483 RID: 5251 RVA: 0x00050D8B File Offset: 0x0004EF8B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x04001462 RID: 5218
			private const int NOT_STARTED = -2;

			// Token: 0x04001463 RID: 5219
			private const int FINISHED = -1;

			// Token: 0x04001464 RID: 5220
			private readonly Array array;

			// Token: 0x04001465 RID: 5221
			private int idx;
		}

		// Token: 0x020001D5 RID: 469
		internal class EmptyInternalEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x06001484 RID: 5252 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public void Dispose()
			{
			}

			// Token: 0x06001485 RID: 5253 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			public bool MoveNext()
			{
				return false;
			}

			// Token: 0x170001EC RID: 492
			// (get) Token: 0x06001486 RID: 5254 RVA: 0x00050D98 File Offset: 0x0004EF98
			public T Current
			{
				get
				{
					throw new InvalidOperationException("Enumeration has not started. Call MoveNext");
				}
			}

			// Token: 0x170001ED RID: 493
			// (get) Token: 0x06001487 RID: 5255 RVA: 0x00050DA4 File Offset: 0x0004EFA4
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06001488 RID: 5256 RVA: 0x00004BF9 File Offset: 0x00002DF9
			void IEnumerator.Reset()
			{
			}

			// Token: 0x04001466 RID: 5222
			public static readonly Array.EmptyInternalEnumerator<T> Value = new Array.EmptyInternalEnumerator<T>();
		}

		// Token: 0x020001D6 RID: 470
		internal sealed class FunctorComparer<T> : IComparer<T>
		{
			// Token: 0x0600148B RID: 5259 RVA: 0x00050DBD File Offset: 0x0004EFBD
			public FunctorComparer(Comparison<T> comparison)
			{
				this.comparison = comparison;
			}

			// Token: 0x0600148C RID: 5260 RVA: 0x00050DCC File Offset: 0x0004EFCC
			public int Compare(T x, T y)
			{
				return this.comparison(x, y);
			}

			// Token: 0x04001467 RID: 5223
			private Comparison<T> comparison;
		}

		// Token: 0x020001D7 RID: 471
		private struct SorterObjectArray
		{
			// Token: 0x0600148D RID: 5261 RVA: 0x00050DDB File Offset: 0x0004EFDB
			internal SorterObjectArray(object[] keys, object[] items, IComparer comparer)
			{
				if (comparer == null)
				{
					comparer = Comparer.Default;
				}
				this.keys = keys;
				this.items = items;
				this.comparer = comparer;
			}

			// Token: 0x0600148E RID: 5262 RVA: 0x00050DFC File Offset: 0x0004EFFC
			internal void SwapIfGreaterWithItems(int a, int b)
			{
				if (a != b && this.comparer.Compare(this.keys[a], this.keys[b]) > 0)
				{
					object obj = this.keys[a];
					this.keys[a] = this.keys[b];
					this.keys[b] = obj;
					if (this.items != null)
					{
						object obj2 = this.items[a];
						this.items[a] = this.items[b];
						this.items[b] = obj2;
					}
				}
			}

			// Token: 0x0600148F RID: 5263 RVA: 0x00050E78 File Offset: 0x0004F078
			private void Swap(int i, int j)
			{
				object obj = this.keys[i];
				this.keys[i] = this.keys[j];
				this.keys[j] = obj;
				if (this.items != null)
				{
					object obj2 = this.items[i];
					this.items[i] = this.items[j];
					this.items[j] = obj2;
				}
			}

			// Token: 0x06001490 RID: 5264 RVA: 0x00050ED1 File Offset: 0x0004F0D1
			internal void Sort(int left, int length)
			{
				this.IntrospectiveSort(left, length);
			}

			// Token: 0x06001491 RID: 5265 RVA: 0x00050EDC File Offset: 0x0004F0DC
			private void IntrospectiveSort(int left, int length)
			{
				if (length < 2)
				{
					return;
				}
				try
				{
					this.IntroSort(left, length + left - 1, 2 * IntrospectiveSortUtilities.FloorLog2PlusOne(this.keys.Length));
				}
				catch (IndexOutOfRangeException)
				{
					IntrospectiveSortUtilities.ThrowOrIgnoreBadComparer(this.comparer);
				}
				catch (Exception innerException)
				{
					throw new InvalidOperationException("Failed to compare two elements in the array.", innerException);
				}
			}

			// Token: 0x06001492 RID: 5266 RVA: 0x00050F44 File Offset: 0x0004F144
			private void IntroSort(int lo, int hi, int depthLimit)
			{
				while (hi > lo)
				{
					int num = hi - lo + 1;
					if (num <= 16)
					{
						if (num == 1)
						{
							return;
						}
						if (num == 2)
						{
							this.SwapIfGreaterWithItems(lo, hi);
							return;
						}
						if (num == 3)
						{
							this.SwapIfGreaterWithItems(lo, hi - 1);
							this.SwapIfGreaterWithItems(lo, hi);
							this.SwapIfGreaterWithItems(hi - 1, hi);
							return;
						}
						this.InsertionSort(lo, hi);
						return;
					}
					else
					{
						if (depthLimit == 0)
						{
							this.Heapsort(lo, hi);
							return;
						}
						depthLimit--;
						int num2 = this.PickPivotAndPartition(lo, hi);
						this.IntroSort(num2 + 1, hi, depthLimit);
						hi = num2 - 1;
					}
				}
			}

			// Token: 0x06001493 RID: 5267 RVA: 0x00050FC8 File Offset: 0x0004F1C8
			private int PickPivotAndPartition(int lo, int hi)
			{
				int num = lo + (hi - lo) / 2;
				this.SwapIfGreaterWithItems(lo, num);
				this.SwapIfGreaterWithItems(lo, hi);
				this.SwapIfGreaterWithItems(num, hi);
				object obj = this.keys[num];
				this.Swap(num, hi - 1);
				int i = lo;
				int num2 = hi - 1;
				while (i < num2)
				{
					while (this.comparer.Compare(this.keys[++i], obj) < 0)
					{
					}
					while (this.comparer.Compare(obj, this.keys[--num2]) < 0)
					{
					}
					if (i >= num2)
					{
						break;
					}
					this.Swap(i, num2);
				}
				this.Swap(i, hi - 1);
				return i;
			}

			// Token: 0x06001494 RID: 5268 RVA: 0x00051064 File Offset: 0x0004F264
			private void Heapsort(int lo, int hi)
			{
				int num = hi - lo + 1;
				for (int i = num / 2; i >= 1; i--)
				{
					this.DownHeap(i, num, lo);
				}
				for (int j = num; j > 1; j--)
				{
					this.Swap(lo, lo + j - 1);
					this.DownHeap(1, j - 1, lo);
				}
			}

			// Token: 0x06001495 RID: 5269 RVA: 0x000510B4 File Offset: 0x0004F2B4
			private void DownHeap(int i, int n, int lo)
			{
				object obj = this.keys[lo + i - 1];
				object obj2 = (this.items != null) ? this.items[lo + i - 1] : null;
				while (i <= n / 2)
				{
					int num = 2 * i;
					if (num < n && this.comparer.Compare(this.keys[lo + num - 1], this.keys[lo + num]) < 0)
					{
						num++;
					}
					if (this.comparer.Compare(obj, this.keys[lo + num - 1]) >= 0)
					{
						break;
					}
					this.keys[lo + i - 1] = this.keys[lo + num - 1];
					if (this.items != null)
					{
						this.items[lo + i - 1] = this.items[lo + num - 1];
					}
					i = num;
				}
				this.keys[lo + i - 1] = obj;
				if (this.items != null)
				{
					this.items[lo + i - 1] = obj2;
				}
			}

			// Token: 0x06001496 RID: 5270 RVA: 0x0005119C File Offset: 0x0004F39C
			private void InsertionSort(int lo, int hi)
			{
				for (int i = lo; i < hi; i++)
				{
					int num = i;
					object obj = this.keys[i + 1];
					object obj2 = (this.items != null) ? this.items[i + 1] : null;
					while (num >= lo && this.comparer.Compare(obj, this.keys[num]) < 0)
					{
						this.keys[num + 1] = this.keys[num];
						if (this.items != null)
						{
							this.items[num + 1] = this.items[num];
						}
						num--;
					}
					this.keys[num + 1] = obj;
					if (this.items != null)
					{
						this.items[num + 1] = obj2;
					}
				}
			}

			// Token: 0x04001468 RID: 5224
			private object[] keys;

			// Token: 0x04001469 RID: 5225
			private object[] items;

			// Token: 0x0400146A RID: 5226
			private IComparer comparer;
		}

		// Token: 0x020001D8 RID: 472
		private struct SorterGenericArray
		{
			// Token: 0x06001497 RID: 5271 RVA: 0x00051249 File Offset: 0x0004F449
			internal SorterGenericArray(Array keys, Array items, IComparer comparer)
			{
				if (comparer == null)
				{
					comparer = Comparer.Default;
				}
				this.keys = keys;
				this.items = items;
				this.comparer = comparer;
			}

			// Token: 0x06001498 RID: 5272 RVA: 0x0005126C File Offset: 0x0004F46C
			internal void SwapIfGreaterWithItems(int a, int b)
			{
				if (a != b && this.comparer.Compare(this.keys.GetValue(a), this.keys.GetValue(b)) > 0)
				{
					object value = this.keys.GetValue(a);
					this.keys.SetValue(this.keys.GetValue(b), a);
					this.keys.SetValue(value, b);
					if (this.items != null)
					{
						object value2 = this.items.GetValue(a);
						this.items.SetValue(this.items.GetValue(b), a);
						this.items.SetValue(value2, b);
					}
				}
			}

			// Token: 0x06001499 RID: 5273 RVA: 0x00051314 File Offset: 0x0004F514
			private void Swap(int i, int j)
			{
				object value = this.keys.GetValue(i);
				this.keys.SetValue(this.keys.GetValue(j), i);
				this.keys.SetValue(value, j);
				if (this.items != null)
				{
					object value2 = this.items.GetValue(i);
					this.items.SetValue(this.items.GetValue(j), i);
					this.items.SetValue(value2, j);
				}
			}

			// Token: 0x0600149A RID: 5274 RVA: 0x0005138D File Offset: 0x0004F58D
			internal void Sort(int left, int length)
			{
				this.IntrospectiveSort(left, length);
			}

			// Token: 0x0600149B RID: 5275 RVA: 0x00051398 File Offset: 0x0004F598
			private void IntrospectiveSort(int left, int length)
			{
				if (length < 2)
				{
					return;
				}
				try
				{
					this.IntroSort(left, length + left - 1, 2 * IntrospectiveSortUtilities.FloorLog2PlusOne(this.keys.Length));
				}
				catch (IndexOutOfRangeException)
				{
					IntrospectiveSortUtilities.ThrowOrIgnoreBadComparer(this.comparer);
				}
				catch (Exception innerException)
				{
					throw new InvalidOperationException("Failed to compare two elements in the array.", innerException);
				}
			}

			// Token: 0x0600149C RID: 5276 RVA: 0x00051404 File Offset: 0x0004F604
			private void IntroSort(int lo, int hi, int depthLimit)
			{
				while (hi > lo)
				{
					int num = hi - lo + 1;
					if (num <= 16)
					{
						if (num == 1)
						{
							return;
						}
						if (num == 2)
						{
							this.SwapIfGreaterWithItems(lo, hi);
							return;
						}
						if (num == 3)
						{
							this.SwapIfGreaterWithItems(lo, hi - 1);
							this.SwapIfGreaterWithItems(lo, hi);
							this.SwapIfGreaterWithItems(hi - 1, hi);
							return;
						}
						this.InsertionSort(lo, hi);
						return;
					}
					else
					{
						if (depthLimit == 0)
						{
							this.Heapsort(lo, hi);
							return;
						}
						depthLimit--;
						int num2 = this.PickPivotAndPartition(lo, hi);
						this.IntroSort(num2 + 1, hi, depthLimit);
						hi = num2 - 1;
					}
				}
			}

			// Token: 0x0600149D RID: 5277 RVA: 0x00051488 File Offset: 0x0004F688
			private int PickPivotAndPartition(int lo, int hi)
			{
				int num = lo + (hi - lo) / 2;
				this.SwapIfGreaterWithItems(lo, num);
				this.SwapIfGreaterWithItems(lo, hi);
				this.SwapIfGreaterWithItems(num, hi);
				object value = this.keys.GetValue(num);
				this.Swap(num, hi - 1);
				int i = lo;
				int num2 = hi - 1;
				while (i < num2)
				{
					while (this.comparer.Compare(this.keys.GetValue(++i), value) < 0)
					{
					}
					while (this.comparer.Compare(value, this.keys.GetValue(--num2)) < 0)
					{
					}
					if (i >= num2)
					{
						break;
					}
					this.Swap(i, num2);
				}
				this.Swap(i, hi - 1);
				return i;
			}

			// Token: 0x0600149E RID: 5278 RVA: 0x00051530 File Offset: 0x0004F730
			private void Heapsort(int lo, int hi)
			{
				int num = hi - lo + 1;
				for (int i = num / 2; i >= 1; i--)
				{
					this.DownHeap(i, num, lo);
				}
				for (int j = num; j > 1; j--)
				{
					this.Swap(lo, lo + j - 1);
					this.DownHeap(1, j - 1, lo);
				}
			}

			// Token: 0x0600149F RID: 5279 RVA: 0x00051580 File Offset: 0x0004F780
			private void DownHeap(int i, int n, int lo)
			{
				object value = this.keys.GetValue(lo + i - 1);
				object value2 = (this.items != null) ? this.items.GetValue(lo + i - 1) : null;
				while (i <= n / 2)
				{
					int num = 2 * i;
					if (num < n && this.comparer.Compare(this.keys.GetValue(lo + num - 1), this.keys.GetValue(lo + num)) < 0)
					{
						num++;
					}
					if (this.comparer.Compare(value, this.keys.GetValue(lo + num - 1)) >= 0)
					{
						break;
					}
					this.keys.SetValue(this.keys.GetValue(lo + num - 1), lo + i - 1);
					if (this.items != null)
					{
						this.items.SetValue(this.items.GetValue(lo + num - 1), lo + i - 1);
					}
					i = num;
				}
				this.keys.SetValue(value, lo + i - 1);
				if (this.items != null)
				{
					this.items.SetValue(value2, lo + i - 1);
				}
			}

			// Token: 0x060014A0 RID: 5280 RVA: 0x00051694 File Offset: 0x0004F894
			private void InsertionSort(int lo, int hi)
			{
				for (int i = lo; i < hi; i++)
				{
					int num = i;
					object value = this.keys.GetValue(i + 1);
					object value2 = (this.items != null) ? this.items.GetValue(i + 1) : null;
					while (num >= lo && this.comparer.Compare(value, this.keys.GetValue(num)) < 0)
					{
						this.keys.SetValue(this.keys.GetValue(num), num + 1);
						if (this.items != null)
						{
							this.items.SetValue(this.items.GetValue(num), num + 1);
						}
						num--;
					}
					this.keys.SetValue(value, num + 1);
					if (this.items != null)
					{
						this.items.SetValue(value2, num + 1);
					}
				}
			}

			// Token: 0x0400146B RID: 5227
			private Array keys;

			// Token: 0x0400146C RID: 5228
			private Array items;

			// Token: 0x0400146D RID: 5229
			private IComparer comparer;
		}
	}
}
