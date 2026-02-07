using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.OpenXml;

namespace MiniExcelLibs.Utils
{
	// Token: 0x0200002A RID: 42
	internal static class CustomPropertyHelper
	{
		// Token: 0x0600011F RID: 287 RVA: 0x000048B8 File Offset: 0x00002AB8
		internal static IDictionary<string, object> GetEmptyExpandoObject(int maxColumnIndex, int startCellIndex)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			for (int i = startCellIndex; i <= maxColumnIndex; i++)
			{
				string alphabetColumnName = ColumnHelper.GetAlphabetColumnName(i);
				if (!dictionary.ContainsKey(alphabetColumnName))
				{
					dictionary.Add(alphabetColumnName, null);
				}
			}
			return dictionary;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000048F0 File Offset: 0x00002AF0
		internal static IDictionary<string, object> GetEmptyExpandoObject(Dictionary<int, string> hearrows)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (KeyValuePair<int, string> keyValuePair in hearrows)
			{
				if (!dictionary.ContainsKey(keyValuePair.Value))
				{
					dictionary.Add(keyValuePair.Value, null);
				}
			}
			return dictionary;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000495C File Offset: 0x00002B5C
		internal static List<ExcelColumnInfo> GetSaveAsProperties(this Type type, Configuration configuration)
		{
			List<ExcelColumnInfo> list = (from prop in CustomPropertyHelper.GetExcelPropertyInfo(type, BindingFlags.Instance | BindingFlags.Public, configuration)
			where prop.Property.CanRead
			select prop).ToList<ExcelColumnInfo>();
			if (list.Count == 0)
			{
				throw new InvalidOperationException(type.Name + " un-ignore properties count can't be 0");
			}
			return CustomPropertyHelper.SortCustomProps(list);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000049C0 File Offset: 0x00002BC0
		internal static List<ExcelColumnInfo> SortCustomProps(List<ExcelColumnInfo> props)
		{
			IEnumerable<ExcelColumnInfo> source = props.Where(delegate(ExcelColumnInfo w)
			{
				if (w.ExcelColumnIndex != null)
				{
					int? excelColumnIndex = w.ExcelColumnIndex;
					int num3 = -1;
					return excelColumnIndex.GetValueOrDefault() > num3 & excelColumnIndex != null;
				}
				return false;
			});
			if ((from g in source
			group g by g.ExcelColumnIndex).Any((IGrouping<int?, ExcelColumnInfo> _) => _.Count<ExcelColumnInfo>() > 1))
			{
				throw new InvalidOperationException("Duplicate column name");
			}
			int num = props.Count - 1;
			if (source.Any<ExcelColumnInfo>())
			{
				num = Math.Max(source.Max((ExcelColumnInfo w) => w.ExcelColumnIndex).Value, num);
			}
			List<ExcelColumnInfo> source2 = (from w in props
			where w.ExcelColumnIndex == null || w.ExcelColumnIndex.GetValueOrDefault() == -1
			select w).ToList<ExcelColumnInfo>();
			List<ExcelColumnInfo> list = new List<ExcelColumnInfo>();
			int num2 = 0;
			int i;
			int j;
			for (i = 0; i <= num; i = j + 1)
			{
				ExcelColumnInfo excelColumnInfo = source.SingleOrDefault(delegate(ExcelColumnInfo s)
				{
					int? excelColumnIndex = s.ExcelColumnIndex;
					int i2 = i;
					return excelColumnIndex.GetValueOrDefault() == i2 & excelColumnIndex != null;
				});
				if (excelColumnInfo != null)
				{
					list.Add(excelColumnInfo);
				}
				else
				{
					ExcelColumnInfo excelColumnInfo2 = source2.ElementAtOrDefault(num2);
					if (excelColumnInfo2 == null)
					{
						list.Add(null);
					}
					else
					{
						excelColumnInfo2.ExcelColumnIndex = new int?(i);
						list.Add(excelColumnInfo2);
					}
					num2++;
				}
				j = i;
			}
			return list;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00004B50 File Offset: 0x00002D50
		internal static List<ExcelColumnInfo> GetExcelCustomPropertyInfos(Type type, string[] keys, Configuration configuration)
		{
			List<ExcelColumnInfo> list = CustomPropertyHelper.GetExcelPropertyInfo(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, configuration).Where(delegate(ExcelColumnInfo prop)
			{
				if (prop.Property.Info.GetSetMethod() != null)
				{
					if (!prop.Property.Info.GetAttributeValue((ExcelIgnoreAttribute x) => x.ExcelIgnore, true))
					{
						return !prop.Property.Info.GetAttributeValue((ExcelColumnAttribute x) => x.Ignore, true);
					}
				}
				return false;
			}).ToList<ExcelColumnInfo>();
			if (list.Count == 0)
			{
				throw new InvalidOperationException(type.Name + " un-ignore properties count can't be 0");
			}
			if ((from g in list.Where(delegate(ExcelColumnInfo w)
			{
				if (w.ExcelColumnIndex != null)
				{
					int? excelColumnIndex2 = w.ExcelColumnIndex;
					int num2 = -1;
					return excelColumnIndex2.GetValueOrDefault() > num2 & excelColumnIndex2 != null;
				}
				return false;
			})
			group g by g.ExcelColumnIndex).Any((IGrouping<int?, ExcelColumnInfo> _) => _.Count<ExcelColumnInfo>() > 1))
			{
				throw new InvalidOperationException("Duplicate column name");
			}
			string text = keys.Last<string>();
			int columnIndex = ColumnHelper.GetColumnIndex(text);
			foreach (ExcelColumnInfo excelColumnInfo in list)
			{
				if (excelColumnInfo.ExcelColumnIndex != null)
				{
					int? excelColumnIndex = excelColumnInfo.ExcelColumnIndex;
					int num = columnIndex;
					if (excelColumnIndex.GetValueOrDefault() > num & excelColumnIndex != null)
					{
						throw new ArgumentException(string.Format("ExcelColumnIndex {0} over haeder max index {1}", excelColumnInfo.ExcelColumnIndex, text));
					}
					if (excelColumnInfo.ExcelColumnName == null)
					{
						throw new InvalidOperationException(string.Format("{0} {1}'s ExcelColumnIndex {2} can't find excel column name", excelColumnInfo.Property.Info.DeclaringType.Name, excelColumnInfo.Property.Name, excelColumnInfo.ExcelColumnIndex));
					}
				}
			}
			return list;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00004D18 File Offset: 0x00002F18
		internal static string DescriptionAttr(Type type, object source)
		{
			FieldInfo field = type.GetField(source.ToString());
			if (field == null)
			{
				return source.ToString();
			}
			DescriptionAttribute[] array = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (array != null && array.Length != 0)
			{
				return array[0].Description;
			}
			return source.ToString();
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00004D70 File Offset: 0x00002F70
		private static IEnumerable<ExcelColumnInfo> ConvertToExcelCustomPropertyInfo(PropertyInfo[] props, Configuration configuration)
		{
			return from _ in props.Select(delegate(PropertyInfo p)
			{
				Type underlyingType = Nullable.GetUnderlyingType(p.PropertyType);
				ExcelColumnNameAttribute attribute = p.GetAttribute(true);
				Type excludeNullableType = underlyingType ?? p.PropertyType;
				ExcelFormatAttribute attribute2 = p.GetAttribute(true);
				string text = (attribute2 != null) ? attribute2.Format : null;
				ExcelColumnAttribute excelColumnAttribute = p.GetAttribute(true);
				if (configuration.DynamicColumns != null && configuration.DynamicColumns.Length != 0)
				{
					DynamicExcelColumn dynamicExcelColumn = configuration.DynamicColumns.SingleOrDefault((DynamicExcelColumn _) => _.Key == p.Name);
					if (dynamicExcelColumn != null)
					{
						excelColumnAttribute = dynamicExcelColumn;
					}
				}
				bool flag;
				if (!p.GetAttributeValue((ExcelIgnoreAttribute x) => x.ExcelIgnore, true))
				{
					if (!p.GetAttributeValue((ExcelColumnAttribute x) => x.Ignore, true))
					{
						flag = (excelColumnAttribute != null && excelColumnAttribute.Ignore);
						goto IL_110;
					}
				}
				flag = true;
				IL_110:
				if (flag)
				{
					return null;
				}
				int? num = (excelColumnAttribute != null && excelColumnAttribute.Index > -1) ? new int?(excelColumnAttribute.Index) : null;
				ExcelColumnInfo excelColumnInfo = new ExcelColumnInfo();
				excelColumnInfo.Property = new Property(p);
				excelColumnInfo.ExcludeNullableType = excludeNullableType;
				excelColumnInfo.Nullable = (underlyingType != null);
				excelColumnInfo.ExcelColumnAliases = (((attribute != null) ? attribute.Aliases : null) ?? ((excelColumnAttribute != null) ? excelColumnAttribute.Aliases : null));
				string excelColumnName;
				if ((excelColumnName = ((attribute != null) ? attribute.ExcelColumnName : null)) == null)
				{
					DisplayNameAttribute attribute3 = p.GetAttribute(true);
					if ((excelColumnName = ((attribute3 != null) ? attribute3.DisplayName : null)) == null)
					{
						excelColumnName = (((excelColumnAttribute != null) ? excelColumnAttribute.Name : null) ?? p.Name);
					}
				}
				excelColumnInfo.ExcelColumnName = excelColumnName;
				ExcelColumnIndexAttribute attribute4 = p.GetAttribute(true);
				excelColumnInfo.ExcelColumnIndex = ((attribute4 != null) ? new int?(attribute4.ExcelColumnIndex) : num);
				ExcelColumnIndexAttribute attribute5 = p.GetAttribute(true);
				excelColumnInfo.ExcelIndexName = (((attribute5 != null) ? attribute5.ExcelXName : null) ?? ((excelColumnAttribute != null) ? excelColumnAttribute.IndexName : null));
				ExcelColumnWidthAttribute attribute6 = p.GetAttribute(true);
				excelColumnInfo.ExcelColumnWidth = ((attribute6 != null) ? new double?(attribute6.ExcelColumnWidth) : ((excelColumnAttribute != null) ? new double?(excelColumnAttribute.Width) : null));
				excelColumnInfo.ExcelFormat = (text ?? ((excelColumnAttribute != null) ? excelColumnAttribute.Format : null));
				excelColumnInfo.ExcelFormatId = ((excelColumnAttribute != null) ? excelColumnAttribute.FormatId : -1);
				excelColumnInfo.ExcelColumnType = ((excelColumnAttribute != null) ? excelColumnAttribute.Type : ColumnType.Value);
				return excelColumnInfo;
			})
			where _ != null
			select _;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00004DC0 File Offset: 0x00002FC0
		private static IEnumerable<ExcelColumnInfo> GetExcelPropertyInfo(Type type, BindingFlags bindingFlags, Configuration configuration)
		{
			return CustomPropertyHelper.ConvertToExcelCustomPropertyInfo(type.GetProperties(bindingFlags), configuration);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00004DD0 File Offset: 0x00002FD0
		internal static ExcellSheetInfo GetExcellSheetInfo(Type type, Configuration configuration)
		{
			ExcellSheetInfo excellSheetInfo = new ExcellSheetInfo
			{
				Key = type.Name,
				ExcelSheetName = null,
				ExcelSheetState = SheetState.Visible
			};
			ExcelSheetAttribute excelSheetAttribute = type.GetCustomAttribute(typeof(ExcelSheetAttribute)) as ExcelSheetAttribute;
			if (excelSheetAttribute != null)
			{
				excellSheetInfo.ExcelSheetName = (excelSheetAttribute.Name ?? type.Name);
				excellSheetInfo.ExcelSheetState = excelSheetAttribute.State;
			}
			OpenXmlConfiguration openXmlConfiguration = configuration as OpenXmlConfiguration;
			if (openXmlConfiguration != null && openXmlConfiguration.DynamicSheets != null && openXmlConfiguration.DynamicSheets.Length != 0)
			{
				DynamicExcelSheet dynamicExcelSheet = openXmlConfiguration.DynamicSheets.SingleOrDefault((DynamicExcelSheet _) => _.Key == type.Name);
				if (dynamicExcelSheet != null)
				{
					excellSheetInfo.ExcelSheetName = dynamicExcelSheet.Name;
					excellSheetInfo.ExcelSheetState = dynamicExcelSheet.State;
				}
			}
			return excellSheetInfo;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004EA8 File Offset: 0x000030A8
		internal static List<ExcelColumnInfo> GetDictionaryColumnInfo(IDictionary<string, object> dicString, IDictionary dic, Configuration configuration)
		{
			List<ExcelColumnInfo> props = new List<ExcelColumnInfo>();
			if (dicString != null)
			{
				using (IEnumerator<string> enumerator = dicString.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string key = enumerator.Current;
						CustomPropertyHelper.SetDictionaryColumnInfo(props, key, configuration);
					}
					goto IL_85;
				}
			}
			if (dic != null)
			{
				using (IEnumerator enumerator2 = dic.Keys.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						object key2 = enumerator2.Current;
						CustomPropertyHelper.SetDictionaryColumnInfo(props, key2, configuration);
					}
					goto IL_85;
				}
			}
			throw new NotSupportedException("SetDictionaryColumnInfo Error");
			IL_85:
			return CustomPropertyHelper.SortCustomProps(props);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00004F5C File Offset: 0x0000315C
		internal static void SetDictionaryColumnInfo(List<ExcelColumnInfo> _props, object key, Configuration configuration)
		{
			ExcelColumnInfo excelColumnInfo = new ExcelColumnInfo();
			ExcelColumnInfo excelColumnInfo2 = excelColumnInfo;
			object key2 = key;
			excelColumnInfo2.ExcelColumnName = ((key2 != null) ? key2.ToString() : null);
			excelColumnInfo.Key = key;
			bool flag = false;
			if (configuration.DynamicColumns != null && configuration.DynamicColumns.Length != 0)
			{
				DynamicExcelColumn dynamicExcelColumn = configuration.DynamicColumns.SingleOrDefault((DynamicExcelColumn _) => _.Key == key.ToString());
				if (dynamicExcelColumn != null)
				{
					excelColumnInfo.Nullable = true;
					if (dynamicExcelColumn.Format != null)
					{
						excelColumnInfo.ExcelFormat = dynamicExcelColumn.Format;
						excelColumnInfo.ExcelFormatId = dynamicExcelColumn.FormatId;
					}
					if (dynamicExcelColumn.Aliases != null)
					{
						excelColumnInfo.ExcelColumnAliases = dynamicExcelColumn.Aliases;
					}
					if (dynamicExcelColumn.IndexName != null)
					{
						excelColumnInfo.ExcelIndexName = dynamicExcelColumn.IndexName;
					}
					excelColumnInfo.ExcelColumnIndex = new int?(dynamicExcelColumn.Index);
					if (dynamicExcelColumn.Name != null)
					{
						excelColumnInfo.ExcelColumnName = dynamicExcelColumn.Name;
					}
					flag = dynamicExcelColumn.Ignore;
					excelColumnInfo.ExcelColumnWidth = new double?(dynamicExcelColumn.Width);
					excelColumnInfo.ExcelColumnType = dynamicExcelColumn.Type;
					excelColumnInfo.CustomFormatter = dynamicExcelColumn.CustomFormatter;
				}
			}
			if (!flag)
			{
				_props.Add(excelColumnInfo);
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005088 File Offset: 0x00003288
		internal static bool TryGetTypeColumnInfo(Type type, Configuration configuration, out List<ExcelColumnInfo> props)
		{
			if (type == null)
			{
				props = null;
				return false;
			}
			if (type.IsValueType)
			{
				throw new NotImplementedException("MiniExcel not support only " + type.Name + " value generic type");
			}
			if (type == typeof(string) || type == typeof(DateTime) || type == typeof(Guid))
			{
				throw new NotImplementedException("MiniExcel not support only " + type.Name + " generic type");
			}
			if (CustomPropertyHelper.ValueIsNeededToDetermineProperties(type))
			{
				props = null;
				return false;
			}
			props = type.GetSaveAsProperties(configuration);
			return true;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005130 File Offset: 0x00003330
		internal static List<ExcelColumnInfo> GetColumnInfoFromValue(object value, Configuration configuration)
		{
			IDictionary<string, object> dictionary = value as IDictionary<string, object>;
			if (dictionary != null)
			{
				return CustomPropertyHelper.GetDictionaryColumnInfo(dictionary, null, configuration);
			}
			IDictionary dictionary2 = value as IDictionary;
			if (dictionary2 == null)
			{
				return value.GetType().GetSaveAsProperties(configuration);
			}
			return CustomPropertyHelper.GetDictionaryColumnInfo(null, dictionary2, configuration);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005171 File Offset: 0x00003371
		private static bool ValueIsNeededToDetermineProperties(Type type)
		{
			return type == typeof(object) || typeof(IDictionary<string, object>).IsAssignableFrom(type) || typeof(IDictionary).IsAssignableFrom(type);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000051AC File Offset: 0x000033AC
		internal static ExcelColumnInfo GetColumnInfosFromDynamicConfiguration(string columnName, Configuration configuration)
		{
			ExcelColumnInfo excelColumnInfo = new ExcelColumnInfo
			{
				ExcelColumnName = columnName,
				Key = columnName
			};
			if (configuration.DynamicColumns == null || configuration.DynamicColumns.Length == 0)
			{
				return excelColumnInfo;
			}
			DynamicExcelColumn dynamicExcelColumn = configuration.DynamicColumns.SingleOrDefault((DynamicExcelColumn _) => string.Equals(_.Key, columnName, StringComparison.OrdinalIgnoreCase));
			if (dynamicExcelColumn == null || dynamicExcelColumn.Ignore)
			{
				return excelColumnInfo;
			}
			excelColumnInfo.Nullable = true;
			excelColumnInfo.ExcelIgnore = dynamicExcelColumn.Ignore;
			excelColumnInfo.ExcelColumnType = dynamicExcelColumn.Type;
			excelColumnInfo.ExcelColumnIndex = new int?(dynamicExcelColumn.Index);
			excelColumnInfo.ExcelColumnWidth = new double?(dynamicExcelColumn.Width);
			excelColumnInfo.CustomFormatter = dynamicExcelColumn.CustomFormatter;
			if (dynamicExcelColumn.Format != null)
			{
				excelColumnInfo.ExcelFormat = dynamicExcelColumn.Format;
				excelColumnInfo.ExcelFormatId = dynamicExcelColumn.FormatId;
			}
			if (dynamicExcelColumn.Aliases != null)
			{
				excelColumnInfo.ExcelColumnAliases = dynamicExcelColumn.Aliases;
			}
			if (dynamicExcelColumn.IndexName != null)
			{
				excelColumnInfo.ExcelIndexName = dynamicExcelColumn.IndexName;
			}
			if (dynamicExcelColumn.Name != null)
			{
				excelColumnInfo.ExcelColumnName = dynamicExcelColumn.Name;
			}
			return excelColumnInfo;
		}
	}
}
