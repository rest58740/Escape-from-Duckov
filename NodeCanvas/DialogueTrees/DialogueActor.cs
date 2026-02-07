using System;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000F1 RID: 241
	[AddComponentMenu("NodeCanvas/Dialogue Actor")]
	public class DialogueActor : MonoBehaviour, IDialogueActor
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00010631 File Offset: 0x0000E831
		public new string name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00010639 File Offset: 0x0000E839
		public Texture2D portrait
		{
			get
			{
				return this._portrait;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00010644 File Offset: 0x0000E844
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

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x000106BA File Offset: 0x0000E8BA
		public Color dialogueColor
		{
			get
			{
				return this._dialogueColor;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x000106C2 File Offset: 0x0000E8C2
		public Vector3 dialoguePosition
		{
			get
			{
				return base.transform.TransformPoint(this._dialogueOffset);
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000106E8 File Offset: 0x0000E8E8
		Transform IDialogueActor.get_transform()
		{
			return base.transform;
		}

		// Token: 0x040002BB RID: 699
		[SerializeField]
		protected string _name;

		// Token: 0x040002BC RID: 700
		[SerializeField]
		protected Texture2D _portrait;

		// Token: 0x040002BD RID: 701
		[SerializeField]
		protected Color _dialogueColor = Color.white;

		// Token: 0x040002BE RID: 702
		[SerializeField]
		protected Vector3 _dialogueOffset;

		// Token: 0x040002BF RID: 703
		private Sprite _portraitSprite;
	}
}
