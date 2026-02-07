using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000767 RID: 1895
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Guid("ED3E4384-D7E2-3FA7-8FFD-8940D330519A")]
	[TypeLibImportClass(typeof(ConstructorBuilder))]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface _ConstructorBuilder
	{
		// Token: 0x060042DE RID: 17118
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060042DF RID: 17119
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060042E0 RID: 17120
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060042E1 RID: 17121
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
