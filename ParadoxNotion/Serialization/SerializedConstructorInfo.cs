using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ParadoxNotion.Serialization
{
	// Token: 0x0200008D RID: 141
	[Serializable]
	public class SerializedConstructorInfo : ISerializedMethodBaseInfo, ISerializedReflectedInfo, ISerializationCallbackReceiver
	{
		// Token: 0x060005AD RID: 1453 RVA: 0x00010744 File Offset: 0x0000E944
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this._hasChanged = false;
			if (this._constructor != null)
			{
				this._baseInfo = this._constructor.RTReflectedOrDeclaredType().FullName + "|$Constructor";
				this._paramsInfo = string.Join("|", (from p in this._constructor.GetParameters()
				select p.ParameterType.FullName).ToArray<string>());
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x000107CC File Offset: 0x0000E9CC
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this._hasChanged = false;
			if (this._baseInfo == null)
			{
				return;
			}
			Type type = ReflectionTools.GetType(this._baseInfo.Split('|', 0)[0], true, null);
			if (type == null)
			{
				this._constructor = null;
				return;
			}
			string[] array = string.IsNullOrEmpty(this._paramsInfo) ? null : this._paramsInfo.Split('|', 0);
			Type[] parameterTypes = (array != null) ? new Type[array.Length] : Type.EmptyTypes;
			bool flag = false;
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					Type type2 = ReflectionTools.GetType(array[i], true, null);
					if (type2 == null)
					{
						flag = true;
						break;
					}
					parameterTypes[i] = type2;
				}
			}
			if (!flag)
			{
				this._constructor = type.RTGetConstructor(parameterTypes);
			}
			if (this._constructor == null)
			{
				this._hasChanged = true;
				ConstructorInfo[] source = type.RTGetConstructors();
				this._constructor = source.FirstOrDefault((ConstructorInfo c) => c.GetParameters().Length == parameterTypes.Length);
				if (this._constructor == null)
				{
					this._constructor = source.FirstOrDefault<ConstructorInfo>();
				}
			}
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x000108F2 File Offset: 0x0000EAF2
		public SerializedConstructorInfo()
		{
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x000108FA File Offset: 0x0000EAFA
		public SerializedConstructorInfo(ConstructorInfo constructor)
		{
			this._hasChanged = false;
			this._constructor = constructor;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00010910 File Offset: 0x0000EB10
		public MemberInfo AsMemberInfo()
		{
			return this._constructor;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00010918 File Offset: 0x0000EB18
		public MethodBase GetMethodBase()
		{
			return this._constructor;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00010920 File Offset: 0x0000EB20
		public bool HasChanged()
		{
			return this._hasChanged;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00010928 File Offset: 0x0000EB28
		public string AsString()
		{
			return string.Format("{0} ({1})", this._baseInfo.Replace("|", "."), this._paramsInfo.Replace("|", ", "));
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001095E File Offset: 0x0000EB5E
		public override string ToString()
		{
			return this.AsString();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00010966 File Offset: 0x0000EB66
		public static implicit operator ConstructorInfo(SerializedConstructorInfo value)
		{
			if (value == null)
			{
				return null;
			}
			return value._constructor;
		}

		// Token: 0x040001C2 RID: 450
		[SerializeField]
		private string _baseInfo;

		// Token: 0x040001C3 RID: 451
		[SerializeField]
		private string _paramsInfo;

		// Token: 0x040001C4 RID: 452
		[NonSerialized]
		private ConstructorInfo _constructor;

		// Token: 0x040001C5 RID: 453
		[NonSerialized]
		private bool _hasChanged;
	}
}
