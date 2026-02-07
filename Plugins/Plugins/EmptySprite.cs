using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public static class EmptySprite
{
	// Token: 0x06000023 RID: 35 RVA: 0x00002603 File Offset: 0x00000803
	public static Sprite Get()
	{
		if (EmptySprite.instance == null)
		{
			EmptySprite.instance = Resources.Load<Sprite>("procedural_ui_image_default_sprite");
		}
		return EmptySprite.instance;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002626 File Offset: 0x00000826
	public static bool IsEmptySprite(Sprite s)
	{
		return EmptySprite.Get() == s;
	}

	// Token: 0x0400000E RID: 14
	private static Sprite instance;
}
