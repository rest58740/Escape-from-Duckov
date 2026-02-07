using System;
using UnityEngine;

// Token: 0x020001C9 RID: 457
public interface IMiniMapEntry
{
	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06000DD7 RID: 3543
	Sprite Sprite { get; }

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06000DD8 RID: 3544
	float PixelSize { get; }

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06000DD9 RID: 3545
	Vector2 Offset { get; }

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06000DDA RID: 3546
	string SceneID { get; }

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06000DDB RID: 3547
	bool Hide { get; }

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06000DDC RID: 3548
	bool NoSignal { get; }
}
