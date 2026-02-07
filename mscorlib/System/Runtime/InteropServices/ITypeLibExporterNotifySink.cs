using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000743 RID: 1859
	[Guid("f1c3bf77-c3e4-11d3-88e7-00902754c43a")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ITypeLibExporterNotifySink
	{
		// Token: 0x06004143 RID: 16707
		void ReportEvent(ExporterEventKind eventKind, int eventCode, string eventMsg);

		// Token: 0x06004144 RID: 16708
		[return: MarshalAs(UnmanagedType.Interface)]
		object ResolveRef(Assembly assembly);
	}
}
