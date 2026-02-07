using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MiniExcelLibs
{
	// Token: 0x02000019 RID: 25
	public class MemberSetter
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00003D16 File Offset: 0x00001F16
		public MemberSetter(PropertyInfo property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			this.setFunc = MemberSetter.CreateSetterDelegate(property);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003D3E File Offset: 0x00001F3E
		public void Invoke(object instance, object value)
		{
			this.setFunc(instance, value);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003D50 File Offset: 0x00001F50
		private static Action<object, object> CreateSetterDelegate(PropertyInfo property)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object));
			Expression instance = Expression.Convert(parameterExpression, property.DeclaringType);
			UnaryExpression unaryExpression = Expression.Convert(parameterExpression2, property.PropertyType);
			return Expression.Lambda<Action<object, object>>(Expression.Call(instance, property.GetSetMethod(true), new Expression[]
			{
				unaryExpression
			}), new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			}).Compile();
		}

		// Token: 0x04000022 RID: 34
		private readonly Action<object, object> setFunc;
	}
}
