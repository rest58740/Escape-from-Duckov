using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000BF RID: 191
	internal static class TypeExtensions
	{
		// Token: 0x0600052F RID: 1327 RVA: 0x000241BC File Offset: 0x000223BC
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

		// Token: 0x06000530 RID: 1328 RVA: 0x00024218 File Offset: 0x00022418
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
				return type.TypeNameGauntlet();
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

		// Token: 0x06000531 RID: 1329 RVA: 0x00024344 File Offset: 0x00022544
		internal static bool HasCastDefined(this Type from, Type to, bool requireImplicitCast)
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

		// Token: 0x06000532 RID: 1330 RVA: 0x00024458 File Offset: 0x00022658
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

		// Token: 0x06000533 RID: 1331 RVA: 0x000244EE File Offset: 0x000226EE
		private static bool IsValidIdentifierStartCharacter(char c)
		{
			return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_' || c == '@' || char.IsLetter(c);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00024516 File Offset: 0x00022716
		private static bool IsValidIdentifierPartCharacter(char c)
		{
			return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_' || (c >= '0' && c <= '9') || char.IsLetter(c);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00024544 File Offset: 0x00022744
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

		// Token: 0x06000536 RID: 1334 RVA: 0x00024598 File Offset: 0x00022798
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

		// Token: 0x06000537 RID: 1335 RVA: 0x00024620 File Offset: 0x00022820
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

		// Token: 0x06000538 RID: 1336 RVA: 0x000246D8 File Offset: 0x000228D8
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

		// Token: 0x06000539 RID: 1337 RVA: 0x0002480C File Offset: 0x00022A0C
		private static bool FloatEqualityComparer(float a, float b)
		{
			return (float.IsNaN(a) && float.IsNaN(b)) || a == b;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00024824 File Offset: 0x00022A24
		private static bool DoubleEqualityComparer(double a, double b)
		{
			return (double.IsNaN(a) && double.IsNaN(b)) || a == b;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0002483C File Offset: 0x00022A3C
		private static bool QuaternionEqualityComparer(Quaternion a, Quaternion b)
		{
			return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00024878 File Offset: 0x00022A78
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

		// Token: 0x0600053D RID: 1341 RVA: 0x000249DC File Offset: 0x00022BDC
		public static T GetAttribute<T>(this Type type, bool inherit) where T : Attribute
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(T), inherit);
			if (customAttributes.Length == 0)
			{
				return default(T);
			}
			return (T)((object)customAttributes[0]);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00024A11 File Offset: 0x00022C11
		public static bool ImplementsOrInherits(this Type type, Type to)
		{
			return to.IsAssignableFrom(type);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00024A1A File Offset: 0x00022C1A
		public static bool ImplementsOpenGenericType(this Type candidateType, Type openGenericType)
		{
			if (openGenericType.IsInterface)
			{
				return candidateType.ImplementsOpenGenericInterface(openGenericType);
			}
			return candidateType.ImplementsOpenGenericClass(openGenericType);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00024A34 File Offset: 0x00022C34
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

		// Token: 0x06000541 RID: 1345 RVA: 0x00024A88 File Offset: 0x00022C88
		public static bool ImplementsOpenGenericClass(this Type candidateType, Type openGenericType)
		{
			if (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == openGenericType)
			{
				return true;
			}
			Type baseType = candidateType.BaseType;
			return baseType != null && baseType.ImplementsOpenGenericClass(openGenericType);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00024AC9 File Offset: 0x00022CC9
		public static Type[] GetArgumentsOfInheritedOpenGenericType(this Type candidateType, Type openGenericType)
		{
			if (openGenericType.IsInterface)
			{
				return candidateType.GetArgumentsOfInheritedOpenGenericInterface(openGenericType);
			}
			return candidateType.GetArgumentsOfInheritedOpenGenericClass(openGenericType);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00024AE4 File Offset: 0x00022CE4
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

		// Token: 0x06000544 RID: 1348 RVA: 0x00024B28 File Offset: 0x00022D28
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

		// Token: 0x06000545 RID: 1349 RVA: 0x00024BC4 File Offset: 0x00022DC4
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

		// Token: 0x06000546 RID: 1350 RVA: 0x00024DDC File Offset: 0x00022FDC
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

		// Token: 0x06000547 RID: 1351 RVA: 0x00024F70 File Offset: 0x00023170
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

		// Token: 0x06000548 RID: 1352 RVA: 0x00025106 File Offset: 0x00023306
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

		// Token: 0x06000549 RID: 1353 RVA: 0x0002511D File Offset: 0x0002331D
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

		// Token: 0x0600054A RID: 1354 RVA: 0x0002513B File Offset: 0x0002333B
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

		// Token: 0x0600054B RID: 1355 RVA: 0x00025154 File Offset: 0x00023354
		public static Type GetGenericBaseType(this Type type, Type baseType)
		{
			int num;
			return type.GetGenericBaseType(baseType, out num);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0002516C File Offset: 0x0002336C
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

		// Token: 0x0600054D RID: 1357 RVA: 0x00025268 File Offset: 0x00023468
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

		// Token: 0x0600054E RID: 1358 RVA: 0x000252A5 File Offset: 0x000234A5
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

		// Token: 0x0600054F RID: 1359 RVA: 0x000252BC File Offset: 0x000234BC
		private static string TypeNameGauntlet(this Type type)
		{
			string text = type.Name;
			string empty = string.Empty;
			if (TypeExtensions.TypeNameAlternatives.TryGetValue(text, ref empty))
			{
				text = empty;
			}
			return text;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x000252E8 File Offset: 0x000234E8
		public static string GetNiceName(this Type type)
		{
			if (type.IsNested && !type.IsGenericParameter)
			{
				return type.DeclaringType.GetNiceName() + "." + TypeExtensions.GetCachedNiceName(type);
			}
			return TypeExtensions.GetCachedNiceName(type);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0002531C File Offset: 0x0002351C
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

		// Token: 0x06000552 RID: 1362 RVA: 0x00025377 File Offset: 0x00023577
		public static string GetCompilableNiceName(this Type type)
		{
			return type.GetNiceName().Replace('<', '_').Replace('>', '_').TrimEnd(new char[]
			{
				'_'
			});
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x000253A1 File Offset: 0x000235A1
		public static string GetCompilableNiceFullName(this Type type)
		{
			return type.GetNiceFullName().Replace('<', '_').Replace('>', '_').TrimEnd(new char[]
			{
				'_'
			});
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000253CC File Offset: 0x000235CC
		public static T GetCustomAttribute<T>(this Type type, bool inherit) where T : Attribute
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(T), inherit);
			if (customAttributes.Length == 0)
			{
				return default(T);
			}
			return customAttributes[0] as T;
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00025406 File Offset: 0x00023606
		public static T GetCustomAttribute<T>(this Type type) where T : Attribute
		{
			return type.GetCustomAttribute(false);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0002540F File Offset: 0x0002360F
		public static IEnumerable<T> GetCustomAttributes<T>(this Type type) where T : Attribute
		{
			return type.GetCustomAttributes(false);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00025418 File Offset: 0x00023618
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

		// Token: 0x06000558 RID: 1368 RVA: 0x0002542F File Offset: 0x0002362F
		public static bool IsDefined<T>(this Type type) where T : Attribute
		{
			return type.IsDefined(typeof(T), false);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00025442 File Offset: 0x00023642
		public static bool IsDefined<T>(this Type type, bool inherit) where T : Attribute
		{
			return type.IsDefined(typeof(T), inherit);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00025455 File Offset: 0x00023655
		public static bool InheritsFrom<TBase>(this Type type)
		{
			return type.InheritsFrom(typeof(TBase));
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00025468 File Offset: 0x00023668
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

		// Token: 0x0600055C RID: 1372 RVA: 0x000254E8 File Offset: 0x000236E8
		public static int GetInheritanceDistance(this Type type, Type baseType)
		{
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

		// Token: 0x0600055D RID: 1373 RVA: 0x000255E0 File Offset: 0x000237E0
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

		// Token: 0x0600055E RID: 1374 RVA: 0x00025644 File Offset: 0x00023844
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

		// Token: 0x0600055F RID: 1375 RVA: 0x000256B0 File Offset: 0x000238B0
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

		// Token: 0x06000560 RID: 1376 RVA: 0x00025708 File Offset: 0x00023908
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

		// Token: 0x06000561 RID: 1377 RVA: 0x00025794 File Offset: 0x00023994
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

		// Token: 0x06000562 RID: 1378 RVA: 0x00025B48 File Offset: 0x00023D48
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

		// Token: 0x06000563 RID: 1379 RVA: 0x00025B98 File Offset: 0x00023D98
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

		// Token: 0x06000564 RID: 1380 RVA: 0x00025BE8 File Offset: 0x00023DE8
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

		// Token: 0x06000565 RID: 1381 RVA: 0x00025C68 File Offset: 0x00023E68
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

		// Token: 0x06000566 RID: 1382 RVA: 0x00025CBC File Offset: 0x00023EBC
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

		// Token: 0x06000567 RID: 1383 RVA: 0x00025F48 File Offset: 0x00024148
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

		// Token: 0x06000568 RID: 1384 RVA: 0x00025FC4 File Offset: 0x000241C4
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

		// Token: 0x06000569 RID: 1385 RVA: 0x00026150 File Offset: 0x00024350
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

		// Token: 0x0600056A RID: 1386 RVA: 0x000262B4 File Offset: 0x000244B4
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

		// Token: 0x0600056B RID: 1387 RVA: 0x00026336 File Offset: 0x00024536
		public static bool IsNullableType(this Type type)
		{
			return !type.IsPrimitive && !type.IsValueType && !type.IsEnum;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00026354 File Offset: 0x00024554
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

		// Token: 0x0600056D RID: 1389 RVA: 0x000263A4 File Offset: 0x000245A4
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

		// Token: 0x0600056E RID: 1390 RVA: 0x000263D4 File Offset: 0x000245D4
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

		// Token: 0x0600056F RID: 1391 RVA: 0x00026404 File Offset: 0x00024604
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

		// Token: 0x06000570 RID: 1392 RVA: 0x00026438 File Offset: 0x00024638
		// Note: this type is marked as 'beforefieldinit'.
		static TypeExtensions()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Single", "float");
			dictionary.Add("Double", "double");
			dictionary.Add("SByte", "sbyte");
			dictionary.Add("Int16", "short");
			dictionary.Add("Int32", "int");
			dictionary.Add("Int64", "long");
			dictionary.Add("Byte", "byte");
			dictionary.Add("UInt16", "ushort");
			dictionary.Add("UInt32", "uint");
			dictionary.Add("UInt64", "ulong");
			dictionary.Add("Decimal", "decimal");
			dictionary.Add("String", "string");
			dictionary.Add("Char", "char");
			dictionary.Add("Boolean", "bool");
			dictionary.Add("Single[]", "float[]");
			dictionary.Add("Double[]", "double[]");
			dictionary.Add("SByte[]", "sbyte[]");
			dictionary.Add("Int16[]", "short[]");
			dictionary.Add("Int32[]", "int[]");
			dictionary.Add("Int64[]", "long[]");
			dictionary.Add("Byte[]", "byte[]");
			dictionary.Add("UInt16[]", "ushort[]");
			dictionary.Add("UInt32[]", "uint[]");
			dictionary.Add("UInt64[]", "ulong[]");
			dictionary.Add("Decimal[]", "decimal[]");
			dictionary.Add("String[]", "string[]");
			dictionary.Add("Char[]", "char[]");
			dictionary.Add("Boolean[]", "bool[]");
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

		// Token: 0x040001EE RID: 494
		private static readonly Func<float, float, bool> FloatEqualityComparerFunc = new Func<float, float, bool>(TypeExtensions.FloatEqualityComparer);

		// Token: 0x040001EF RID: 495
		private static readonly Func<double, double, bool> DoubleEqualityComparerFunc = new Func<double, double, bool>(TypeExtensions.DoubleEqualityComparer);

		// Token: 0x040001F0 RID: 496
		private static readonly Func<Quaternion, Quaternion, bool> QuaternionEqualityComparerFunc = new Func<Quaternion, Quaternion, bool>(TypeExtensions.QuaternionEqualityComparer);

		// Token: 0x040001F1 RID: 497
		private static readonly object GenericConstraintsSatisfaction_LOCK = new object();

		// Token: 0x040001F2 RID: 498
		private static readonly Dictionary<Type, Type> GenericConstraintsSatisfactionInferredParameters = new Dictionary<Type, Type>();

		// Token: 0x040001F3 RID: 499
		private static readonly Dictionary<Type, Type> GenericConstraintsSatisfactionResolvedMap = new Dictionary<Type, Type>();

		// Token: 0x040001F4 RID: 500
		private static readonly HashSet<Type> GenericConstraintsSatisfactionProcessedParams = new HashSet<Type>();

		// Token: 0x040001F5 RID: 501
		private static readonly HashSet<Type> GenericConstraintsSatisfactionTypesToCheck = new HashSet<Type>();

		// Token: 0x040001F6 RID: 502
		private static readonly List<Type> GenericConstraintsSatisfactionTypesToCheck_ToAdd = new List<Type>();

		// Token: 0x040001F7 RID: 503
		private static readonly Type GenericListInterface = typeof(IList);

		// Token: 0x040001F8 RID: 504
		private static readonly Type GenericCollectionInterface = typeof(ICollection);

		// Token: 0x040001F9 RID: 505
		private static readonly object WeaklyTypedTypeCastDelegates_LOCK = new object();

		// Token: 0x040001FA RID: 506
		private static readonly object StronglyTypedTypeCastDelegates_LOCK = new object();

		// Token: 0x040001FB RID: 507
		private static readonly DoubleLookupDictionary<Type, Type, Func<object, object>> WeaklyTypedTypeCastDelegates = new DoubleLookupDictionary<Type, Type, Func<object, object>>();

		// Token: 0x040001FC RID: 508
		private static readonly DoubleLookupDictionary<Type, Type, Delegate> StronglyTypedTypeCastDelegates = new DoubleLookupDictionary<Type, Type, Delegate>();

		// Token: 0x040001FD RID: 509
		private static readonly Type[] TwoLengthTypeArray_Cached = new Type[2];

		// Token: 0x040001FE RID: 510
		private static readonly Stack<Type> GenericArgumentsContainsTypes_ArgsToCheckCached = new Stack<Type>();

		// Token: 0x040001FF RID: 511
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

		// Token: 0x04000200 RID: 512
		public static readonly Dictionary<string, string> TypeNameAlternatives;

		// Token: 0x04000201 RID: 513
		private static readonly object CachedNiceNames_LOCK;

		// Token: 0x04000202 RID: 514
		private static readonly Dictionary<Type, string> CachedNiceNames;

		// Token: 0x04000203 RID: 515
		private static readonly Type VoidPointerType;

		// Token: 0x04000204 RID: 516
		private static readonly Dictionary<Type, HashSet<Type>> PrimitiveImplicitCasts;

		// Token: 0x04000205 RID: 517
		private static readonly HashSet<Type> ExplicitCastIntegrals;
	}
}
