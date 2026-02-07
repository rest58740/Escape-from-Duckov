using System;
using System.Linq;
using System.Reflection;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000025 RID: 37
	internal static class AttributeExtension
	{
		// Token: 0x060000ED RID: 237 RVA: 0x00004420 File Offset: 0x00002620
		internal static TValue GetAttributeValue<TAttribute, TValue>(this Type attrType, Func<TAttribute, TValue> selector) where TAttribute : Attribute
		{
			TAttribute attr = attrType.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault<object>() as TAttribute;
			return AttributeExtension.GetValueOrDefault<TAttribute, TValue>(selector, attr);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004458 File Offset: 0x00002658
		private static TValue GetValueOrDefault<TAttribute, TValue>(Func<TAttribute, TValue> selector, TAttribute attr) where TAttribute : Attribute
		{
			if (attr != null)
			{
				return selector(attr);
			}
			return default(TValue);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000447E File Offset: 0x0000267E
		internal static TAttribute GetAttribute<TAttribute>(this PropertyInfo prop, bool isInherit = true) where TAttribute : Attribute
		{
			return prop.GetAttributeValue((TAttribute attr) => attr, isInherit);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000044A8 File Offset: 0x000026A8
		internal static TValue GetAttributeValue<TAttribute, TValue>(this PropertyInfo prop, Func<TAttribute, TValue> selector, bool isInherit = true) where TAttribute : Attribute
		{
			TAttribute attr = Attribute.GetCustomAttribute(prop, typeof(TAttribute), isInherit) as TAttribute;
			return AttributeExtension.GetValueOrDefault<TAttribute, TValue>(selector, attr);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000044D8 File Offset: 0x000026D8
		internal static bool IsUseAttribute<TAttribute>(this PropertyInfo prop)
		{
			return Attribute.GetCustomAttribute(prop, typeof(TAttribute)) != null;
		}
	}
}
