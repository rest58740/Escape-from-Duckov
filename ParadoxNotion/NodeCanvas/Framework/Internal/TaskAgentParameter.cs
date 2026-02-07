using System;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000049 RID: 73
	[fsAutoInstance(false)]
	[Serializable]
	public sealed class TaskAgentParameter : BBParameter<Object>
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00009F62 File Offset: 0x00008162
		public override Type varType
		{
			get
			{
				return this._type ?? typeof(Object);
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00009F78 File Offset: 0x00008178
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00009FB0 File Offset: 0x000081B0
		public new Object value
		{
			get
			{
				Object value = base.value;
				if (value is GameObject)
				{
					return (value as GameObject).transform;
				}
				if (value is Component)
				{
					return (Component)value;
				}
				return null;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00009FB9 File Offset: 0x000081B9
		public override object GetValueBoxed()
		{
			return this.value;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00009FC1 File Offset: 0x000081C1
		public override void SetValueBoxed(object newValue)
		{
			this.value = (newValue as Object);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00009FCF File Offset: 0x000081CF
		public void SetType(Type newType)
		{
			if (typeof(Object).IsAssignableFrom(newType))
			{
				this._type = newType;
			}
		}

		// Token: 0x04000100 RID: 256
		[SerializeField]
		private Type _type;
	}
}
