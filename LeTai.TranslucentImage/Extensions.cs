using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LeTai.Asset.TranslucentImage
{
	// Token: 0x0200000D RID: 13
	public static class Extensions
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000321C File Offset: 0x0000141C
		private static Mesh FullscreenTriangle
		{
			get
			{
				if (Extensions.fullscreenTriangle != null)
				{
					return Extensions.fullscreenTriangle;
				}
				Extensions.fullscreenTriangle = new Mesh
				{
					name = "Fullscreen Triangle"
				};
				Extensions.fullscreenTriangle.SetVertices(new List<Vector3>
				{
					new Vector3(-1f, -1f, 0f),
					new Vector3(-1f, 3f, 0f),
					new Vector3(3f, -1f, 0f)
				});
				Extensions.fullscreenTriangle.SetIndices(new int[]
				{
					0,
					1,
					2
				}, MeshTopology.Triangles, 0, false);
				Extensions.fullscreenTriangle.UploadMeshData(false);
				return Extensions.fullscreenTriangle;
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000032D9 File Offset: 0x000014D9
		public static void BlitCustom(this CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, int passIndex, bool useBuiltin = false)
		{
			if (useBuiltin)
			{
				cmd.Blit(source, destination, material, passIndex);
				return;
			}
			if (SystemInfo.graphicsShaderLevel >= 30 && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2)
			{
				cmd.BlitProcedural(source, destination, material, passIndex);
				return;
			}
			cmd.BlitFullscreenTriangle(source, destination, material, passIndex);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003313 File Offset: 0x00001513
		public static void BlitFullscreenTriangle(this CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, int pass)
		{
			cmd.SetGlobalTexture("_MainTex", source);
			cmd.SetRenderTarget(destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
			cmd.DrawMesh(Extensions.FullscreenTriangle, Matrix4x4.identity, material, 0, pass);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000333E File Offset: 0x0000153E
		public static void BlitProcedural(this CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, int passIndex)
		{
			cmd.SetGlobalTexture(ShaderId.MAIN_TEX, source);
			cmd.SetRenderTarget(new RenderTargetIdentifier(destination, 0, CubemapFace.Unknown, -1), RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
			cmd.DrawProcedural(Matrix4x4.identity, material, passIndex, MeshTopology.Quads, 4, 1, null);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003374 File Offset: 0x00001574
		internal static bool Approximately(this Rect self, Rect other)
		{
			return Extensions.QuickApproximate(self.x, other.x) && Extensions.QuickApproximate(self.y, other.y) && Extensions.QuickApproximate(self.width, other.width) && Extensions.QuickApproximate(self.height, other.height);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000033D5 File Offset: 0x000015D5
		private static bool QuickApproximate(float a, float b)
		{
			return Mathf.Abs(b - a) < 5.9604645E-08f;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000033E6 File Offset: 0x000015E6
		public static Vector4 ToMinMaxVector(this Rect self)
		{
			return new Vector4(self.xMin, self.yMin, self.xMax, self.yMax);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003409 File Offset: 0x00001609
		public static Vector4 ToVector4(this Rect self)
		{
			return new Vector4(self.xMin, self.yMin, self.width, self.height);
		}

		// Token: 0x04000036 RID: 54
		private static Mesh fullscreenTriangle;

		// Token: 0x04000037 RID: 55
		private const float EPSILON01 = 5.9604645E-08f;
	}
}
