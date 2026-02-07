using System;
using System.Reflection;
using UnityEngine;

namespace ParadoxNotion.Serialization
{
	// Token: 0x0200008E RID: 142
	[Serializable]
	public class SerializedEventInfo : ISerializedReflectedInfo, ISerializationCallbackReceiver
	{
		// Token: 0x060005B7 RID: 1463 RVA: 0x00010973 File Offset: 0x0000EB73
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this._event != null)
			{
				this._baseInfo = string.Format("{0}|{1}", this._event.RTReflectedOrDeclaredType().FullName, this._event.Name);
			}
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x000109B0 File Offset: 0x0000EBB0
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this._baseInfo == null)
			{
				return;
			}
			string[] array = this._baseInfo.Split('|', 0);
			Type type = ReflectionTools.GetType(array[0], true, null);
			if (type == null)
			{
				this._event = null;
				return;
			}
			string name = array[1];
			this._event = type.RTGetEvent(name);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00010A02 File Offset: 0x0000EC02
		public SerializedEventInfo()
		{
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00010A0A File Offset: 0x0000EC0A
		public SerializedEventInfo(EventInfo info)
		{
			this._event = info;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00010A19 File Offset: 0x0000EC19
		public MemberInfo AsMemberInfo()
		{
			return this._event;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00010A21 File Offset: 0x0000EC21
		public string AsString()
		{
			if (this._baseInfo == null)
			{
				return null;
			}
			return this._baseInfo.Replace("|", ".");
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00010A42 File Offset: 0x0000EC42
		public override string ToString()
		{
			return this.AsString();
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00010A4A File Offset: 0x0000EC4A
		public static implicit operator EventInfo(SerializedEventInfo value)
		{
			if (value == null)
			{
				return null;
			}
			return value._event;
		}

		// Token: 0x040001C6 RID: 454
		[SerializeField]
		private string _baseInfo;

		// Token: 0x040001C7 RID: 455
		[NonSerialized]
		private EventInfo _event;
	}
}
