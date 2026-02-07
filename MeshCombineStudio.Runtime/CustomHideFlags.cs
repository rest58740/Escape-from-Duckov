using System;

namespace MeshCombineStudio
{
	// Token: 0x0200003E RID: 62
	public enum CustomHideFlags
	{
		// Token: 0x0400019E RID: 414
		HideInHierarchy = 1,
		// Token: 0x0400019F RID: 415
		HideInInspector,
		// Token: 0x040001A0 RID: 416
		DontSaveInEditor = 4,
		// Token: 0x040001A1 RID: 417
		NotEditable = 8,
		// Token: 0x040001A2 RID: 418
		DontSaveInBuild = 16,
		// Token: 0x040001A3 RID: 419
		DontUnloadUnusedAsset = 32
	}
}
