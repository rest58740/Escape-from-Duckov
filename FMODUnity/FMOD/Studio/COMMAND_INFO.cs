using System;

namespace FMOD.Studio
{
	// Token: 0x020000EC RID: 236
	public struct COMMAND_INFO
	{
		// Token: 0x0400053D RID: 1341
		public StringWrapper commandname;

		// Token: 0x0400053E RID: 1342
		public int parentcommandindex;

		// Token: 0x0400053F RID: 1343
		public int framenumber;

		// Token: 0x04000540 RID: 1344
		public float frametime;

		// Token: 0x04000541 RID: 1345
		public INSTANCETYPE instancetype;

		// Token: 0x04000542 RID: 1346
		public INSTANCETYPE outputtype;

		// Token: 0x04000543 RID: 1347
		public uint instancehandle;

		// Token: 0x04000544 RID: 1348
		public uint outputhandle;
	}
}
