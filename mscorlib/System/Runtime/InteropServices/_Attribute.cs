using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000766 RID: 1894
	[TypeLibImportClass(typeof(Attribute))]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[Guid("917B14D0-2D9E-38B8-92A9-381ACF52F7C0")]
	[ComVisible(true)]
	public interface _Attribute
	{
		// Token: 0x060042DA RID: 17114
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060042DB RID: 17115
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060042DC RID: 17116
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060042DD RID: 17117
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
