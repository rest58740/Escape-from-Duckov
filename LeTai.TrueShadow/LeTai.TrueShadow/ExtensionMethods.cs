using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LeTai
{
	// Token: 0x02000003 RID: 3
	public static class ExtensionMethods
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public static Vector4 ToMinMaxVector(this Rect self)
		{
			return new Vector4(self.xMin, self.yMin, self.xMax, self.yMax);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020E4 File Offset: 0x000002E4
		private static Mesh FullscreenTriangle
		{
			get
			{
				if (ExtensionMethods.fullscreenTriangle != null)
				{
					return ExtensionMethods.fullscreenTriangle;
				}
				ExtensionMethods.fullscreenTriangle = new Mesh
				{
					name = "Fullscreen Triangle"
				};
				ExtensionMethods.fullscreenTriangle.SetVertices(new List<Vector3>
				{
					new Vector3(-1f, -1f, 0f),
					new Vector3(-1f, 3f, 0f),
					new Vector3(3f, -1f, 0f)
				});
				ExtensionMethods.fullscreenTriangle.SetIndices(new int[]
				{
					0,
					1,
					2
				}, MeshTopology.Triangles, 0, false);
				ExtensionMethods.fullscreenTriangle.UploadMeshData(false);
				return ExtensionMethods.fullscreenTriangle;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021A1 File Offset: 0x000003A1
		public static void BlitFullscreenTriangle(this CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, int pass = 0)
		{
			cmd.SetGlobalTexture("_MainTex", source);
			cmd.SetRenderTarget(destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
			cmd.DrawMesh(ExtensionMethods.FullscreenTriangle, Matrix4x4.identity, material, 0, pass);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021CC File Offset: 0x000003CC
		internal static bool Approximately(this Rect self, Rect other)
		{
			return ExtensionMethods.QuickApproximate(self.x, other.x) && ExtensionMethods.QuickApproximate(self.y, other.y) && ExtensionMethods.QuickApproximate(self.width, other.width) && ExtensionMethods.QuickApproximate(self.height, other.height);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000222D File Offset: 0x0000042D
		private static bool QuickApproximate(float a, float b)
		{
			return Mathf.Abs(b - a) < 1.175494E-38f;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000223E File Offset: 0x0000043E
		public static Vector3 WithZ(this Vector2 self, float z)
		{
			return new Vector3(self.x, self.y, z);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002252 File Offset: 0x00000452
		public static Color WithA(this Color self, float a)
		{
			return new Color(self.r, self.g, self.b, a);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000226C File Offset: 0x0000046C
		public static void NextFrames(this MonoBehaviour behaviour, Action action, int nFrames = 1)
		{
			behaviour.StartCoroutine(ExtensionMethods.NextFrame(action, nFrames));
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000227C File Offset: 0x0000047C
		private static IEnumerator NextFrame(Action action, int nFrames)
		{
			int num;
			for (int i = 0; i < nFrames; i = num + 1)
			{
				yield return null;
				num = i;
			}
			action();
			yield break;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002292 File Offset: 0x00000492
		public static void SetKeyword(this Material material, string keyword, bool enabled)
		{
			if (enabled)
			{
				material.EnableKeyword(keyword);
				return;
			}
			material.DisableKeyword(keyword);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022A6 File Offset: 0x000004A6
		public static Vector2 Frac(this Vector2 vec)
		{
			return new Vector2(vec.x - Mathf.Floor(vec.x), vec.y - Mathf.Floor(vec.y));
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022D1 File Offset: 0x000004D1
		public static Vector2 LocalToScreenPoint(this RectTransform rt, Vector3 localPoint, Camera referenceCamera = null)
		{
			return RectTransformUtility.WorldToScreenPoint(referenceCamera, rt.TransformPoint(localPoint));
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022E0 File Offset: 0x000004E0
		public static Vector2 ScreenToCanvasSize(this RectTransform rt, Vector2 size, Camera referenceCamera = null)
		{
			Vector2 b;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Vector2.zero, referenceCamera, out b);
			Vector2 a;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, size, referenceCamera, out a);
			return a - b;
		}

		// Token: 0x04000001 RID: 1
		private static Mesh fullscreenTriangle;
	}
}
