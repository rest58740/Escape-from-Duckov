using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pathfinding.Util
{
	// Token: 0x02000295 RID: 661
	[BurstCompile]
	internal static class MeshUtility
	{
		// Token: 0x06000FDA RID: 4058 RVA: 0x00060EAC File Offset: 0x0005F0AC
		public static void GetMeshData(Mesh.MeshDataArray meshData, int meshIndex, out NativeArray<Vector3> vertices, out NativeArray<int> indices)
		{
			Mesh.MeshData meshData2 = meshData[meshIndex];
			vertices = new NativeArray<Vector3>(meshData2.vertexCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			meshData2.GetVertices(vertices);
			int num = 0;
			for (int i = 0; i < meshData2.subMeshCount; i++)
			{
				num += meshData2.GetSubMesh(i).indexCount;
			}
			indices = new NativeArray<int>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			int num2 = 0;
			for (int j = 0; j < meshData2.subMeshCount; j++)
			{
				SubMeshDescriptor subMesh = meshData2.GetSubMesh(j);
				meshData2.GetIndices(indices.GetSubArray(num2, subMesh.indexCount), j, true);
				num2 += subMesh.indexCount;
			}
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x00060F5E File Offset: 0x0005F15E
		[BurstCompile]
		public static void MakeTrianglesClockwise(ref UnsafeSpan<Int3> vertices, ref UnsafeSpan<int> triangles)
		{
			MeshUtility.MakeTrianglesClockwise_00000EBC$BurstDirectCall.Invoke(ref vertices, ref triangles);
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00060F68 File Offset: 0x0005F168
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void MakeTrianglesClockwise$BurstManaged(ref UnsafeSpan<Int3> vertices, ref UnsafeSpan<int> triangles)
		{
			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (!VectorMath.IsClockwiseXZ(*vertices[*triangles[i]], *vertices[*triangles[i + 1]], *vertices[*triangles[i + 2]]))
				{
					int num = *triangles[i];
					*triangles[i] = *triangles[i + 2];
					*triangles[i + 2] = num;
				}
			}
		}

		// Token: 0x02000296 RID: 662
		[BurstCompile]
		public struct JobMergeNearbyVertices : IJob
		{
			// Token: 0x06000FDD RID: 4061 RVA: 0x00060FF0 File Offset: 0x0005F1F0
			public unsafe void Execute()
			{
				NativeArray<int> nativeArray = new NativeArray<int>(this.vertices.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				for (int i = 0; i < nativeArray.Length; i++)
				{
					nativeArray[i] = i;
				}
				nativeArray.Sort(new MeshUtility.JobMergeNearbyVertices.CoordinateSorter
				{
					vertices = this.vertices.AsUnsafeSpan<Int3>().Reinterpret<int3>()
				});
				UnsafeSpan<int> unsafeSpan = nativeArray.AsUnsafeSpan<int>();
				NativeArray<int> nativeArray2 = new NativeArray<int>(this.vertices.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				UnsafeSpan<int3> unsafeSpan2 = this.vertices.AsUnsafeSpan<Int3>().Reinterpret<int3>();
				UnsafeSpan<int> unsafeSpan3 = this.triangles.AsUnsafeSpan<int>();
				UnsafeSpan<int3> unsafeSpan4 = new NativeArray<int3>(this.vertices.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory).AsUnsafeSpan<int3>();
				int num = 0;
				int num2 = (int)math.ceil(math.sqrt((float)this.mergeRadiusSq));
				uint num3 = 1U;
				for (uint num4 = 0U; num4 < unsafeSpan.length; num4 += 1U)
				{
					if (*unsafeSpan[num4] != -1)
					{
						int3 @int = *unsafeSpan2[*unsafeSpan[num4]];
						nativeArray2[*unsafeSpan[num4]] = num;
						while (num3 < unsafeSpan.length && unsafeSpan2[*unsafeSpan[num3]].x <= @int.x + num2)
						{
							num3 += 1U;
						}
						int3 lhs = @int;
						int num5 = 1;
						for (uint num6 = num4 + 1U; num6 < num3; num6 += 1U)
						{
							if (*unsafeSpan[num6] != -1)
							{
								int3 int2 = *unsafeSpan2[*unsafeSpan[num6]];
								if (math.lengthsq(int2 - @int) <= (float)this.mergeRadiusSq)
								{
									lhs += int2;
									num5++;
									nativeArray2[*unsafeSpan[num6]] = num;
									*unsafeSpan[num6] = -1;
								}
							}
						}
						*unsafeSpan4[num] = lhs / num5;
						num++;
					}
				}
				this.vertices.Length = num;
				unsafeSpan4.Slice(0, num).CopyTo(this.vertices.AsUnsafeSpan<Int3>().Reinterpret<int3>());
				for (uint num7 = 0U; num7 < unsafeSpan3.length; num7 += 1U)
				{
					*unsafeSpan3[num7] = nativeArray2[*unsafeSpan3[num7]];
				}
			}

			// Token: 0x04000BA4 RID: 2980
			public NativeList<Int3> vertices;

			// Token: 0x04000BA5 RID: 2981
			public NativeList<int> triangles;

			// Token: 0x04000BA6 RID: 2982
			public int mergeRadiusSq;

			// Token: 0x02000297 RID: 663
			private struct CoordinateSorter : IComparer<int>
			{
				// Token: 0x06000FDE RID: 4062 RVA: 0x00061264 File Offset: 0x0005F464
				public int Compare(int a, int b)
				{
					Hint.Assume(a < (int)this.vertices.length);
					Hint.Assume(b < (int)this.vertices.length);
					return this.vertices[a].x.CompareTo(this.vertices[b].x);
				}

				// Token: 0x04000BA7 RID: 2983
				public UnsafeSpan<int3> vertices;
			}
		}

		// Token: 0x02000298 RID: 664
		[BurstCompile]
		public struct JobRemoveDegenerateTriangles : IJob
		{
			// Token: 0x06000FDF RID: 4063 RVA: 0x000612C0 File Offset: 0x0005F4C0
			public static int3 cross(int3 lhs, int3 rhs)
			{
				return (lhs * rhs.yzx - lhs.yzx * rhs).yzx;
			}

			// Token: 0x06000FE0 RID: 4064 RVA: 0x000612F4 File Offset: 0x0005F4F4
			public unsafe void Execute()
			{
				int num = 0;
				UnsafeSpan<int3> unsafeSpan = this.vertices.AsUnsafeSpan<Int3>().Reinterpret<int3>();
				UnsafeSpan<int3> unsafeSpan2 = this.triangles.AsUnsafeSpan<int>().Reinterpret<int3>(4);
				UnsafeSpan<int> unsafeSpan3 = this.tags.AsUnsafeSpan<int>();
				uint num2 = 0U;
				for (uint num3 = 0U; num3 < unsafeSpan2.length; num3 += 1U)
				{
					int3 @int = *unsafeSpan2[num3];
					if (math.all(MeshUtility.JobRemoveDegenerateTriangles.cross(*unsafeSpan[@int.y] - *unsafeSpan[@int.x], *unsafeSpan[@int.z] - *unsafeSpan[@int.x]) == 0))
					{
						num++;
					}
					else
					{
						*unsafeSpan2[num2] = @int;
						*unsafeSpan3[num2] = *unsafeSpan3[num3];
						num2 += 1U;
					}
				}
				this.triangles.Length = (int)(num2 * 3U);
				this.tags.Length = (int)num2;
				if (this.verbose && num > 0)
				{
					Debug.LogWarning(string.Format("Input mesh contained {0} degenerate triangles. These have been removed.\nA degenerate triangle is a triangle with zero area. It resembles a line or a point.", num));
				}
			}

			// Token: 0x04000BA8 RID: 2984
			public NativeList<Int3> vertices;

			// Token: 0x04000BA9 RID: 2985
			public NativeList<int> triangles;

			// Token: 0x04000BAA RID: 2986
			public NativeList<int> tags;

			// Token: 0x04000BAB RID: 2987
			public bool verbose;
		}

		// Token: 0x02000299 RID: 665
		// (Invoke) Token: 0x06000FE2 RID: 4066
		internal delegate void MakeTrianglesClockwise_00000EBC$PostfixBurstDelegate(ref UnsafeSpan<Int3> vertices, ref UnsafeSpan<int> triangles);

		// Token: 0x0200029A RID: 666
		internal static class MakeTrianglesClockwise_00000EBC$BurstDirectCall
		{
			// Token: 0x06000FE5 RID: 4069 RVA: 0x0006143D File Offset: 0x0005F63D
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (MeshUtility.MakeTrianglesClockwise_00000EBC$BurstDirectCall.Pointer == 0)
				{
					MeshUtility.MakeTrianglesClockwise_00000EBC$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(MeshUtility.MakeTrianglesClockwise_00000EBC$BurstDirectCall.DeferredCompilation, methodof(MeshUtility.MakeTrianglesClockwise$BurstManaged(UnsafeSpan<Int3>*, UnsafeSpan<int>*)).MethodHandle, typeof(MeshUtility.MakeTrianglesClockwise_00000EBC$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = MeshUtility.MakeTrianglesClockwise_00000EBC$BurstDirectCall.Pointer;
			}

			// Token: 0x06000FE6 RID: 4070 RVA: 0x0006146C File Offset: 0x0005F66C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				MeshUtility.MakeTrianglesClockwise_00000EBC$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000FE7 RID: 4071 RVA: 0x00061484 File Offset: 0x0005F684
			public unsafe static void Constructor()
			{
				MeshUtility.MakeTrianglesClockwise_00000EBC$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(MeshUtility.MakeTrianglesClockwise(UnsafeSpan<Int3>*, UnsafeSpan<int>*)).MethodHandle);
			}

			// Token: 0x06000FE8 RID: 4072 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000FE9 RID: 4073 RVA: 0x00061495 File Offset: 0x0005F695
			// Note: this type is marked as 'beforefieldinit'.
			static MakeTrianglesClockwise_00000EBC$BurstDirectCall()
			{
				MeshUtility.MakeTrianglesClockwise_00000EBC$BurstDirectCall.Constructor();
			}

			// Token: 0x06000FEA RID: 4074 RVA: 0x0006149C File Offset: 0x0005F69C
			public static void Invoke(ref UnsafeSpan<Int3> vertices, ref UnsafeSpan<int> triangles)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = MeshUtility.MakeTrianglesClockwise_00000EBC$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Int3>&,Pathfinding.Collections.UnsafeSpan`1<System.Int32>&), ref vertices, ref triangles, functionPointer);
						return;
					}
				}
				MeshUtility.MakeTrianglesClockwise$BurstManaged(ref vertices, ref triangles);
			}

			// Token: 0x04000BAC RID: 2988
			private static IntPtr Pointer;

			// Token: 0x04000BAD RID: 2989
			private static IntPtr DeferredCompilation;
		}
	}
}
