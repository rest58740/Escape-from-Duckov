using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200076B RID: 1899
	[CLSCompliant(false)]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibImportClass(typeof(EventBuilder))]
	[Guid("AADABA99-895D-3D65-9760-B1F12621FAE8")]
	public interface _EventBuilder
	{
		// Token: 0x0600430F RID: 17167
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06004310 RID: 17168
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06004311 RID: 17169
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06004312 RID: 17170
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
