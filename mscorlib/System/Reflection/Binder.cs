using System;
using System.Globalization;

namespace System.Reflection
{
	// Token: 0x02000892 RID: 2194
	public abstract class Binder
	{
		// Token: 0x0600487B RID: 18555
		public abstract FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture);

		// Token: 0x0600487C RID: 18556
		public abstract MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] names, out object state);

		// Token: 0x0600487D RID: 18557
		public abstract object ChangeType(object value, Type type, CultureInfo culture);

		// Token: 0x0600487E RID: 18558
		public abstract void ReorderArgumentArray(ref object[] args, object state);

		// Token: 0x0600487F RID: 18559
		public abstract MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06004880 RID: 18560
		public abstract PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers);

		// Token: 0x06004881 RID: 18561 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool CanChangeType(object value, Type type, CultureInfo culture)
		{
			return false;
		}
	}
}
