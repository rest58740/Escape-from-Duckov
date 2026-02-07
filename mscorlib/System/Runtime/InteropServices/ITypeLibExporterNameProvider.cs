using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000742 RID: 1858
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("fa1f3615-acb9-486d-9eac-1bef87e36b09")]
	[ComVisible(true)]
	public interface ITypeLibExporterNameProvider
	{
		// Token: 0x06004142 RID: 16706
		[return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
		string[] GetNames();
	}
}
