using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200077D RID: 1917
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Guid("7D13DD37-5A04-393C-BBCA-A5FEA802893D")]
	[TypeLibImportClass(typeof(SignatureHelper))]
	public interface _SignatureHelper
	{
		// Token: 0x060043FE RID: 17406
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060043FF RID: 17407
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06004400 RID: 17408
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06004401 RID: 17409
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
