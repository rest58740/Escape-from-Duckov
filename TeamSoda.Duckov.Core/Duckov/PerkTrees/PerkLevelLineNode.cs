using System;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x0200025E RID: 606
	public class PerkLevelLineNode : PerkRelationNodeBase
	{
		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001336 RID: 4918 RVA: 0x00049203 File Offset: 0x00047403
		public string DisplayName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x0004920B File Offset: 0x0004740B
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001338 RID: 4920 RVA: 0x0004920E File Offset: 0x0004740E
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04000E9C RID: 3740
		public Vector2 cachedPosition;
	}
}
