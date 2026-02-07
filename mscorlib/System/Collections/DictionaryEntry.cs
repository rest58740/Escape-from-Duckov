using System;

namespace System.Collections
{
	// Token: 0x02000A10 RID: 2576
	[Serializable]
	public struct DictionaryEntry
	{
		// Token: 0x06005B73 RID: 23411 RVA: 0x00134B7E File Offset: 0x00132D7E
		public DictionaryEntry(object key, object value)
		{
			this._key = key;
			this._value = value;
		}

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x06005B74 RID: 23412 RVA: 0x00134B8E File Offset: 0x00132D8E
		// (set) Token: 0x06005B75 RID: 23413 RVA: 0x00134B96 File Offset: 0x00132D96
		public object Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x06005B76 RID: 23414 RVA: 0x00134B9F File Offset: 0x00132D9F
		// (set) Token: 0x06005B77 RID: 23415 RVA: 0x00134BA7 File Offset: 0x00132DA7
		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x06005B78 RID: 23416 RVA: 0x00134BB0 File Offset: 0x00132DB0
		public void Deconstruct(out object key, out object value)
		{
			key = this.Key;
			value = this.Value;
		}

		// Token: 0x04003867 RID: 14439
		private object _key;

		// Token: 0x04003868 RID: 14440
		private object _value;
	}
}
