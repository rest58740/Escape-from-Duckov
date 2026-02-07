using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000759 RID: 1881
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete]
	[Guid("00000102-0000-0000-c000-000000000046")]
	[ComImport]
	public interface UCOMIEnumMoniker
	{
		// Token: 0x0600425E RID: 16990
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] UCOMIMoniker[] rgelt, out int pceltFetched);

		// Token: 0x0600425F RID: 16991
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06004260 RID: 16992
		[PreserveSig]
		int Reset();

		// Token: 0x06004261 RID: 16993
		void Clone(out UCOMIEnumMoniker ppenum);
	}
}
