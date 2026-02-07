using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Policy;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000763 RID: 1891
	[ComVisible(true)]
	[CLSCompliant(false)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("17156360-2F1A-384A-BC52-FDE93C215C5B")]
	[TypeLibImportClass(typeof(Assembly))]
	public interface _Assembly
	{
		// Token: 0x060042A6 RID: 17062
		string ToString();

		// Token: 0x060042A7 RID: 17063
		bool Equals(object other);

		// Token: 0x060042A8 RID: 17064
		int GetHashCode();

		// Token: 0x060042A9 RID: 17065
		Type GetType();

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x060042AA RID: 17066
		string CodeBase { get; }

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x060042AB RID: 17067
		string EscapedCodeBase { get; }

		// Token: 0x060042AC RID: 17068
		AssemblyName GetName();

		// Token: 0x060042AD RID: 17069
		AssemblyName GetName(bool copiedName);

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x060042AE RID: 17070
		string FullName { get; }

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x060042AF RID: 17071
		MethodInfo EntryPoint { get; }

		// Token: 0x060042B0 RID: 17072
		Type GetType(string name);

		// Token: 0x060042B1 RID: 17073
		Type GetType(string name, bool throwOnError);

		// Token: 0x060042B2 RID: 17074
		Type[] GetExportedTypes();

		// Token: 0x060042B3 RID: 17075
		Type[] GetTypes();

		// Token: 0x060042B4 RID: 17076
		Stream GetManifestResourceStream(Type type, string name);

		// Token: 0x060042B5 RID: 17077
		Stream GetManifestResourceStream(string name);

		// Token: 0x060042B6 RID: 17078
		FileStream GetFile(string name);

		// Token: 0x060042B7 RID: 17079
		FileStream[] GetFiles();

		// Token: 0x060042B8 RID: 17080
		FileStream[] GetFiles(bool getResourceModules);

		// Token: 0x060042B9 RID: 17081
		string[] GetManifestResourceNames();

		// Token: 0x060042BA RID: 17082
		ManifestResourceInfo GetManifestResourceInfo(string resourceName);

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x060042BB RID: 17083
		string Location { get; }

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x060042BC RID: 17084
		Evidence Evidence { get; }

		// Token: 0x060042BD RID: 17085
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x060042BE RID: 17086
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x060042BF RID: 17087
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x060042C0 RID: 17088
		void GetObjectData(SerializationInfo info, StreamingContext context);

		// Token: 0x060042C1 RID: 17089
		Type GetType(string name, bool throwOnError, bool ignoreCase);

		// Token: 0x060042C2 RID: 17090
		Assembly GetSatelliteAssembly(CultureInfo culture);

		// Token: 0x060042C3 RID: 17091
		Assembly GetSatelliteAssembly(CultureInfo culture, Version version);

		// Token: 0x060042C4 RID: 17092
		Module LoadModule(string moduleName, byte[] rawModule);

		// Token: 0x060042C5 RID: 17093
		Module LoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore);

		// Token: 0x060042C6 RID: 17094
		object CreateInstance(string typeName);

		// Token: 0x060042C7 RID: 17095
		object CreateInstance(string typeName, bool ignoreCase);

		// Token: 0x060042C8 RID: 17096
		object CreateInstance(string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes);

		// Token: 0x060042C9 RID: 17097
		Module[] GetLoadedModules();

		// Token: 0x060042CA RID: 17098
		Module[] GetLoadedModules(bool getResourceModules);

		// Token: 0x060042CB RID: 17099
		Module[] GetModules();

		// Token: 0x060042CC RID: 17100
		Module[] GetModules(bool getResourceModules);

		// Token: 0x060042CD RID: 17101
		Module GetModule(string name);

		// Token: 0x060042CE RID: 17102
		AssemblyName[] GetReferencedAssemblies();

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x060042CF RID: 17103
		bool GlobalAssemblyCache { get; }

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060042D0 RID: 17104
		// (remove) Token: 0x060042D1 RID: 17105
		event ModuleResolveEventHandler ModuleResolve;
	}
}
