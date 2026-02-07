using System;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000F2 RID: 242
	[CreateAssetMenu(menuName = "ParadoxNotion/NodeCanvas/Dialogue Actor")]
	public class DialogueActorAsset : ScriptableObject, IDialogueActor
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x000106F0 File Offset: 0x0000E8F0
		public new string name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x000106F8 File Offset: 0x0000E8F8
		public Texture2D portrait
		{
			get
			{
				return this._portrait;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00010700 File Offset: 0x0000E900
		public Color dialogueColor
		{
			get
			{
				return this._dialogueColor;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00010708 File Offset: 0x0000E908
		public Vector3 dialoguePosition
		{
			get
			{
				return Vector3.zero;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0001070F File Offset: 0x0000E90F
		public Transform transform
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00010714 File Offset: 0x0000E914
		public Sprite portraitSprite
		{
			get
			{
				if (this._portraitSprite == null && this.portrait != null)
				{
					this._portraitSprite = Sprite.Create(this.portrait, new Rect(0f, 0f, (float)this.portrait.width, (float)this.portrait.height), new Vector2(0.5f, 0.5f));
				}
				return this._portraitSprite;
			}
		}

		// Token: 0x040002C0 RID: 704
		[SerializeField]
		protected string _name;

		// Token: 0x040002C1 RID: 705
		[SerializeField]
		protected Texture2D _portrait;

		// Token: 0x040002C2 RID: 706
		[SerializeField]
		protected Color _dialogueColor = Color.white;

		// Token: 0x040002C3 RID: 707
		[SerializeField]
		protected Vector3 _dialogueOffset;

		// Token: 0x040002C4 RID: 708
		private Sprite _portraitSprite;
	}
}
