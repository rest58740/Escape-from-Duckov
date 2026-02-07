using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200001E RID: 30
	public class ImCanvasContext
	{
		// Token: 0x06000B2B RID: 2859 RVA: 0x000157EA File Offset: 0x000139EA
		internal void UpdateParams(Canvas canvas, Camera camera, RectTransform cnvTf, Matrix4x4 canvasToWorldNet)
		{
			this.camera = camera;
			this.canvas = canvas;
			this.canvasRect = cnvTf.rect;
			this.worldToCanvas = cnvTf.worldToLocalMatrix;
			this.canvasToWorld = cnvTf.localToWorldMatrix;
			this.canvasToWorldNet = canvasToWorldNet;
		}

		// Token: 0x040000F4 RID: 244
		public Camera camera;

		// Token: 0x040000F5 RID: 245
		public Canvas canvas;

		// Token: 0x040000F6 RID: 246
		public Rect canvasRect;

		// Token: 0x040000F7 RID: 247
		public Matrix4x4 worldToCanvas;

		// Token: 0x040000F8 RID: 248
		public Matrix4x4 canvasToWorld;

		// Token: 0x040000F9 RID: 249
		public Matrix4x4 canvasToWorldNet;
	}
}
