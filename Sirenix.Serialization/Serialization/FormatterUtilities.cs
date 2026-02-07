using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Sirenix.Serialization.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sirenix.Serialization
{
	// Token: 0x02000065 RID: 101
	public static class FormatterUtilities
	{
		// Token: 0x06000362 RID: 866 RVA: 0x00018014 File Offset: 0x00016214
		public static Dictionary<string, MemberInfo> GetSerializableMembersMap(Type type, ISerializationPolicy policy)
		{
			if (policy == null)
			{
				policy = SerializationPolicies.Strict;
			}
			object @lock = FormatterUtilities.LOCK;
			Dictionary<string, MemberInfo> dictionary;
			lock (@lock)
			{
				if (!FormatterUtilities.MemberMapCache.TryGetInnerValue(policy, type, out dictionary))
				{
					dictionary = FormatterUtilities.FindSerializableMembersMap(type, policy);
					FormatterUtilities.MemberMapCache.AddInner(policy, type, dictionary);
				}
			}
			return dictionary;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00018080 File Offset: 0x00016280
		public static MemberInfo[] GetSerializableMembers(Type type, ISerializationPolicy policy)
		{
			if (policy == null)
			{
				policy = SerializationPolicies.Strict;
			}
			object @lock = FormatterUtilities.LOCK;
			MemberInfo[] array;
			lock (@lock)
			{
				if (!FormatterUtilities.MemberArrayCache.TryGetInnerValue(policy, type, out array))
				{
					List<MemberInfo> list = new List<MemberInfo>();
					FormatterUtilities.FindSerializableMembers(type, list, policy);
					array = list.ToArray();
					FormatterUtilities.MemberArrayCache.AddInner(policy, type, array);
				}
			}
			return array;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000180F8 File Offset: 0x000162F8
		public static Object CreateUnityNull(Type nullType, Type owningType)
		{
			if (nullType == null || owningType == null)
			{
				throw new ArgumentNullException();
			}
			if (!nullType.ImplementsOrInherits(typeof(Object)))
			{
				throw new ArgumentException("Type " + nullType.Name + " is not a Unity object.");
			}
			if (!owningType.ImplementsOrInherits(typeof(Object)))
			{
				throw new ArgumentException("Type " + owningType.Name + " is not a Unity object.");
			}
			Object @object = (Object)FormatterServices.GetUninitializedObject(nullType);
			if (FormatterUtilities.UnityObjectRuntimeErrorStringField != null)
			{
				FormatterUtilities.UnityObjectRuntimeErrorStringField.SetValue(@object, string.Format(CultureInfo.InvariantCulture, "The variable nullValue of {0} has not been assigned.\r\nYou probably need to assign the nullValue variable of the {0} script in the inspector.", owningType.Name));
			}
			return @object;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x000181B4 File Offset: 0x000163B4
		public static bool IsPrimitiveType(Type type)
		{
			return type.IsPrimitive || type.IsEnum || type == typeof(decimal) || type == typeof(string) || type == typeof(Guid);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00018207 File Offset: 0x00016407
		public static bool IsPrimitiveArrayType(Type type)
		{
			return FormatterUtilities.PrimitiveArrayTypes.Contains(type);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00018214 File Offset: 0x00016414
		public static Type GetContainedType(MemberInfo member)
		{
			if (member is FieldInfo)
			{
				return (member as FieldInfo).FieldType;
			}
			if (member is PropertyInfo)
			{
				return (member as PropertyInfo).PropertyType;
			}
			throw new ArgumentException("Can't get the contained type of a " + member.GetType().Name);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00018264 File Offset: 0x00016464
		public static object GetMemberValue(MemberInfo member, object obj)
		{
			if (member is FieldInfo)
			{
				return (member as FieldInfo).GetValue(obj);
			}
			if (member is PropertyInfo)
			{
				return (member as PropertyInfo).GetGetMethod(true).Invoke(obj, null);
			}
			throw new ArgumentException("Can't get the value of a " + member.GetType().Name);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x000182BC File Offset: 0x000164BC
		public static void SetMemberValue(MemberInfo member, object obj, object value)
		{
			if (member is FieldInfo)
			{
				(member as FieldInfo).SetValue(obj, value);
				return;
			}
			if (!(member is PropertyInfo))
			{
				throw new ArgumentException("Can't set the value of a " + member.GetType().Name);
			}
			MethodInfo setMethod = (member as PropertyInfo).GetSetMethod(true);
			if (setMethod != null)
			{
				setMethod.Invoke(obj, new object[]
				{
					value
				});
				return;
			}
			throw new ArgumentException("Property " + member.Name + " has no setter");
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00018348 File Offset: 0x00016548
		private static Dictionary<string, MemberInfo> FindSerializableMembersMap(Type type, ISerializationPolicy policy)
		{
			Dictionary<string, MemberInfo> dictionary = FormatterUtilities.GetSerializableMembers(type, policy).ToDictionary((MemberInfo n) => n.Name, (MemberInfo n) => n);
			foreach (MemberInfo memberInfo in dictionary.Values.ToList<MemberInfo>())
			{
				IEnumerable<FormerlySerializedAsAttribute> attributes = memberInfo.GetAttributes<FormerlySerializedAsAttribute>();
				foreach (FormerlySerializedAsAttribute formerlySerializedAsAttribute in attributes)
				{
					if (!dictionary.ContainsKey(formerlySerializedAsAttribute.oldName))
					{
						dictionary.Add(formerlySerializedAsAttribute.oldName, memberInfo);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00018440 File Offset: 0x00016640
		private static void FindSerializableMembers(Type type, List<MemberInfo> members, ISerializationPolicy policy)
		{
			if (type.BaseType != typeof(object) && type.BaseType != null)
			{
				FormatterUtilities.FindSerializableMembers(type.BaseType, members, policy);
			}
			using (IEnumerator<MemberInfo> enumerator = (from n in type.GetMembers(54)
			where n is FieldInfo || n is PropertyInfo
			select n).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MemberInfo member = enumerator.Current;
					if (policy.ShouldSerializeMember(member))
					{
						bool flag = members.Any((MemberInfo n) => n.Name == member.Name);
						if (FormatterUtilities.MemberIsPrivate(member) && flag)
						{
							members.Add(FormatterUtilities.GetPrivateMemberAlias(member, null, null));
						}
						else if (flag)
						{
							members.Add(FormatterUtilities.GetPrivateMemberAlias(member, null, null));
						}
						else
						{
							members.Add(member);
						}
					}
				}
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00018558 File Offset: 0x00016758
		internal static MemberInfo GetPrivateMemberAlias(MemberInfo member, string prefixString = null, string separatorString = null)
		{
			if (member is FieldInfo)
			{
				if (separatorString != null)
				{
					return new MemberAliasFieldInfo(member as FieldInfo, prefixString ?? member.DeclaringType.Name, separatorString);
				}
				return new MemberAliasFieldInfo(member as FieldInfo, prefixString ?? member.DeclaringType.Name);
			}
			else if (member is PropertyInfo)
			{
				if (separatorString != null)
				{
					return new MemberAliasPropertyInfo(member as PropertyInfo, prefixString ?? member.DeclaringType.Name, separatorString);
				}
				return new MemberAliasPropertyInfo(member as PropertyInfo, prefixString ?? member.DeclaringType.Name);
			}
			else
			{
				if (!(member is MethodInfo))
				{
					throw new NotImplementedException();
				}
				if (separatorString != null)
				{
					return new MemberAliasMethodInfo(member as MethodInfo, prefixString ?? member.DeclaringType.Name, separatorString);
				}
				return new MemberAliasMethodInfo(member as MethodInfo, prefixString ?? member.DeclaringType.Name);
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00018638 File Offset: 0x00016838
		private static bool MemberIsPrivate(MemberInfo member)
		{
			if (member is FieldInfo)
			{
				return (member as FieldInfo).IsPrivate;
			}
			if (member is PropertyInfo)
			{
				PropertyInfo propertyInfo = member as PropertyInfo;
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				MethodInfo setMethod = propertyInfo.GetSetMethod();
				return getMethod != null && setMethod != null && getMethod.IsPrivate && setMethod.IsPrivate;
			}
			if (member is MethodInfo)
			{
				return (member as MethodInfo).IsPrivate;
			}
			throw new NotImplementedException();
		}

		// Token: 0x04000130 RID: 304
		private static readonly DoubleLookupDictionary<ISerializationPolicy, Type, MemberInfo[]> MemberArrayCache = new DoubleLookupDictionary<ISerializationPolicy, Type, MemberInfo[]>();

		// Token: 0x04000131 RID: 305
		private static readonly DoubleLookupDictionary<ISerializationPolicy, Type, Dictionary<string, MemberInfo>> MemberMapCache = new DoubleLookupDictionary<ISerializationPolicy, Type, Dictionary<string, MemberInfo>>();

		// Token: 0x04000132 RID: 306
		private static readonly object LOCK = new object();

		// Token: 0x04000133 RID: 307
		private static readonly HashSet<Type> PrimitiveArrayTypes = new HashSet<Type>(FastTypeComparer.Instance)
		{
			typeof(char),
			typeof(sbyte),
			typeof(short),
			typeof(int),
			typeof(long),
			typeof(byte),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
			typeof(decimal),
			typeof(bool),
			typeof(float),
			typeof(double),
			typeof(Guid)
		};

		// Token: 0x04000134 RID: 308
		private static readonly FieldInfo UnityObjectRuntimeErrorStringField;

		// Token: 0x04000135 RID: 309
		private const string UnityObjectRuntimeErrorString = "The variable nullValue of {0} has not been assigned.\r\nYou probably need to assign the nullValue variable of the {0} script in the inspector.";
	}
}
