using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Pathfinding
{
	// Token: 0x02000128 RID: 296
	[BurstCompile]
	internal static class NavmeshCutJobs
	{
		// Token: 0x06000922 RID: 2338 RVA: 0x000319F7 File Offset: 0x0002FBF7
		[BurstCompile(FloatPrecision.Standard, FloatMode.Fast)]
		public static void CalculateContour(ref NavmeshCutJobs.JobCalculateContour job)
		{
			NavmeshCutJobs.CalculateContour_000008B4$BurstDirectCall.Invoke(ref job);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x000319FF File Offset: 0x0002FBFF
		private static float ApproximateCircleWithPolylineRadius(float radius, int resolution)
		{
			return radius / (1f - (1f - math.cos(3.1415927f / (float)resolution)) * 0.5f);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00031A24 File Offset: 0x0002FC24
		public unsafe static void CapsuleConvexHullXZ(float4x4 matrix, UnsafeList<float2>* points, float height, float radius, float radiusMargin, int circleResolution, out int numPoints, out float minY, out float maxY)
		{
			height = math.max(height, radius * 2f);
			int num = circleResolution / 2;
			radius = NavmeshCutJobs.ApproximateCircleWithPolylineRadius(radius, num * 2);
			float x = math.length(matrix.c0.xyz);
			float y = math.length(matrix.c2.xyz);
			radius *= math.max(x, y);
			float3 lhs = math.normalizesafe(matrix.c1.xyz, default(float3));
			float3 @float = math.transform(matrix, new float3(0f, -height * 0.5f, 0f)) + lhs * radius;
			float3 float2 = math.transform(matrix, new float3(0f, height * 0.5f, 0f)) - lhs * radius;
			float2 xz = @float.xz;
			float2 xz2 = float2.xz;
			bool flag = false;
			float2 float3;
			if (math.lengthsq(xz - xz2) < 0.005f)
			{
				float3 = new float2(1f, 0f);
				flag = true;
			}
			else
			{
				float3 = math.normalize(xz2 - xz);
			}
			float2 float4 = new float2(-float3.y, float3.x);
			radius += radiusMargin;
			float3 *= radius;
			float4 *= radius;
			minY = math.min(@float.y, float2.y) - radius;
			maxY = math.max(@float.y, float2.y) + radius;
			float num2 = 3.1415927f / (float)num;
			if (flag)
			{
				numPoints = num * 2;
				int length = points->Length;
				points->Resize(points->Length + numPoints, NativeArrayOptions.UninitializedMemory);
				for (int i = 0; i < num; i++)
				{
					float lhs2;
					float lhs3;
					math.sincos((float)i * num2, out lhs2, out lhs3);
					float2 rhs = lhs2 * float3 + lhs3 * float4;
					float2 float5 = xz - rhs;
					float2 float6 = xz2 + rhs;
					*points->ElementAt(length + i) = float5;
					*points->ElementAt(length + i + num) = float6;
				}
				return;
			}
			numPoints = (num + 1) * 2;
			int length2 = points->Length;
			points->Resize(points->Length + numPoints, NativeArrayOptions.UninitializedMemory);
			for (int j = 0; j < num + 1; j++)
			{
				float lhs4;
				float lhs5;
				math.sincos((float)j * num2, out lhs4, out lhs5);
				float2 rhs2 = lhs4 * float3 + lhs5 * float4;
				float2 float7 = xz - rhs2;
				float2 float8 = xz2 + rhs2;
				*points->ElementAt(length2 + j) = float7;
				*points->ElementAt(length2 + j + num + 1) = float8;
			}
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00031CE0 File Offset: 0x0002FEE0
		public unsafe static void BoxConvexHullXZ(float4x4 matrix, UnsafeList<float2>* points, out int numPoints, out float minY, out float maxY)
		{
			minY = float.PositiveInfinity;
			maxY = float.NegativeInfinity;
			int length = points->Length;
			points->Resize(points->Length + NavmeshCutJobs.BoxCorners.Length, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < NavmeshCutJobs.BoxCorners.Length; i++)
			{
				float4 @float = math.mul(matrix, NavmeshCutJobs.BoxCorners[i]);
				minY = math.min(minY, @float.y);
				maxY = math.max(maxY, @float.y);
				*points->ElementAt(length + i) = @float.xz;
			}
			numPoints = NavmeshCutJobs.ConvexHull(points->Ptr + length, NavmeshCutJobs.BoxCorners.Length, 0.01f);
			points->Length = length + numPoints;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00031DA0 File Offset: 0x0002FFA0
		public unsafe static int ConvexHull(float2* points, int nPoints, float vertexMergeDistance)
		{
			int num = 0;
			for (int i = 0; i < nPoints; i++)
			{
				if (points[i].x < points[num].x || (points[i].x == points[num].x && points[i].y < points[num].y))
				{
					num = i;
				}
			}
			NativeSortExtension.Sort<float2, NavmeshCutJobs.AngleComparator>(points, nPoints, new NavmeshCutJobs.AngleComparator
			{
				origin = points[num]
			});
			int j = 0;
			for (int k = 0; k < nPoints; k++)
			{
				float2 @float = points[k];
				while (j >= 2)
				{
					float2 float2 = points[j - 1] - @float;
					float2 float3 = points[j - 2] - @float;
					if (float2.x * float3.y - float2.y * float3.x < 0f && math.lengthsq(float2) >= vertexMergeDistance)
					{
						break;
					}
					j--;
				}
				if (j == 1 && math.lengthsq(points[j - 1] - @float) < vertexMergeDistance)
				{
					j--;
				}
				points[j] = @float;
				j++;
			}
			return j;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00032040 File Offset: 0x00030240
		[BurstCompile(FloatPrecision.Standard, FloatMode.Fast)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void CalculateContour$BurstManaged(ref NavmeshCutJobs.JobCalculateContour job)
		{
			job.Execute();
		}

		// Token: 0x04000648 RID: 1608
		private static readonly float4[] BoxCorners = new float4[]
		{
			new float4(-0.5f, -0.5f, -0.5f, 1f),
			new float4(0.5f, -0.5f, -0.5f, 1f),
			new float4(-0.5f, 0.5f, -0.5f, 1f),
			new float4(0.5f, 0.5f, -0.5f, 1f),
			new float4(-0.5f, -0.5f, 0.5f, 1f),
			new float4(0.5f, -0.5f, 0.5f, 1f),
			new float4(-0.5f, 0.5f, 0.5f, 1f),
			new float4(0.5f, 0.5f, 0.5f, 1f)
		};

		// Token: 0x02000129 RID: 297
		public struct JobCalculateContour
		{
			// Token: 0x06000929 RID: 2345 RVA: 0x00032048 File Offset: 0x00030248
			public unsafe void Execute()
			{
				this.circleResolution = math.max(this.circleResolution, 3);
				float4x4 float4x = math.mul(this.matrix, this.localToWorldMatrix);
				float num = math.length(float4x.c0);
				float num2 = math.length(float4x.c1);
				float num3 = math.length(float4x.c2);
				switch (this.meshType)
				{
				case NavmeshCut.MeshType.Rectangle:
				{
					this.rectangleSize = new float2(math.abs(this.rectangleSize.x), math.abs(this.rectangleSize.y)) + math.rcp(new float2(num, num3)) * this.radiusMargin * 2f;
					ref UnsafeList<float2> ptr = ref *this.outputVertices;
					float2 xz = math.transform(float4x, new float3(-this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f).xz;
					ptr.Add(xz);
					ref UnsafeList<float2> ptr2 = ref *this.outputVertices;
					xz = math.transform(float4x, new float3(this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f).xz;
					ptr2.Add(xz);
					ref UnsafeList<float2> ptr3 = ref *this.outputVertices;
					xz = math.transform(float4x, new float3(this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f).xz;
					ptr3.Add(xz);
					ref UnsafeList<float2> ptr4 = ref *this.outputVertices;
					xz = math.transform(float4x, new float3(-this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f).xz;
					ptr4.Add(xz);
					float y = float4x.c3.y;
					ref UnsafeList<NavmeshCut.ContourBurst> ptr5 = ref *this.outputContours;
					NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
					contourBurst.ymin = y - this.height * 0.5f * num2;
					contourBurst.ymax = y + this.height * 0.5f * num2;
					contourBurst.startIndex = this.outputVertices->Length - 4;
					contourBurst.endIndex = this.outputVertices->Length;
					ptr5.Add(contourBurst);
					break;
				}
				case NavmeshCut.MeshType.Circle:
				{
					this.circleRadius = math.abs(this.circleRadius);
					float num4 = this.height + this.radiusMargin / num2;
					float num5 = this.circleRadius + this.radiusMargin / num;
					float num6 = this.circleRadius + this.radiusMargin / num3;
					float num7 = 6.2831855f / (float)this.circleResolution;
					for (int i = 0; i < this.circleResolution; i++)
					{
						float num8;
						float num9;
						math.sincos((float)i * num7, out num8, out num9);
						ref UnsafeList<float2> ptr6 = ref *this.outputVertices;
						float2 xz = math.transform(float4x, new float3(num9 * num5, 0f, num8 * num6)).xz;
						ptr6.Add(xz);
					}
					float y2 = float4x.c3.y;
					ref UnsafeList<NavmeshCut.ContourBurst> ptr7 = ref *this.outputContours;
					NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
					contourBurst.ymin = y2 - num4 * 0.5f * num2;
					contourBurst.ymax = y2 + num4 * 0.5f * num2;
					contourBurst.startIndex = this.outputVertices->Length - this.circleResolution;
					contourBurst.endIndex = this.outputVertices->Length;
					ptr7.Add(contourBurst);
					break;
				}
				case NavmeshCut.MeshType.CustomMesh:
					if (this.meshContours != null && this.meshContourVertices != null && this.meshScale > 0f)
					{
						float4x = math.mul(float4x, float4x4.Scale(new float3(this.meshScale)));
						int length = this.outputVertices->Length;
						for (int j = 0; j < this.meshContourVertices->Length; j++)
						{
							ref UnsafeList<float2> ptr8 = ref *this.outputVertices;
							float2 xz = math.transform(float4x, *this.meshContourVertices->ElementAt(j)).xz;
							ptr8.Add(xz);
						}
						float y3 = float4x.c3.y;
						for (int k = 0; k < this.meshContours->Length; k++)
						{
							ref UnsafeList<NavmeshCut.ContourBurst> ptr9 = ref *this.outputContours;
							NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
							contourBurst.ymin = y3 - this.height * 0.5f * num2;
							contourBurst.ymax = y3 + this.height * 0.5f * num2;
							contourBurst.startIndex = length + this.meshContours->ElementAt(k).startIndex;
							contourBurst.endIndex = length + this.meshContours->ElementAt(k).endIndex;
							ptr9.Add(contourBurst);
						}
					}
					break;
				case NavmeshCut.MeshType.Box:
				{
					float3 scales = new float3(this.rectangleSize.x, this.height, this.rectangleSize.y) + math.rcp(new float3(num, num2, num3)) * this.radiusMargin * 2f;
					float4x = math.mul(float4x, float4x4.Scale(scales));
					int num10;
					float ymin;
					float ymax;
					NavmeshCutJobs.BoxConvexHullXZ(float4x, this.outputVertices, out num10, out ymin, out ymax);
					ref UnsafeList<NavmeshCut.ContourBurst> ptr10 = ref *this.outputContours;
					NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
					contourBurst.ymin = ymin;
					contourBurst.ymax = ymax;
					contourBurst.startIndex = this.outputVertices->Length - num10;
					contourBurst.endIndex = this.outputVertices->Length;
					ptr10.Add(contourBurst);
					break;
				}
				case NavmeshCut.MeshType.Sphere:
				{
					this.circleRadius = math.abs(this.circleRadius);
					float4x = math.mul(this.matrix, float4x4.Translate(this.localToWorldMatrix.c3.xyz));
					float num11 = math.max(num, math.max(num2, num3));
					float4x = math.mul(float4x, float4x4.Scale(num11));
					float num12 = this.circleRadius + this.radiusMargin / num11;
					num12 = NavmeshCutJobs.ApproximateCircleWithPolylineRadius(num12, this.circleResolution);
					float num13 = 6.2831855f / (float)this.circleResolution;
					for (int l = 0; l < this.circleResolution; l++)
					{
						float num14;
						float num15;
						math.sincos((float)l * num13, out num14, out num15);
						ref UnsafeList<float2> ptr11 = ref *this.outputVertices;
						float2 xz = math.transform(float4x, new float3(num15 * num12, 0f, num14 * num12)).xz;
						ptr11.Add(xz);
					}
					float y4 = float4x.c3.y;
					ref UnsafeList<NavmeshCut.ContourBurst> ptr12 = ref *this.outputContours;
					NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
					contourBurst.ymin = y4 - num12 * num11;
					contourBurst.ymax = y4 + num12 * num11;
					contourBurst.startIndex = this.outputVertices->Length - this.circleResolution;
					contourBurst.endIndex = this.outputVertices->Length;
					ptr12.Add(contourBurst);
					break;
				}
				case NavmeshCut.MeshType.Capsule:
				{
					this.circleResolution = math.max(this.circleResolution, 6);
					float radius = this.circleRadius;
					float num16 = this.height;
					num16 *= num2;
					float4x = math.mul(float4x, float4x4.Scale(new float3(1f, 1f / num2, 1f)));
					int num17;
					float ymin2;
					float ymax2;
					NavmeshCutJobs.CapsuleConvexHullXZ(float4x, this.outputVertices, num16, radius, this.radiusMargin, this.circleResolution, out num17, out ymin2, out ymax2);
					ref UnsafeList<NavmeshCut.ContourBurst> ptr13 = ref *this.outputContours;
					NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
					contourBurst.ymin = ymin2;
					contourBurst.ymax = ymax2;
					contourBurst.startIndex = this.outputVertices->Length - num17;
					contourBurst.endIndex = this.outputVertices->Length;
					ptr13.Add(contourBurst);
					break;
				}
				}
				for (int m = 0; m < this.outputContours->Length; m++)
				{
					NavmeshCut.ContourBurst contourBurst2 = *this.outputContours->ElementAt(m);
					this.WindCounterClockwise(this.outputVertices, contourBurst2.startIndex, contourBurst2.endIndex);
				}
			}

			// Token: 0x0600092A RID: 2346 RVA: 0x00032830 File Offset: 0x00030A30
			private unsafe void WindCounterClockwise(UnsafeList<float2>* vertices, int startIndex, int endIndex)
			{
				int num = 0;
				float2 @float = new float2(float.PositiveInfinity, float.PositiveInfinity);
				for (int i = startIndex; i < endIndex; i++)
				{
					float2 float2 = *vertices->ElementAt(i);
					if (float2.x < @float.x || (float2.x == @float.x && float2.y < @float.y))
					{
						num = i;
						@float = float2;
					}
				}
				int num2 = endIndex - startIndex;
				float2 float3 = (*vertices)[(num - 1 - startIndex + num2) % num2 + startIndex];
				float2 float4 = @float;
				float2 float5 = (*vertices)[(num + 1 - startIndex) % num2 + startIndex];
				if ((float4.x - float3.x) * (float5.y - float3.y) - (float5.x - float3.x) * (float4.y - float3.y) > 0f)
				{
					int j = startIndex;
					int num3 = endIndex - 1;
					while (j < num3)
					{
						float2 float6 = *vertices->ElementAt(j);
						*vertices->ElementAt(j) = *vertices->ElementAt(num3);
						*vertices->ElementAt(num3) = float6;
						j++;
						num3--;
					}
				}
			}

			// Token: 0x04000649 RID: 1609
			public unsafe UnsafeList<float2>* outputVertices;

			// Token: 0x0400064A RID: 1610
			public unsafe UnsafeList<NavmeshCut.ContourBurst>* outputContours;

			// Token: 0x0400064B RID: 1611
			public unsafe UnsafeList<NavmeshCut.ContourBurst>* meshContours;

			// Token: 0x0400064C RID: 1612
			public unsafe UnsafeList<float3>* meshContourVertices;

			// Token: 0x0400064D RID: 1613
			public float4x4 matrix;

			// Token: 0x0400064E RID: 1614
			public float4x4 localToWorldMatrix;

			// Token: 0x0400064F RID: 1615
			public float radiusMargin;

			// Token: 0x04000650 RID: 1616
			public int circleResolution;

			// Token: 0x04000651 RID: 1617
			public float circleRadius;

			// Token: 0x04000652 RID: 1618
			public float2 rectangleSize;

			// Token: 0x04000653 RID: 1619
			public float height;

			// Token: 0x04000654 RID: 1620
			public float meshScale;

			// Token: 0x04000655 RID: 1621
			public NavmeshCut.MeshType meshType;
		}

		// Token: 0x0200012A RID: 298
		private struct AngleComparator : IComparer<float2>
		{
			// Token: 0x0600092B RID: 2347 RVA: 0x00032964 File Offset: 0x00030B64
			public int Compare(float2 lhs, float2 rhs)
			{
				float2 @float = lhs - this.origin;
				float2 float2 = rhs - this.origin;
				float num = @float.x * float2.y - @float.y * float2.x;
				if (Hint.Unlikely(num == 0f))
				{
					float num2 = math.lengthsq(@float);
					float num3 = math.lengthsq(float2);
					if (num2 < num3)
					{
						return 1;
					}
					if (num2 > num3)
					{
						return -1;
					}
					if (@float.x < float2.x)
					{
						return 1;
					}
					if (@float.x > float2.x)
					{
						return -1;
					}
					if (@float.y < float2.y)
					{
						return 1;
					}
					if (@float.y > float2.y)
					{
						return -1;
					}
					return 0;
				}
				else
				{
					if (num >= 0f)
					{
						return -1;
					}
					return 1;
				}
			}

			// Token: 0x04000656 RID: 1622
			public float2 origin;
		}

		// Token: 0x0200012B RID: 299
		// (Invoke) Token: 0x0600092D RID: 2349
		internal delegate void CalculateContour_000008B4$PostfixBurstDelegate(ref NavmeshCutJobs.JobCalculateContour job);

		// Token: 0x0200012C RID: 300
		internal static class CalculateContour_000008B4$BurstDirectCall
		{
			// Token: 0x06000930 RID: 2352 RVA: 0x00032A20 File Offset: 0x00030C20
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (NavmeshCutJobs.CalculateContour_000008B4$BurstDirectCall.Pointer == 0)
				{
					NavmeshCutJobs.CalculateContour_000008B4$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(NavmeshCutJobs.CalculateContour_000008B4$BurstDirectCall.DeferredCompilation, methodof(NavmeshCutJobs.CalculateContour$BurstManaged(NavmeshCutJobs.JobCalculateContour*)).MethodHandle, typeof(NavmeshCutJobs.CalculateContour_000008B4$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = NavmeshCutJobs.CalculateContour_000008B4$BurstDirectCall.Pointer;
			}

			// Token: 0x06000931 RID: 2353 RVA: 0x00032A4C File Offset: 0x00030C4C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				NavmeshCutJobs.CalculateContour_000008B4$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000932 RID: 2354 RVA: 0x00032A64 File Offset: 0x00030C64
			public unsafe static void Constructor()
			{
				NavmeshCutJobs.CalculateContour_000008B4$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(NavmeshCutJobs.CalculateContour(NavmeshCutJobs.JobCalculateContour*)).MethodHandle);
			}

			// Token: 0x06000933 RID: 2355 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000934 RID: 2356 RVA: 0x00032A75 File Offset: 0x00030C75
			// Note: this type is marked as 'beforefieldinit'.
			static CalculateContour_000008B4$BurstDirectCall()
			{
				NavmeshCutJobs.CalculateContour_000008B4$BurstDirectCall.Constructor();
			}

			// Token: 0x06000935 RID: 2357 RVA: 0x00032A7C File Offset: 0x00030C7C
			public static void Invoke(ref NavmeshCutJobs.JobCalculateContour job)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = NavmeshCutJobs.CalculateContour_000008B4$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.NavmeshCutJobs/JobCalculateContour&), ref job, functionPointer);
						return;
					}
				}
				NavmeshCutJobs.CalculateContour$BurstManaged(ref job);
			}

			// Token: 0x04000657 RID: 1623
			private static IntPtr Pointer;

			// Token: 0x04000658 RID: 1624
			private static IntPtr DeferredCompilation;
		}
	}
}
