using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000078 RID: 120
	public static class SerializationPolicies
	{
		// Token: 0x060003E5 RID: 997 RVA: 0x0001AB8C File Offset: 0x00018D8C
		public static bool TryGetByID(string name, out ISerializationPolicy policy)
		{
			if (!(name == "OdinSerializerPolicies.Everything"))
			{
				if (!(name == "OdinSerializerPolicies.Unity"))
				{
					if (!(name == "OdinSerializerPolicies.Strict"))
					{
						policy = null;
					}
					else
					{
						policy = SerializationPolicies.Strict;
					}
				}
				else
				{
					policy = SerializationPolicies.Unity;
				}
			}
			else
			{
				policy = SerializationPolicies.Everything;
			}
			return policy != null;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0001ABE8 File Offset: 0x00018DE8
		public static ISerializationPolicy Everything
		{
			get
			{
				if (SerializationPolicies.everythingPolicy == null)
				{
					object @lock = SerializationPolicies.LOCK;
					lock (@lock)
					{
						if (SerializationPolicies.everythingPolicy == null)
						{
							SerializationPolicies.everythingPolicy = new CustomSerializationPolicy("OdinSerializerPolicies.Everything", true, (MemberInfo member) => member is FieldInfo && (member.IsDefined(true) || !member.IsDefined(true)));
						}
					}
				}
				return SerializationPolicies.everythingPolicy;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0001AC6C File Offset: 0x00018E6C
		public static ISerializationPolicy Unity
		{
			get
			{
				if (SerializationPolicies.unityPolicy == null)
				{
					object @lock = SerializationPolicies.LOCK;
					lock (@lock)
					{
						if (SerializationPolicies.unityPolicy == null)
						{
							Type tupleInterface = typeof(string).Assembly.GetType("System.ITuple") ?? typeof(string).Assembly.GetType("System.ITupleInternal");
							SerializationPolicies.unityPolicy = new CustomSerializationPolicy("OdinSerializerPolicies.Unity", true, delegate(MemberInfo member)
							{
								if (member is PropertyInfo)
								{
									PropertyInfo propertyInfo = member as PropertyInfo;
									if (propertyInfo.GetGetMethod(true) == null || propertyInfo.GetSetMethod(true) == null)
									{
										return false;
									}
								}
								return (!member.IsDefined(true) || member.IsDefined<OdinSerializeAttribute>()) && ((member is FieldInfo && ((member as FieldInfo).IsPublic || (member.DeclaringType.IsNestedPrivate && member.DeclaringType.IsDefined<CompilerGeneratedAttribute>()) || (tupleInterface != null && tupleInterface.IsAssignableFrom(member.DeclaringType)))) || member.IsDefined(false) || member.IsDefined(false) || (UnitySerializationUtility.SerializeReferenceAttributeType != null && member.IsDefined(UnitySerializationUtility.SerializeReferenceAttributeType, false)));
							});
						}
					}
				}
				return SerializationPolicies.unityPolicy;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0001AD24 File Offset: 0x00018F24
		public static ISerializationPolicy Strict
		{
			get
			{
				if (SerializationPolicies.strictPolicy == null)
				{
					object @lock = SerializationPolicies.LOCK;
					lock (@lock)
					{
						if (SerializationPolicies.strictPolicy == null)
						{
							SerializationPolicies.strictPolicy = new CustomSerializationPolicy("OdinSerializerPolicies.Strict", true, (MemberInfo member) => (!(member is PropertyInfo) || ((PropertyInfo)member).IsAutoProperty(false)) && !member.IsDefined<NonSerializedAttribute>() && ((member is FieldInfo && member.DeclaringType.IsNestedPrivate && member.DeclaringType.IsDefined<CompilerGeneratedAttribute>()) || member.IsDefined(false) || member.IsDefined(false) || (UnitySerializationUtility.SerializeReferenceAttributeType != null && member.IsDefined(UnitySerializationUtility.SerializeReferenceAttributeType, false))));
						}
					}
				}
				return SerializationPolicies.strictPolicy;
			}
		}

		// Token: 0x04000160 RID: 352
		private static readonly object LOCK = new object();

		// Token: 0x04000161 RID: 353
		private static volatile ISerializationPolicy everythingPolicy;

		// Token: 0x04000162 RID: 354
		private static volatile ISerializationPolicy unityPolicy;

		// Token: 0x04000163 RID: 355
		private static volatile ISerializationPolicy strictPolicy;
	}
}
