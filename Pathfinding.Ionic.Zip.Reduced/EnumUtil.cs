using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Pathfinding.Ionic
{
	// Token: 0x02000022 RID: 34
	internal sealed class EnumUtil
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00003CF4 File Offset: 0x00001EF4
		private EnumUtil()
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003CFC File Offset: 0x00001EFC
		internal static string GetDescription(Enum value)
		{
			FieldInfo field = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] array = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (array.Length > 0)
			{
				return array[0].Description;
			}
			return value.ToString();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003D4C File Offset: 0x00001F4C
		internal static object Parse(Type enumType, string stringRepresentation)
		{
			return EnumUtil.Parse(enumType, stringRepresentation, false);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003D58 File Offset: 0x00001F58
		public static Enum[] GetEnumValues(Type type)
		{
			if (!type.IsEnum)
			{
				throw new ArgumentException("not an enum");
			}
			List<Enum> list = new List<Enum>();
			foreach (FieldInfo fieldInfo in type.GetFields(24))
			{
				if (fieldInfo.IsLiteral)
				{
					list.Add((Enum)fieldInfo.GetValue(null));
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003DC8 File Offset: 0x00001FC8
		public static string[] GetEnumStrings<T>()
		{
			Type typeFromHandle = typeof(T);
			if (!typeFromHandle.IsEnum)
			{
				throw new ArgumentException("not an enum");
			}
			List<string> list = new List<string>();
			foreach (FieldInfo fieldInfo in typeFromHandle.GetFields(24))
			{
				if (fieldInfo.IsLiteral)
				{
					list.Add(fieldInfo.Name);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003E40 File Offset: 0x00002040
		internal static object Parse(Type enumType, string stringRepresentation, bool ignoreCase)
		{
			if (ignoreCase)
			{
				stringRepresentation = stringRepresentation.ToLower();
			}
			foreach (Enum @enum in EnumUtil.GetEnumValues(enumType))
			{
				string text = EnumUtil.GetDescription(@enum);
				if (ignoreCase)
				{
					text = text.ToLower();
				}
				if (text == stringRepresentation)
				{
					return @enum;
				}
			}
			return Enum.Parse(enumType, stringRepresentation, ignoreCase);
		}
	}
}
