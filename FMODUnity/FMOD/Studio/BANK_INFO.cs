using System;

namespace FMOD.Studio
{
	// Token: 0x020000D2 RID: 210
	public struct BANK_INFO
	{
		// Token: 0x040004B9 RID: 1209
		public int size;

		// Token: 0x040004BA RID: 1210
		public IntPtr userdata;

		// Token: 0x040004BB RID: 1211
		public int userdatalength;

		// Token: 0x040004BC RID: 1212
		public FILE_OPEN_CALLBACK opencallback;

		// Token: 0x040004BD RID: 1213
		public FILE_CLOSE_CALLBACK closecallback;

		// Token: 0x040004BE RID: 1214
		public FILE_READ_CALLBACK readcallback;

		// Token: 0x040004BF RID: 1215
		public FILE_SEEK_CALLBACK seekcallback;
	}
}
