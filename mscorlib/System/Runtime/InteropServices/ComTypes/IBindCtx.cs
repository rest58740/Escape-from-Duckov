using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200079F RID: 1951
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000000e-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IBindCtx
	{
		// Token: 0x060044FB RID: 17659
		void RegisterObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x060044FC RID: 17660
		void RevokeObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x060044FD RID: 17661
		void ReleaseBoundObjects();

		// Token: 0x060044FE RID: 17662
		void SetBindOptions([In] ref BIND_OPTS pbindopts);

		// Token: 0x060044FF RID: 17663
		void GetBindOptions(ref BIND_OPTS pbindopts);

		// Token: 0x06004500 RID: 17664
		void GetRunningObjectTable(out IRunningObjectTable pprot);

		// Token: 0x06004501 RID: 17665
		void RegisterObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x06004502 RID: 17666
		void GetObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] out object ppunk);

		// Token: 0x06004503 RID: 17667
		void EnumObjectParam(out IEnumString ppenum);

		// Token: 0x06004504 RID: 17668
		[PreserveSig]
		int RevokeObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey);
	}
}
