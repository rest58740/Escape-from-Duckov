using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020009D6 RID: 2518
	[ComVisible(true)]
	public interface ISymbolBinder1
	{
		// Token: 0x06005A44 RID: 23108
		ISymbolReader GetReader(IntPtr importer, string filename, string searchPath);
	}
}
