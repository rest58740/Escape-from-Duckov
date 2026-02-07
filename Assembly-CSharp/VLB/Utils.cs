using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000044 RID: 68
	public static class Utils
	{
		// Token: 0x06000269 RID: 617 RVA: 0x0000A00D File Offset: 0x0000820D
		public static float ComputeConeRadiusEnd(float fallOffEnd, float spotAngle)
		{
			return fallOffEnd * Mathf.Tan(spotAngle * 0.017453292f * 0.5f);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000A023 File Offset: 0x00008223
		public static float ComputeSpotAngle(float fallOffEnd, float coneRadiusEnd)
		{
			return Mathf.Atan2(coneRadiusEnd, fallOffEnd) * 57.29578f * 2f;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000A038 File Offset: 0x00008238
		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000A05F File Offset: 0x0000825F
		public static void ResizeArray<T>(ref T[] array, int newSize)
		{
			Array.Resize<T>(ref array, newSize);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000A068 File Offset: 0x00008268
		public static bool IsValidIndex<T>(this T[] array, int idx)
		{
			return idx >= 0 && idx < array.Length;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000A076 File Offset: 0x00008276
		public static string GetPath(Transform current)
		{
			if (current.parent == null)
			{
				return "/" + current.name;
			}
			return Utils.GetPath(current.parent) + "/" + current.name;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000A0B2 File Offset: 0x000082B2
		public static T NewWithComponent<T>(string name) where T : Component
		{
			return new GameObject(name, new Type[]
			{
				typeof(T)
			}).GetComponent<T>();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000A0D4 File Offset: 0x000082D4
		public static T GetOrAddComponent<T>(this GameObject self) where T : Component
		{
			T t = self.GetComponent<T>();
			if (t == null)
			{
				t = self.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000A0FE File Offset: 0x000082FE
		public static T GetOrAddComponent<T>(this MonoBehaviour self) where T : Component
		{
			return self.gameObject.GetOrAddComponent<T>();
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000A10C File Offset: 0x0000830C
		public static void ForeachComponentsInAnyChildrenOnly<T>(this GameObject self, Action<T> lambda, bool includeInactive = false) where T : Component
		{
			foreach (T t in self.GetComponentsInChildren<T>(includeInactive))
			{
				if (t.gameObject != self)
				{
					lambda(t);
				}
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000A154 File Offset: 0x00008354
		public static void ForeachComponentsInDirectChildrenOnly<T>(this GameObject self, Action<T> lambda, bool includeInactive = false) where T : Component
		{
			foreach (T t in self.GetComponentsInChildren<T>(includeInactive))
			{
				if (t.transform.parent == self.transform)
				{
					lambda(t);
				}
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000A1A4 File Offset: 0x000083A4
		public static void SetupDepthCamera(Camera depthCamera, float coneApexOffsetZ, float maxGeometryDistance, float coneRadiusStart, float coneRadiusEnd, Vector3 beamLocalForward, Vector3 lossyScale, bool isScalable, Quaternion beamInternalLocalRotation, bool shouldScaleMinNearClipPlane)
		{
			if (!isScalable)
			{
				lossyScale.x = (lossyScale.y = 1f);
			}
			bool flag = coneApexOffsetZ >= 0f;
			float num = Mathf.Max(coneApexOffsetZ, 0f);
			depthCamera.orthographic = !flag;
			depthCamera.transform.localPosition = beamLocalForward * -num;
			Quaternion quaternion = beamInternalLocalRotation;
			if (Mathf.Sign(lossyScale.z) < 0f)
			{
				quaternion *= Quaternion.Euler(0f, 180f, 0f);
			}
			depthCamera.transform.localRotation = quaternion;
			if (!Mathf.Approximately(lossyScale.y * lossyScale.z, 0f))
			{
				float num2 = flag ? 0.1f : 0f;
				float num3 = Mathf.Abs(lossyScale.z);
				depthCamera.nearClipPlane = Mathf.Max(num * num3, num2 * (shouldScaleMinNearClipPlane ? num3 : 1f));
				depthCamera.farClipPlane = (maxGeometryDistance + num * (isScalable ? 1f : num3)) * (isScalable ? num3 : 1f);
				depthCamera.aspect = Mathf.Abs(lossyScale.x / lossyScale.y);
				if (flag)
				{
					float fieldOfView = Mathf.Atan2(coneRadiusEnd * Mathf.Abs(lossyScale.y), depthCamera.farClipPlane) * 57.29578f * 2f;
					depthCamera.fieldOfView = fieldOfView;
					return;
				}
				depthCamera.orthographicSize = coneRadiusStart * lossyScale.y;
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000A31E File Offset: 0x0000851E
		public static bool HasFlag(this Enum mask, Enum flags)
		{
			return ((int)mask & (int)flags) == (int)flags;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000A338 File Offset: 0x00008538
		public static Vector3 Divide(this Vector3 aVector, Vector3 scale)
		{
			if (Mathf.Approximately(scale.x * scale.y * scale.z, 0f))
			{
				return Vector3.zero;
			}
			return new Vector3(aVector.x / scale.x, aVector.y / scale.y, aVector.z / scale.z);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A397 File Offset: 0x00008597
		public static Vector2 xy(this Vector3 aVector)
		{
			return new Vector2(aVector.x, aVector.y);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000A3AA File Offset: 0x000085AA
		public static Vector2 xz(this Vector3 aVector)
		{
			return new Vector2(aVector.x, aVector.z);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000A3BD File Offset: 0x000085BD
		public static Vector2 yz(this Vector3 aVector)
		{
			return new Vector2(aVector.y, aVector.z);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000A3D0 File Offset: 0x000085D0
		public static Vector2 yx(this Vector3 aVector)
		{
			return new Vector2(aVector.y, aVector.x);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000A3E3 File Offset: 0x000085E3
		public static Vector2 zx(this Vector3 aVector)
		{
			return new Vector2(aVector.z, aVector.x);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A3F6 File Offset: 0x000085F6
		public static Vector2 zy(this Vector3 aVector)
		{
			return new Vector2(aVector.z, aVector.y);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A409 File Offset: 0x00008609
		public static bool Approximately(this float a, float b, float epsilon = 1E-05f)
		{
			return Mathf.Abs(a - b) < epsilon;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A416 File Offset: 0x00008616
		public static bool Approximately(this Vector2 a, Vector2 b, float epsilon = 1E-05f)
		{
			return Vector2.SqrMagnitude(a - b) < epsilon;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000A427 File Offset: 0x00008627
		public static bool Approximately(this Vector3 a, Vector3 b, float epsilon = 1E-05f)
		{
			return Vector3.SqrMagnitude(a - b) < epsilon;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000A438 File Offset: 0x00008638
		public static bool Approximately(this Vector4 a, Vector4 b, float epsilon = 1E-05f)
		{
			return Vector4.SqrMagnitude(a - b) < epsilon;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000A449 File Offset: 0x00008649
		public static Vector4 AsVector4(this Vector3 vec3, float w)
		{
			return new Vector4(vec3.x, vec3.y, vec3.z, w);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000A463 File Offset: 0x00008663
		public static Vector4 PlaneEquation(Vector3 normalizedNormal, Vector3 pt)
		{
			return normalizedNormal.AsVector4(-Vector3.Dot(normalizedNormal, pt));
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000A473 File Offset: 0x00008673
		public static float GetVolumeCubic(this Bounds self)
		{
			return self.size.x * self.size.y * self.size.z;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000A49C File Offset: 0x0000869C
		public static float GetMaxArea2D(this Bounds self)
		{
			return Mathf.Max(Mathf.Max(self.size.x * self.size.y, self.size.y * self.size.z), self.size.x * self.size.z);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000A4FE File Offset: 0x000086FE
		public static Color Opaque(this Color self)
		{
			return new Color(self.r, self.g, self.b, 1f);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000A51C File Offset: 0x0000871C
		public static Color ComputeComplementaryColor(this Color self, bool blackAndWhite)
		{
			if (!blackAndWhite)
			{
				return new Color(1f - self.r, 1f - self.g, 1f - self.b);
			}
			if ((double)self.r * 0.299 + (double)self.g * 0.587 + (double)self.b * 0.114 <= 0.729411780834198)
			{
				return Color.white;
			}
			return Color.black;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000A5A1 File Offset: 0x000087A1
		public static Plane TranslateCustom(this Plane plane, Vector3 translation)
		{
			plane.distance += Vector3.Dot(translation.normalized, plane.normal) * translation.magnitude;
			return plane;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000A5CD File Offset: 0x000087CD
		public static Vector3 ClosestPointOnPlaneCustom(this Plane plane, Vector3 point)
		{
			return point - plane.GetDistanceToPoint(point) * plane.normal;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000A5E9 File Offset: 0x000087E9
		public static bool IsAlmostZero(float f)
		{
			return Mathf.Abs(f) < 0.001f;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000A5F8 File Offset: 0x000087F8
		public static bool IsValid(this Plane plane)
		{
			return plane.normal.sqrMagnitude > 0.5f;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000A61B File Offset: 0x0000881B
		public static void SetKeywordEnabled(this Material mat, string name, bool enabled)
		{
			if (enabled)
			{
				mat.EnableKeyword(name);
				return;
			}
			mat.DisableKeyword(name);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000A62F File Offset: 0x0000882F
		public static void SetShaderKeywordEnabled(string name, bool enabled)
		{
			if (enabled)
			{
				Shader.EnableKeyword(name);
				return;
			}
			Shader.DisableKeyword(name);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000A644 File Offset: 0x00008844
		public static Matrix4x4 SampleInMatrix(this Gradient self, int floatPackingPrecision)
		{
			Matrix4x4 result = default(Matrix4x4);
			for (int i = 0; i < 16; i++)
			{
				Color color = self.Evaluate(Mathf.Clamp01((float)i / 15f));
				result[i] = color.PackToFloat(floatPackingPrecision);
			}
			return result;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000A68C File Offset: 0x0000888C
		public static Color[] SampleInArray(this Gradient self, int samplesCount)
		{
			Color[] array = new Color[samplesCount];
			for (int i = 0; i < samplesCount; i++)
			{
				array[i] = self.Evaluate(Mathf.Clamp01((float)i / (float)(samplesCount - 1)));
			}
			return array;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000A6C6 File Offset: 0x000088C6
		private static Vector4 Vector4_Floor(Vector4 vec)
		{
			return new Vector4(Mathf.Floor(vec.x), Mathf.Floor(vec.y), Mathf.Floor(vec.z), Mathf.Floor(vec.w));
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000A6FC File Offset: 0x000088FC
		public static float PackToFloat(this Color color, int floatPackingPrecision)
		{
			Vector4 vector = Utils.Vector4_Floor(color * (float)(floatPackingPrecision - 1));
			return 0f + vector.x * (float)floatPackingPrecision * (float)floatPackingPrecision * (float)floatPackingPrecision + vector.y * (float)floatPackingPrecision * (float)floatPackingPrecision + vector.z * (float)floatPackingPrecision + vector.w;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000A751 File Offset: 0x00008951
		public static Utils.FloatPackingPrecision GetFloatPackingPrecision()
		{
			if (Utils.ms_FloatPackingPrecision == Utils.FloatPackingPrecision.Undef)
			{
				Utils.ms_FloatPackingPrecision = ((SystemInfo.graphicsShaderLevel >= 35) ? Utils.FloatPackingPrecision.High : Utils.FloatPackingPrecision.Low);
			}
			return Utils.ms_FloatPackingPrecision;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000A772 File Offset: 0x00008972
		public static bool HasAtLeastOneFlag(this Enum mask, Enum flags)
		{
			return ((int)mask & (int)flags) != 0;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000A784 File Offset: 0x00008984
		public static void MarkCurrentSceneDirty()
		{
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000A786 File Offset: 0x00008986
		public static void MarkObjectDirty(UnityEngine.Object obj)
		{
		}

		// Token: 0x0400019B RID: 411
		private const float kEpsilon = 1E-05f;

		// Token: 0x0400019C RID: 412
		private static Utils.FloatPackingPrecision ms_FloatPackingPrecision;

		// Token: 0x0400019D RID: 413
		private const int kFloatPackingHighMinShaderLevel = 35;

		// Token: 0x020000C7 RID: 199
		public enum FloatPackingPrecision
		{
			// Token: 0x04000410 RID: 1040
			High = 64,
			// Token: 0x04000411 RID: 1041
			Low = 8,
			// Token: 0x04000412 RID: 1042
			Undef = 0
		}
	}
}
