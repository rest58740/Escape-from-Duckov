using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace UI_Spline_Renderer
{
	// Token: 0x02000004 RID: 4
	[BurstCompile]
	internal static class InternalUtility
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		internal static Vector3 GetPivotInWorldSpace(this RectTransform source)
		{
			Vector2 vector;
			vector..ctor(source.rect.xMin + source.pivot.x * source.rect.width, source.rect.yMin + source.pivot.y * source.rect.height);
			return source.TransformPoint(new Vector3(vector.x, vector.y, 0f));
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002144 File Offset: 0x00000344
		internal static void SetPivotWithoutRect(this RectTransform source, Vector3 pivot)
		{
			Rect rect = source.rect;
			if (float.IsNaN(rect.x) || float.IsNaN(rect.y) || float.IsNaN(rect.size.x) || float.IsNaN(rect.size.y))
			{
				return;
			}
			pivot = source.InverseTransformPoint(pivot);
			Vector2 vector;
			vector..ctor((pivot.x - rect.xMin) / rect.width, (pivot.y - rect.yMin) / rect.height);
			Vector2 vector2 = vector - source.pivot;
			vector2.Scale(source.rect.size);
			Vector3 position = source.position + source.TransformVector(vector2);
			source.pivot = vector;
			source.position = position;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002220 File Offset: 0x00000420
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static float Remap(this float value, float beforeRangeMin, float beforeRangeMax, float targetRangeMin, float targetRangeMax)
		{
			float num = beforeRangeMax - beforeRangeMin;
			if (num == 0f)
			{
				return targetRangeMin;
			}
			float num2 = (value - beforeRangeMin) / num;
			return (targetRangeMax - targetRangeMin) * num2 + targetRangeMin;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002249 File Offset: 0x00000449
		internal static float Remap(this int value, float beforeRangeMin, float beforeRangeMax, float targetRangeMin, float targetRangeMax)
		{
			return ((float)value).Remap(beforeRangeMin, beforeRangeMax, targetRangeMin, targetRangeMax);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002257 File Offset: 0x00000457
		internal static float sqr(this float value)
		{
			return value * value;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000225C File Offset: 0x0000045C
		internal static float LerpPoint(float value, float min, float max)
		{
			return (value - min) / (max - min);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002268 File Offset: 0x00000468
		internal static StartEndImagePreset GetCurrentStartImagePreset(Sprite target)
		{
			if (target == null)
			{
				return StartEndImagePreset.None;
			}
			if (target == UISplineRendererSettings.Instance.triangleHead)
			{
				return StartEndImagePreset.Triangle;
			}
			if (target == UISplineRendererSettings.Instance.arrowHead)
			{
				return StartEndImagePreset.Arrow;
			}
			if (target == UISplineRendererSettings.Instance.emptyCircleHead)
			{
				return StartEndImagePreset.EmptyCircle;
			}
			if (target == UISplineRendererSettings.Instance.filledCircleHead)
			{
				return StartEndImagePreset.FilledCircle;
			}
			return StartEndImagePreset.Custom;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000022D1 File Offset: 0x000004D1
		internal static int CalcVertexCount(this Spline spline, float segmentLength, Vector2 renderRange)
		{
			return Mathf.Max(Mathf.CeilToInt(spline.GetLength() * (renderRange.y - renderRange.x) * segmentLength), 1) * 2 + 4;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022F8 File Offset: 0x000004F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int CalcVertexCount(this NativeSpline spline, float segmentLength, float2 renderRange)
		{
			return math.max((int)math.ceil(spline.GetLength() * (renderRange.y - renderRange.x) * segmentLength), 1) * 2 + 4;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002324 File Offset: 0x00000524
		internal static NativeColorGradient ToNative(this Gradient gradient, Allocator allocator = 3)
		{
			NativeArray<float2> alphaKeyFrames = new NativeArray<float2>(gradient.alphaKeys.Length, allocator, 1);
			for (int i = 0; i < gradient.alphaKeys.Length; i++)
			{
				GradientAlphaKey gradientAlphaKey = gradient.alphaKeys[i];
				alphaKeyFrames[i] = new float2(gradientAlphaKey.alpha, gradientAlphaKey.time);
			}
			NativeArray<float4> colorKeyFrames = new NativeArray<float4>(gradient.colorKeys.Length, allocator, 1);
			for (int j = 0; j < gradient.colorKeys.Length; j++)
			{
				GradientColorKey gradientColorKey = gradient.colorKeys[j];
				colorKeyFrames[j] = new float4(gradientColorKey.color.r, gradientColorKey.color.g, gradientColorKey.color.b, gradientColorKey.time);
			}
			return new NativeColorGradient
			{
				alphaKeyFrames = alphaKeyFrames,
				colorKeyFrames = colorKeyFrames
			};
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002408 File Offset: 0x00000608
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ExtrudeEdge(float w, float V, in Color clr, ref float3 pos, in float3 tan, in float3 up, bool keepBillboard, bool keepZeroZ, in float2 uvMultiplier, in float2 uvOffset, out UIVertex v0, out UIVertex v1)
		{
			InternalUtility.ExtrudeEdge_0000000D$BurstDirectCall.Invoke(w, V, clr, ref pos, tan, up, keepBillboard, keepZeroZ, uvMultiplier, uvOffset, out v0, out v1);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002430 File Offset: 0x00000630
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ExtrudeEdge$BurstManaged(float w, float V, in Color clr, ref float3 pos, in float3 tan, in float3 up, bool keepBillboard, bool keepZeroZ, in float2 uvMultiplier, in float2 uvOffset, out UIVertex v0, out UIVertex v1)
		{
			float3 @float = keepBillboard ? math.normalizesafe(math.cross(tan, new float3(0f, 0f, -1f)), default(float3)) : math.normalizesafe(math.cross(tan, up), default(float3));
			if (keepZeroZ)
			{
				pos.z = 0f;
			}
			float2 float2 = new float2(0f, V) * uvMultiplier - uvOffset;
			UIVertex uivertex = default(UIVertex);
			uivertex.position = pos + @float * w * 0.5f;
			uivertex.uv0 = new Vector4(float2.x, float2.y);
			uivertex.color = clr;
			UIVertex uivertex2 = uivertex;
			v0 = uivertex2;
			float2 = new float2(1f, V) * uvMultiplier - uvOffset;
			uivertex = default(UIVertex);
			uivertex.position = pos - @float * w * 0.5f;
			uivertex.uv0 = new Vector4(float2.x, float2.y);
			uivertex.color = clr;
			uivertex2 = uivertex;
			v1 = uivertex2;
		}

		// Token: 0x02000005 RID: 5
		// (Invoke) Token: 0x06000010 RID: 16
		internal delegate void ExtrudeEdge_0000000D$PostfixBurstDelegate(float w, float V, in Color clr, ref float3 pos, in float3 tan, in float3 up, bool keepBillboard, bool keepZeroZ, in float2 uvMultiplier, in float2 uvOffset, out UIVertex v0, out UIVertex v1);

		// Token: 0x02000006 RID: 6
		internal static class ExtrudeEdge_0000000D$BurstDirectCall
		{
			// Token: 0x06000013 RID: 19 RVA: 0x000025B4 File Offset: 0x000007B4
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (InternalUtility.ExtrudeEdge_0000000D$BurstDirectCall.Pointer == 0)
				{
					InternalUtility.ExtrudeEdge_0000000D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(InternalUtility.ExtrudeEdge_0000000D$BurstDirectCall.DeferredCompilation, methodof(InternalUtility.ExtrudeEdge$BurstManaged(float, float, Color*, float3*, float3*, float3*, bool, bool, float2*, float2*, UIVertex*, UIVertex*)).MethodHandle, typeof(InternalUtility.ExtrudeEdge_0000000D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = InternalUtility.ExtrudeEdge_0000000D$BurstDirectCall.Pointer;
			}

			// Token: 0x06000014 RID: 20 RVA: 0x000025E0 File Offset: 0x000007E0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				InternalUtility.ExtrudeEdge_0000000D$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000015 RID: 21 RVA: 0x000025F8 File Offset: 0x000007F8
			public unsafe static void Constructor()
			{
				InternalUtility.ExtrudeEdge_0000000D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(InternalUtility.ExtrudeEdge(float, float, Color*, float3*, float3*, float3*, bool, bool, float2*, float2*, UIVertex*, UIVertex*)).MethodHandle);
			}

			// Token: 0x06000016 RID: 22 RVA: 0x00002609 File Offset: 0x00000809
			public static void Initialize()
			{
			}

			// Token: 0x06000017 RID: 23 RVA: 0x0000260B File Offset: 0x0000080B
			// Note: this type is marked as 'beforefieldinit'.
			static ExtrudeEdge_0000000D$BurstDirectCall()
			{
				InternalUtility.ExtrudeEdge_0000000D$BurstDirectCall.Constructor();
			}

			// Token: 0x06000018 RID: 24 RVA: 0x00002614 File Offset: 0x00000814
			public static void Invoke(float w, float V, in Color clr, ref float3 pos, in float3 tan, in float3 up, bool keepBillboard, bool keepZeroZ, in float2 uvMultiplier, in float2 uvOffset, out UIVertex v0, out UIVertex v1)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = InternalUtility.ExtrudeEdge_0000000D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.Single,System.Single,UnityEngine.Color&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,System.Boolean,System.Boolean,Unity.Mathematics.float2&,Unity.Mathematics.float2&,UnityEngine.UIVertex&,UnityEngine.UIVertex&), w, V, ref clr, ref pos, ref tan, ref up, keepBillboard, keepZeroZ, ref uvMultiplier, ref uvOffset, ref v0, ref v1, functionPointer);
						return;
					}
				}
				InternalUtility.ExtrudeEdge$BurstManaged(w, V, clr, ref pos, tan, up, keepBillboard, keepZeroZ, uvMultiplier, uvOffset, out v0, out v1);
			}

			// Token: 0x04000006 RID: 6
			private static IntPtr Pointer;

			// Token: 0x04000007 RID: 7
			private static IntPtr DeferredCompilation;
		}
	}
}
