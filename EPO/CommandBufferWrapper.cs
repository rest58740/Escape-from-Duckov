using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace EPOOutline
{
	// Token: 0x02000009 RID: 9
	public abstract class CommandBufferWrapper
	{
		// Token: 0x06000021 RID: 33
		public abstract void Clear();

		// Token: 0x06000022 RID: 34
		public abstract void SetGlobalInt(int hash, int value);

		// Token: 0x06000023 RID: 35
		public abstract void SetGlobalFloat(int hash, float value);

		// Token: 0x06000024 RID: 36
		public abstract void SetGlobalVector(int hash, Vector4 value);

		// Token: 0x06000025 RID: 37
		public abstract void SetGlobalColor(int hash, Color color);

		// Token: 0x06000026 RID: 38
		public abstract void SetGlobalTexture(int hash, RTHandle texture);

		// Token: 0x06000027 RID: 39
		public abstract void SetRenderTarget(RTHandle color, int slice);

		// Token: 0x06000028 RID: 40
		public abstract void SetRenderTarget(RTHandle color, RTHandle depth, int slice);

		// Token: 0x06000029 RID: 41
		public abstract void SetViewport(Rect rect);

		// Token: 0x0600002A RID: 42
		public abstract void DisableShaderKeyword(string keyword);

		// Token: 0x0600002B RID: 43
		public abstract void EnableShaderKeyword(string keyword);

		// Token: 0x0600002C RID: 44
		public abstract void ClearRenderTarget(bool depth, bool color, Color clearColor);

		// Token: 0x0600002D RID: 45
		public abstract void DrawRenderer(Renderer target, Material material, int submesh);

		// Token: 0x0600002E RID: 46
		public abstract void DrawMeshInstanced(Mesh mesh, int submesh, Material material, int pass, Matrix4x4[] matrices, int countToDraw, MaterialPropertyBlock block);

		// Token: 0x0600002F RID: 47
		public abstract void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submesh, int pass);
	}
}
