using System;

namespace FMOD
{
	// Token: 0x0200001F RID: 31
	public struct ERRORCALLBACK_INFO
	{
		// Token: 0x04000167 RID: 359
		public RESULT result;

		// Token: 0x04000168 RID: 360
		public ERRORCALLBACK_INSTANCETYPE instancetype;

		// Token: 0x04000169 RID: 361
		public IntPtr instance;

		// Token: 0x0400016A RID: 362
		public StringWrapper functionname;

		// Token: 0x0400016B RID: 363
		public StringWrapper functionparams;
	}
}
