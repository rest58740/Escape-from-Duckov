using System;
using System.Reflection;

namespace System
{
	// Token: 0x02000178 RID: 376
	public class ResolveEventArgs : EventArgs
	{
		// Token: 0x06000EE1 RID: 3809 RVA: 0x0003CA71 File Offset: 0x0003AC71
		public ResolveEventArgs(string name)
		{
			this.Name = name;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0003CA80 File Offset: 0x0003AC80
		public ResolveEventArgs(string name, Assembly requestingAssembly)
		{
			this.Name = name;
			this.RequestingAssembly = requestingAssembly;
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x0003CA96 File Offset: 0x0003AC96
		public string Name { get; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x0003CA9E File Offset: 0x0003AC9E
		public Assembly RequestingAssembly { get; }
	}
}
