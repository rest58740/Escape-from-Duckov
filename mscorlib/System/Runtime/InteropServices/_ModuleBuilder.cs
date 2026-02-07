using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000778 RID: 1912
	[ComVisible(true)]
	[CLSCompliant(false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("D05FFA9A-04AF-3519-8EE1-8D93AD73430B")]
	[TypeLibImportClass(typeof(ModuleBuilder))]
	public interface _ModuleBuilder
	{
		// Token: 0x060043CF RID: 17359
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060043D0 RID: 17360
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060043D1 RID: 17361
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060043D2 RID: 17362
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
