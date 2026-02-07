using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200077A RID: 1914
	[ComVisible(true)]
	[CLSCompliant(false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("993634C4-E47A-32CC-BE08-85F567DC27D6")]
	[TypeLibImportClass(typeof(ParameterInfo))]
	public interface _ParameterInfo
	{
		// Token: 0x060043D7 RID: 17367
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060043D8 RID: 17368
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060043D9 RID: 17369
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060043DA RID: 17370
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
