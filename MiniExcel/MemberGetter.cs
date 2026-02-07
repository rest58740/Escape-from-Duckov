using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MiniExcelLibs
{
	// Token: 0x02000018 RID: 24
	public class MemberGetter
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00003C3A File Offset: 0x00001E3A
		public MemberGetter(PropertyInfo property)
		{
			this.m_getFunc = MemberGetter.CreateGetterDelegate(property);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003C4E File Offset: 0x00001E4E
		public MemberGetter(FieldInfo fieldInfo)
		{
			this.m_getFunc = MemberGetter.CreateGetterDelegate(fieldInfo);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003C62 File Offset: 0x00001E62
		public object Invoke(object instance)
		{
			return this.m_getFunc(instance);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003C70 File Offset: 0x00001E70
		private static Func<object, object> CreateGetterDelegate(PropertyInfo property)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
			return Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Property(Expression.Convert(parameterExpression, property.DeclaringType), property), typeof(object)), new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003CC4 File Offset: 0x00001EC4
		private static Func<object, object> CreateGetterDelegate(FieldInfo fieldInfo)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
			return Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Field(Expression.Convert(parameterExpression, fieldInfo.DeclaringType), fieldInfo), typeof(object)), new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		// Token: 0x04000021 RID: 33
		private readonly Func<object, object> m_getFunc;
	}
}
