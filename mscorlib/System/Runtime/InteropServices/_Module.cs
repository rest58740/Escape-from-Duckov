using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000777 RID: 1911
	[CLSCompliant(false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("D002E9BA-D9E3-3749-B1D3-D565A08B13E7")]
	[TypeLibImportClass(typeof(Module))]
	[ComVisible(true)]
	public interface _Module
	{
		// Token: 0x060043CB RID: 17355
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060043CC RID: 17356
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060043CD RID: 17357
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060043CE RID: 17358
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
