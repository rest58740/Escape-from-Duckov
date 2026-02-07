using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Policy;
using System.Security.Principal;

namespace System
{
	// Token: 0x02000202 RID: 514
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Guid("05F696DC-2B29-3663-AD8B-C4389CF2A713")]
	public interface _AppDomain
	{
		// Token: 0x06001618 RID: 5656
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001619 RID: 5657
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x0600161A RID: 5658
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x0600161B RID: 5659
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x0600161C RID: 5660
		string ToString();

		// Token: 0x0600161D RID: 5661
		bool Equals(object other);

		// Token: 0x0600161E RID: 5662
		int GetHashCode();

		// Token: 0x0600161F RID: 5663
		Type GetType();

		// Token: 0x06001620 RID: 5664
		[SecurityCritical]
		object InitializeLifetimeService();

		// Token: 0x06001621 RID: 5665
		[SecurityCritical]
		object GetLifetimeService();

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06001622 RID: 5666
		// (remove) Token: 0x06001623 RID: 5667
		event EventHandler DomainUnload;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06001624 RID: 5668
		// (remove) Token: 0x06001625 RID: 5669
		event AssemblyLoadEventHandler AssemblyLoad;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06001626 RID: 5670
		// (remove) Token: 0x06001627 RID: 5671
		event EventHandler ProcessExit;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06001628 RID: 5672
		// (remove) Token: 0x06001629 RID: 5673
		event ResolveEventHandler TypeResolve;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600162A RID: 5674
		// (remove) Token: 0x0600162B RID: 5675
		event ResolveEventHandler ResourceResolve;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600162C RID: 5676
		// (remove) Token: 0x0600162D RID: 5677
		event ResolveEventHandler AssemblyResolve;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600162E RID: 5678
		// (remove) Token: 0x0600162F RID: 5679
		event UnhandledExceptionEventHandler UnhandledException;

		// Token: 0x06001630 RID: 5680
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access);

		// Token: 0x06001631 RID: 5681
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir);

		// Token: 0x06001632 RID: 5682
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Evidence evidence);

		// Token: 0x06001633 RID: 5683
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions);

		// Token: 0x06001634 RID: 5684
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence);

		// Token: 0x06001635 RID: 5685
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions);

		// Token: 0x06001636 RID: 5686
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions);

		// Token: 0x06001637 RID: 5687
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions);

		// Token: 0x06001638 RID: 5688
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions, bool isSynchronized);

		// Token: 0x06001639 RID: 5689
		ObjectHandle CreateInstance(string assemblyName, string typeName);

		// Token: 0x0600163A RID: 5690
		ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName);

		// Token: 0x0600163B RID: 5691
		ObjectHandle CreateInstance(string assemblyName, string typeName, object[] activationAttributes);

		// Token: 0x0600163C RID: 5692
		ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, object[] activationAttributes);

		// Token: 0x0600163D RID: 5693
		ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes);

		// Token: 0x0600163E RID: 5694
		ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes);

		// Token: 0x0600163F RID: 5695
		Assembly Load(AssemblyName assemblyRef);

		// Token: 0x06001640 RID: 5696
		Assembly Load(string assemblyString);

		// Token: 0x06001641 RID: 5697
		Assembly Load(byte[] rawAssembly);

		// Token: 0x06001642 RID: 5698
		Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore);

		// Token: 0x06001643 RID: 5699
		Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence);

		// Token: 0x06001644 RID: 5700
		Assembly Load(AssemblyName assemblyRef, Evidence assemblySecurity);

		// Token: 0x06001645 RID: 5701
		Assembly Load(string assemblyString, Evidence assemblySecurity);

		// Token: 0x06001646 RID: 5702
		int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity);

		// Token: 0x06001647 RID: 5703
		int ExecuteAssembly(string assemblyFile);

		// Token: 0x06001648 RID: 5704
		int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity, string[] args);

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06001649 RID: 5705
		string FriendlyName { get; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600164A RID: 5706
		string BaseDirectory { get; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600164B RID: 5707
		string RelativeSearchPath { get; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600164C RID: 5708
		bool ShadowCopyFiles { get; }

		// Token: 0x0600164D RID: 5709
		Assembly[] GetAssemblies();

		// Token: 0x0600164E RID: 5710
		[SecurityCritical]
		void AppendPrivatePath(string path);

		// Token: 0x0600164F RID: 5711
		[SecurityCritical]
		void ClearPrivatePath();

		// Token: 0x06001650 RID: 5712
		[SecurityCritical]
		void SetShadowCopyPath(string s);

		// Token: 0x06001651 RID: 5713
		[SecurityCritical]
		void ClearShadowCopyPath();

		// Token: 0x06001652 RID: 5714
		[SecurityCritical]
		void SetCachePath(string s);

		// Token: 0x06001653 RID: 5715
		[SecurityCritical]
		void SetData(string name, object data);

		// Token: 0x06001654 RID: 5716
		object GetData(string name);

		// Token: 0x06001655 RID: 5717
		void DoCallBack(CrossAppDomainDelegate theDelegate);

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001656 RID: 5718
		string DynamicDirectory { get; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06001657 RID: 5719
		Evidence Evidence { get; }

		// Token: 0x06001658 RID: 5720
		[SecurityCritical]
		void SetAppDomainPolicy(PolicyLevel domainPolicy);

		// Token: 0x06001659 RID: 5721
		void SetPrincipalPolicy(PrincipalPolicy policy);

		// Token: 0x0600165A RID: 5722
		void SetThreadPrincipal(IPrincipal principal);
	}
}
