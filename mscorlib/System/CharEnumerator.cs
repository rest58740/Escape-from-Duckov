using System;
using System.Collections;
using System.Collections.Generic;
using Unity;

namespace System
{
	// Token: 0x02000106 RID: 262
	[Serializable]
	public sealed class CharEnumerator : IEnumerator, IEnumerator<char>, IDisposable, ICloneable
	{
		// Token: 0x0600082E RID: 2094 RVA: 0x000231BB File Offset: 0x000213BB
		internal CharEnumerator(string str)
		{
			this._str = str;
			this._index = -1;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x000231D1 File Offset: 0x000213D1
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000231DC File Offset: 0x000213DC
		public bool MoveNext()
		{
			if (this._index < this._str.Length - 1)
			{
				this._index++;
				this._currentElement = this._str[this._index];
				return true;
			}
			this._index = this._str.Length;
			return false;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00023237 File Offset: 0x00021437
		public void Dispose()
		{
			if (this._str != null)
			{
				this._index = this._str.Length;
			}
			this._str = null;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x00023259 File Offset: 0x00021459
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x00023266 File Offset: 0x00021466
		public char Current
		{
			get
			{
				if (this._index == -1)
				{
					throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
				}
				if (this._index >= this._str.Length)
				{
					throw new InvalidOperationException("Enumeration already finished.");
				}
				return this._currentElement;
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x000232A0 File Offset: 0x000214A0
		public void Reset()
		{
			this._currentElement = '\0';
			this._index = -1;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x000173AD File Offset: 0x000155AD
		internal CharEnumerator()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04001081 RID: 4225
		private string _str;

		// Token: 0x04001082 RID: 4226
		private int _index;

		// Token: 0x04001083 RID: 4227
		private char _currentElement;
	}
}
