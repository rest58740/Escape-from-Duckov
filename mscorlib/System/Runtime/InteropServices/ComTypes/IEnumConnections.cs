using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007A2 RID: 1954
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B196B287-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface IEnumConnections
	{
		// Token: 0x06004509 RID: 17673
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] CONNECTDATA[] rgelt, IntPtr pceltFetched);

		// Token: 0x0600450A RID: 17674
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x0600450B RID: 17675
		void Reset();

		// Token: 0x0600450C RID: 17676
		void Clone(out IEnumConnections ppenum);
	}
}
