using System;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;
using Mono;

namespace System.Reflection
{
	// Token: 0x020008E8 RID: 2280
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_Assembly))]
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public abstract class Assembly : ICustomAttributeProvider, _Assembly, IEvidenceFactory, ISerializable
	{
		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06004BE3 RID: 19427 RVA: 0x000479FC File Offset: 0x00045BFC
		// (remove) Token: 0x06004BE4 RID: 19428 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual event ModuleResolveEventHandler ModuleResolve
		{
			[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
			add
			{
				throw new NotImplementedException();
			}
			[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
			remove
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06004BE5 RID: 19429 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual string CodeBase
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual string EscapedCodeBase
		{
			[SecuritySafeCritical]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06004BE7 RID: 19431 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual string FullName
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06004BE8 RID: 19432 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual MethodInfo EntryPoint
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06004BE9 RID: 19433 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual Evidence Evidence
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004BEA RID: 19434 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual Evidence UnprotectedGetEvidence()
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06004BEB RID: 19435 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual IntPtr MonoAssembly
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (set) Token: 0x06004BEC RID: 19436 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual bool FromByteArray
		{
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x06004BED RID: 19437 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual string Location
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06004BEE RID: 19438 RVA: 0x000479FC File Offset: 0x00045BFC
		[ComVisible(false)]
		public virtual string ImageRuntimeVersion
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x000479FC File Offset: 0x00045BFC
		[SecurityCritical]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BF2 RID: 19442 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BF3 RID: 19443 RVA: 0x000F19F1 File Offset: 0x000EFBF1
		public virtual FileStream[] GetFiles()
		{
			return this.GetFiles(false);
		}

		// Token: 0x06004BF4 RID: 19444 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual FileStream[] GetFiles(bool getResourceModules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BF5 RID: 19445 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual FileStream GetFile(string name)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BF6 RID: 19446 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual Stream GetManifestResourceStream(string name)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual Stream GetManifestResourceStream(Type type, string name)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BF8 RID: 19448 RVA: 0x000F19FC File Offset: 0x000EFBFC
		internal Stream GetManifestResourceStream(Type type, string name, bool skipSecurityCheck, ref StackCrawlMark stackMark)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (type == null)
			{
				if (name == null)
				{
					throw new ArgumentNullException("type");
				}
			}
			else
			{
				string @namespace = type.Namespace;
				if (@namespace != null)
				{
					stringBuilder.Append(@namespace);
					if (name != null)
					{
						stringBuilder.Append(Type.Delimiter);
					}
				}
			}
			if (name != null)
			{
				stringBuilder.Append(name);
			}
			return this.GetManifestResourceStream(stringBuilder.ToString());
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x000F1A5E File Offset: 0x000EFC5E
		internal Stream GetManifestResourceStream(string name, ref StackCrawlMark stackMark, bool skipSecurityCheck)
		{
			return this.GetManifestResourceStream(null, name, skipSecurityCheck, ref stackMark);
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x000F1A6A File Offset: 0x000EFC6A
		internal string GetSimpleName()
		{
			return this.GetName(true).Name;
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x000F1A78 File Offset: 0x000EFC78
		internal byte[] GetPublicKey()
		{
			return this.GetName(true).GetPublicKey();
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x000F1A86 File Offset: 0x000EFC86
		internal Version GetVersion()
		{
			return this.GetName(true).Version;
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x000F1A94 File Offset: 0x000EFC94
		private AssemblyNameFlags GetFlags()
		{
			return this.GetName(true).Flags;
		}

		// Token: 0x06004BFE RID: 19454
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal virtual extern Type[] GetTypes(bool exportedOnly);

		// Token: 0x06004BFF RID: 19455 RVA: 0x000F1AA2 File Offset: 0x000EFCA2
		public virtual Type[] GetTypes()
		{
			return this.GetTypes(false);
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual Type[] GetExportedTypes()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x000F1AAB File Offset: 0x000EFCAB
		public virtual Type GetType(string name, bool throwOnError)
		{
			return this.GetType(name, throwOnError, false);
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x000F1AB6 File Offset: 0x000EFCB6
		public virtual Type GetType(string name)
		{
			return this.GetType(name, false, false);
		}

		// Token: 0x06004C03 RID: 19459
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Type InternalGetType(Module module, string name, bool throwOnError, bool ignoreCase);

		// Token: 0x06004C04 RID: 19460
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalGetAssemblyName(string assemblyFile, out MonoAssemblyName aname, out string codebase);

		// Token: 0x06004C05 RID: 19461 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual AssemblyName GetName(bool copiedName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C06 RID: 19462 RVA: 0x000F1AC1 File Offset: 0x000EFCC1
		public virtual AssemblyName GetName()
		{
			return this.GetName(false);
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x00097E3F File Offset: 0x0009603F
		public override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x06004C08 RID: 19464 RVA: 0x000F1ACA File Offset: 0x000EFCCA
		public static string CreateQualifiedName(string assemblyName, string typeName)
		{
			return typeName + ", " + assemblyName;
		}

		// Token: 0x06004C09 RID: 19465 RVA: 0x000F1AD8 File Offset: 0x000EFCD8
		public static Assembly GetAssembly(Type type)
		{
			if (type != null)
			{
				return type.Assembly;
			}
			throw new ArgumentNullException("type");
		}

		// Token: 0x06004C0A RID: 19466
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Assembly GetEntryAssembly();

		// Token: 0x06004C0B RID: 19467 RVA: 0x000F1AF4 File Offset: 0x000EFCF4
		internal Assembly GetSatelliteAssembly(CultureInfo culture, Version version, bool throwOnError, ref StackCrawlMark stackMark)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			string name = this.GetSimpleName() + ".resources";
			return this.InternalGetSatelliteAssembly(name, culture, version, true, ref stackMark);
		}

		// Token: 0x06004C0C RID: 19468 RVA: 0x000F1B2C File Offset: 0x000EFD2C
		internal RuntimeAssembly InternalGetSatelliteAssembly(string name, CultureInfo culture, Version version, bool throwOnFileNotFound, ref StackCrawlMark stackMark)
		{
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.SetPublicKey(this.GetPublicKey());
			assemblyName.Flags = (this.GetFlags() | AssemblyNameFlags.PublicKey);
			if (version == null)
			{
				assemblyName.Version = this.GetVersion();
			}
			else
			{
				assemblyName.Version = version;
			}
			assemblyName.CultureInfo = culture;
			assemblyName.Name = name;
			try
			{
				Assembly assembly = AppDomain.CurrentDomain.LoadSatellite(assemblyName, false, ref stackMark);
				if (assembly != null)
				{
					return (RuntimeAssembly)assembly;
				}
			}
			catch (FileNotFoundException)
			{
			}
			if (string.IsNullOrEmpty(this.Location))
			{
				return null;
			}
			string text = Path.Combine(Path.GetDirectoryName(this.Location), Path.Combine(culture.Name, assemblyName.Name + ".dll"));
			RuntimeAssembly result;
			try
			{
				result = (RuntimeAssembly)Assembly.LoadFrom(text, false, ref stackMark);
			}
			catch
			{
				if (throwOnFileNotFound || File.Exists(text))
				{
					throw;
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06004C0D RID: 19469 RVA: 0x00047214 File Offset: 0x00045414
		Type _Assembly.GetType()
		{
			return base.GetType();
		}

		// Token: 0x06004C0E RID: 19470
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Assembly LoadFrom(string assemblyFile, bool refOnly, ref StackCrawlMark stackMark);

		// Token: 0x06004C0F RID: 19471
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Assembly LoadFile_internal(string assemblyFile, ref StackCrawlMark stackMark);

		// Token: 0x06004C10 RID: 19472 RVA: 0x000F1C2C File Offset: 0x000EFE2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly LoadFrom(string assemblyFile)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.LoadFrom(assemblyFile, false, ref stackCrawlMark);
		}

		// Token: 0x06004C11 RID: 19473 RVA: 0x000F1C44 File Offset: 0x000EFE44
		[Obsolete]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly LoadFrom(string assemblyFile, Evidence securityEvidence)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			Assembly assembly = Assembly.LoadFrom(assemblyFile, false, ref stackCrawlMark);
			if (assembly != null && securityEvidence != null)
			{
				assembly.Evidence.Merge(securityEvidence);
			}
			return assembly;
		}

		// Token: 0x06004C12 RID: 19474 RVA: 0x000479FC File Offset: 0x00045BFC
		[Obsolete]
		[MonoTODO("This overload is not currently implemented")]
		public static Assembly LoadFrom(string assemblyFile, Evidence securityEvidence, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C13 RID: 19475 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public static Assembly LoadFrom(string assemblyFile, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C14 RID: 19476 RVA: 0x000F1C78 File Offset: 0x000EFE78
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly UnsafeLoadFrom(string assemblyFile)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.LoadFrom(assemblyFile, false, ref stackCrawlMark);
		}

		// Token: 0x06004C15 RID: 19477 RVA: 0x000F1C90 File Offset: 0x000EFE90
		[Obsolete]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly LoadFile(string path, Evidence securityEvidence)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path == string.Empty)
			{
				throw new ArgumentException("Path can't be empty", "path");
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			Assembly assembly = Assembly.LoadFile_internal(path, ref stackCrawlMark);
			if (assembly != null && securityEvidence != null)
			{
				throw new NotImplementedException();
			}
			return assembly;
		}

		// Token: 0x06004C16 RID: 19478 RVA: 0x000F1CE4 File Offset: 0x000EFEE4
		public static Assembly LoadFile(string path)
		{
			return Assembly.LoadFile(path, null);
		}

		// Token: 0x06004C17 RID: 19479 RVA: 0x000F1CED File Offset: 0x000EFEED
		public static Assembly Load(string assemblyString)
		{
			return AppDomain.CurrentDomain.Load(assemblyString);
		}

		// Token: 0x06004C18 RID: 19480 RVA: 0x000F1CFA File Offset: 0x000EFEFA
		[Obsolete]
		public static Assembly Load(string assemblyString, Evidence assemblySecurity)
		{
			return AppDomain.CurrentDomain.Load(assemblyString, assemblySecurity);
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x000F1D08 File Offset: 0x000EFF08
		public static Assembly Load(AssemblyName assemblyRef)
		{
			return AppDomain.CurrentDomain.Load(assemblyRef);
		}

		// Token: 0x06004C1A RID: 19482 RVA: 0x000F1D15 File Offset: 0x000EFF15
		[Obsolete]
		public static Assembly Load(AssemblyName assemblyRef, Evidence assemblySecurity)
		{
			return AppDomain.CurrentDomain.Load(assemblyRef, assemblySecurity);
		}

		// Token: 0x06004C1B RID: 19483 RVA: 0x000F1D23 File Offset: 0x000EFF23
		public static Assembly Load(byte[] rawAssembly)
		{
			return AppDomain.CurrentDomain.Load(rawAssembly);
		}

		// Token: 0x06004C1C RID: 19484 RVA: 0x000F1D30 File Offset: 0x000EFF30
		public static Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore)
		{
			return AppDomain.CurrentDomain.Load(rawAssembly, rawSymbolStore);
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x000F1D3E File Offset: 0x000EFF3E
		[Obsolete]
		public static Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence)
		{
			return AppDomain.CurrentDomain.Load(rawAssembly, rawSymbolStore, securityEvidence);
		}

		// Token: 0x06004C1E RID: 19486 RVA: 0x000F1D30 File Offset: 0x000EFF30
		[MonoLimitation("Argument securityContextSource is ignored")]
		public static Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, SecurityContextSource securityContextSource)
		{
			return AppDomain.CurrentDomain.Load(rawAssembly, rawSymbolStore);
		}

		// Token: 0x06004C1F RID: 19487 RVA: 0x000F1D4D File Offset: 0x000EFF4D
		public static Assembly ReflectionOnlyLoad(byte[] rawAssembly)
		{
			return AppDomain.CurrentDomain.Load(rawAssembly, null, null, true);
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x000F1D60 File Offset: 0x000EFF60
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly ReflectionOnlyLoad(string assemblyString)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return AppDomain.CurrentDomain.Load(assemblyString, null, true, ref stackCrawlMark);
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x000F1D80 File Offset: 0x000EFF80
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly ReflectionOnlyLoadFrom(string assemblyFile)
		{
			if (assemblyFile == null)
			{
				throw new ArgumentNullException("assemblyFile");
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.LoadFrom(assemblyFile, true, ref stackCrawlMark);
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x000F1DA6 File Offset: 0x000EFFA6
		[Obsolete("This method has been deprecated. Please use Assembly.Load() instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static Assembly LoadWithPartialName(string partialName)
		{
			return Assembly.LoadWithPartialName(partialName, null);
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("Not implemented")]
		public Module LoadModule(string moduleName, byte[] rawModule)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("Not implemented")]
		public virtual Module LoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C25 RID: 19493
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Assembly load_with_partial_name(string name, Evidence e);

		// Token: 0x06004C26 RID: 19494 RVA: 0x000F1DAF File Offset: 0x000EFFAF
		[Obsolete("This method has been deprecated. Please use Assembly.Load() instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static Assembly LoadWithPartialName(string partialName, Evidence securityEvidence)
		{
			return Assembly.LoadWithPartialName(partialName, securityEvidence, true);
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x000F1DB9 File Offset: 0x000EFFB9
		internal static Assembly LoadWithPartialName(string partialName, Evidence securityEvidence, bool oldBehavior)
		{
			if (!oldBehavior)
			{
				throw new NotImplementedException();
			}
			if (partialName == null)
			{
				throw new NullReferenceException();
			}
			return Assembly.load_with_partial_name(partialName, securityEvidence);
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x000F1DD4 File Offset: 0x000EFFD4
		public object CreateInstance(string typeName)
		{
			return this.CreateInstance(typeName, false);
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x000F1DE0 File Offset: 0x000EFFE0
		public object CreateInstance(string typeName, bool ignoreCase)
		{
			Type type = this.GetType(typeName, false, ignoreCase);
			if (type == null)
			{
				return null;
			}
			object result;
			try
			{
				result = Activator.CreateInstance(type);
			}
			catch (InvalidOperationException)
			{
				throw new ArgumentException("It is illegal to invoke a method on a Type loaded via ReflectionOnly methods.");
			}
			return result;
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x000F1E2C File Offset: 0x000F002C
		public virtual object CreateInstance(string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			Type type = this.GetType(typeName, false, ignoreCase);
			if (type == null)
			{
				return null;
			}
			object result;
			try
			{
				result = Activator.CreateInstance(type, bindingAttr, binder, args, culture, activationAttributes);
			}
			catch (InvalidOperationException)
			{
				throw new ArgumentException("It is illegal to invoke a method on a Type loaded via ReflectionOnly methods.");
			}
			return result;
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x000F1E80 File Offset: 0x000F0080
		public Module[] GetLoadedModules()
		{
			return this.GetLoadedModules(false);
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x000F1E89 File Offset: 0x000F0089
		public Module[] GetModules()
		{
			return this.GetModules(false);
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual Module[] GetModulesInternal()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C2E RID: 19502
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Assembly GetExecutingAssembly();

		// Token: 0x06004C2F RID: 19503
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Assembly GetCallingAssembly();

		// Token: 0x06004C30 RID: 19504
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr InternalGetReferencedAssemblies(Assembly module);

		// Token: 0x06004C31 RID: 19505 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual string[] GetManifestResourceNames()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x000F1E94 File Offset: 0x000F0094
		internal unsafe static AssemblyName[] GetReferencedAssemblies(Assembly module)
		{
			AssemblyName[] result;
			using (SafeGPtrArrayHandle safeGPtrArrayHandle = new SafeGPtrArrayHandle(Assembly.InternalGetReferencedAssemblies(module)))
			{
				int length = safeGPtrArrayHandle.Length;
				try
				{
					AssemblyName[] array = new AssemblyName[length];
					for (int i = 0; i < length; i++)
					{
						AssemblyName assemblyName = new AssemblyName();
						MonoAssemblyName* native = (MonoAssemblyName*)((void*)safeGPtrArrayHandle[i]);
						assemblyName.FillName(native, null, true, false, true, true);
						array[i] = assemblyName;
					}
					result = array;
				}
				finally
				{
					for (int j = 0; j < length; j++)
					{
						MonoAssemblyName* name = (MonoAssemblyName*)((void*)safeGPtrArrayHandle[j]);
						RuntimeMarshal.FreeAssemblyName(ref *name, true);
					}
				}
			}
			return result;
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual ManifestResourceInfo GetManifestResourceInfo(string resourceName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x06004C34 RID: 19508 RVA: 0x0005CD52 File Offset: 0x0005AF52
		[MonoTODO("Currently it always returns zero")]
		[ComVisible(false)]
		public virtual long HostContext
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual Module GetManifestModule()
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x06004C36 RID: 19510 RVA: 0x000479FC File Offset: 0x00045BFC
		[ComVisible(false)]
		public virtual bool ReflectionOnly
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x000930F4 File Offset: 0x000912F4
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x00097E36 File Offset: 0x00096036
		public override bool Equals(object o)
		{
			return base.Equals(o);
		}

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06004C39 RID: 19513 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual PermissionSet GrantedPermissionSet
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06004C3A RID: 19514 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual PermissionSet DeniedPermissionSet
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06004C3B RID: 19515 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual PermissionSet PermissionSet
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06004C3C RID: 19516 RVA: 0x000F1F4C File Offset: 0x000F014C
		public virtual SecurityRuleSet SecurityRuleSet
		{
			get
			{
				throw Assembly.CreateNIE();
			}
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x000F1F53 File Offset: 0x000F0153
		private static Exception CreateNIE()
		{
			return new NotImplementedException("Derived classes must implement it");
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual IList<CustomAttributeData> GetCustomAttributesData()
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06004C3F RID: 19519 RVA: 0x000040F7 File Offset: 0x000022F7
		[MonoTODO]
		public bool IsFullyTrusted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004C40 RID: 19520 RVA: 0x000F1F4C File Offset: 0x000F014C
		public virtual Type GetType(string name, bool throwOnError, bool ignoreCase)
		{
			throw Assembly.CreateNIE();
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x000F1F4C File Offset: 0x000F014C
		public virtual Module GetModule(string name)
		{
			throw Assembly.CreateNIE();
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x000F1F4C File Offset: 0x000F014C
		public virtual AssemblyName[] GetReferencedAssemblies()
		{
			throw Assembly.CreateNIE();
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x000F1F4C File Offset: 0x000F014C
		public virtual Module[] GetModules(bool getResourceModules)
		{
			throw Assembly.CreateNIE();
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x000F1F4C File Offset: 0x000F014C
		[MonoTODO("Always returns the same as GetModules")]
		public virtual Module[] GetLoadedModules(bool getResourceModules)
		{
			throw Assembly.CreateNIE();
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x000F1F4C File Offset: 0x000F014C
		public virtual Assembly GetSatelliteAssembly(CultureInfo culture)
		{
			throw Assembly.CreateNIE();
		}

		// Token: 0x06004C46 RID: 19526 RVA: 0x000F1F4C File Offset: 0x000F014C
		public virtual Assembly GetSatelliteAssembly(CultureInfo culture, Version version)
		{
			throw Assembly.CreateNIE();
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06004C47 RID: 19527 RVA: 0x000F1F4C File Offset: 0x000F014C
		public virtual Module ManifestModule
		{
			get
			{
				throw Assembly.CreateNIE();
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06004C48 RID: 19528 RVA: 0x000F1F4C File Offset: 0x000F014C
		public virtual bool GlobalAssemblyCache
		{
			get
			{
				throw Assembly.CreateNIE();
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06004C49 RID: 19529 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsDynamic
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004C4A RID: 19530 RVA: 0x000F1F5F File Offset: 0x000F015F
		public static bool operator ==(Assembly left, Assembly right)
		{
			return left == right || (!(left == null ^ right == null) && left.Equals(right));
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x000F1F7B File Offset: 0x000F017B
		public static bool operator !=(Assembly left, Assembly right)
		{
			return left != right && ((left == null ^ right == null) || !left.Equals(right));
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06004C4C RID: 19532 RVA: 0x000F1F9A File Offset: 0x000F019A
		public virtual IEnumerable<TypeInfo> DefinedTypes
		{
			get
			{
				foreach (Type type in this.GetTypes())
				{
					yield return type.GetTypeInfo();
				}
				Type[] array = null;
				yield break;
			}
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06004C4D RID: 19533 RVA: 0x000F1FAA File Offset: 0x000F01AA
		public virtual IEnumerable<Type> ExportedTypes
		{
			get
			{
				return this.GetExportedTypes();
			}
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06004C4E RID: 19534 RVA: 0x000F1FB2 File Offset: 0x000F01B2
		public virtual IEnumerable<Module> Modules
		{
			get
			{
				return this.GetModules();
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06004C4F RID: 19535 RVA: 0x000F1FBA File Offset: 0x000F01BA
		public virtual IEnumerable<CustomAttributeData> CustomAttributes
		{
			get
			{
				return this.GetCustomAttributesData();
			}
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual Type[] GetForwardedTypes()
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x020008E9 RID: 2281
		internal class ResolveEventHolder
		{
			// Token: 0x1400001D RID: 29
			// (add) Token: 0x06004C52 RID: 19538 RVA: 0x000F1FC4 File Offset: 0x000F01C4
			// (remove) Token: 0x06004C53 RID: 19539 RVA: 0x000F1FFC File Offset: 0x000F01FC
			public event ModuleResolveEventHandler ModuleResolve;
		}
	}
}
