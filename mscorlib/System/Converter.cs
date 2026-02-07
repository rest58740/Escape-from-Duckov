using System;

namespace System
{
	// Token: 0x020000EF RID: 239
	// (Invoke) Token: 0x060006DF RID: 1759
	public delegate TOutput Converter<in TInput, out TOutput>(TInput input);
}
