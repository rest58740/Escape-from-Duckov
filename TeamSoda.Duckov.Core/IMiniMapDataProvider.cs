using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C8 RID: 456
public interface IMiniMapDataProvider
{
	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06000DD3 RID: 3539
	Sprite CombinedSprite { get; }

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06000DD4 RID: 3540
	List<IMiniMapEntry> Maps { get; }

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06000DD5 RID: 3541
	float PixelSize { get; }

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06000DD6 RID: 3542
	Vector3 CombinedCenter { get; }
}
