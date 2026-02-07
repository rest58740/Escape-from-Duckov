using System;
using System.Threading;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200077E RID: 1918
	[TypeLibImportClass(typeof(Thread))]
	[Guid("C281C7F1-4AA9-3517-961A-463CFED57E75")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[ComVisible(true)]
	public interface _Thread
	{
		// Token: 0x06004402 RID: 17410
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06004403 RID: 17411
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06004404 RID: 17412
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06004405 RID: 17413
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
