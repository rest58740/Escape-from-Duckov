using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000056 RID: 86
	public struct DSP_BUFFER_ARRAY
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00004E01 File Offset: 0x00003001
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x00004E2A File Offset: 0x0000302A
		public int numchannels
		{
			get
			{
				if (this.buffernumchannels != IntPtr.Zero && this.numbuffers != 0)
				{
					return Marshal.ReadInt32(this.buffernumchannels);
				}
				return 0;
			}
			set
			{
				if (this.buffernumchannels != IntPtr.Zero && this.numbuffers != 0)
				{
					Marshal.WriteInt32(this.buffernumchannels, value);
				}
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00004E52 File Offset: 0x00003052
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x00004E7F File Offset: 0x0000307F
		public IntPtr buffer
		{
			get
			{
				if (this.buffers != IntPtr.Zero && this.numbuffers != 0)
				{
					return Marshal.ReadIntPtr(this.buffers);
				}
				return IntPtr.Zero;
			}
			set
			{
				if (this.buffers != IntPtr.Zero && this.numbuffers != 0)
				{
					Marshal.WriteIntPtr(this.buffers, value);
				}
			}
		}

		// Token: 0x04000269 RID: 617
		public int numbuffers;

		// Token: 0x0400026A RID: 618
		public IntPtr buffernumchannels;

		// Token: 0x0400026B RID: 619
		public IntPtr bufferchannelmask;

		// Token: 0x0400026C RID: 620
		public IntPtr buffers;

		// Token: 0x0400026D RID: 621
		public SPEAKERMODE speakermode;
	}
}
