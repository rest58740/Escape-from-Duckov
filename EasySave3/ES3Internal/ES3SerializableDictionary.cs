using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ES3Internal
{
	// Token: 0x020000D9 RID: 217
	[Serializable]
	public abstract class ES3SerializableDictionary<TKey, TVal> : Dictionary<TKey, TVal>, ISerializationCallbackReceiver
	{
		// Token: 0x06000484 RID: 1156
		protected abstract bool KeysAreEqual(TKey a, TKey b);

		// Token: 0x06000485 RID: 1157
		protected abstract bool ValuesAreEqual(TVal a, TVal b);

		// Token: 0x06000486 RID: 1158 RVA: 0x0001CEC8 File Offset: 0x0001B0C8
		public void OnBeforeSerialize()
		{
			this._Keys = new List<TKey>();
			this._Values = new List<TVal>();
			foreach (KeyValuePair<TKey, TVal> keyValuePair in this)
			{
				try
				{
					this._Keys.Add(keyValuePair.Key);
					this._Values.Add(keyValuePair.Value);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0001CF5C File Offset: 0x0001B15C
		public void OnAfterDeserialize()
		{
			if (this._Keys == null || this._Values == null)
			{
				return;
			}
			if (this._Keys.Count != this._Values.Count)
			{
				throw new Exception(string.Format("Key count is different to value count after deserialising dictionary.", Array.Empty<object>()));
			}
			base.Clear();
			for (int i = 0; i < this._Keys.Count; i++)
			{
				if (this._Keys[i] != null)
				{
					try
					{
						base.Add(this._Keys[i], this._Values[i]);
					}
					catch
					{
					}
				}
			}
			this._Keys = null;
			this._Values = null;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001D018 File Offset: 0x0001B218
		public int RemoveNullValues()
		{
			List<TKey> list = (from pair in this
			where pair.Value == null
			select pair.Key).ToList<TKey>();
			foreach (TKey key in list)
			{
				base.Remove(key);
			}
			return list.Count;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001D0BC File Offset: 0x0001B2BC
		public bool ChangeKey(TKey oldKey, TKey newKey)
		{
			if (this.KeysAreEqual(oldKey, newKey))
			{
				return false;
			}
			TVal value = base[oldKey];
			base.Remove(oldKey);
			base[newKey] = value;
			return true;
		}

		// Token: 0x0400013B RID: 315
		[SerializeField]
		private List<TKey> _Keys;

		// Token: 0x0400013C RID: 316
		[SerializeField]
		private List<TVal> _Values;
	}
}
