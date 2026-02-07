using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Drawing;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.Collections
{
	// Token: 0x0200026B RID: 619
	[BurstCompile]
	public struct BBTree : IDisposable
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000ED8 RID: 3800 RVA: 0x0005B878 File Offset: 0x00059A78
		public IntRect Size
		{
			get
			{
				if (this.tree.Length != 0)
				{
					return this.tree[0].rect;
				}
				return default(IntRect);
			}
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0005B8AD File Offset: 0x00059AAD
		public void Dispose()
		{
			this.nodePermutation.Dispose();
			this.tree.Dispose();
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0005B8C5 File Offset: 0x00059AC5
		public BBTree(UnsafeSpan<int> triangles, UnsafeSpan<Int3> vertices)
		{
			if (triangles.Length % 3 != 0)
			{
				throw new ArgumentException("triangles must be a multiple of 3 in length");
			}
			BBTree.Build(ref triangles, ref vertices, out this);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0005B8E7 File Offset: 0x00059AE7
		[BurstCompile]
		private static void Build(ref UnsafeSpan<int> triangles, ref UnsafeSpan<Int3> vertices, out BBTree bbTree)
		{
			BBTree.Build_00000DDE$BurstDirectCall.Invoke(ref triangles, ref vertices, out bbTree);
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0005B8F4 File Offset: 0x00059AF4
		private static int SplitByX(NativeArray<IntRect> nodesBounds, NativeArray<int> permutation, int from, int to, int divider)
		{
			int num = to;
			for (int i = from; i < num; i++)
			{
				IntRect intRect = nodesBounds[permutation[i]];
				if ((intRect.xmin + intRect.xmax) / 2 > divider)
				{
					num--;
					int value = permutation[num];
					permutation[num] = permutation[i];
					permutation[i] = value;
					i--;
				}
			}
			return num;
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0005B960 File Offset: 0x00059B60
		private static int SplitByZ(NativeArray<IntRect> nodesBounds, NativeArray<int> permutation, int from, int to, int divider)
		{
			int num = to;
			for (int i = from; i < num; i++)
			{
				IntRect intRect = nodesBounds[permutation[i]];
				if ((intRect.ymin + intRect.ymax) / 2 > divider)
				{
					num--;
					int value = permutation[num];
					permutation[num] = permutation[i];
					permutation[i] = value;
					i--;
				}
			}
			return num;
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0005B9CC File Offset: 0x00059BCC
		private static int BuildSubtree(NativeArray<int> permutation, NativeArray<IntRect> nodeBounds, ref UnsafeList<int> nodes, ref UnsafeList<BBTree.BBTreeBox> tree, int from, int to, bool odd, int depth)
		{
			IntRect intRect = BBTree.NodeBounds(permutation, nodeBounds, from, to);
			int length = tree.Length;
			BBTree.BBTreeBox bbtreeBox = new BBTree.BBTreeBox(intRect);
			tree.Add(bbtreeBox);
			if (to - from <= 4)
			{
				if (depth > 26)
				{
					Debug.LogWarning(string.Format("Maximum tree height of {0} exceeded (got depth of {1}). Querying this tree may fail. Is the tree very unbalanced?", 26, depth));
				}
				BBTree.BBTreeBox value = tree[length];
				int num = value.nodeOffset = nodes.Length;
				tree[length] = value;
				nodes.Length += 4;
				for (int i = 0; i < 4; i++)
				{
					nodes[num + i] = ((i < to - from) ? permutation[from + i] : -1);
				}
				return length;
			}
			int num2;
			if (odd)
			{
				int divider = (intRect.xmin + intRect.xmax) / 2;
				num2 = BBTree.SplitByX(nodeBounds, permutation, from, to, divider);
			}
			else
			{
				int divider2 = (intRect.ymin + intRect.ymax) / 2;
				num2 = BBTree.SplitByZ(nodeBounds, permutation, from, to, divider2);
			}
			int num3 = (to - from) / 8;
			if (num2 <= from + num3 || num2 >= to - num3)
			{
				if (!odd)
				{
					int divider3 = (intRect.xmin + intRect.xmax) / 2;
					num2 = BBTree.SplitByX(nodeBounds, permutation, from, to, divider3);
				}
				else
				{
					int divider4 = (intRect.ymin + intRect.ymax) / 2;
					num2 = BBTree.SplitByZ(nodeBounds, permutation, from, to, divider4);
				}
				if (num2 <= from + num3 || num2 >= to - num3)
				{
					num2 = (from + to) / 2;
				}
			}
			int left = BBTree.BuildSubtree(permutation, nodeBounds, ref nodes, ref tree, from, num2, !odd, depth + 1);
			int right = BBTree.BuildSubtree(permutation, nodeBounds, ref nodes, ref tree, num2, to, !odd, depth + 1);
			BBTree.BBTreeBox value2 = tree[length];
			value2.left = left;
			value2.right = right;
			tree[length] = value2;
			return length;
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0005BBB8 File Offset: 0x00059DB8
		private static IntRect NodeBounds(NativeArray<int> permutation, NativeArray<IntRect> nodeBounds, int from, int to)
		{
			int2 @int = nodeBounds[permutation[from]].Min.ToInt2();
			int2 int2 = nodeBounds[permutation[from]].Max.ToInt2();
			for (int i = from + 1; i < to; i++)
			{
				IntRect intRect = nodeBounds[permutation[i]];
				int2 y = new int2(intRect.xmin, intRect.ymin);
				int2 y2 = new int2(intRect.xmax, intRect.ymax);
				@int = math.min(@int, y);
				int2 = math.max(int2, y2);
			}
			return new IntRect(@int.x, @int.y, int2.x, int2.y);
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0005BC77 File Offset: 0x00059E77
		public float DistanceSqrLowerBound(float3 p, in BBTree.ProjectionParams projection)
		{
			if (this.tree.Length == 0)
			{
				return float.PositiveInfinity;
			}
			return projection.SquaredRectPointDistanceOnPlane(this.tree[0].rect, p);
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0005BCA4 File Offset: 0x00059EA4
		public unsafe void QueryClosest(float3 p, NNConstraint constraint, in BBTree.ProjectionParams projection, ref float distanceSqr, ref NNInfo previous, GraphNode[] nodes, UnsafeSpan<int> triangles, UnsafeSpan<Int3> vertices)
		{
			if (this.tree.Length == 0)
			{
				return;
			}
			BBTree.NearbyNodesIterator.BoxWithDist* ptr = stackalloc BBTree.NearbyNodesIterator.BoxWithDist[checked(unchecked((UIntPtr)26) * (UIntPtr)sizeof(BBTree.NearbyNodesIterator.BoxWithDist))];
			UnsafeSpan<BBTree.NearbyNodesIterator.BoxWithDist> stack = new UnsafeSpan<BBTree.NearbyNodesIterator.BoxWithDist>((void*)ptr, 26);
			*stack[0] = new BBTree.NearbyNodesIterator.BoxWithDist
			{
				index = 0,
				distSqr = 0f
			};
			BBTree.NearbyNodesIterator nearbyNodesIterator = new BBTree.NearbyNodesIterator
			{
				stack = stack,
				stackSize = 1,
				indexInLeaf = 0,
				point = p,
				projection = projection,
				distanceThresholdSqr = distanceSqr,
				tieBreakingDistanceThreshold = float.PositiveInfinity,
				tree = this.tree.AsUnsafeSpan<BBTree.BBTreeBox>(),
				nodes = this.nodePermutation.AsUnsafeSpan<int>(),
				triangles = triangles,
				vertices = vertices
			};
			NNInfo nninfo = previous;
			while (nearbyNodesIterator.stackSize > 0 && nearbyNodesIterator.MoveNext())
			{
				BBTree.CloseNode current = nearbyNodesIterator.current;
				if (constraint == null || constraint.Suitable(nodes[current.node]))
				{
					nearbyNodesIterator.distanceThresholdSqr = current.distanceSq;
					nearbyNodesIterator.tieBreakingDistanceThreshold = current.tieBreakingDistance;
					nninfo = new NNInfo(nodes[current.node], current.closestPointOnNode, current.distanceSq);
				}
			}
			distanceSqr = nearbyNodesIterator.distanceThresholdSqr;
			previous = nninfo;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0005BE0F File Offset: 0x0005A00F
		public void DrawGizmos(CommandBuilder draw)
		{
			Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
			if (this.tree.Length == 0)
			{
				return;
			}
			this.DrawGizmos(ref draw, 0, 0);
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0005BE48 File Offset: 0x0005A048
		private void DrawGizmos(ref CommandBuilder draw, int boxi, int depth)
		{
			BBTree.BBTreeBox bbtreeBox = this.tree[boxi];
			Vector3 vector = (Vector3)new Int3(bbtreeBox.rect.xmin, 0, bbtreeBox.rect.ymin);
			Vector3 vector2 = (Vector3)new Int3(bbtreeBox.rect.xmax, 0, bbtreeBox.rect.ymax);
			Vector3 v = (vector + vector2) * 0.5f;
			Vector3 vector3 = vector2 - vector;
			vector3 = new Vector3(vector3.x, 1f, vector3.z);
			v.y += (float)(depth * 2);
			draw.xz.WireRectangle(v, new float2(vector3.x, vector3.z), AstarMath.IntToColor(depth, 1f));
			if (!bbtreeBox.IsLeaf)
			{
				this.DrawGizmos(ref draw, bbtreeBox.left, depth + 1);
				this.DrawGizmos(ref draw, bbtreeBox.right, depth + 1);
			}
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0005BF48 File Offset: 0x0005A148
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void Build$BurstManaged(ref UnsafeSpan<int> triangles, ref UnsafeSpan<Int3> vertices, out BBTree bbTree)
		{
			int num = triangles.Length / 3;
			UnsafeList<BBTree.BBTreeBox> unsafeList = new UnsafeList<BBTree.BBTreeBox>((int)((float)num * 2.1f), Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			UnsafeList<int> unsafeList2 = new UnsafeList<int>((int)((float)num * 1.1f), Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			NativeArray<int> permutation = new NativeArray<int>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < num; i++)
			{
				permutation[i] = i;
			}
			NativeArray<IntRect> nodeBounds = new NativeArray<IntRect>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int j = 0; j < num; j++)
			{
				int2 xz = ((int3)(*vertices[*triangles[j * 3]])).xz;
				int2 xz2 = ((int3)(*vertices[*triangles[j * 3 + 1]])).xz;
				int2 xz3 = ((int3)(*vertices[*triangles[j * 3 + 2]])).xz;
				int2 @int = math.min(xz, math.min(xz2, xz3));
				int2 int2 = math.max(xz, math.max(xz2, xz3));
				nodeBounds[j] = new IntRect(@int.x, @int.y, int2.x, int2.y);
			}
			if (num > 0)
			{
				BBTree.BuildSubtree(permutation, nodeBounds, ref unsafeList2, ref unsafeList, 0, num, false, 0);
			}
			nodeBounds.Dispose();
			permutation.Dispose();
			bbTree = new BBTree
			{
				tree = unsafeList,
				nodePermutation = unsafeList2
			};
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0005C0D3 File Offset: 0x0005A2D3
		public static void Initialize$NearbyNodesIterator_MoveNext_00000DF0$BurstDirectCall()
		{
			BBTree.NearbyNodesIterator.MoveNext_00000DF0$BurstDirectCall.Initialize();
		}

		// Token: 0x04000B19 RID: 2841
		private UnsafeList<BBTree.BBTreeBox> tree;

		// Token: 0x04000B1A RID: 2842
		private UnsafeList<int> nodePermutation;

		// Token: 0x04000B1B RID: 2843
		private const int MaximumLeafSize = 4;

		// Token: 0x04000B1C RID: 2844
		private const int MAX_TREE_HEIGHT = 26;

		// Token: 0x0200026C RID: 620
		[BurstCompile]
		public readonly struct ProjectionParams
		{
			// Token: 0x1700020F RID: 527
			// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x0005C0DA File Offset: 0x0005A2DA
			public bool alignedWithXZPlane
			{
				get
				{
					return this.alignedWithXZPlaneBacking > 0;
				}
			}

			// Token: 0x06000EE7 RID: 3815 RVA: 0x0005C0E5 File Offset: 0x0005A2E5
			public float SquaredRectPointDistanceOnPlane(IntRect rect, float3 p)
			{
				return BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane(this, ref rect, ref p);
			}

			// Token: 0x06000EE8 RID: 3816 RVA: 0x0005C0F1 File Offset: 0x0005A2F1
			[BurstCompile(FloatMode = FloatMode.Fast)]
			[IgnoredByDeepProfiler]
			private static float SquaredRectPointDistanceOnPlane(in BBTree.ProjectionParams projection, ref IntRect rect, ref float3 p)
			{
				return BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000DE9$BurstDirectCall.Invoke(projection, ref rect, ref p);
			}

			// Token: 0x06000EE9 RID: 3817 RVA: 0x0005C0FC File Offset: 0x0005A2FC
			public ProjectionParams(NNConstraint constraint, GraphTransform graphTransform)
			{
				if (constraint == null || !(constraint.distanceMetric.projectionAxis != Vector3.zero))
				{
					this.projectionAxis = float3.zero;
					this.planeProjection = default(float2x3);
					this.projectedUpNormalized = default(float2);
					this.distanceMetric = BBTree.DistanceMetric.Euclidean;
					this.alignedWithXZPlaneBacking = 1;
					this.distanceScaleAlongProjectionAxis = 0f;
					return;
				}
				if (float.IsPositiveInfinity(constraint.distanceMetric.projectionAxis.x))
				{
					this.projectionAxis = new float3(0f, 1f, 0f);
				}
				else
				{
					this.projectionAxis = math.normalizesafe(graphTransform.InverseTransformVector(constraint.distanceMetric.projectionAxis), default(float3));
				}
				if (this.projectionAxis.x * this.projectionAxis.x + this.projectionAxis.z * this.projectionAxis.z < 0.0001f)
				{
					this.projectedUpNormalized = float2.zero;
					this.planeProjection = new float2x3(1f, 0f, 0f, 0f, 0f, 1f);
					this.distanceMetric = BBTree.DistanceMetric.ScaledManhattan;
					this.alignedWithXZPlaneBacking = 1;
					this.distanceScaleAlongProjectionAxis = math.max(constraint.distanceMetric.distanceScaleAlongProjectionDirection, 0f);
					return;
				}
				float3 @float = math.normalizesafe(math.cross(new float3(1f, 0f, 1f), this.projectionAxis), default(float3));
				if (math.all(@float == 0f))
				{
					@float = math.normalizesafe(math.cross(new float3(-1f, 0f, 1f), this.projectionAxis), default(float3));
				}
				float3 c = math.normalizesafe(math.cross(this.projectionAxis, @float), default(float3));
				this.planeProjection = math.transpose(new float3x2(@float, c));
				this.projectedUpNormalized = ((math.lengthsq(this.planeProjection.c1) <= 0.0001f) ? float2.zero : math.normalize(this.planeProjection.c1));
				this.distanceMetric = BBTree.DistanceMetric.ScaledManhattan;
				this.alignedWithXZPlaneBacking = (math.all(this.projectedUpNormalized == 0f) ? 1 : 0);
				this.distanceScaleAlongProjectionAxis = math.max(constraint.distanceMetric.distanceScaleAlongProjectionDirection, 0f);
			}

			// Token: 0x06000EEA RID: 3818 RVA: 0x0005C36C File Offset: 0x0005A56C
			[BurstCompile(FloatMode = FloatMode.Fast)]
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static float SquaredRectPointDistanceOnPlane$BurstManaged(in BBTree.ProjectionParams projection, ref IntRect rect, ref float3 p)
			{
				if (projection.alignedWithXZPlane)
				{
					float2 lowerBound = new float2((float)rect.xmin, (float)rect.ymin) * 0.001f;
					float2 upperBound = new float2((float)rect.xmax, (float)rect.ymax) * 0.001f;
					return math.lengthsq(math.clamp(p.xz, lowerBound, upperBound) - p.xz);
				}
				float3 b = new float3((float)rect.xmin, 0f, (float)rect.ymin) * 0.001f - p;
				float3 b2 = new float3((float)rect.xmax, 0f, (float)rect.ymax) * 0.001f - p;
				float3 b3 = new float3((float)rect.xmin, 0f, (float)rect.ymax) * 0.001f - p;
				float3 b4 = new float3((float)rect.xmax, 0f, (float)rect.ymin) * 0.001f - p;
				float2 c = math.mul(projection.planeProjection, b);
				float2 c2 = math.mul(projection.planeProjection, b3);
				float2 c3 = math.mul(projection.planeProjection, b4);
				float2 c4 = math.mul(projection.planeProjection, b2);
				float2 b5 = new float2(projection.projectedUpNormalized.y, -projection.projectedUpNormalized.x);
				float4 x = math.mul(math.transpose(new float2x4(c, c2, c3, c4)), b5);
				float num = math.clamp(0f, math.cmin(x), math.cmax(x));
				return num * num;
			}

			// Token: 0x04000B1D RID: 2845
			public readonly float2x3 planeProjection;

			// Token: 0x04000B1E RID: 2846
			public readonly float2 projectedUpNormalized;

			// Token: 0x04000B1F RID: 2847
			public readonly float3 projectionAxis;

			// Token: 0x04000B20 RID: 2848
			public readonly float distanceScaleAlongProjectionAxis;

			// Token: 0x04000B21 RID: 2849
			public readonly BBTree.DistanceMetric distanceMetric;

			// Token: 0x04000B22 RID: 2850
			private readonly byte alignedWithXZPlaneBacking;

			// Token: 0x0200026D RID: 621
			// (Invoke) Token: 0x06000EEC RID: 3820
			internal delegate float SquaredRectPointDistanceOnPlane_00000DE9$PostfixBurstDelegate(in BBTree.ProjectionParams projection, ref IntRect rect, ref float3 p);

			// Token: 0x0200026E RID: 622
			internal static class SquaredRectPointDistanceOnPlane_00000DE9$BurstDirectCall
			{
				// Token: 0x06000EEF RID: 3823 RVA: 0x0005C51E File Offset: 0x0005A71E
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000DE9$BurstDirectCall.Pointer == 0)
					{
						BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000DE9$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000DE9$BurstDirectCall.DeferredCompilation, methodof(BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane$BurstManaged(BBTree.ProjectionParams*, IntRect*, float3*)).MethodHandle, typeof(BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000DE9$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000DE9$BurstDirectCall.Pointer;
				}

				// Token: 0x06000EF0 RID: 3824 RVA: 0x0005C54C File Offset: 0x0005A74C
				private static IntPtr GetFunctionPointer()
				{
					IntPtr result = (IntPtr)0;
					BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000DE9$BurstDirectCall.GetFunctionPointerDiscard(ref result);
					return result;
				}

				// Token: 0x06000EF1 RID: 3825 RVA: 0x0005C564 File Offset: 0x0005A764
				public unsafe static void Constructor()
				{
					BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000DE9$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane(BBTree.ProjectionParams*, IntRect*, float3*)).MethodHandle);
				}

				// Token: 0x06000EF2 RID: 3826 RVA: 0x000035CE File Offset: 0x000017CE
				public static void Initialize()
				{
				}

				// Token: 0x06000EF3 RID: 3827 RVA: 0x0005C575 File Offset: 0x0005A775
				// Note: this type is marked as 'beforefieldinit'.
				static SquaredRectPointDistanceOnPlane_00000DE9$BurstDirectCall()
				{
					BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000DE9$BurstDirectCall.Constructor();
				}

				// Token: 0x06000EF4 RID: 3828 RVA: 0x0005C57C File Offset: 0x0005A77C
				public static float Invoke(in BBTree.ProjectionParams projection, ref IntRect rect, ref float3 p)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000DE9$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							return calli(System.Single(Pathfinding.Collections.BBTree/ProjectionParams&,Pathfinding.IntRect&,Unity.Mathematics.float3&), ref projection, ref rect, ref p, functionPointer);
						}
					}
					return BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane$BurstManaged(projection, ref rect, ref p);
				}

				// Token: 0x04000B23 RID: 2851
				private static IntPtr Pointer;

				// Token: 0x04000B24 RID: 2852
				private static IntPtr DeferredCompilation;
			}
		}

		// Token: 0x0200026F RID: 623
		private struct CloseNode
		{
			// Token: 0x04000B25 RID: 2853
			public int node;

			// Token: 0x04000B26 RID: 2854
			public float distanceSq;

			// Token: 0x04000B27 RID: 2855
			public float tieBreakingDistance;

			// Token: 0x04000B28 RID: 2856
			public float3 closestPointOnNode;
		}

		// Token: 0x02000270 RID: 624
		public enum DistanceMetric : byte
		{
			// Token: 0x04000B2A RID: 2858
			Euclidean,
			// Token: 0x04000B2B RID: 2859
			ScaledManhattan
		}

		// Token: 0x02000271 RID: 625
		[BurstCompile]
		private struct NearbyNodesIterator : IEnumerator<BBTree.CloseNode>, IEnumerator, IDisposable
		{
			// Token: 0x17000210 RID: 528
			// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0005C5B1 File Offset: 0x0005A7B1
			public BBTree.CloseNode Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x06000EF6 RID: 3830 RVA: 0x0005C5B9 File Offset: 0x0005A7B9
			public bool MoveNext()
			{
				return BBTree.NearbyNodesIterator.MoveNext(ref this);
			}

			// Token: 0x06000EF7 RID: 3831 RVA: 0x000035CE File Offset: 0x000017CE
			void IDisposable.Dispose()
			{
			}

			// Token: 0x06000EF8 RID: 3832 RVA: 0x00003786 File Offset: 0x00001986
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x17000211 RID: 529
			// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x00003786 File Offset: 0x00001986
			object IEnumerator.Current
			{
				get
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x06000EFA RID: 3834 RVA: 0x0005C5C1 File Offset: 0x0005A7C1
			[BurstCompile(FloatMode = FloatMode.Default)]
			private static bool MoveNext(ref BBTree.NearbyNodesIterator it)
			{
				return BBTree.NearbyNodesIterator.MoveNext_00000DF0$BurstDirectCall.Invoke(ref it);
			}

			// Token: 0x06000EFB RID: 3835 RVA: 0x0005C5CC File Offset: 0x0005A7CC
			[BurstCompile(FloatMode = FloatMode.Default)]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static bool MoveNext$BurstManaged(ref BBTree.NearbyNodesIterator it)
			{
				float num = it.distanceThresholdSqr;
				while (it.stackSize != 0)
				{
					BBTree.NearbyNodesIterator.BoxWithDist boxWithDist = *it.stack[it.stackSize - 1];
					if (boxWithDist.distSqr > num)
					{
						it.stackSize--;
						it.indexInLeaf = 0;
					}
					else
					{
						BBTree.BBTreeBox bbtreeBox = *it.tree[boxWithDist.index];
						if (bbtreeBox.IsLeaf)
						{
							for (int i = it.indexInLeaf; i < 4; i++)
							{
								int num2 = *it.nodes[bbtreeBox.nodeOffset + i];
								if (num2 == -1)
								{
									break;
								}
								uint num3 = (uint)(num2 * 3);
								uint num4 = (uint)(num2 * 3 + 1);
								uint num5 = (uint)(num2 * 3 + 2);
								if (num5 >= it.triangles.length)
								{
									throw new Exception("Invalid node index");
								}
								Hint.Assume(num3 < it.triangles.length && num4 < it.triangles.length && num5 < it.triangles.length);
								Int3 ob = *it.vertices[*it.triangles[num3]];
								Int3 ob2 = *it.vertices[*it.triangles[num4]];
								Int3 ob3 = *it.vertices[*it.triangles[num5]];
								if (it.projection.distanceMetric == BBTree.DistanceMetric.Euclidean)
								{
									float3 @float = (float3)ob;
									float3 float2 = (float3)ob2;
									float3 float3 = (float3)ob3;
									float3 float4;
									Polygon.ClosestPointOnTriangleByRef(@float, float2, float3, it.point, out float4);
									float num6 = math.distancesq(float4, it.point);
									if (num6 < num)
									{
										it.indexInLeaf = i + 1;
										it.current = new BBTree.CloseNode
										{
											node = num2,
											distanceSq = num6,
											tieBreakingDistance = 0f,
											closestPointOnNode = float4
										};
										return true;
									}
								}
								else
								{
									float3 closestPointOnNode;
									float num7;
									float num8;
									Polygon.ClosestPointOnTriangleProjected(ref ob, ref ob2, ref ob3, ref it.projection, ref it.point, out closestPointOnNode, out num7, out num8);
									if (num7 < num || (num7 == num && num8 < it.tieBreakingDistanceThreshold))
									{
										it.indexInLeaf = i + 1;
										it.current = new BBTree.CloseNode
										{
											node = num2,
											distanceSq = num7,
											tieBreakingDistance = num8,
											closestPointOnNode = closestPointOnNode
										};
										return true;
									}
								}
							}
							it.indexInLeaf = 0;
							it.stackSize--;
						}
						else
						{
							it.stackSize--;
							int left = bbtreeBox.left;
							int right = bbtreeBox.right;
							float num9 = it.projection.SquaredRectPointDistanceOnPlane(it.tree[left].rect, it.point);
							float num10 = it.projection.SquaredRectPointDistanceOnPlane(it.tree[right].rect, it.point);
							if (num10 < num9)
							{
								Memory.Swap<int>(ref left, ref right);
								Memory.Swap<float>(ref num9, ref num10);
							}
							if (it.stackSize + 2 > it.stack.Length)
							{
								throw new InvalidOperationException("Tree is too deep. Overflowed the internal stack.");
							}
							if (num10 <= num)
							{
								int num11 = it.stackSize;
								it.stackSize = num11 + 1;
								*it.stack[num11] = new BBTree.NearbyNodesIterator.BoxWithDist
								{
									index = right,
									distSqr = num10
								};
							}
							if (num9 <= num)
							{
								int num11 = it.stackSize;
								it.stackSize = num11 + 1;
								*it.stack[num11] = new BBTree.NearbyNodesIterator.BoxWithDist
								{
									index = left,
									distSqr = num9
								};
							}
						}
					}
				}
				return false;
			}

			// Token: 0x04000B2C RID: 2860
			public UnsafeSpan<BBTree.NearbyNodesIterator.BoxWithDist> stack;

			// Token: 0x04000B2D RID: 2861
			public int stackSize;

			// Token: 0x04000B2E RID: 2862
			public UnsafeSpan<BBTree.BBTreeBox> tree;

			// Token: 0x04000B2F RID: 2863
			public UnsafeSpan<int> nodes;

			// Token: 0x04000B30 RID: 2864
			public UnsafeSpan<int> triangles;

			// Token: 0x04000B31 RID: 2865
			public UnsafeSpan<Int3> vertices;

			// Token: 0x04000B32 RID: 2866
			public int indexInLeaf;

			// Token: 0x04000B33 RID: 2867
			public float3 point;

			// Token: 0x04000B34 RID: 2868
			public BBTree.ProjectionParams projection;

			// Token: 0x04000B35 RID: 2869
			public float distanceThresholdSqr;

			// Token: 0x04000B36 RID: 2870
			public float tieBreakingDistanceThreshold;

			// Token: 0x04000B37 RID: 2871
			internal BBTree.CloseNode current;

			// Token: 0x02000272 RID: 626
			public struct BoxWithDist
			{
				// Token: 0x04000B38 RID: 2872
				public int index;

				// Token: 0x04000B39 RID: 2873
				public float distSqr;
			}

			// Token: 0x02000273 RID: 627
			// (Invoke) Token: 0x06000EFD RID: 3837
			internal delegate bool MoveNext_00000DF0$PostfixBurstDelegate(ref BBTree.NearbyNodesIterator it);

			// Token: 0x02000274 RID: 628
			internal static class MoveNext_00000DF0$BurstDirectCall
			{
				// Token: 0x06000F00 RID: 3840 RVA: 0x0005C984 File Offset: 0x0005AB84
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (BBTree.NearbyNodesIterator.MoveNext_00000DF0$BurstDirectCall.Pointer == 0)
					{
						BBTree.NearbyNodesIterator.MoveNext_00000DF0$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BBTree.NearbyNodesIterator.MoveNext_00000DF0$BurstDirectCall.DeferredCompilation, methodof(BBTree.NearbyNodesIterator.MoveNext$BurstManaged(BBTree.NearbyNodesIterator*)).MethodHandle, typeof(BBTree.NearbyNodesIterator.MoveNext_00000DF0$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = BBTree.NearbyNodesIterator.MoveNext_00000DF0$BurstDirectCall.Pointer;
				}

				// Token: 0x06000F01 RID: 3841 RVA: 0x0005C9B0 File Offset: 0x0005ABB0
				private static IntPtr GetFunctionPointer()
				{
					IntPtr result = (IntPtr)0;
					BBTree.NearbyNodesIterator.MoveNext_00000DF0$BurstDirectCall.GetFunctionPointerDiscard(ref result);
					return result;
				}

				// Token: 0x06000F02 RID: 3842 RVA: 0x0005C9C8 File Offset: 0x0005ABC8
				public unsafe static void Constructor()
				{
					BBTree.NearbyNodesIterator.MoveNext_00000DF0$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BBTree.NearbyNodesIterator.MoveNext(BBTree.NearbyNodesIterator*)).MethodHandle);
				}

				// Token: 0x06000F03 RID: 3843 RVA: 0x000035CE File Offset: 0x000017CE
				public static void Initialize()
				{
				}

				// Token: 0x06000F04 RID: 3844 RVA: 0x0005C9D9 File Offset: 0x0005ABD9
				// Note: this type is marked as 'beforefieldinit'.
				static MoveNext_00000DF0$BurstDirectCall()
				{
					BBTree.NearbyNodesIterator.MoveNext_00000DF0$BurstDirectCall.Constructor();
				}

				// Token: 0x06000F05 RID: 3845 RVA: 0x0005C9E0 File Offset: 0x0005ABE0
				public static bool Invoke(ref BBTree.NearbyNodesIterator it)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = BBTree.NearbyNodesIterator.MoveNext_00000DF0$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							return calli(System.Boolean(Pathfinding.Collections.BBTree/NearbyNodesIterator&), ref it, functionPointer);
						}
					}
					return BBTree.NearbyNodesIterator.MoveNext$BurstManaged(ref it);
				}

				// Token: 0x04000B3A RID: 2874
				private static IntPtr Pointer;

				// Token: 0x04000B3B RID: 2875
				private static IntPtr DeferredCompilation;
			}
		}

		// Token: 0x02000275 RID: 629
		private struct BBTreeBox
		{
			// Token: 0x17000212 RID: 530
			// (get) Token: 0x06000F06 RID: 3846 RVA: 0x0005CA11 File Offset: 0x0005AC11
			public bool IsLeaf
			{
				get
				{
					return this.nodeOffset >= 0;
				}
			}

			// Token: 0x06000F07 RID: 3847 RVA: 0x0005CA20 File Offset: 0x0005AC20
			public BBTreeBox(IntRect rect)
			{
				this.nodeOffset = -1;
				this.rect = rect;
				this.left = (this.right = -1);
			}

			// Token: 0x04000B3C RID: 2876
			public IntRect rect;

			// Token: 0x04000B3D RID: 2877
			public int nodeOffset;

			// Token: 0x04000B3E RID: 2878
			public int left;

			// Token: 0x04000B3F RID: 2879
			public int right;
		}

		// Token: 0x02000276 RID: 630
		// (Invoke) Token: 0x06000F09 RID: 3849
		internal delegate void Build_00000DDE$PostfixBurstDelegate(ref UnsafeSpan<int> triangles, ref UnsafeSpan<Int3> vertices, out BBTree bbTree);

		// Token: 0x02000277 RID: 631
		internal static class Build_00000DDE$BurstDirectCall
		{
			// Token: 0x06000F0C RID: 3852 RVA: 0x0005CA4B File Offset: 0x0005AC4B
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (BBTree.Build_00000DDE$BurstDirectCall.Pointer == 0)
				{
					BBTree.Build_00000DDE$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BBTree.Build_00000DDE$BurstDirectCall.DeferredCompilation, methodof(BBTree.Build$BurstManaged(UnsafeSpan<int>*, UnsafeSpan<Int3>*, BBTree*)).MethodHandle, typeof(BBTree.Build_00000DDE$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = BBTree.Build_00000DDE$BurstDirectCall.Pointer;
			}

			// Token: 0x06000F0D RID: 3853 RVA: 0x0005CA78 File Offset: 0x0005AC78
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				BBTree.Build_00000DDE$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000F0E RID: 3854 RVA: 0x0005CA90 File Offset: 0x0005AC90
			public unsafe static void Constructor()
			{
				BBTree.Build_00000DDE$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BBTree.Build(UnsafeSpan<int>*, UnsafeSpan<Int3>*, BBTree*)).MethodHandle);
			}

			// Token: 0x06000F0F RID: 3855 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000F10 RID: 3856 RVA: 0x0005CAA1 File Offset: 0x0005ACA1
			// Note: this type is marked as 'beforefieldinit'.
			static Build_00000DDE$BurstDirectCall()
			{
				BBTree.Build_00000DDE$BurstDirectCall.Constructor();
			}

			// Token: 0x06000F11 RID: 3857 RVA: 0x0005CAA8 File Offset: 0x0005ACA8
			public static void Invoke(ref UnsafeSpan<int> triangles, ref UnsafeSpan<Int3> vertices, out BBTree bbTree)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = BBTree.Build_00000DDE$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<System.Int32>&,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Int3>&,Pathfinding.Collections.BBTree&), ref triangles, ref vertices, ref bbTree, functionPointer);
						return;
					}
				}
				BBTree.Build$BurstManaged(ref triangles, ref vertices, out bbTree);
			}

			// Token: 0x04000B40 RID: 2880
			private static IntPtr Pointer;

			// Token: 0x04000B41 RID: 2881
			private static IntPtr DeferredCompilation;
		}
	}
}
