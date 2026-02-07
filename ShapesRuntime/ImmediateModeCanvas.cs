using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Shapes
{
	// Token: 0x02000021 RID: 33
	[ExecuteAlways]
	[RequireComponent(typeof(Canvas))]
	public class ImmediateModeCanvas : ImmediateModeShapeDrawer
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00015D38 File Offset: 0x00013F38
		private Canvas Canvas
		{
			get
			{
				return this.canvas = ((this.canvas != null) ? this.canvas : base.GetComponent<Canvas>());
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x00015D6C File Offset: 0x00013F6C
		private RectTransform CanvasRectTf
		{
			get
			{
				return this.canvasRectTf = ((this.canvasRectTf != null) ? this.canvasRectTf : base.GetComponent<RectTransform>());
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x00015DA0 File Offset: 0x00013FA0
		private Camera CamUI
		{
			get
			{
				return this.camUI = ((this.camUI != null) ? this.camUI : this.Canvas.worldCamera);
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x00015DD7 File Offset: 0x00013FD7
		private bool IsCameraBasedUI
		{
			get
			{
				return this.Canvas.worldCamera != null && this.Canvas.renderMode == RenderMode.WorldSpace;
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00015DFC File Offset: 0x00013FFC
		public void Add(ImmediateModePanel panel)
		{
			this.panels.Add(panel);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00015E0A File Offset: 0x0001400A
		public void Remove(ImmediateModePanel panel)
		{
			this.panels.Remove(panel);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00015E1C File Offset: 0x0001401C
		protected void DrawPanels()
		{
			using (Draw.Scope)
			{
				if (this.Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					Draw.Matrix *= ImmediateModeCanvas.canvasContext.worldToCanvas;
				}
				foreach (ImmediateModePanel immediateModePanel in this.panels)
				{
					using (Draw.Scope)
					{
						if (this.Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
						{
							Draw.Matrix = ShapesMath.AffineMtxMul(Draw.Matrix, immediateModePanel.transform.localToWorldMatrix);
						}
						else
						{
							Draw.Matrix = immediateModePanel.transform.localToWorldMatrix;
						}
						immediateModePanel.DrawPanel(ImmediateModeCanvas.canvasContext);
					}
				}
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00015F14 File Offset: 0x00014114
		private bool CameraShouldRenderUI(Camera cam)
		{
			if (cam.cameraType != CameraType.Game)
			{
				return false;
			}
			if (this.canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				return cam.targetDisplay == this.canvas.targetDisplay;
			}
			return cam == this.CamUI;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00015F50 File Offset: 0x00014150
		public override void DrawShapes(Camera cam)
		{
			if (!this.Canvas.enabled)
			{
				return;
			}
			if (!this.CameraShouldRenderUI(cam))
			{
				return;
			}
			using (Draw.Command(cam, RenderPassEvent.BeforeRenderingPostProcessing))
			{
				Draw.ZTest = CompareFunction.Always;
				RectTransform rectTransform = this.CanvasRectTf;
				ImmediateModeCanvas.canvasContext.UpdateParams(this.Canvas, cam, rectTransform, this.DisplayAsWorldSpacePanel(cam) ? rectTransform.localToWorldMatrix : this.GetOverlayToWorldMatrix(cam));
				Draw.Matrix = ImmediateModeCanvas.canvasContext.canvasToWorldNet;
				this.DrawCanvasShapes(ImmediateModeCanvas.canvasContext);
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00015FF0 File Offset: 0x000141F0
		private bool DisplayAsWorldSpacePanel(Camera cam)
		{
			return cam.cameraType == CameraType.SceneView || (this.IsCameraBasedUI && cam == this.Canvas.worldCamera);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00016018 File Offset: 0x00014218
		private Matrix4x4 GetOverlayToWorldMatrix(Camera cam)
		{
			float num = (cam.nearClipPlane + cam.farClipPlane) / 2f;
			Transform transform = cam.transform;
			Vector3 forward = transform.forward;
			Vector3 vector = transform.TransformPoint(0f, 0f, num);
			RectTransform rectTransform = (RectTransform)this.Canvas.transform;
			float d;
			if (cam.orthographic)
			{
				d = 2f * cam.orthographicSize / rectTransform.sizeDelta.y;
			}
			else
			{
				double num2 = (double)cam.fieldOfView * 0.017453292519943295 / 2.0;
				double num3 = (double)((float)((double)num * Math.Tan(num2)));
				d = (float)(2.0 * num3 / (double)rectTransform.sizeDelta.y);
			}
			Vector3 v = transform.right * d;
			Vector3 v2 = transform.up * d;
			Vector3 v3 = forward * d;
			return new Matrix4x4(v, v2, v3, new Vector4(vector.x, vector.y, vector.z, 1f));
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00016135 File Offset: 0x00014335
		public virtual void DrawCanvasShapes(ImCanvasContext ctx)
		{
		}

		// Token: 0x04000101 RID: 257
		private static ImCanvasContext canvasContext = new ImCanvasContext();

		// Token: 0x04000102 RID: 258
		private Canvas canvas;

		// Token: 0x04000103 RID: 259
		private RectTransform canvasRectTf;

		// Token: 0x04000104 RID: 260
		private Camera camUI;

		// Token: 0x04000105 RID: 261
		private List<ImmediateModePanel> panels = new List<ImmediateModePanel>();
	}
}
