using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020009D8 RID: 2520
	[ComVisible(true)]
	public interface ISymbolDocumentWriter
	{
		// Token: 0x06005A4F RID: 23119
		void SetCheckSum(Guid algorithmId, byte[] checkSum);

		// Token: 0x06005A50 RID: 23120
		void SetSource(byte[] source);
	}
}
