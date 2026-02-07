using System;
using System.Runtime.ConstrainedExecution;

namespace System.Runtime
{
	// Token: 0x02000552 RID: 1362
	public static class GCSettings
	{
		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x060035B3 RID: 13747 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[MonoTODO("Always returns false")]
		public static bool IsServerGC
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x060035B4 RID: 13748 RVA: 0x000040F7 File Offset: 0x000022F7
		// (set) Token: 0x060035B5 RID: 13749 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[MonoTODO("Always returns GCLatencyMode.Interactive and ignores set")]
		public static GCLatencyMode LatencyMode
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return GCLatencyMode.Interactive;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x060035B6 RID: 13750 RVA: 0x000C2005 File Offset: 0x000C0205
		// (set) Token: 0x060035B7 RID: 13751 RVA: 0x000C200C File Offset: 0x000C020C
		public static GCLargeObjectHeapCompactionMode LargeObjectHeapCompactionMode { [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)] get; [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)] set; }
	}
}
