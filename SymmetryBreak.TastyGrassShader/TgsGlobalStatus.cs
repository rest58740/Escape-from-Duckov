using System;

namespace SymmetryBreakStudio
{
	// Token: 0x02000004 RID: 4
	public static class TgsGlobalStatus
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020C8 File Offset: 0x000002C8
		// (set) Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public static int instances { get; internal set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020D7 File Offset: 0x000002D7
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000020CF File Offset: 0x000002CF
		public static int instancesReady { get; internal set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020DE File Offset: 0x000002DE
		public static bool areAllInstancesReady
		{
			get
			{
				return TgsGlobalStatus.instancesReady >= TgsGlobalStatus.instances;
			}
		}
	}
}
