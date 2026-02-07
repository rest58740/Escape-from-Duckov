using System;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A1 RID: 673
	[Serializable]
	public class ShopEntity
	{
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001648 RID: 5704 RVA: 0x00053014 File Offset: 0x00051214
		public string ID
		{
			get
			{
				if (!this.artifact)
				{
					return null;
				}
				return this.artifact.ID;
			}
		}

		// Token: 0x04001077 RID: 4215
		public GoldMinerArtifact artifact;

		// Token: 0x04001078 RID: 4216
		public bool locked;

		// Token: 0x04001079 RID: 4217
		public bool sold;

		// Token: 0x0400107A RID: 4218
		public float priceFactor = 1f;
	}
}
