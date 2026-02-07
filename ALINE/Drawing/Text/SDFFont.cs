using System;
using UnityEngine;

namespace Drawing.Text
{
	// Token: 0x0200005B RID: 91
	internal struct SDFFont
	{
		// Token: 0x04000172 RID: 370
		public string name;

		// Token: 0x04000173 RID: 371
		public int size;

		// Token: 0x04000174 RID: 372
		public int width;

		// Token: 0x04000175 RID: 373
		public int height;

		// Token: 0x04000176 RID: 374
		public bool bold;

		// Token: 0x04000177 RID: 375
		public bool italic;

		// Token: 0x04000178 RID: 376
		public SDFCharacter[] characters;

		// Token: 0x04000179 RID: 377
		public Material material;
	}
}
