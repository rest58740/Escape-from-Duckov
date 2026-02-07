using System;
using System.Collections.Generic;

namespace System.Reflection
{
	// Token: 0x020008DC RID: 2268
	public static class RuntimeReflectionExtensions
	{
		// Token: 0x06004B84 RID: 19332 RVA: 0x000F08F9 File Offset: 0x000EEAF9
		public static IEnumerable<FieldInfo> GetRuntimeFields(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x000F0917 File Offset: 0x000EEB17
		public static IEnumerable<MethodInfo> GetRuntimeMethods(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x000F0935 File Offset: 0x000EEB35
		public static IEnumerable<PropertyInfo> GetRuntimeProperties(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06004B87 RID: 19335 RVA: 0x000F0953 File Offset: 0x000EEB53
		public static IEnumerable<EventInfo> GetRuntimeEvents(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return type.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x000F0971 File Offset: 0x000EEB71
		public static FieldInfo GetRuntimeField(this Type type, string name)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return type.GetField(name);
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x000F098E File Offset: 0x000EEB8E
		public static MethodInfo GetRuntimeMethod(this Type type, string name, Type[] parameters)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return type.GetMethod(name, parameters);
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x000F09AC File Offset: 0x000EEBAC
		public static PropertyInfo GetRuntimeProperty(this Type type, string name)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return type.GetProperty(name);
		}

		// Token: 0x06004B8B RID: 19339 RVA: 0x000F09C9 File Offset: 0x000EEBC9
		public static EventInfo GetRuntimeEvent(this Type type, string name)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return type.GetEvent(name);
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x000F09E6 File Offset: 0x000EEBE6
		public static MethodInfo GetRuntimeBaseDefinition(this MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			return method.GetBaseDefinition();
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x000F0A02 File Offset: 0x000EEC02
		public static InterfaceMapping GetRuntimeInterfaceMap(this TypeInfo typeInfo, Type interfaceType)
		{
			if (typeInfo == null)
			{
				throw new ArgumentNullException("typeInfo");
			}
			return typeInfo.GetInterfaceMap(interfaceType);
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x000F0A1F File Offset: 0x000EEC1F
		public static MethodInfo GetMethodInfo(this Delegate del)
		{
			if (del == null)
			{
				throw new ArgumentNullException("del");
			}
			return del.Method;
		}

		// Token: 0x04002F66 RID: 12134
		private const BindingFlags Everything = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
