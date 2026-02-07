using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000097 RID: 151
	public struct DSP_STATE
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00004F78 File Offset: 0x00003178
		public DSP_STATE_FUNCTIONS functions
		{
			get
			{
				return Marshal.PtrToStructure<DSP_STATE_FUNCTIONS>(this.functions_internal);
			}
		}

		// Token: 0x0400030E RID: 782
		public IntPtr instance;

		// Token: 0x0400030F RID: 783
		public IntPtr plugindata;

		// Token: 0x04000310 RID: 784
		public uint channelmask;

		// Token: 0x04000311 RID: 785
		public int source_speakermode;

		// Token: 0x04000312 RID: 786
		public IntPtr sidechaindata;

		// Token: 0x04000313 RID: 787
		public int sidechainchannels;

		// Token: 0x04000314 RID: 788
		private IntPtr functions_internal;

		// Token: 0x04000315 RID: 789
		public int systemobject;
	}
}
