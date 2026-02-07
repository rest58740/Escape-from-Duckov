using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007A0 RID: 1952
	[Guid("B196B285-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumConnectionPoints
	{
		// Token: 0x06004505 RID: 17669
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] IConnectionPoint[] rgelt, IntPtr pceltFetched);

		// Token: 0x06004506 RID: 17670
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06004507 RID: 17671
		void Reset();

		// Token: 0x06004508 RID: 17672
		void Clone(out IEnumConnectionPoints ppenum);
	}
}
