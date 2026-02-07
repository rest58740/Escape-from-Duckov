using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000771 RID: 1905
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibImportClass(typeof(LocalBuilder))]
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Guid("4E6350D1-A08B-3DEC-9A3E-C465F9AEEC0C")]
	public interface _LocalBuilder
	{
		// Token: 0x06004367 RID: 17255
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06004368 RID: 17256
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06004369 RID: 17257
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x0600436A RID: 17258
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
