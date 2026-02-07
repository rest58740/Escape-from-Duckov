using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200075F RID: 1887
	[Obsolete]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000000c-0000-0000-c000-000000000046")]
	[ComImport]
	public interface UCOMIStream
	{
		// Token: 0x0600428B RID: 17035
		void Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pv, int cb, IntPtr pcbRead);

		// Token: 0x0600428C RID: 17036
		void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, IntPtr pcbWritten);

		// Token: 0x0600428D RID: 17037
		void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

		// Token: 0x0600428E RID: 17038
		void SetSize(long libNewSize);

		// Token: 0x0600428F RID: 17039
		void CopyTo(UCOMIStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

		// Token: 0x06004290 RID: 17040
		void Commit(int grfCommitFlags);

		// Token: 0x06004291 RID: 17041
		void Revert();

		// Token: 0x06004292 RID: 17042
		void LockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x06004293 RID: 17043
		void UnlockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x06004294 RID: 17044
		void Stat(out STATSTG pstatstg, int grfStatFlag);

		// Token: 0x06004295 RID: 17045
		void Clone(out UCOMIStream ppstm);
	}
}
