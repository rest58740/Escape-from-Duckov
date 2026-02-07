using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200075D RID: 1885
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete]
	[Guid("0000010b-0000-0000-c000-000000000046")]
	[ComImport]
	public interface UCOMIPersistFile
	{
		// Token: 0x0600427E RID: 17022
		void GetClassID(out Guid pClassID);

		// Token: 0x0600427F RID: 17023
		[PreserveSig]
		int IsDirty();

		// Token: 0x06004280 RID: 17024
		void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);

		// Token: 0x06004281 RID: 17025
		void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x06004282 RID: 17026
		void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

		// Token: 0x06004283 RID: 17027
		void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
	}
}
