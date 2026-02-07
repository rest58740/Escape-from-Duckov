using System;
using System.Collections;
using Unity;

namespace System.Runtime.Serialization
{
	// Token: 0x0200064D RID: 1613
	public sealed class SerializationInfoEnumerator : IEnumerator
	{
		// Token: 0x06003C58 RID: 15448 RVA: 0x000D1503 File Offset: 0x000CF703
		internal SerializationInfoEnumerator(string[] members, object[] info, Type[] types, int numItems)
		{
			this._members = members;
			this._data = info;
			this._types = types;
			this._numItems = numItems - 1;
			this._currItem = -1;
			this._current = false;
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x000D1538 File Offset: 0x000CF738
		public bool MoveNext()
		{
			if (this._currItem < this._numItems)
			{
				this._currItem++;
				this._current = true;
			}
			else
			{
				this._current = false;
			}
			return this._current;
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06003C5A RID: 15450 RVA: 0x000D156C File Offset: 0x000CF76C
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06003C5B RID: 15451 RVA: 0x000D157C File Offset: 0x000CF77C
		public SerializationEntry Current
		{
			get
			{
				if (!this._current)
				{
					throw new InvalidOperationException("Enumeration has either not started or has already finished.");
				}
				return new SerializationEntry(this._members[this._currItem], this._data[this._currItem], this._types[this._currItem]);
			}
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x000D15C8 File Offset: 0x000CF7C8
		public void Reset()
		{
			this._currItem = -1;
			this._current = false;
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06003C5D RID: 15453 RVA: 0x000D15D8 File Offset: 0x000CF7D8
		public string Name
		{
			get
			{
				if (!this._current)
				{
					throw new InvalidOperationException("Enumeration has either not started or has already finished.");
				}
				return this._members[this._currItem];
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06003C5E RID: 15454 RVA: 0x000D15FA File Offset: 0x000CF7FA
		public object Value
		{
			get
			{
				if (!this._current)
				{
					throw new InvalidOperationException("Enumeration has either not started or has already finished.");
				}
				return this._data[this._currItem];
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06003C5F RID: 15455 RVA: 0x000D161C File Offset: 0x000CF81C
		public Type ObjectType
		{
			get
			{
				if (!this._current)
				{
					throw new InvalidOperationException("Enumeration has either not started or has already finished.");
				}
				return this._types[this._currItem];
			}
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x000173AD File Offset: 0x000155AD
		internal SerializationInfoEnumerator()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04002719 RID: 10009
		private readonly string[] _members;

		// Token: 0x0400271A RID: 10010
		private readonly object[] _data;

		// Token: 0x0400271B RID: 10011
		private readonly Type[] _types;

		// Token: 0x0400271C RID: 10012
		private readonly int _numItems;

		// Token: 0x0400271D RID: 10013
		private int _currItem;

		// Token: 0x0400271E RID: 10014
		private bool _current;
	}
}
