using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x02000023 RID: 35
	public class ImmediateModeShapeDrawer : MonoBehaviour
	{
		// Token: 0x06000B4B RID: 2891 RVA: 0x00016214 File Offset: 0x00014414
		public virtual void DrawShapes(Camera cam)
		{
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00016218 File Offset: 0x00014418
		private void OnCameraPreRender(Camera cam)
		{
			CameraType cameraType = cam.cameraType;
			if (cameraType == CameraType.Preview || cameraType == CameraType.Reflection)
			{
				return;
			}
			if (this.useCullingMasks && (cam.cullingMask & 1 << base.gameObject.layer) == 0)
			{
				return;
			}
			this.DrawShapes(cam);
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0001625F File Offset: 0x0001445F
		public virtual void OnEnable()
		{
			RenderPipelineManager.beginCameraRendering += new Action<ScriptableRenderContext, Camera>(this.DrawShapesSRP);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00016272 File Offset: 0x00014472
		public virtual void OnDisable()
		{
			RenderPipelineManager.beginCameraRendering -= new Action<ScriptableRenderContext, Camera>(this.DrawShapesSRP);
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00016285 File Offset: 0x00014485
		private void DrawShapesSRP(ScriptableRenderContext ctx, Camera cam)
		{
			this.OnCameraPreRender(cam);
		}

		// Token: 0x04000107 RID: 263
		[Tooltip("When enabled, shapes will only draw in cameras that can see the layer of this GameObject")]
		public bool useCullingMasks;
	}
}
