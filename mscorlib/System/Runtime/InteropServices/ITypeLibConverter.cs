using System;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000741 RID: 1857
	[ComVisible(true)]
	[Guid("F1C3BF78-C3E4-11D3-88E7-00902754C43A")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ITypeLibConverter
	{
		// Token: 0x0600413E RID: 16702
		[return: MarshalAs(UnmanagedType.Interface)]
		object ConvertAssemblyToTypeLib(Assembly assembly, string typeLibName, TypeLibExporterFlags flags, ITypeLibExporterNotifySink notifySink);

		// Token: 0x0600413F RID: 16703
		AssemblyBuilder ConvertTypeLibToAssembly([MarshalAs(UnmanagedType.Interface)] object typeLib, string asmFileName, int flags, ITypeLibImporterNotifySink notifySink, byte[] publicKey, StrongNameKeyPair keyPair, bool unsafeInterfaces);

		// Token: 0x06004140 RID: 16704
		AssemblyBuilder ConvertTypeLibToAssembly([MarshalAs(UnmanagedType.Interface)] object typeLib, string asmFileName, TypeLibImporterFlags flags, ITypeLibImporterNotifySink notifySink, byte[] publicKey, StrongNameKeyPair keyPair, string asmNamespace, Version asmVersion);

		// Token: 0x06004141 RID: 16705
		bool GetPrimaryInteropAssembly(Guid g, int major, int minor, int lcid, out string asmName, out string asmCodeBase);
	}
}
