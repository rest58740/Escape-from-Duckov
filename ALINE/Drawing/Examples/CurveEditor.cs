using System;
using System.Collections.Generic;
using UnityEngine;

namespace Drawing.Examples
{
	// Token: 0x02000064 RID: 100
	public class CurveEditor : MonoBehaviour
	{
		// Token: 0x060003D7 RID: 983 RVA: 0x00012F15 File Offset: 0x00011115
		private void Awake()
		{
			this.cam = Camera.main;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00012F24 File Offset: 0x00011124
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				this.curves.Add(new CurveEditor.CurvePoint
				{
					position = Input.mousePosition,
					controlPoint0 = Vector2.zero,
					controlPoint1 = Vector2.zero
				});
			}
			if (this.curves.Count > 0 && Input.GetKey(KeyCode.Mouse0) && (Input.mousePosition - this.curves[this.curves.Count - 1].position).magnitude > 4f)
			{
				CurveEditor.CurvePoint curvePoint = this.curves[this.curves.Count - 1];
				curvePoint.controlPoint1 = Input.mousePosition - curvePoint.position;
				curvePoint.controlPoint0 = -curvePoint.controlPoint1;
			}
			this.Render();
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001301C File Offset: 0x0001121C
		private void Render()
		{
			using (CommandBuilder builder = DrawingManager.GetBuilder(true))
			{
				using (builder.InScreenSpace(this.cam))
				{
					for (int i = 0; i < this.curves.Count; i++)
					{
						builder.xy.Circle(this.curves[i].position, 2f, Color.blue);
					}
					for (int j = 0; j < this.curves.Count - 1; j++)
					{
						Vector2 position = this.curves[j].position;
						Vector2 v = position + this.curves[j].controlPoint1;
						Vector2 position2 = this.curves[j + 1].position;
						Vector2 v2 = position2 + this.curves[j + 1].controlPoint0;
						builder.Bezier(position, v, v2, position2, this.curveColor);
					}
				}
			}
		}

		// Token: 0x04000198 RID: 408
		private List<CurveEditor.CurvePoint> curves = new List<CurveEditor.CurvePoint>();

		// Token: 0x04000199 RID: 409
		private Camera cam;

		// Token: 0x0400019A RID: 410
		public Color curveColor;

		// Token: 0x02000065 RID: 101
		private class CurvePoint
		{
			// Token: 0x0400019B RID: 411
			public Vector2 position;

			// Token: 0x0400019C RID: 412
			public Vector2 controlPoint0;

			// Token: 0x0400019D RID: 413
			public Vector2 controlPoint1;
		}
	}
}
