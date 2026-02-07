using System;
using System.Reflection;

namespace System.Runtime.InteropServices.Expando
{
	// Token: 0x0200079B RID: 1947
	[ComVisible(true)]
	[Guid("AFBF15E6-C37C-11d2-B88E-00A0C9B471B8")]
	public interface IExpando : IReflect
	{
		// Token: 0x060044F0 RID: 17648
		FieldInfo AddField(string name);

		// Token: 0x060044F1 RID: 17649
		PropertyInfo AddProperty(string name);

		// Token: 0x060044F2 RID: 17650
		MethodInfo AddMethod(string name, Delegate method);

		// Token: 0x060044F3 RID: 17651
		void RemoveMember(MemberInfo m);
	}
}
