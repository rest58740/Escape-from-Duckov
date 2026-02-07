using System;
using System.Globalization;

namespace System.Reflection
{
	// Token: 0x020008A2 RID: 2210
	public interface IReflect
	{
		// Token: 0x060048F6 RID: 18678
		MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x060048F7 RID: 18679
		MethodInfo GetMethod(string name, BindingFlags bindingAttr);

		// Token: 0x060048F8 RID: 18680
		MethodInfo[] GetMethods(BindingFlags bindingAttr);

		// Token: 0x060048F9 RID: 18681
		FieldInfo GetField(string name, BindingFlags bindingAttr);

		// Token: 0x060048FA RID: 18682
		FieldInfo[] GetFields(BindingFlags bindingAttr);

		// Token: 0x060048FB RID: 18683
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr);

		// Token: 0x060048FC RID: 18684
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x060048FD RID: 18685
		PropertyInfo[] GetProperties(BindingFlags bindingAttr);

		// Token: 0x060048FE RID: 18686
		MemberInfo[] GetMember(string name, BindingFlags bindingAttr);

		// Token: 0x060048FF RID: 18687
		MemberInfo[] GetMembers(BindingFlags bindingAttr);

		// Token: 0x06004900 RID: 18688
		object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06004901 RID: 18689
		Type UnderlyingSystemType { get; }
	}
}
