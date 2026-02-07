using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020009D5 RID: 2517
	[ComVisible(true)]
	public interface ISymbolBinder
	{
		// Token: 0x06005A43 RID: 23107
		[Obsolete("This interface is not 64-bit clean.  Use ISymbolBinder1 instead")]
		ISymbolReader GetReader(int importer, string filename, string searchPath);
	}
}
