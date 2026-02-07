using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000744 RID: 1860
	[ComVisible(true)]
	[Guid("f1c3bf76-c3e4-11d3-88e7-00902754c43a")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ITypeLibImporterNotifySink
	{
		// Token: 0x06004145 RID: 16709
		void ReportEvent(ImporterEventKind eventKind, int eventCode, string eventMsg);

		// Token: 0x06004146 RID: 16710
		Assembly ResolveRef([MarshalAs(UnmanagedType.Interface)] object typeLib);
	}
}
