using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200077B RID: 1915
	[Guid("15F9A479-9397-3A63-ACBD-F51977FB0F02")]
	[TypeLibImportClass(typeof(PropertyBuilder))]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[CLSCompliant(false)]
	public interface _PropertyBuilder
	{
		// Token: 0x060043DB RID: 17371
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060043DC RID: 17372
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060043DD RID: 17373
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060043DE RID: 17374
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
