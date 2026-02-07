using System;
using System.Linq;
using System.Reflection;

namespace FlexFramework.Excel
{
	// Token: 0x02000010 RID: 16
	public static class Validator
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00003010 File Offset: 0x00001210
		public static bool CanCast(Type from, Type to)
		{
			if (from.IsAssignableFrom(to))
			{
				return true;
			}
			if (Validator.HasImplicitConversion(from, to) || Validator.HasImplicitConversion(from, from, to) || Validator.HasImplicitConversion(to, from, to))
			{
				return true;
			}
			if (to.IsEnum)
			{
				return Validator.CanCast(from, Enum.GetUnderlyingType(to));
			}
			return Nullable.GetUnderlyingType(to) != null && Validator.CanCast(from, Nullable.GetUnderlyingType(to));
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003078 File Offset: 0x00001278
		private static bool HasImplicitConversion(Type definedOn, Type baseType, Type targetType)
		{
			return (from m in definedOn.GetMethods(BindingFlags.Static | BindingFlags.Public)
			where m.Name == "op_Implicit" && m.ReturnType == targetType
			select m).Any(delegate(MethodInfo m)
			{
				ParameterInfo parameterInfo = m.GetParameters().FirstOrDefault<ParameterInfo>();
				return parameterInfo != null && parameterInfo.ParameterType == baseType;
			});
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000030C4 File Offset: 0x000012C4
		public static bool HasImplicitConversion(Type source, Type target)
		{
			TypeCode typeCode = Type.GetTypeCode(source);
			TypeCode typeCode2 = Type.GetTypeCode(target);
			switch (typeCode)
			{
			case TypeCode.Char:
				return typeCode2 - TypeCode.UInt16 <= 7;
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				}
				return false;
			case TypeCode.Byte:
				return typeCode2 - TypeCode.Int16 <= 8;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				}
				return false;
			case TypeCode.UInt16:
				return typeCode2 - TypeCode.Int32 <= 6;
			case TypeCode.Int32:
				return typeCode2 == TypeCode.Int64 || typeCode2 - TypeCode.Single <= 2;
			case TypeCode.UInt32:
				return typeCode2 == TypeCode.UInt32 || typeCode2 - TypeCode.UInt64 <= 3;
			case TypeCode.Int64:
			case TypeCode.UInt64:
				return typeCode2 - TypeCode.Single <= 2;
			case TypeCode.Single:
				return typeCode2 == TypeCode.Double;
			default:
				return false;
			}
		}
	}
}
