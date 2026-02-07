using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200075E RID: 1886
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000010-0000-0000-c000-000000000046")]
	[Obsolete]
	[ComImport]
	public interface UCOMIRunningObjectTable
	{
		// Token: 0x06004284 RID: 17028
		void Register(int grfFlags, [MarshalAs(UnmanagedType.Interface)] object punkObject, UCOMIMoniker pmkObjectName, out int pdwRegister);

		// Token: 0x06004285 RID: 17029
		void Revoke(int dwRegister);

		// Token: 0x06004286 RID: 17030
		void IsRunning(UCOMIMoniker pmkObjectName);

		// Token: 0x06004287 RID: 17031
		void GetObject(UCOMIMoniker pmkObjectName, [MarshalAs(UnmanagedType.Interface)] out object ppunkObject);

		// Token: 0x06004288 RID: 17032
		void NoteChangeTime(int dwRegister, ref FILETIME pfiletime);

		// Token: 0x06004289 RID: 17033
		void GetTimeOfLastChange(UCOMIMoniker pmkObjectName, out FILETIME pfiletime);

		// Token: 0x0600428A RID: 17034
		void EnumRunning(out UCOMIEnumMoniker ppenumMoniker);
	}
}
