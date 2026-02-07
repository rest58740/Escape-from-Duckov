using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000096 RID: 150
	public struct DSP_STATE_FUNCTIONS
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00004F5E File Offset: 0x0000315E
		public DSP_STATE_DFT_FUNCTIONS dft
		{
			get
			{
				return Marshal.PtrToStructure<DSP_STATE_DFT_FUNCTIONS>(this.dft_internal);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x00004F6B File Offset: 0x0000316B
		public DSP_STATE_PAN_FUNCTIONS pan
		{
			get
			{
				return Marshal.PtrToStructure<DSP_STATE_PAN_FUNCTIONS>(this.pan_internal);
			}
		}

		// Token: 0x04000302 RID: 770
		public DSP_ALLOC_FUNC alloc;

		// Token: 0x04000303 RID: 771
		public DSP_REALLOC_FUNC realloc;

		// Token: 0x04000304 RID: 772
		public DSP_FREE_FUNC free;

		// Token: 0x04000305 RID: 773
		public DSP_GETSAMPLERATE_FUNC getsamplerate;

		// Token: 0x04000306 RID: 774
		public DSP_GETBLOCKSIZE_FUNC getblocksize;

		// Token: 0x04000307 RID: 775
		public IntPtr dft_internal;

		// Token: 0x04000308 RID: 776
		public IntPtr pan_internal;

		// Token: 0x04000309 RID: 777
		public DSP_GETSPEAKERMODE_FUNC getspeakermode;

		// Token: 0x0400030A RID: 778
		public DSP_GETCLOCK_FUNC getclock;

		// Token: 0x0400030B RID: 779
		public DSP_GETLISTENERATTRIBUTES_FUNC getlistenerattributes;

		// Token: 0x0400030C RID: 780
		public DSP_LOG_FUNC log;

		// Token: 0x0400030D RID: 781
		public DSP_GETUSERDATA_FUNC getuserdata;
	}
}
