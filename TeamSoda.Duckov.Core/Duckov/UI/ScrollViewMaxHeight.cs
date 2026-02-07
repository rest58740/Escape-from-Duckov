using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003E2 RID: 994
	[RequireComponent(typeof(ScrollRect))]
	[ExecuteInEditMode]
	public class ScrollViewMaxHeight : UIBehaviour, ILayoutElement
	{
		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002443 RID: 9283 RVA: 0x0007F630 File Offset: 0x0007D830
		public float preferredHeight
		{
			get
			{
				float y = this.scrollRect.content.sizeDelta.y;
				float num = this.maxHeight;
				if (this.useTargetParentSize)
				{
					float num2 = 0f;
					foreach (RectTransform rectTransform in this.siblings)
					{
						num2 += rectTransform.rect.height;
					}
					num = this.targetParentHeight - num2 - this.parentLayoutMargin;
				}
				if (y > num)
				{
					return num;
				}
				return y;
			}
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x0007F6D4 File Offset: 0x0007D8D4
		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x0007F6D6 File Offset: 0x0007D8D6
		public virtual void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002446 RID: 9286 RVA: 0x0007F6D8 File Offset: 0x0007D8D8
		public virtual float minWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002447 RID: 9287 RVA: 0x0007F6DF File Offset: 0x0007D8DF
		public virtual float minHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002448 RID: 9288 RVA: 0x0007F6E6 File Offset: 0x0007D8E6
		public virtual float preferredWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002449 RID: 9289 RVA: 0x0007F6ED File Offset: 0x0007D8ED
		public virtual float flexibleWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x0600244A RID: 9290 RVA: 0x0007F6F4 File Offset: 0x0007D8F4
		public virtual float flexibleHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x0600244B RID: 9291 RVA: 0x0007F6FB File Offset: 0x0007D8FB
		public virtual int layoutPriority
		{
			get
			{
				return this.m_layoutPriority;
			}
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0007F703 File Offset: 0x0007D903
		private void OnContentRectChange(RectTransform rectTransform)
		{
			this.SetDirty();
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x0600244D RID: 9293 RVA: 0x0007F70B File Offset: 0x0007D90B
		private RectTransform rectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = (base.transform as RectTransform);
				}
				return this._rectTransform;
			}
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x0007F734 File Offset: 0x0007D934
		protected override void OnEnable()
		{
			if (this.scrollRect == null)
			{
				this.scrollRect = base.GetComponent<ScrollRect>();
			}
			if (this.contentRectChangeEventEmitter == null)
			{
				this.contentRectChangeEventEmitter = this.scrollRect.content.GetComponent<RectTransformChangeEventEmitter>();
			}
			if (this.contentRectChangeEventEmitter == null)
			{
				this.contentRectChangeEventEmitter = this.scrollRect.content.gameObject.AddComponent<RectTransformChangeEventEmitter>();
			}
			base.OnEnable();
			this.contentRectChangeEventEmitter.OnRectTransformChange += this.OnContentRectChange;
			this.SetDirty();
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x0007F7CB File Offset: 0x0007D9CB
		protected override void OnDisable()
		{
			this.contentRectChangeEventEmitter.OnRectTransformChange -= this.OnContentRectChange;
			this.SetDirty();
			base.OnDisable();
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x0007F7F0 File Offset: 0x0007D9F0
		private void Update()
		{
			if (this.preferredHeight != this.rectTransform.rect.height)
			{
				this.SetDirty();
			}
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x0007F81E File Offset: 0x0007DA1E
		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(base.transform as RectTransform);
		}

		// Token: 0x040018A9 RID: 6313
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x040018AA RID: 6314
		[SerializeField]
		private RectTransformChangeEventEmitter contentRectChangeEventEmitter;

		// Token: 0x040018AB RID: 6315
		[SerializeField]
		private int m_layoutPriority = 1;

		// Token: 0x040018AC RID: 6316
		[SerializeField]
		private bool useTargetParentSize;

		// Token: 0x040018AD RID: 6317
		[SerializeField]
		private float targetParentHeight = 935f;

		// Token: 0x040018AE RID: 6318
		[SerializeField]
		private List<RectTransform> siblings = new List<RectTransform>();

		// Token: 0x040018AF RID: 6319
		[SerializeField]
		private float parentLayoutMargin = 16f;

		// Token: 0x040018B0 RID: 6320
		[SerializeField]
		private float maxHeight = 100f;

		// Token: 0x040018B1 RID: 6321
		private RectTransform _rectTransform;
	}
}
