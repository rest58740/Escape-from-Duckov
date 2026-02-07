using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000770 RID: 1904
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("A4924B27-6E3B-37F7-9B83-A4501955E6A7")]
	[TypeLibImportClass(typeof(ILGenerator))]
	[CLSCompliant(false)]
	public interface _ILGenerator
	{
		// Token: 0x06004363 RID: 17251
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06004364 RID: 17252
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06004365 RID: 17253
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06004366 RID: 17254
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
