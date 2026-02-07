using System;
using System.Reflection;

namespace System
{
	// Token: 0x020000FA RID: 250
	public class AssemblyLoadEventArgs : EventArgs
	{
		// Token: 0x06000747 RID: 1863 RVA: 0x00021718 File Offset: 0x0001F918
		public AssemblyLoadEventArgs(Assembly loadedAssembly)
		{
			this.LoadedAssembly = loadedAssembly;
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x00021727 File Offset: 0x0001F927
		public Assembly LoadedAssembly { get; }
	}
}
