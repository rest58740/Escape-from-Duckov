using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000764 RID: 1892
	[TypeLibImportClass(typeof(AssemblyBuilder))]
	[Guid("BEBB2505-8B54-3443-AEAD-142A16DD9CC7")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[CLSCompliant(false)]
	public interface _AssemblyBuilder
	{
		// Token: 0x060042D2 RID: 17106
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060042D3 RID: 17107
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060042D4 RID: 17108
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060042D5 RID: 17109
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
