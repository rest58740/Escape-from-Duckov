using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000003 RID: 3
	internal sealed class AsyncMethodBuilderAttribute : Attribute
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C3 File Offset: 0x000002C3
		public Type BuilderType { get; }

		// Token: 0x06000004 RID: 4 RVA: 0x000020CB File Offset: 0x000002CB
		public AsyncMethodBuilderAttribute(Type builderType)
		{
			this.BuilderType = builderType;
		}
	}
}
