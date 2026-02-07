using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000009 RID: 9
	public class IMCanvasSample : ImmediateModeCanvas
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00003390 File Offset: 0x00001590
		public override void DrawCanvasShapes(ImCanvasContext ctx)
		{
			float num = Mathf.Min(ctx.canvasRect.width, ctx.canvasRect.height) / 2f * 0.9f;
			Draw.Ring(Vector3.zero, Quaternion.identity, num, 1f, new Color(1f, 1f, 1f, 0.3f));
			Draw.RectangleBorder(ctx.canvasRect, 8f, 16f, Color.white);
			base.DrawPanels();
			Draw.Disc(Vector3.zero, 4f);
			Vector2 vector = new Vector2(14f, 0f);
			Vector2 vector2 = new Vector2(28f, 0f);
			for (int i = 0; i < 4; i++)
			{
				Draw.Line(vector, vector2, 4f, 2);
				vector = ShapesMath.Rotate90CCW(vector);
				vector2 = ShapesMath.Rotate90CCW(vector2);
			}
		}
	}
}
