using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007A7 RID: 1959
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020404-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IEnumVARIANT
	{
		// Token: 0x06004519 RID: 17689
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] object[] rgVar, IntPtr pceltFetched);

		// Token: 0x0600451A RID: 17690
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x0600451B RID: 17691
		[PreserveSig]
		int Reset();

		// Token: 0x0600451C RID: 17692
		IEnumVARIANT Clone();
	}
}
