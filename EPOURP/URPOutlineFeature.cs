using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace EPOOutline
{
	// Token: 0x02000003 RID: 3
	public class URPOutlineFeature : ScriptableRendererFeature
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020B8 File Offset: 0x000002B8
		private bool GetOutlinersToRenderWith(RenderingData renderingData, List<Outliner> outliners)
		{
			outliners.Clear();
			GameObject gameObject = renderingData.cameraData.camera.gameObject;
			gameObject.GetComponents<Outliner>(outliners);
			if (outliners.Count == 0)
			{
				return false;
			}
			bool flag = outliners.Count > 0;
			if (flag)
			{
				this.lastSelectedCamera = gameObject;
			}
			return flag;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002100 File Offset: 0x00000300
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (!this.GetOutlinersToRenderWith(renderingData, this.outliners))
			{
				return;
			}
			foreach (Outliner outliner in this.outliners)
			{
				URPOutlineFeature.SRPOutline srpoutline = this.outlinePool.Get();
				srpoutline.Outliner = outliner;
				srpoutline.Renderer = renderer;
				srpoutline.renderPassEvent = ((outliner.RenderStage == 1) ? RenderPassEvent.AfterRenderingTransparents : RenderPassEvent.AfterRenderingOpaques);
				renderer.EnqueuePass(srpoutline);
			}
			this.outlinePool.ReleaseAll();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021A8 File Offset: 0x000003A8
		public override void Create()
		{
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021AA File Offset: 0x000003AA
		protected override void Dispose(bool disposing)
		{
			URPOutlineFeature.Pool pool = this.outlinePool;
			if (pool == null)
			{
				return;
			}
			pool.Dispose();
		}

		// Token: 0x04000001 RID: 1
		private GameObject lastSelectedCamera;

		// Token: 0x04000002 RID: 2
		private URPOutlineFeature.Pool outlinePool = new URPOutlineFeature.Pool();

		// Token: 0x04000003 RID: 3
		private List<Outliner> outliners = new List<Outliner>();

		// Token: 0x02000006 RID: 6
		private class SRPOutline : ScriptableRenderPass, IDisposable
		{
			// Token: 0x06000008 RID: 8 RVA: 0x000021DA File Offset: 0x000003DA
			private bool IsDepthTextureAvailable(ScriptableRenderer renderer)
			{
				return renderer.cameraDepthTargetHandle.rt != null;
			}

			// Token: 0x06000009 RID: 9 RVA: 0x000021ED File Offset: 0x000003ED
			private RenderTargetIdentifier GetDepthTarget(ScriptableRenderer renderer)
			{
				return this.Renderer.cameraDepthTargetHandle;
			}

			// Token: 0x0600000A RID: 10 RVA: 0x000021FF File Offset: 0x000003FF
			private RenderTargetIdentifier GetColorTarget(ScriptableRenderer renderer)
			{
				return renderer.cameraColorTargetHandle;
			}

			// Token: 0x0600000B RID: 11 RVA: 0x0000220C File Offset: 0x0000040C
			private void Setup(OutlineParameters parameters)
			{
				if (this.Outliner.RenderingStrategy == null)
				{
					OutlineEffect.SetupOutline(parameters);
					parameters.BlitMesh = null;
					parameters.MeshPool.ReleaseAllMeshes();
					return;
				}
				URPOutlineFeature.SRPOutline.temporaryOutlinables.Clear();
				URPOutlineFeature.SRPOutline.temporaryOutlinables.AddRange(parameters.OutlinablesToRender);
				parameters.OutlinablesToRender.Clear();
				parameters.OutlinablesToRender.Add(null);
				foreach (Outlinable value in URPOutlineFeature.SRPOutline.temporaryOutlinables)
				{
					parameters.OutlinablesToRender[0] = value;
					OutlineEffect.SetupOutline(parameters);
					parameters.BlitMesh = null;
				}
				parameters.MeshPool.ReleaseAllMeshes();
			}

			// Token: 0x0600000C RID: 12 RVA: 0x000022D4 File Offset: 0x000004D4
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				BasicCommandBufferWrapper basicCommandBufferWrapper = this.Parameters.Buffer as BasicCommandBufferWrapper;
				if (basicCommandBufferWrapper != null)
				{
					if (basicCommandBufferWrapper.UnderlyingBuffer == null)
					{
						basicCommandBufferWrapper.SetCommandBuffer(new CommandBuffer
						{
							name = "EPO"
						});
					}
					else
					{
						basicCommandBufferWrapper.UnderlyingBuffer.Clear();
					}
				}
				else
				{
					basicCommandBufferWrapper = null;
				}
				Outliner outliner = this.Outliner;
				if (outliner == null || !outliner.enabled)
				{
					return;
				}
				Outlinable.GetAllActiveOutlinables(this.Parameters.OutlinablesToRender);
				this.Outliner.UpdateSharedParameters(this.Parameters, renderingData.cameraData.camera, renderingData.cameraData.isSceneViewCamera, false, false);
				RendererFilteringUtility.Filter(renderingData.cameraData.camera, this.Parameters);
				this.Parameters.TargetWidth = renderingData.cameraData.cameraTargetDescriptor.width;
				this.Parameters.TargetHeight = renderingData.cameraData.cameraTargetDescriptor.height;
				this.Parameters.Viewport = new Rect(0f, 0f, (float)this.Parameters.TargetWidth, (float)this.Parameters.TargetHeight);
				ValueTuple<int, int> scaledSize = this.Parameters.ScaledSize;
				this.Parameters.ScaledBufferWidth = scaledSize.Item1;
				this.Parameters.ScaledBufferHeight = scaledSize.Item2;
				this.Parameters.Antialiasing = renderingData.cameraData.cameraTargetDescriptor.msaaSamples;
				this.Parameters.Target = OutlineEffect.HandleSystem.Alloc(RenderTargetUtility.ComposeTarget(this.Parameters, this.GetColorTarget(this.Renderer)));
				this.Parameters.DepthTarget = OutlineEffect.HandleSystem.Alloc(RenderTargetUtility.ComposeTarget(this.Parameters, (!this.IsDepthTextureAvailable(this.Renderer)) ? this.GetColorTarget(this.Renderer) : this.GetDepthTarget(this.Renderer)));
				this.Outliner.ReplaceHandles(this.Parameters);
				this.Setup(this.Parameters);
				if (basicCommandBufferWrapper != null)
				{
					context.ExecuteCommandBuffer(basicCommandBufferWrapper.UnderlyingBuffer);
				}
			}

			// Token: 0x0600000D RID: 13 RVA: 0x000024DE File Offset: 0x000006DE
			public void Dispose()
			{
				OutlineParameters parameters = this.Parameters;
				if (parameters == null)
				{
					return;
				}
				parameters.Dispose();
			}

			// Token: 0x0400000B RID: 11
			private static FieldInfo nameId = typeof(RenderTargetIdentifier).GetField("m_NameID", BindingFlags.Instance | BindingFlags.NonPublic);

			// Token: 0x0400000C RID: 12
			private static List<Outlinable> temporaryOutlinables = new List<Outlinable>();

			// Token: 0x0400000D RID: 13
			public ScriptableRenderer Renderer;

			// Token: 0x0400000E RID: 14
			public Outliner Outliner;

			// Token: 0x0400000F RID: 15
			private OutlineParameters Parameters = new OutlineParameters(new BasicCommandBufferWrapper(null));

			// Token: 0x04000010 RID: 16
			private List<Outliner> outliners = new List<Outliner>();
		}

		// Token: 0x02000007 RID: 7
		private class Pool : IDisposable
		{
			// Token: 0x06000010 RID: 16 RVA: 0x0000253C File Offset: 0x0000073C
			public URPOutlineFeature.SRPOutline Get()
			{
				if (this.outlines.Count != 0)
				{
					return this.outlines.Pop();
				}
				this.outlines.Push(new URPOutlineFeature.SRPOutline());
				this.createdOutlines.Add(this.outlines.Peek());
				return this.outlines.Pop();
			}

			// Token: 0x06000011 RID: 17 RVA: 0x00002594 File Offset: 0x00000794
			public void ReleaseAll()
			{
				this.outlines.Clear();
				foreach (URPOutlineFeature.SRPOutline item in this.createdOutlines)
				{
					this.outlines.Push(item);
				}
			}

			// Token: 0x06000012 RID: 18 RVA: 0x000025F8 File Offset: 0x000007F8
			public void Dispose()
			{
				foreach (URPOutlineFeature.SRPOutline srpoutline in this.createdOutlines)
				{
					if (srpoutline != null)
					{
						srpoutline.Dispose();
					}
				}
			}

			// Token: 0x04000011 RID: 17
			private Stack<URPOutlineFeature.SRPOutline> outlines = new Stack<URPOutlineFeature.SRPOutline>();

			// Token: 0x04000012 RID: 18
			private List<URPOutlineFeature.SRPOutline> createdOutlines = new List<URPOutlineFeature.SRPOutline>();
		}
	}
}
