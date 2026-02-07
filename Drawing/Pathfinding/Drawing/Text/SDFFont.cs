using System;
using UnityEngine;

namespace Pathfinding.Drawing.Text
{
	// Token: 0x0200005D RID: 93
	internal struct SDFFont
	{
		// Token: 0x0400017D RID: 381
		public string name;

		// Token: 0x0400017E RID: 382
		public int size;

		// Token: 0x0400017F RID: 383
		public int width;

		// Token: 0x04000180 RID: 384
		public int height;

		// Token: 0x04000181 RID: 385
		public bool bold;

		// Token: 0x04000182 RID: 386
		public bool italic;

		// Token: 0x04000183 RID: 387
		public SDFCharacter[] characters;

		// Token: 0x04000184 RID: 388
		public Material material;
	}
}
