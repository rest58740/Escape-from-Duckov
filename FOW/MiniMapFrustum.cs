using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace FOW
{
	// Token: 0x02000004 RID: 4
	public class MiniMapFrustum : MonoBehaviour
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000021EE File Offset: 0x000003EE
		public RenderTexture GetFrustumRT()
		{
			return this.Frustum_RT;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021F8 File Offset: 0x000003F8
		private void Start()
		{
			this.blitMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
			this.blitMaterial.SetInt("_SrcBlend", 4);
			this.blitMaterial.SetInt("_DstBlend", 0);
			this.blitMaterial.SetInt("_Cull", 0);
			this.blitMaterial.SetInt("_ZWrite", 0);
			this.blitMaterial.SetInt("_ZTest", 8);
			this.points = new Vector3[4];
			this.screenPositions = new Vector2[4];
			this.UVs = new Vector2[4];
			this.InitFrustumRT();
			this.fallbackPlane = default(Plane);
			this.fallbackPlane.SetNormalAndPosition(FogOfWarWorld.UpVector, this.MapCollider.transform.position);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000022C8 File Offset: 0x000004C8
		private void InitFrustumRT()
		{
			this.Frustum_RT = new RenderTexture(this.ResolutionX, this.ResolutionY, 0);
			this.Frustum_RT.format = RenderTextureFormat.ARGBHalf;
			this.Frustum_RT.antiAliasing = 8;
			this.Frustum_RT.filterMode = FilterMode.Trilinear;
			this.Frustum_RT.anisoLevel = 9;
			this.Frustum_RT.Create();
			RenderTexture.active = this.Frustum_RT;
			Material material = new Material(Shader.Find("Hidden/Internal-Colored"));
			material.SetInt("_SrcBlend", 4);
			material.SetInt("_DstBlend", 0);
			material.SetInt("_Cull", 0);
			material.SetInt("_ZWrite", 0);
			material.SetInt("_ZTest", 8);
			material.SetPass(0);
			GL.Begin(4);
			GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
			GL.End();
			if (this.RawImageComponent != null)
			{
				this.RawImageComponent.texture = this.Frustum_RT;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023D4 File Offset: 0x000005D4
		private Vector3 GetWorldSpaceFrustomCorner(Vector2 ScreenPosition)
		{
			Ray ray = Camera.main.ScreenPointToRay(ScreenPosition);
			if (this.MapCollider.Raycast(ray, out this.rayHit, this.RayDistance))
			{
				return this.rayHit.point;
			}
			float distance;
			this.fallbackPlane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002430 File Offset: 0x00000630
		private void SetScreenPositions()
		{
			this.screenPositions[0].x = 0f;
			this.screenPositions[0].y = 0f;
			this.screenPositions[1].x = 0f;
			this.screenPositions[1].y = (float)Screen.height;
			this.screenPositions[2].x = (float)Screen.width;
			this.screenPositions[2].y = (float)Screen.height;
			this.screenPositions[3].x = (float)Screen.width;
			this.screenPositions[3].y = 0f;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000024F4 File Offset: 0x000006F4
		private void Update()
		{
			this.SetScreenPositions();
			this.points[0] = this.GetWorldSpaceFrustomCorner(this.screenPositions[0]);
			this.points[1] = this.GetWorldSpaceFrustomCorner(this.screenPositions[1]);
			this.points[2] = this.GetWorldSpaceFrustomCorner(this.screenPositions[2]);
			this.points[3] = this.GetWorldSpaceFrustomCorner(this.screenPositions[3]);
			this._worldBounds = FogOfWarWorld.instance.GetBoundsVectorForShader();
			this.frustumCenterUV.x = 0f;
			this.frustumCenterUV.y = 0f;
			for (int i = 0; i < 4; i++)
			{
				this.UVs[i] = this.GetUV(this.points[i]);
				this.frustumCenterUV += this.UVs[i];
			}
			this.frustumCenterUV /= 4f;
			this.DrawMiniMapFrustum();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002614 File Offset: 0x00000814
		private Vector2 GetUV(Vector3 WorldPosition)
		{
			Vector2 fowPositionFromWorldPosition = FogOfWarWorld.instance.GetFowPositionFromWorldPosition(WorldPosition);
			return new Vector2((fowPositionFromWorldPosition.x - this._worldBounds.y + this._worldBounds.x / 2f) / this._worldBounds.x, (fowPositionFromWorldPosition.y - this._worldBounds.w + this._worldBounds.z / 2f) / this._worldBounds.z);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002694 File Offset: 0x00000894
		private void DrawMiniMapFrustum()
		{
			MiniMapFrustum.<>c__DisplayClass23_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			GL.PushMatrix();
			this.blitMaterial.SetPass(0);
			GL.LoadOrtho();
			RenderTexture.active = this.Frustum_RT;
			GL.Begin(4);
			GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
			GL.End();
			GL.Begin(7);
			GL.Color(this.LineColor);
			CS$<>8__locals1.CrossVector1 = this.frustumCenterUV - this.UVs[0];
			CS$<>8__locals1.CrossVector2 = this.frustumCenterUV - this.UVs[1];
			this.<DrawMiniMapFrustum>g__NormalizeVectors|23_1(ref CS$<>8__locals1);
			this.<DrawMiniMapFrustum>g__DrawLine|23_0(this.UVs[0], this.UVs[1], ref CS$<>8__locals1);
			CS$<>8__locals1.CrossVector1 = this.frustumCenterUV - this.UVs[1];
			CS$<>8__locals1.CrossVector2 = this.frustumCenterUV - this.UVs[2];
			this.<DrawMiniMapFrustum>g__NormalizeVectors|23_1(ref CS$<>8__locals1);
			this.<DrawMiniMapFrustum>g__DrawLine|23_0(this.UVs[1], this.UVs[2], ref CS$<>8__locals1);
			CS$<>8__locals1.CrossVector1 = this.frustumCenterUV - this.UVs[2];
			CS$<>8__locals1.CrossVector2 = this.frustumCenterUV - this.UVs[3];
			this.<DrawMiniMapFrustum>g__NormalizeVectors|23_1(ref CS$<>8__locals1);
			this.<DrawMiniMapFrustum>g__DrawLine|23_0(this.UVs[2], this.UVs[3], ref CS$<>8__locals1);
			CS$<>8__locals1.CrossVector1 = this.frustumCenterUV - this.UVs[3];
			CS$<>8__locals1.CrossVector2 = this.frustumCenterUV - this.UVs[0];
			this.<DrawMiniMapFrustum>g__NormalizeVectors|23_1(ref CS$<>8__locals1);
			this.<DrawMiniMapFrustum>g__DrawLine|23_0(this.UVs[3], this.UVs[0], ref CS$<>8__locals1);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000028E0 File Offset: 0x00000AE0
		[CompilerGenerated]
		private void <DrawMiniMapFrustum>g__DrawLine|23_0(Vector2 uv1, Vector2 uv2, ref MiniMapFrustum.<>c__DisplayClass23_0 A_3)
		{
			GL.Vertex(new Vector3(uv1.x, uv1.y, 0f));
			GL.Vertex(new Vector3(uv2.x, uv2.y, 0f));
			GL.Vertex(new Vector3(uv2.x + A_3.CrossVector2.x, uv2.y + A_3.CrossVector2.y, 0f));
			GL.Vertex(new Vector3(uv1.x + A_3.CrossVector1.x, uv1.y + A_3.CrossVector1.y, 0f));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002989 File Offset: 0x00000B89
		[CompilerGenerated]
		private void <DrawMiniMapFrustum>g__NormalizeVectors|23_1(ref MiniMapFrustum.<>c__DisplayClass23_0 A_1)
		{
			A_1.CrossVector1 *= this.LineWidth;
			A_1.CrossVector2 *= this.LineWidth;
		}

		// Token: 0x04000002 RID: 2
		public Collider MapCollider;

		// Token: 0x04000003 RID: 3
		public float RayDistance = 100f;

		// Token: 0x04000004 RID: 4
		public int ResolutionX = 256;

		// Token: 0x04000005 RID: 5
		public int ResolutionY = 256;

		// Token: 0x04000006 RID: 6
		public RawImage RawImageComponent;

		// Token: 0x04000007 RID: 7
		public Color LineColor = Color.white;

		// Token: 0x04000008 RID: 8
		public float LineWidth = 0.05f;

		// Token: 0x04000009 RID: 9
		private Plane fallbackPlane;

		// Token: 0x0400000A RID: 10
		private Material blitMaterial;

		// Token: 0x0400000B RID: 11
		private RenderTexture Frustum_RT;

		// Token: 0x0400000C RID: 12
		private Vector4 _worldBounds;

		// Token: 0x0400000D RID: 13
		private Vector3[] points;

		// Token: 0x0400000E RID: 14
		private Vector2[] screenPositions;

		// Token: 0x0400000F RID: 15
		private Vector2[] UVs;

		// Token: 0x04000010 RID: 16
		private Vector2 frustumCenterUV;

		// Token: 0x04000011 RID: 17
		private RaycastHit rayHit;
	}
}
