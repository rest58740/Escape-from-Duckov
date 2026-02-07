using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x02000798 RID: 1944
	[ComVisible(false)]
	public class NamespaceResolveEventArgs : EventArgs
	{
		// Token: 0x060044DD RID: 17629 RVA: 0x000E4F34 File Offset: 0x000E3134
		public NamespaceResolveEventArgs(string namespaceName, Assembly requestingAssembly)
		{
			this.NamespaceName = namespaceName;
			this.RequestingAssembly = requestingAssembly;
			this.ResolvedAssemblies = new Collection<Assembly>();
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x060044DE RID: 17630 RVA: 0x000E4F55 File Offset: 0x000E3155
		// (set) Token: 0x060044DF RID: 17631 RVA: 0x000E4F5D File Offset: 0x000E315D
		public string NamespaceName { get; private set; }

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x060044E0 RID: 17632 RVA: 0x000E4F66 File Offset: 0x000E3166
		// (set) Token: 0x060044E1 RID: 17633 RVA: 0x000E4F6E File Offset: 0x000E316E
		public Assembly RequestingAssembly { get; private set; }

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x060044E2 RID: 17634 RVA: 0x000E4F77 File Offset: 0x000E3177
		// (set) Token: 0x060044E3 RID: 17635 RVA: 0x000E4F7F File Offset: 0x000E317F
		public Collection<Assembly> ResolvedAssemblies { get; private set; }
	}
}
