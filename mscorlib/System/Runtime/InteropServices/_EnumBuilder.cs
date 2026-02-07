using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200076A RID: 1898
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Guid("C7BD73DE-9F85-3290-88EE-090B8BDFE2DF")]
	[TypeLibImportClass(typeof(EnumBuilder))]
	public interface _EnumBuilder
	{
		// Token: 0x0600430B RID: 17163
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x0600430C RID: 17164
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x0600430D RID: 17165
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x0600430E RID: 17166
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
