using System;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000FB RID: 251
	[Serializable]
	public class ProxyDialogueActor : IDialogueActor
	{
		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00011191 File Offset: 0x0000F391
		public string name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00011199 File Offset: 0x0000F399
		public Texture2D portrait
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0001119C File Offset: 0x0000F39C
		public Sprite portraitSprite
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0001119F File Offset: 0x0000F39F
		public Color dialogueColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x000111A6 File Offset: 0x0000F3A6
		public Vector3 dialoguePosition
		{
			get
			{
				return Vector3.zero;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x000111AD File Offset: 0x0000F3AD
		public Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x000111B5 File Offset: 0x0000F3B5
		public ProxyDialogueActor(string name, Transform transform)
		{
			this._name = name;
			this._transform = transform;
		}

		// Token: 0x040002DD RID: 733
		private string _name;

		// Token: 0x040002DE RID: 734
		private Transform _transform;
	}
}
