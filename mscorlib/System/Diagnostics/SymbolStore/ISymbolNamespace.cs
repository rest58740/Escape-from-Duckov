using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020009DA RID: 2522
	[ComVisible(true)]
	public interface ISymbolNamespace
	{
		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06005A5B RID: 23131
		string Name { get; }

		// Token: 0x06005A5C RID: 23132
		ISymbolNamespace[] GetNamespaces();

		// Token: 0x06005A5D RID: 23133
		ISymbolVariable[] GetVariables();
	}
}
