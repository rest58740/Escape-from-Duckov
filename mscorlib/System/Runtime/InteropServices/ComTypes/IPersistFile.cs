using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007AB RID: 1963
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000010b-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IPersistFile
	{
		// Token: 0x06004535 RID: 17717
		void GetClassID(out Guid pClassID);

		// Token: 0x06004536 RID: 17718
		[PreserveSig]
		int IsDirty();

		// Token: 0x06004537 RID: 17719
		void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);

		// Token: 0x06004538 RID: 17720
		void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x06004539 RID: 17721
		void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

		// Token: 0x0600453A RID: 17722
		void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
	}
}
