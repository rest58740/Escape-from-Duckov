using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace EPOOutline
{
	// Token: 0x02000003 RID: 3
	public class BasicCommandBufferWrapper : CommandBufferWrapper, IUnderlyingBufferProvider, IDisposable
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		private static RenderTargetIdentifier ConvertToRTI(RTHandle handle)
		{
			if (!(handle.rt != null))
			{
				return handle.nameID;
			}
			return handle.rt;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020E2 File Offset: 0x000002E2
		public BasicCommandBufferWrapper(CommandBuffer buffer)
		{
			this.buffer = buffer;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020F1 File Offset: 0x000002F1
		public CommandBuffer UnderlyingBuffer
		{
			get
			{
				return this.buffer;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020F9 File Offset: 0x000002F9
		public void SetCommandBuffer(CommandBuffer buffer)
		{
			if (this.buffer != null)
			{
				this.buffer.Release();
			}
			this.buffer = buffer;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002115 File Offset: 0x00000315
		public override void Clear()
		{
			this.buffer.Clear();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002122 File Offset: 0x00000322
		public override void SetGlobalInt(int hash, int value)
		{
			this.buffer.SetGlobalInt(hash, value);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002131 File Offset: 0x00000331
		public override void SetGlobalFloat(int hash, float value)
		{
			this.buffer.SetGlobalFloat(hash, value);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002140 File Offset: 0x00000340
		public override void SetGlobalVector(int hash, Vector4 value)
		{
			this.buffer.SetGlobalVector(hash, value);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000214F File Offset: 0x0000034F
		public override void SetGlobalColor(int hash, Color color)
		{
			this.buffer.SetGlobalColor(hash, color);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000215E File Offset: 0x0000035E
		public override void SetGlobalTexture(int hash, RTHandle texture)
		{
			this.buffer.SetGlobalTexture(hash, BasicCommandBufferWrapper.ConvertToRTI(texture));
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002174 File Offset: 0x00000374
		public override void SetRenderTarget(RTHandle color, int slice)
		{
			RenderTargetIdentifier renderTarget = new RenderTargetIdentifier(BasicCommandBufferWrapper.ConvertToRTI(color), 0, CubemapFace.Unknown, slice);
			this.buffer.SetRenderTarget(renderTarget);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021A0 File Offset: 0x000003A0
		public override void SetRenderTarget(RTHandle color, RTHandle depth, int slice)
		{
			RenderTargetIdentifier color2 = new RenderTargetIdentifier(BasicCommandBufferWrapper.ConvertToRTI(color), 0, CubemapFace.Unknown, slice);
			RenderTargetIdentifier depth2 = new RenderTargetIdentifier(BasicCommandBufferWrapper.ConvertToRTI(depth), 0, CubemapFace.Unknown, slice);
			this.buffer.SetRenderTarget(color2, depth2);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021DA File Offset: 0x000003DA
		public override void SetViewport(Rect rect)
		{
			this.buffer.SetViewport(rect);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021E8 File Offset: 0x000003E8
		public override void DisableShaderKeyword(string keyword)
		{
			CommandBuffer commandBuffer = this.buffer;
			GlobalKeyword globalKeyword = GlobalKeyword.Create(keyword);
			commandBuffer.DisableKeyword(globalKeyword);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000220C File Offset: 0x0000040C
		public override void EnableShaderKeyword(string keyword)
		{
			CommandBuffer commandBuffer = this.buffer;
			GlobalKeyword globalKeyword = GlobalKeyword.Create(keyword);
			commandBuffer.EnableKeyword(globalKeyword);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000222D File Offset: 0x0000042D
		public override void ClearRenderTarget(bool depth, bool clr, Color clearColor)
		{
			this.buffer.ClearRenderTarget(depth, clr, clearColor);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000223D File Offset: 0x0000043D
		public override void DrawRenderer(Renderer target, Material material, int submesh)
		{
			this.buffer.DrawRenderer(target, material, submesh);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000224D File Offset: 0x0000044D
		public override void DrawMeshInstanced(Mesh mesh, int submesh, Material material, int pass, Matrix4x4[] matrices, int countToDraw, MaterialPropertyBlock block)
		{
			this.buffer.DrawMeshInstanced(mesh, submesh, material, pass, matrices, countToDraw, block);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002265 File Offset: 0x00000465
		public override void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submesh, int pass)
		{
			this.buffer.DrawMesh(mesh, matrix, material, submesh, pass);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002279 File Offset: 0x00000479
		public void Dispose()
		{
			CommandBuffer commandBuffer = this.buffer;
			if (commandBuffer != null)
			{
				commandBuffer.Dispose();
			}
			this.buffer = null;
		}

		// Token: 0x04000001 RID: 1
		private CommandBuffer buffer;
	}
}
