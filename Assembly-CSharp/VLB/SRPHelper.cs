using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace VLB
{
	// Token: 0x02000041 RID: 65
	public static class SRPHelper
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00009C5F File Offset: 0x00007E5F
		public static string renderPipelineScriptingDefineSymbolAsString
		{
			get
			{
				return "VLB_URP";
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00009C66 File Offset: 0x00007E66
		public static RenderPipeline projectRenderPipeline
		{
			get
			{
				if (!SRPHelper.m_IsRenderPipelineCached)
				{
					SRPHelper.m_RenderPipelineCached = SRPHelper.ComputeRenderPipeline();
					SRPHelper.m_IsRenderPipelineCached = true;
				}
				return SRPHelper.m_RenderPipelineCached;
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00009C84 File Offset: 0x00007E84
		private static RenderPipeline ComputeRenderPipeline()
		{
			RenderPipelineAsset defaultRenderPipeline = GraphicsSettings.defaultRenderPipeline;
			if (defaultRenderPipeline)
			{
				string text = defaultRenderPipeline.GetType().ToString();
				if (text.Contains("Universal"))
				{
					return RenderPipeline.URP;
				}
				if (text.Contains("Lightweight"))
				{
					return RenderPipeline.URP;
				}
				if (text.Contains("HD"))
				{
					return RenderPipeline.HDRP;
				}
			}
			return RenderPipeline.BuiltIn;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00009CD9 File Offset: 0x00007ED9
		public static bool IsUsingCustomRenderPipeline()
		{
			return RenderPipelineManager.currentPipeline != null || GraphicsSettings.defaultRenderPipeline != null;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00009CEF File Offset: 0x00007EEF
		public static void RegisterOnBeginCameraRendering(Action<ScriptableRenderContext, Camera> cb)
		{
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				RenderPipelineManager.beginCameraRendering -= cb;
				RenderPipelineManager.beginCameraRendering += cb;
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00009D04 File Offset: 0x00007F04
		public static void UnregisterOnBeginCameraRendering(Action<ScriptableRenderContext, Camera> cb)
		{
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				RenderPipelineManager.beginCameraRendering -= cb;
			}
		}

		// Token: 0x04000192 RID: 402
		private static bool m_IsRenderPipelineCached;

		// Token: 0x04000193 RID: 403
		private static RenderPipeline m_RenderPipelineCached;
	}
}
