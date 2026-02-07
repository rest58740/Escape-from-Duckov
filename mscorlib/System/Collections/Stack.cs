using System;
using System.Diagnostics;
using System.Threading;

namespace System.Collections
{
	// Token: 0x02000A32 RID: 2610
	[DebuggerTypeProxy(typeof(Stack.StackDebugView))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class Stack : ICollection, IEnumerable, ICloneable
	{
		// Token: 0x06005CB6 RID: 23734 RVA: 0x00137911 File Offset: 0x00135B11
		public Stack()
		{
			this._array = new object[10];
			this._size = 0;
			this._version = 0;
		}

		// Token: 0x06005CB7 RID: 23735 RVA: 0x00137934 File Offset: 0x00135B34
		public Stack(int initialCapacity)
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentOutOfRangeException("initialCapacity", "Non-negative number required.");
			}
			if (initialCapacity < 10)
			{
				initialCapacity = 10;
			}
			this._array = new object[initialCapacity];
			this._size = 0;
			this._version = 0;
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x00137974 File Offset: 0x00135B74
		public Stack(ICollection col) : this((col == null) ? 32 : col.Count)
		{
			if (col == null)
			{
				throw new ArgumentNullException("col");
			}
			foreach (object obj in col)
			{
				this.Push(obj);
			}
		}

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x06005CB9 RID: 23737 RVA: 0x001379BF File Offset: 0x00135BBF
		public virtual int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x06005CBA RID: 23738 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x06005CBB RID: 23739 RVA: 0x001379C7 File Offset: 0x00135BC7
		public virtual object SyncRoot
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

		// Token: 0x06005CBC RID: 23740 RVA: 0x001379E9 File Offset: 0x00135BE9
		public virtual void Clear()
		{
			Array.Clear(this._array, 0, this._size);
			this._size = 0;
			this._version++;
		}

		// Token: 0x06005CBD RID: 23741 RVA: 0x00137A14 File Offset: 0x00135C14
		public virtual object Clone()
		{
			Stack stack = new Stack(this._size);
			stack._size = this._size;
			Array.Copy(this._array, 0, stack._array, 0, this._size);
			stack._version = this._version;
			return stack;
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x00137A60 File Offset: 0x00135C60
		public virtual bool Contains(object obj)
		{
			int size = this._size;
			while (size-- > 0)
			{
				if (obj == null)
				{
					if (this._array[size] == null)
					{
						return true;
					}
				}
				else if (this._array[size] != null && this._array[size].Equals(obj))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005CBF RID: 23743 RVA: 0x00137AAC File Offset: 0x00135CAC
		public virtual void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.", "array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}
			if (array.Length - index < this._size)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			int i = 0;
			object[] array2 = array as object[];
			if (array2 != null)
			{
				while (i < this._size)
				{
					array2[i + index] = this._array[this._size - i - 1];
					i++;
				}
				return;
			}
			while (i < this._size)
			{
				array.SetValue(this._array[this._size - i - 1], i + index);
				i++;
			}
		}

		// Token: 0x06005CC0 RID: 23744 RVA: 0x00137B68 File Offset: 0x00135D68
		public virtual IEnumerator GetEnumerator()
		{
			return new Stack.StackEnumerator(this);
		}

		// Token: 0x06005CC1 RID: 23745 RVA: 0x00137B70 File Offset: 0x00135D70
		public virtual object Peek()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException("Stack empty.");
			}
			return this._array[this._size - 1];
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x00137B94 File Offset: 0x00135D94
		public virtual object Pop()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException("Stack empty.");
			}
			this._version++;
			object[] array = this._array;
			int num = this._size - 1;
			this._size = num;
			object result = array[num];
			this._array[this._size] = null;
			return result;
		}

		// Token: 0x06005CC3 RID: 23747 RVA: 0x00137BE8 File Offset: 0x00135DE8
		public virtual void Push(object obj)
		{
			if (this._size == this._array.Length)
			{
				object[] array = new object[2 * this._array.Length];
				Array.Copy(this._array, 0, array, 0, this._size);
				this._array = array;
			}
			object[] array2 = this._array;
			int size = this._size;
			this._size = size + 1;
			array2[size] = obj;
			this._version++;
		}

		// Token: 0x06005CC4 RID: 23748 RVA: 0x00137C57 File Offset: 0x00135E57
		public static Stack Synchronized(Stack stack)
		{
			if (stack == null)
			{
				throw new ArgumentNullException("stack");
			}
			return new Stack.SyncStack(stack);
		}

		// Token: 0x06005CC5 RID: 23749 RVA: 0x00137C70 File Offset: 0x00135E70
		public virtual object[] ToArray()
		{
			if (this._size == 0)
			{
				return Array.Empty<object>();
			}
			object[] array = new object[this._size];
			for (int i = 0; i < this._size; i++)
			{
				array[i] = this._array[this._size - i - 1];
			}
			return array;
		}

		// Token: 0x040038B6 RID: 14518
		private object[] _array;

		// Token: 0x040038B7 RID: 14519
		private int _size;

		// Token: 0x040038B8 RID: 14520
		private int _version;

		// Token: 0x040038B9 RID: 14521
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x040038BA RID: 14522
		private const int _defaultCapacity = 10;

		// Token: 0x02000A33 RID: 2611
		[Serializable]
		private class SyncStack : Stack
		{
			// Token: 0x06005CC6 RID: 23750 RVA: 0x00137CBD File Offset: 0x00135EBD
			internal SyncStack(Stack stack)
			{
				this._s = stack;
				this._root = stack.SyncRoot;
			}

			// Token: 0x17001027 RID: 4135
			// (get) Token: 0x06005CC7 RID: 23751 RVA: 0x000040F7 File Offset: 0x000022F7
			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001028 RID: 4136
			// (get) Token: 0x06005CC8 RID: 23752 RVA: 0x00137CD8 File Offset: 0x00135ED8
			public override object SyncRoot
			{
				get
				{
					return this._root;
				}
			}

			// Token: 0x17001029 RID: 4137
			// (get) Token: 0x06005CC9 RID: 23753 RVA: 0x00137CE0 File Offset: 0x00135EE0
			public override int Count
			{
				get
				{
					object root = this._root;
					int count;
					lock (root)
					{
						count = this._s.Count;
					}
					return count;
				}
			}

			// Token: 0x06005CCA RID: 23754 RVA: 0x00137D28 File Offset: 0x00135F28
			public override bool Contains(object obj)
			{
				object root = this._root;
				bool result;
				lock (root)
				{
					result = this._s.Contains(obj);
				}
				return result;
			}

			// Token: 0x06005CCB RID: 23755 RVA: 0x00137D70 File Offset: 0x00135F70
			public override object Clone()
			{
				object root = this._root;
				object result;
				lock (root)
				{
					result = new Stack.SyncStack((Stack)this._s.Clone());
				}
				return result;
			}

			// Token: 0x06005CCC RID: 23756 RVA: 0x00137DC4 File Offset: 0x00135FC4
			public override void Clear()
			{
				object root = this._root;
				lock (root)
				{
					this._s.Clear();
				}
			}

			// Token: 0x06005CCD RID: 23757 RVA: 0x00137E0C File Offset: 0x0013600C
			public override void CopyTo(Array array, int arrayIndex)
			{
				object root = this._root;
				lock (root)
				{
					this._s.CopyTo(array, arrayIndex);
				}
			}

			// Token: 0x06005CCE RID: 23758 RVA: 0x00137E54 File Offset: 0x00136054
			public override void Push(object value)
			{
				object root = this._root;
				lock (root)
				{
					this._s.Push(value);
				}
			}

			// Token: 0x06005CCF RID: 23759 RVA: 0x00137E9C File Offset: 0x0013609C
			public override object Pop()
			{
				object root = this._root;
				object result;
				lock (root)
				{
					result = this._s.Pop();
				}
				return result;
			}

			// Token: 0x06005CD0 RID: 23760 RVA: 0x00137EE4 File Offset: 0x001360E4
			public override IEnumerator GetEnumerator()
			{
				object root = this._root;
				IEnumerator enumerator;
				lock (root)
				{
					enumerator = this._s.GetEnumerator();
				}
				return enumerator;
			}

			// Token: 0x06005CD1 RID: 23761 RVA: 0x00137F2C File Offset: 0x0013612C
			public override object Peek()
			{
				object root = this._root;
				object result;
				lock (root)
				{
					result = this._s.Peek();
				}
				return result;
			}

			// Token: 0x06005CD2 RID: 23762 RVA: 0x00137F74 File Offset: 0x00136174
			public override object[] ToArray()
			{
				object root = this._root;
				object[] result;
				lock (root)
				{
					result = this._s.ToArray();
				}
				return result;
			}

			// Token: 0x040038BB RID: 14523
			private Stack _s;

			// Token: 0x040038BC RID: 14524
			private object _root;
		}

		// Token: 0x02000A34 RID: 2612
		[Serializable]
		private class StackEnumerator : IEnumerator, ICloneable
		{
			// Token: 0x06005CD3 RID: 23763 RVA: 0x00137FBC File Offset: 0x001361BC
			internal StackEnumerator(Stack stack)
			{
				this._stack = stack;
				this._version = this._stack._version;
				this._index = -2;
				this._currentElement = null;
			}

			// Token: 0x06005CD4 RID: 23764 RVA: 0x000231D1 File Offset: 0x000213D1
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x06005CD5 RID: 23765 RVA: 0x00137FEC File Offset: 0x001361EC
			public virtual bool MoveNext()
			{
				if (this._version != this._stack._version)
				{
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
				}
				if (this._index == -2)
				{
					this._index = this._stack._size - 1;
					bool flag = this._index >= 0;
					if (flag)
					{
						this._currentElement = this._stack._array[this._index];
					}
					return flag;
				}
				if (this._index == -1)
				{
					return false;
				}
				int num = this._index - 1;
				this._index = num;
				bool flag2 = num >= 0;
				if (flag2)
				{
					this._currentElement = this._stack._array[this._index];
					return flag2;
				}
				this._currentElement = null;
				return flag2;
			}

			// Token: 0x1700102A RID: 4138
			// (get) Token: 0x06005CD6 RID: 23766 RVA: 0x001380A1 File Offset: 0x001362A1
			public virtual object Current
			{
				get
				{
					if (this._index == -2)
					{
						throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException("Enumeration already finished.");
					}
					return this._currentElement;
				}
			}

			// Token: 0x06005CD7 RID: 23767 RVA: 0x001380D2 File Offset: 0x001362D2
			public virtual void Reset()
			{
				if (this._version != this._stack._version)
				{
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
				}
				this._index = -2;
				this._currentElement = null;
			}

			// Token: 0x040038BD RID: 14525
			private Stack _stack;

			// Token: 0x040038BE RID: 14526
			private int _index;

			// Token: 0x040038BF RID: 14527
			private int _version;

			// Token: 0x040038C0 RID: 14528
			private object _currentElement;
		}

		// Token: 0x02000A35 RID: 2613
		internal class StackDebugView
		{
			// Token: 0x06005CD8 RID: 23768 RVA: 0x00138101 File Offset: 0x00136301
			public StackDebugView(Stack stack)
			{
				if (stack == null)
				{
					throw new ArgumentNullException("stack");
				}
				this._stack = stack;
			}

			// Token: 0x1700102B RID: 4139
			// (get) Token: 0x06005CD9 RID: 23769 RVA: 0x0013811E File Offset: 0x0013631E
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public object[] Items
			{
				get
				{
					return this._stack.ToArray();
				}
			}

			// Token: 0x040038C1 RID: 14529
			private Stack _stack;
		}
	}
}
