using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020009D9 RID: 2521
	[ComVisible(true)]
	public interface ISymbolMethod
	{
		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06005A51 RID: 23121
		ISymbolScope RootScope { get; }

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06005A52 RID: 23122
		int SequencePointCount { get; }

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06005A53 RID: 23123
		SymbolToken Token { get; }

		// Token: 0x06005A54 RID: 23124
		ISymbolNamespace GetNamespace();

		// Token: 0x06005A55 RID: 23125
		int GetOffset(ISymbolDocument document, int line, int column);

		// Token: 0x06005A56 RID: 23126
		ISymbolVariable[] GetParameters();

		// Token: 0x06005A57 RID: 23127
		int[] GetRanges(ISymbolDocument document, int line, int column);

		// Token: 0x06005A58 RID: 23128
		ISymbolScope GetScope(int offset);

		// Token: 0x06005A59 RID: 23129
		void GetSequencePoints(int[] offsets, ISymbolDocument[] documents, int[] lines, int[] columns, int[] endLines, int[] endColumns);

		// Token: 0x06005A5A RID: 23130
		bool GetSourceStartEnd(ISymbolDocument[] docs, int[] lines, int[] columns);
	}
}
