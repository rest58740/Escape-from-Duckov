using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020009DB RID: 2523
	[ComVisible(true)]
	public interface ISymbolReader
	{
		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x06005A5E RID: 23134
		SymbolToken UserEntryPoint { get; }

		// Token: 0x06005A5F RID: 23135
		ISymbolDocument GetDocument(string url, Guid language, Guid languageVendor, Guid documentType);

		// Token: 0x06005A60 RID: 23136
		ISymbolDocument[] GetDocuments();

		// Token: 0x06005A61 RID: 23137
		ISymbolVariable[] GetGlobalVariables();

		// Token: 0x06005A62 RID: 23138
		ISymbolMethod GetMethod(SymbolToken method);

		// Token: 0x06005A63 RID: 23139
		ISymbolMethod GetMethod(SymbolToken method, int version);

		// Token: 0x06005A64 RID: 23140
		ISymbolMethod GetMethodFromDocumentPosition(ISymbolDocument document, int line, int column);

		// Token: 0x06005A65 RID: 23141
		ISymbolNamespace[] GetNamespaces();

		// Token: 0x06005A66 RID: 23142
		byte[] GetSymAttribute(SymbolToken parent, string name);

		// Token: 0x06005A67 RID: 23143
		ISymbolVariable[] GetVariables(SymbolToken parent);
	}
}
