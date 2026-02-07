using System;

namespace FMOD
{
	// Token: 0x0200000A RID: 10
	public struct ASYNCREADINFO
	{
		// Token: 0x0400006A RID: 106
		public IntPtr handle;

		// Token: 0x0400006B RID: 107
		public uint offset;

		// Token: 0x0400006C RID: 108
		public uint sizebytes;

		// Token: 0x0400006D RID: 109
		public int priority;

		// Token: 0x0400006E RID: 110
		public IntPtr userdata;

		// Token: 0x0400006F RID: 111
		public IntPtr buffer;

		// Token: 0x04000070 RID: 112
		public uint bytesread;

		// Token: 0x04000071 RID: 113
		public FILE_ASYNCDONE_FUNC done;
	}
}
