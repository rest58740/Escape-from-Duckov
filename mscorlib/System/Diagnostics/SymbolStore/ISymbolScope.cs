using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020009DC RID: 2524
	[ComVisible(true)]
	public interface ISymbolScope
	{
		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x06005A68 RID: 23144
		int EndOffset { get; }

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x06005A69 RID: 23145
		ISymbolMethod Method { get; }

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x06005A6A RID: 23146
		ISymbolScope Parent { get; }

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x06005A6B RID: 23147
		int StartOffset { get; }

		// Token: 0x06005A6C RID: 23148
		ISymbolScope[] GetChildren();

		// Token: 0x06005A6D RID: 23149
		ISymbolVariable[] GetLocals();

		// Token: 0x06005A6E RID: 23150
		ISymbolNamespace[] GetNamespaces();
	}
}
