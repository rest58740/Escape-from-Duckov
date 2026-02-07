using System;

namespace System.Reflection
{
	// Token: 0x020008A5 RID: 2213
	public struct InterfaceMapping
	{
		// Token: 0x04002EB3 RID: 11955
		public Type TargetType;

		// Token: 0x04002EB4 RID: 11956
		public Type InterfaceType;

		// Token: 0x04002EB5 RID: 11957
		public MethodInfo[] TargetMethods;

		// Token: 0x04002EB6 RID: 11958
		public MethodInfo[] InterfaceMethods;
	}
}
