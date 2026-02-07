using System;
using System.Reflection;
using UnityEngine;

namespace ParadoxNotion.Serialization
{
	// Token: 0x02000091 RID: 145
	[Serializable]
	public class SerializedTypeInfo : ISerializedReflectedInfo, ISerializationCallbackReceiver
	{
		// Token: 0x060005D1 RID: 1489 RVA: 0x00010F1B File Offset: 0x0000F11B
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this._type != null)
			{
				this._baseInfo = this._type.FullName;
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00010F3C File Offset: 0x0000F13C
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this._baseInfo == null)
			{
				return;
			}
			this._type = ReflectionTools.GetType(this._baseInfo, true, null);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00010F5A File Offset: 0x0000F15A
		public SerializedTypeInfo()
		{
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00010F62 File Offset: 0x0000F162
		public SerializedTypeInfo(Type info)
		{
			this._baseInfo = null;
			this._type = info;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00010F78 File Offset: 0x0000F178
		public MemberInfo AsMemberInfo()
		{
			return this._type;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00010F80 File Offset: 0x0000F180
		public string AsString()
		{
			return this._baseInfo;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00010F88 File Offset: 0x0000F188
		public override string ToString()
		{
			return this._baseInfo;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00010F90 File Offset: 0x0000F190
		public static implicit operator Type(SerializedTypeInfo value)
		{
			if (value == null)
			{
				return null;
			}
			return value._type;
		}

		// Token: 0x040001CF RID: 463
		[SerializeField]
		private string _baseInfo;

		// Token: 0x040001D0 RID: 464
		[NonSerialized]
		private Type _type;
	}
}
