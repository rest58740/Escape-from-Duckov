using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007A6 RID: 1958
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000101-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IEnumString
	{
		// Token: 0x06004515 RID: 17685
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] [Out] string[] rgelt, IntPtr pceltFetched);

		// Token: 0x06004516 RID: 17686
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06004517 RID: 17687
		void Reset();

		// Token: 0x06004518 RID: 17688
		void Clone(out IEnumString ppenum);
	}
}
