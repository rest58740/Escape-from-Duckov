using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace System.Collections.ObjectModel
{
	// Token: 0x02000A81 RID: 2689
	[DebuggerTypeProxy(typeof(ICollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class ReadOnlyCollection<T> : IList<!0>, ICollection<!0>, IEnumerable<!0>, IEnumerable, IList, ICollection, IReadOnlyList<!0>, IReadOnlyCollection<T>
	{
		// Token: 0x06006020 RID: 24608 RVA: 0x00142691 File Offset: 0x00140891
		public ReadOnlyCollection(IList<T> list)
		{
			if (list == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.list);
			}
			this.list = list;
		}

		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x06006021 RID: 24609 RVA: 0x001426A9 File Offset: 0x001408A9
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x170010EE RID: 4334
		public T this[int index]
		{
			get
			{
				return this.list[index];
			}
		}

		// Token: 0x06006023 RID: 24611 RVA: 0x001426C4 File Offset: 0x001408C4
		public bool Contains(T value)
		{
			return this.list.Contains(value);
		}

		// Token: 0x06006024 RID: 24612 RVA: 0x001426D2 File Offset: 0x001408D2
		public void CopyTo(T[] array, int index)
		{
			this.list.CopyTo(array, index);
		}

		// Token: 0x06006025 RID: 24613 RVA: 0x001426E1 File Offset: 0x001408E1
		public IEnumerator<T> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06006026 RID: 24614 RVA: 0x001426EE File Offset: 0x001408EE
		public int IndexOf(T value)
		{
			return this.list.IndexOf(value);
		}

		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x06006027 RID: 24615 RVA: 0x001426FC File Offset: 0x001408FC
		protected IList<T> Items
		{
			get
			{
				return this.list;
			}
		}

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06006028 RID: 24616 RVA: 0x000040F7 File Offset: 0x000022F7
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170010F1 RID: 4337
		T IList<!0>.this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
		}

		// Token: 0x0600602B RID: 24619 RVA: 0x00142704 File Offset: 0x00140904
		void ICollection<!0>.Add(T value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x0600602C RID: 24620 RVA: 0x00142704 File Offset: 0x00140904
		void ICollection<!0>.Clear()
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x0600602D RID: 24621 RVA: 0x00142704 File Offset: 0x00140904
		void IList<!0>.Insert(int index, T value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x0600602E RID: 24622 RVA: 0x0014270D File Offset: 0x0014090D
		bool ICollection<!0>.Remove(T value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			return false;
		}

		// Token: 0x0600602F RID: 24623 RVA: 0x00142704 File Offset: 0x00140904
		void IList<!0>.RemoveAt(int index)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06006030 RID: 24624 RVA: 0x00142717 File Offset: 0x00140917
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x06006031 RID: 24625 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06006032 RID: 24626 RVA: 0x00142724 File Offset: 0x00140924
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					ICollection collection = this.list as ICollection;
					if (collection != null)
					{
						this._syncRoot = collection.SyncRoot;
					}
					else
					{
						Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
					}
				}
				return this._syncRoot;
			}
		}

		// Token: 0x06006033 RID: 24627 RVA: 0x00142770 File Offset: 0x00140970
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
				this.list.CopyTo(array2, index);
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
			int count = this.list.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					array3[index++] = this.list[i];
				}
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
			}
		}

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06006034 RID: 24628 RVA: 0x000040F7 File Offset: 0x000022F7
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x06006035 RID: 24629 RVA: 0x000040F7 File Offset: 0x000022F7
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170010F6 RID: 4342
		object IList.this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
		}

		// Token: 0x06006038 RID: 24632 RVA: 0x0014287F File Offset: 0x00140A7F
		int IList.Add(object value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			return -1;
		}

		// Token: 0x06006039 RID: 24633 RVA: 0x00142704 File Offset: 0x00140904
		void IList.Clear()
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x0014288C File Offset: 0x00140A8C
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && default(T) == null);
		}

		// Token: 0x0600603B RID: 24635 RVA: 0x001428B9 File Offset: 0x00140AB9
		bool IList.Contains(object value)
		{
			return ReadOnlyCollection<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x0600603C RID: 24636 RVA: 0x001428D1 File Offset: 0x00140AD1
		int IList.IndexOf(object value)
		{
			if (ReadOnlyCollection<T>.IsCompatibleObject(value))
			{
				return this.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x0600603D RID: 24637 RVA: 0x00142704 File Offset: 0x00140904
		void IList.Insert(int index, object value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x0600603E RID: 24638 RVA: 0x00142704 File Offset: 0x00140904
		void IList.Remove(object value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x0600603F RID: 24639 RVA: 0x00142704 File Offset: 0x00140904
		void IList.RemoveAt(int index)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x040039AA RID: 14762
		private IList<T> list;

		// Token: 0x040039AB RID: 14763
		[NonSerialized]
		private object _syncRoot;
	}
}
