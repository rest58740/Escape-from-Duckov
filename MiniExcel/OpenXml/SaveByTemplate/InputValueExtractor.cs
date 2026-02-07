using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.OpenXml.SaveByTemplate
{
	// Token: 0x02000052 RID: 82
	public class InputValueExtractor : IInputValueExtractor
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0000DB70 File Offset: 0x0000BD70
		public IDictionary<string, object> ToValueDictionary(object valueObject)
		{
			Dictionary<string, object> dictionary = valueObject as Dictionary<string, object>;
			if (dictionary == null)
			{
				return InputValueExtractor.GetValuesFromObject(valueObject);
			}
			return InputValueExtractor.GetValuesFromDictionary(dictionary);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000DB94 File Offset: 0x0000BD94
		private static IDictionary<string, object> GetValuesFromDictionary(Dictionary<string, object> valueDictionary)
		{
			return valueDictionary.ToDictionary((KeyValuePair<string, object> x) => x.Key, delegate(KeyValuePair<string, object> x)
			{
				IDataReader dataReader = x.Value as IDataReader;
				if (dataReader == null)
				{
					return x.Value;
				}
				return TypeHelper.ConvertToEnumerableDictionary(dataReader).ToList<IDictionary<string, object>>();
			});
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000DBE8 File Offset: 0x0000BDE8
		private static IDictionary<string, object> GetValuesFromObject(object valueObject)
		{
			Type type = valueObject.GetType();
			var first = from property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			select new
			{
				Name = property.Name,
				Value = property.GetValue(valueObject)
			};
			var second = from field in type.GetFields(BindingFlags.Instance | BindingFlags.Public)
			select new
			{
				Name = field.Name,
				Value = field.GetValue(valueObject)
			};
			return first.Concat(second).ToDictionary(x => x.Name, x => x.Value);
		}
	}
}
