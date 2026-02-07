using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace EPOOutline
{
	// Token: 0x02000004 RID: 4
	public static class PipelineFetcher
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002138 File Offset: 0x00000338
		public static RenderPipelineAsset CurrentAsset
		{
			get
			{
				RenderPipelineAsset renderPipelineAsset = QualitySettings.renderPipeline;
				if (renderPipelineAsset == null)
				{
					renderPipelineAsset = GraphicsSettings.renderPipelineAsset;
				}
				return renderPipelineAsset;
			}
		}
	}
}
