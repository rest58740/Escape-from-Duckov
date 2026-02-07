using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000022 RID: 34
	[ExecuteAlways]
	public class ImmediateModePanel : MonoBehaviour
	{
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x00016158 File Offset: 0x00014358
		private ImmediateModeCanvas ImCanvas
		{
			get
			{
				if (!(this.imCanvas != null))
				{
					return this.imCanvas = base.GetComponentInParent<ImmediateModeCanvas>();
				}
				return this.imCanvas;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x00016189 File Offset: 0x00014389
		public bool Valid
		{
			get
			{
				return this.ImCanvas != null;
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00016197 File Offset: 0x00014397
		public virtual void OnEnable()
		{
			if (this.Valid)
			{
				this.ImCanvas.Add(this);
				return;
			}
			Debug.LogWarning("ImmediateModePanel attached to " + base.gameObject.name + " is missing an ImmediateModeCanvas component on its canvas", this);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x000161CE File Offset: 0x000143CE
		public virtual void OnDisable()
		{
			if (this.Valid)
			{
				this.ImCanvas.Remove(this);
			}
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x000161E4 File Offset: 0x000143E4
		internal void DrawPanel(ImCanvasContext ctx)
		{
			RectTransform rectTransform = base.transform as RectTransform;
			this.DrawPanelShapes(rectTransform.rect, ctx);
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0001620A File Offset: 0x0001440A
		public virtual void DrawPanelShapes(Rect rect, ImCanvasContext ctx)
		{
		}

		// Token: 0x04000106 RID: 262
		private ImmediateModeCanvas imCanvas;
	}
}
