using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x0200005F RID: 95
	public class DefaultSerializationBinder : TwoWaySerializationBinder
	{
		// Token: 0x06000338 RID: 824 RVA: 0x0001701C File Offset: 0x0001521C
		static DefaultSerializationBinder()
		{
			AppDomain.CurrentDomain.AssemblyLoad += delegate(object sender, AssemblyLoadEventArgs args)
			{
				object assembly_REGISTER_QUEUE_LOCK2 = DefaultSerializationBinder.ASSEMBLY_REGISTER_QUEUE_LOCK;
				lock (assembly_REGISTER_QUEUE_LOCK2)
				{
					DefaultSerializationBinder.assemblyLoadEventsQueuedForRegister.Add(args);
				}
			};
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				object assembly_REGISTER_QUEUE_LOCK = DefaultSerializationBinder.ASSEMBLY_REGISTER_QUEUE_LOCK;
				lock (assembly_REGISTER_QUEUE_LOCK)
				{
					DefaultSerializationBinder.assembliesQueuedForRegister.Add(assembly);
				}
			}
			object assembly_LOOKUP_LOCK = DefaultSerializationBinder.ASSEMBLY_LOOKUP_LOCK;
			lock (assembly_LOOKUP_LOCK)
			{
				DefaultSerializationBinder.customTypeNameToTypeBindings["System.Reflection.MonoMethod"] = typeof(MethodInfo);
				DefaultSerializationBinder.customTypeNameToTypeBindings["System.Reflection.MonoMethod, mscorlib"] = typeof(MethodInfo);
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00017164 File Offset: 0x00015364
		private static void RegisterAllQueuedAssembliesRepeating()
		{
			while (DefaultSerializationBinder.RegisterQueuedAssemblies())
			{
			}
			while (DefaultSerializationBinder.RegisterQueuedAssemblyLoadEvents())
			{
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00017174 File Offset: 0x00015374
		private static bool RegisterQueuedAssemblies()
		{
			Assembly[] array = null;
			object assembly_REGISTER_QUEUE_LOCK = DefaultSerializationBinder.ASSEMBLY_REGISTER_QUEUE_LOCK;
			lock (assembly_REGISTER_QUEUE_LOCK)
			{
				if (DefaultSerializationBinder.assembliesQueuedForRegister.Count > 0)
				{
					array = DefaultSerializationBinder.assembliesQueuedForRegister.ToArray();
					DefaultSerializationBinder.assembliesQueuedForRegister.Clear();
				}
			}
			if (array == null)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				DefaultSerializationBinder.RegisterAssembly(array[i]);
			}
			return true;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000171F0 File Offset: 0x000153F0
		private static bool RegisterQueuedAssemblyLoadEvents()
		{
			AssemblyLoadEventArgs[] array = null;
			object assembly_REGISTER_QUEUE_LOCK = DefaultSerializationBinder.ASSEMBLY_REGISTER_QUEUE_LOCK;
			lock (assembly_REGISTER_QUEUE_LOCK)
			{
				if (DefaultSerializationBinder.assemblyLoadEventsQueuedForRegister.Count > 0)
				{
					array = DefaultSerializationBinder.assemblyLoadEventsQueuedForRegister.ToArray();
					DefaultSerializationBinder.assemblyLoadEventsQueuedForRegister.Clear();
				}
			}
			if (array == null)
			{
				return false;
			}
			int i = 0;
			while (i < array.Length)
			{
				AssemblyLoadEventArgs assemblyLoadEventArgs = array[i];
				Assembly loadedAssembly;
				try
				{
					loadedAssembly = assemblyLoadEventArgs.LoadedAssembly;
				}
				catch
				{
					goto IL_63;
				}
				goto IL_5C;
				IL_63:
				i++;
				continue;
				IL_5C:
				DefaultSerializationBinder.RegisterAssembly(loadedAssembly);
				goto IL_63;
			}
			return true;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00017288 File Offset: 0x00015488
		private static void RegisterAssembly(Assembly assembly)
		{
			string name;
			try
			{
				name = assembly.GetName().Name;
			}
			catch
			{
				return;
			}
			bool flag = false;
			object assembly_LOOKUP_LOCK = DefaultSerializationBinder.ASSEMBLY_LOOKUP_LOCK;
			lock (assembly_LOOKUP_LOCK)
			{
				if (!DefaultSerializationBinder.assemblyNameLookUp.ContainsKey(name))
				{
					DefaultSerializationBinder.assemblyNameLookUp.Add(name, assembly);
					flag = true;
				}
			}
			if (flag)
			{
				try
				{
					object[] array = assembly.SafeGetCustomAttributes(typeof(BindTypeNameToTypeAttribute), false);
					if (array != null)
					{
						for (int i = 0; i < array.Length; i++)
						{
							BindTypeNameToTypeAttribute bindTypeNameToTypeAttribute = array[i] as BindTypeNameToTypeAttribute;
							if (bindTypeNameToTypeAttribute != null && bindTypeNameToTypeAttribute.NewType != null)
							{
								object assembly_LOOKUP_LOCK2 = DefaultSerializationBinder.ASSEMBLY_LOOKUP_LOCK;
								lock (assembly_LOOKUP_LOCK2)
								{
									DefaultSerializationBinder.customTypeNameToTypeBindings[bindTypeNameToTypeAttribute.OldTypeName] = bindTypeNameToTypeAttribute.NewType;
								}
							}
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000173A4 File Offset: 0x000155A4
		public override string BindToName(Type type, DebugContext debugContext = null)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			object typetoname_LOCK = DefaultSerializationBinder.TYPETONAME_LOCK;
			string text;
			lock (typetoname_LOCK)
			{
				if (!DefaultSerializationBinder.nameMap.TryGetValue(type, ref text))
				{
					if (type.IsGenericType)
					{
						List<Type> list = type.GetGenericArguments().ToList<Type>();
						HashSet<Assembly> hashSet = new HashSet<Assembly>();
						while (list.Count > 0)
						{
							Type type2 = list[0];
							if (type2.IsGenericType)
							{
								list.AddRange(type2.GetGenericArguments());
							}
							hashSet.Add(type2.Assembly);
							list.RemoveAt(0);
						}
						text = type.FullName + ", " + type.Assembly.GetName().Name;
						using (HashSet<Assembly>.Enumerator enumerator = hashSet.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Assembly assembly = enumerator.Current;
								text = text.Replace(assembly.FullName, assembly.GetName().Name);
							}
							goto IL_153;
						}
					}
					if (type.IsDefined(typeof(CompilerGeneratedAttribute), false))
					{
						text = type.FullName + ", " + type.Assembly.GetName().Name;
					}
					else
					{
						text = type.FullName + ", " + type.Assembly.GetName().Name;
					}
					IL_153:
					DefaultSerializationBinder.nameMap.Add(type, text);
				}
			}
			return text;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00017554 File Offset: 0x00015754
		public override bool ContainsType(string typeName)
		{
			object nametotype_LOCK = DefaultSerializationBinder.NAMETOTYPE_LOCK;
			bool result;
			lock (nametotype_LOCK)
			{
				result = DefaultSerializationBinder.typeMap.ContainsKey(typeName);
			}
			return result;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0001759C File Offset: 0x0001579C
		public override Type BindToType(string typeName, DebugContext debugContext = null)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			DefaultSerializationBinder.RegisterAllQueuedAssembliesRepeating();
			object nametotype_LOCK = DefaultSerializationBinder.NAMETOTYPE_LOCK;
			Type type;
			lock (nametotype_LOCK)
			{
				if (!DefaultSerializationBinder.typeMap.TryGetValue(typeName, ref type))
				{
					type = this.ParseTypeName(typeName, debugContext);
					if (type == null && debugContext != null)
					{
						debugContext.LogWarning("Failed deserialization type lookup for type name '" + typeName + "'.");
					}
					DefaultSerializationBinder.typeMap.Add(typeName, type);
				}
			}
			return type;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00017630 File Offset: 0x00015830
		private Type ParseTypeName(string typeName, DebugContext debugContext)
		{
			object assembly_LOOKUP_LOCK = DefaultSerializationBinder.ASSEMBLY_LOOKUP_LOCK;
			Type type;
			lock (assembly_LOOKUP_LOCK)
			{
				if (DefaultSerializationBinder.customTypeNameToTypeBindings.TryGetValue(typeName, ref type))
				{
					return type;
				}
			}
			type = Type.GetType(typeName);
			if (type != null)
			{
				return type;
			}
			type = this.ParseGenericAndOrArrayType(typeName, debugContext);
			if (type != null)
			{
				return type;
			}
			string text;
			string text2;
			DefaultSerializationBinder.ParseName(typeName, out text, out text2);
			if (!string.IsNullOrEmpty(text))
			{
				object assembly_LOOKUP_LOCK2 = DefaultSerializationBinder.ASSEMBLY_LOOKUP_LOCK;
				lock (assembly_LOOKUP_LOCK2)
				{
					if (DefaultSerializationBinder.customTypeNameToTypeBindings.TryGetValue(text, ref type))
					{
						return type;
					}
				}
				if (text2 != null)
				{
					object assembly_LOOKUP_LOCK3 = DefaultSerializationBinder.ASSEMBLY_LOOKUP_LOCK;
					Assembly assembly;
					lock (assembly_LOOKUP_LOCK3)
					{
						DefaultSerializationBinder.assemblyNameLookUp.TryGetValue(text2, ref assembly);
					}
					if (assembly == null)
					{
						try
						{
							assembly = Assembly.Load(text2);
						}
						catch
						{
						}
					}
					if (assembly != null)
					{
						try
						{
							type = assembly.GetType(text);
						}
						catch
						{
						}
						if (type != null)
						{
							return type;
						}
					}
				}
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					try
					{
						Assembly assembly;
						type = assembly.GetType(text, false);
					}
					catch
					{
					}
					if (type != null)
					{
						return type;
					}
				}
			}
			return null;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x000177E4 File Offset: 0x000159E4
		private static void ParseName(string fullName, out string typeName, out string assemblyName)
		{
			typeName = null;
			assemblyName = null;
			int num = fullName.IndexOf(',');
			if (num < 0 || num + 1 == fullName.Length)
			{
				typeName = fullName.Trim(new char[]
				{
					',',
					' '
				});
				return;
			}
			typeName = fullName.Substring(0, num);
			int num2 = fullName.IndexOf(',', num + 1);
			if (num2 < 0)
			{
				assemblyName = fullName.Substring(num).Trim(new char[]
				{
					',',
					' '
				});
				return;
			}
			assemblyName = fullName.Substring(num, num2 - num).Trim(new char[]
			{
				',',
				' '
			});
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00017884 File Offset: 0x00015A84
		private Type ParseGenericAndOrArrayType(string typeName, DebugContext debugContext)
		{
			string typeName2;
			bool flag;
			List<string> list;
			bool flag2;
			int num;
			if (!DefaultSerializationBinder.TryParseGenericAndOrArrayTypeName(typeName, out typeName2, out flag, out list, out flag2, out num))
			{
				return null;
			}
			Type type = this.BindToType(typeName2, debugContext);
			if (type == null)
			{
				return null;
			}
			if (flag)
			{
				if (!type.IsGenericType)
				{
					return null;
				}
				using (Cache<List<Type>> cache = Cache<List<Type>>.Claim())
				{
					List<Type> value = cache.Value;
					value.Clear();
					for (int i = 0; i < list.Count; i++)
					{
						Type type2 = this.BindToType(list[i], debugContext);
						if (type2 == null)
						{
							return null;
						}
						value.Add(type2);
					}
					Type[] array = value.ToArray();
					if (!type.AreGenericConstraintsSatisfiedBy(array))
					{
						if (debugContext != null)
						{
							string text = "";
							foreach (Type type3 in array)
							{
								if (text != "")
								{
									text += ", ";
								}
								text += type3.GetNiceFullName();
							}
							debugContext.LogWarning(string.Concat(new string[]
							{
								"Deserialization type lookup failure: The generic type arguments '",
								text,
								"' do not satisfy the generic constraints of generic type definition '",
								type.GetNiceFullName(),
								"'. All this parsed from the full type name string: '",
								typeName,
								"'"
							}));
						}
						return null;
					}
					type = type.MakeGenericType(array);
					value.Clear();
				}
			}
			if (flag2)
			{
				if (num == 1)
				{
					type = type.MakeArrayType();
				}
				else
				{
					type = type.MakeArrayType(num);
				}
			}
			return type;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00017A3C File Offset: 0x00015C3C
		private static bool TryParseGenericAndOrArrayTypeName(string typeName, out string actualTypeName, out bool isGeneric, out List<string> genericArgNames, out bool isArray, out int arrayRank)
		{
			isGeneric = false;
			isArray = false;
			arrayRank = 0;
			bool flag = false;
			genericArgNames = null;
			actualTypeName = null;
			for (int i = 0; i < typeName.Length; i++)
			{
				if (typeName.get_Chars(i) == '[')
				{
					char c = DefaultSerializationBinder.Peek(typeName, i, 1);
					if (c == ',' || c == ']')
					{
						if (actualTypeName == null)
						{
							actualTypeName = typeName.Substring(0, i);
						}
						isArray = true;
						arrayRank = 1;
						i++;
						if (c == ',')
						{
							while (c == ',')
							{
								arrayRank++;
								c = DefaultSerializationBinder.Peek(typeName, i, 1);
								i++;
							}
							if (c != ']')
							{
								return false;
							}
						}
					}
					else if (!isGeneric)
					{
						actualTypeName = typeName.Substring(0, i);
						isGeneric = true;
						flag = true;
						genericArgNames = new List<string>();
					}
					else
					{
						string text;
						if (!isGeneric || !DefaultSerializationBinder.ReadGenericArg(typeName, ref i, out text))
						{
							return false;
						}
						genericArgNames.Add(text);
					}
				}
				else if (typeName.get_Chars(i) == ']')
				{
					if (!flag)
					{
						return false;
					}
					flag = false;
				}
				else if (typeName.get_Chars(i) == ',' && !flag)
				{
					actualTypeName += typeName.Substring(i);
					break;
				}
			}
			return isArray | isGeneric;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00017B4B File Offset: 0x00015D4B
		private static char Peek(string str, int i, int ahead)
		{
			if (i + ahead < str.Length)
			{
				return str.get_Chars(i + ahead);
			}
			return '\0';
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00017B64 File Offset: 0x00015D64
		private static bool ReadGenericArg(string typeName, ref int i, out string argName)
		{
			argName = null;
			if (typeName.get_Chars(i) != '[')
			{
				return false;
			}
			int num = i + 1;
			int num2 = 0;
			while (i < typeName.Length)
			{
				if (typeName.get_Chars(i) == '[')
				{
					num2++;
				}
				else if (typeName.get_Chars(i) == ']')
				{
					num2--;
					if (num2 == 0)
					{
						int num3 = i - num;
						argName = typeName.Substring(num, num3);
						return true;
					}
				}
				i++;
			}
			return false;
		}

		// Token: 0x04000109 RID: 265
		private static readonly object ASSEMBLY_LOOKUP_LOCK = new object();

		// Token: 0x0400010A RID: 266
		private static readonly Dictionary<string, Assembly> assemblyNameLookUp = new Dictionary<string, Assembly>();

		// Token: 0x0400010B RID: 267
		private static readonly Dictionary<string, Type> customTypeNameToTypeBindings = new Dictionary<string, Type>();

		// Token: 0x0400010C RID: 268
		private static readonly object TYPETONAME_LOCK = new object();

		// Token: 0x0400010D RID: 269
		private static readonly Dictionary<Type, string> nameMap = new Dictionary<Type, string>(FastTypeComparer.Instance);

		// Token: 0x0400010E RID: 270
		private static readonly object NAMETOTYPE_LOCK = new object();

		// Token: 0x0400010F RID: 271
		private static readonly Dictionary<string, Type> typeMap = new Dictionary<string, Type>();

		// Token: 0x04000110 RID: 272
		private static readonly object ASSEMBLY_REGISTER_QUEUE_LOCK = new object();

		// Token: 0x04000111 RID: 273
		private static readonly List<Assembly> assembliesQueuedForRegister = new List<Assembly>();

		// Token: 0x04000112 RID: 274
		private static readonly List<AssemblyLoadEventArgs> assemblyLoadEventsQueuedForRegister = new List<AssemblyLoadEventArgs>();
	}
}
