using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007AD RID: 1965
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000010-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IRunningObjectTable
	{
		// Token: 0x06004547 RID: 17735
		int Register(int grfFlags, [MarshalAs(UnmanagedType.Interface)] object punkObject, IMoniker pmkObjectName);

		// Token: 0x06004548 RID: 17736
		void Revoke(int dwRegister);

		// Token: 0x06004549 RID: 17737
		[PreserveSig]
		int IsRunning(IMoniker pmkObjectName);

		// Token: 0x0600454A RID: 17738
		[PreserveSig]
		int GetObject(IMoniker pmkObjectName, [MarshalAs(UnmanagedType.Interface)] out object ppunkObject);

		// Token: 0x0600454B RID: 17739
		void NoteChangeTime(int dwRegister, ref FILETIME pfiletime);

		// Token: 0x0600454C RID: 17740
		[PreserveSig]
		int GetTimeOfLastChange(IMoniker pmkObjectName, out FILETIME pfiletime);

		// Token: 0x0600454D RID: 17741
		void EnumRunning(out IEnumMoniker ppenumMoniker);
	}
}
