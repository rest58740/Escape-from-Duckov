using System;

namespace EPOOutline
{
	// Token: 0x0200001A RID: 26
	[Flags]
	public enum RenderersAddingMode
	{
		// Token: 0x040000B1 RID: 177
		All = -1,
		// Token: 0x040000B2 RID: 178
		None = 0,
		// Token: 0x040000B3 RID: 179
		MeshRenderer = 1,
		// Token: 0x040000B4 RID: 180
		SkinnedMeshRenderer = 2,
		// Token: 0x040000B5 RID: 181
		SpriteRenderer = 4,
		// Token: 0x040000B6 RID: 182
		Others = 4096
	}
}
