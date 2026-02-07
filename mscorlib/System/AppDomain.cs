using System;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;
using System.Threading;
using Mono.Security;

namespace System
{
	// Token: 0x02000214 RID: 532
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_AppDomain))]
	[ComVisible(true)]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class AppDomain : MarshalByRefObject, _AppDomain, IEvidenceFactory
	{
		// Token: 0x0600175A RID: 5978 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal static bool IsAppXModel()
		{
			return false;
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal static bool IsAppXDesignMode()
		{
			return false;
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x00004BF9 File Offset: 0x00002DF9
		internal static void CheckReflectionOnlyLoadSupported()
		{
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x00004BF9 File Offset: 0x00002DF9
		internal static void CheckLoadFromSupported()
		{
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x00053955 File Offset: 0x00051B55
		private AppDomain()
		{
		}

		// Token: 0x0600175F RID: 5983
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern AppDomainSetup getSetup();

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06001760 RID: 5984 RVA: 0x0005B246 File Offset: 0x00059446
		private AppDomainSetup SetupInformationNoCopy
		{
			get
			{
				return this.getSetup();
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06001761 RID: 5985 RVA: 0x0005B24E File Offset: 0x0005944E
		public AppDomainSetup SetupInformation
		{
			get
			{
				return new AppDomainSetup(this.getSetup());
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06001762 RID: 5986 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public ApplicationTrust ApplicationTrust
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06001763 RID: 5987 RVA: 0x0005B25C File Offset: 0x0005945C
		public string BaseDirectory
		{
			get
			{
				string applicationBase = this.SetupInformationNoCopy.ApplicationBase;
				if (SecurityManager.SecurityEnabled && applicationBase != null && applicationBase.Length > 0)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, applicationBase).Demand();
				}
				return applicationBase;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x0005B298 File Offset: 0x00059498
		public string RelativeSearchPath
		{
			get
			{
				string privateBinPath = this.SetupInformationNoCopy.PrivateBinPath;
				if (SecurityManager.SecurityEnabled && privateBinPath != null && privateBinPath.Length > 0)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, privateBinPath).Demand();
				}
				return privateBinPath;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06001765 RID: 5989 RVA: 0x0005B2D4 File Offset: 0x000594D4
		public string DynamicDirectory
		{
			[SecuritySafeCritical]
			get
			{
				AppDomainSetup setupInformationNoCopy = this.SetupInformationNoCopy;
				if (setupInformationNoCopy.DynamicBase == null)
				{
					return null;
				}
				string text = Path.Combine(setupInformationNoCopy.DynamicBase, setupInformationNoCopy.ApplicationName);
				if (SecurityManager.SecurityEnabled && text != null && text.Length > 0)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
				}
				return text;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x0005B324 File Offset: 0x00059524
		public bool ShadowCopyFiles
		{
			get
			{
				return this.SetupInformationNoCopy.ShadowCopyFiles == "true";
			}
		}

		// Token: 0x06001767 RID: 5991
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string getFriendlyName();

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06001768 RID: 5992 RVA: 0x0005B33B File Offset: 0x0005953B
		public string FriendlyName
		{
			[SecuritySafeCritical]
			get
			{
				return this.getFriendlyName();
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06001769 RID: 5993 RVA: 0x0005B344 File Offset: 0x00059544
		public Evidence Evidence
		{
			[SecuritySafeCritical]
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
			get
			{
				if (this._evidence == null)
				{
					lock (this)
					{
						Assembly entryAssembly = Assembly.GetEntryAssembly();
						if (entryAssembly == null)
						{
							if (this == AppDomain.DefaultDomain)
							{
								return new Evidence();
							}
							this._evidence = AppDomain.DefaultDomain.Evidence;
						}
						else
						{
							this._evidence = Evidence.GetDefaultHostEvidence(entryAssembly);
						}
					}
				}
				return new Evidence(this._evidence);
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x0600176A RID: 5994 RVA: 0x0005B3CC File Offset: 0x000595CC
		internal IPrincipal DefaultPrincipal
		{
			get
			{
				if (AppDomain._principal == null)
				{
					PrincipalPolicy principalPolicy = this._principalPolicy;
					if (principalPolicy != PrincipalPolicy.UnauthenticatedPrincipal)
					{
						if (principalPolicy == PrincipalPolicy.WindowsPrincipal)
						{
							AppDomain._principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
						}
					}
					else
					{
						AppDomain._principal = new GenericPrincipal(new GenericIdentity(string.Empty, string.Empty), null);
					}
				}
				return AppDomain._principal;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x0600176B RID: 5995 RVA: 0x0005B420 File Offset: 0x00059620
		internal PermissionSet GrantedPermissionSet
		{
			get
			{
				return this._granted;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600176C RID: 5996 RVA: 0x0005B428 File Offset: 0x00059628
		public PermissionSet PermissionSet
		{
			get
			{
				PermissionSet result;
				if ((result = this._granted) == null)
				{
					result = (this._granted = new PermissionSet(PermissionState.Unrestricted));
				}
				return result;
			}
		}

		// Token: 0x0600176D RID: 5997
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AppDomain getCurDomain();

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x0005B44E File Offset: 0x0005964E
		public static AppDomain CurrentDomain
		{
			get
			{
				return AppDomain.getCurDomain();
			}
		}

		// Token: 0x0600176F RID: 5999
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AppDomain getRootDomain();

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06001770 RID: 6000 RVA: 0x0005B458 File Offset: 0x00059658
		internal static AppDomain DefaultDomain
		{
			get
			{
				if (AppDomain.default_domain == null)
				{
					AppDomain rootDomain = AppDomain.getRootDomain();
					if (rootDomain == AppDomain.CurrentDomain)
					{
						AppDomain.default_domain = rootDomain;
					}
					else
					{
						AppDomain.default_domain = (AppDomain)RemotingServices.GetDomainProxy(rootDomain);
					}
				}
				return AppDomain.default_domain;
			}
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x0005B498 File Offset: 0x00059698
		[Obsolete("AppDomain.AppendPrivatePath has been deprecated. Please investigate the use of AppDomainSetup.PrivateBinPath instead.")]
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void AppendPrivatePath(string path)
		{
			if (path == null || path.Length == 0)
			{
				return;
			}
			AppDomainSetup setupInformationNoCopy = this.SetupInformationNoCopy;
			string text = setupInformationNoCopy.PrivateBinPath;
			if (text == null || text.Length == 0)
			{
				setupInformationNoCopy.PrivateBinPath = path;
				return;
			}
			text = text.Trim();
			if (text[text.Length - 1] != Path.PathSeparator)
			{
				text += Path.PathSeparator.ToString();
			}
			setupInformationNoCopy.PrivateBinPath = text + path;
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x0005B50C File Offset: 0x0005970C
		[Obsolete("AppDomain.ClearPrivatePath has been deprecated. Please investigate the use of AppDomainSetup.PrivateBinPath instead.")]
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void ClearPrivatePath()
		{
			this.SetupInformationNoCopy.PrivateBinPath = string.Empty;
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x0005B51E File Offset: 0x0005971E
		[SecurityCritical]
		[Obsolete("Use AppDomainSetup.ShadowCopyDirectories")]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void ClearShadowCopyPath()
		{
			this.SetupInformationNoCopy.ShadowCopyDirectories = string.Empty;
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x0005B530 File Offset: 0x00059730
		public ObjectHandle CreateComInstanceFrom(string assemblyName, string typeName)
		{
			return Activator.CreateComInstanceFrom(assemblyName, typeName);
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x0005B539 File Offset: 0x00059739
		public ObjectHandle CreateComInstanceFrom(string assemblyFile, string typeName, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			return Activator.CreateComInstanceFrom(assemblyFile, typeName, hashValue, hashAlgorithm);
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x0005B545 File Offset: 0x00059745
		internal ObjectHandle InternalCreateInstanceWithNoSecurity(string assemblyName, string typeName)
		{
			return this.CreateInstance(assemblyName, typeName);
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x0005B550 File Offset: 0x00059750
		internal ObjectHandle InternalCreateInstanceWithNoSecurity(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			return this.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x0005B572 File Offset: 0x00059772
		internal ObjectHandle InternalCreateInstanceFromWithNoSecurity(string assemblyName, string typeName)
		{
			return this.CreateInstanceFrom(assemblyName, typeName);
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x0005B57C File Offset: 0x0005977C
		internal ObjectHandle InternalCreateInstanceFromWithNoSecurity(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			return this.CreateInstanceFrom(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x0005B59E File Offset: 0x0005979E
		public ObjectHandle CreateInstance(string assemblyName, string typeName)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			return Activator.CreateInstance(assemblyName, typeName);
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x0005B5B5 File Offset: 0x000597B5
		public ObjectHandle CreateInstance(string assemblyName, string typeName, object[] activationAttributes)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			return Activator.CreateInstance(assemblyName, typeName, activationAttributes);
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x0005B5D0 File Offset: 0x000597D0
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		public ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x0005B600 File Offset: 0x00059800
		public object CreateInstanceAndUnwrap(string assemblyName, string typeName)
		{
			ObjectHandle objectHandle = this.CreateInstance(assemblyName, typeName);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x0005B624 File Offset: 0x00059824
		public object CreateInstanceAndUnwrap(string assemblyName, string typeName, object[] activationAttributes)
		{
			ObjectHandle objectHandle = this.CreateInstance(assemblyName, typeName, activationAttributes);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x0005B648 File Offset: 0x00059848
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		public object CreateInstanceAndUnwrap(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			ObjectHandle objectHandle = this.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x0005B678 File Offset: 0x00059878
		public ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, null);
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x0005B6A8 File Offset: 0x000598A8
		public object CreateInstanceAndUnwrap(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			ObjectHandle objectHandle = this.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x0005B6D4 File Offset: 0x000598D4
		public ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			if (assemblyFile == null)
			{
				throw new ArgumentNullException("assemblyFile");
			}
			return Activator.CreateInstanceFrom(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, null);
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x0005B704 File Offset: 0x00059904
		public object CreateInstanceFromAndUnwrap(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			ObjectHandle objectHandle = this.CreateInstanceFrom(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x0005B730 File Offset: 0x00059930
		public ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName)
		{
			if (assemblyFile == null)
			{
				throw new ArgumentNullException("assemblyFile");
			}
			return Activator.CreateInstanceFrom(assemblyFile, typeName);
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x0005B747 File Offset: 0x00059947
		public ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, object[] activationAttributes)
		{
			if (assemblyFile == null)
			{
				throw new ArgumentNullException("assemblyFile");
			}
			return Activator.CreateInstanceFrom(assemblyFile, typeName, activationAttributes);
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x0005B760 File Offset: 0x00059960
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		public ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			if (assemblyFile == null)
			{
				throw new ArgumentNullException("assemblyFile");
			}
			return Activator.CreateInstanceFrom(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x0005B790 File Offset: 0x00059990
		public object CreateInstanceFromAndUnwrap(string assemblyName, string typeName)
		{
			ObjectHandle objectHandle = this.CreateInstanceFrom(assemblyName, typeName);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x0005B7B4 File Offset: 0x000599B4
		public object CreateInstanceFromAndUnwrap(string assemblyName, string typeName, object[] activationAttributes)
		{
			ObjectHandle objectHandle = this.CreateInstanceFrom(assemblyName, typeName, activationAttributes);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x0005B7D8 File Offset: 0x000599D8
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		public object CreateInstanceFromAndUnwrap(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			ObjectHandle objectHandle = this.CreateInstanceFrom(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
			if (objectHandle == null)
			{
				return null;
			}
			return objectHandle.Unwrap();
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x0005B808 File Offset: 0x00059A08
		[SecuritySafeCritical]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
		{
			return this.DefineDynamicAssembly(name, access, null, null, null, null, null, false);
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x0005B824 File Offset: 0x00059A24
		[Obsolete("Declarative security for assembly level is no longer enforced")]
		[SecuritySafeCritical]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Evidence evidence)
		{
			return this.DefineDynamicAssembly(name, access, null, evidence, null, null, null, false);
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x0005B840 File Offset: 0x00059A40
		[SecuritySafeCritical]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir)
		{
			return this.DefineDynamicAssembly(name, access, dir, null, null, null, null, false);
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x0005B85C File Offset: 0x00059A5C
		[Obsolete("Declarative security for assembly level is no longer enforced")]
		[SecuritySafeCritical]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence)
		{
			return this.DefineDynamicAssembly(name, access, dir, evidence, null, null, null, false);
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x0005B878 File Offset: 0x00059A78
		[Obsolete("Declarative security for assembly level is no longer enforced")]
		[SecuritySafeCritical]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
		{
			return this.DefineDynamicAssembly(name, access, null, null, requiredPermissions, optionalPermissions, refusedPermissions, false);
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x0005B898 File Offset: 0x00059A98
		[Obsolete("Declarative security for assembly level is no longer enforced")]
		[SecuritySafeCritical]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
		{
			return this.DefineDynamicAssembly(name, access, null, evidence, requiredPermissions, optionalPermissions, refusedPermissions, false);
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x0005B8B8 File Offset: 0x00059AB8
		[SecuritySafeCritical]
		[Obsolete("Declarative security for assembly level is no longer enforced")]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
		{
			return this.DefineDynamicAssembly(name, access, dir, null, requiredPermissions, optionalPermissions, refusedPermissions, false);
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x0005B8D8 File Offset: 0x00059AD8
		[Obsolete("Declarative security for assembly level is no longer enforced")]
		[SecuritySafeCritical]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
		{
			return this.DefineDynamicAssembly(name, access, dir, evidence, requiredPermissions, optionalPermissions, refusedPermissions, false);
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x0005B8F7 File Offset: 0x00059AF7
		[Obsolete("Declarative security for assembly level is no longer enforced")]
		[SecuritySafeCritical]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions, bool isSynchronized)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			AppDomain.ValidateAssemblyName(name.Name);
			AssemblyBuilder assemblyBuilder = new AssemblyBuilder(name, dir, access, false);
			assemblyBuilder.AddPermissionRequests(requiredPermissions, optionalPermissions, refusedPermissions);
			return assemblyBuilder;
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x0005B928 File Offset: 0x00059B28
		[Obsolete("Declarative security for assembly level is no longer enforced")]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions, bool isSynchronized, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
		{
			AssemblyBuilder assemblyBuilder = this.DefineDynamicAssembly(name, access, dir, evidence, requiredPermissions, optionalPermissions, refusedPermissions, isSynchronized);
			if (assemblyAttributes != null)
			{
				foreach (CustomAttributeBuilder customAttribute in assemblyAttributes)
				{
					assemblyBuilder.SetCustomAttribute(customAttribute);
				}
			}
			return assemblyBuilder;
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0005B98C File Offset: 0x00059B8C
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
		{
			return this.DefineDynamicAssembly(name, access, null, null, null, null, null, false, assemblyAttributes);
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0005B9A8 File Offset: 0x00059BA8
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, bool isSynchronized, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
		{
			return this.DefineDynamicAssembly(name, access, dir, null, null, null, null, isSynchronized, assemblyAttributes);
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x0005B9C6 File Offset: 0x00059BC6
		[MonoLimitation("The argument securityContextSource is ignored")]
		public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, IEnumerable<CustomAttributeBuilder> assemblyAttributes, SecurityContextSource securityContextSource)
		{
			return this.DefineDynamicAssembly(name, access, assemblyAttributes);
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x0005B9D1 File Offset: 0x00059BD1
		internal AssemblyBuilder DefineInternalDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
		{
			return new AssemblyBuilder(name, null, access, true);
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x0005B9DC File Offset: 0x00059BDC
		public void DoCallBack(CrossAppDomainDelegate callBackDelegate)
		{
			if (callBackDelegate != null)
			{
				callBackDelegate();
			}
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x0005B9E7 File Offset: 0x00059BE7
		public int ExecuteAssembly(string assemblyFile)
		{
			return this.ExecuteAssembly(assemblyFile, null, null);
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x0005B9F2 File Offset: 0x00059BF2
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		public int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity)
		{
			return this.ExecuteAssembly(assemblyFile, assemblySecurity, null);
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x0005BA00 File Offset: 0x00059C00
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		public int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity, string[] args)
		{
			Assembly a = Assembly.LoadFrom(assemblyFile, assemblySecurity);
			return this.ExecuteAssemblyInternal(a, args);
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x0005BA20 File Offset: 0x00059C20
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		public int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity, string[] args, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			Assembly a = Assembly.LoadFrom(assemblyFile, assemblySecurity, hashValue, hashAlgorithm);
			return this.ExecuteAssemblyInternal(a, args);
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x0005BA44 File Offset: 0x00059C44
		public int ExecuteAssembly(string assemblyFile, string[] args)
		{
			Assembly a = Assembly.LoadFrom(assemblyFile, null);
			return this.ExecuteAssemblyInternal(a, args);
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x0005BA64 File Offset: 0x00059C64
		public int ExecuteAssembly(string assemblyFile, string[] args, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			Assembly a = Assembly.LoadFrom(assemblyFile, null, hashValue, hashAlgorithm);
			return this.ExecuteAssemblyInternal(a, args);
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x0005BA84 File Offset: 0x00059C84
		private int ExecuteAssemblyInternal(Assembly a, string[] args)
		{
			if (a.EntryPoint == null)
			{
				throw new MissingMethodException("Entry point not found in assembly '" + a.FullName + "'.");
			}
			return this.ExecuteAssembly(a, args);
		}

		// Token: 0x060017A0 RID: 6048
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int ExecuteAssembly(Assembly a, string[] args);

		// Token: 0x060017A1 RID: 6049
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Assembly[] GetAssemblies(bool refOnly);

		// Token: 0x060017A2 RID: 6050 RVA: 0x0005BAB7 File Offset: 0x00059CB7
		public Assembly[] GetAssemblies()
		{
			return this.GetAssemblies(false);
		}

		// Token: 0x060017A3 RID: 6051
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object GetData(string name);

		// Token: 0x060017A4 RID: 6052 RVA: 0x00047214 File Offset: 0x00045414
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x0000AF5E File Offset: 0x0000915E
		[SecurityCritical]
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x060017A6 RID: 6054
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Assembly LoadAssembly(string assemblyRef, Evidence securityEvidence, bool refOnly, ref StackCrawlMark stackMark);

		// Token: 0x060017A7 RID: 6055 RVA: 0x0005BAC0 File Offset: 0x00059CC0
		[SecuritySafeCritical]
		public Assembly Load(AssemblyName assemblyRef)
		{
			return this.Load(assemblyRef, null);
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x0005BACA File Offset: 0x00059CCA
		internal Assembly LoadSatellite(AssemblyName assemblyRef, bool throwOnError, ref StackCrawlMark stackMark)
		{
			if (assemblyRef == null)
			{
				throw new ArgumentNullException("assemblyRef");
			}
			Assembly assembly = this.LoadAssembly(assemblyRef.FullName, null, false, ref stackMark);
			if (assembly == null && throwOnError)
			{
				throw new FileNotFoundException(null, assemblyRef.Name);
			}
			return assembly;
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x0005BB04 File Offset: 0x00059D04
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Assembly Load(AssemblyName assemblyRef, Evidence assemblySecurity)
		{
			if (assemblyRef == null)
			{
				throw new ArgumentNullException("assemblyRef");
			}
			if (assemblyRef.Name == null || assemblyRef.Name.Length == 0)
			{
				if (assemblyRef.CodeBase != null)
				{
					return Assembly.LoadFrom(assemblyRef.CodeBase, assemblySecurity);
				}
				throw new ArgumentException(Locale.GetText("assemblyRef.Name cannot be empty."), "assemblyRef");
			}
			else
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				Assembly assembly = this.LoadAssembly(assemblyRef.FullName, assemblySecurity, false, ref stackCrawlMark);
				if (assembly != null)
				{
					return assembly;
				}
				if (assemblyRef.CodeBase == null)
				{
					throw new FileNotFoundException(null, assemblyRef.Name);
				}
				string text = assemblyRef.CodeBase;
				if (text.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
				{
					text = new Uri(text).LocalPath;
				}
				try
				{
					assembly = Assembly.LoadFrom(text, assemblySecurity);
				}
				catch
				{
					throw new FileNotFoundException(null, assemblyRef.Name);
				}
				AssemblyName name = assembly.GetName();
				if (assemblyRef.Name != name.Name)
				{
					throw new FileNotFoundException(null, assemblyRef.Name);
				}
				if (assemblyRef.Version != null && assemblyRef.Version != new Version(0, 0, 0, 0) && assemblyRef.Version != name.Version)
				{
					throw new FileNotFoundException(null, assemblyRef.Name);
				}
				if (assemblyRef.CultureInfo != null && assemblyRef.CultureInfo.Equals(name))
				{
					throw new FileNotFoundException(null, assemblyRef.Name);
				}
				byte[] publicKeyToken = assemblyRef.GetPublicKeyToken();
				if (publicKeyToken != null && publicKeyToken.Length != 0)
				{
					byte[] publicKeyToken2 = name.GetPublicKeyToken();
					if (publicKeyToken2 == null || publicKeyToken.Length != publicKeyToken2.Length)
					{
						throw new FileNotFoundException(null, assemblyRef.Name);
					}
					for (int i = publicKeyToken.Length - 1; i >= 0; i--)
					{
						if (publicKeyToken2[i] != publicKeyToken[i])
						{
							throw new FileNotFoundException(null, assemblyRef.Name);
						}
					}
				}
				return assembly;
			}
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x0005BCCC File Offset: 0x00059ECC
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Assembly Load(string assemblyString)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.Load(assemblyString, null, false, ref stackCrawlMark);
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x0005BCE8 File Offset: 0x00059EE8
		[SecuritySafeCritical]
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public Assembly Load(string assemblyString, Evidence assemblySecurity)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.Load(assemblyString, assemblySecurity, false, ref stackCrawlMark);
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x0005BD02 File Offset: 0x00059F02
		internal Assembly Load(string assemblyString, Evidence assemblySecurity, bool refonly, ref StackCrawlMark stackMark)
		{
			if (assemblyString == null)
			{
				throw new ArgumentNullException("assemblyString");
			}
			if (assemblyString.Length == 0)
			{
				throw new ArgumentException("assemblyString cannot have zero length");
			}
			Assembly assembly = this.LoadAssembly(assemblyString, assemblySecurity, refonly, ref stackMark);
			if (assembly == null)
			{
				throw new FileNotFoundException(null, assemblyString);
			}
			return assembly;
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x0005BD41 File Offset: 0x00059F41
		[SecuritySafeCritical]
		public Assembly Load(byte[] rawAssembly)
		{
			return this.Load(rawAssembly, null, null);
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0005BD4C File Offset: 0x00059F4C
		[SecuritySafeCritical]
		public Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore)
		{
			return this.Load(rawAssembly, rawSymbolStore, null);
		}

		// Token: 0x060017AF RID: 6063
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Assembly LoadAssemblyRaw(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence, bool refonly);

		// Token: 0x060017B0 RID: 6064 RVA: 0x0005BD57 File Offset: 0x00059F57
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		[SecuritySafeCritical]
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
		public Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence)
		{
			return this.Load(rawAssembly, rawSymbolStore, securityEvidence, false);
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x0005BD63 File Offset: 0x00059F63
		internal Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence, bool refonly)
		{
			if (rawAssembly == null)
			{
				throw new ArgumentNullException("rawAssembly");
			}
			Assembly assembly = this.LoadAssemblyRaw(rawAssembly, rawSymbolStore, securityEvidence, refonly);
			assembly.FromByteArray = true;
			return assembly;
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x0005BD88 File Offset: 0x00059F88
		[Obsolete("AppDomain policy levels are obsolete")]
		[SecurityCritical]
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public void SetAppDomainPolicy(PolicyLevel domainPolicy)
		{
			if (domainPolicy == null)
			{
				throw new ArgumentNullException("domainPolicy");
			}
			if (this._granted != null)
			{
				throw new PolicyException(Locale.GetText("An AppDomain policy is already specified."));
			}
			if (this.IsFinalizingForUnload())
			{
				throw new AppDomainUnloadedException();
			}
			PolicyStatement policyStatement = domainPolicy.Resolve(this._evidence);
			this._granted = policyStatement.PermissionSet;
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x0005BDE2 File Offset: 0x00059FE2
		[Obsolete("Use AppDomainSetup.SetCachePath")]
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetCachePath(string path)
		{
			this.SetupInformationNoCopy.CachePath = path;
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x0005BDF0 File Offset: 0x00059FF0
		[SecuritySafeCritical]
		[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
		public void SetPrincipalPolicy(PrincipalPolicy policy)
		{
			if (this.IsFinalizingForUnload())
			{
				throw new AppDomainUnloadedException();
			}
			this._principalPolicy = policy;
			AppDomain._principal = null;
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x0005BE0D File Offset: 0x0005A00D
		[Obsolete("Use AppDomainSetup.ShadowCopyFiles")]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetShadowCopyFiles()
		{
			this.SetupInformationNoCopy.ShadowCopyFiles = "true";
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x0005BE1F File Offset: 0x0005A01F
		[Obsolete("Use AppDomainSetup.ShadowCopyDirectories")]
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetShadowCopyPath(string path)
		{
			this.SetupInformationNoCopy.ShadowCopyDirectories = path;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x0005BE2D File Offset: 0x0005A02D
		[SecuritySafeCritical]
		[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
		public void SetThreadPrincipal(IPrincipal principal)
		{
			if (principal == null)
			{
				throw new ArgumentNullException("principal");
			}
			if (AppDomain._principal != null)
			{
				throw new PolicyException(Locale.GetText("principal already present."));
			}
			if (this.IsFinalizingForUnload())
			{
				throw new AppDomainUnloadedException();
			}
			AppDomain._principal = principal;
		}

		// Token: 0x060017B8 RID: 6072
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AppDomain InternalSetDomainByID(int domain_id);

		// Token: 0x060017B9 RID: 6073
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AppDomain InternalSetDomain(AppDomain context);

		// Token: 0x060017BA RID: 6074
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalPushDomainRef(AppDomain domain);

		// Token: 0x060017BB RID: 6075
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalPushDomainRefByID(int domain_id);

		// Token: 0x060017BC RID: 6076
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalPopDomainRef();

		// Token: 0x060017BD RID: 6077
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Context InternalSetContext(Context context);

		// Token: 0x060017BE RID: 6078
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Context InternalGetContext();

		// Token: 0x060017BF RID: 6079
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Context InternalGetDefaultContext();

		// Token: 0x060017C0 RID: 6080
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string InternalGetProcessGuid(string newguid);

		// Token: 0x060017C1 RID: 6081 RVA: 0x0005BE68 File Offset: 0x0005A068
		internal static object InvokeInDomain(AppDomain domain, MethodInfo method, object obj, object[] args)
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			bool flag = false;
			object result;
			try
			{
				AppDomain.InternalPushDomainRef(domain);
				flag = true;
				AppDomain.InternalSetDomain(domain);
				Exception ex;
				object obj2 = ((RuntimeMethodInfo)method).InternalInvoke(obj, args, out ex);
				if (ex != null)
				{
					throw ex;
				}
				result = obj2;
			}
			finally
			{
				AppDomain.InternalSetDomain(currentDomain);
				if (flag)
				{
					AppDomain.InternalPopDomainRef();
				}
			}
			return result;
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x0005BEC4 File Offset: 0x0005A0C4
		internal static object InvokeInDomainByID(int domain_id, MethodInfo method, object obj, object[] args)
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			bool flag = false;
			object result;
			try
			{
				AppDomain.InternalPushDomainRefByID(domain_id);
				flag = true;
				AppDomain.InternalSetDomainByID(domain_id);
				Exception ex;
				object obj2 = ((RuntimeMethodInfo)method).InternalInvoke(obj, args, out ex);
				if (ex != null)
				{
					throw ex;
				}
				result = obj2;
			}
			finally
			{
				AppDomain.InternalSetDomain(currentDomain);
				if (flag)
				{
					AppDomain.InternalPopDomainRef();
				}
			}
			return result;
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x0005BF20 File Offset: 0x0005A120
		internal static string GetProcessGuid()
		{
			if (AppDomain._process_guid == null)
			{
				AppDomain._process_guid = AppDomain.InternalGetProcessGuid(Guid.NewGuid().ToString());
			}
			return AppDomain._process_guid;
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x0005BF56 File Offset: 0x0005A156
		public static AppDomain CreateDomain(string friendlyName)
		{
			return AppDomain.CreateDomain(friendlyName, null, null);
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0005BF60 File Offset: 0x0005A160
		public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo)
		{
			return AppDomain.CreateDomain(friendlyName, securityInfo, null);
		}

		// Token: 0x060017C6 RID: 6086
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AppDomain createDomain(string friendlyName, AppDomainSetup info);

		// Token: 0x060017C7 RID: 6087 RVA: 0x0005BF6C File Offset: 0x0005A16C
		[MonoLimitation("Currently it does not allow the setup in the other domain")]
		[SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
		public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo, AppDomainSetup info)
		{
			if (friendlyName == null)
			{
				throw new ArgumentNullException("friendlyName");
			}
			AppDomain defaultDomain = AppDomain.DefaultDomain;
			if (info == null)
			{
				if (defaultDomain == null)
				{
					info = new AppDomainSetup();
				}
				else
				{
					info = defaultDomain.SetupInformation;
				}
			}
			else
			{
				info = new AppDomainSetup(info);
			}
			if (defaultDomain != null)
			{
				if (!info.Equals(defaultDomain.SetupInformation))
				{
					if (info.ApplicationBase == null)
					{
						info.ApplicationBase = defaultDomain.SetupInformation.ApplicationBase;
					}
					if (info.ConfigurationFile == null)
					{
						info.ConfigurationFile = Path.GetFileName(defaultDomain.SetupInformation.ConfigurationFile);
					}
				}
			}
			else if (info.ConfigurationFile == null)
			{
				info.ConfigurationFile = "[I don't have a config file]";
			}
			if (info.AppDomainInitializer != null && !info.AppDomainInitializer.Method.IsStatic)
			{
				throw new ArgumentException("Non-static methods cannot be invoked as an appdomain initializer");
			}
			info.SerializeNonPrimitives();
			AppDomain appDomain = (AppDomain)RemotingServices.GetDomainProxy(AppDomain.createDomain(friendlyName, info));
			if (securityInfo == null)
			{
				if (defaultDomain == null)
				{
					appDomain._evidence = null;
				}
				else
				{
					appDomain._evidence = defaultDomain.Evidence;
				}
			}
			else
			{
				appDomain._evidence = new Evidence(securityInfo);
			}
			if (info.AppDomainInitializer != null)
			{
				AppDomain.Loader @object = new AppDomain.Loader(info.AppDomainInitializer.Method.DeclaringType.Assembly.Location);
				appDomain.DoCallBack(new CrossAppDomainDelegate(@object.Load));
				AppDomain.Initializer object2 = new AppDomain.Initializer(info.AppDomainInitializer, info.AppDomainInitializerArguments);
				appDomain.DoCallBack(new CrossAppDomainDelegate(object2.Initialize));
			}
			return appDomain;
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0005C0D0 File Offset: 0x0005A2D0
		public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo, string appBasePath, string appRelativeSearchPath, bool shadowCopyFiles)
		{
			return AppDomain.CreateDomain(friendlyName, securityInfo, AppDomain.CreateDomainSetup(appBasePath, appRelativeSearchPath, shadowCopyFiles));
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0005C0E2 File Offset: 0x0005A2E2
		public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo, AppDomainSetup info, PermissionSet grantSet, params System.Security.Policy.StrongName[] fullTrustAssemblies)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.ApplicationTrust = new ApplicationTrust(grantSet, fullTrustAssemblies ?? EmptyArray<System.Security.Policy.StrongName>.Value);
			return AppDomain.CreateDomain(friendlyName, securityInfo, info);
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x0005C114 File Offset: 0x0005A314
		private static AppDomainSetup CreateDomainSetup(string appBasePath, string appRelativeSearchPath, bool shadowCopyFiles)
		{
			AppDomainSetup appDomainSetup = new AppDomainSetup();
			appDomainSetup.ApplicationBase = appBasePath;
			appDomainSetup.PrivateBinPath = appRelativeSearchPath;
			if (shadowCopyFiles)
			{
				appDomainSetup.ShadowCopyFiles = "true";
			}
			else
			{
				appDomainSetup.ShadowCopyFiles = "false";
			}
			return appDomainSetup;
		}

		// Token: 0x060017CB RID: 6091
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool InternalIsFinalizingForUnload(int domain_id);

		// Token: 0x060017CC RID: 6092 RVA: 0x0005C151 File Offset: 0x0005A351
		public bool IsFinalizingForUnload()
		{
			return AppDomain.InternalIsFinalizingForUnload(this.getDomainID());
		}

		// Token: 0x060017CD RID: 6093
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalUnload(int domain_id);

		// Token: 0x060017CE RID: 6094 RVA: 0x0005C15E File Offset: 0x0005A35E
		private int getDomainID()
		{
			return Thread.GetDomainID();
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x0005C165 File Offset: 0x0005A365
		[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.MayFail)]
		[SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
		public static void Unload(AppDomain domain)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			AppDomain.InternalUnload(domain.getDomainID());
		}

		// Token: 0x060017D0 RID: 6096
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetData(string name, object data);

		// Token: 0x060017D1 RID: 6097 RVA: 0x0005C180 File Offset: 0x0005A380
		[MonoLimitation("The permission field is ignored")]
		public void SetData(string name, object data, IPermission permission)
		{
			this.SetData(name, data);
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x0005C18A File Offset: 0x0005A38A
		[Obsolete("Use AppDomainSetup.DynamicBase")]
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetDynamicBase(string path)
		{
			this.SetupInformationNoCopy.DynamicBase = path;
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x0005C198 File Offset: 0x0005A398
		[Obsolete("AppDomain.GetCurrentThreadId has been deprecated because it does not provide a stable Id when managed threads are running on fibers (aka lightweight threads). To get a stable identifier for a managed thread, use the ManagedThreadId property on Thread.'")]
		public static int GetCurrentThreadId()
		{
			return Thread.CurrentThreadId;
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x0005B33B File Offset: 0x0005953B
		[SecuritySafeCritical]
		public override string ToString()
		{
			return this.getFriendlyName();
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x0005C1A0 File Offset: 0x0005A3A0
		private static void ValidateAssemblyName(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException("The Name of AssemblyName cannot be null or a zero-length string.");
			}
			bool flag = true;
			for (int i = 0; i < name.Length; i++)
			{
				char c = name[i];
				if (i == 0 && char.IsWhiteSpace(c))
				{
					flag = false;
					break;
				}
				if (c == '/' || c == '\\' || c == ':')
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				throw new ArgumentException("The Name of AssemblyName cannot start with whitespace, or contain '/', '\\'  or ':'.");
			}
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x0005C20E File Offset: 0x0005A40E
		private void DoAssemblyLoad(Assembly assembly)
		{
			if (this.AssemblyLoad == null)
			{
				return;
			}
			this.AssemblyLoad(this, new AssemblyLoadEventArgs(assembly));
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x0005C22C File Offset: 0x0005A42C
		private Assembly DoAssemblyResolve(string name, Assembly requestingAssembly, bool refonly)
		{
			ResolveEventHandler resolveEventHandler;
			if (refonly)
			{
				resolveEventHandler = this.ReflectionOnlyAssemblyResolve;
			}
			else
			{
				resolveEventHandler = this.AssemblyResolve;
			}
			if (resolveEventHandler == null)
			{
				return null;
			}
			Dictionary<string, object> dictionary;
			if (refonly)
			{
				dictionary = AppDomain.assembly_resolve_in_progress_refonly;
				if (dictionary == null)
				{
					dictionary = new Dictionary<string, object>();
					AppDomain.assembly_resolve_in_progress_refonly = dictionary;
				}
			}
			else
			{
				dictionary = AppDomain.assembly_resolve_in_progress;
				if (dictionary == null)
				{
					dictionary = new Dictionary<string, object>();
					AppDomain.assembly_resolve_in_progress = dictionary;
				}
			}
			if (dictionary.ContainsKey(name))
			{
				return null;
			}
			dictionary[name] = null;
			Assembly result;
			try
			{
				Delegate[] invocationList = resolveEventHandler.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					Assembly assembly = ((ResolveEventHandler)invocationList[i])(this, new ResolveEventArgs(name, requestingAssembly));
					if (assembly != null)
					{
						return assembly;
					}
				}
				result = null;
			}
			finally
			{
				dictionary.Remove(name);
			}
			return result;
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x0005C2F0 File Offset: 0x0005A4F0
		internal Assembly DoTypeBuilderResolve(TypeBuilder tb)
		{
			if (this.TypeResolve == null)
			{
				return null;
			}
			return this.DoTypeResolve(tb.FullName);
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x0005C308 File Offset: 0x0005A508
		internal Assembly DoTypeResolve(string name)
		{
			if (this.TypeResolve == null)
			{
				return null;
			}
			Dictionary<string, object> dictionary = AppDomain.type_resolve_in_progress;
			if (dictionary == null)
			{
				dictionary = (AppDomain.type_resolve_in_progress = new Dictionary<string, object>());
			}
			if (dictionary.ContainsKey(name))
			{
				return null;
			}
			dictionary[name] = null;
			Assembly result;
			try
			{
				Delegate[] invocationList = this.TypeResolve.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					Assembly assembly = ((ResolveEventHandler)invocationList[i])(this, new ResolveEventArgs(name));
					if (assembly != null)
					{
						return assembly;
					}
				}
				result = null;
			}
			finally
			{
				dictionary.Remove(name);
			}
			return result;
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x0005C3A4 File Offset: 0x0005A5A4
		internal Assembly DoResourceResolve(string name, Assembly requesting)
		{
			if (this.ResourceResolve == null)
			{
				return null;
			}
			Delegate[] invocationList = this.ResourceResolve.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				Assembly assembly = ((ResolveEventHandler)invocationList[i])(this, new ResolveEventArgs(name, requesting));
				if (assembly != null)
				{
					return assembly;
				}
			}
			return null;
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x0005C3F7 File Offset: 0x0005A5F7
		private void DoDomainUnload()
		{
			if (this.DomainUnload != null)
			{
				this.DomainUnload(this, null);
			}
		}

		// Token: 0x060017DC RID: 6108
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void DoUnhandledException(Exception e);

		// Token: 0x060017DD RID: 6109 RVA: 0x0005C40E File Offset: 0x0005A60E
		internal void DoUnhandledException(UnhandledExceptionEventArgs args)
		{
			if (this.UnhandledException != null)
			{
				this.UnhandledException(this, args);
			}
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x0005C425 File Offset: 0x0005A625
		internal byte[] GetMarshalledDomainObjRef()
		{
			return CADSerializer.SerializeObject(RemotingServices.Marshal(AppDomain.CurrentDomain, null, typeof(AppDomain))).GetBuffer();
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0005C448 File Offset: 0x0005A648
		internal void ProcessMessageInDomain(byte[] arrRequest, CADMethodCallMessage cadMsg, out byte[] arrResponse, out CADMethodReturnMessage cadMrm)
		{
			IMessage msg;
			if (arrRequest != null)
			{
				msg = CADSerializer.DeserializeMessage(new MemoryStream(arrRequest), null);
			}
			else
			{
				msg = new MethodCall(cadMsg);
			}
			IMessage message = ChannelServices.SyncDispatchMessage(msg);
			cadMrm = CADMethodReturnMessage.Create(message);
			if (cadMrm == null)
			{
				arrResponse = CADSerializer.SerializeMessage(message).GetBuffer();
				return;
			}
			arrResponse = null;
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060017E0 RID: 6112 RVA: 0x0005C494 File Offset: 0x0005A694
		// (remove) Token: 0x060017E1 RID: 6113 RVA: 0x0005C4CC File Offset: 0x0005A6CC
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event AssemblyLoadEventHandler AssemblyLoad;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060017E2 RID: 6114 RVA: 0x0005C504 File Offset: 0x0005A704
		// (remove) Token: 0x060017E3 RID: 6115 RVA: 0x0005C53C File Offset: 0x0005A73C
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event ResolveEventHandler AssemblyResolve;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060017E4 RID: 6116 RVA: 0x0005C574 File Offset: 0x0005A774
		// (remove) Token: 0x060017E5 RID: 6117 RVA: 0x0005C5AC File Offset: 0x0005A7AC
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event EventHandler DomainUnload;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060017E6 RID: 6118 RVA: 0x0005C5E4 File Offset: 0x0005A7E4
		// (remove) Token: 0x060017E7 RID: 6119 RVA: 0x0005C61C File Offset: 0x0005A81C
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event EventHandler ProcessExit;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060017E8 RID: 6120 RVA: 0x0005C654 File Offset: 0x0005A854
		// (remove) Token: 0x060017E9 RID: 6121 RVA: 0x0005C68C File Offset: 0x0005A88C
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event ResolveEventHandler ResourceResolve;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060017EA RID: 6122 RVA: 0x0005C6C4 File Offset: 0x0005A8C4
		// (remove) Token: 0x060017EB RID: 6123 RVA: 0x0005C6FC File Offset: 0x0005A8FC
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event ResolveEventHandler TypeResolve;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060017EC RID: 6124 RVA: 0x0005C734 File Offset: 0x0005A934
		// (remove) Token: 0x060017ED RID: 6125 RVA: 0x0005C76C File Offset: 0x0005A96C
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event UnhandledExceptionEventHandler UnhandledException;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060017EE RID: 6126 RVA: 0x0005C7A4 File Offset: 0x0005A9A4
		// (remove) Token: 0x060017EF RID: 6127 RVA: 0x0005C7DC File Offset: 0x0005A9DC
		public event EventHandler<FirstChanceExceptionEventArgs> FirstChanceException;

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060017F0 RID: 6128 RVA: 0x000040F7 File Offset: 0x000022F7
		[MonoTODO]
		public bool IsHomogenous
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060017F1 RID: 6129 RVA: 0x000040F7 File Offset: 0x000022F7
		[MonoTODO]
		public bool IsFullyTrusted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060017F2 RID: 6130 RVA: 0x0005C811 File Offset: 0x0005AA11
		public AppDomainManager DomainManager
		{
			get
			{
				return this._domain_manager;
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060017F3 RID: 6131 RVA: 0x0005C81C File Offset: 0x0005AA1C
		// (remove) Token: 0x060017F4 RID: 6132 RVA: 0x0005C854 File Offset: 0x0005AA54
		public event ResolveEventHandler ReflectionOnlyAssemblyResolve;

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060017F5 RID: 6133 RVA: 0x0005C889 File Offset: 0x0005AA89
		public ActivationContext ActivationContext
		{
			get
			{
				return this._activation;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060017F6 RID: 6134 RVA: 0x0005C891 File Offset: 0x0005AA91
		public ApplicationIdentity ApplicationIdentity
		{
			get
			{
				return this._applicationIdentity;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060017F7 RID: 6135 RVA: 0x0005C899 File Offset: 0x0005AA99
		public int Id
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.getDomainID();
			}
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x0005C8A1 File Offset: 0x0005AAA1
		[ComVisible(false)]
		[MonoTODO("This routine only returns the parameter currently")]
		public string ApplyPolicy(string assemblyName)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (assemblyName.Length == 0)
			{
				throw new ArgumentException("assemblyName");
			}
			return assemblyName;
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x0005C8C8 File Offset: 0x0005AAC8
		public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo, string appBasePath, string appRelativeSearchPath, bool shadowCopyFiles, AppDomainInitializer adInit, string[] adInitArgs)
		{
			AppDomainSetup appDomainSetup = AppDomain.CreateDomainSetup(appBasePath, appRelativeSearchPath, shadowCopyFiles);
			appDomainSetup.AppDomainInitializerArguments = adInitArgs;
			appDomainSetup.AppDomainInitializer = adInit;
			return AppDomain.CreateDomain(friendlyName, securityInfo, appDomainSetup);
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x0005C8F7 File Offset: 0x0005AAF7
		public int ExecuteAssemblyByName(string assemblyName)
		{
			return this.ExecuteAssemblyByName(assemblyName, null, null);
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x0005C902 File Offset: 0x0005AB02
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		public int ExecuteAssemblyByName(string assemblyName, Evidence assemblySecurity)
		{
			return this.ExecuteAssemblyByName(assemblyName, assemblySecurity, null);
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x0005C910 File Offset: 0x0005AB10
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		public int ExecuteAssemblyByName(string assemblyName, Evidence assemblySecurity, params string[] args)
		{
			Assembly a = Assembly.Load(assemblyName, assemblySecurity);
			return this.ExecuteAssemblyInternal(a, args);
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x0005C930 File Offset: 0x0005AB30
		[Obsolete("Use an overload that does not take an Evidence parameter")]
		public int ExecuteAssemblyByName(AssemblyName assemblyName, Evidence assemblySecurity, params string[] args)
		{
			Assembly a = Assembly.Load(assemblyName, assemblySecurity);
			return this.ExecuteAssemblyInternal(a, args);
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x0005C950 File Offset: 0x0005AB50
		public int ExecuteAssemblyByName(string assemblyName, params string[] args)
		{
			Assembly a = Assembly.Load(assemblyName, null);
			return this.ExecuteAssemblyInternal(a, args);
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x0005C970 File Offset: 0x0005AB70
		public int ExecuteAssemblyByName(AssemblyName assemblyName, params string[] args)
		{
			Assembly a = Assembly.Load(assemblyName, null);
			return this.ExecuteAssemblyInternal(a, args);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x0005C98D File Offset: 0x0005AB8D
		public bool IsDefaultAppDomain()
		{
			return this == AppDomain.DefaultDomain;
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x0005C997 File Offset: 0x0005AB97
		public Assembly[] ReflectionOnlyGetAssemblies()
		{
			return this.GetAssemblies(true);
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x000479FC File Offset: 0x00045BFC
		void _AppDomain.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x000479FC File Offset: 0x00045BFC
		void _AppDomain.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x000479FC File Offset: 0x00045BFC
		void _AppDomain.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x000479FC File Offset: 0x00045BFC
		void _AppDomain.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x0005C9A0 File Offset: 0x0005ABA0
		public bool? IsCompatibilitySwitchSet(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return new bool?(this.compatibility_switch != null && this.compatibility_switch.Contains(value));
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x0005C9CC File Offset: 0x0005ABCC
		internal void SetCompatibilitySwitch(string value)
		{
			if (this.compatibility_switch == null)
			{
				this.compatibility_switch = new List<string>();
			}
			this.compatibility_switch.Add(value);
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06001808 RID: 6152 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		// (set) Token: 0x06001809 RID: 6153 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("Currently always returns false")]
		public static bool MonitoringIsEnabled
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600180A RID: 6154 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public long MonitoringSurvivedMemorySize
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x0600180B RID: 6155 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public static long MonitoringSurvivedProcessMemorySize
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x0600180C RID: 6156 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public long MonitoringTotalAllocatedMemorySize
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600180D RID: 6157 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public TimeSpan MonitoringTotalProcessorTime
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x04001654 RID: 5716
		private IntPtr _mono_app_domain;

		// Token: 0x04001655 RID: 5717
		private static string _process_guid;

		// Token: 0x04001656 RID: 5718
		[ThreadStatic]
		private static Dictionary<string, object> type_resolve_in_progress;

		// Token: 0x04001657 RID: 5719
		[ThreadStatic]
		private static Dictionary<string, object> assembly_resolve_in_progress;

		// Token: 0x04001658 RID: 5720
		[ThreadStatic]
		private static Dictionary<string, object> assembly_resolve_in_progress_refonly;

		// Token: 0x04001659 RID: 5721
		private Evidence _evidence;

		// Token: 0x0400165A RID: 5722
		private PermissionSet _granted;

		// Token: 0x0400165B RID: 5723
		private PrincipalPolicy _principalPolicy;

		// Token: 0x0400165C RID: 5724
		[ThreadStatic]
		private static IPrincipal _principal;

		// Token: 0x0400165D RID: 5725
		private static AppDomain default_domain;

		// Token: 0x04001666 RID: 5734
		private AppDomainManager _domain_manager;

		// Token: 0x04001668 RID: 5736
		private ActivationContext _activation;

		// Token: 0x04001669 RID: 5737
		private ApplicationIdentity _applicationIdentity;

		// Token: 0x0400166A RID: 5738
		private List<string> compatibility_switch;

		// Token: 0x02000215 RID: 533
		[Serializable]
		private class Loader
		{
			// Token: 0x0600180E RID: 6158 RVA: 0x0005C9ED File Offset: 0x0005ABED
			public Loader(string assembly)
			{
				this.assembly = assembly;
			}

			// Token: 0x0600180F RID: 6159 RVA: 0x0005C9FC File Offset: 0x0005ABFC
			public void Load()
			{
				Assembly.LoadFrom(this.assembly);
			}

			// Token: 0x0400166B RID: 5739
			private string assembly;
		}

		// Token: 0x02000216 RID: 534
		[Serializable]
		private class Initializer
		{
			// Token: 0x06001810 RID: 6160 RVA: 0x0005CA0A File Offset: 0x0005AC0A
			public Initializer(AppDomainInitializer initializer, string[] arguments)
			{
				this.initializer = initializer;
				this.arguments = arguments;
			}

			// Token: 0x06001811 RID: 6161 RVA: 0x0005CA20 File Offset: 0x0005AC20
			public void Initialize()
			{
				this.initializer(this.arguments);
			}

			// Token: 0x0400166C RID: 5740
			private AppDomainInitializer initializer;

			// Token: 0x0400166D RID: 5741
			private string[] arguments;
		}
	}
}
