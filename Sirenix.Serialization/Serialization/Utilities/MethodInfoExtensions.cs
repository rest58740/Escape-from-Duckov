using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000BA RID: 186
	internal static class MethodInfoExtensions
	{
		// Token: 0x06000523 RID: 1315 RVA: 0x00023DE0 File Offset: 0x00021FE0
		public static string GetFullName(this MethodBase method, string extensionMethodPrefix)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = method.IsExtensionMethod();
			if (flag)
			{
				stringBuilder.Append(extensionMethodPrefix);
			}
			stringBuilder.Append(method.Name);
			stringBuilder.Append("(");
			stringBuilder.Append(method.GetParamsNames());
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00023E40 File Offset: 0x00022040
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

		// Token: 0x06000525 RID: 1317 RVA: 0x00023ED3 File Offset: 0x000220D3
		public static string GetFullName(this MethodBase method)
		{
			return method.GetFullName("[ext] ");
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00023EE0 File Offset: 0x000220E0
		public static bool IsExtensionMethod(this MethodBase method)
		{
			Type declaringType = method.DeclaringType;
			return declaringType.IsSealed && !declaringType.IsGenericType && !declaringType.IsNested && method.IsDefined(typeof(ExtensionAttribute), false);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00023F1F File Offset: 0x0002211F
		public static bool IsAliasMethod(this MethodInfo methodInfo)
		{
			return methodInfo is MemberAliasMethodInfo;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00023F2C File Offset: 0x0002212C
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
