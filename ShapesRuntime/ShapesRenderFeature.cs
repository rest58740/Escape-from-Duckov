using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Shapes
{
	// Token: 0x0200003D RID: 61
	public class ShapesRenderFeature : ScriptableRendererFeature
	{
		// Token: 0x06000C1D RID: 3101 RVA: 0x000185E3 File Offset: 0x000167E3
		public override void Create()
		{
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x000185E8 File Offset: 0x000167E8
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			Camera camera = renderingData.cameraData.camera;
			List<DrawCommand> list;
			if (DrawCommand.cBuffersRendering.TryGetValue(camera, ref list))
			{
				foreach (DrawCommand drawCommand in list)
				{
					renderer.EnqueuePass(ObjectPool<ShapesRenderPass>.Alloc().Init(drawCommand));
				}
			}
		}
	}
}
