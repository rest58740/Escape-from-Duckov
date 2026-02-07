using System;
using System.Collections.Generic;

namespace ParadoxNotion
{
	// Token: 0x0200007F RID: 127
	public class WeakReferenceTable<TKey, TValue> where TKey : class where TValue : IDisposable
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x0000DD5C File Offset: 0x0000BF5C
		public int Count
		{
			get
			{
				return this.keys.Count;
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0000DD69 File Offset: 0x0000BF69
		public WeakReferenceTable()
		{
			this.keys = new List<WeakReference<TKey>>();
			this.values = new List<TValue>();
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0000DD87 File Offset: 0x0000BF87
		public void Clear()
		{
			this.keys.Clear();
			this.values.Clear();
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0000DD9F File Offset: 0x0000BF9F
		public void Add(TKey key, TValue value)
		{
			this.CheckCount();
			this.keys.Insert(0, new WeakReference<TKey>(key));
			this.values.Insert(0, value);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0000DDC8 File Offset: 0x0000BFC8
		public void Remove(TKey key)
		{
			this.CheckCount();
			int count = this.keys.Count;
			while (count-- > 0)
			{
				TKey tkey;
				if (this.keys[count].TryGetTarget(ref tkey) && tkey == key)
				{
					this.keys.RemoveAt(count);
					TValue tvalue = this.values[count];
					tvalue.Dispose();
					this.values.RemoveAt(count);
				}
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0000DE48 File Offset: 0x0000C048
		public bool TryGetValueWithRefCheck(TKey key, out TValue value)
		{
			this.CheckCount();
			int count = this.keys.Count;
			while (count-- > 0)
			{
				TKey tkey;
				if (!this.keys[count].TryGetTarget(ref tkey))
				{
					this.keys.RemoveAt(count);
					TValue tvalue = this.values[count];
					tvalue.Dispose();
					this.values.RemoveAt(count);
				}
				if (tkey == key)
				{
					value = this.values[count];
					return true;
				}
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0000DEE4 File Offset: 0x0000C0E4
		public void RemoveMissingReferences()
		{
			this.CheckCount();
			int count = this.keys.Count;
			while (count-- > 0)
			{
				TKey tkey;
				if (!this.keys[count].TryGetTarget(ref tkey))
				{
					this.keys.RemoveAt(count);
					TValue tvalue = this.values[count];
					tvalue.Dispose();
					this.values.RemoveAt(count);
				}
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0000DF54 File Offset: 0x0000C154
		private void CheckCount()
		{
			if (this.keys.Count != this.values.Count)
			{
				throw new Exception("Mismatched indeces");
			}
		}

		// Token: 0x0400017F RID: 383
		private List<WeakReference<TKey>> keys;

		// Token: 0x04000180 RID: 384
		private List<TValue> values;
	}
}
