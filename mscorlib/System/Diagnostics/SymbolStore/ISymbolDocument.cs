using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020009D7 RID: 2519
	[ComVisible(true)]
	public interface ISymbolDocument
	{
		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06005A45 RID: 23109
		Guid CheckSumAlgorithmId { get; }

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06005A46 RID: 23110
		Guid DocumentType { get; }

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06005A47 RID: 23111
		bool HasEmbeddedSource { get; }

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06005A48 RID: 23112
		Guid Language { get; }

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x06005A49 RID: 23113
		Guid LanguageVendor { get; }

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x06005A4A RID: 23114
		int SourceLength { get; }

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x06005A4B RID: 23115
		string URL { get; }

		// Token: 0x06005A4C RID: 23116
		int FindClosestLine(int line);

		// Token: 0x06005A4D RID: 23117
		byte[] GetCheckSum();

		// Token: 0x06005A4E RID: 23118
		byte[] GetSourceRange(int startLine, int startColumn, int endLine, int endColumn);
	}
}
