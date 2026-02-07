using System;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x02000062 RID: 98
	[Serializable]
	public sealed class DynamicParameterDefinition : ISerializationCallbackReceiver
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x0000A3EF File Offset: 0x000085EF
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this.type != null)
			{
				this._type = this.type.FullName;
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000A410 File Offset: 0x00008610
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.type = ReflectionTools.GetType(this._type, true, null);
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x0000A425 File Offset: 0x00008625
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x0000A446 File Offset: 0x00008646
		public string ID
		{
			get
			{
				if (string.IsNullOrEmpty(this._ID))
				{
					this._ID = this.name;
				}
				return this._ID;
			}
			private set
			{
				this._ID = value;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0000A44F File Offset: 0x0000864F
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x0000A457 File Offset: 0x00008657
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000A460 File Offset: 0x00008660
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x0000A468 File Offset: 0x00008668
		public Type type { get; set; }

		// Token: 0x06000406 RID: 1030 RVA: 0x0000A471 File Offset: 0x00008671
		public DynamicParameterDefinition()
		{
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000A47C File Offset: 0x0000867C
		public DynamicParameterDefinition(string name, Type type)
		{
			this.ID = Guid.NewGuid().ToString();
			this.name = name;
			this.type = type;
		}

		// Token: 0x04000110 RID: 272
		[SerializeField]
		private string _ID;

		// Token: 0x04000111 RID: 273
		[SerializeField]
		private string _name;

		// Token: 0x04000112 RID: 274
		[SerializeField]
		private string _type;
	}
}
