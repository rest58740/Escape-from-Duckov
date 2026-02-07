using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200076E RID: 1902
	[TypeLibImportClass(typeof(FieldBuilder))]
	[Guid("CE1A3BF5-975E-30CC-97C9-1EF70F8F3993")]
	[CLSCompliant(false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	public interface _FieldBuilder
	{
		// Token: 0x0600433C RID: 17212
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x0600433D RID: 17213
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x0600433E RID: 17214
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x0600433F RID: 17215
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
