using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ParadoxNotion.Serialization
{
	// Token: 0x02000090 RID: 144
	[Serializable]
	public class SerializedMethodInfo : ISerializedMethodBaseInfo, ISerializedReflectedInfo, ISerializationCallbackReceiver
	{
		// Token: 0x060005C7 RID: 1479 RVA: 0x00010B40 File Offset: 0x0000ED40
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this._hasChanged = false;
			if (this._method != null)
			{
				this._baseInfo = string.Format("{0}|{1}|{2}", this._method.RTReflectedOrDeclaredType().FullName, this._method.Name, this._method.ReturnType.FullName);
				this._paramsInfo = string.Join("|", (from p in this._method.GetParameters()
				select p.ParameterType.FullName).ToArray<string>());
				string genericArgumentsInfo;
				if (!this._method.IsGenericMethod)
				{
					genericArgumentsInfo = null;
				}
				else
				{
					genericArgumentsInfo = string.Join("|", (from a in this._method.RTGetGenericArguments()
					select a.FullName).ToArray<string>());
				}
				this._genericArgumentsInfo = genericArgumentsInfo;
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00010C38 File Offset: 0x0000EE38
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this._hasChanged = false;
			if (this._baseInfo == null)
			{
				return;
			}
			string[] array = this._baseInfo.Split('|', 0);
			Type type = ReflectionTools.GetType(array[0], true, null);
			if (type == null)
			{
				this._method = null;
				return;
			}
			string name = array[1];
			Type returnType = (array.Length >= 3) ? ReflectionTools.GetType(array[2], true, null) : null;
			bool isSerializedGeneric = !string.IsNullOrEmpty(this._genericArgumentsInfo);
			string[] array2 = string.IsNullOrEmpty(this._paramsInfo) ? null : this._paramsInfo.Split('|', 0);
			Type[] parameterTypes = (array2 != null) ? new Type[array2.Length] : Type.EmptyTypes;
			bool flag = false;
			for (int i = 0; i < parameterTypes.Length; i++)
			{
				Type type2 = ReflectionTools.GetType(array2[i], true, null);
				if (type2 == null)
				{
					flag = true;
					break;
				}
				parameterTypes[i] = type2;
			}
			if (!flag)
			{
				if (isSerializedGeneric)
				{
					string[] array3 = this._genericArgumentsInfo.Split('|', 0);
					Type[] array4 = new Type[array3.Length];
					bool flag2 = false;
					for (int j = 0; j < array4.Length; j++)
					{
						Type type3 = ReflectionTools.GetType(array3[j], true, null);
						if (type3 == null)
						{
							flag2 = true;
							break;
						}
						array4[j] = type3;
					}
					if (!flag2)
					{
						this._method = type.RTGetMethod(name, parameterTypes, returnType, array4);
					}
				}
				else
				{
					this._method = type.RTGetMethod(name, parameterTypes, returnType, null);
				}
			}
			if (this._method == null)
			{
				this._hasChanged = true;
				MethodInfo[] source = type.RTGetMethods();
				this._method = source.FirstOrDefault((MethodInfo m) => m.Name == name && m.GetParameters().Length == parameterTypes.Length && isSerializedGeneric == m.IsGenericMethod);
				if (this._method == null)
				{
					this._method = source.FirstOrDefault((MethodInfo m) => m.Name == name);
				}
				if (this._method != null && this._method.IsGenericMethod)
				{
					Type type4 = isSerializedGeneric ? ReflectionTools.GetType(this._genericArgumentsInfo.Split('|', 0).First<string>(), true, null) : this._method.GetFirstGenericParameterConstraintType();
					this._method = this._method.MakeGenericMethod(new Type[]
					{
						type4
					});
				}
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00010E9A File Offset: 0x0000F09A
		public SerializedMethodInfo()
		{
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00010EA2 File Offset: 0x0000F0A2
		public SerializedMethodInfo(MethodInfo method)
		{
			this._hasChanged = false;
			this._method = method;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00010EB8 File Offset: 0x0000F0B8
		public MemberInfo AsMemberInfo()
		{
			return this._method;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00010EC0 File Offset: 0x0000F0C0
		public MethodBase GetMethodBase()
		{
			return this._method;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00010EC8 File Offset: 0x0000F0C8
		public bool HasChanged()
		{
			return this._hasChanged;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00010ED0 File Offset: 0x0000F0D0
		public string AsString()
		{
			return string.Format("{0} ({1})", this._baseInfo.Replace("|", "."), this._paramsInfo.Replace("|", ", "));
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00010F06 File Offset: 0x0000F106
		public override string ToString()
		{
			return this.AsString();
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00010F0E File Offset: 0x0000F10E
		public static implicit operator MethodInfo(SerializedMethodInfo value)
		{
			if (value == null)
			{
				return null;
			}
			return value._method;
		}

		// Token: 0x040001CA RID: 458
		[SerializeField]
		private string _baseInfo;

		// Token: 0x040001CB RID: 459
		[SerializeField]
		private string _paramsInfo;

		// Token: 0x040001CC RID: 460
		[SerializeField]
		private string _genericArgumentsInfo;

		// Token: 0x040001CD RID: 461
		[NonSerialized]
		private MethodInfo _method;

		// Token: 0x040001CE RID: 462
		[NonSerialized]
		private bool _hasChanged;
	}
}
