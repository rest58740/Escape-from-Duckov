using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x0200019C RID: 412
	[BurstCompile]
	internal static class ColliderMeshBuilder2D
	{
		// Token: 0x06000B43 RID: 2883 RVA: 0x0003F39C File Offset: 0x0003D59C
		private static int GetShapes(Collider2D coll, PhysicsShapeGroup2D group, HashSet<Rigidbody2D> handledRigidbodies)
		{
			Rigidbody2D attachedRigidbody = coll.attachedRigidbody;
			if (!(attachedRigidbody != null))
			{
				TilemapCollider2D tilemapCollider2D = coll as TilemapCollider2D;
				if (tilemapCollider2D != null)
				{
					tilemapCollider2D.ProcessTilemapChanges();
				}
				return coll.GetShapes(group);
			}
			if (handledRigidbodies.Add(attachedRigidbody))
			{
				return attachedRigidbody.GetShapes(group);
			}
			return 0;
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0003F3E4 File Offset: 0x0003D5E4
		public unsafe static int GenerateMeshesFromColliders(Collider2D[] colliders, int numColliders, float maxError, out NativeArray<float3> outputVertices, out NativeArray<int> outputIndices, out NativeArray<ColliderMeshBuilder2D.ShapeMesh> outputShapeMeshes)
		{
			PhysicsShapeGroup2D physicsShapeGroup2D = new PhysicsShapeGroup2D(1, 8);
			NativeList<PhysicsShape2D> list = new NativeList<PhysicsShape2D>(numColliders, Allocator.Temp);
			NativeList<Vector2> list2 = new NativeList<Vector2>(numColliders * 4, Allocator.Temp);
			NativeList<Matrix4x4> list3 = new NativeList<Matrix4x4>(numColliders, Allocator.Temp);
			NativeList<int> list4 = new NativeList<int>(numColliders, Allocator.Temp);
			HashSet<Rigidbody2D> handledRigidbodies = new HashSet<Rigidbody2D>();
			int num = 0;
			for (int i = 0; i < numColliders; i++)
			{
				Collider2D collider2D = colliders[i];
				if (!(collider2D == null) && collider2D.shapeCount != 0)
				{
					int shapes = ColliderMeshBuilder2D.GetShapes(collider2D, physicsShapeGroup2D, handledRigidbodies);
					if (shapes != 0)
					{
						list.Length += shapes;
						list2.Length += physicsShapeGroup2D.vertexCount;
						NativeArray<PhysicsShape2D> subArray = list.AsArray().GetSubArray(list.Length - shapes, shapes);
						NativeArray<Vector2> subArray2 = list2.AsArray().GetSubArray(list2.Length - physicsShapeGroup2D.vertexCount, physicsShapeGroup2D.vertexCount);
						physicsShapeGroup2D.GetShapeData(subArray, subArray2);
						for (int j = 0; j < shapes; j++)
						{
							PhysicsShape2D value = subArray[j];
							value.vertexStartIndex += num;
							subArray[j] = value;
						}
						num += subArray2.Length;
						Matrix4x4 localToWorldMatrix = physicsShapeGroup2D.localToWorldMatrix;
						list3.AddReplicate(localToWorldMatrix, shapes);
						list4.AddReplicate(i, shapes);
					}
				}
			}
			NativeList<float3> nativeList = new NativeList<float3>(Allocator.Temp);
			NativeList<int3> nativeList2 = new NativeList<int3>(Allocator.Temp);
			UnsafeSpan<PhysicsShape2D> unsafeSpan = list.AsUnsafeSpan<PhysicsShape2D>();
			UnsafeSpan<float2> unsafeSpan2 = list2.AsUnsafeSpan<Vector2>().Reinterpret<float2>();
			UnsafeSpan<Matrix4x4> unsafeSpan3 = list3.AsUnsafeSpan<Matrix4x4>();
			UnsafeSpan<int> unsafeSpan4 = list4.AsUnsafeSpan<int>();
			outputShapeMeshes = new NativeArray<ColliderMeshBuilder2D.ShapeMesh>(list.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh> unsafeSpan5 = outputShapeMeshes.AsUnsafeSpan<ColliderMeshBuilder2D.ShapeMesh>();
			int result = ColliderMeshBuilder2D.GenerateMeshesFromShapes(ref unsafeSpan, ref unsafeSpan2, ref unsafeSpan3, ref unsafeSpan4, UnsafeUtility.AsRef<UnsafeList<float3>>((void*)nativeList.GetUnsafeList()), UnsafeUtility.AsRef<UnsafeList<int3>>((void*)nativeList2.GetUnsafeList()), ref unsafeSpan5, maxError);
			outputVertices = nativeList.ToArray(Allocator.Persistent);
			outputIndices = new NativeArray<int>(nativeList2.AsArray().Reinterpret<int>(12), Allocator.Persistent);
			return result;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0003F61C File Offset: 0x0003D81C
		private static void AddCapsuleMesh(float2 c1, float2 c2, ref Matrix4x4 shapeMatrix, float radius, float maxError, ref UnsafeList<float3> outputVertices, ref UnsafeList<int3> outputIndices, ref float3 mn, ref float3 mx)
		{
			int num = math.max(4, CircleGeometryUtilities.CircleSteps(shapeMatrix, radius, maxError));
			num = num / 2 + 1;
			radius *= CircleGeometryUtilities.CircleRadiusAdjustmentFactor(2 * (num - 1));
			Vector3 a = new Vector3(c1.x, c1.y, 0f);
			Vector3 a2 = new Vector3(c2.x, c2.y, 0f);
			float2 @float = math.normalizesafe(c2 - c1, default(float2));
			float2 float2 = new float2(-@float.y, @float.x);
			Vector3 a3 = radius * new Vector3(float2.x, float2.y, 0f);
			Vector3 a4 = radius * new Vector3(@float.x, @float.y, 0f);
			float num2 = 3.1415927f / (float)(num - 1);
			int length = outputVertices.Length;
			int num3 = length + num;
			outputVertices.Length += num * 2;
			for (int i = 0; i < num; i++)
			{
				float d;
				float d2;
				math.sincos(num2 * (float)i, out d, out d2);
				Vector3 v = a + d2 * a3 - d * a4;
				mn = math.min(mn, v);
				mx = math.max(mx, v);
				outputVertices[length + i] = v;
				v = a2 - d2 * a3 + d * a4;
				mn = math.min(mn, v);
				mx = math.max(mx, v);
				outputVertices[num3 + i] = v;
			}
			int length2 = outputIndices.Length;
			int num4 = length2 + num - 2;
			outputIndices.Length += (num - 2) * 2;
			for (int j = 1; j < num - 1; j++)
			{
				outputIndices[length2 + j - 1] = new int3(length, length + j, length + j + 1);
				outputIndices[num4 + j - 1] = new int3(num3, num3 + j, num3 + j + 1);
			}
			int3 @int = new int3(length, length + num - 1, num3);
			outputIndices.Add(@int);
			@int = new int3(length, num3, num3 + num - 1);
			outputIndices.Add(@int);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0003F8B4 File Offset: 0x0003DAB4
		[BurstCompile]
		public static int GenerateMeshesFromShapes(ref UnsafeSpan<PhysicsShape2D> shapes, ref UnsafeSpan<float2> vertices, ref UnsafeSpan<Matrix4x4> shapeMatrices, ref UnsafeSpan<int> groupIndices, ref UnsafeList<float3> outputVertices, ref UnsafeList<int3> outputIndices, ref UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh> outputShapeMeshes, float maxError)
		{
			return ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000A8B$BurstDirectCall.Invoke(ref shapes, ref vertices, ref shapeMatrices, ref groupIndices, ref outputVertices, ref outputIndices, ref outputShapeMeshes, maxError);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0003F8C8 File Offset: 0x0003DAC8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int GenerateMeshesFromShapes$BurstManaged(ref UnsafeSpan<PhysicsShape2D> shapes, ref UnsafeSpan<float2> vertices, ref UnsafeSpan<Matrix4x4> shapeMatrices, ref UnsafeSpan<int> groupIndices, ref UnsafeList<float3> outputVertices, ref UnsafeList<int3> outputIndices, ref UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh> outputShapeMeshes, float maxError)
		{
			int num = 0;
			float3 @float = new float3(float.MaxValue, float.MaxValue, float.MaxValue);
			float3 float2 = new float3(float.MinValue, float.MinValue, float.MinValue);
			int result = 0;
			for (int i = 0; i < shapes.Length; i++)
			{
				PhysicsShape2D physicsShape2D = *shapes[i];
				UnsafeSpan<float2> unsafeSpan = vertices.Slice(physicsShape2D.vertexStartIndex, physicsShape2D.vertexCount);
				Matrix4x4 matrix4x = *shapeMatrices[i];
				switch (physicsShape2D.shapeType)
				{
				case PhysicsShapeType2D.Circle:
				{
					int num2 = CircleGeometryUtilities.CircleSteps(matrix4x, physicsShape2D.radius, maxError);
					float num3 = physicsShape2D.radius * CircleGeometryUtilities.CircleRadiusAdjustmentFactor(num2);
					Vector3 a = new Vector3(unsafeSpan[0].x, unsafeSpan[0].y, 0f);
					Vector3 a2 = new Vector3(num3, 0f, 0f);
					Vector3 a3 = new Vector3(0f, num3, 0f);
					float num4 = 6.2831855f / (float)num2;
					int length = outputVertices.Length;
					for (int j = 0; j < num2; j++)
					{
						float d;
						float d2;
						math.sincos(num4 * (float)j, out d, out d2);
						Vector3 v = a + d2 * a2 + d * a3;
						@float = math.min(@float, v);
						float2 = math.max(float2, v);
						float3 float3 = v;
						outputVertices.Add(float3);
					}
					for (int k = 1; k < num2; k++)
					{
						int3 @int = new int3(length, length + k, length + (k + 1) % num2);
						outputIndices.Add(@int);
					}
					break;
				}
				case PhysicsShapeType2D.Capsule:
				{
					float2 c = *unsafeSpan[0];
					float2 c2 = *unsafeSpan[1];
					ColliderMeshBuilder2D.AddCapsuleMesh(c, c2, ref matrix4x, physicsShape2D.radius, maxError, ref outputVertices, ref outputIndices, ref @float, ref float2);
					break;
				}
				case PhysicsShapeType2D.Polygon:
				{
					int length2 = outputVertices.Length;
					outputVertices.Resize(length2 + physicsShape2D.vertexCount, NativeArrayOptions.UninitializedMemory);
					for (int l = 0; l < physicsShape2D.vertexCount; l++)
					{
						Vector3 v2 = new Vector3(unsafeSpan[l].x, unsafeSpan[l].y, 0f);
						@float = math.min(@float, v2);
						float2 = math.max(float2, v2);
						outputVertices[length2 + l] = v2;
					}
					outputIndices.SetCapacity(math.ceilpow2(outputIndices.Length + (physicsShape2D.vertexCount - 2)));
					for (int m = 1; m < physicsShape2D.vertexCount - 1; m++)
					{
						outputIndices.AddNoResize(new int3(length2, length2 + m, length2 + m + 1));
					}
					break;
				}
				case PhysicsShapeType2D.Edges:
					if (physicsShape2D.radius > maxError)
					{
						for (int n = 0; n < physicsShape2D.vertexCount - 1; n++)
						{
							ColliderMeshBuilder2D.AddCapsuleMesh(*unsafeSpan[n], *unsafeSpan[n + 1], ref matrix4x, physicsShape2D.radius, maxError, ref outputVertices, ref outputIndices, ref @float, ref float2);
						}
					}
					else
					{
						int length3 = outputVertices.Length;
						outputVertices.Resize(length3 + physicsShape2D.vertexCount, NativeArrayOptions.UninitializedMemory);
						for (int num5 = 0; num5 < physicsShape2D.vertexCount; num5++)
						{
							Vector3 v3 = new Vector3(unsafeSpan[num5].x, unsafeSpan[num5].y, 0f);
							@float = math.min(@float, v3);
							float2 = math.max(float2, v3);
							outputVertices[length3 + num5] = v3;
						}
						outputIndices.SetCapacity(math.ceilpow2(outputIndices.Length + (physicsShape2D.vertexCount - 1)));
						for (int num6 = 0; num6 < physicsShape2D.vertexCount - 1; num6++)
						{
							outputIndices.AddNoResize(new int3(length3 + num6, length3 + num6 + 1, length3 + num6 + 1));
						}
					}
					break;
				default:
					throw new Exception("Unexpected PhysicsShapeType2D");
				}
				if (i == shapes.Length - 1 || *groupIndices[i] != *groupIndices[i + 1] || outputIndices.Length - num > 100)
				{
					ToWorldMatrix toWorldMatrix = new ToWorldMatrix(new float3x3(matrix4x));
					Bounds bounds = new Bounds((@float + float2) * 0.5f, float2 - @float);
					bounds = toWorldMatrix.ToWorld(bounds);
					bounds.center += matrix4x.GetColumn(3);
					*outputShapeMeshes[result++] = new ColliderMeshBuilder2D.ShapeMesh
					{
						bounds = bounds,
						matrix = matrix4x,
						startIndex = num * 3,
						endIndex = outputIndices.Length * 3,
						tag = *groupIndices[i]
					};
					@float = new float3(float.MaxValue, float.MaxValue, float.MaxValue);
					float2 = new float3(float.MinValue, float.MinValue, float.MinValue);
					num = outputIndices.Length;
				}
			}
			return result;
		}

		// Token: 0x0200019D RID: 413
		public struct ShapeMesh
		{
			// Token: 0x040007A9 RID: 1961
			public Matrix4x4 matrix;

			// Token: 0x040007AA RID: 1962
			public Bounds bounds;

			// Token: 0x040007AB RID: 1963
			public int startIndex;

			// Token: 0x040007AC RID: 1964
			public int endIndex;

			// Token: 0x040007AD RID: 1965
			public int tag;
		}

		// Token: 0x0200019E RID: 414
		// (Invoke) Token: 0x06000B49 RID: 2889
		internal delegate int GenerateMeshesFromShapes_00000A8B$PostfixBurstDelegate(ref UnsafeSpan<PhysicsShape2D> shapes, ref UnsafeSpan<float2> vertices, ref UnsafeSpan<Matrix4x4> shapeMatrices, ref UnsafeSpan<int> groupIndices, ref UnsafeList<float3> outputVertices, ref UnsafeList<int3> outputIndices, ref UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh> outputShapeMeshes, float maxError);

		// Token: 0x0200019F RID: 415
		internal static class GenerateMeshesFromShapes_00000A8B$BurstDirectCall
		{
			// Token: 0x06000B4C RID: 2892 RVA: 0x0003FE23 File Offset: 0x0003E023
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000A8B$BurstDirectCall.Pointer == 0)
				{
					ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000A8B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000A8B$BurstDirectCall.DeferredCompilation, methodof(ColliderMeshBuilder2D.GenerateMeshesFromShapes$BurstManaged(UnsafeSpan<PhysicsShape2D>*, UnsafeSpan<float2>*, UnsafeSpan<Matrix4x4>*, UnsafeSpan<int>*, UnsafeList<float3>*, UnsafeList<int3>*, UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh>*, float)).MethodHandle, typeof(ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000A8B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000A8B$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B4D RID: 2893 RVA: 0x0003FE50 File Offset: 0x0003E050
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000A8B$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000B4E RID: 2894 RVA: 0x0003FE68 File Offset: 0x0003E068
			public unsafe static void Constructor()
			{
				ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000A8B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ColliderMeshBuilder2D.GenerateMeshesFromShapes(UnsafeSpan<PhysicsShape2D>*, UnsafeSpan<float2>*, UnsafeSpan<Matrix4x4>*, UnsafeSpan<int>*, UnsafeList<float3>*, UnsafeList<int3>*, UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh>*, float)).MethodHandle);
			}

			// Token: 0x06000B4F RID: 2895 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000B50 RID: 2896 RVA: 0x0003FE79 File Offset: 0x0003E079
			// Note: this type is marked as 'beforefieldinit'.
			static GenerateMeshesFromShapes_00000A8B$BurstDirectCall()
			{
				ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000A8B$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B51 RID: 2897 RVA: 0x0003FE80 File Offset: 0x0003E080
			public static int Invoke(ref UnsafeSpan<PhysicsShape2D> shapes, ref UnsafeSpan<float2> vertices, ref UnsafeSpan<Matrix4x4> shapeMatrices, ref UnsafeSpan<int> groupIndices, ref UnsafeList<float3> outputVertices, ref UnsafeList<int3> outputIndices, ref UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh> outputShapeMeshes, float maxError)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000A8B$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(Pathfinding.Collections.UnsafeSpan`1<UnityEngine.PhysicsShape2D>&,Pathfinding.Collections.UnsafeSpan`1<Unity.Mathematics.float2>&,Pathfinding.Collections.UnsafeSpan`1<UnityEngine.Matrix4x4>&,Pathfinding.Collections.UnsafeSpan`1<System.Int32>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Mathematics.float3>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Mathematics.int3>&,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Graphs.Navmesh.ColliderMeshBuilder2D/ShapeMesh>&,System.Single), ref shapes, ref vertices, ref shapeMatrices, ref groupIndices, ref outputVertices, ref outputIndices, ref outputShapeMeshes, maxError, functionPointer);
					}
				}
				return ColliderMeshBuilder2D.GenerateMeshesFromShapes$BurstManaged(ref shapes, ref vertices, ref shapeMatrices, ref groupIndices, ref outputVertices, ref outputIndices, ref outputShapeMeshes, maxError);
			}

			// Token: 0x040007AE RID: 1966
			private static IntPtr Pointer;

			// Token: 0x040007AF RID: 1967
			private static IntPtr DeferredCompilation;
		}
	}
}
