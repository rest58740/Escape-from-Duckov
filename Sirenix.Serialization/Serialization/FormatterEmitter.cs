using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000032 RID: 50
	public static class FormatterEmitter
	{
		// Token: 0x06000274 RID: 628 RVA: 0x00011B74 File Offset: 0x0000FD74
		public static IFormatter GetEmittedFormatter(Type type, ISerializationPolicy policy)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (policy == null)
			{
				policy = SerializationPolicies.Strict;
			}
			IFormatter formatter = null;
			if (!FormatterEmitter.Formatters.TryGetInnerValue(policy, type, out formatter))
			{
				object @lock = FormatterEmitter.LOCK;
				lock (@lock)
				{
					if (!FormatterEmitter.Formatters.TryGetInnerValue(policy, type, out formatter))
					{
						FormatterEmitter.EnsureRuntimeAssembly();
						try
						{
							formatter = FormatterEmitter.CreateGenericFormatter(type, FormatterEmitter.runtimeEmittedModule, policy);
						}
						catch (Exception exception)
						{
							Debug.LogError("The following error occurred while emitting a formatter for the type " + type.Name);
							Debug.LogException(exception);
						}
						FormatterEmitter.Formatters.AddInner(policy, type, formatter);
					}
				}
			}
			return formatter;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00011C3C File Offset: 0x0000FE3C
		private static void EnsureRuntimeAssembly()
		{
			if (FormatterEmitter.runtimeEmittedAssembly == null)
			{
				AssemblyName assemblyName = new AssemblyName("Sirenix.Serialization.RuntimeEmitted");
				assemblyName.CultureInfo = CultureInfo.InvariantCulture;
				assemblyName.Flags = 0;
				assemblyName.ProcessorArchitecture = 1;
				assemblyName.VersionCompatibility = 3;
				FormatterEmitter.runtimeEmittedAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, 1);
			}
			if (FormatterEmitter.runtimeEmittedModule == null)
			{
				bool flag = false;
				FormatterEmitter.runtimeEmittedModule = FormatterEmitter.runtimeEmittedAssembly.DefineDynamicModule("Sirenix.Serialization.RuntimeEmitted", flag);
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00011CB8 File Offset: 0x0000FEB8
		public static Type EmitAOTFormatter(Type formattedType, ModuleBuilder moduleBuilder, ISerializationPolicy policy)
		{
			Dictionary<string, MemberInfo> serializableMembersMap = FormatterUtilities.GetSerializableMembersMap(formattedType, policy);
			string text = moduleBuilder.Name + "." + formattedType.GetCompilableNiceFullName() + "__AOTFormatter";
			string helperTypeName = moduleBuilder.Name + "." + formattedType.GetCompilableNiceFullName() + "__FormatterHelper";
			if (serializableMembersMap.Count == 0)
			{
				return moduleBuilder.DefineType(text, 257, typeof(FormatterEmitter.EmptyAOTEmittedFormatter<>).MakeGenericType(new Type[]
				{
					formattedType
				})).CreateType();
			}
			Dictionary<Type, MethodInfo> serializerReadMethods;
			Dictionary<Type, MethodInfo> serializerWriteMethods;
			Dictionary<Type, FieldBuilder> serializerFields;
			FieldBuilder dictField;
			Dictionary<MemberInfo, List<string>> memberNames;
			FormatterEmitter.BuildHelperType(moduleBuilder, helperTypeName, formattedType, serializableMembersMap, out serializerReadMethods, out serializerWriteMethods, out serializerFields, out dictField, out memberNames);
			TypeBuilder typeBuilder = moduleBuilder.DefineType(text, 257, typeof(FormatterEmitter.AOTEmittedFormatter<>).MakeGenericType(new Type[]
			{
				formattedType
			}));
			MethodInfo method = typeBuilder.BaseType.GetMethod("ReadDataEntry", 52);
			MethodBuilder readMethod = typeBuilder.DefineMethod(method.Name, 68, method.ReturnType, (from n in method.GetParameters()
			select n.ParameterType).ToArray<Type>());
			method.GetParameters().ForEach(delegate(ParameterInfo n)
			{
				readMethod.DefineParameter(n.Position, n.Attributes, n.Name);
			});
			FormatterEmitter.EmitReadMethodContents(readMethod.GetILGenerator(), formattedType, dictField, serializerFields, memberNames, serializerReadMethods);
			MethodInfo method2 = typeBuilder.BaseType.GetMethod("WriteDataEntries", 52);
			MethodBuilder dynamicWriteMethod = typeBuilder.DefineMethod(method2.Name, 68, method2.ReturnType, (from n in method2.GetParameters()
			select n.ParameterType).ToArray<Type>());
			method2.GetParameters().ForEach(delegate(ParameterInfo n)
			{
				dynamicWriteMethod.DefineParameter(n.Position + 1, n.Attributes, n.Name);
			});
			FormatterEmitter.EmitWriteMethodContents(dynamicWriteMethod.GetILGenerator(), formattedType, serializerFields, memberNames, serializerWriteMethods);
			Type result = typeBuilder.CreateType();
			((AssemblyBuilder)moduleBuilder.Assembly).SetCustomAttribute(new CustomAttributeBuilder(typeof(RegisterFormatterAttribute).GetConstructor(new Type[]
			{
				typeof(Type),
				typeof(int)
			}), new object[]
			{
				typeBuilder,
				-1
			}));
			return result;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00011F10 File Offset: 0x00010110
		private static IFormatter CreateGenericFormatter(Type formattedType, ModuleBuilder moduleBuilder, ISerializationPolicy policy)
		{
			Dictionary<string, MemberInfo> serializableMembersMap = FormatterUtilities.GetSerializableMembersMap(formattedType, policy);
			if (serializableMembersMap.Count == 0)
			{
				return (IFormatter)Activator.CreateInstance(typeof(EmptyTypeFormatter<>).MakeGenericType(new Type[]
				{
					formattedType
				}));
			}
			string helperTypeName = string.Concat(new string[]
			{
				moduleBuilder.Name,
				".",
				formattedType.GetCompilableNiceFullName(),
				"___",
				formattedType.Assembly.GetName().Name,
				"___FormatterHelper___",
				Interlocked.Increment(ref FormatterEmitter.helperFormatterNameId).ToString()
			});
			Dictionary<Type, MethodInfo> serializerReadMethods;
			Dictionary<Type, MethodInfo> serializerWriteMethods;
			Dictionary<Type, FieldBuilder> serializerFields;
			FieldBuilder dictField;
			Dictionary<MemberInfo, List<string>> memberNames;
			FormatterEmitter.BuildHelperType(moduleBuilder, helperTypeName, formattedType, serializableMembersMap, out serializerReadMethods, out serializerWriteMethods, out serializerFields, out dictField, out memberNames);
			Type type = typeof(FormatterEmitter.RuntimeEmittedFormatter<>).MakeGenericType(new Type[]
			{
				formattedType
			});
			Type type2 = typeof(FormatterEmitter.ReadDataEntryMethodDelegate<>).MakeGenericType(new Type[]
			{
				formattedType
			});
			MethodInfo method = type.GetMethod("ReadDataEntry", 52);
			DynamicMethod dynamicReadMethod = new DynamicMethod("Dynamic_" + formattedType.GetCompilableNiceFullName(), null, (from n in method.GetParameters()
			select n.ParameterType).ToArray<Type>(), true);
			method.GetParameters().ForEach(delegate(ParameterInfo n)
			{
				dynamicReadMethod.DefineParameter(n.Position, n.Attributes, n.Name);
			});
			FormatterEmitter.EmitReadMethodContents(dynamicReadMethod.GetILGenerator(), formattedType, dictField, serializerFields, memberNames, serializerReadMethods);
			Delegate @delegate = dynamicReadMethod.CreateDelegate(type2);
			Type type3 = typeof(FormatterEmitter.WriteDataEntriesMethodDelegate<>).MakeGenericType(new Type[]
			{
				formattedType
			});
			MethodInfo method2 = type.GetMethod("WriteDataEntries", 52);
			DynamicMethod dynamicWriteMethod = new DynamicMethod("Dynamic_Write_" + formattedType.GetCompilableNiceFullName(), null, (from n in method2.GetParameters()
			select n.ParameterType).ToArray<Type>(), true);
			method2.GetParameters().ForEach(delegate(ParameterInfo n)
			{
				dynamicWriteMethod.DefineParameter(n.Position + 1, n.Attributes, n.Name);
			});
			FormatterEmitter.EmitWriteMethodContents(dynamicWriteMethod.GetILGenerator(), formattedType, serializerFields, memberNames, serializerWriteMethods);
			Delegate delegate2 = dynamicWriteMethod.CreateDelegate(type3);
			return (IFormatter)Activator.CreateInstance(type, new object[]
			{
				@delegate,
				delegate2
			});
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00012180 File Offset: 0x00010380
		private static Type BuildHelperType(ModuleBuilder moduleBuilder, string helperTypeName, Type formattedType, Dictionary<string, MemberInfo> serializableMembers, out Dictionary<Type, MethodInfo> serializerReadMethods, out Dictionary<Type, MethodInfo> serializerWriteMethods, out Dictionary<Type, FieldBuilder> serializerFields, out FieldBuilder dictField, out Dictionary<MemberInfo, List<string>> memberNames)
		{
			TypeBuilder typeBuilder = moduleBuilder.DefineType(helperTypeName, 257);
			memberNames = new Dictionary<MemberInfo, List<string>>();
			foreach (KeyValuePair<string, MemberInfo> keyValuePair in serializableMembers)
			{
				List<string> list;
				if (!memberNames.TryGetValue(keyValuePair.Value, ref list))
				{
					list = new List<string>();
					memberNames.Add(keyValuePair.Value, list);
				}
				list.Add(keyValuePair.Key);
			}
			dictField = typeBuilder.DefineField("SwitchLookup", typeof(Dictionary<string, int>), 54);
			List<Type> list2 = (from n in memberNames.Keys
			select FormatterUtilities.GetContainedType(n)).Distinct<Type>().ToList<Type>();
			serializerReadMethods = new Dictionary<Type, MethodInfo>(list2.Count);
			serializerWriteMethods = new Dictionary<Type, MethodInfo>(list2.Count);
			serializerFields = new Dictionary<Type, FieldBuilder>(list2.Count);
			foreach (Type type in list2)
			{
				string name = type.GetCompilableNiceFullName() + "__Serializer";
				int num = 1;
				while (serializerFields.Values.Any((FieldBuilder n) => n.Name == name))
				{
					num++;
					name = type.GetCompilableNiceFullName() + "__Serializer" + num.ToString();
				}
				Type type2 = typeof(Serializer<>).MakeGenericType(new Type[]
				{
					type
				});
				serializerReadMethods.Add(type, type2.GetMethod("ReadValue", 22));
				serializerWriteMethods.Add(type, type2.GetMethod("WriteValue", 22, null, new Type[]
				{
					typeof(string),
					type,
					typeof(IDataWriter)
				}, null));
				serializerFields.Add(type, typeBuilder.DefineField(name, type2, 54));
			}
			MethodInfo method = typeof(Dictionary<string, int>).GetMethod("Add", 20);
			ConstructorInfo constructor = typeof(Dictionary<string, int>).GetConstructor(Type.EmptyTypes);
			MethodInfo method2 = typeof(Serializer).GetMethod("Get", 24, null, new Type[]
			{
				typeof(Type)
			}, null);
			MethodInfo method3 = typeof(Type).GetMethod("GetTypeFromHandle", 24, null, new Type[]
			{
				typeof(RuntimeTypeHandle)
			}, null);
			ConstructorBuilder constructorBuilder = typeBuilder.DefineTypeInitializer();
			ILGenerator ilgenerator = constructorBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Newobj, constructor);
			int num2 = 0;
			foreach (KeyValuePair<MemberInfo, List<string>> keyValuePair2 in memberNames)
			{
				foreach (string text in keyValuePair2.Value)
				{
					ilgenerator.Emit(OpCodes.Dup);
					ilgenerator.Emit(OpCodes.Ldstr, text);
					ilgenerator.Emit(OpCodes.Ldc_I4, num2);
					ilgenerator.Emit(OpCodes.Call, method);
				}
				num2++;
			}
			ilgenerator.Emit(OpCodes.Stsfld, dictField);
			foreach (KeyValuePair<Type, FieldBuilder> keyValuePair3 in serializerFields)
			{
				ilgenerator.Emit(OpCodes.Ldtoken, keyValuePair3.Key);
				ilgenerator.Emit(OpCodes.Call, method3);
				ilgenerator.Emit(OpCodes.Call, method2);
				ilgenerator.Emit(OpCodes.Stsfld, keyValuePair3.Value);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return typeBuilder.CreateType();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x000125F8 File Offset: 0x000107F8
		private static void EmitReadMethodContents(ILGenerator gen, Type formattedType, FieldInfo dictField, Dictionary<Type, FieldBuilder> serializerFields, Dictionary<MemberInfo, List<string>> memberNames, Dictionary<Type, MethodInfo> serializerReadMethods)
		{
			MethodInfo method = typeof(IDataReader).GetMethod("SkipEntry", 20);
			MethodInfo method2 = typeof(Dictionary<string, int>).GetMethod("TryGetValue", 20);
			LocalBuilder localBuilder = gen.DeclareLocal(typeof(int));
			Label label = gen.DefineLabel();
			Label label2 = gen.DefineLabel();
			Label label3 = gen.DefineLabel();
			Label[] array = (from n in memberNames
			select gen.DefineLabel()).ToArray<Label>();
			gen.Emit(OpCodes.Ldarg_1);
			gen.Emit(OpCodes.Ldnull);
			gen.Emit(OpCodes.Ceq);
			gen.Emit(OpCodes.Brtrue, label);
			gen.Emit(OpCodes.Ldsfld, dictField);
			gen.Emit(OpCodes.Ldarg_1);
			gen.Emit(OpCodes.Ldloca, (short)localBuilder.LocalIndex);
			gen.Emit(OpCodes.Callvirt, method2);
			gen.Emit(OpCodes.Brtrue, label2);
			gen.Emit(OpCodes.Br, label);
			gen.MarkLabel(label2);
			gen.Emit(OpCodes.Ldloc, localBuilder);
			gen.Emit(OpCodes.Switch, array);
			int num = 0;
			foreach (MemberInfo memberInfo in memberNames.Keys)
			{
				Type containedType = FormatterUtilities.GetContainedType(memberInfo);
				PropertyInfo propertyInfo = memberInfo as PropertyInfo;
				FieldInfo fieldInfo = memberInfo as FieldInfo;
				gen.MarkLabel(array[num]);
				gen.Emit(OpCodes.Ldarg_0);
				if (!formattedType.IsValueType)
				{
					gen.Emit(OpCodes.Ldind_Ref);
				}
				gen.Emit(OpCodes.Ldsfld, serializerFields[containedType]);
				gen.Emit(OpCodes.Ldarg, 3);
				gen.Emit(OpCodes.Callvirt, serializerReadMethods[containedType]);
				if (fieldInfo != null)
				{
					gen.Emit(OpCodes.Stfld, fieldInfo.DeAliasField(false));
				}
				else
				{
					if (!(propertyInfo != null))
					{
						throw new NotImplementedException();
					}
					gen.Emit(OpCodes.Callvirt, propertyInfo.DeAliasProperty(false).GetSetMethod(true));
				}
				gen.Emit(OpCodes.Br, label3);
				num++;
			}
			gen.MarkLabel(label);
			gen.Emit(OpCodes.Ldarg, 3);
			gen.Emit(OpCodes.Callvirt, method);
			gen.MarkLabel(label3);
			gen.Emit(OpCodes.Ret);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00012920 File Offset: 0x00010B20
		private static void EmitWriteMethodContents(ILGenerator gen, Type formattedType, Dictionary<Type, FieldBuilder> serializerFields, Dictionary<MemberInfo, List<string>> memberNames, Dictionary<Type, MethodInfo> serializerWriteMethods)
		{
			foreach (MemberInfo memberInfo in memberNames.Keys)
			{
				Type containedType = FormatterUtilities.GetContainedType(memberInfo);
				gen.Emit(OpCodes.Ldsfld, serializerFields[containedType]);
				gen.Emit(OpCodes.Ldstr, memberInfo.Name);
				if (memberInfo is FieldInfo)
				{
					FieldInfo fieldInfo = memberInfo as FieldInfo;
					if (formattedType.IsValueType)
					{
						gen.Emit(OpCodes.Ldarg_0);
						gen.Emit(OpCodes.Ldfld, fieldInfo.DeAliasField(false));
					}
					else
					{
						gen.Emit(OpCodes.Ldarg_0);
						gen.Emit(OpCodes.Ldind_Ref);
						gen.Emit(OpCodes.Ldfld, fieldInfo.DeAliasField(false));
					}
				}
				else
				{
					if (!(memberInfo is PropertyInfo))
					{
						throw new NotImplementedException();
					}
					PropertyInfo propertyInfo = memberInfo as PropertyInfo;
					if (formattedType.IsValueType)
					{
						gen.Emit(OpCodes.Ldarg_0);
						gen.Emit(OpCodes.Call, propertyInfo.DeAliasProperty(false).GetGetMethod(true));
					}
					else
					{
						gen.Emit(OpCodes.Ldarg_0);
						gen.Emit(OpCodes.Ldind_Ref);
						gen.Emit(OpCodes.Callvirt, propertyInfo.DeAliasProperty(false).GetGetMethod(true));
					}
				}
				gen.Emit(OpCodes.Ldarg_1);
				gen.Emit(OpCodes.Callvirt, serializerWriteMethods[containedType]);
			}
			gen.Emit(OpCodes.Ret);
		}

		// Token: 0x040000BD RID: 189
		private static int helperFormatterNameId;

		// Token: 0x040000BE RID: 190
		public const string PRE_EMITTED_ASSEMBLY_NAME = "Sirenix.Serialization.AOTGenerated";

		// Token: 0x040000BF RID: 191
		public const string RUNTIME_EMITTED_ASSEMBLY_NAME = "Sirenix.Serialization.RuntimeEmitted";

		// Token: 0x040000C0 RID: 192
		private static readonly object LOCK = new object();

		// Token: 0x040000C1 RID: 193
		private static readonly DoubleLookupDictionary<ISerializationPolicy, Type, IFormatter> Formatters = new DoubleLookupDictionary<ISerializationPolicy, Type, IFormatter>();

		// Token: 0x040000C2 RID: 194
		private static AssemblyBuilder runtimeEmittedAssembly;

		// Token: 0x040000C3 RID: 195
		private static ModuleBuilder runtimeEmittedModule;

		// Token: 0x020000EA RID: 234
		[EmittedFormatter]
		public abstract class AOTEmittedFormatter<T> : EasyBaseFormatter<T>
		{
		}

		// Token: 0x020000EB RID: 235
		public abstract class EmptyAOTEmittedFormatter<T> : FormatterEmitter.AOTEmittedFormatter<T>
		{
			// Token: 0x06000690 RID: 1680 RVA: 0x00011B63 File Offset: 0x0000FD63
			protected override void ReadDataEntry(ref T value, string entryName, EntryType entryType, IDataReader reader)
			{
				reader.SkipEntry();
			}

			// Token: 0x06000691 RID: 1681 RVA: 0x000021B8 File Offset: 0x000003B8
			protected override void WriteDataEntries(ref T value, IDataWriter writer)
			{
			}
		}

		// Token: 0x020000EC RID: 236
		// (Invoke) Token: 0x06000694 RID: 1684
		public delegate void ReadDataEntryMethodDelegate<T>(ref T value, string entryName, EntryType entryType, IDataReader reader);

		// Token: 0x020000ED RID: 237
		// (Invoke) Token: 0x06000698 RID: 1688
		public delegate void WriteDataEntriesMethodDelegate<T>(ref T value, IDataWriter writer);

		// Token: 0x020000EE RID: 238
		[EmittedFormatter]
		public sealed class RuntimeEmittedFormatter<T> : EasyBaseFormatter<T>
		{
			// Token: 0x0600069B RID: 1691 RVA: 0x0002B3BA File Offset: 0x000295BA
			public RuntimeEmittedFormatter(FormatterEmitter.ReadDataEntryMethodDelegate<T> read, FormatterEmitter.WriteDataEntriesMethodDelegate<T> write)
			{
				this.Read = read;
				this.Write = write;
			}

			// Token: 0x0600069C RID: 1692 RVA: 0x0002B3D0 File Offset: 0x000295D0
			protected override void ReadDataEntry(ref T value, string entryName, EntryType entryType, IDataReader reader)
			{
				this.Read(ref value, entryName, entryType, reader);
			}

			// Token: 0x0600069D RID: 1693 RVA: 0x0002B3E2 File Offset: 0x000295E2
			protected override void WriteDataEntries(ref T value, IDataWriter writer)
			{
				this.Write(ref value, writer);
			}

			// Token: 0x04000269 RID: 617
			public readonly FormatterEmitter.ReadDataEntryMethodDelegate<T> Read;

			// Token: 0x0400026A RID: 618
			public readonly FormatterEmitter.WriteDataEntriesMethodDelegate<T> Write;
		}
	}
}
