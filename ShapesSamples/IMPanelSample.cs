using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200000C RID: 12
	public class IMPanelSample : ImmediateModePanel
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00003A6C File Offset: 0x00001C6C
		public override void DrawPanelShapes(Rect rect, ImCanvasContext ctx)
		{
			if (this.colorGradient == null)
			{
				return;
			}
			Draw.Rectangle(rect, 8f, Color.black);
			Rect rect2 = this.Inset(rect, 8f);
			rect2.width *= this.fillAmount;
			Draw.Rectangle(rect2, this.colorGradient.Evaluate(this.fillAmount));
			Draw.RectangleBorder(rect, 4f, 8f, Color.white);
			Draw.FontSize = 240f;
			Draw.Text(new Vector2(rect.xMin + 6f, rect.yMax + 6f), this.title, 18);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003B1A File Offset: 0x00001D1A
		private Rect Inset(Rect r, float amount)
		{
			return new Rect(r.x + amount, r.y + amount, r.width - amount * 2f, r.height - amount * 2f);
		}

		// Token: 0x0400005A RID: 90
		[Range(0f, 1f)]
		public float fillAmount = 1f;

		// Token: 0x0400005B RID: 91
		public Gradient colorGradient;

		// Token: 0x0400005C RID: 92
		public string title = "Title";
	}
}
