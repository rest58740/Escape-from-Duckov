using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sirenix.Utilities
{
	// Token: 0x02000009 RID: 9
	public static class MethodInfoExtensions
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00003118 File Offset: 0x00001318
		public static string GetFullName(this MethodBase method, string extensionMethodPrefix)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = method.IsExtensionMethod();
			if (flag)
			{
				stringBuilder.Append(extensionMethodPrefix);
			}
			stringBuilder.Append(method.Name);
			if (method.IsGenericMethod)
			{
				Type[] genericArguments = method.GetGenericArguments();
				stringBuilder.Append("<");
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(genericArguments[i].GetNiceName());
				}
				stringBuilder.Append(">");
			}
			stringBuilder.Append("(");
			stringBuilder.Append(method.GetParamsNames());
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000031C8 File Offset: 0x000013C8
		public static string GetParamsNames(this MethodBase method)
		{
			ParameterInfo[] array = method.IsExtensionMethod() ? method.GetParameters().Skip(1).ToArray<ParameterInfo>() : method.GetParameters();
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				ParameterInfo parameterInfo = array[i];
				string niceName = parameterInfo.ParameterType.GetNiceName();
				stringBuilder.Append(niceName);
				stringBuilder.Append(" ");
				stringBuilder.Append(parameterInfo.Name);
				if (i < num - 1)
				{
					stringBuilder.Append(", ");
				}
				i++;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000325B File Offset: 0x0000145B
		public static string GetFullName(this MethodBase method)
		{
			return method.GetFullName("[ext] ");
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003268 File Offset: 0x00001468
		public static bool IsExtensionMethod(this MethodBase method)
		{
			Type declaringType = method.DeclaringType;
			return declaringType.IsSealed && !declaringType.IsGenericType && !declaringType.IsNested && method.IsDefined(typeof(ExtensionAttribute), false);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000032A7 File Offset: 0x000014A7
		public static bool IsAliasMethod(this MethodInfo methodInfo)
		{
			return methodInfo is MemberAliasMethodInfo;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000032B4 File Offset: 0x000014B4
		public static MethodInfo DeAliasMethod(this MethodInfo methodInfo, bool throwOnNotAliased = false)
		{
			MemberAliasMethodInfo memberAliasMethodInfo = methodInfo as MemberAliasMethodInfo;
			if (memberAliasMethodInfo != null)
			{
				while (memberAliasMethodInfo.AliasedMethod is MemberAliasMethodInfo)
				{
					memberAliasMethodInfo = (memberAliasMethodInfo.AliasedMethod as MemberAliasMethodInfo);
				}
				return memberAliasMethodInfo.AliasedMethod;
			}
			if (throwOnNotAliased)
			{
				throw new ArgumentException("The method " + methodInfo.GetNiceName() + " was not aliased.");
			}
			return methodInfo;
		}
	}
}
