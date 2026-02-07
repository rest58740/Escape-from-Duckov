using System;
using System.Reflection;
using UnityEngine;

namespace ParadoxNotion.Serialization
{
	// Token: 0x0200008F RID: 143
	[Serializable]
	public class SerializedFieldInfo : ISerializedReflectedInfo, ISerializationCallbackReceiver
	{
		// Token: 0x060005BF RID: 1471 RVA: 0x00010A57 File Offset: 0x0000EC57
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this._field != null)
			{
				this._baseInfo = string.Format("{0}|{1}", this._field.RTReflectedOrDeclaredType().FullName, this._field.Name);
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00010A94 File Offset: 0x0000EC94
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
				this._field = null;
				return;
			}
			string name = array[1];
			this._field = type.RTGetField(name, false);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00010AE7 File Offset: 0x0000ECE7
		public SerializedFieldInfo()
		{
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00010AEF File Offset: 0x0000ECEF
		public SerializedFieldInfo(FieldInfo info)
		{
			this._field = info;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00010AFE File Offset: 0x0000ECFE
		public MemberInfo AsMemberInfo()
		{
			return this._field;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00010B06 File Offset: 0x0000ED06
		public string AsString()
		{
			if (this._baseInfo == null)
			{
				return "None";
			}
			return this._baseInfo.Replace("|", ".");
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00010B2B File Offset: 0x0000ED2B
		public override string ToString()
		{
			return this.AsString();
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00010B33 File Offset: 0x0000ED33
		public static implicit operator FieldInfo(SerializedFieldInfo value)
		{
			if (value == null)
			{
				return null;
			}
			return value._field;
		}

		// Token: 0x040001C8 RID: 456
		[SerializeField]
		private string _baseInfo;

		// Token: 0x040001C9 RID: 457
		[NonSerialized]
		private FieldInfo _field;
	}
}
