using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pathfinding.Util
{
	// Token: 0x0200027E RID: 638
	internal static class AssemblySearcher
	{
		// Token: 0x06000F28 RID: 3880 RVA: 0x0005D850 File Offset: 0x0005BA50
		public static List<Type> FindTypesInheritingFrom<T>()
		{
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				string name = assembly.GetName().Name;
				if (!name.StartsWith("Unity.") && !name.StartsWith("UnityEngine.") && !(name == "UnityEngine") && !name.StartsWith("UnityEditor.") && !(name == "UnityEditor") && !name.StartsWith("Mono.") && !name.StartsWith("System.") && !(name == "System") && !name.StartsWith("mscorlib") && !name.StartsWith("I18N") && !(name == "netstandard") && !(name == "nunit.framework"))
				{
					Type[] array = null;
					try
					{
						array = assembly.GetTypes();
					}
					catch
					{
						goto IL_158;
					}
					foreach (Type type in array)
					{
						Type baseType = type.BaseType;
						while (baseType != null)
						{
							if (object.Equals(baseType, typeof(T)))
							{
								list.Add(type);
								break;
							}
							baseType = baseType.BaseType;
						}
					}
				}
				IL_158:;
			}
			return list;
		}
	}
}
