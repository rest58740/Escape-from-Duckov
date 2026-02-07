using System;
using System.Collections.Generic;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x0200079A RID: 1946
	[MonoTODO]
	public static class WindowsRuntimeMetadata
	{
		// Token: 0x060044EA RID: 17642 RVA: 0x000479FC File Offset: 0x00045BFC
		public static IEnumerable<string> ResolveNamespace(string namespaceName, IEnumerable<string> packageGraphFilePaths)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x000479FC File Offset: 0x00045BFC
		public static IEnumerable<string> ResolveNamespace(string namespaceName, string windowsSdkFilePath, IEnumerable<string> packageGraphFilePaths)
		{
			throw new NotImplementedException();
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060044EC RID: 17644 RVA: 0x000E4F88 File Offset: 0x000E3188
		// (remove) Token: 0x060044ED RID: 17645 RVA: 0x000E4FBC File Offset: 0x000E31BC
		public static event EventHandler<DesignerNamespaceResolveEventArgs> DesignerNamespaceResolve;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060044EE RID: 17646 RVA: 0x000E4FF0 File Offset: 0x000E31F0
		// (remove) Token: 0x060044EF RID: 17647 RVA: 0x000E5024 File Offset: 0x000E3224
		public static event EventHandler<NamespaceResolveEventArgs> ReflectionOnlyNamespaceResolve;
	}
}
