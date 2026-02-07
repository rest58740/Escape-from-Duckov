using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x02000AA3 RID: 2723
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(ICollectionDebugView<>))]
	[Serializable]
	public class List<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IReadOnlyList<!0>, IReadOnlyCollection<T>
	{
		// Token: 0x06006142 RID: 24898 RVA: 0x001452CE File Offset: 0x001434CE
		public List()
		{
			this._items = List<T>.s_emptyArray;
		}

		// Token: 0x06006143 RID: 24899 RVA: 0x001452E1 File Offset: 0x001434E1
		public List(int capacity)
		{
			if (capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (capacity == 0)
			{
				this._items = List<T>.s_emptyArray;
				return;
			}
			this._items = new T[capacity];
		}

		// Token: 0x06006144 RID: 24900 RVA: 0x00145310 File Offset: 0x00143510
		public List(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			ICollection<T> collection2 = collection as ICollection<!0>;
			if (collection2 == null)
			{
				this._size = 0;
				this._items = List<T>.s_emptyArray;
				this.AddEnumerable(collection);
				return;
			}
			int count = collection2.Count;
			if (count == 0)
			{
				this._items = List<T>.s_emptyArray;
				return;
			}
			this._items = new T[count];
			collection2.CopyTo(this._items, 0);
			this._size = count;
		}

		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x06006145 RID: 24901 RVA: 0x00145386 File Offset: 0x00143586
		// (set) Token: 0x06006146 RID: 24902 RVA: 0x00145390 File Offset: 0x00143590
		public int Capacity
		{
			get
			{
				return this._items.Length;
			}
			set
			{
				if (value < this._size)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
				}
				if (value != this._items.Length)
				{
					if (value > 0)
					{
						T[] array = new T[value];
						if (this._size > 0)
						{
							Array.Copy(this._items, 0, array, 0, this._size);
						}
						this._items = array;
						return;
					}
					this._items = List<T>.s_emptyArray;
				}
			}
		}

		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x06006147 RID: 24903 RVA: 0x001453F5 File Offset: 0x001435F5
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x06006148 RID: 24904 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x06006149 RID: 24905 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x0600614A RID: 24906 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x0600614B RID: 24907 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x0600614C RID: 24908 RVA: 0x001453FD File Offset: 0x001435FD
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x17001155 RID: 4437
		public T this[int index]
		{
			get
			{
				if (index >= this._size)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				return this._items[index];
			}
			set
			{
				if (index >= this._size)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				this._items[index] = value;
				this._version++;
			}
		}

		// Token: 0x0600614F RID: 24911 RVA: 0x00145468 File Offset: 0x00143668
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && default(T) == null);
		}

		// Token: 0x17001156 RID: 4438
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, ExceptionArgument.value);
				try
				{
					this[index] = (T)((object)value);
				}
				catch (InvalidCastException)
				{
					ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
				}
			}
		}

		// Token: 0x06006152 RID: 24914 RVA: 0x001454EC File Offset: 0x001436EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(T item)
		{
			this._version++;
			T[] items = this._items;
			int size = this._size;
			if (size < items.Length)
			{
				this._size = size + 1;
				items[size] = item;
				return;
			}
			this.AddWithResize(item);
		}

		// Token: 0x06006153 RID: 24915 RVA: 0x00145534 File Offset: 0x00143734
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void AddWithResize(T item)
		{
			int size = this._size;
			this.EnsureCapacity(size + 1);
			this._size = size + 1;
			this._items[size] = item;
		}

		// Token: 0x06006154 RID: 24916 RVA: 0x00145568 File Offset: 0x00143768
		int IList.Add(object item)
		{
			ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(item, ExceptionArgument.item);
			try
			{
				this.Add((T)((object)item));
			}
			catch (InvalidCastException)
			{
				ThrowHelper.ThrowWrongValueTypeArgumentException(item, typeof(T));
			}
			return this.Count - 1;
		}

		// Token: 0x06006155 RID: 24917 RVA: 0x001455B8 File Offset: 0x001437B8
		public void AddRange(IEnumerable<T> collection)
		{
			this.InsertRange(this._size, collection);
		}

		// Token: 0x06006156 RID: 24918 RVA: 0x001455C7 File Offset: 0x001437C7
		public ReadOnlyCollection<T> AsReadOnly()
		{
			return new ReadOnlyCollection<T>(this);
		}

		// Token: 0x06006157 RID: 24919 RVA: 0x001455CF File Offset: 0x001437CF
		public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
		{
			if (index < 0)
			{
				ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
			}
			if (count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			return Array.BinarySearch<T>(this._items, index, count, item, comparer);
		}

		// Token: 0x06006158 RID: 24920 RVA: 0x00145608 File Offset: 0x00143808
		public int BinarySearch(T item)
		{
			return this.BinarySearch(0, this.Count, item, null);
		}

		// Token: 0x06006159 RID: 24921 RVA: 0x00145619 File Offset: 0x00143819
		public int BinarySearch(T item, IComparer<T> comparer)
		{
			return this.BinarySearch(0, this.Count, item, comparer);
		}

		// Token: 0x0600615A RID: 24922 RVA: 0x0014562C File Offset: 0x0014382C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear()
		{
			this._version++;
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				int size = this._size;
				this._size = 0;
				if (size > 0)
				{
					Array.Clear(this._items, 0, size);
					return;
				}
			}
			else
			{
				this._size = 0;
			}
		}

		// Token: 0x0600615B RID: 24923 RVA: 0x00145675 File Offset: 0x00143875
		public bool Contains(T item)
		{
			return this._size != 0 && this.IndexOf(item) != -1;
		}

		// Token: 0x0600615C RID: 24924 RVA: 0x0014568E File Offset: 0x0014388E
		bool IList.Contains(object item)
		{
			return List<T>.IsCompatibleObject(item) && this.Contains((T)((object)item));
		}

		// Token: 0x0600615D RID: 24925 RVA: 0x001456A8 File Offset: 0x001438A8
		public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
		{
			if (converter == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.converter);
			}
			List<TOutput> list = new List<TOutput>(this._size);
			for (int i = 0; i < this._size; i++)
			{
				list._items[i] = converter(this._items[i]);
			}
			list._size = this._size;
			return list;
		}

		// Token: 0x0600615E RID: 24926 RVA: 0x00145707 File Offset: 0x00143907
		public void CopyTo(T[] array)
		{
			this.CopyTo(array, 0);
		}

		// Token: 0x0600615F RID: 24927 RVA: 0x00145714 File Offset: 0x00143914
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			if (array != null && array.Rank != 1)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			try
			{
				Array.Copy(this._items, 0, array, arrayIndex, this._size);
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
			}
		}

		// Token: 0x06006160 RID: 24928 RVA: 0x00145764 File Offset: 0x00143964
		public void CopyTo(int index, T[] array, int arrayIndex, int count)
		{
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			Array.Copy(this._items, index, array, arrayIndex, count);
		}

		// Token: 0x06006161 RID: 24929 RVA: 0x00145789 File Offset: 0x00143989
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this._items, 0, array, arrayIndex, this._size);
		}

		// Token: 0x06006162 RID: 24930 RVA: 0x001457A0 File Offset: 0x001439A0
		private void EnsureCapacity(int min)
		{
			if (this._items.Length < min)
			{
				int num = (this._items.Length == 0) ? 4 : (this._items.Length * 2);
				if (num > 2146435071)
				{
					num = 2146435071;
				}
				if (num < min)
				{
					num = min;
				}
				this.Capacity = num;
			}
		}

		// Token: 0x06006163 RID: 24931 RVA: 0x001457EA File Offset: 0x001439EA
		public bool Exists(Predicate<T> match)
		{
			return this.FindIndex(match) != -1;
		}

		// Token: 0x06006164 RID: 24932 RVA: 0x001457FC File Offset: 0x001439FC
		public T Find(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = 0; i < this._size; i++)
			{
				if (match(this._items[i]))
				{
					return this._items[i];
				}
			}
			return default(T);
		}

		// Token: 0x06006165 RID: 24933 RVA: 0x00145850 File Offset: 0x00143A50
		public List<T> FindAll(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			List<T> list = new List<T>();
			for (int i = 0; i < this._size; i++)
			{
				if (match(this._items[i]))
				{
					list.Add(this._items[i]);
				}
			}
			return list;
		}

		// Token: 0x06006166 RID: 24934 RVA: 0x001458A4 File Offset: 0x00143AA4
		public int FindIndex(Predicate<T> match)
		{
			return this.FindIndex(0, this._size, match);
		}

		// Token: 0x06006167 RID: 24935 RVA: 0x001458B4 File Offset: 0x00143AB4
		public int FindIndex(int startIndex, Predicate<T> match)
		{
			return this.FindIndex(startIndex, this._size - startIndex, match);
		}

		// Token: 0x06006168 RID: 24936 RVA: 0x001458C8 File Offset: 0x00143AC8
		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			if (startIndex > this._size)
			{
				ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index();
			}
			if (count < 0 || startIndex > this._size - count)
			{
				ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
			}
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (match(this._items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06006169 RID: 24937 RVA: 0x00145928 File Offset: 0x00143B28
		public T FindLast(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = this._size - 1; i >= 0; i--)
			{
				if (match(this._items[i]))
				{
					return this._items[i];
				}
			}
			return default(T);
		}

		// Token: 0x0600616A RID: 24938 RVA: 0x0014597B File Offset: 0x00143B7B
		public int FindLastIndex(Predicate<T> match)
		{
			return this.FindLastIndex(this._size - 1, this._size, match);
		}

		// Token: 0x0600616B RID: 24939 RVA: 0x00145992 File Offset: 0x00143B92
		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			return this.FindLastIndex(startIndex, startIndex + 1, match);
		}

		// Token: 0x0600616C RID: 24940 RVA: 0x001459A0 File Offset: 0x00143BA0
		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			if (this._size == 0)
			{
				if (startIndex != -1)
				{
					ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index();
				}
			}
			else if (startIndex >= this._size)
			{
				ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index();
			}
			if (count < 0 || startIndex - count + 1 < 0)
			{
				ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
			}
			int num = startIndex - count;
			for (int i = startIndex; i > num; i--)
			{
				if (match(this._items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600616D RID: 24941 RVA: 0x00145A10 File Offset: 0x00143C10
		public void ForEach(Action<T> action)
		{
			if (action == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.action);
			}
			int version = this._version;
			int num = 0;
			while (num < this._size && version == this._version)
			{
				action(this._items[num]);
				num++;
			}
			if (version != this._version)
			{
				ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
			}
		}

		// Token: 0x0600616E RID: 24942 RVA: 0x00145A68 File Offset: 0x00143C68
		public List<T>.Enumerator GetEnumerator()
		{
			return new List<T>.Enumerator(this);
		}

		// Token: 0x0600616F RID: 24943 RVA: 0x00145A70 File Offset: 0x00143C70
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return new List<T>.Enumerator(this);
		}

		// Token: 0x06006170 RID: 24944 RVA: 0x00145A70 File Offset: 0x00143C70
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new List<T>.Enumerator(this);
		}

		// Token: 0x06006171 RID: 24945 RVA: 0x00145A80 File Offset: 0x00143C80
		public List<T> GetRange(int index, int count)
		{
			if (index < 0)
			{
				ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
			}
			if (count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			List<T> list = new List<T>(count);
			Array.Copy(this._items, index, list._items, 0, count);
			list._size = count;
			return list;
		}

		// Token: 0x06006172 RID: 24946 RVA: 0x00145AD7 File Offset: 0x00143CD7
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this._items, item, 0, this._size);
		}

		// Token: 0x06006173 RID: 24947 RVA: 0x00145AEC File Offset: 0x00143CEC
		int IList.IndexOf(object item)
		{
			if (List<T>.IsCompatibleObject(item))
			{
				return this.IndexOf((T)((object)item));
			}
			return -1;
		}

		// Token: 0x06006174 RID: 24948 RVA: 0x00145B04 File Offset: 0x00143D04
		public int IndexOf(T item, int index)
		{
			if (index > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRange_IndexException();
			}
			return Array.IndexOf<T>(this._items, item, index, this._size - index);
		}

		// Token: 0x06006175 RID: 24949 RVA: 0x00145B29 File Offset: 0x00143D29
		public int IndexOf(T item, int index, int count)
		{
			if (index > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRange_IndexException();
			}
			if (count < 0 || index > this._size - count)
			{
				ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
			}
			return Array.IndexOf<T>(this._items, item, index, count);
		}

		// Token: 0x06006176 RID: 24950 RVA: 0x00145B5C File Offset: 0x00143D5C
		public void Insert(int index, T item)
		{
			if (index > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_ListInsert);
			}
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			if (index < this._size)
			{
				Array.Copy(this._items, index, this._items, index + 1, this._size - index);
			}
			this._items[index] = item;
			this._size++;
			this._version++;
		}

		// Token: 0x06006177 RID: 24951 RVA: 0x00145BE8 File Offset: 0x00143DE8
		void IList.Insert(int index, object item)
		{
			ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(item, ExceptionArgument.item);
			try
			{
				this.Insert(index, (T)((object)item));
			}
			catch (InvalidCastException)
			{
				ThrowHelper.ThrowWrongValueTypeArgumentException(item, typeof(T));
			}
		}

		// Token: 0x06006178 RID: 24952 RVA: 0x00145C30 File Offset: 0x00143E30
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			if (index > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRange_IndexException();
			}
			ICollection<T> collection2 = collection as ICollection<!0>;
			if (collection2 != null)
			{
				int count = collection2.Count;
				if (count > 0)
				{
					this.EnsureCapacity(this._size + count);
					if (index < this._size)
					{
						Array.Copy(this._items, index, this._items, index + count, this._size - index);
					}
					if (this == collection2)
					{
						Array.Copy(this._items, 0, this._items, index, index);
						Array.Copy(this._items, index + count, this._items, index * 2, this._size - index);
					}
					else
					{
						collection2.CopyTo(this._items, index);
					}
					this._size += count;
				}
			}
			else
			{
				if (index < this._size)
				{
					using (IEnumerator<T> enumerator = collection.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							!0 item = enumerator.Current;
							this.Insert(index++, item);
						}
						goto IL_FB;
					}
				}
				this.AddEnumerable(collection);
			}
			IL_FB:
			this._version++;
		}

		// Token: 0x06006179 RID: 24953 RVA: 0x00145D58 File Offset: 0x00143F58
		public int LastIndexOf(T item)
		{
			if (this._size == 0)
			{
				return -1;
			}
			return this.LastIndexOf(item, this._size - 1, this._size);
		}

		// Token: 0x0600617A RID: 24954 RVA: 0x00145D79 File Offset: 0x00143F79
		public int LastIndexOf(T item, int index)
		{
			if (index >= this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRange_IndexException();
			}
			return this.LastIndexOf(item, index, index + 1);
		}

		// Token: 0x0600617B RID: 24955 RVA: 0x00145D94 File Offset: 0x00143F94
		public int LastIndexOf(T item, int index, int count)
		{
			if (this.Count != 0 && index < 0)
			{
				ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
			}
			if (this.Count != 0 && count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size == 0)
			{
				return -1;
			}
			if (index >= this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
			}
			if (count > index + 1)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
			}
			return Array.LastIndexOf<T>(this._items, item, index, count);
		}

		// Token: 0x0600617C RID: 24956 RVA: 0x00145E00 File Offset: 0x00144000
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num >= 0)
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x0600617D RID: 24957 RVA: 0x00145E23 File Offset: 0x00144023
		void IList.Remove(object item)
		{
			if (List<T>.IsCompatibleObject(item))
			{
				this.Remove((T)((object)item));
			}
		}

		// Token: 0x0600617E RID: 24958 RVA: 0x00145E3C File Offset: 0x0014403C
		public int RemoveAll(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			int num = 0;
			while (num < this._size && !match(this._items[num]))
			{
				num++;
			}
			if (num >= this._size)
			{
				return 0;
			}
			int i = num + 1;
			while (i < this._size)
			{
				while (i < this._size && match(this._items[i]))
				{
					i++;
				}
				if (i < this._size)
				{
					this._items[num++] = this._items[i++];
				}
			}
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				Array.Clear(this._items, num, this._size - num);
			}
			int result = this._size - num;
			this._size = num;
			this._version++;
			return result;
		}

		// Token: 0x0600617F RID: 24959 RVA: 0x00145F14 File Offset: 0x00144114
		public void RemoveAt(int index)
		{
			if (index >= this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRange_IndexException();
			}
			this._size--;
			if (index < this._size)
			{
				Array.Copy(this._items, index + 1, this._items, index, this._size - index);
			}
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				this._items[this._size] = default(T);
			}
			this._version++;
		}

		// Token: 0x06006180 RID: 24960 RVA: 0x00145F94 File Offset: 0x00144194
		public void RemoveRange(int index, int count)
		{
			if (index < 0)
			{
				ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
			}
			if (count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			if (count > 0)
			{
				int size = this._size;
				this._size -= count;
				if (index < this._size)
				{
					Array.Copy(this._items, index + count, this._items, index, this._size - index);
				}
				this._version++;
				if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				{
					Array.Clear(this._items, this._size, count);
				}
			}
		}

		// Token: 0x06006181 RID: 24961 RVA: 0x0014602E File Offset: 0x0014422E
		public void Reverse()
		{
			this.Reverse(0, this.Count);
		}

		// Token: 0x06006182 RID: 24962 RVA: 0x00146040 File Offset: 0x00144240
		public void Reverse(int index, int count)
		{
			if (index < 0)
			{
				ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
			}
			if (count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			if (count > 1)
			{
				Array.Reverse<T>(this._items, index, count);
			}
			this._version++;
		}

		// Token: 0x06006183 RID: 24963 RVA: 0x00146093 File Offset: 0x00144293
		public void Sort()
		{
			this.Sort(0, this.Count, null);
		}

		// Token: 0x06006184 RID: 24964 RVA: 0x001460A3 File Offset: 0x001442A3
		public void Sort(IComparer<T> comparer)
		{
			this.Sort(0, this.Count, comparer);
		}

		// Token: 0x06006185 RID: 24965 RVA: 0x001460B4 File Offset: 0x001442B4
		public void Sort(int index, int count, IComparer<T> comparer)
		{
			if (index < 0)
			{
				ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
			}
			if (count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			if (count > 1)
			{
				Array.Sort<T>(this._items, index, count, comparer);
			}
			this._version++;
		}

		// Token: 0x06006186 RID: 24966 RVA: 0x00146108 File Offset: 0x00144308
		public void Sort(Comparison<T> comparison)
		{
			if (comparison == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.comparison);
			}
			if (this._size > 1)
			{
				ArraySortHelper<T>.Sort(this._items, 0, this._size, comparison);
			}
			this._version++;
		}

		// Token: 0x06006187 RID: 24967 RVA: 0x00146140 File Offset: 0x00144340
		public T[] ToArray()
		{
			if (this._size == 0)
			{
				return List<T>.s_emptyArray;
			}
			T[] array = new T[this._size];
			Array.Copy(this._items, 0, array, 0, this._size);
			return array;
		}

		// Token: 0x06006188 RID: 24968 RVA: 0x0014617C File Offset: 0x0014437C
		public void TrimExcess()
		{
			int num = (int)((double)this._items.Length * 0.9);
			if (this._size < num)
			{
				this.Capacity = this._size;
			}
		}

		// Token: 0x06006189 RID: 24969 RVA: 0x001461B4 File Offset: 0x001443B4
		public bool TrueForAll(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = 0; i < this._size; i++)
			{
				if (!match(this._items[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600618A RID: 24970 RVA: 0x001461F4 File Offset: 0x001443F4
		private void AddEnumerable(IEnumerable<T> enumerable)
		{
			this._version++;
			foreach (T t in enumerable)
			{
				if (this._size == this._items.Length)
				{
					this.EnsureCapacity(this._size + 1);
				}
				T[] items = this._items;
				int size = this._size;
				this._size = size + 1;
				items[size] = t;
			}
		}

		// Token: 0x040039E8 RID: 14824
		private const int DefaultCapacity = 4;

		// Token: 0x040039E9 RID: 14825
		private T[] _items;

		// Token: 0x040039EA RID: 14826
		private int _size;

		// Token: 0x040039EB RID: 14827
		private int _version;

		// Token: 0x040039EC RID: 14828
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x040039ED RID: 14829
		private static readonly T[] s_emptyArray = new T[0];

		// Token: 0x02000AA4 RID: 2724
		[Serializable]
		public struct Enumerator : IEnumerator<!0>, IDisposable, IEnumerator
		{
			// Token: 0x0600618C RID: 24972 RVA: 0x0014628D File Offset: 0x0014448D
			internal Enumerator(List<T> list)
			{
				this._list = list;
				this._index = 0;
				this._version = list._version;
				this._current = default(T);
			}

			// Token: 0x0600618D RID: 24973 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public void Dispose()
			{
			}

			// Token: 0x0600618E RID: 24974 RVA: 0x001462B8 File Offset: 0x001444B8
			public bool MoveNext()
			{
				List<T> list = this._list;
				if (this._version == list._version && this._index < list._size)
				{
					this._current = list._items[this._index];
					this._index++;
					return true;
				}
				return this.MoveNextRare();
			}

			// Token: 0x0600618F RID: 24975 RVA: 0x00146315 File Offset: 0x00144515
			private bool MoveNextRare()
			{
				if (this._version != this._list._version)
				{
					ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
				}
				this._index = this._list._size + 1;
				this._current = default(T);
				return false;
			}

			// Token: 0x17001157 RID: 4439
			// (get) Token: 0x06006190 RID: 24976 RVA: 0x0014634F File Offset: 0x0014454F
			public T Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x17001158 RID: 4440
			// (get) Token: 0x06006191 RID: 24977 RVA: 0x00146357 File Offset: 0x00144557
			object IEnumerator.Current
			{
				get
				{
					if (this._index == 0 || this._index == this._list._size + 1)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
					}
					return this.Current;
				}
			}

			// Token: 0x06006192 RID: 24978 RVA: 0x00146386 File Offset: 0x00144586
			void IEnumerator.Reset()
			{
				if (this._version != this._list._version)
				{
					ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
				}
				this._index = 0;
				this._current = default(T);
			}

			// Token: 0x040039EE RID: 14830
			private List<T> _list;

			// Token: 0x040039EF RID: 14831
			private int _index;

			// Token: 0x040039F0 RID: 14832
			private int _version;

			// Token: 0x040039F1 RID: 14833
			private T _current;
		}
	}
}
