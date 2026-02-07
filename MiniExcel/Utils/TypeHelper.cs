using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MiniExcelLibs.Exceptions;

namespace MiniExcelLibs.Utils
{
	// Token: 0x0200003B RID: 59
	internal static class TypeHelper
	{
		// Token: 0x06000180 RID: 384 RVA: 0x0000673D File Offset: 0x0000493D
		public static IEnumerable<IDictionary<string, object>> ConvertToEnumerableDictionary(IDataReader reader)
		{
			while (reader.Read())
			{
				yield return Enumerable.Range(0, reader.FieldCount).ToDictionary((int i) => reader.GetName(i), (int i) => reader.GetValue(i));
			}
			yield break;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006750 File Offset: 0x00004950
		public static IEnumerable<Type> GetGenericIEnumerables(object o)
		{
			return from t in o.GetType().GetInterfaces()
			where t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)
			select t.GetGenericArguments()[0];
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000067B0 File Offset: 0x000049B0
		public static bool IsNumericType(Type type, bool isNullableUnderlyingType = false)
		{
			if (isNullableUnderlyingType)
			{
				type = (Nullable.GetUnderlyingType(type) ?? type);
			}
			TypeCode typeCode = Type.GetTypeCode(type);
			return typeCode - TypeCode.Int16 <= 8;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000067E0 File Offset: 0x000049E0
		public static object TypeMapping<T>(T v, ExcelColumnInfo pInfo, object newValue, object itemValue, int rowIndex, string startCell, Configuration _config) where T : class, new()
		{
			object result;
			try
			{
				result = TypeHelper.TypeMappingImpl<T>(v, pInfo, ref newValue, itemValue, _config);
			}
			catch (Exception ex) when (ex is InvalidCastException || ex is FormatException)
			{
				string text = pInfo.ExcelColumnName ?? pInfo.Property.Name;
				int num = ReferenceHelper.ConvertCellToXY(startCell).Item2 + rowIndex + 1;
				throw new ExcelInvalidCastException(text, num, itemValue, pInfo.Property.Info.PropertyType, string.Format("ColumnName : {0}, CellRow : {1}, Value : {2}, it can't cast to {3} type.", new object[]
				{
					text,
					num,
					itemValue,
					pInfo.Property.Info.PropertyType.Name
				}));
			}
			return result;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000068B4 File Offset: 0x00004AB4
		private static object TypeMappingImpl<T>(T v, ExcelColumnInfo pInfo, ref object newValue, object itemValue, Configuration _config) where T : class, new()
		{
			if (pInfo.Nullable)
			{
				object itemValue2 = itemValue;
				if (string.IsNullOrWhiteSpace((itemValue2 != null) ? itemValue2.ToString() : null))
				{
					newValue = null;
					goto IL_37C;
				}
			}
			if (pInfo.ExcludeNullableType == typeof(Guid))
			{
				newValue = Guid.Parse(itemValue.ToString());
			}
			else if (pInfo.ExcludeNullableType == typeof(DateTimeOffset))
			{
				object itemValue3 = itemValue;
				string text = (itemValue3 != null) ? itemValue3.ToString() : null;
				if (pInfo.ExcelFormat != null)
				{
					DateTimeOffset dateTimeOffset;
					if (DateTimeOffset.TryParseExact(text, pInfo.ExcelFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeOffset))
					{
						newValue = dateTimeOffset;
					}
				}
				else
				{
					DateTimeOffset dateTimeOffset2;
					if (!DateTimeOffset.TryParse(text, _config.Culture, DateTimeStyles.None, out dateTimeOffset2))
					{
						throw new InvalidCastException(text + " can't cast to datetime");
					}
					newValue = dateTimeOffset2;
				}
			}
			else if (pInfo.ExcludeNullableType == typeof(DateTime))
			{
				if (itemValue is DateTime || itemValue is DateTime?)
				{
					newValue = itemValue;
					pInfo.Property.SetValue(v, newValue);
					return newValue;
				}
				object itemValue4 = itemValue;
				string text2 = (itemValue4 != null) ? itemValue4.ToString() : null;
				DateTime dateTime2;
				DateTime dateTime3;
				if (pInfo.ExcelFormat != null)
				{
					DateTimeOffset dateTimeOffset3;
					DateTime dateTime;
					if (pInfo.Property.Info.PropertyType == typeof(DateTimeOffset) && DateTimeOffset.TryParseExact(text2, pInfo.ExcelFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeOffset3))
					{
						newValue = dateTimeOffset3;
					}
					else if (DateTime.TryParseExact(text2, pInfo.ExcelFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
					{
						newValue = dateTime;
					}
				}
				else if (DateTime.TryParse(text2, _config.Culture, DateTimeStyles.None, out dateTime2))
				{
					newValue = dateTime2;
				}
				else if (DateTime.TryParseExact(text2, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime3))
				{
					newValue = dateTime3;
				}
				else
				{
					double d;
					if (!double.TryParse(text2, NumberStyles.None, CultureInfo.InvariantCulture, out d))
					{
						throw new InvalidCastException(text2 + " can't cast to datetime");
					}
					newValue = DateTimeHelper.FromOADate(d);
				}
			}
			else if (pInfo.ExcludeNullableType == typeof(bool))
			{
				string text3 = itemValue.ToString();
				if (text3 == "1")
				{
					newValue = true;
				}
				else if (text3 == "0")
				{
					newValue = false;
				}
				else
				{
					newValue = bool.Parse(text3);
				}
			}
			else if (pInfo.Property.Info.PropertyType == typeof(string))
			{
				object itemValue5 = itemValue;
				newValue = XmlEncoder.DecodeString((itemValue5 != null) ? itemValue5.ToString() : null);
			}
			else if (pInfo.ExcludeNullableType.IsEnum)
			{
				FieldInfo fieldInfo = pInfo.ExcludeNullableType.GetFields().FirstOrDefault(delegate(FieldInfo e)
				{
					DescriptionAttribute customAttribute = e.GetCustomAttribute(false);
					string a = (customAttribute != null) ? customAttribute.Description : null;
					object itemValue7 = itemValue;
					return a == ((itemValue7 != null) ? itemValue7.ToString() : null);
				});
				if (fieldInfo != null)
				{
					newValue = Enum.Parse(pInfo.ExcludeNullableType, fieldInfo.Name, true);
				}
				else
				{
					Type excludeNullableType = pInfo.ExcludeNullableType;
					object itemValue6 = itemValue;
					newValue = Enum.Parse(excludeNullableType, (itemValue6 != null) ? itemValue6.ToString() : null, true);
				}
			}
			else
			{
				newValue = Convert.ChangeType(itemValue, pInfo.ExcludeNullableType, _config.Culture);
			}
			IL_37C:
			pInfo.Property.SetValue(v, newValue);
			return newValue;
		}
	}
}
