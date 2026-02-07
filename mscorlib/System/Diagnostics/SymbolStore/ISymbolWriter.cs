using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020009DE RID: 2526
	[ComVisible(true)]
	public interface ISymbolWriter
	{
		// Token: 0x06005A78 RID: 23160
		void Close();

		// Token: 0x06005A79 RID: 23161
		void CloseMethod();

		// Token: 0x06005A7A RID: 23162
		void CloseNamespace();

		// Token: 0x06005A7B RID: 23163
		void CloseScope(int endOffset);

		// Token: 0x06005A7C RID: 23164
		ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType);

		// Token: 0x06005A7D RID: 23165
		void DefineField(SymbolToken parent, string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3);

		// Token: 0x06005A7E RID: 23166
		void DefineGlobalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3);

		// Token: 0x06005A7F RID: 23167
		void DefineLocalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset);

		// Token: 0x06005A80 RID: 23168
		void DefineParameter(string name, ParameterAttributes attributes, int sequence, SymAddressKind addrKind, int addr1, int addr2, int addr3);

		// Token: 0x06005A81 RID: 23169
		void DefineSequencePoints(ISymbolDocumentWriter document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns);

		// Token: 0x06005A82 RID: 23170
		void Initialize(IntPtr emitter, string filename, bool fFullBuild);

		// Token: 0x06005A83 RID: 23171
		void OpenMethod(SymbolToken method);

		// Token: 0x06005A84 RID: 23172
		void OpenNamespace(string name);

		// Token: 0x06005A85 RID: 23173
		int OpenScope(int startOffset);

		// Token: 0x06005A86 RID: 23174
		void SetMethodSourceRange(ISymbolDocumentWriter startDoc, int startLine, int startColumn, ISymbolDocumentWriter endDoc, int endLine, int endColumn);

		// Token: 0x06005A87 RID: 23175
		void SetScopeRange(int scopeID, int startOffset, int endOffset);

		// Token: 0x06005A88 RID: 23176
		void SetSymAttribute(SymbolToken parent, string name, byte[] data);

		// Token: 0x06005A89 RID: 23177
		void SetUnderlyingWriter(IntPtr underlyingWriter);

		// Token: 0x06005A8A RID: 23178
		void SetUserEntryPoint(SymbolToken entryMethod);

		// Token: 0x06005A8B RID: 23179
		void UsingNamespace(string fullName);
	}
}
