using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200075A RID: 1882
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000101-0000-0000-c000-000000000046")]
	[Obsolete]
	[ComImport]
	public interface UCOMIEnumString
	{
		// Token: 0x06004262 RID: 16994
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] [Out] string[] rgelt, out int pceltFetched);

		// Token: 0x06004263 RID: 16995
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06004264 RID: 16996
		[PreserveSig]
		int Reset();

		// Token: 0x06004265 RID: 16997
		void Clone(out UCOMIEnumString ppenum);
	}
}
