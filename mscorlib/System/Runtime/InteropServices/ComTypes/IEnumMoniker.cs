using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007A5 RID: 1957
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000102-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IEnumMoniker
	{
		// Token: 0x06004511 RID: 17681
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] IMoniker[] rgelt, IntPtr pceltFetched);

		// Token: 0x06004512 RID: 17682
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06004513 RID: 17683
		void Reset();

		// Token: 0x06004514 RID: 17684
		void Clone(out IEnumMoniker ppenum);
	}
}
