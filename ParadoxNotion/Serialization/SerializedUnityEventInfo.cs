using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace ParadoxNotion.Serialization
{
	// Token: 0x02000092 RID: 146
	[Serializable]
	public class SerializedUnityEventInfo : ISerializedReflectedInfo, ISerializationCallbackReceiver
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x00010F9D File Offset: 0x0000F19D
		public bool isStatic
		{
			get
			{
				if (this._memberInfo is FieldInfo)
				{
					return (this._memberInfo as FieldInfo).IsStatic;
				}
				return this._memberInfo is PropertyInfo && (this._memberInfo as PropertyInfo).IsStatic();
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x00010FDC File Offset: 0x0000F1DC
		public Type memberType
		{
			get
			{
				if (this._memberInfo is FieldInfo)
				{
					return (this._memberInfo as FieldInfo).FieldType;
				}
				if (this._memberInfo is PropertyInfo)
				{
					return (this._memberInfo as PropertyInfo).PropertyType;
				}
				return null;
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001101B File Offset: 0x0000F21B
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this._memberInfo != null)
			{
				this._baseInfo = string.Format("{0}|{1}", this._memberInfo.RTReflectedOrDeclaredType().FullName, this._memberInfo.Name);
			}
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00011058 File Offset: 0x0000F258
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
				this._memberInfo = null;
				return;
			}
			string name = array[1];
			MemberInfo memberInfo = type.RTGetFieldOrProp(name);
			this._memberInfo = null;
			if (memberInfo is FieldInfo && typeof(UnityEventBase).RTIsAssignableFrom((memberInfo as FieldInfo).FieldType))
			{
				this._memberInfo = memberInfo;
				return;
			}
			if (memberInfo is PropertyInfo && typeof(UnityEventBase).RTIsAssignableFrom((memberInfo as PropertyInfo).PropertyType))
			{
				this._memberInfo = memberInfo;
				return;
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00011104 File Offset: 0x0000F304
		public SerializedUnityEventInfo()
		{
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001110C File Offset: 0x0000F30C
		public SerializedUnityEventInfo(FieldInfo info)
		{
			this._memberInfo = info;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001111B File Offset: 0x0000F31B
		public SerializedUnityEventInfo(PropertyInfo info)
		{
			this._memberInfo = info;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001112A File Offset: 0x0000F32A
		public SerializedUnityEventInfo(MemberInfo info)
		{
			if (info is FieldInfo || info is PropertyInfo)
			{
				this._memberInfo = info;
				return;
			}
			throw new Exception("MemberInfo is neither Field nor Property");
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00011154 File Offset: 0x0000F354
		public MemberInfo AsMemberInfo()
		{
			return this._memberInfo;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001115C File Offset: 0x0000F35C
		public string AsString()
		{
			if (this._baseInfo == null)
			{
				return "None";
			}
			return this._baseInfo.Replace("|", ".");
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00011181 File Offset: 0x0000F381
		public override string ToString()
		{
			return this.AsString();
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00011189 File Offset: 0x0000F389
		public static implicit operator FieldInfo(SerializedUnityEventInfo value)
		{
			if (value == null)
			{
				return null;
			}
			return value._memberInfo as FieldInfo;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001119B File Offset: 0x0000F39B
		public static implicit operator PropertyInfo(SerializedUnityEventInfo value)
		{
			if (value == null)
			{
				return null;
			}
			return value._memberInfo as PropertyInfo;
		}

		// Token: 0x040001D1 RID: 465
		[SerializeField]
		private string _baseInfo;

		// Token: 0x040001D2 RID: 466
		[NonSerialized]
		private MemberInfo _memberInfo;
	}
}
