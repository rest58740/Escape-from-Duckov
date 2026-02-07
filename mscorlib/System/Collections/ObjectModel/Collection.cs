using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Collections.ObjectModel
{
	// Token: 0x02000A80 RID: 2688
	[DebuggerTypeProxy(typeof(ICollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class Collection<T> : IList<!0>, ICollection<!0>, IEnumerable<!0>, IEnumerable, IList, ICollection, IReadOnlyList<!0>, IReadOnlyCollection<T>
	{
		// Token: 0x06005FFE RID: 24574 RVA: 0x0014219F File Offset: 0x0014039F
		public Collection()
		{
			this.items = new List<T>();
		}

		// Token: 0x06005FFF RID: 24575 RVA: 0x001421B2 File Offset: 0x001403B2
		public Collection(IList<T> list)
		{
			if (list == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.list);
			}
			this.items = list;
		}

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x06006000 RID: 24576 RVA: 0x001421CA File Offset: 0x001403CA
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x06006001 RID: 24577 RVA: 0x001421D7 File Offset: 0x001403D7
		protected IList<T> Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x170010E6 RID: 4326
		public T this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				if (this.items.IsReadOnly)
				{
					ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
				}
				if (index >= this.items.Count)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				this.SetItem(index, value);
			}
		}

		// Token: 0x06006004 RID: 24580 RVA: 0x00142220 File Offset: 0x00140420
		public void Add(T item)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			int count = this.items.Count;
			this.InsertItem(count, item);
		}

		// Token: 0x06006005 RID: 24581 RVA: 0x00142255 File Offset: 0x00140455
		public void Clear()
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			this.ClearItems();
		}

		// Token: 0x06006006 RID: 24582 RVA: 0x00142271 File Offset: 0x00140471
		public void CopyTo(T[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x06006007 RID: 24583 RVA: 0x00142280 File Offset: 0x00140480
		public bool Contains(T item)
		{
			return this.items.Contains(item);
		}

		// Token: 0x06006008 RID: 24584 RVA: 0x0014228E File Offset: 0x0014048E
		public IEnumerator<T> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06006009 RID: 24585 RVA: 0x0014229B File Offset: 0x0014049B
		public int IndexOf(T item)
		{
			return this.items.IndexOf(item);
		}

		// Token: 0x0600600A RID: 24586 RVA: 0x001422A9 File Offset: 0x001404A9
		public void Insert(int index, T item)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			if (index > this.items.Count)
			{
				ThrowHelper.ThrowArgumentOutOfRange_IndexException();
			}
			this.InsertItem(index, item);
		}

		// Token: 0x0600600B RID: 24587 RVA: 0x001422DC File Offset: 0x001404DC
		public bool Remove(T item)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			int num = this.items.IndexOf(item);
			if (num < 0)
			{
				return false;
			}
			this.RemoveItem(num);
			return true;
		}

		// Token: 0x0600600C RID: 24588 RVA: 0x00142318 File Offset: 0x00140518
		public void RemoveAt(int index)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			if (index >= this.items.Count)
			{
				ThrowHelper.ThrowArgumentOutOfRange_IndexException();
			}
			this.RemoveItem(index);
		}

		// Token: 0x0600600D RID: 24589 RVA: 0x00142348 File Offset: 0x00140548
		protected virtual void ClearItems()
		{
			this.items.Clear();
		}

		// Token: 0x0600600E RID: 24590 RVA: 0x00142355 File Offset: 0x00140555
		protected virtual void InsertItem(int index, T item)
		{
			this.items.Insert(index, item);
		}

		// Token: 0x0600600F RID: 24591 RVA: 0x00142364 File Offset: 0x00140564
		protected virtual void RemoveItem(int index)
		{
			this.items.RemoveAt(index);
		}

		// Token: 0x06006010 RID: 24592 RVA: 0x00142372 File Offset: 0x00140572
		protected virtual void SetItem(int index, T item)
		{
			this.items[index] = item;
		}

		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x06006011 RID: 24593 RVA: 0x00142381 File Offset: 0x00140581
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return this.items.IsReadOnly;
			}
		}

		// Token: 0x06006012 RID: 24594 RVA: 0x0014238E File Offset: 0x0014058E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x06006013 RID: 24595 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x06006014 RID: 24596 RVA: 0x0014239C File Offset: 0x0014059C
		object ICollection.SyncRoot
		{
			get
			{
				ICollection collection = this.items as ICollection;
				if (collection == null)
				{
					return this;
				}
				return collection.SyncRoot;
			}
		}

		// Token: 0x06006015 RID: 24597 RVA: 0x001423C0 File Offset: 0x001405C0
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (array.Rank != 1)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			if (array.GetLowerBound(0) != 0)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
			}
			if (index < 0)
			{
				ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			T[] array2 = array as T[];
			if (array2 != null)
			{
				this.items.CopyTo(array2, index);
				return;
			}
			Type elementType = array.GetType().GetElementType();
			Type typeFromHandle = typeof(T);
			if (!elementType.IsAssignableFrom(typeFromHandle) && !typeFromHandle.IsAssignableFrom(elementType))
			{
				ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
			}
			object[] array3 = array as object[];
			if (array3 == null)
			{
				ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
			}
			int count = this.items.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					array3[index++] = this.items[i];
				}
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
			}
		}

		// Token: 0x170010EA RID: 4330
		object IList.this[int index]
		{
			get
			{
				return this.items[index];
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

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06006018 RID: 24600 RVA: 0x00142381 File Offset: 0x00140581
		bool IList.IsReadOnly
		{
			get
			{
				return this.items.IsReadOnly;
			}
		}

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06006019 RID: 24601 RVA: 0x00142518 File Offset: 0x00140718
		bool IList.IsFixedSize
		{
			get
			{
				IList list = this.items as IList;
				if (list != null)
				{
					return list.IsFixedSize;
				}
				return this.items.IsReadOnly;
			}
		}

		// Token: 0x0600601A RID: 24602 RVA: 0x00142548 File Offset: 0x00140748
		int IList.Add(object value)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, ExceptionArgument.value);
			try
			{
				this.Add((T)((object)value));
			}
			catch (InvalidCastException)
			{
				ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
			}
			return this.Count - 1;
		}

		// Token: 0x0600601B RID: 24603 RVA: 0x001425AC File Offset: 0x001407AC
		bool IList.Contains(object value)
		{
			return Collection<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x0600601C RID: 24604 RVA: 0x001425C4 File Offset: 0x001407C4
		int IList.IndexOf(object value)
		{
			if (Collection<T>.IsCompatibleObject(value))
			{
				return this.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x0600601D RID: 24605 RVA: 0x001425DC File Offset: 0x001407DC
		void IList.Insert(int index, object value)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, ExceptionArgument.value);
			try
			{
				this.Insert(index, (T)((object)value));
			}
			catch (InvalidCastException)
			{
				ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
			}
		}

		// Token: 0x0600601E RID: 24606 RVA: 0x00142638 File Offset: 0x00140838
		void IList.Remove(object value)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			if (Collection<T>.IsCompatibleObject(value))
			{
				this.Remove((T)((object)value));
			}
		}

		// Token: 0x0600601F RID: 24607 RVA: 0x00142664 File Offset: 0x00140864
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && default(T) == null);
		}

		// Token: 0x040039A9 RID: 14761
		private IList<T> items;
	}
}
