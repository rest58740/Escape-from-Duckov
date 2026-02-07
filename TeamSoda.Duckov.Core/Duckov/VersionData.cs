using System;

namespace Duckov
{
	// Token: 0x02000243 RID: 579
	[Serializable]
	public struct VersionData
	{
		// Token: 0x06001250 RID: 4688 RVA: 0x00047164 File Offset: 0x00045364
		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}{3}", new object[]
			{
				this.mainVersion,
				this.subVersion,
				this.buildVersion,
				this.suffix
			});
		}

		// Token: 0x04000E1B RID: 3611
		public int mainVersion;

		// Token: 0x04000E1C RID: 3612
		public int subVersion;

		// Token: 0x04000E1D RID: 3613
		public int buildVersion;

		// Token: 0x04000E1E RID: 3614
		public string suffix;
	}
}
