using System;
using ParadoxNotion;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x0200004B RID: 75
	[Serializable]
	public class BBObjectParameter : BBParameter<object>, ISerializationCallbackReceiver
	{
		// Token: 0x0600038F RID: 911 RVA: 0x0000A044 File Offset: 0x00008244
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this.type != null)
			{
				this._type = this.type.FullName;
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000A065 File Offset: 0x00008265
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.type = ReflectionTools.GetType(this._type, true, null);
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000A07A File Offset: 0x0000827A
		// (set) Token: 0x06000392 RID: 914 RVA: 0x0000A082 File Offset: 0x00008282
		private Type type { get; set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000A08B File Offset: 0x0000828B
		public override Type varType
		{
			get
			{
				if (!(this.type != null))
				{
					return typeof(object);
				}
				return this.type;
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000A0AC File Offset: 0x000082AC
		public BBObjectParameter()
		{
			this.SetType(typeof(object));
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000A0C4 File Offset: 0x000082C4
		public BBObjectParameter(Type t)
		{
			this.SetType(t);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000A0D3 File Offset: 0x000082D3
		public BBObjectParameter(BBParameter source)
		{
			if (source != null)
			{
				this.type = source.varType;
				this._value = source.value;
				base.name = source.name;
				base.targetVariableID = source.targetVariableID;
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000A110 File Offset: 0x00008310
		public void SetType(Type t)
		{
			if (t == null)
			{
				t = typeof(object);
			}
			if (t != this.type || (t.RTIsValueType() && this._value == null))
			{
				this._value = (t.RTIsValueType() ? Activator.CreateInstance(t) : null);
			}
			this.type = t;
		}

		// Token: 0x04000104 RID: 260
		[SerializeField]
		private string _type;
	}
}
