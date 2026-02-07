using System;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000FA RID: 250
	public interface IDialogueActor
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600051D RID: 1309
		string name { get; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600051E RID: 1310
		Texture2D portrait { get; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600051F RID: 1311
		Sprite portraitSprite { get; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000520 RID: 1312
		Color dialogueColor { get; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000521 RID: 1313
		Vector3 dialoguePosition { get; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000522 RID: 1314
		Transform transform { get; }
	}
}
