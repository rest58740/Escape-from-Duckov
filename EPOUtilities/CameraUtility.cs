using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace EPOOutline
{
	// Token: 0x02000003 RID: 3
	public static class CameraUtility
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
		public static int GetMSAA(Camera camera)
		{
			if (camera.targetTexture != null)
			{
				return camera.targetTexture.antiAliasing;
			}
			int result = Mathf.Max(CameraUtility.GetRenderPipelineMSAA(), 1);
			if (!camera.allowMSAA)
			{
				result = 1;
			}
			if (camera.actualRenderingPath != RenderingPath.Forward && camera.actualRenderingPath != RenderingPath.VertexLit)
			{
				result = 1;
			}
			return result;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002110 File Offset: 0x00000310
		private static int GetRenderPipelineMSAA()
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = PipelineFetcher.CurrentAsset as UniversalRenderPipelineAsset;
			if (universalRenderPipelineAsset != null)
			{
				return universalRenderPipelineAsset.msaaSampleCount;
			}
			return QualitySettings.antiAliasing;
		}
	}
}
