using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000765 RID: 1893
	[Guid("B42B6AAC-317E-34D5-9FA9-093BB4160C50")]
	[ComVisible(true)]
	[CLSCompliant(false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibImportClass(typeof(AssemblyName))]
	public interface _AssemblyName
	{
		// Token: 0x060042D6 RID: 17110
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060042D7 RID: 17111
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060042D8 RID: 17112
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060042D9 RID: 17113
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
