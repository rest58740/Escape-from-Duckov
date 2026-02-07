using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000774 RID: 1908
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("007D8A14-FDF3-363E-9A0B-FEC0618260A2")]
	[TypeLibImportClass(typeof(MethodBuilder))]
	[ComVisible(true)]
	[CLSCompliant(false)]
	public interface _MethodBuilder
	{
		// Token: 0x0600439D RID: 17309
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x0600439E RID: 17310
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x0600439F RID: 17311
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060043A0 RID: 17312
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
