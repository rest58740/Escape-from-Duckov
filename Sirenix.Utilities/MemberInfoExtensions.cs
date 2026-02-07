using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Sirenix.Utilities
{
	// Token: 0x02000008 RID: 8
	public static class MemberInfoExtensions
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002CE0 File Offset: 0x00000EE0
		public static bool IsDefined<T>(this ICustomAttributeProvider member, bool inherit) where T : Attribute
		{
			bool result;
			try
			{
				result = member.IsDefined(typeof(T), inherit);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002D18 File Offset: 0x00000F18
		public static bool IsDefined<T>(this ICustomAttributeProvider member) where T : Attribute
		{
			return member.IsDefined(false);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002D24 File Offset: 0x00000F24
		public static T GetAttribute<T>(this ICustomAttributeProvider member, bool inherit) where T : Attribute
		{
			T[] array = member.GetAttributes(inherit).ToArray<T>();
			if (array != null && array.Length != 0)
			{
				return array[0];
			}
			return default(T);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002D56 File Offset: 0x00000F56
		public static T GetAttribute<T>(this ICustomAttributeProvider member) where T : Attribute
		{
			return member.GetAttribute(false);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002D5F File Offset: 0x00000F5F
		public static IEnumerable<T> GetAttributes<T>(this ICustomAttributeProvider member) where T : Attribute
		{
			return member.GetAttributes(false);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002D68 File Offset: 0x00000F68
		public static IEnumerable<T> GetAttributes<T>(this ICustomAttributeProvider member, bool inherit) where T : Attribute
		{
			IEnumerable<T> result;
			try
			{
				result = member.GetCustomAttributes(typeof(T), inherit).Cast<T>();
			}
			catch
			{
				result = new T[0];
			}
			return result;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002DAC File Offset: 0x00000FAC
		public static Attribute[] GetAttributes(this ICustomAttributeProvider member)
		{
			Attribute[] result;
			try
			{
				result = member.GetAttributes<Attribute>().ToArray<Attribute>();
			}
			catch
			{
				result = new Attribute[0];
			}
			return result;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002DE4 File Offset: 0x00000FE4
		public static Attribute[] GetAttributes(this ICustomAttributeProvider member, bool inherit)
		{
			Attribute[] result;
			try
			{
				result = member.GetAttributes(inherit).ToArray<Attribute>();
			}
			catch
			{
				result = new Attribute[0];
			}
			return result;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002E1C File Offset: 0x0000101C
		public static string GetNiceName(this MemberInfo member)
		{
			MethodBase methodBase = member as MethodBase;
			string input;
			if (methodBase != null)
			{
				input = methodBase.GetFullName();
			}
			else
			{
				input = member.Name;
			}
			return input.ToTitleCase();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002E50 File Offset: 0x00001050
		public static bool IsStatic(this MemberInfo member)
		{
			FieldInfo fieldInfo = member as FieldInfo;
			if (fieldInfo != null)
			{
				return fieldInfo.IsStatic;
			}
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				if (!propertyInfo.CanRead)
				{
					return propertyInfo.GetSetMethod(true).IsStatic;
				}
				return propertyInfo.GetGetMethod(true).IsStatic;
			}
			else
			{
				MethodBase methodBase = member as MethodBase;
				if (methodBase != null)
				{
					return methodBase.IsStatic;
				}
				EventInfo eventInfo = member as EventInfo;
				if (eventInfo != null)
				{
					return eventInfo.GetRaiseMethod(true).IsStatic;
				}
				Type type = member as Type;
				if (type != null)
				{
					return type.IsSealed && type.IsAbstract;
				}
				string text = string.Format(CultureInfo.InvariantCulture, "Unable to determine IsStatic for member {0}.{1}MemberType was {2} but only fields, properties, methods, events and types are supported.", member.DeclaringType.FullName, member.Name, member.GetType().FullName);
				throw new NotSupportedException(text);
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002F33 File Offset: 0x00001133
		public static bool IsAlias(this MemberInfo memberInfo)
		{
			return memberInfo is MemberAliasFieldInfo || memberInfo is MemberAliasPropertyInfo || memberInfo is MemberAliasMethodInfo;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002F50 File Offset: 0x00001150
		public static MemberInfo DeAlias(this MemberInfo memberInfo, bool throwOnNotAliased = false)
		{
			MemberAliasFieldInfo memberAliasFieldInfo = memberInfo as MemberAliasFieldInfo;
			if (memberAliasFieldInfo != null)
			{
				return memberAliasFieldInfo.AliasedField;
			}
			MemberAliasPropertyInfo memberAliasPropertyInfo = memberInfo as MemberAliasPropertyInfo;
			if (memberAliasPropertyInfo != null)
			{
				return memberAliasPropertyInfo.AliasedProperty;
			}
			MemberAliasMethodInfo memberAliasMethodInfo = memberInfo as MemberAliasMethodInfo;
			if (memberAliasMethodInfo != null)
			{
				return memberAliasMethodInfo.AliasedMethod;
			}
			if (throwOnNotAliased)
			{
				throw new ArgumentException("The member " + memberInfo.GetNiceName() + " was not aliased.");
			}
			return memberInfo;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002FC4 File Offset: 0x000011C4
		public static bool SignaturesAreEqual(this MemberInfo a, MemberInfo b)
		{
			if (a.MemberType != b.MemberType)
			{
				return false;
			}
			if (a.Name != b.Name)
			{
				return false;
			}
			if (a.GetReturnType() != b.GetReturnType())
			{
				return false;
			}
			if (a.IsStatic() != b.IsStatic())
			{
				return false;
			}
			MethodInfo methodInfo = a as MethodInfo;
			MethodInfo methodInfo2 = b as MethodInfo;
			if (methodInfo != null)
			{
				if (methodInfo.IsPublic != methodInfo2.IsPublic)
				{
					return false;
				}
				if (methodInfo.IsPrivate != methodInfo2.IsPrivate)
				{
					return false;
				}
				if (methodInfo.IsPublic != methodInfo2.IsPublic)
				{
					return false;
				}
				ParameterInfo[] parameters = methodInfo.GetParameters();
				ParameterInfo[] parameters2 = methodInfo2.GetParameters();
				if (parameters.Length != parameters2.Length)
				{
					return false;
				}
				for (int i = 0; i < parameters.Length; i++)
				{
					if (parameters[i].ParameterType != parameters2[i].ParameterType)
					{
						return false;
					}
				}
			}
			PropertyInfo propertyInfo = a as PropertyInfo;
			PropertyInfo propertyInfo2 = b as PropertyInfo;
			if (propertyInfo != null)
			{
				MethodInfo[] accessors = propertyInfo.GetAccessors(true);
				MethodInfo[] accessors2 = propertyInfo2.GetAccessors(true);
				if (accessors.Length != accessors2.Length)
				{
					return false;
				}
				if (accessors[0].IsPublic != accessors2[0].IsPublic)
				{
					return false;
				}
				if (accessors.Length > 1 && accessors[1].IsPublic != accessors2[1].IsPublic)
				{
					return false;
				}
			}
			return true;
		}
	}
}
