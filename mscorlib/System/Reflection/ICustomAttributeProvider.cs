using System;

namespace System.Reflection
{
	// Token: 0x020008A1 RID: 2209
	public interface ICustomAttributeProvider
	{
		// Token: 0x060048F3 RID: 18675
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x060048F4 RID: 18676
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x060048F5 RID: 18677
		bool IsDefined(Type attributeType, bool inherit);
	}
}
