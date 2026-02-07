using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using ParadoxNotion.Serialization;
using ParadoxNotion.Services;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x02000075 RID: 117
	public static class ReflectionTools
	{
		// Token: 0x06000424 RID: 1060 RVA: 0x0000A740 File Offset: 0x00008940
		static ReflectionTools()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			dictionary.Add("op_Equality", "Equal");
			dictionary.Add("op_Inequality", "Not Equal");
			dictionary.Add("op_GreaterThan", "Greater");
			dictionary.Add("op_LessThan", "Less");
			dictionary.Add("op_GreaterThanOrEqual", "Greater Or Equal");
			dictionary.Add("op_LessThanOrEqual", "Less Or Equal");
			dictionary.Add("op_Addition", "Add");
			dictionary.Add("op_Subtraction", "Subtract");
			dictionary.Add("op_Division", "Divide");
			dictionary.Add("op_Multiply", "Multiply");
			dictionary.Add("op_UnaryNegation", "Negate");
			dictionary.Add("op_UnaryPlus", "Positive");
			dictionary.Add("op_Increment", "Increment");
			dictionary.Add("op_Decrement", "Decrement");
			dictionary.Add("op_LogicalNot", "NOT");
			dictionary.Add("op_OnesComplement", "Complements");
			dictionary.Add("op_False", "FALSE");
			dictionary.Add("op_True", "TRUE");
			dictionary.Add("op_Modulus", "MOD");
			dictionary.Add("op_BitwiseAnd", "AND");
			dictionary.Add("op_BitwiseOR", "OR");
			dictionary.Add("op_LeftShift", "Shift Left");
			dictionary.Add("op_RightShift", "Shift Right");
			dictionary.Add("op_ExclusiveOr", "XOR");
			dictionary.Add("op_Implicit", "Convert");
			dictionary.Add("op_Explicit", "Convert");
			ReflectionTools.op_FriendlyNamesLong = dictionary;
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			dictionary2.Add("op_Equality", "=");
			dictionary2.Add("op_Inequality", "≠");
			dictionary2.Add("op_GreaterThan", ">");
			dictionary2.Add("op_LessThan", "<");
			dictionary2.Add("op_GreaterThanOrEqual", "≥");
			dictionary2.Add("op_LessThanOrEqual", "≤");
			dictionary2.Add("op_Addition", "+");
			dictionary2.Add("op_Subtraction", "-");
			dictionary2.Add("op_Division", "÷");
			dictionary2.Add("op_Multiply", "×");
			dictionary2.Add("op_UnaryNegation", "Negate");
			dictionary2.Add("op_UnaryPlus", "Positive");
			dictionary2.Add("op_Increment", "++");
			dictionary2.Add("op_Decrement", "--");
			dictionary2.Add("op_LogicalNot", "NOT");
			dictionary2.Add("op_OnesComplement", "~");
			dictionary2.Add("op_False", "FALSE");
			dictionary2.Add("op_True", "TRUE");
			dictionary2.Add("op_Modulus", "MOD");
			dictionary2.Add("op_BitwiseAnd", "AND");
			dictionary2.Add("op_BitwiseOR", "OR");
			dictionary2.Add("op_LeftShift", "<<");
			dictionary2.Add("op_RightShift", ">>");
			dictionary2.Add("op_ExclusiveOr", "XOR");
			dictionary2.Add("op_Implicit", "Convert");
			dictionary2.Add("op_Explicit", "Convert");
			ReflectionTools.op_FriendlyNamesShort = dictionary2;
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			dictionary3.Add("!=", "≠");
			dictionary3.Add(">=", "≥");
			dictionary3.Add("<=", "≤");
			dictionary3.Add("/", "÷");
			dictionary3.Add("*", "×");
			ReflectionTools.op_CSharpAliases = dictionary3;
			ReflectionTools.FlushMem();
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000AB10 File Offset: 0x00008D10
		public static void FlushMem()
		{
			ReflectionTools._loadedAssemblies = null;
			ReflectionTools._allTypes = null;
			ReflectionTools._tempArgs = new object[1];
			ReflectionTools._typesMap = new Dictionary<string, Type>();
			ReflectionTools._subTypesMap = new Dictionary<Type, Type[]>();
			ReflectionTools._methodSpecialType = new Dictionary<MethodBase, ReflectionTools.MethodType>();
			ReflectionTools._typeFriendlyName = new Dictionary<Type, string>();
			ReflectionTools._typeFriendlyNameCompileSafe = new Dictionary<Type, string>();
			ReflectionTools._methodSignatures = new Dictionary<MethodBase, string>();
			ReflectionTools._typeConstructors = new Dictionary<Type, ConstructorInfo[]>();
			ReflectionTools._typeMethods = new Dictionary<Type, MethodInfo[]>();
			ReflectionTools._typeFields = new Dictionary<Type, FieldInfo[]>();
			ReflectionTools._typeProperties = new Dictionary<Type, PropertyInfo[]>();
			ReflectionTools._typeEvents = new Dictionary<Type, EventInfo[]>();
			ReflectionTools._memberAttributes = new Dictionary<MemberInfo, object[]>();
			ReflectionTools._obsoleteCache = new Dictionary<MemberInfo, bool>();
			ReflectionTools._typeExtensions = new Dictionary<Type, MethodInfo[]>();
			ReflectionTools._genericArgsTypeCache = new Dictionary<Type, Type[]>();
			ReflectionTools._genericArgsMathodCache = new Dictionary<MethodInfo, Type[]>();
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0000ABD4 File Offset: 0x00008DD4
		private static Assembly[] loadedAssemblies
		{
			get
			{
				if (ReflectionTools._loadedAssemblies == null)
				{
					return ReflectionTools._loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
				}
				return ReflectionTools._loadedAssemblies;
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000ABF3 File Offset: 0x00008DF3
		public static Type GetType(string typeFullName)
		{
			return ReflectionTools.GetType(typeFullName, false, null);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000ABFD File Offset: 0x00008DFD
		public static Type GetType(string typeFullName, Type fallbackAssignable)
		{
			return ReflectionTools.GetType(typeFullName, true, fallbackAssignable);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000AC08 File Offset: 0x00008E08
		public static Type GetType(string typeFullName, bool fallbackNoNamespace = false, Type fallbackAssignable = null)
		{
			if (string.IsNullOrEmpty(typeFullName))
			{
				return null;
			}
			Type type = null;
			if (ReflectionTools._typesMap.TryGetValue(typeFullName, ref type))
			{
				return type;
			}
			type = ReflectionTools.GetTypeDirect(typeFullName);
			if (type != null)
			{
				return ReflectionTools._typesMap[typeFullName] = type;
			}
			type = ReflectionTools.TryResolveGenericType(typeFullName, fallbackNoNamespace, fallbackAssignable);
			if (type != null)
			{
				return ReflectionTools._typesMap[typeFullName] = type;
			}
			type = ReflectionTools.TryResolveDeserializeFromAttribute(typeFullName);
			if (type != null)
			{
				return ReflectionTools._typesMap[typeFullName] = type;
			}
			if (fallbackNoNamespace)
			{
				type = ReflectionTools.TryResolveWithoutNamespace(typeFullName, fallbackAssignable);
				if (type != null)
				{
					return ReflectionTools._typesMap[typeFullName] = type;
				}
			}
			return ReflectionTools._typesMap[typeFullName] = null;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000ACC8 File Offset: 0x00008EC8
		private static Type GetTypeDirect(string typeFullName)
		{
			Type type = Type.GetType(typeFullName);
			if (type != null)
			{
				return type;
			}
			int i = 0;
			while (i < ReflectionTools.loadedAssemblies.Length)
			{
				Assembly assembly = ReflectionTools.loadedAssemblies[i];
				try
				{
					type = assembly.GetType(typeFullName);
				}
				catch
				{
					goto IL_36;
				}
				goto IL_2B;
				IL_36:
				i++;
				continue;
				IL_2B:
				if (type != null)
				{
					return type;
				}
				goto IL_36;
			}
			return null;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000AD2C File Offset: 0x00008F2C
		private static Type TryResolveGenericType(string typeFullName, bool fallbackNoNamespace = false, Type fallbackAssignable = null)
		{
			if (!typeFullName.Contains('`') || !typeFullName.Contains('['))
			{
				return null;
			}
			Type result;
			try
			{
				int num = typeFullName.IndexOf('`');
				Type type = ReflectionTools.GetType(typeFullName.Substring(0, num + 2), fallbackNoNamespace, fallbackAssignable);
				if (type == null)
				{
					result = null;
				}
				else
				{
					int num2 = Convert.ToInt32(typeFullName.Substring(num + 1, 1));
					string[] array;
					if (typeFullName.Substring(num + 2, typeFullName.Length - num - 2).StartsWith("[["))
					{
						int num3 = typeFullName.IndexOf("[[") + 2;
						int num4 = typeFullName.LastIndexOf("]]");
						array = typeFullName.Substring(num3, num4 - num3).Split(new string[]
						{
							"],["
						}, num2, 1);
					}
					else
					{
						int num5 = typeFullName.IndexOf('[') + 1;
						int num6 = typeFullName.LastIndexOf(']');
						array = typeFullName.Substring(num5, num6 - num5).Split(new char[]
						{
							','
						}, num2, 1);
					}
					Type[] array2 = new Type[num2];
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						if (!text.Contains('`') && text.Contains(','))
						{
							text = text.Substring(0, text.IndexOf(','));
						}
						Type type2 = ReflectionTools.GetType(text, true, null);
						if (type2 == null)
						{
							return null;
						}
						array2[i] = type2;
					}
					result = type.RTMakeGenericType(array2);
				}
			}
			catch (Exception exception)
			{
				ParadoxNotion.Services.Logger.LogException(exception, "Type Request Bug. Please report. :-(", null);
				result = null;
			}
			return result;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000AEC8 File Offset: 0x000090C8
		private static Type TryResolveDeserializeFromAttribute(string typeName)
		{
			foreach (Type type in ReflectionTools.GetAllTypes(true))
			{
				DeserializeFromAttribute deserializeFromAttribute = CustomAttributeExtensions.GetCustomAttribute(type, typeof(DeserializeFromAttribute), false) as DeserializeFromAttribute;
				if (deserializeFromAttribute != null && deserializeFromAttribute.previousTypeFullName == typeName)
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000AF1C File Offset: 0x0000911C
		private static Type TryResolveWithoutNamespace(string typeName, Type fallbackAssignable = null)
		{
			if (typeName.Contains('`') && typeName.Contains('['))
			{
				return null;
			}
			if (typeName.Contains(','))
			{
				typeName = typeName.Substring(0, typeName.IndexOf(','));
			}
			if (typeName.Contains('.'))
			{
				int num = typeName.LastIndexOf('.') + 1;
				typeName = typeName.Substring(num, typeName.Length - num);
			}
			foreach (Type type in ReflectionTools.GetAllTypes(true))
			{
				if (type.Name == typeName && (fallbackAssignable == null || fallbackAssignable.RTIsAssignableFrom(type)))
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000AFBC File Offset: 0x000091BC
		public static Type[] GetAllTypes(bool includeObsolete)
		{
			if (ReflectionTools._allTypes != null)
			{
				return ReflectionTools._allTypes;
			}
			List<Type> list = new List<Type>();
			Func<Type, bool> <>9__2;
			for (int i = 0; i < ReflectionTools.loadedAssemblies.Length; i++)
			{
				Assembly assembly = ReflectionTools.loadedAssemblies[i];
				try
				{
					List<Type> list2 = list;
					IEnumerable<Type> exportedTypes = assembly.GetExportedTypes();
					Func<Type, bool> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((Type t) => includeObsolete || !t.RTIsDefined(false)));
					}
					list2.AddRange(exportedTypes.Where(predicate));
				}
				catch
				{
				}
			}
			return ReflectionTools._allTypes = (from t in list
			orderby t.Namespace, t.FriendlyName(false)
			select t).ToArray<Type>();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000B0A0 File Offset: 0x000092A0
		public static Type[] GetImplementationsOf(Type baseType)
		{
			Type[] result = null;
			if (ReflectionTools._subTypesMap.TryGetValue(baseType, ref result))
			{
				return result;
			}
			List<Type> list = new List<Type>();
			foreach (Type type in ReflectionTools.GetAllTypes(false))
			{
				if (baseType.RTIsAssignableFrom(type) && !type.RTIsAbstract())
				{
					list.Add(type);
				}
			}
			return ReflectionTools._subTypesMap[baseType] = list.ToArray();
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000B111 File Offset: 0x00009311
		public static object[] SingleTempArgsArray(object arg)
		{
			ReflectionTools._tempArgs[0] = arg;
			return ReflectionTools._tempArgs;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000B120 File Offset: 0x00009320
		public static ReflectionTools.MethodType GetMethodSpecialType(this MethodBase method)
		{
			ReflectionTools.MethodType result;
			if (ReflectionTools._methodSpecialType.TryGetValue(method, ref result))
			{
				return result;
			}
			string name = method.Name;
			if (method.IsSpecialName)
			{
				if (name.StartsWith("get_") || name.StartsWith("set_"))
				{
					return ReflectionTools._methodSpecialType[method] = ReflectionTools.MethodType.PropertyAccessor;
				}
				if (name.StartsWith("add_") || name.StartsWith("remove_"))
				{
					return ReflectionTools._methodSpecialType[method] = ReflectionTools.MethodType.Event;
				}
				if (name.StartsWith("op_"))
				{
					return ReflectionTools._methodSpecialType[method] = ReflectionTools.MethodType.Operator;
				}
			}
			return ReflectionTools._methodSpecialType[method] = ReflectionTools.MethodType.Normal;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000B1D0 File Offset: 0x000093D0
		public static string FriendlyName(this Type t, bool compileSafe = false)
		{
			if (t == null)
			{
				return null;
			}
			if (!compileSafe && t.IsByRef)
			{
				t = t.GetElementType();
			}
			if (!compileSafe && t == typeof(Object))
			{
				return "UnityObject";
			}
			string text;
			if (!compileSafe && ReflectionTools._typeFriendlyName.TryGetValue(t, ref text))
			{
				return text;
			}
			if (compileSafe && ReflectionTools._typeFriendlyNameCompileSafe.TryGetValue(t, ref text))
			{
				return text;
			}
			text = (compileSafe ? t.FullName : t.Name);
			if (!compileSafe)
			{
				if (text == "Single")
				{
					text = "Float";
				}
				if (text == "Single[]")
				{
					text = "Float[]";
				}
				if (text == "Int32")
				{
					text = "Integer";
				}
				if (text == "Int32[]")
				{
					text = "Integer[]";
				}
			}
			if (t.RTIsGenericParameter())
			{
				text = "T";
			}
			if (t.RTIsGenericType())
			{
				text = ((compileSafe && !string.IsNullOrEmpty(t.Namespace)) ? (t.Namespace + "." + t.Name) : t.Name);
				Type[] array = t.RTGetGenericArguments();
				if (array.Length != 0)
				{
					text = text.Replace("`" + array.Length.ToString(), "");
					text += (compileSafe ? "<" : " (");
					for (int i = 0; i < array.Length; i++)
					{
						text = text + ((i == 0) ? "" : ", ") + array[i].FriendlyName(compileSafe);
					}
					text += (compileSafe ? ">" : ")");
				}
			}
			if (compileSafe)
			{
				return ReflectionTools._typeFriendlyNameCompileSafe[t] = text;
			}
			return ReflectionTools._typeFriendlyName[t] = text;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000B392 File Offset: 0x00009592
		public static string FriendlyName(this MemberInfo info)
		{
			if (info == null)
			{
				return null;
			}
			if (info is Type)
			{
				return ((Type)info).FriendlyName(false);
			}
			return info.ReflectedType.FriendlyName(false) + "." + info.Name;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000B3D0 File Offset: 0x000095D0
		public static string FriendlyName(this MethodBase method)
		{
			ReflectionTools.MethodType methodType = ReflectionTools.MethodType.Normal;
			return method.FriendlyName(out methodType);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000B3E8 File Offset: 0x000095E8
		public static string FriendlyName(this MethodBase method, out ReflectionTools.MethodType specialNameType)
		{
			specialNameType = ReflectionTools.MethodType.Normal;
			string text = method.Name;
			if (method.IsSpecialName)
			{
				if (text.StartsWith("get_"))
				{
					text = "Get " + text.Substring("get_".Length).CapitalizeFirst();
					specialNameType = ReflectionTools.MethodType.PropertyAccessor;
					return text;
				}
				if (text.StartsWith("set_"))
				{
					text = "Set " + text.Substring("set_".Length).CapitalizeFirst();
					specialNameType = ReflectionTools.MethodType.PropertyAccessor;
					return text;
				}
				if (text.StartsWith("add_"))
				{
					text = text.Substring("add_".Length) + " +=";
					specialNameType = ReflectionTools.MethodType.Event;
					return text;
				}
				if (text.StartsWith("remove_"))
				{
					text = text.Substring("remove_".Length) + " -=";
					specialNameType = ReflectionTools.MethodType.Event;
					return text;
				}
				if (text.StartsWith("op_"))
				{
					ReflectionTools.op_FriendlyNamesLong.TryGetValue(text, ref text);
					specialNameType = ReflectionTools.MethodType.Operator;
					return text;
				}
			}
			return text;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000B4EC File Offset: 0x000096EC
		public static string SignatureName(this MethodBase method)
		{
			string text = null;
			if (ReflectionTools._methodSignatures.TryGetValue(method, ref text))
			{
				return text;
			}
			ReflectionTools.MethodType methodType = ReflectionTools.MethodType.Normal;
			string text2 = method.FriendlyName(out methodType);
			ParameterInfo[] parameters = method.GetParameters();
			if (method is ConstructorInfo)
			{
				text = string.Format("new {0} (", method.DeclaringType.FriendlyName(false));
			}
			else
			{
				text = string.Format("{0}{1} (", (method.IsStatic && methodType != ReflectionTools.MethodType.Operator) ? "static " : "", text2);
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				if (parameterInfo.IsParams(parameters))
				{
					text += "params ";
				}
				text = text + (parameterInfo.ParameterType.IsByRef ? (parameterInfo.IsOut ? "out " : "ref ") : "") + parameterInfo.ParameterType.FriendlyName(false) + ((i < parameters.Length - 1) ? ", " : "");
			}
			if (method is MethodInfo)
			{
				text = text + ") : " + (method as MethodInfo).ReturnType.FriendlyName(false);
			}
			else
			{
				text += ")";
			}
			return ReflectionTools._methodSignatures[method] = text;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000B628 File Offset: 0x00009828
		public static string FriendlyTypeName(string fullName)
		{
			if (fullName.Contains("`1"))
			{
				string stringWithinInner = fullName.GetStringWithinInner('[', ',');
				string stringWithinInner2 = fullName.GetStringWithinInner('.', '`');
				return string.Format("{0}({1})", stringWithinInner2, stringWithinInner);
			}
			if (fullName.Contains('.'))
			{
				int num = fullName.LastIndexOf('.') + 1;
				return fullName.Substring(num, fullName.Length - num);
			}
			return fullName;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000B68B File Offset: 0x0000988B
		public static Type RTReflectedOrDeclaredType(this MemberInfo member)
		{
			if (!(member.ReflectedType != null))
			{
				return member.DeclaringType;
			}
			return member.ReflectedType;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000B6A8 File Offset: 0x000098A8
		public static bool RTIsAssignableFrom(this Type type, Type other)
		{
			return type.IsAssignableFrom(other);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000B6B1 File Offset: 0x000098B1
		public static bool RTIsAssignableTo(this Type type, Type other)
		{
			return other.RTIsAssignableFrom(type);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000B6BA File Offset: 0x000098BA
		public static bool RTIsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000B6C2 File Offset: 0x000098C2
		public static bool RTIsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000B6CA File Offset: 0x000098CA
		public static bool RTIsArray(this Type type)
		{
			return type.IsArray;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000B6D2 File Offset: 0x000098D2
		public static bool RTIsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000B6DA File Offset: 0x000098DA
		public static bool RTIsSubclassOf(this Type type, Type other)
		{
			return type.IsSubclassOf(other);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000B6E3 File Offset: 0x000098E3
		public static bool RTIsGenericParameter(this Type type)
		{
			return type.IsGenericParameter;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000B6EB File Offset: 0x000098EB
		public static bool RTIsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000B6F3 File Offset: 0x000098F3
		public static MethodInfo RTGetGetMethod(this PropertyInfo prop)
		{
			return prop.GetGetMethod();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000B6FB File Offset: 0x000098FB
		public static MethodInfo RTGetSetMethod(this PropertyInfo prop)
		{
			return prop.GetSetMethod();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000B703 File Offset: 0x00009903
		public static MethodInfo RTGetDelegateMethodInfo(this Delegate del)
		{
			return del.Method;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000B70B File Offset: 0x0000990B
		public static Type RTMakeGenericType(this Type type, params Type[] typeArgs)
		{
			return type.MakeGenericType(typeArgs);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000B714 File Offset: 0x00009914
		public static Type[] RTGetEmptyTypes()
		{
			return Type.EmptyTypes;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000B71B File Offset: 0x0000991B
		public static Type RTGetElementType(this Type type)
		{
			if (type == null)
			{
				return null;
			}
			return type.GetElementType();
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000B72E File Offset: 0x0000992E
		public static bool RTIsByRef(this Type type)
		{
			return !(type == null) && type.IsByRef;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000B744 File Offset: 0x00009944
		public static Type[] RTGetGenericArguments(this Type type)
		{
			Type[] result = null;
			if (ReflectionTools._genericArgsTypeCache.TryGetValue(type, ref result))
			{
				return result;
			}
			return ReflectionTools._genericArgsTypeCache[type] = (result = type.GetGenericArguments());
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000B77C File Offset: 0x0000997C
		public static Type[] RTGetGenericArguments(this MethodInfo method)
		{
			Type[] result = null;
			if (ReflectionTools._genericArgsMathodCache.TryGetValue(method, ref result))
			{
				return result;
			}
			return ReflectionTools._genericArgsMathodCache[method] = (result = method.GetGenericArguments());
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000B7B2 File Offset: 0x000099B2
		public static object CreateObject(this Type type)
		{
			if (type == null)
			{
				return null;
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000B7C5 File Offset: 0x000099C5
		public static object CreateObjectUninitialized(this Type type)
		{
			if (type == null)
			{
				return null;
			}
			return FormatterServices.GetUninitializedObject(type);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000B7D8 File Offset: 0x000099D8
		public static ConstructorInfo RTGetDefaultConstructor(this Type type)
		{
			ConstructorInfo[] array = type.RTGetConstructors();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetParameters().Length == 0)
				{
					return array[i];
				}
			}
			return null;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000B80C File Offset: 0x00009A0C
		public static ConstructorInfo RTGetConstructor(this Type type, Type[] paramTypes)
		{
			foreach (ConstructorInfo constructorInfo in type.RTGetConstructors())
			{
				ParameterInfo[] parameters = constructorInfo.GetParameters();
				if (parameters.Length == paramTypes.Length)
				{
					bool flag = true;
					for (int j = 0; j < parameters.Length; j++)
					{
						if (parameters[j].ParameterType != paramTypes[j])
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return constructorInfo;
					}
				}
			}
			return null;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000B878 File Offset: 0x00009A78
		private static bool MemberResolvedFromDeserializeAttribute(MemberInfo member, string targetName)
		{
			DeserializeFromAttribute deserializeFromAttribute = member.RTGetAttribute(true);
			return deserializeFromAttribute != null && deserializeFromAttribute.previousTypeFullName == targetName;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000B8A0 File Offset: 0x00009AA0
		public static MethodInfo RTGetMethod(this Type type, string name)
		{
			foreach (MethodInfo methodInfo in type.RTGetMethods())
			{
				if (methodInfo.Name == name || ReflectionTools.MemberResolvedFromDeserializeAttribute(methodInfo, name))
				{
					return methodInfo;
				}
			}
			return null;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000B8E0 File Offset: 0x00009AE0
		public static MethodInfo RTGetMethod(this Type type, string name, Type[] paramTypes, Type returnType = null, Type[] genericArgumentTypes = null)
		{
			foreach (MethodInfo methodInfo in type.RTGetMethods())
			{
				if ((methodInfo.Name == name || ReflectionTools.MemberResolvedFromDeserializeAttribute(methodInfo, name)) && (genericArgumentTypes == null || methodInfo.IsGenericMethod))
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (parameters.Length == paramTypes.Length)
					{
						if (genericArgumentTypes != null)
						{
							methodInfo = methodInfo.MakeGenericMethod(genericArgumentTypes);
							parameters = methodInfo.GetParameters();
						}
						if (!(returnType != null) || !(methodInfo.ReturnType != returnType))
						{
							bool flag = true;
							for (int j = 0; j < parameters.Length; j++)
							{
								if (parameters[j].ParameterType != paramTypes[j])
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								return methodInfo;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000B9A0 File Offset: 0x00009BA0
		public static FieldInfo RTGetField(this Type type, string name, bool includePrivateBase = false)
		{
			Type type2 = type;
			while (type2 != null)
			{
				foreach (FieldInfo fieldInfo in type2.RTGetFields())
				{
					if (fieldInfo.Name == name || ReflectionTools.MemberResolvedFromDeserializeAttribute(fieldInfo, name))
					{
						return fieldInfo;
					}
				}
				if (!includePrivateBase)
				{
					break;
				}
				type2 = type2.BaseType;
			}
			return null;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000B9F8 File Offset: 0x00009BF8
		public static PropertyInfo RTGetProperty(this Type type, string name)
		{
			foreach (PropertyInfo propertyInfo in type.RTGetProperties())
			{
				if (propertyInfo.Name == name || ReflectionTools.MemberResolvedFromDeserializeAttribute(propertyInfo, name))
				{
					return propertyInfo;
				}
			}
			return null;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000BA38 File Offset: 0x00009C38
		public static MemberInfo RTGetFieldOrProp(this Type type, string name)
		{
			foreach (FieldInfo fieldInfo in type.RTGetFields())
			{
				if (fieldInfo.Name == name || ReflectionTools.MemberResolvedFromDeserializeAttribute(fieldInfo, name))
				{
					return fieldInfo;
				}
			}
			foreach (PropertyInfo propertyInfo in type.RTGetProperties())
			{
				if (propertyInfo.Name == name || ReflectionTools.MemberResolvedFromDeserializeAttribute(propertyInfo, name))
				{
					return propertyInfo;
				}
			}
			return null;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000BAB4 File Offset: 0x00009CB4
		public static EventInfo RTGetEvent(this Type type, string name)
		{
			foreach (EventInfo eventInfo in type.RTGetEvents())
			{
				if (eventInfo.Name == name || ReflectionTools.MemberResolvedFromDeserializeAttribute(eventInfo, name))
				{
					return eventInfo;
				}
			}
			return null;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000BAF4 File Offset: 0x00009CF4
		public static object RTGetFieldOrPropValue(this MemberInfo member, object instance, int index = -1)
		{
			if (member is FieldInfo)
			{
				return (member as FieldInfo).GetValue(instance);
			}
			if (member is PropertyInfo)
			{
				return (member as PropertyInfo).GetValue(instance, (index == -1) ? null : ReflectionTools.SingleTempArgsArray(index));
			}
			return null;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000BB33 File Offset: 0x00009D33
		public static void RTSetFieldOrPropValue(this MemberInfo member, object instance, object value, int index = -1)
		{
			if (member is FieldInfo)
			{
				(member as FieldInfo).SetValue(instance, value);
			}
			if (member is PropertyInfo)
			{
				(member as PropertyInfo).SetValue(instance, value, (index == -1) ? null : ReflectionTools.SingleTempArgsArray(index));
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000BB74 File Offset: 0x00009D74
		public static ConstructorInfo[] RTGetConstructors(this Type type)
		{
			ConstructorInfo[] constructors;
			if (!ReflectionTools._typeConstructors.TryGetValue(type, ref constructors))
			{
				constructors = type.GetConstructors(124);
				ReflectionTools._typeConstructors[type] = constructors;
			}
			return constructors;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000BBA8 File Offset: 0x00009DA8
		public static MethodInfo[] RTGetMethods(this Type type)
		{
			MethodInfo[] methods;
			if (!ReflectionTools._typeMethods.TryGetValue(type, ref methods))
			{
				methods = type.GetMethods(124);
				ReflectionTools._typeMethods[type] = methods;
			}
			return methods;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000BBDC File Offset: 0x00009DDC
		public static FieldInfo[] RTGetFields(this Type type)
		{
			FieldInfo[] fields;
			if (!ReflectionTools._typeFields.TryGetValue(type, ref fields))
			{
				fields = type.GetFields(124);
				ReflectionTools._typeFields[type] = fields;
			}
			return fields;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000BC10 File Offset: 0x00009E10
		public static PropertyInfo[] RTGetProperties(this Type type)
		{
			PropertyInfo[] properties;
			if (!ReflectionTools._typeProperties.TryGetValue(type, ref properties))
			{
				properties = type.GetProperties(124);
				ReflectionTools._typeProperties[type] = properties;
			}
			return properties;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000BC44 File Offset: 0x00009E44
		public static EventInfo[] RTGetEvents(this Type type)
		{
			EventInfo[] events;
			if (!ReflectionTools._typeEvents.TryGetValue(type, ref events))
			{
				events = type.GetEvents(124);
				ReflectionTools._typeEvents[type] = events;
			}
			return events;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000BC76 File Offset: 0x00009E76
		public static bool RTIsDefined<T>(this Type type, bool inherited) where T : Attribute
		{
			return type.RTIsDefined(typeof(T), inherited);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000BC89 File Offset: 0x00009E89
		public static bool RTIsDefined(this Type type, Type attributeType, bool inherited)
		{
			return type.IsDefined(attributeType, inherited);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000BC93 File Offset: 0x00009E93
		public static T RTGetAttribute<T>(this Type type, bool inherited) where T : Attribute
		{
			return (T)((object)type.RTGetAttribute(typeof(T), inherited));
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000BCAB File Offset: 0x00009EAB
		public static Attribute RTGetAttribute(this Type type, Type attributeType, bool inherited)
		{
			return CustomAttributeExtensions.GetCustomAttribute(type, attributeType, inherited);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000BCB8 File Offset: 0x00009EB8
		public static object[] RTGetAllAttributes(this MemberInfo member)
		{
			object[] customAttributes;
			if (!ReflectionTools._memberAttributes.TryGetValue(member, ref customAttributes))
			{
				customAttributes = member.GetCustomAttributes(true);
				ReflectionTools._memberAttributes[member] = customAttributes;
			}
			return customAttributes;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000BCE9 File Offset: 0x00009EE9
		public static bool RTIsDefined<T>(this MemberInfo member, bool inherited) where T : Attribute
		{
			return member.RTIsDefined(typeof(T), inherited);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000BCFC File Offset: 0x00009EFC
		public static bool RTIsDefined(this MemberInfo member, Type attributeType, bool inherited)
		{
			return member.IsDefined(attributeType, inherited);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000BD06 File Offset: 0x00009F06
		public static T RTGetAttribute<T>(this MemberInfo member, bool inherited) where T : Attribute
		{
			return (T)((object)member.RTGetAttribute(typeof(T), inherited));
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000BD1E File Offset: 0x00009F1E
		public static Attribute RTGetAttribute(this MemberInfo member, Type attributeType, bool inherited)
		{
			return CustomAttributeExtensions.GetCustomAttribute(member, attributeType, inherited);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000BD28 File Offset: 0x00009F28
		public static IEnumerable<T> RTGetAttributesRecursive<T>(this Type type) where T : Attribute
		{
			Type current = type;
			while (current != null)
			{
				T t = current.RTGetAttribute(false);
				if (t != null)
				{
					yield return t;
				}
				current = current.BaseType;
			}
			yield break;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000BD38 File Offset: 0x00009F38
		public static ParameterInfo[] RTGetDelegateTypeParameters(this Type delegateType)
		{
			if (delegateType == null || !delegateType.RTIsSubclassOf(typeof(Delegate)))
			{
				return new ParameterInfo[0];
			}
			return delegateType.RTGetMethod("Invoke").GetParameters();
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000BD6C File Offset: 0x00009F6C
		public static T RTCreateDelegate<T>(this MethodInfo method, object instance) where T : Delegate
		{
			return (T)((object)method.RTCreateDelegate(typeof(T), instance));
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000BD84 File Offset: 0x00009F84
		public static Delegate RTCreateDelegate(this MethodInfo method, Type type, object instance)
		{
			if (instance != null)
			{
				Type type2 = instance.GetType();
				if (method.DeclaringType != type2)
				{
					method = type2.RTGetMethod(method.Name, (from p in method.GetParameters()
					select p.ParameterType).ToArray<Type>(), null, null);
				}
			}
			return Delegate.CreateDelegate(type, instance, method);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000BDF0 File Offset: 0x00009FF0
		public static Delegate ConvertDelegate(Delegate originalDelegate, Type targetDelegateType)
		{
			return Delegate.CreateDelegate(targetDelegateType, originalDelegate.Target, originalDelegate.Method);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000BE04 File Offset: 0x0000A004
		public static bool IsReadOnly(this FieldInfo field)
		{
			return field.IsInitOnly || field.IsLiteral;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000BE16 File Offset: 0x0000A016
		public static bool IsConstant(this FieldInfo field)
		{
			return field.IsReadOnly() && field.IsStatic;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000BE28 File Offset: 0x0000A028
		public static bool IsStatic(this EventInfo info)
		{
			MethodInfo addMethod = info.GetAddMethod();
			return addMethod != null && addMethod.IsStatic;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000BE50 File Offset: 0x0000A050
		public static bool IsStatic(this PropertyInfo info)
		{
			MethodInfo getMethod = info.GetGetMethod();
			return getMethod != null && getMethod.IsStatic;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000BE75 File Offset: 0x0000A075
		public static bool IsParams(this ParameterInfo parameter, ParameterInfo[] parameters)
		{
			return parameter.Position == parameters.Length - 1 && parameter.IsDefined(typeof(ParamArrayAttribute), false);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0000BE98 File Offset: 0x0000A098
		public static bool IsObsolete(this MemberInfo member, bool inherited = true)
		{
			bool result;
			if (ReflectionTools._obsoleteCache.TryGetValue(member, ref result))
			{
				return result;
			}
			MemberInfo member2 = member;
			if (member is MethodInfo)
			{
				MethodInfo method = (MethodInfo)member;
				if (method.IsPropertyAccessor())
				{
					member2 = method.GetAccessorProperty();
				}
			}
			bool flag = member2.RTIsDefined(inherited);
			return ReflectionTools._obsoleteCache[member] = flag;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000BEF0 File Offset: 0x0000A0F0
		public static PropertyInfo GetBaseDefinition(this PropertyInfo propertyInfo)
		{
			MethodInfo methodInfo = propertyInfo.GetAccessors(true).FirstOrDefault<MethodInfo>();
			if (methodInfo == null)
			{
				return null;
			}
			MethodInfo baseDefinition = methodInfo.GetBaseDefinition();
			if (baseDefinition == methodInfo)
			{
				return propertyInfo;
			}
			Type[] array = (from p in propertyInfo.GetIndexParameters()
			select p.ParameterType).ToArray<Type>();
			return baseDefinition.DeclaringType.GetProperty(propertyInfo.Name, 124, null, propertyInfo.PropertyType, array, null);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000BF73 File Offset: 0x0000A173
		public static FieldInfo GetBaseDefinition(this FieldInfo fieldInfo)
		{
			return fieldInfo.DeclaringType.RTGetField(fieldInfo.Name, false);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000BF88 File Offset: 0x0000A188
		public static MethodInfo[] GetExtensionMethods(this Type targetType)
		{
			MethodInfo[] result = null;
			if (ReflectionTools._typeExtensions.TryGetValue(targetType, ref result))
			{
				return result;
			}
			List<MethodInfo> list = new List<MethodInfo>();
			foreach (Type type in ReflectionTools.GetAllTypes(false))
			{
				if (type.IsSealed && !type.IsGenericType && type.RTIsDefined(true))
				{
					foreach (MethodInfo methodInfo in type.RTGetMethods())
					{
						if (methodInfo.IsExtensionMethod() && methodInfo.GetParameters()[0].ParameterType.RTIsAssignableFrom(targetType))
						{
							list.Add(methodInfo);
						}
					}
				}
			}
			return ReflectionTools._typeExtensions[targetType] = list.ToArray();
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000C044 File Offset: 0x0000A244
		public static bool IsExtensionMethod(this MethodInfo method)
		{
			return method.RTIsDefined(true);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000C04D File Offset: 0x0000A24D
		public static bool IsPropertyAccessor(this MethodInfo method)
		{
			return method.GetMethodSpecialType() == ReflectionTools.MethodType.PropertyAccessor;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000C058 File Offset: 0x0000A258
		public static bool IsIndexerProperty(this PropertyInfo property)
		{
			return property.GetIndexParameters().Length != 0;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000C064 File Offset: 0x0000A264
		public static bool IsAutoProperty(this PropertyInfo property)
		{
			if (!property.CanRead || !property.CanWrite)
			{
				return false;
			}
			string name = "<" + property.Name + ">k__BackingField";
			return property.DeclaringType.RTGetField(name, false) != null;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000C0AC File Offset: 0x0000A2AC
		public static PropertyInfo GetAccessorProperty(this MethodInfo method)
		{
			if (method.IsPropertyAccessor())
			{
				return method.RTReflectedOrDeclaredType().RTGetProperty(method.Name.Substring(4));
			}
			return null;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000C0CF File Offset: 0x0000A2CF
		public static bool IsEnumerableCollection(this Type type)
		{
			return !(type == null) && typeof(IEnumerable).RTIsAssignableFrom(type) && (type.RTIsGenericType() || type.RTIsArray());
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000C100 File Offset: 0x0000A300
		public static Type GetEnumerableElementType(this Type type)
		{
			if (type == null)
			{
				return null;
			}
			if (!typeof(IEnumerable).RTIsAssignableFrom(type))
			{
				return null;
			}
			if (type.HasElementType || type.RTIsArray())
			{
				return type.GetElementType();
			}
			if (type.RTIsGenericType())
			{
				Type[] array = type.RTGetGenericArguments();
				if (array.Length == 1)
				{
					return array[0];
				}
				if (typeof(IDictionary).RTIsAssignableFrom(type) && array.Length == 2)
				{
					return array[1];
				}
			}
			return null;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000C17C File Offset: 0x0000A37C
		public static Type GetSingleGenericArgument(this Type type)
		{
			if (!type.RTIsGenericType())
			{
				return null;
			}
			Type[] array = type.RTGetGenericArguments();
			if (array.Length != 1)
			{
				return null;
			}
			return array[0];
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000C1A8 File Offset: 0x0000A3A8
		public static Type GetFirstGenericParameterConstraintType(this Type type)
		{
			if (type == null || !type.RTIsGenericType())
			{
				return null;
			}
			type = type.GetGenericTypeDefinition();
			Type type2 = type.RTGetGenericArguments().First<Type>().GetGenericParameterConstraints().FirstOrDefault<Type>();
			if (!(type2 != null))
			{
				return typeof(object);
			}
			return type2;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000C1FC File Offset: 0x0000A3FC
		public static Type GetFirstGenericParameterConstraintType(this MethodInfo method)
		{
			if (method == null || !method.IsGenericMethod)
			{
				return null;
			}
			method = method.GetGenericMethodDefinition();
			Type type = method.RTGetGenericArguments().First<Type>().GetGenericParameterConstraints().FirstOrDefault<Type>();
			if (!(type != null))
			{
				return typeof(object);
			}
			return type;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000C250 File Offset: 0x0000A450
		public static bool TryMakeGeneric(this Type def, Type argType, out Type result)
		{
			result = null;
			if (def == null || argType == null || !def.IsGenericType)
			{
				return false;
			}
			bool result2;
			try
			{
				result = def.GetGenericTypeDefinition().MakeGenericType(new Type[]
				{
					argType
				});
				result2 = true;
			}
			catch
			{
				result2 = false;
			}
			return result2;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000C2B0 File Offset: 0x0000A4B0
		public static bool TryMakeGeneric(this MethodInfo def, Type argType, out MethodInfo result)
		{
			result = null;
			if (def == null || argType == null || !def.IsGenericMethod)
			{
				return false;
			}
			bool result2;
			try
			{
				result = def.GetGenericMethodDefinition().MakeGenericMethod(new Type[]
				{
					argType
				});
				result2 = true;
			}
			catch
			{
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000C310 File Offset: 0x0000A510
		public static Array Resize(this Array array, int newSize)
		{
			if (array == null)
			{
				return null;
			}
			int length = array.Length;
			Array array2 = Array.CreateInstance(array.GetType().GetElementType(), newSize);
			int num = Math.Min(length, newSize);
			if (num > 0)
			{
				Array.Copy(array, array2, num);
			}
			return array2;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000C350 File Offset: 0x0000A550
		public static bool TryConvert(Type fromType, Type toType, out UnaryExpression exp)
		{
			bool result;
			try
			{
				exp = Expression.Convert(Expression.Parameter(fromType, null), toType);
				result = true;
			}
			catch
			{
				exp = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000C38C File Offset: 0x0000A58C
		public static void DigFields(object root, Predicate<FieldInfo> move, Action<object> push, Action<object> pop)
		{
			if (root == null)
			{
				return;
			}
			Type type = root.GetType();
			if (type.IsPrimitive || type == typeof(string))
			{
				return;
			}
			if (push != null)
			{
				push.Invoke(root);
			}
			foreach (FieldInfo fieldInfo in type.RTGetFields())
			{
				if (!fieldInfo.IsStatic && !fieldInfo.FieldType.IsPrimitive && fieldInfo.FieldType != typeof(string) && move.Invoke(fieldInfo))
				{
					object value = fieldInfo.GetValue(root);
					if (value != null)
					{
						if (value is IList)
						{
							using (IEnumerator enumerator = ((IList)value).GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									object root2 = enumerator.Current;
									ReflectionTools.DigFields(root2, move, push, pop);
								}
								goto IL_132;
							}
						}
						if (value is IDictionary)
						{
							using (IEnumerator enumerator = ((IDictionary)value).Values.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									object root3 = enumerator.Current;
									ReflectionTools.DigFields(root3, move, push, pop);
								}
								goto IL_132;
							}
						}
						ReflectionTools.DigFields(value, move, push, pop);
					}
				}
				IL_132:;
			}
			if (pop != null)
			{
				pop.Invoke(root);
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000C500 File Offset: 0x0000A700
		public static Func<T, TResult> GetFieldGetter<T, TResult>(FieldInfo info)
		{
			DynamicMethod dynamicMethod = new DynamicMethod(string.Format("__get_field_{0}_", info.Name), typeof(TResult), new Type[]
			{
				typeof(T)
			}, typeof(T));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldfld, info);
			ilgenerator.Emit(OpCodes.Ret);
			return (Func<T, TResult>)dynamicMethod.CreateDelegate(typeof(Func<T, TResult>));
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000C584 File Offset: 0x0000A784
		public static Action<T, TValue> GetFieldSetter<T, TValue>(FieldInfo info)
		{
			DynamicMethod dynamicMethod = new DynamicMethod(string.Format("__set_field_{0}_", info.Name), typeof(void), new Type[]
			{
				typeof(T),
				typeof(TValue)
			}, typeof(T));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Stfld, info);
			ilgenerator.Emit(OpCodes.Ret);
			return (Action<T, TValue>)dynamicMethod.CreateDelegate(typeof(Action<T, TValue>));
		}

		// Token: 0x04000158 RID: 344
		public const BindingFlags FLAGS_ALL = 124;

		// Token: 0x04000159 RID: 345
		public const BindingFlags FLAGS_ALL_DECLARED = 62;

		// Token: 0x0400015A RID: 346
		private static Assembly[] _loadedAssemblies;

		// Token: 0x0400015B RID: 347
		private static Type[] _allTypes;

		// Token: 0x0400015C RID: 348
		private static object[] _tempArgs;

		// Token: 0x0400015D RID: 349
		private static Dictionary<string, Type> _typesMap;

		// Token: 0x0400015E RID: 350
		private static Dictionary<Type, Type[]> _subTypesMap;

		// Token: 0x0400015F RID: 351
		private static Dictionary<MethodBase, ReflectionTools.MethodType> _methodSpecialType;

		// Token: 0x04000160 RID: 352
		private static Dictionary<Type, string> _typeFriendlyName;

		// Token: 0x04000161 RID: 353
		private static Dictionary<Type, string> _typeFriendlyNameCompileSafe;

		// Token: 0x04000162 RID: 354
		private static Dictionary<MethodBase, string> _methodSignatures;

		// Token: 0x04000163 RID: 355
		private static Dictionary<Type, ConstructorInfo[]> _typeConstructors;

		// Token: 0x04000164 RID: 356
		private static Dictionary<Type, MethodInfo[]> _typeMethods;

		// Token: 0x04000165 RID: 357
		private static Dictionary<Type, FieldInfo[]> _typeFields;

		// Token: 0x04000166 RID: 358
		private static Dictionary<Type, PropertyInfo[]> _typeProperties;

		// Token: 0x04000167 RID: 359
		private static Dictionary<Type, EventInfo[]> _typeEvents;

		// Token: 0x04000168 RID: 360
		private static Dictionary<MemberInfo, object[]> _memberAttributes;

		// Token: 0x04000169 RID: 361
		private static Dictionary<MemberInfo, bool> _obsoleteCache;

		// Token: 0x0400016A RID: 362
		private static Dictionary<Type, MethodInfo[]> _typeExtensions;

		// Token: 0x0400016B RID: 363
		private static Dictionary<Type, Type[]> _genericArgsTypeCache;

		// Token: 0x0400016C RID: 364
		private static Dictionary<MethodInfo, Type[]> _genericArgsMathodCache;

		// Token: 0x0400016D RID: 365
		public static readonly Dictionary<string, string> op_FriendlyNamesLong;

		// Token: 0x0400016E RID: 366
		public static readonly Dictionary<string, string> op_FriendlyNamesShort;

		// Token: 0x0400016F RID: 367
		public static readonly Dictionary<string, string> op_CSharpAliases;

		// Token: 0x04000170 RID: 368
		public const string METHOD_SPECIAL_NAME_GET = "get_";

		// Token: 0x04000171 RID: 369
		public const string METHOD_SPECIAL_NAME_SET = "set_";

		// Token: 0x04000172 RID: 370
		public const string METHOD_SPECIAL_NAME_ADD = "add_";

		// Token: 0x04000173 RID: 371
		public const string METHOD_SPECIAL_NAME_REMOVE = "remove_";

		// Token: 0x04000174 RID: 372
		public const string METHOD_SPECIAL_NAME_OP = "op_";

		// Token: 0x02000119 RID: 281
		public enum MethodType
		{
			// Token: 0x040002CD RID: 717
			Normal,
			// Token: 0x040002CE RID: 718
			PropertyAccessor,
			// Token: 0x040002CF RID: 719
			Event,
			// Token: 0x040002D0 RID: 720
			Operator
		}
	}
}
