using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000755 RID: 1877
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete]
	[Guid("0000000e-0000-0000-c000-000000000046")]
	[ComImport]
	public interface UCOMIBindCtx
	{
		// Token: 0x06004249 RID: 16969
		void RegisterObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x0600424A RID: 16970
		void RevokeObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x0600424B RID: 16971
		void ReleaseBoundObjects();

		// Token: 0x0600424C RID: 16972
		void SetBindOptions([In] ref BIND_OPTS pbindopts);

		// Token: 0x0600424D RID: 16973
		void GetBindOptions(ref BIND_OPTS pbindopts);

		// Token: 0x0600424E RID: 16974
		void GetRunningObjectTable(out UCOMIRunningObjectTable pprot);

		// Token: 0x0600424F RID: 16975
		void RegisterObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x06004250 RID: 16976
		void GetObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] out object ppunk);

		// Token: 0x06004251 RID: 16977
		void EnumObjectParam(out UCOMIEnumString ppenum);

		// Token: 0x06004252 RID: 16978
		void RevokeObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey);
	}
}
