using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007AF RID: 1967
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000000c-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IStream
	{
		// Token: 0x0600454E RID: 17742
		void Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pv, int cb, IntPtr pcbRead);

		// Token: 0x0600454F RID: 17743
		void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, IntPtr pcbWritten);

		// Token: 0x06004550 RID: 17744
		void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

		// Token: 0x06004551 RID: 17745
		void SetSize(long libNewSize);

		// Token: 0x06004552 RID: 17746
		void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

		// Token: 0x06004553 RID: 17747
		void Commit(int grfCommitFlags);

		// Token: 0x06004554 RID: 17748
		void Revert();

		// Token: 0x06004555 RID: 17749
		void LockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x06004556 RID: 17750
		void UnlockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x06004557 RID: 17751
		void Stat(out STATSTG pstatstg, int grfStatFlag);

		// Token: 0x06004558 RID: 17752
		void Clone(out IStream ppstm);
	}
}
