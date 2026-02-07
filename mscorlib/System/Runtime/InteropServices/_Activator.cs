using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000762 RID: 1890
	[ComVisible(true)]
	[CLSCompliant(false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("03973551-57A1-3900-A2B5-9083E3FF2943")]
	[TypeLibImportClass(typeof(Activator))]
	public interface _Activator
	{
		// Token: 0x060042A2 RID: 17058
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060042A3 RID: 17059
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060042A4 RID: 17060
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060042A5 RID: 17061
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
