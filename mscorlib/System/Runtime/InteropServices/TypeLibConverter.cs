using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000752 RID: 1874
	[Guid("f1c3bf79-c3e4-11d3-88e7-00902754c43a")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	public sealed class TypeLibConverter : ITypeLibConverter
	{
		// Token: 0x06004245 RID: 16965 RVA: 0x000479FC File Offset: 0x00045BFC
		[SecuritySafeCritical]
		[MonoTODO("implement")]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public object ConvertAssemblyToTypeLib(Assembly assembly, string strTypeLibName, TypeLibExporterFlags flags, ITypeLibExporterNotifySink notifySink)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("implement")]
		[SecuritySafeCritical]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public AssemblyBuilder ConvertTypeLibToAssembly([MarshalAs(UnmanagedType.Interface)] object typeLib, string asmFileName, int flags, ITypeLibImporterNotifySink notifySink, byte[] publicKey, StrongNameKeyPair keyPair, bool unsafeInterfaces)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x000479FC File Offset: 0x00045BFC
		[SecuritySafeCritical]
		[MonoTODO("implement")]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public AssemblyBuilder ConvertTypeLibToAssembly([MarshalAs(UnmanagedType.Interface)] object typeLib, string asmFileName, TypeLibImporterFlags flags, ITypeLibImporterNotifySink notifySink, byte[] publicKey, StrongNameKeyPair keyPair, string asmNamespace, Version asmVersion)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("implement")]
		public bool GetPrimaryInteropAssembly(Guid g, int major, int minor, int lcid, out string asmName, out string asmCodeBase)
		{
			throw new NotImplementedException();
		}
	}
}
