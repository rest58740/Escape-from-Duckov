using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x02000010 RID: 16
	public static class TypeExtensions
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x000043CC File Offset: 0x000025CC
		private static string GetCachedNiceName(Type type)
		{
			object cachedNiceNames_LOCK = TypeExtensions.CachedNiceNames_LOCK;
			string text;
			lock (cachedNiceNames_LOCK)
			{
				if (!TypeExtensions.CachedNiceNames.TryGetValue(type, ref text))
				{
					text = TypeExtensions.CreateNiceName(type);
					TypeExtensions.CachedNiceNames.Add(type, text);
				}
			}
			return text;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004428 File Offset: 0x00002628
		private static string CreateNiceName(Type type)
		{
			if (type.IsArray)
			{
				int arrayRank = type.GetArrayRank();
				return type.GetElementType().GetNiceName() + ((arrayRank == 1) ? "[]" : "[,]");
			}
			if (type.InheritsFrom(typeof(Nullable)))
			{
				return type.GetGenericArguments()[0].GetNiceName() + "?";
			}
			if (type.IsByRef)
			{
				return "ref " + type.GetElementType().GetNiceName();
			}
			if (type.IsGenericParameter || !type.IsGenericType)
			{
				return type.GetMaybeSimplifiedTypeName();
			}
			StringBuilder stringBuilder = new StringBuilder();
			string name = type.Name;
			int num = name.IndexOf("`");
			if (num != -1)
			{
				stringBuilder.Append(name.Substring(0, num));
			}
			else
			{
				stringBuilder.Append(name);
			}
			stringBuilder.Append('<');
			Type[] genericArguments = type.GetGenericArguments();
			for (int i = 0; i < genericArguments.Length; i++)
			{
				Type type2 = genericArguments[i];
				if (i != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(type2.GetNiceName());
			}
			stringBuilder.Append('>');
			return stringBuilder.ToString();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004554 File Offset: 0x00002754
		private static bool HasCastDefined(this Type from, Type to, bool requireImplicitCast)
		{
			if (from.IsEnum)
			{
				return Enum.GetUnderlyingType(from).IsCastableTo(to, false);
			}
			if (to.IsEnum)
			{
				return Enum.GetUnderlyingType(to).IsCastableTo(from, false);
			}
			if ((!from.IsPrimitive && !(from == TypeExtensions.VoidPointerType)) || (!to.IsPrimitive && !(to == TypeExtensions.VoidPointerType)))
			{
				return from.GetCastMethod(to, requireImplicitCast) != null;
			}
			if (requireImplicitCast)
			{
				return TypeExtensions.PrimitiveImplicitCasts[from].Contains(to);
			}
			if (from == typeof(IntPtr))
			{
				if (to == typeof(UIntPtr))
				{
					return false;
				}
				if (to == TypeExtensions.VoidPointerType)
				{
					return true;
				}
			}
			else if (from == typeof(UIntPtr))
			{
				if (to == typeof(IntPtr))
				{
					return false;
				}
				if (to == TypeExtensions.VoidPointerType)
				{
					return true;
				}
			}
			return TypeExtensions.ExplicitCastIntegrals.Contains(from) && TypeExtensions.ExplicitCastIntegrals.Contains(to);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004668 File Offset: 0x00002868
		public static bool IsValidIdentifier(string identifier)
		{
			if (identifier == null || identifier.Length == 0)
			{
				return false;
			}
			int num = identifier.IndexOf('.');
			if (num >= 0)
			{
				string[] array = identifier.Split(new char[]
				{
					'.'
				});
				for (int i = 0; i < array.Length; i++)
				{
					if (!TypeExtensions.IsValidIdentifier(array[i]))
					{
						return false;
					}
				}
				return true;
			}
			if (TypeExtensions.ReservedCSharpKeywords.Contains(identifier))
			{
				return false;
			}
			if (!TypeExtensions.IsValidIdentifierStartCharacter(identifier.get_Chars(0)))
			{
				return false;
			}
			for (int j = 1; j < identifier.Length; j++)
			{
				if (!TypeExtensions.IsValidIdentifierPartCharacter(identifier.get_Chars(j)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000046FE File Offset: 0x000028FE
		private static bool IsValidIdentifierStartCharacter(char c)
		{
			return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_' || c == '@' || char.IsLetter(c);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004726 File Offset: 0x00002926
		private static bool IsValidIdentifierPartCharacter(char c)
		{
			return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_' || (c >= '0' && c <= '9') || char.IsLetter(c);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004754 File Offset: 0x00002954
		public static bool IsCastableTo(this Type from, Type to, bool requireImplicitCast = false)
		{
			if (from == null)
			{
				throw new ArgumentNullException("from");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to");
			}
			return from == to || to.IsAssignableFrom(from) || from.HasCastDefined(to, requireImplicitCast);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000047A8 File Offset: 0x000029A8
		public static Func<object, object> GetCastMethodDelegate(this Type from, Type to, bool requireImplicitCast = false)
		{
			object weaklyTypedTypeCastDelegates_LOCK = TypeExtensions.WeaklyTypedTypeCastDelegates_LOCK;
			Func<object, object> func;
			lock (weaklyTypedTypeCastDelegates_LOCK)
			{
				if (!TypeExtensions.WeaklyTypedTypeCastDelegates.TryGetInnerValue(from, to, out func))
				{
					MethodInfo method = from.GetCastMethod(to, requireImplicitCast);
					if (method != null)
					{
						func = ((object obj) => method.Invoke(null, new object[]
						{
							obj
						}));
					}
					TypeExtensions.WeaklyTypedTypeCastDelegates.AddInner(from, to, func);
				}
			}
			return func;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004830 File Offset: 0x00002A30
		public static Func<TFrom, TTo> GetCastMethodDelegate<TFrom, TTo>(bool requireImplicitCast = false)
		{
			object stronglyTypedTypeCastDelegates_LOCK = TypeExtensions.StronglyTypedTypeCastDelegates_LOCK;
			Delegate @delegate;
			lock (stronglyTypedTypeCastDelegates_LOCK)
			{
				if (!TypeExtensions.StronglyTypedTypeCastDelegates.TryGetInnerValue(typeof(TFrom), typeof(TTo), out @delegate))
				{
					MethodInfo castMethod = typeof(TFrom).GetCastMethod(typeof(TTo), requireImplicitCast);
					if (castMethod != null)
					{
						@delegate = Delegate.CreateDelegate(typeof(Func<TFrom, TTo>), castMethod);
					}
					TypeExtensions.StronglyTypedTypeCastDelegates.AddInner(typeof(TFrom), typeof(TTo), @delegate);
				}
			}
			return (Func<TFrom, TTo>)@delegate;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000048E8 File Offset: 0x00002AE8
		public static MethodInfo GetCastMethod(this Type from, Type to, bool requireImplicitCast = false)
		{
			IEnumerable<MethodInfo> allMembers = from.GetAllMembers(24);
			foreach (MethodInfo methodInfo in allMembers)
			{
				if ((methodInfo.Name == "op_Implicit" || (!requireImplicitCast && methodInfo.Name == "op_Explicit")) && methodInfo.GetParameters()[0].ParameterType.IsAssignableFrom(from) && to.IsAssignableFrom(methodInfo.ReturnType))
				{
					return methodInfo;
				}
			}
			IEnumerable<MethodInfo> allMembers2 = to.GetAllMembers(24);
			foreach (MethodInfo methodInfo2 in allMembers2)
			{
				if ((methodInfo2.Name == "op_Implicit" || (!requireImplicitCast && methodInfo2.Name == "op_Explicit")) && methodInfo2.GetParameters()[0].ParameterType.IsAssignableFrom(from) && to.IsAssignableFrom(methodInfo2.ReturnType))
				{
					return methodInfo2;
				}
			}
			return null;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004A1C File Offset: 0x00002C1C
		private static bool FloatEqualityComparer(float a, float b)
		{
			return (float.IsNaN(a) && float.IsNaN(b)) || a == b;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004A34 File Offset: 0x00002C34
		private static bool DoubleEqualityComparer(double a, double b)
		{
			return (double.IsNaN(a) && double.IsNaN(b)) || a == b;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004A4C File Offset: 0x00002C4C
		private static bool QuaternionEqualityComparer(Quaternion a, Quaternion b)
		{
			return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004A88 File Offset: 0x00002C88
		public static Func<T, T, bool> GetEqualityComparerDelegate<T>()
		{
			if (typeof(T) == typeof(float))
			{
				return (Func<T, T, bool>)TypeExtensions.FloatEqualityComparerFunc;
			}
			if (typeof(T) == typeof(double))
			{
				return (Func<T, T, bool>)TypeExtensions.DoubleEqualityComparerFunc;
			}
			if (typeof(T) == typeof(Quaternion))
			{
				return (Func<T, T, bool>)TypeExtensions.QuaternionEqualityComparerFunc;
			}
			Func<T, T, bool> func = null;
			if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
			{
				if (typeof(T).IsValueType)
				{
					func = ((T a, T b) => ((IEquatable<T>)((object)a)).Equals(b));
				}
				else
				{
					func = ((T a, T b) => a == b || (a != null && ((IEquatable<T>)((object)a)).Equals(b)));
				}
			}
			else
			{
				Type type = typeof(T);
				while (type != null && type != typeof(object))
				{
					MethodInfo operatorMethod = type.GetOperatorMethod(Operator.Equality, type, type);
					if (operatorMethod != null)
					{
						func = (Func<T, T, bool>)Delegate.CreateDelegate(typeof(Func<T, T, bool>), operatorMethod, true);
						break;
					}
					type = type.BaseType;
				}
			}
			if (func == null)
			{
				EqualityComparer<T> @default = EqualityComparer<T>.Default;
				func = new Func<T, T, bool>(@default.Equals);
			}
			return func;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004BEC File Offset: 0x00002DEC
		public static T GetAttribute<T>(this Type type, bool inherit) where T : Attribute
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(T), inherit);
			if (customAttributes.Length == 0)
			{
				return default(T);
			}
			return (T)((object)customAttributes[0]);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004C21 File Offset: 0x00002E21
		public static bool ImplementsOrInherits(this Type type, Type to)
		{
			return to.IsAssignableFrom(type);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004C2A File Offset: 0x00002E2A
		public static bool ImplementsOpenGenericType(this Type candidateType, Type openGenericType)
		{
			if (openGenericType.IsInterface)
			{
				return candidateType.ImplementsOpenGenericInterface(openGenericType);
			}
			return candidateType.ImplementsOpenGenericClass(openGenericType);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004C44 File Offset: 0x00002E44
		public static bool ImplementsOpenGenericInterface(this Type candidateType, Type openGenericInterfaceType)
		{
			if (candidateType == openGenericInterfaceType)
			{
				return true;
			}
			if (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == openGenericInterfaceType)
			{
				return true;
			}
			Type[] interfaces = candidateType.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				if (interfaces[i].ImplementsOpenGenericInterface(openGenericInterfaceType))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004C98 File Offset: 0x00002E98
		public static bool ImplementsOpenGenericClass(this Type candidateType, Type openGenericType)
		{
			if (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == openGenericType)
			{
				return true;
			}
			Type baseType = candidateType.BaseType;
			return baseType != null && baseType.ImplementsOpenGenericClass(openGenericType);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004CD9 File Offset: 0x00002ED9
		public static Type[] GetArgumentsOfInheritedOpenGenericType(this Type candidateType, Type openGenericType)
		{
			if (openGenericType.IsInterface)
			{
				return candidateType.GetArgumentsOfInheritedOpenGenericInterface(openGenericType);
			}
			return candidateType.GetArgumentsOfInheritedOpenGenericClass(openGenericType);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004CF4 File Offset: 0x00002EF4
		public static Type[] GetArgumentsOfInheritedOpenGenericClass(this Type candidateType, Type openGenericType)
		{
			if (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == openGenericType)
			{
				return candidateType.GetGenericArguments();
			}
			Type baseType = candidateType.BaseType;
			if (baseType != null)
			{
				return baseType.GetArgumentsOfInheritedOpenGenericClass(openGenericType);
			}
			return null;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004D38 File Offset: 0x00002F38
		public static Type[] GetArgumentsOfInheritedOpenGenericInterface(this Type candidateType, Type openGenericInterfaceType)
		{
			if ((openGenericInterfaceType == TypeExtensions.GenericListInterface || openGenericInterfaceType == TypeExtensions.GenericCollectionInterface) && candidateType.IsArray)
			{
				return new Type[]
				{
					candidateType.GetElementType()
				};
			}
			if (candidateType == openGenericInterfaceType)
			{
				return candidateType.GetGenericArguments();
			}
			if (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == openGenericInterfaceType)
			{
				return candidateType.GetGenericArguments();
			}
			foreach (Type type in candidateType.GetInterfaces())
			{
				if (type.IsGenericType)
				{
					Type[] argumentsOfInheritedOpenGenericInterface = type.GetArgumentsOfInheritedOpenGenericInterface(openGenericInterfaceType);
					if (argumentsOfInheritedOpenGenericInterface != null)
					{
						return argumentsOfInheritedOpenGenericInterface;
					}
				}
			}
			return null;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004DD4 File Offset: 0x00002FD4
		public static MethodInfo GetOperatorMethod(this Type type, Operator op, Type leftOperand, Type rightOperand)
		{
			string text;
			switch (op)
			{
			case Operator.Equality:
				text = "op_Equality";
				break;
			case Operator.Inequality:
				text = "op_Inequality";
				break;
			case Operator.Addition:
				text = "op_Addition";
				break;
			case Operator.Subtraction:
				text = "op_Subtraction";
				break;
			case Operator.Multiply:
				text = "op_Multiply";
				break;
			case Operator.Division:
				text = "op_Division";
				break;
			case Operator.LessThan:
				text = "op_LessThan";
				break;
			case Operator.GreaterThan:
				text = "op_GreaterThan";
				break;
			case Operator.LessThanOrEqual:
				text = "op_LessThanOrEqual";
				break;
			case Operator.GreaterThanOrEqual:
				text = "op_GreaterThanOrEqual";
				break;
			case Operator.Modulus:
				text = "op_Modulus";
				break;
			case Operator.RightShift:
				text = "op_RightShift";
				break;
			case Operator.LeftShift:
				text = "op_LeftShift";
				break;
			case Operator.BitwiseAnd:
				text = "op_BitwiseAnd";
				break;
			case Operator.BitwiseOr:
				text = "op_BitwiseOr";
				break;
			case Operator.ExclusiveOr:
				text = "op_ExclusiveOr";
				break;
			case Operator.BitwiseComplement:
				text = "op_OnesComplement";
				break;
			case Operator.LogicalAnd:
			case Operator.LogicalOr:
				return null;
			case Operator.LogicalNot:
				text = "op_LogicalNot";
				break;
			default:
				throw new NotImplementedException();
			}
			Type[] twoLengthTypeArray_Cached = TypeExtensions.TwoLengthTypeArray_Cached;
			Type[] array = twoLengthTypeArray_Cached;
			MethodInfo result;
			lock (array)
			{
				twoLengthTypeArray_Cached[0] = leftOperand;
				twoLengthTypeArray_Cached[1] = rightOperand;
				try
				{
					MethodInfo method = type.GetMethod(text, 56, null, twoLengthTypeArray_Cached, null);
					if (method != null && method.ReturnType != typeof(bool))
					{
						result = null;
					}
					else
					{
						result = method;
					}
				}
				catch (AmbiguousMatchException)
				{
					foreach (MethodInfo methodInfo in type.GetMethods(56))
					{
						if (!(methodInfo.Name != text) && !(methodInfo.ReturnType != typeof(bool)))
						{
							ParameterInfo[] parameters = methodInfo.GetParameters();
							if (parameters.Length == 2 && parameters[0].ParameterType.IsAssignableFrom(leftOperand) && parameters[1].ParameterType.IsAssignableFrom(rightOperand))
							{
								return methodInfo;
							}
						}
					}
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004FEC File Offset: 0x000031EC
		public static MethodInfo GetOperatorMethod(this Type type, Operator op)
		{
			string methodName;
			switch (op)
			{
			case Operator.Equality:
				methodName = "op_Equality";
				break;
			case Operator.Inequality:
				methodName = "op_Inequality";
				break;
			case Operator.Addition:
				methodName = "op_Addition";
				break;
			case Operator.Subtraction:
				methodName = "op_Subtraction";
				break;
			case Operator.Multiply:
				methodName = "op_Multiply";
				break;
			case Operator.Division:
				methodName = "op_Division";
				break;
			case Operator.LessThan:
				methodName = "op_LessThan";
				break;
			case Operator.GreaterThan:
				methodName = "op_GreaterThan";
				break;
			case Operator.LessThanOrEqual:
				methodName = "op_LessThanOrEqual";
				break;
			case Operator.GreaterThanOrEqual:
				methodName = "op_GreaterThanOrEqual";
				break;
			case Operator.Modulus:
				methodName = "op_Modulus";
				break;
			case Operator.RightShift:
				methodName = "op_RightShift";
				break;
			case Operator.LeftShift:
				methodName = "op_LeftShift";
				break;
			case Operator.BitwiseAnd:
				methodName = "op_BitwiseAnd";
				break;
			case Operator.BitwiseOr:
				methodName = "op_BitwiseOr";
				break;
			case Operator.ExclusiveOr:
				methodName = "op_ExclusiveOr";
				break;
			case Operator.BitwiseComplement:
				methodName = "op_OnesComplement";
				break;
			case Operator.LogicalAnd:
			case Operator.LogicalOr:
				return null;
			case Operator.LogicalNot:
				methodName = "op_LogicalNot";
				break;
			default:
				throw new NotImplementedException();
			}
			return type.GetAllMembers(56).FirstOrDefault((MethodInfo m) => m.Name == methodName);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005180 File Offset: 0x00003380
		public static MethodInfo[] GetOperatorMethods(this Type type, Operator op)
		{
			string methodName;
			switch (op)
			{
			case Operator.Equality:
				methodName = "op_Equality";
				break;
			case Operator.Inequality:
				methodName = "op_Inequality";
				break;
			case Operator.Addition:
				methodName = "op_Addition";
				break;
			case Operator.Subtraction:
				methodName = "op_Subtraction";
				break;
			case Operator.Multiply:
				methodName = "op_Multiply";
				break;
			case Operator.Division:
				methodName = "op_Division";
				break;
			case Operator.LessThan:
				methodName = "op_LessThan";
				break;
			case Operator.GreaterThan:
				methodName = "op_GreaterThan";
				break;
			case Operator.LessThanOrEqual:
				methodName = "op_LessThanOrEqual";
				break;
			case Operator.GreaterThanOrEqual:
				methodName = "op_GreaterThanOrEqual";
				break;
			case Operator.Modulus:
				methodName = "op_Modulus";
				break;
			case Operator.RightShift:
				methodName = "op_RightShift";
				break;
			case Operator.LeftShift:
				methodName = "op_LeftShift";
				break;
			case Operator.BitwiseAnd:
				methodName = "op_BitwiseAnd";
				break;
			case Operator.BitwiseOr:
				methodName = "op_BitwiseOr";
				break;
			case Operator.ExclusiveOr:
				methodName = "op_ExclusiveOr";
				break;
			case Operator.BitwiseComplement:
				methodName = "op_OnesComplement";
				break;
			case Operator.LogicalAnd:
			case Operator.LogicalOr:
				return null;
			case Operator.LogicalNot:
				methodName = "op_LogicalNot";
				break;
			default:
				throw new NotImplementedException();
			}
			return (from x in type.GetAllMembers(56)
			where x.Name == methodName
			select x).ToArray<MethodInfo>();
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005316 File Offset: 0x00003516
		public static IEnumerable<MemberInfo> GetAllMembers(this Type type, BindingFlags flags = 0)
		{
			Type currentType = type;
			if ((flags & 2) == 2)
			{
				foreach (MemberInfo memberInfo in currentType.GetMembers(flags))
				{
					yield return memberInfo;
				}
				MemberInfo[] array = null;
			}
			else
			{
				flags |= 2;
				do
				{
					foreach (MemberInfo memberInfo2 in currentType.GetMembers(flags))
					{
						yield return memberInfo2;
					}
					MemberInfo[] array = null;
					currentType = currentType.BaseType;
				}
				while (currentType != null);
			}
			yield break;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000532D File Offset: 0x0000352D
		public static IEnumerable<MemberInfo> GetAllMembers(this Type type, string name, BindingFlags flags = 0)
		{
			foreach (MemberInfo memberInfo in type.GetAllMembers(flags))
			{
				if (!(memberInfo.Name != name))
				{
					yield return memberInfo;
				}
			}
			IEnumerator<MemberInfo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000534B File Offset: 0x0000354B
		public static IEnumerable<T> GetAllMembers<T>(this Type type, BindingFlags flags = 0) where T : MemberInfo
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type == typeof(object))
			{
				yield break;
			}
			Type currentType = type;
			if ((flags & 2) == 2)
			{
				foreach (MemberInfo memberInfo in currentType.GetMembers(flags))
				{
					T t = memberInfo as T;
					if (t != null)
					{
						yield return t;
					}
				}
				MemberInfo[] array = null;
			}
			else
			{
				flags |= 2;
				do
				{
					foreach (MemberInfo memberInfo2 in currentType.GetMembers(flags))
					{
						T t2 = memberInfo2 as T;
						if (t2 != null)
						{
							yield return t2;
						}
					}
					MemberInfo[] array = null;
					currentType = currentType.BaseType;
				}
				while (currentType != null);
			}
			yield break;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005364 File Offset: 0x00003564
		public static Type GetGenericBaseType(this Type type, Type baseType)
		{
			int num;
			return type.GetGenericBaseType(baseType, out num);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000537C File Offset: 0x0000357C
		public static Type GetGenericBaseType(this Type type, Type baseType, out int depthCount)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (baseType == null)
			{
				throw new ArgumentNullException("baseType");
			}
			if (!baseType.IsGenericType)
			{
				throw new ArgumentException("Type " + baseType.Name + " is not a generic type.");
			}
			if (!type.InheritsFrom(baseType))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Type ",
					type.Name,
					" does not inherit from ",
					baseType.Name,
					"."
				}));
			}
			Type type2 = type;
			depthCount = 0;
			while (type2 != null && (!type2.IsGenericType || type2.GetGenericTypeDefinition() != baseType))
			{
				depthCount++;
				type2 = type2.BaseType;
			}
			if (type2 == null)
			{
				throw new ArgumentException(type.Name + " is assignable from " + baseType.Name + ", but base type was not found?");
			}
			return type2;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005478 File Offset: 0x00003678
		public static IEnumerable<Type> GetBaseTypes(this Type type, bool includeSelf = false)
		{
			IEnumerable<Type> enumerable = type.GetBaseClasses(includeSelf).Concat(type.GetInterfaces());
			if (includeSelf && type.IsInterface)
			{
				enumerable.Concat(new Type[]
				{
					type
				});
			}
			return enumerable;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000054B5 File Offset: 0x000036B5
		public static IEnumerable<Type> GetBaseClasses(this Type type, bool includeSelf = false)
		{
			if (type == null || type.BaseType == null)
			{
				yield break;
			}
			if (includeSelf)
			{
				yield return type;
			}
			Type current = type.BaseType;
			while (current != null)
			{
				yield return current;
				current = current.BaseType;
			}
			yield break;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000054CC File Offset: 0x000036CC
		private static string GetMaybeSimplifiedTypeName(this Type type)
		{
			string result;
			if (TypeExtensions.TypeNameAlternatives.TryGetValue(type, ref result))
			{
				return result;
			}
			return type.Name;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000054F0 File Offset: 0x000036F0
		public static string GetNiceName(this Type type)
		{
			if (type.IsNested && !type.IsGenericParameter)
			{
				return type.DeclaringType.GetNiceName() + "." + TypeExtensions.GetCachedNiceName(type);
			}
			return TypeExtensions.GetCachedNiceName(type);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005524 File Offset: 0x00003724
		public static string GetNiceFullName(this Type type)
		{
			if (type.IsNested && !type.IsGenericParameter)
			{
				return type.DeclaringType.GetNiceFullName() + "." + TypeExtensions.GetCachedNiceName(type);
			}
			string text = TypeExtensions.GetCachedNiceName(type);
			if (type.Namespace != null)
			{
				text = type.Namespace + "." + text;
			}
			return text;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000557F File Offset: 0x0000377F
		public static string GetCompilableNiceName(this Type type)
		{
			return type.GetNiceName().Replace('<', '_').Replace('>', '_').TrimEnd(new char[]
			{
				'_'
			});
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000055A9 File Offset: 0x000037A9
		public static string GetCompilableNiceFullName(this Type type)
		{
			return type.GetNiceFullName().Replace('<', '_').Replace('>', '_').TrimEnd(new char[]
			{
				'_'
			});
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000055D4 File Offset: 0x000037D4
		public static T GetCustomAttribute<T>(this Type type, bool inherit) where T : Attribute
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(T), inherit);
			if (customAttributes.Length == 0)
			{
				return default(T);
			}
			return customAttributes[0] as T;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000560E File Offset: 0x0000380E
		public static T GetCustomAttribute<T>(this Type type) where T : Attribute
		{
			return type.GetCustomAttribute(false);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005617 File Offset: 0x00003817
		public static IEnumerable<T> GetCustomAttributes<T>(this Type type) where T : Attribute
		{
			return type.GetCustomAttributes(false);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005620 File Offset: 0x00003820
		public static IEnumerable<T> GetCustomAttributes<T>(this Type type, bool inherit) where T : Attribute
		{
			object[] attrs = type.GetCustomAttributes(typeof(T), inherit);
			int num;
			for (int i = 0; i < attrs.Length; i = num + 1)
			{
				yield return attrs[i] as T;
				num = i;
			}
			yield break;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005637 File Offset: 0x00003837
		public static bool IsDefined<T>(this Type type) where T : Attribute
		{
			return type.IsDefined(typeof(T), false);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000564A File Offset: 0x0000384A
		public static bool IsDefined<T>(this Type type, bool inherit) where T : Attribute
		{
			return type.IsDefined(typeof(T), inherit);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000565D File Offset: 0x0000385D
		public static bool InheritsFrom<TBase>(this Type type)
		{
			return type.InheritsFrom(typeof(TBase));
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005670 File Offset: 0x00003870
		public static bool InheritsFrom(this Type type, Type baseType)
		{
			if (baseType.IsAssignableFrom(type))
			{
				return true;
			}
			if (type.IsInterface && !baseType.IsInterface)
			{
				return false;
			}
			if (baseType.IsInterface)
			{
				return type.GetInterfaces().Contains(baseType);
			}
			Type type2 = type;
			while (type2 != null)
			{
				if (type2 == baseType)
				{
					return true;
				}
				if (baseType.IsGenericTypeDefinition && type2.IsGenericType && type2.GetGenericTypeDefinition() == baseType)
				{
					return true;
				}
				type2 = type2.BaseType;
			}
			return false;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000056F0 File Offset: 0x000038F0
		public static int GetInheritanceDistance(this Type type, Type baseType)
		{
			if (type == baseType)
			{
				return 0;
			}
			Type type2;
			Type type3;
			if (type.IsAssignableFrom(baseType))
			{
				type2 = type;
				type3 = baseType;
			}
			else
			{
				if (!baseType.IsAssignableFrom(type))
				{
					throw new ArgumentException(string.Concat(new string[]
					{
						"Cannot assign types '",
						type.GetNiceName(),
						"' and '",
						baseType.GetNiceName(),
						"' to each other."
					}));
				}
				type2 = baseType;
				type3 = type;
			}
			Type type4 = type3;
			int num = 0;
			if (type2.IsInterface)
			{
				while (type4 != null)
				{
					if (!(type4 != typeof(object)))
					{
						break;
					}
					num++;
					type4 = type4.BaseType;
					Type[] interfaces = type4.GetInterfaces();
					for (int i = 0; i < interfaces.Length; i++)
					{
						if (interfaces[i] == type2)
						{
							type4 = null;
							break;
						}
					}
				}
			}
			else
			{
				while (type4 != type2 && type4 != null && type4 != typeof(object))
				{
					num++;
					type4 = type4.BaseType;
				}
			}
			return num;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000057F4 File Offset: 0x000039F4
		public static bool HasParamaters(this MethodInfo methodInfo, IList<Type> paramTypes, bool inherit = true)
		{
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (parameters.Length == paramTypes.Count)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					if (inherit && !paramTypes[i].InheritsFrom(parameters[i].ParameterType))
					{
						return false;
					}
					if (parameters[i].ParameterType != paramTypes[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005858 File Offset: 0x00003A58
		public static Type GetReturnType(this MemberInfo memberInfo)
		{
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return fieldInfo.FieldType;
			}
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.PropertyType;
			}
			MethodInfo methodInfo = memberInfo as MethodInfo;
			if (methodInfo != null)
			{
				return methodInfo.ReturnType;
			}
			EventInfo eventInfo = memberInfo as EventInfo;
			if (eventInfo != null)
			{
				return eventInfo.EventHandlerType;
			}
			return null;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000058C4 File Offset: 0x00003AC4
		public static object GetMemberValue(this MemberInfo member, object obj)
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

		// Token: 0x060000D4 RID: 212 RVA: 0x0000591C File Offset: 0x00003B1C
		public static void SetMemberValue(this MemberInfo member, object obj, object value)
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

		// Token: 0x060000D5 RID: 213 RVA: 0x000059A8 File Offset: 0x00003BA8
		public static bool TryInferGenericParameters(this Type genericTypeDefinition, out Type[] inferredParams, params Type[] knownParameters)
		{
			if (genericTypeDefinition == null)
			{
				throw new ArgumentNullException("genericTypeDefinition");
			}
			if (knownParameters == null)
			{
				throw new ArgumentNullException("knownParameters");
			}
			if (!genericTypeDefinition.IsGenericType)
			{
				throw new ArgumentException("The genericTypeDefinition parameter must be a generic type.");
			}
			object genericConstraintsSatisfaction_LOCK = TypeExtensions.GenericConstraintsSatisfaction_LOCK;
			bool result;
			lock (genericConstraintsSatisfaction_LOCK)
			{
				Dictionary<Type, Type> genericConstraintsSatisfactionInferredParameters = TypeExtensions.GenericConstraintsSatisfactionInferredParameters;
				genericConstraintsSatisfactionInferredParameters.Clear();
				HashSet<Type> genericConstraintsSatisfactionTypesToCheck = TypeExtensions.GenericConstraintsSatisfactionTypesToCheck;
				genericConstraintsSatisfactionTypesToCheck.Clear();
				List<Type> genericConstraintsSatisfactionTypesToCheck_ToAdd = TypeExtensions.GenericConstraintsSatisfactionTypesToCheck_ToAdd;
				genericConstraintsSatisfactionTypesToCheck_ToAdd.Clear();
				for (int i = 0; i < knownParameters.Length; i++)
				{
					genericConstraintsSatisfactionTypesToCheck.Add(knownParameters[i]);
				}
				Type[] genericArguments = genericTypeDefinition.GetGenericArguments();
				if (!genericTypeDefinition.IsGenericTypeDefinition)
				{
					Type[] array = genericArguments;
					genericTypeDefinition = genericTypeDefinition.GetGenericTypeDefinition();
					genericArguments = genericTypeDefinition.GetGenericArguments();
					int num = 0;
					for (int j = 0; j < array.Length; j++)
					{
						if (!array[j].IsGenericParameter && (!array[j].IsGenericType || array[j].IsFullyConstructedGenericType()))
						{
							genericConstraintsSatisfactionInferredParameters[genericArguments[j]] = array[j];
						}
						else
						{
							num++;
						}
					}
					if (num == knownParameters.Length)
					{
						int num2 = 0;
						for (int k = 0; k < array.Length; k++)
						{
							if (array[k].IsGenericParameter)
							{
								array[k] = knownParameters[num2++];
							}
						}
						if (genericTypeDefinition.AreGenericConstraintsSatisfiedBy(array))
						{
							inferredParams = array;
							return true;
						}
					}
				}
				if (genericArguments.Length == knownParameters.Length && genericTypeDefinition.AreGenericConstraintsSatisfiedBy(knownParameters))
				{
					inferredParams = knownParameters;
					result = true;
				}
				else
				{
					foreach (Type type in genericArguments)
					{
						Type[] genericParameterConstraints = type.GetGenericParameterConstraints();
						foreach (Type type2 in genericParameterConstraints)
						{
							foreach (Type type3 in genericConstraintsSatisfactionTypesToCheck)
							{
								if (type2.IsGenericType)
								{
									Type genericTypeDefinition2 = type2.GetGenericTypeDefinition();
									Type[] genericArguments2 = type2.GetGenericArguments();
									Type[] array4;
									if (type3.IsGenericType && genericTypeDefinition2 == type3.GetGenericTypeDefinition())
									{
										array4 = type3.GetGenericArguments();
									}
									else if (genericTypeDefinition2.IsInterface && type3.ImplementsOpenGenericInterface(genericTypeDefinition2))
									{
										array4 = type3.GetArgumentsOfInheritedOpenGenericInterface(genericTypeDefinition2);
									}
									else
									{
										if (!genericTypeDefinition2.IsClass || !type3.ImplementsOpenGenericClass(genericTypeDefinition2))
										{
											continue;
										}
										array4 = type3.GetArgumentsOfInheritedOpenGenericClass(genericTypeDefinition2);
									}
									genericConstraintsSatisfactionInferredParameters[type] = type3;
									genericConstraintsSatisfactionTypesToCheck_ToAdd.Add(type3);
									for (int n = 0; n < genericArguments2.Length; n++)
									{
										if (genericArguments2[n].IsGenericParameter)
										{
											genericConstraintsSatisfactionInferredParameters[genericArguments2[n]] = array4[n];
											genericConstraintsSatisfactionTypesToCheck_ToAdd.Add(array4[n]);
										}
									}
								}
							}
							foreach (Type item in genericConstraintsSatisfactionTypesToCheck_ToAdd)
							{
								genericConstraintsSatisfactionTypesToCheck.Add(item);
							}
							genericConstraintsSatisfactionTypesToCheck_ToAdd.Clear();
						}
					}
					if (genericConstraintsSatisfactionInferredParameters.Count == genericArguments.Length)
					{
						inferredParams = new Type[genericConstraintsSatisfactionInferredParameters.Count];
						for (int num3 = 0; num3 < genericArguments.Length; num3++)
						{
							inferredParams[num3] = genericConstraintsSatisfactionInferredParameters[genericArguments[num3]];
						}
						if (genericTypeDefinition.AreGenericConstraintsSatisfiedBy(inferredParams))
						{
							return true;
						}
					}
					inferredParams = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005D5C File Offset: 0x00003F5C
		public static bool AreGenericConstraintsSatisfiedBy(this Type genericType, params Type[] parameters)
		{
			if (genericType == null)
			{
				throw new ArgumentNullException("genericType");
			}
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			if (!genericType.IsGenericType)
			{
				throw new ArgumentException("The genericTypeDefinition parameter must be a generic type.");
			}
			return TypeExtensions.AreGenericConstraintsSatisfiedBy(genericType.GetGenericArguments(), parameters);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005DAC File Offset: 0x00003FAC
		public static bool AreGenericConstraintsSatisfiedBy(this MethodBase genericMethod, params Type[] parameters)
		{
			if (genericMethod == null)
			{
				throw new ArgumentNullException("genericMethod");
			}
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			if (!genericMethod.IsGenericMethod)
			{
				throw new ArgumentException("The genericMethod parameter must be a generic method.");
			}
			return TypeExtensions.AreGenericConstraintsSatisfiedBy(genericMethod.GetGenericArguments(), parameters);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005DFC File Offset: 0x00003FFC
		public static bool AreGenericConstraintsSatisfiedBy(Type[] definitions, Type[] parameters)
		{
			if (definitions.Length != parameters.Length)
			{
				return false;
			}
			object genericConstraintsSatisfaction_LOCK = TypeExtensions.GenericConstraintsSatisfaction_LOCK;
			bool result;
			lock (genericConstraintsSatisfaction_LOCK)
			{
				Dictionary<Type, Type> genericConstraintsSatisfactionResolvedMap = TypeExtensions.GenericConstraintsSatisfactionResolvedMap;
				genericConstraintsSatisfactionResolvedMap.Clear();
				for (int i = 0; i < definitions.Length; i++)
				{
					Type genericParameterDefinition = definitions[i];
					Type parameterType = parameters[i];
					if (!genericParameterDefinition.GenericParameterIsFulfilledBy(parameterType, genericConstraintsSatisfactionResolvedMap, null))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005E7C File Offset: 0x0000407C
		public static bool GenericParameterIsFulfilledBy(this Type genericParameterDefinition, Type parameterType)
		{
			object genericConstraintsSatisfaction_LOCK = TypeExtensions.GenericConstraintsSatisfaction_LOCK;
			bool result;
			lock (genericConstraintsSatisfaction_LOCK)
			{
				TypeExtensions.GenericConstraintsSatisfactionResolvedMap.Clear();
				result = genericParameterDefinition.GenericParameterIsFulfilledBy(parameterType, TypeExtensions.GenericConstraintsSatisfactionResolvedMap, null);
			}
			return result;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005ED0 File Offset: 0x000040D0
		private static bool GenericParameterIsFulfilledBy(this Type genericParameterDefinition, Type parameterType, Dictionary<Type, Type> resolvedMap, HashSet<Type> processedParams = null)
		{
			if (genericParameterDefinition == null)
			{
				throw new ArgumentNullException("genericParameterDefinition");
			}
			if (parameterType == null)
			{
				throw new ArgumentNullException("parameterType");
			}
			if (resolvedMap == null)
			{
				throw new ArgumentNullException("resolvedMap");
			}
			if (!genericParameterDefinition.IsGenericParameter && genericParameterDefinition == parameterType)
			{
				return true;
			}
			if (!genericParameterDefinition.IsGenericParameter)
			{
				return false;
			}
			if (processedParams == null)
			{
				processedParams = TypeExtensions.GenericConstraintsSatisfactionProcessedParams;
				processedParams.Clear();
			}
			processedParams.Add(genericParameterDefinition);
			GenericParameterAttributes genericParameterAttributes = genericParameterDefinition.GenericParameterAttributes;
			if (genericParameterAttributes != null)
			{
				if ((genericParameterAttributes & 8) == 8)
				{
					if (!parameterType.IsValueType || (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Nullable)))
					{
						return false;
					}
				}
				else if ((genericParameterAttributes & 4) == 4 && parameterType.IsValueType)
				{
					return false;
				}
				if ((genericParameterAttributes & 16) == 16 && (parameterType.IsAbstract || (!parameterType.IsValueType && parameterType.GetConstructor(Type.EmptyTypes) == null)))
				{
					return false;
				}
			}
			if (resolvedMap.ContainsKey(genericParameterDefinition) && !parameterType.IsAssignableFrom(resolvedMap[genericParameterDefinition]))
			{
				return false;
			}
			foreach (Type type in genericParameterDefinition.GetGenericParameterConstraints())
			{
				if (type.IsGenericParameter && resolvedMap.ContainsKey(type))
				{
					type = resolvedMap[type];
				}
				if (type.IsGenericParameter)
				{
					if (!type.GenericParameterIsFulfilledBy(parameterType, resolvedMap, processedParams))
					{
						return false;
					}
				}
				else
				{
					if (!type.IsClass && !type.IsInterface && !type.IsValueType)
					{
						throw new Exception("Unknown parameter constraint type! " + type.GetNiceName());
					}
					if (type.IsGenericType)
					{
						Type genericTypeDefinition = type.GetGenericTypeDefinition();
						Type[] genericArguments = type.GetGenericArguments();
						Type[] array;
						if (parameterType.IsGenericType && genericTypeDefinition == parameterType.GetGenericTypeDefinition())
						{
							array = parameterType.GetGenericArguments();
						}
						else if (genericTypeDefinition.IsClass)
						{
							if (!parameterType.ImplementsOpenGenericClass(genericTypeDefinition))
							{
								return false;
							}
							array = parameterType.GetArgumentsOfInheritedOpenGenericClass(genericTypeDefinition);
						}
						else
						{
							if (!parameterType.ImplementsOpenGenericInterface(genericTypeDefinition))
							{
								return false;
							}
							array = parameterType.GetArgumentsOfInheritedOpenGenericInterface(genericTypeDefinition);
						}
						for (int j = 0; j < genericArguments.Length; j++)
						{
							Type type2 = genericArguments[j];
							Type type3 = array[j];
							if (type2.IsGenericParameter && resolvedMap.ContainsKey(type2))
							{
								type2 = resolvedMap[type2];
							}
							if (type2.IsGenericParameter)
							{
								if (!processedParams.Contains(type2) && !type2.GenericParameterIsFulfilledBy(type3, resolvedMap, processedParams))
								{
									return false;
								}
							}
							else if (type2 != type3 && !type2.IsAssignableFrom(type3))
							{
								return false;
							}
						}
					}
					else if (!type.IsAssignableFrom(parameterType))
					{
						return false;
					}
				}
			}
			resolvedMap[genericParameterDefinition] = parameterType;
			return true;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000615C File Offset: 0x0000435C
		public static string GetGenericConstraintsString(this Type type, bool useFullTypeNames = false)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!type.IsGenericTypeDefinition)
			{
				throw new ArgumentException("Type '" + type.GetNiceName() + "' is not a generic type definition!");
			}
			Type[] genericArguments = type.GetGenericArguments();
			string[] array = new string[genericArguments.Length];
			for (int i = 0; i < genericArguments.Length; i++)
			{
				array[i] = genericArguments[i].GetGenericParameterConstraintsString(useFullTypeNames);
			}
			return string.Join(" ", array);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000061D8 File Offset: 0x000043D8
		public static string GetGenericParameterConstraintsString(this Type type, bool useFullTypeNames = false)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!type.IsGenericParameter)
			{
				throw new ArgumentException("Type '" + type.GetNiceName() + "' is not a generic parameter!");
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			GenericParameterAttributes genericParameterAttributes = type.GenericParameterAttributes;
			if ((genericParameterAttributes & 8) == 8)
			{
				stringBuilder.Append("where ").Append(type.Name).Append(" : struct");
				flag = true;
			}
			else if ((genericParameterAttributes & 4) == 4)
			{
				stringBuilder.Append("where ").Append(type.Name).Append(" : class");
				flag = true;
			}
			if ((genericParameterAttributes & 16) == 16)
			{
				if (flag)
				{
					stringBuilder.Append(", new()");
				}
				else
				{
					stringBuilder.Append("where ").Append(type.Name).Append(" : new()");
					flag = true;
				}
			}
			Type[] genericParameterConstraints = type.GetGenericParameterConstraints();
			if (genericParameterConstraints.Length != 0)
			{
				foreach (Type type2 in genericParameterConstraints)
				{
					if (flag)
					{
						stringBuilder.Append(", ");
						if (useFullTypeNames)
						{
							stringBuilder.Append(type2.GetNiceFullName());
						}
						else
						{
							stringBuilder.Append(type2.GetNiceName());
						}
					}
					else
					{
						stringBuilder.Append("where ").Append(type.Name).Append(" : ");
						if (useFullTypeNames)
						{
							stringBuilder.Append(type2.GetNiceFullName());
						}
						else
						{
							stringBuilder.Append(type2.GetNiceName());
						}
						flag = true;
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006364 File Offset: 0x00004564
		public static bool GenericArgumentsContainsTypes(this Type type, params Type[] types)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!type.IsGenericType)
			{
				return false;
			}
			bool[] array = new bool[types.Length];
			Type[] genericArguments = type.GetGenericArguments();
			Stack<Type> genericArgumentsContainsTypes_ArgsToCheckCached = TypeExtensions.GenericArgumentsContainsTypes_ArgsToCheckCached;
			Stack<Type> stack = genericArgumentsContainsTypes_ArgsToCheckCached;
			lock (stack)
			{
				genericArgumentsContainsTypes_ArgsToCheckCached.Clear();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					genericArgumentsContainsTypes_ArgsToCheckCached.Push(genericArguments[i]);
				}
				while (genericArgumentsContainsTypes_ArgsToCheckCached.Count > 0)
				{
					Type type2 = genericArgumentsContainsTypes_ArgsToCheckCached.Pop();
					for (int j = 0; j < types.Length; j++)
					{
						Type type3 = types[j];
						if (type3 == type2)
						{
							array[j] = true;
						}
						else if (type3.IsGenericTypeDefinition && type2.IsGenericType && !type2.IsGenericTypeDefinition && type2.GetGenericTypeDefinition() == type3)
						{
							array[j] = true;
						}
					}
					bool flag2 = true;
					for (int k = 0; k < array.Length; k++)
					{
						if (!array[k])
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						return true;
					}
					if (type2.IsGenericType)
					{
						foreach (Type type4 in type2.GetGenericArguments())
						{
							genericArgumentsContainsTypes_ArgsToCheckCached.Push(type4);
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000064C8 File Offset: 0x000046C8
		public static bool IsFullyConstructedGenericType(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type.IsGenericTypeDefinition)
			{
				return false;
			}
			if (type.HasElementType)
			{
				Type elementType = type.GetElementType();
				if (elementType.IsGenericParameter || !elementType.IsFullyConstructedGenericType())
				{
					return false;
				}
			}
			foreach (Type type2 in type.GetGenericArguments())
			{
				if (type2.IsGenericParameter)
				{
					return false;
				}
				if (!type2.IsFullyConstructedGenericType())
				{
					return false;
				}
			}
			return !type.IsGenericTypeDefinition;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000654A File Offset: 0x0000474A
		public static bool IsNullableType(this Type type)
		{
			return !type.IsPrimitive && !type.IsValueType && !type.IsEnum;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006568 File Offset: 0x00004768
		public static ulong GetEnumBitmask(object value, Type enumType)
		{
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("enumType");
			}
			ulong result;
			try
			{
				result = Convert.ToUInt64(value, CultureInfo.InvariantCulture);
			}
			catch (OverflowException)
			{
				result = (ulong)Convert.ToInt64(value, CultureInfo.InvariantCulture);
			}
			return result;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000065B8 File Offset: 0x000047B8
		public static bool IsCSharpKeyword(string identifier)
		{
			return TypeExtensions.ReservedCSharpKeywords.Contains(identifier);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000065C8 File Offset: 0x000047C8
		public static Type[] SafeGetTypes(this Assembly assembly)
		{
			Type[] result;
			try
			{
				result = assembly.GetTypes();
			}
			catch
			{
				result = Type.EmptyTypes;
			}
			return result;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000065F8 File Offset: 0x000047F8
		public static bool SafeIsDefined(this Assembly assembly, Type attribute, bool inherit)
		{
			bool result;
			try
			{
				result = assembly.IsDefined(attribute, inherit);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006628 File Offset: 0x00004828
		public static object[] SafeGetCustomAttributes(this Assembly assembly, Type type, bool inherit)
		{
			object[] result;
			try
			{
				result = assembly.GetCustomAttributes(type, inherit);
			}
			catch
			{
				result = new object[0];
			}
			return result;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000665C File Offset: 0x0000485C
		public static bool HasDefaultConstructor(this Type self)
		{
			return self == typeof(string) || self.IsArray || self.IsValueType || self.GetConstructor(Type.EmptyTypes) != null;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006694 File Offset: 0x00004894
		public static object InstantiateDefault(this Type type, bool preferUninitializedOverNonDefault)
		{
			if (type.IsInterface || type.IsAbstract)
			{
				string text = (type.BaseType != null) ? (type.GetNiceName() + " : " + type.BaseType.GetNiceName()) : type.GetNiceName();
				throw new ArgumentException("Invalid argument received in parameter 'type', can't construct Interfaces or Abstracts (" + text + ").");
			}
			if (type.IsArray)
			{
				Type elementType = type.GetElementType();
				if (elementType == null)
				{
					return null;
				}
				int arrayRank = type.GetArrayRank();
				switch (arrayRank)
				{
				case 0:
					return null;
				case 1:
					return Array.CreateInstance(elementType, 0);
				case 2:
					return Array.CreateInstance(elementType, 0, 0);
				case 3:
					return Array.CreateInstance(elementType, 0, 0, 0);
				default:
				{
					int[] array = new int[arrayRank];
					return Array.CreateInstance(elementType, array);
				}
				}
			}
			else
			{
				if (type == typeof(string))
				{
					return string.Empty;
				}
				if (type.HasDefaultConstructor())
				{
					return Activator.CreateInstance(type);
				}
				if (preferUninitializedOverNonDefault)
				{
					return FormatterServices.GetUninitializedObject(type);
				}
				ConstructorInfo constructorInfo = TypeExtensions.FindIdealConstructor(type, 0);
				if (constructorInfo != null)
				{
					ParameterInfo[] parameters = constructorInfo.GetParameters();
					object[] array2 = new object[parameters.Length];
					for (int i = 0; i < array2.Length; i++)
					{
						if (parameters[i].HasDefaultValue)
						{
							array2[i] = parameters[i].DefaultValue;
						}
						else
						{
							Type parameterType = parameters[i].ParameterType;
							array2[i] = parameterType.InstantiateDefault(false);
						}
					}
					return Activator.CreateInstance(type, array2);
				}
				if (!type.IsValueType)
				{
					return FormatterServices.GetUninitializedObject(type);
				}
				return Activator.CreateInstance(type);
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006818 File Offset: 0x00004A18
		public static ConstructorInfo FindIdealConstructor(Type type, BindingFlags flags = 0)
		{
			ConstructorInfo[] array = (flags == null) ? type.GetConstructors() : type.GetConstructors(flags);
			if (array.Length < 1)
			{
				return null;
			}
			List<ConstructorInfo> list = new List<ConstructorInfo>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].IsUnmanagedCtor() && !TypeExtensions.HasNonDefaultContractTypes(array[i]))
				{
					list.Add(array[i]);
				}
			}
			if (list.Count < 1)
			{
				return null;
			}
			ConstructorInfo constructorInfo = list[0];
			int num = TypeExtensions.GetCtorScore(constructorInfo);
			for (int j = 1; j < list.Count; j++)
			{
				ConstructorInfo constructorInfo2 = list[j];
				int ctorScore = TypeExtensions.GetCtorScore(constructorInfo2);
				if (ctorScore > num)
				{
					constructorInfo = constructorInfo2;
					num = ctorScore;
				}
			}
			return constructorInfo;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000068CC File Offset: 0x00004ACC
		public static bool IsUnmanagedCtor(this ConstructorInfo ctor)
		{
			ParameterInfo[] parameters = ctor.GetParameters();
			for (int i = 0; i < parameters.Length; i++)
			{
				Type parameterType = parameters[i].ParameterType;
				if (parameterType.IsPointer || parameterType.IsByRef || parameterType.IsMarshalByRef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006914 File Offset: 0x00004B14
		public static bool HasNonDefaultContractTypes(ConstructorInfo info)
		{
			ParameterInfo[] parameters = info.GetParameters();
			for (int i = 0; i < parameters.Length; i++)
			{
				Type parameterType = parameters[i].ParameterType;
				if (parameterType.IsAbstract && parameterType.IsInterface && !parameters[i].HasDefaultValue)
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000695C File Offset: 0x00004B5C
		public static int GetCtorScore(ConstructorInfo ctor)
		{
			int num = 4;
			int num2 = 0;
			ParameterInfo[] parameters = ctor.GetParameters();
			if (parameters.Length == 0)
			{
				return 50000;
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				Type parameterType = parameters[i].ParameterType;
				num += -3;
				bool isValueType = parameterType.IsValueType;
				num += (isValueType ? 5 : -60);
				if (parameters[i].HasDefaultValue)
				{
					num += (isValueType ? 30 : 50);
					num2++;
				}
			}
			if (num2 == parameters.Length)
			{
				num += 10000;
			}
			return num;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000069DC File Offset: 0x00004BDC
		// Note: this type is marked as 'beforefieldinit'.
		static TypeExtensions()
		{
			Dictionary<Type, string> dictionary = new Dictionary<Type, string>();
			dictionary.Add(typeof(float), "float");
			dictionary.Add(typeof(double), "double");
			dictionary.Add(typeof(sbyte), "sbyte");
			dictionary.Add(typeof(short), "short");
			dictionary.Add(typeof(int), "int");
			dictionary.Add(typeof(long), "long");
			dictionary.Add(typeof(byte), "byte");
			dictionary.Add(typeof(ushort), "ushort");
			dictionary.Add(typeof(uint), "uint");
			dictionary.Add(typeof(ulong), "ulong");
			dictionary.Add(typeof(decimal), "decimal");
			dictionary.Add(typeof(string), "string");
			dictionary.Add(typeof(char), "char");
			dictionary.Add(typeof(bool), "bool");
			dictionary.Add(typeof(float[]), "float[]");
			dictionary.Add(typeof(double[]), "double[]");
			dictionary.Add(typeof(sbyte[]), "sbyte[]");
			dictionary.Add(typeof(short[]), "short[]");
			dictionary.Add(typeof(int[]), "int[]");
			dictionary.Add(typeof(long[]), "long[]");
			dictionary.Add(typeof(byte[]), "byte[]");
			dictionary.Add(typeof(ushort[]), "ushort[]");
			dictionary.Add(typeof(uint[]), "uint[]");
			dictionary.Add(typeof(ulong[]), "ulong[]");
			dictionary.Add(typeof(decimal[]), "decimal[]");
			dictionary.Add(typeof(string[]), "string[]");
			dictionary.Add(typeof(char[]), "char[]");
			dictionary.Add(typeof(bool[]), "bool[]");
			TypeExtensions.TypeNameAlternatives = dictionary;
			TypeExtensions.CachedNiceNames_LOCK = new object();
			TypeExtensions.CachedNiceNames = new Dictionary<Type, string>();
			TypeExtensions.VoidPointerType = typeof(void).MakePointerType();
			Dictionary<Type, HashSet<Type>> dictionary2 = new Dictionary<Type, HashSet<Type>>();
			dictionary2.Add(typeof(long), new HashSet<Type>
			{
				typeof(float),
				typeof(double),
				typeof(decimal)
			});
			dictionary2.Add(typeof(int), new HashSet<Type>
			{
				typeof(long),
				typeof(float),
				typeof(double),
				typeof(decimal)
			});
			dictionary2.Add(typeof(short), new HashSet<Type>
			{
				typeof(int),
				typeof(long),
				typeof(float),
				typeof(double),
				typeof(decimal)
			});
			dictionary2.Add(typeof(sbyte), new HashSet<Type>
			{
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(float),
				typeof(double),
				typeof(decimal)
			});
			dictionary2.Add(typeof(ulong), new HashSet<Type>
			{
				typeof(float),
				typeof(double),
				typeof(decimal)
			});
			dictionary2.Add(typeof(uint), new HashSet<Type>
			{
				typeof(long),
				typeof(ulong),
				typeof(float),
				typeof(double),
				typeof(decimal)
			});
			dictionary2.Add(typeof(ushort), new HashSet<Type>
			{
				typeof(int),
				typeof(uint),
				typeof(long),
				typeof(ulong),
				typeof(float),
				typeof(double),
				typeof(decimal)
			});
			dictionary2.Add(typeof(byte), new HashSet<Type>
			{
				typeof(short),
				typeof(ushort),
				typeof(int),
				typeof(uint),
				typeof(long),
				typeof(ulong),
				typeof(float),
				typeof(double),
				typeof(decimal)
			});
			dictionary2.Add(typeof(char), new HashSet<Type>
			{
				typeof(ushort),
				typeof(int),
				typeof(uint),
				typeof(long),
				typeof(ulong),
				typeof(float),
				typeof(double),
				typeof(decimal)
			});
			dictionary2.Add(typeof(bool), new HashSet<Type>());
			dictionary2.Add(typeof(decimal), new HashSet<Type>());
			dictionary2.Add(typeof(float), new HashSet<Type>
			{
				typeof(double)
			});
			dictionary2.Add(typeof(double), new HashSet<Type>());
			dictionary2.Add(typeof(IntPtr), new HashSet<Type>());
			dictionary2.Add(typeof(UIntPtr), new HashSet<Type>());
			dictionary2.Add(TypeExtensions.VoidPointerType, new HashSet<Type>());
			TypeExtensions.PrimitiveImplicitCasts = dictionary2;
			TypeExtensions.ExplicitCastIntegrals = new HashSet<Type>
			{
				typeof(long),
				typeof(int),
				typeof(short),
				typeof(sbyte),
				typeof(ulong),
				typeof(uint),
				typeof(ushort),
				typeof(byte),
				typeof(char),
				typeof(decimal),
				typeof(float),
				typeof(double),
				typeof(IntPtr),
				typeof(UIntPtr)
			};
		}

		// Token: 0x0400001C RID: 28
		private static readonly Func<float, float, bool> FloatEqualityComparerFunc = new Func<float, float, bool>(TypeExtensions.FloatEqualityComparer);

		// Token: 0x0400001D RID: 29
		private static readonly Func<double, double, bool> DoubleEqualityComparerFunc = new Func<double, double, bool>(TypeExtensions.DoubleEqualityComparer);

		// Token: 0x0400001E RID: 30
		private static readonly Func<Quaternion, Quaternion, bool> QuaternionEqualityComparerFunc = new Func<Quaternion, Quaternion, bool>(TypeExtensions.QuaternionEqualityComparer);

		// Token: 0x0400001F RID: 31
		private static readonly object GenericConstraintsSatisfaction_LOCK = new object();

		// Token: 0x04000020 RID: 32
		private static readonly Dictionary<Type, Type> GenericConstraintsSatisfactionInferredParameters = new Dictionary<Type, Type>();

		// Token: 0x04000021 RID: 33
		private static readonly Dictionary<Type, Type> GenericConstraintsSatisfactionResolvedMap = new Dictionary<Type, Type>();

		// Token: 0x04000022 RID: 34
		private static readonly HashSet<Type> GenericConstraintsSatisfactionProcessedParams = new HashSet<Type>();

		// Token: 0x04000023 RID: 35
		private static readonly HashSet<Type> GenericConstraintsSatisfactionTypesToCheck = new HashSet<Type>();

		// Token: 0x04000024 RID: 36
		private static readonly List<Type> GenericConstraintsSatisfactionTypesToCheck_ToAdd = new List<Type>();

		// Token: 0x04000025 RID: 37
		private static readonly Type GenericListInterface = typeof(IList);

		// Token: 0x04000026 RID: 38
		private static readonly Type GenericCollectionInterface = typeof(ICollection);

		// Token: 0x04000027 RID: 39
		private static readonly object WeaklyTypedTypeCastDelegates_LOCK = new object();

		// Token: 0x04000028 RID: 40
		private static readonly object StronglyTypedTypeCastDelegates_LOCK = new object();

		// Token: 0x04000029 RID: 41
		private static readonly DoubleLookupDictionary<Type, Type, Func<object, object>> WeaklyTypedTypeCastDelegates = new DoubleLookupDictionary<Type, Type, Func<object, object>>();

		// Token: 0x0400002A RID: 42
		private static readonly DoubleLookupDictionary<Type, Type, Delegate> StronglyTypedTypeCastDelegates = new DoubleLookupDictionary<Type, Type, Delegate>();

		// Token: 0x0400002B RID: 43
		private static readonly Type[] TwoLengthTypeArray_Cached = new Type[2];

		// Token: 0x0400002C RID: 44
		private static readonly Stack<Type> GenericArgumentsContainsTypes_ArgsToCheckCached = new Stack<Type>();

		// Token: 0x0400002D RID: 45
		private static HashSet<string> ReservedCSharpKeywords = new HashSet<string>
		{
			"abstract",
			"as",
			"base",
			"bool",
			"break",
			"byte",
			"case",
			"catch",
			"char",
			"checked",
			"class",
			"const",
			"continue",
			"decimal",
			"default",
			"delegate",
			"do",
			"double",
			"else",
			"enum",
			"event",
			"explicit",
			"extern",
			"false",
			"finally",
			"fixed",
			"float",
			"for",
			"foreach",
			"goto",
			"if",
			"implicit",
			"in",
			"int",
			"interface",
			"internal",
			"is",
			"lock",
			"long",
			"namespace",
			"new",
			"null",
			"object",
			"operator",
			"out",
			"override",
			"params",
			"private",
			"protected",
			"public",
			"readonly",
			"ref",
			"return",
			"sbyte",
			"sealed",
			"short",
			"sizeof",
			"stackalloc",
			"static",
			"string",
			"struct",
			"switch",
			"this",
			"throw",
			"true",
			"try",
			"typeof",
			"uint",
			"ulong",
			"unchecked",
			"unsafe",
			"ushort",
			"using",
			"static",
			"void",
			"volatile",
			"while",
			"in",
			"get",
			"set",
			"var"
		};

		// Token: 0x0400002E RID: 46
		public static readonly Dictionary<Type, string> TypeNameAlternatives;

		// Token: 0x0400002F RID: 47
		private static readonly object CachedNiceNames_LOCK;

		// Token: 0x04000030 RID: 48
		private static readonly Dictionary<Type, string> CachedNiceNames;

		// Token: 0x04000031 RID: 49
		private static readonly Type VoidPointerType;

		// Token: 0x04000032 RID: 50
		private static readonly Dictionary<Type, HashSet<Type>> PrimitiveImplicitCasts;

		// Token: 0x04000033 RID: 51
		private static readonly HashSet<Type> ExplicitCastIntegrals;
	}
}
