using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using andywiecko.BurstTriangulator;
using andywiecko.BurstTriangulator.LowLevel.Unsafe;
using AOT;
using Pathfinding.Clipper2Lib;
using Pathfinding.Collections;
using Pathfinding.Graphs.Navmesh.Voxelization;
using Pathfinding.Pooling;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001B3 RID: 435
	[BurstCompile]
	public static class TileHandler
	{
		// Token: 0x06000BAF RID: 2991 RVA: 0x000434E8 File Offset: 0x000416E8
		internal unsafe static TileHandler.CutCollection CollectCuts(GridLookup<NavmeshClipper> cuts, List<Vector2Int> tileCoordinates, float characterRadius, TileLayout tileLayout, ref UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags)
		{
			UnsafeList<float2> arr = new UnsafeList<float2>(0, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			UnsafeList<NavmeshCut.ContourBurst> contours = new UnsafeList<NavmeshCut.ContourBurst>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			UnsafeList<TileHandler.ContourMeta> contoursExtra = new UnsafeList<TileHandler.ContourMeta>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			bool flag = false;
			UnsafeList<TileHandler.TileCuts> tileCuts = new UnsafeList<TileHandler.TileCuts>(tileCoordinates.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			Int3[] array = null;
			for (int i = 0; i < tileCoordinates.Count; i++)
			{
				Vector2Int vector2Int = tileCoordinates[i];
				Vector3 min = tileLayout.GetTileBoundsInGraphSpace(vector2Int.x, vector2Int.y, 1, 1).min;
				GraphTransform graphTransform = tileLayout.transform * Matrix4x4.Translate(min);
				Vector2Int vector2Int2 = tileCoordinates[i];
				List<NavmeshCut> list = cuts.QueryRect<NavmeshCut>(new IntRect(vector2Int2.x, vector2Int2.y, vector2Int2.x, vector2Int2.y));
				int length = contours.Length;
				flag |= (list.Count > 0);
				for (int j = 0; j < list.Count; j++)
				{
					int length2 = contours.Length;
					NavmeshCut navmeshCut = list[j];
					navmeshCut.GetContourBurst(&arr, &contours, graphTransform.inverseMatrix, characterRadius);
					TileHandler.ContourMeta contourMeta = new TileHandler.ContourMeta
					{
						isDual = navmeshCut.isDual,
						cutsAddedGeom = navmeshCut.cutsAddedGeom
					};
					contoursExtra.AddReplicate(contourMeta, contours.Length - length2);
				}
				ListPool<NavmeshCut>.Release(ref list);
				List<NavmeshAdd> list2 = cuts.QueryRect<NavmeshAdd>(new IntRect(vector2Int2.x, vector2Int2.y, vector2Int2.x, vector2Int2.y));
				flag |= (list2.Count > 0);
				for (int k = 0; k < list2.Count; k++)
				{
					int[] array2;
					int num;
					list2[k].GetMesh(ref array, out array2, out num, graphTransform);
					UnsafeSpan<Int3> unsafeSpan = new UnsafeSpan<Int3>(Allocator.Persistent, num);
					UnsafeSpan<int> unsafeSpan2 = new UnsafeSpan<int>(Allocator.Persistent, array2.Length);
					for (int l = 0; l < num; l++)
					{
						*unsafeSpan[l] = array[l];
					}
					for (int m = 0; m < array2.Length; m++)
					{
						*unsafeSpan2[m] = array2[m];
					}
					UnsafeSpan<int> span = new UnsafeSpan<int>(Allocator.Persistent, array2.Length / 3);
					span.FillZeros<int>();
					tileVertices[i].Add(unsafeSpan);
					tileTriangles[i].Add(unsafeSpan2);
					tileTags[i].Add(span);
				}
				ListPool<NavmeshAdd>.Release(ref list2);
				tileCuts.AddNoResize(new TileHandler.TileCuts
				{
					contourStartIndex = length,
					contourEndIndex = contours.Length
				});
			}
			UnsafeSpan<float2> unsafeSpan3 = arr.AsUnsafeSpan<float2>();
			Vector2 tileWorldSize = tileLayout.TileWorldSize;
			UnsafeList<TileHandler.Point64Wrapper> contourVertices;
			TileHandler.ConvertVerticesAndSnapToTileBoundaries(ref unsafeSpan3, out contourVertices, ref tileWorldSize);
			ArrayPool<Int3>.Release(ref array, false);
			return new TileHandler.CutCollection
			{
				contourVertices = contourVertices,
				contours = contours,
				contoursExtra = contoursExtra,
				tileCuts = tileCuts,
				cuttingRequired = flag
			};
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x000437F4 File Offset: 0x000419F4
		[BurstCompile]
		private static void ConvertVerticesAndSnapToTileBoundaries(ref UnsafeSpan<float2> contourVertices, out UnsafeList<TileHandler.Point64Wrapper> outputVertices, ref Vector2 tileSize)
		{
			TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000AD4$BurstDirectCall.Invoke(ref contourVertices, out outputVertices, ref tileSize);
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x000437FE File Offset: 0x000419FE
		[BurstCompile]
		internal static void CutTiles(ref UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags, ref Vector2Int tileSize, ref TileHandler.CutCollection cutCollection, ref UnsafeSpan<TileMesh.TileMeshUnsafe> output, Allocator allocator)
		{
			TileHandler.CutTiles_00000AD5$BurstDirectCall.Invoke(ref tileVertices, ref tileTriangles, ref tileTags, ref tileSize, ref cutCollection, ref output, allocator);
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00043810 File Offset: 0x00041A10
		private unsafe static void ScaleUpCoordinates(UnsafeSpan<long> coords)
		{
			for (int i = 0; i < coords.Length; i++)
			{
				*coords[i] *= 16L;
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00043840 File Offset: 0x00041A40
		private unsafe static void ScaleDownCoordinates(UnsafeSpan<int> coords)
		{
			for (int i = 0; i < coords.Length; i++)
			{
				*coords[i] /= 16;
			}
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00043870 File Offset: 0x00041A70
		private unsafe static void RemoveDegenerateSegments(ref UnsafeSpan<int2> polygon)
		{
			for (int i = 0; i < polygon.Length; i++)
			{
				int2 @int = *polygon[i];
				int2 int2 = *polygon[(i + 1) % polygon.Length];
				int2 int3 = *polygon[(i + 2) % polygon.Length];
				if (VectorMath.IsColinear(@int, int2, int3) && math.dot(int2 - @int, int3 - int2) < 0)
				{
					UnsafeSpan<int2>.RemoveAt(ref polygon, (i + 1) % polygon.Length);
					i--;
				}
			}
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x000438FC File Offset: 0x00041AFC
		private unsafe static void CollectCutsTouchingBounds(UnsafeSpan<IntBounds> cutBounds, NativeList<int> outputCutIndices, IntBounds bounds)
		{
			outputCutIndices.Clear();
			for (int i = 0; i < cutBounds.Length; i++)
			{
				if (IntBounds.Intersects(bounds, *cutBounds[i]))
				{
					outputCutIndices.AddNoResize(i);
				}
			}
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00043940 File Offset: 0x00041B40
		private static IntBounds TriangleBounds(Int3 a, Int3 b, Int3 c)
		{
			int3 x = (int3)a;
			int3 @int = (int3)a;
			int3 min = math.min(math.min(x, (int3)b), (int3)c);
			@int = math.max(@int, (int3)b);
			@int = math.max(@int, (int3)c);
			return new IntBounds(min, @int + 1);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00043998 File Offset: 0x00041B98
		private static TileMesh.TileMeshUnsafe CompressAndRefineTile(NativeList<Int3> tileOutputVertices, NativeList<int> tileOutputTriangles, NativeList<int> tileOutputTags, Allocator allocator)
		{
			MeshUtility.JobMergeNearbyVertices jobMergeNearbyVertices = default(MeshUtility.JobMergeNearbyVertices);
			jobMergeNearbyVertices.vertices = tileOutputVertices;
			jobMergeNearbyVertices.triangles = tileOutputTriangles;
			jobMergeNearbyVertices.mergeRadiusSq = 8;
			jobMergeNearbyVertices.Execute();
			MeshUtility.JobRemoveDegenerateTriangles jobRemoveDegenerateTriangles = default(MeshUtility.JobRemoveDegenerateTriangles);
			jobRemoveDegenerateTriangles.vertices = tileOutputVertices;
			jobRemoveDegenerateTriangles.triangles = tileOutputTriangles;
			jobRemoveDegenerateTriangles.tags = tileOutputTags;
			jobRemoveDegenerateTriangles.verbose = false;
			jobRemoveDegenerateTriangles.Execute();
			int num = TileHandler.DelaunayRefinement(tileOutputVertices.AsUnsafeSpan<Int3>(), tileOutputTriangles.AsUnsafeSpan<int>(), tileOutputTags.AsUnsafeSpan<int>(), true, true);
			tileOutputTriangles.Length = num;
			tileOutputTags.Length = num / 3;
			return new TileMesh.TileMeshUnsafe
			{
				verticesInTileSpace = tileOutputVertices.AsUnsafeSpan<Int3>().Clone(allocator),
				triangles = tileOutputTriangles.AsUnsafeSpan<int>().Clone(allocator),
				tags = tileOutputTags.AsUnsafeSpan<int>().Reinterpret<uint>().Clone(allocator)
			};
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00043A80 File Offset: 0x00041C80
		private static void CopyTriangulationToOutput(ref OutputData<int2> triangulatorOutput, NativeList<Int3> tileOutputVertices, NativeList<int> tileOutputTriangles, NativeList<int> tileOutputTags, int tag, Int3 a, Int3 b, Int3 c)
		{
			int length = tileOutputVertices.Length;
			Polygon.BarycentricTriangleInterpolator barycentricTriangleInterpolator = new Polygon.BarycentricTriangleInterpolator(a, b, c);
			for (int i = 0; i < triangulatorOutput.Positions.Length; i++)
			{
				Int3 @int = new Int3(triangulatorOutput.Positions[i].x, 0, triangulatorOutput.Positions[i].y);
				@int.y = barycentricTriangleInterpolator.SampleY(new int2(@int.x, @int.z));
				tileOutputVertices.Add(@int);
			}
			tileOutputTags.AddReplicate(tag, triangulatorOutput.Triangles.Length / 3);
			for (int j = 0; j < triangulatorOutput.Triangles.Length; j += 3)
			{
				int num = triangulatorOutput.Triangles[j] + length;
				int num2 = triangulatorOutput.Triangles[j + 1] + length;
				int num3 = triangulatorOutput.Triangles[j + 2] + length;
				tileOutputTriangles.Add(num);
				tileOutputTriangles.Add(num2);
				tileOutputTriangles.Add(num3);
			}
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00043B8C File Offset: 0x00041D8C
		private unsafe static void SnapEdges(ref NativeArray<TileHandler.Point64Wrapper> triBuffer, ref int vertexCount, UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> contours, Vector2Int tileSize)
		{
			int i = 0;
			int index = vertexCount - 1;
			while (i < vertexCount)
			{
				int2 @int = new int2((int)triBuffer[index].x, (int)triBuffer[index].y);
				int2 int2 = new int2((int)triBuffer[i].x, (int)triBuffer[i].y);
				int2 int3 = int2 - @int;
				long num = (long)int3.x * (long)int3.x + (long)int3.y * (long)int3.y;
				long num2 = (long)math.sqrt((double)num) * 2L;
				for (int j = 0; j < contours.Length; j++)
				{
					UnsafeSpan<TileHandler.Point64Wrapper> unsafeSpan = *contours[j];
					for (uint num3 = 0U; num3 < unsafeSpan.length; num3 += 1U)
					{
						int2 int4 = new int2((int)unsafeSpan[num3].x, (int)unsafeSpan[num3].y);
						if (math.abs(VectorMath.SignedTriangleAreaTimes2(@int, int2, int4)) <= num2)
						{
							int2 int5 = int4 - @int;
							long num4 = (long)int5.x * (long)int3.x + (long)int5.y * (long)int3.y;
							if (num4 > 0L && num4 < num && (@int.x != 0 || int2.x != 0 || int4.x == 0) && (@int.y != 0 || int2.y != 0 || int4.y == 0) && (@int.x != tileSize.x || int2.x != tileSize.x || int4.x == tileSize.x) && (@int.y != tileSize.y || int2.y != tileSize.y || int4.y == tileSize.y))
							{
								if (triBuffer.Length < vertexCount + 1)
								{
									NativeArray<TileHandler.Point64Wrapper> nativeArray = new NativeArray<TileHandler.Point64Wrapper>(vertexCount * 2, Allocator.Temp, NativeArrayOptions.ClearMemory);
									triBuffer.AsUnsafeSpan<TileHandler.Point64Wrapper>().CopyTo(nativeArray.AsUnsafeSpan<TileHandler.Point64Wrapper>());
									triBuffer.Dispose();
									triBuffer = nativeArray;
								}
								triBuffer.AsUnsafeSpan<TileHandler.Point64Wrapper>().Move(i, i + 1, vertexCount - i);
								triBuffer[i] = new TileHandler.Point64Wrapper((long)int4.x, (long)int4.y);
								vertexCount++;
								int2 = int4;
								int3 = int2 - @int;
								num = (long)int3.x * (long)int3.x + (long)int3.y * (long)int3.y;
								num2 = (long)math.sqrt((double)num) * 2L;
							}
						}
					}
				}
				index = i;
				i++;
			}
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00043E50 File Offset: 0x00042050
		private static NativeArray<IntBounds> CalculateCutBounds(ref TileHandler.CutCollection cutCollection, ref UnsafeList<TileHandler.Point64Wrapper> contourVerticesP64)
		{
			NativeArray<IntBounds> result = new NativeArray<IntBounds>(cutCollection.contours.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < cutCollection.contours.Length; i++)
			{
				NavmeshCut.ContourBurst contourBurst = cutCollection.contours[i];
				int2 @int = new int2(int.MaxValue, int.MaxValue);
				int2 int2 = new int2(int.MinValue, int.MinValue);
				for (int j = contourBurst.startIndex; j < contourBurst.endIndex; j++)
				{
					int2 y = new int2((int)contourVerticesP64[j].x, (int)contourVerticesP64[j].y);
					@int = math.min(@int, y);
					int2 = math.max(int2, y);
				}
				result[i] = new IntBounds(new int3(@int.x, (int)(contourBurst.ymin * 1000f), @int.y), new int3(int2.x + 1, (int)math.ceil(contourBurst.ymax * 1000f), int2.y + 1));
			}
			return result;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00043F64 File Offset: 0x00042164
		private unsafe static void AddContours(Clipper64 clipper, ref UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> contours)
		{
			for (int i = 0; i < contours.Length; i++)
			{
				clipper.AddPath((Point64*)contours[i].ptr, contours[i].Length, PathType.Clip, false);
			}
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00043FA4 File Offset: 0x000421A4
		private static void CopyClipperOutput(List<List<Point64>> closedSolutions, ref UnsafeList<Vector2Int> outputVertices, ref UnsafeList<int> outputVertexCountPerPolygon)
		{
			outputVertexCountPerPolygon.Length = closedSolutions.Count;
			for (int i = 0; i < closedSolutions.Count; i++)
			{
				List<Point64> list = closedSolutions[i];
				for (int j = 0; j < list.Count; j++)
				{
					Vector2Int vector2Int = new Vector2Int((int)list[j].X, (int)list[j].Y);
					outputVertices.Add(vector2Int);
				}
				outputVertexCountPerPolygon[i] = list.Count;
			}
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0004401C File Offset: 0x0004221C
		[MonoPInvokeCallback(typeof(TileHandler.CutFunction))]
		private unsafe static bool CutPolygon(ref UnsafeSpan<TileHandler.Point64Wrapper> subject, ref UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> contours, ref UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> contoursDual, ref UnsafeList<Vector2Int> outputVertices, ref UnsafeList<int> outputVertexCountPerPolygon, int mode)
		{
			int threadIndex = JobsUtility.ThreadIndex;
			Clipper64 clipper = TileHandlerCache.cachedClippers[threadIndex] = (TileHandlerCache.cachedClippers[threadIndex] ?? new Clipper64());
			clipper.PreserveCollinear = true;
			List<List<Point64>> list = ListPool<List<Point64>>.Claim();
			List<List<Point64>> list2 = ListPool<List<Point64>>.Claim();
			if (mode != 1)
			{
				clipper.Clear();
				clipper.AddPath((Point64*)subject.ptr, subject.Length, PathType.Subject, false);
				TileHandler.AddContours(clipper, ref contours);
				TileHandler.AddContours(clipper, ref contoursDual);
			}
			else
			{
				clipper.Clear();
				clipper.AddPath((Point64*)subject.ptr, subject.Length, PathType.Subject, false);
				TileHandler.AddContours(clipper, ref contoursDual);
				if (!clipper.Execute(ClipType.Intersection, FillRule.NonZero, list, list2))
				{
					return false;
				}
				clipper.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					List<Point64> list3 = list[i];
					if (Clipper.IsPositive(list3))
					{
						clipper.AddSubject(list3);
					}
					else
					{
						clipper.AddClip(list3);
					}
				}
				TileHandler.AddContours(clipper, ref contours);
				list.Clear();
				list2.Clear();
			}
			if (!clipper.Execute(ClipType.Difference, FillRule.NonZero, list, list2))
			{
				return false;
			}
			TileHandler.CopyClipperOutput(list, ref outputVertices, ref outputVertexCountPerPolygon);
			ListPool<List<Point64>>.Release(ref list);
			ListPool<List<Point64>>.Release(ref list2);
			return true;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0004413C File Offset: 0x0004233C
		internal unsafe static void InitDelegates()
		{
			if (TileHandler.DelegateGCRoot == null)
			{
				TileHandler.CutFunction cutFunction = new TileHandler.CutFunction(TileHandler.CutPolygon);
				TileHandler.DelegateGCRoot = cutFunction;
				*TileHandler.CutFunctionPtr.Data = Marshal.GetFunctionPointerForDelegate<TileHandler.CutFunction>(cutFunction);
			}
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00044174 File Offset: 0x00042374
		private static int ClipAgainstRectangle(UnsafeSpan<Int3> clipIn, UnsafeSpan<Int3> clipTmp, Vector2Int size)
		{
			Int3PolygonClipper int3PolygonClipper = default(Int3PolygonClipper);
			int num = int3PolygonClipper.ClipPolygon(clipIn, 3, clipTmp, 1, 0, 0);
			if (num == 0)
			{
				return num;
			}
			num = int3PolygonClipper.ClipPolygon(clipTmp, num, clipIn, -1, size.x, 0);
			if (num == 0)
			{
				return num;
			}
			num = int3PolygonClipper.ClipPolygon(clipIn, num, clipTmp, 1, 0, 2);
			if (num == 0)
			{
				return num;
			}
			return int3PolygonClipper.ClipPolygon(clipTmp, num, clipIn, -1, size.y, 2);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x000441E0 File Offset: 0x000423E0
		private unsafe static bool ClipAgainstHalfPlane(UnsafeSpan<TileHandler.Point64Wrapper> clipIn, NativeList<TileHandler.Point64Wrapper> clipOut, TileHandler.Point64Wrapper a, TileHandler.Point64Wrapper b)
		{
			if (clipIn.length == 0U)
			{
				return false;
			}
			bool result = false;
			long num = TileHandler.<ClipAgainstHalfPlane>g__SignedDistanceToHalfPlane|41_0(a, b, *clipIn[clipIn.length - 1U]);
			uint num2 = 0U;
			uint index = clipIn.length - 1U;
			while (num2 < clipIn.length)
			{
				long num3 = TileHandler.<ClipAgainstHalfPlane>g__SignedDistanceToHalfPlane|41_0(a, b, *clipIn[num2]);
				if (num > 0L != num3 > 0L)
				{
					double num4 = (double)num / (double)(num - num3);
					long x = clipIn[index].x + (long)math.round((double)(clipIn[num2].x - clipIn[index].x) * num4);
					long y = clipIn[index].y + (long)math.round((double)(clipIn[num2].y - clipIn[index].y) * num4);
					TileHandler.Point64Wrapper point64Wrapper = new TileHandler.Point64Wrapper(x, y);
					clipOut.Add(point64Wrapper);
				}
				if (num3 > 0L)
				{
					clipOut.Add(clipIn[num2]);
				}
				else
				{
					result = true;
				}
				num = num3;
				index = num2;
				num2 += 1U;
			}
			return result;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00044304 File Offset: 0x00042504
		private static void ClipAgainstHorizontalHalfPlane(ref UnsafeSpan<TileHandler.Point64Wrapper> contourVertices, NativeList<TileHandler.Point64Wrapper> scratchVertices, int h, Int3 a, Int3 b, Int3 c, bool preserveBelow)
		{
			Int3 @int = a;
			Int3 int2 = b;
			Int3 int3 = c;
			int i = 0;
			while (i < 3)
			{
				if ((int2.y < h && @int.y >= h && int3.y >= h) || (int2.y > h && @int.y <= h && int3.y <= h))
				{
					double num = (double)(h - int2.y) / (double)(@int.y - int2.y);
					double num2 = (double)(h - int2.y) / (double)(int3.y - int2.y);
					TileHandler.Point64Wrapper b2 = new TileHandler.Point64Wrapper((long)math.round(16.0 * ((double)int2.x + num * (double)(@int.x - int2.x))), (long)math.round(16.0 * ((double)int2.z + num * (double)(@int.z - int2.z))));
					TileHandler.Point64Wrapper a2 = new TileHandler.Point64Wrapper((long)math.round(16.0 * ((double)int2.x + num2 * (double)(int3.x - int2.x))), (long)math.round(16.0 * ((double)int2.z + num2 * (double)(int3.z - int2.z))));
					if (int2.y > h != preserveBelow)
					{
						Memory.Swap<TileHandler.Point64Wrapper>(ref b2, ref a2);
					}
					int length = scratchVertices.Length;
					if (TileHandler.ClipAgainstHalfPlane(contourVertices, scratchVertices, a2, b2))
					{
						contourVertices = scratchVertices.AsUnsafeSpan<TileHandler.Point64Wrapper>().Slice(length);
						uint length2 = contourVertices.length;
						return;
					}
					scratchVertices.Length = length;
					return;
				}
				else
				{
					Int3 int4 = @int;
					@int = int2;
					int2 = int3;
					int3 = int4;
					i++;
				}
			}
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x000444B0 File Offset: 0x000426B0
		private unsafe static int DelaunayRefinement(UnsafeSpan<Int3> verts, UnsafeSpan<int> tris, UnsafeSpan<int> tags, bool delaunay, bool colinear)
		{
			if (tris.Length % 3 != 0)
			{
				throw new ArgumentException("Triangle array length must be a multiple of 3");
			}
			if (tags.Length != tris.Length / 3)
			{
				throw new ArgumentException("There must be exactly 1 tag per 3 triangle indices");
			}
			NativeHashMap<Vector2Int, int> lookup = new NativeHashMap<Vector2Int, int>(tris.Length, Allocator.Temp);
			for (int i = 0; i < tris.Length; i += 3)
			{
				if (!VectorMath.IsClockwiseXZ(*verts[*tris[i]], *verts[*tris[i + 1]], *verts[*tris[i + 2]]))
				{
					int num = *tris[i];
					*tris[i] = *tris[i + 2];
					*tris[i + 2] = num;
				}
				TileHandler.<DelaunayRefinement>g__AddTriangleToLookup|43_0(lookup, tris, i);
			}
			int length = tris.Length;
			int j = 0;
			int num2 = 0;
			while (j < length)
			{
				int num3 = *tags[num2];
				for (int k = 0; k < 3; k++)
				{
					int num4 = *tris[j + k % 3];
					int num5 = *tris[j + (k + 1) % 3];
					int num6;
					if (lookup.TryGetValue(new Vector2Int(num5, num4), out num6))
					{
						int num7 = *tris[j + (k + 2) % 3];
						Int3 @int = *verts[num7];
						Int3 int2 = *verts[num5];
						Int3 int3 = *verts[num4];
						Int3 int4 = *verts[*tris[num6]];
						int num8 = *tags[num6 / 3];
						if (num3 == num8)
						{
							@int.y = 0;
							int2.y = 0;
							int3.y = 0;
							int4.y = 0;
							bool flag = false;
							if (!VectorMath.RightOrColinearXZ(@int, int3, int4) || VectorMath.RightXZ(@int, int2, int4))
							{
								if (!colinear)
								{
									goto IL_38F;
								}
								flag = true;
							}
							if (colinear && VectorMath.SqrDistancePointSegmentApproximate(@int, int4, int2) <= 9f && !lookup.ContainsKey(new Vector2Int(num7, num5)) && !lookup.ContainsKey(new Vector2Int(num5, *tris[num6])))
							{
								*tris[j + (k + 1) % 3] = *tris[num6];
								TileHandler.<DelaunayRefinement>g__RemoveTriangleWithVertex|43_1(num6, ref length, tris, tags, lookup);
								TileHandler.<DelaunayRefinement>g__AddTriangleToLookup|43_0(lookup, tris, j);
								k--;
							}
							else if (delaunay && !flag)
							{
								float num9 = Int3.Angle(int2 - @int, int3 - @int);
								if (Int3.Angle(int2 - int4, int3 - int4) > 6.2831855f - 2f * num9)
								{
									*tris[j + (k + 1) % 3] = *tris[num6];
									int num10 = num6 / 3 * 3;
									int num11 = num6 - num10;
									*tris[num10 + (num11 - 1 + 3) % 3] = *tris[j + (k + 2) % 3];
									TileHandler.<DelaunayRefinement>g__AddTriangleToLookup|43_0(lookup, tris, j);
									lookup[new Vector2Int(*tris[num10], *tris[num10 + 1])] = num10 + 2;
									lookup[new Vector2Int(*tris[num10 + 1], *tris[num10 + 2])] = num10;
									lookup[new Vector2Int(*tris[num10 + 2], *tris[num10])] = num10 + 1;
								}
							}
						}
					}
					IL_38F:;
				}
				j += 3;
				num2++;
			}
			return length;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0004495D File Offset: 0x00042B5D
		[CompilerGenerated]
		internal static long <ClipAgainstHalfPlane>g__SignedDistanceToHalfPlane|41_0(TileHandler.Point64Wrapper a, TileHandler.Point64Wrapper b, TileHandler.Point64Wrapper p)
		{
			return (b.x - a.x) * (p.y - a.y) - (p.x - a.x) * (b.y - a.y);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00044998 File Offset: 0x00042B98
		[CompilerGenerated]
		internal unsafe static void <DelaunayRefinement>g__AddTriangleToLookup|43_0(NativeHashMap<Vector2Int, int> lookup, UnsafeSpan<int> tris, int i)
		{
			lookup[new Vector2Int(*tris[i], *tris[i + 1])] = i + 2;
			lookup[new Vector2Int(*tris[i + 1], *tris[i + 2])] = i;
			lookup[new Vector2Int(*tris[i + 2], *tris[i])] = i + 1;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00044A10 File Offset: 0x00042C10
		[CompilerGenerated]
		internal unsafe static void <DelaunayRefinement>g__RemoveTriangleWithVertex|43_1(int vertexIndex, ref int tCount, UnsafeSpan<int> tris, UnsafeSpan<int> tags, NativeHashMap<Vector2Int, int> lookup)
		{
			tCount -= 3;
			int num = vertexIndex / 3 * 3;
			if (num != tCount)
			{
				*tris[num] = *tris[tCount];
				*tris[num + 1] = *tris[tCount + 1];
				*tris[num + 2] = *tris[tCount + 2];
				*tags[num / 3] = *tags[tCount / 3];
				lookup[new Vector2Int(*tris[num], *tris[num + 1])] = num + 2;
				lookup[new Vector2Int(*tris[num + 1], *tris[num + 2])] = num;
				lookup[new Vector2Int(*tris[num + 2], *tris[num])] = num + 1;
				*tris[tCount] = 0;
				*tris[tCount + 1] = 0;
				*tris[tCount + 2] = 0;
			}
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00044B18 File Offset: 0x00042D18
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void ConvertVerticesAndSnapToTileBoundaries$BurstManaged(ref UnsafeSpan<float2> contourVertices, out UnsafeList<TileHandler.Point64Wrapper> outputVertices, ref Vector2 tileSize)
		{
			outputVertices = new UnsafeList<TileHandler.Point64Wrapper>(contourVertices.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			outputVertices.Length = contourVertices.Length;
			UnsafeSpan<TileHandler.Point64Wrapper> unsafeSpan = outputVertices.AsUnsafeSpan<TileHandler.Point64Wrapper>();
			int2 @int = new int2(Mathf.RoundToInt(tileSize.x * 1000f), Mathf.RoundToInt(tileSize.y * 1000f));
			uint num = 0U;
			while ((ulong)num < (ulong)((long)contourVertices.Length))
			{
				Hint.Assume(num < contourVertices.length);
				int2 int2 = (int2)math.round(*contourVertices[num] * 1000f);
				int2 int3 = int2 % @int;
				if (int2.x < 0)
				{
					int3.x += @int.x;
				}
				if (int2.y < 0)
				{
					int3.y += @int.y;
				}
				int2 int4 = math.select(0, -int3, int3 <= 20);
				int4 += math.select(0, @int - int3, int3 >= @int - 20);
				int2 += int4;
				Hint.Assume(num < unsafeSpan.length);
				*unsafeSpan[num] = new TileHandler.Point64Wrapper((long)int2.x, (long)int2.y);
				num += 1U;
			}
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x00044C84 File Offset: 0x00042E84
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void CutTiles$BurstManaged(ref UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags, ref Vector2Int tileSize, ref TileHandler.CutCollection cutCollection, ref UnsafeSpan<TileMesh.TileMeshUnsafe> output, Allocator allocator)
		{
			UnsafeList<TileHandler.Point64Wrapper> contourVertices = cutCollection.contourVertices;
			NativeArray<IntBounds> arr = TileHandler.CalculateCutBounds(ref cutCollection, ref contourVertices);
			TileHandler.ScaleUpCoordinates(contourVertices.AsUnsafeSpan<TileHandler.Point64Wrapper>().Reinterpret<long>(16));
			int length = tileVertices.Length;
			NativeList<int> outputCutIndices = new NativeList<int>(4, Allocator.Temp);
			NativeArray<Int3> arr2 = new NativeArray<Int3>(7, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<Int3> arr3 = new NativeArray<Int3>(7, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<TileHandler.Point64Wrapper> arr4 = new NativeArray<TileHandler.Point64Wrapper>(16, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeList<UnsafeSpan<TileHandler.Point64Wrapper>> nativeList = new NativeList<UnsafeSpan<TileHandler.Point64Wrapper>>(Allocator.Temp);
			NativeList<UnsafeSpan<TileHandler.Point64Wrapper>> nativeList2 = new NativeList<UnsafeSpan<TileHandler.Point64Wrapper>>(Allocator.Temp);
			NativeList<TileHandler.Point64Wrapper> scratchVertices = new NativeList<TileHandler.Point64Wrapper>(32, Allocator.Temp);
			NativeList<int2> nativeList3 = new NativeList<int2>(Allocator.Temp);
			NativeList<int> nativeList4 = new NativeList<int>(Allocator.Temp);
			NativeHashMap<int2, int> nativeHashMap = new NativeHashMap<int2, int>(64, Allocator.Temp);
			OutputData<int2> outputData = new OutputData<int2>
			{
				Positions = new NativeList<int2>(Allocator.Temp),
				Triangles = new NativeList<int>(Allocator.Temp),
				Status = new NativeReference<Status>(Allocator.Temp, NativeArrayOptions.ClearMemory),
				Halfedges = new NativeList<int>(Allocator.Temp),
				ConstrainedHalfedges = new NativeList<HalfedgeState>(Allocator.Temp)
			};
			NativeList<Int3> tileOutputVertices = new NativeList<Int3>(Allocator.Temp);
			NativeList<int> tileOutputTriangles = new NativeList<int>(Allocator.Temp);
			NativeList<int> tileOutputTags = new NativeList<int>(Allocator.Temp);
			NativeList<Vector2Int> list = new NativeList<Vector2Int>(0, Allocator.Temp);
			NativeList<int> nativeList5 = new NativeList<int>(0, Allocator.Temp);
			for (int i = 0; i < length; i++)
			{
				UnsafeList<UnsafeSpan<Int3>> unsafeList = *tileVertices[i];
				UnsafeList<UnsafeSpan<int>> unsafeList2 = *tileTriangles[i];
				TileHandler.TileCuts tileCuts = cutCollection.tileCuts[i];
				if (tileCuts.contourStartIndex == tileCuts.contourEndIndex && unsafeList.Length == 1)
				{
					*output[i] = new TileMesh.TileMeshUnsafe
					{
						verticesInTileSpace = unsafeList[0].Clone(allocator),
						triangles = unsafeList2[0].Clone(allocator),
						tags = tileTags[i][0].Clone(allocator).Reinterpret<uint>()
					};
				}
				else
				{
					tileOutputVertices.Clear();
					tileOutputTriangles.Clear();
					tileOutputTags.Clear();
					UnsafeSpan<TileHandler.ContourMeta> unsafeSpan = cutCollection.contoursExtra.AsUnsafeSpan<TileHandler.ContourMeta>().Slice(tileCuts.contourStartIndex, tileCuts.contourEndIndex - tileCuts.contourStartIndex);
					UnsafeSpan<IntBounds> cutBounds = arr.AsUnsafeSpan<IntBounds>().Slice(tileCuts.contourStartIndex, tileCuts.contourEndIndex - tileCuts.contourStartIndex);
					if (cutBounds.Length > outputCutIndices.Capacity)
					{
						outputCutIndices.SetCapacity(cutBounds.Length);
					}
					for (int j = 0; j < unsafeList.Length; j++)
					{
						UnsafeSpan<Int3> unsafeSpan2 = unsafeList[j];
						UnsafeSpan<int3> unsafeSpan3 = unsafeList2[j].Reinterpret<int3>(4);
						UnsafeSpan<int> unsafeSpan4 = tileTags[i][j];
						for (int k = 0; k < unsafeSpan3.Length; k++)
						{
							int3 @int = *unsafeSpan3[k];
							Int3 int2 = *unsafeSpan2[@int.x];
							Int3 int3 = *unsafeSpan2[@int.y];
							Int3 int4 = *unsafeSpan2[@int.z];
							IntBounds intBounds = TileHandler.TriangleBounds(int2, int3, int4);
							intBounds.max.xz = intBounds.max.xz + 1;
							intBounds.min.xz = intBounds.min.xz - 1;
							TileHandler.CollectCutsTouchingBounds(cutBounds, outputCutIndices, intBounds);
							bool flag = j > 0;
							int tag = *unsafeSpan4[k];
							if (outputCutIndices.Length == 0 && !flag)
							{
								int length2 = tileOutputVertices.Length;
								tileOutputVertices.Capacity = math.max(tileOutputVertices.Capacity, tileOutputVertices.Length + 3);
								tileOutputTriangles.Capacity = math.max(tileOutputTriangles.Capacity, tileOutputTriangles.Length + 3);
								tileOutputVertices.AddNoResize(int2);
								tileOutputVertices.AddNoResize(int3);
								tileOutputVertices.AddNoResize(int4);
								for (int l = 0; l < 3; l++)
								{
									tileOutputTriangles.AddNoResize(length2 + l);
								}
								tileOutputTags.Add(tag);
							}
							else
							{
								UnsafeSpan<NavmeshCut.ContourBurst> unsafeSpan5 = cutCollection.contours.AsUnsafeSpan<NavmeshCut.ContourBurst>().Slice(tileCuts.contourStartIndex, tileCuts.contourEndIndex - tileCuts.contourStartIndex);
								scratchVertices.Clear();
								nativeList.Clear();
								nativeList2.Clear();
								int num;
								if (flag)
								{
									arr2[0] = int2;
									arr2[1] = int3;
									arr2[2] = int4;
									num = TileHandler.ClipAgainstRectangle(arr2.AsUnsafeSpan<Int3>(), arr3.AsUnsafeSpan<Int3>(), tileSize);
									for (int m = 0; m < num; m++)
									{
										arr4[m] = new TileHandler.Point64Wrapper((long)(arr2[m].x * 16), (long)(arr2[m].z * 16));
									}
									for (int n = 0; n < outputCutIndices.Length; n++)
									{
										int index = outputCutIndices[n];
										if (unsafeSpan[index].cutsAddedGeom)
										{
											NavmeshCut.ContourBurst contourBurst = *unsafeSpan5[index];
											UnsafeSpan<TileHandler.Point64Wrapper> unsafeSpan6 = contourVertices.AsUnsafeSpan<TileHandler.Point64Wrapper>().Slice(contourBurst.startIndex, contourBurst.endIndex - contourBurst.startIndex);
											if (unsafeSpan[index].isDual)
											{
												nativeList2.Add(unsafeSpan6);
											}
											else
											{
												nativeList.Add(unsafeSpan6);
											}
										}
									}
								}
								else
								{
									num = 3;
									arr4[0] = new TileHandler.Point64Wrapper((long)(int2.x * 16), (long)(int2.z * 16));
									arr4[1] = new TileHandler.Point64Wrapper((long)(int3.x * 16), (long)(int3.z * 16));
									arr4[2] = new TileHandler.Point64Wrapper((long)(int4.x * 16), (long)(int4.z * 16));
									bool flag2 = intBounds.min.y == intBounds.max.y - 1;
									for (int num2 = 0; num2 < outputCutIndices.Length; num2++)
									{
										IntBounds intBounds2 = *cutBounds[outputCutIndices[num2]];
										NavmeshCut.ContourBurst contourBurst2 = *unsafeSpan5[outputCutIndices[num2]];
										UnsafeSpan<TileHandler.Point64Wrapper> unsafeSpan7 = contourVertices.AsUnsafeSpan<TileHandler.Point64Wrapper>().Slice(contourBurst2.startIndex, contourBurst2.endIndex - contourBurst2.startIndex);
										if (!flag2)
										{
											int y = intBounds2.min.y;
											if (intBounds.min.y <= y && intBounds.max.y - 1 >= y)
											{
												if (y == intBounds.max.y - 1)
												{
													unsafeSpan7 = default(UnsafeSpan<TileHandler.Point64Wrapper>);
												}
												TileHandler.ClipAgainstHorizontalHalfPlane(ref unsafeSpan7, scratchVertices, y, int2, int3, int4, false);
											}
											int y2 = intBounds2.max.y;
											if (intBounds.min.y <= y2 && intBounds.max.y - 1 >= y2)
											{
												if (y2 == intBounds.min.y)
												{
													unsafeSpan7 = default(UnsafeSpan<TileHandler.Point64Wrapper>);
												}
												TileHandler.ClipAgainstHorizontalHalfPlane(ref unsafeSpan7, scratchVertices, y2, int2, int3, int4, true);
											}
										}
										if (unsafeSpan7.length > 0U)
										{
											(unsafeSpan[outputCutIndices[num2]].isDual ? nativeList2 : nativeList).Add(unsafeSpan7);
										}
									}
								}
								TileHandler.SnapEdges(ref arr4, ref num, nativeList.AsUnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>>(), tileSize);
								int num3 = 0;
								while (num3 < 2 && (num3 != 1 || nativeList2.Length != 0))
								{
									UnsafeSpan<TileHandler.Point64Wrapper> unsafeSpan8 = arr4.AsUnsafeReadOnlySpan<TileHandler.Point64Wrapper>().Slice(0, num);
									UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> unsafeSpan9 = nativeList.AsUnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>>();
									UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> unsafeSpan10 = nativeList2.AsUnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>>();
									contourVertices.AsUnsafeSpan<TileHandler.Point64Wrapper>();
									list.Clear();
									nativeList5.Clear();
									FunctionPointer<TileHandler.CutFunction> functionPointer = new FunctionPointer<TileHandler.CutFunction>(*TileHandler.CutFunctionPtr.Data);
									if (!functionPointer.Invoke(ref unsafeSpan8, ref unsafeSpan9, ref unsafeSpan10, UnsafeUtility.AsRef<UnsafeList<Vector2Int>>((void*)list.GetUnsafeList()), UnsafeUtility.AsRef<UnsafeList<int>>((void*)nativeList5.GetUnsafeList()), num3))
									{
										Debug.LogError("Error during cutting");
									}
									else if (nativeList5.Length != 0)
									{
										if (list.Length == 3 && nativeList5.Length == 1)
										{
											int length3 = tileOutputVertices.Length;
											tileOutputVertices.Capacity = math.max(tileOutputVertices.Capacity, tileOutputVertices.Length + 3);
											tileOutputTriangles.Capacity = math.max(tileOutputTriangles.Capacity, tileOutputTriangles.Length + 3);
											Polygon.BarycentricTriangleInterpolator barycentricTriangleInterpolator = new Polygon.BarycentricTriangleInterpolator(int2, int3, int4);
											for (int num4 = 0; num4 < 3; num4++)
											{
												Int3 int5 = new Int3(list[num4].x / 16, 0, list[num4].y / 16);
												int5.y = barycentricTriangleInterpolator.SampleY(new int2(int5.x, int5.z));
												tileOutputVertices.Add(int5);
											}
											int num5 = length3;
											tileOutputTriangles.Add(num5);
											num5 = length3 + 1;
											tileOutputTriangles.Add(num5);
											num5 = length3 + 2;
											tileOutputTriangles.Add(num5);
											tileOutputTags.Add(tag);
										}
										else
										{
											nativeList3.Clear();
											nativeList4.Clear();
											nativeHashMap.Clear();
											TileHandler.ScaleDownCoordinates(list.AsUnsafeSpan<Vector2Int>().Reinterpret<int>(8));
											int num6 = 0;
											for (int num7 = 0; num7 < nativeList5.Length; num7++)
											{
												int num8 = num6;
												num6 += nativeList5[num7];
												UnsafeSpan<int2> unsafeSpan11 = list.AsUnsafeSpan<Vector2Int>().Slice(num8, num6 - num8).Reinterpret<int2>();
												TileHandler.RemoveDegenerateSegments(ref unsafeSpan11);
												int num9 = -1;
												int length4 = nativeList4.Length;
												int num10 = 0;
												while ((long)num10 < (long)((ulong)unsafeSpan11.length))
												{
													int2 key = *unsafeSpan11[num10];
													int length5;
													if (!nativeHashMap.TryGetValue(key, out length5))
													{
														length5 = nativeList3.Length;
														nativeList3.Add(key);
														nativeHashMap.Add(key, length5);
													}
													if (num9 != -1)
													{
														nativeList4.Add(num9);
														nativeList4.Add(length5);
													}
													num9 = length5;
													num10++;
												}
												nativeList4.Add(num9);
												int num5 = nativeList4[length4];
												nativeList4.Add(num5);
											}
											InputData<int2> inputData = new InputData<int2>
											{
												Positions = nativeList3.AsArray(),
												ConstraintEdges = nativeList4.AsArray()
											};
											Args args;
											args..ctor(0, 1000000, true, false, false, false, false, 0f, 0f);
											Extensions.Triangulate(default(UnsafeTriangulator<int2>), inputData, outputData, args, Allocator.Temp);
											if (outputData.Status.Value.IsError)
											{
												Debug.LogError("Error during triangulation");
											}
											else
											{
												TileHandler.CopyTriangulationToOutput(ref outputData, tileOutputVertices, tileOutputTriangles, tileOutputTags, tag, int2, int3, int4);
											}
										}
									}
									num3++;
								}
							}
						}
					}
					*output[i] = TileHandler.CompressAndRefineTile(tileOutputVertices, tileOutputTriangles, tileOutputTags, allocator);
				}
			}
			tileOutputVertices.Dispose();
			tileOutputTriangles.Dispose();
			tileOutputTags.Dispose();
		}

		// Token: 0x0400080E RID: 2062
		private static readonly ProfilerMarker MarkerTriangulate = new ProfilerMarker("Triangulate");

		// Token: 0x0400080F RID: 2063
		private static readonly ProfilerMarker MarkerClipping = new ProfilerMarker("Clipping");

		// Token: 0x04000810 RID: 2064
		private static readonly ProfilerMarker MarkerPrepare = new ProfilerMarker("Prepare");

		// Token: 0x04000811 RID: 2065
		private static readonly ProfilerMarker MarkerAllocate = new ProfilerMarker("Allocate");

		// Token: 0x04000812 RID: 2066
		private static readonly ProfilerMarker MarkerCore = new ProfilerMarker("Core");

		// Token: 0x04000813 RID: 2067
		private static readonly ProfilerMarker MarkerCompress = new ProfilerMarker("Compress");

		// Token: 0x04000814 RID: 2068
		private static readonly ProfilerMarker MarkerRemoveDegenerateTriangles = new ProfilerMarker("Remove Degenerate Tris");

		// Token: 0x04000815 RID: 2069
		private static readonly ProfilerMarker MarkerRefine = new ProfilerMarker("Refine");

		// Token: 0x04000816 RID: 2070
		private static readonly ProfilerMarker MarkerEdgeSnapping = new ProfilerMarker("EdgeSnapping");

		// Token: 0x04000817 RID: 2071
		private static readonly ProfilerMarker MarkerRemoveDegenerateLines = new ProfilerMarker("Remove Degenerate Lines");

		// Token: 0x04000818 RID: 2072
		private static readonly ProfilerMarker MarkerClipHorizontal = new ProfilerMarker("ClipHorizontal");

		// Token: 0x04000819 RID: 2073
		private static readonly ProfilerMarker MarkerCopyClippingResult = new ProfilerMarker("CopyClippingResult");

		// Token: 0x0400081A RID: 2074
		private static readonly ProfilerMarker CopyTriangulationToOutputMarker = new ProfilerMarker("Copy to output");

		// Token: 0x0400081B RID: 2075
		private static readonly SharedStatic<IntPtr> CutFunctionPtr = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -3121683876094195160L, 0L);

		// Token: 0x0400081C RID: 2076
		private static TileHandler.CutFunction DelegateGCRoot;

		// Token: 0x0400081D RID: 2077
		private const int EdgeSnappingMaxDistance = 1;

		// Token: 0x0400081E RID: 2078
		private const int Scale = 16;

		// Token: 0x0400081F RID: 2079
		public const int TileSnappingMaxDistance = 20;

		// Token: 0x020001B4 RID: 436
		// (Invoke) Token: 0x06000BCA RID: 3018
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate bool CutFunction(ref UnsafeSpan<TileHandler.Point64Wrapper> subject, ref UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> contours, ref UnsafeSpan<UnsafeSpan<TileHandler.Point64Wrapper>> contoursDual, ref UnsafeList<Vector2Int> outputVertices, ref UnsafeList<int> outputVertexCountPerPolygon, int dual);

		// Token: 0x020001B5 RID: 437
		private struct CutFunctionKey
		{
		}

		// Token: 0x020001B6 RID: 438
		internal struct TileCuts
		{
			// Token: 0x04000820 RID: 2080
			public int contourStartIndex;

			// Token: 0x04000821 RID: 2081
			public int contourEndIndex;
		}

		// Token: 0x020001B7 RID: 439
		internal struct ContourMeta
		{
			// Token: 0x04000822 RID: 2082
			public bool isDual;

			// Token: 0x04000823 RID: 2083
			public bool cutsAddedGeom;
		}

		// Token: 0x020001B8 RID: 440
		internal struct CutCollection : IDisposable
		{
			// Token: 0x06000BCD RID: 3021 RVA: 0x000457F9 File Offset: 0x000439F9
			public void Dispose()
			{
				this.contourVertices.Dispose();
				this.contours.Dispose();
				this.contoursExtra.Dispose();
				this.tileCuts.Dispose();
			}

			// Token: 0x04000824 RID: 2084
			public UnsafeList<TileHandler.Point64Wrapper> contourVertices;

			// Token: 0x04000825 RID: 2085
			public UnsafeList<NavmeshCut.ContourBurst> contours;

			// Token: 0x04000826 RID: 2086
			public UnsafeList<TileHandler.ContourMeta> contoursExtra;

			// Token: 0x04000827 RID: 2087
			public UnsafeList<TileHandler.TileCuts> tileCuts;

			// Token: 0x04000828 RID: 2088
			[MarshalAs(UnmanagedType.U1)]
			public bool cuttingRequired;
		}

		// Token: 0x020001B9 RID: 441
		public struct Point64Wrapper
		{
			// Token: 0x06000BCE RID: 3022 RVA: 0x00045827 File Offset: 0x00043A27
			public Point64Wrapper(long x, long y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x04000829 RID: 2089
			public long x;

			// Token: 0x0400082A RID: 2090
			public long y;
		}

		// Token: 0x020001BA RID: 442
		// (Invoke) Token: 0x06000BD0 RID: 3024
		internal delegate void ConvertVerticesAndSnapToTileBoundaries_00000AD4$PostfixBurstDelegate(ref UnsafeSpan<float2> contourVertices, out UnsafeList<TileHandler.Point64Wrapper> outputVertices, ref Vector2 tileSize);

		// Token: 0x020001BB RID: 443
		internal static class ConvertVerticesAndSnapToTileBoundaries_00000AD4$BurstDirectCall
		{
			// Token: 0x06000BD3 RID: 3027 RVA: 0x00045837 File Offset: 0x00043A37
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000AD4$BurstDirectCall.Pointer == 0)
				{
					TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000AD4$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000AD4$BurstDirectCall.DeferredCompilation, methodof(TileHandler.ConvertVerticesAndSnapToTileBoundaries$BurstManaged(UnsafeSpan<float2>*, UnsafeList<TileHandler.Point64Wrapper>*, Vector2*)).MethodHandle, typeof(TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000AD4$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000AD4$BurstDirectCall.Pointer;
			}

			// Token: 0x06000BD4 RID: 3028 RVA: 0x00045864 File Offset: 0x00043A64
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000AD4$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000BD5 RID: 3029 RVA: 0x0004587C File Offset: 0x00043A7C
			public unsafe static void Constructor()
			{
				TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000AD4$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(TileHandler.ConvertVerticesAndSnapToTileBoundaries(UnsafeSpan<float2>*, UnsafeList<TileHandler.Point64Wrapper>*, Vector2*)).MethodHandle);
			}

			// Token: 0x06000BD6 RID: 3030 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000BD7 RID: 3031 RVA: 0x0004588D File Offset: 0x00043A8D
			// Note: this type is marked as 'beforefieldinit'.
			static ConvertVerticesAndSnapToTileBoundaries_00000AD4$BurstDirectCall()
			{
				TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000AD4$BurstDirectCall.Constructor();
			}

			// Token: 0x06000BD8 RID: 3032 RVA: 0x00045894 File Offset: 0x00043A94
			public static void Invoke(ref UnsafeSpan<float2> contourVertices, out UnsafeList<TileHandler.Point64Wrapper> outputVertices, ref Vector2 tileSize)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000AD4$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<Unity.Mathematics.float2>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Graphs.Navmesh.TileHandler/Point64Wrapper>&,UnityEngine.Vector2&), ref contourVertices, ref outputVertices, ref tileSize, functionPointer);
						return;
					}
				}
				TileHandler.ConvertVerticesAndSnapToTileBoundaries$BurstManaged(ref contourVertices, out outputVertices, ref tileSize);
			}

			// Token: 0x0400082B RID: 2091
			private static IntPtr Pointer;

			// Token: 0x0400082C RID: 2092
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001BC RID: 444
		// (Invoke) Token: 0x06000BDA RID: 3034
		internal delegate void CutTiles_00000AD5$PostfixBurstDelegate(ref UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags, ref Vector2Int tileSize, ref TileHandler.CutCollection cutCollection, ref UnsafeSpan<TileMesh.TileMeshUnsafe> output, Allocator allocator);

		// Token: 0x020001BD RID: 445
		internal static class CutTiles_00000AD5$BurstDirectCall
		{
			// Token: 0x06000BDD RID: 3037 RVA: 0x000458C9 File Offset: 0x00043AC9
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (TileHandler.CutTiles_00000AD5$BurstDirectCall.Pointer == 0)
				{
					TileHandler.CutTiles_00000AD5$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(TileHandler.CutTiles_00000AD5$BurstDirectCall.DeferredCompilation, methodof(TileHandler.CutTiles$BurstManaged(UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>>*, UnsafeSpan<UnsafeList<UnsafeSpan<int>>>*, UnsafeSpan<UnsafeList<UnsafeSpan<int>>>*, Vector2Int*, TileHandler.CutCollection*, UnsafeSpan<TileMesh.TileMeshUnsafe>*, Allocator)).MethodHandle, typeof(TileHandler.CutTiles_00000AD5$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = TileHandler.CutTiles_00000AD5$BurstDirectCall.Pointer;
			}

			// Token: 0x06000BDE RID: 3038 RVA: 0x000458F8 File Offset: 0x00043AF8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				TileHandler.CutTiles_00000AD5$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000BDF RID: 3039 RVA: 0x00045910 File Offset: 0x00043B10
			public unsafe static void Constructor()
			{
				TileHandler.CutTiles_00000AD5$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(TileHandler.CutTiles(UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>>*, UnsafeSpan<UnsafeList<UnsafeSpan<int>>>*, UnsafeSpan<UnsafeList<UnsafeSpan<int>>>*, Vector2Int*, TileHandler.CutCollection*, UnsafeSpan<TileMesh.TileMeshUnsafe>*, Allocator)).MethodHandle);
			}

			// Token: 0x06000BE0 RID: 3040 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000BE1 RID: 3041 RVA: 0x00045921 File Offset: 0x00043B21
			// Note: this type is marked as 'beforefieldinit'.
			static CutTiles_00000AD5$BurstDirectCall()
			{
				TileHandler.CutTiles_00000AD5$BurstDirectCall.Constructor();
			}

			// Token: 0x06000BE2 RID: 3042 RVA: 0x00045928 File Offset: 0x00043B28
			public static void Invoke(ref UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles, ref UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags, ref Vector2Int tileSize, ref TileHandler.CutCollection cutCollection, ref UnsafeSpan<TileMesh.TileMeshUnsafe> output, Allocator allocator)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = TileHandler.CutTiles_00000AD5$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Int3>>>&,Pathfinding.Collections.UnsafeSpan`1<Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Collections.UnsafeSpan`1<System.Int32>>>&,Pathfinding.Collections.UnsafeSpan`1<Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Pathfinding.Collections.UnsafeSpan`1<System.Int32>>>&,UnityEngine.Vector2Int&,Pathfinding.Graphs.Navmesh.TileHandler/CutCollection&,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Graphs.Navmesh.TileMesh/TileMeshUnsafe>&,Unity.Collections.Allocator), ref tileVertices, ref tileTriangles, ref tileTags, ref tileSize, ref cutCollection, ref output, allocator, functionPointer);
						return;
					}
				}
				TileHandler.CutTiles$BurstManaged(ref tileVertices, ref tileTriangles, ref tileTags, ref tileSize, ref cutCollection, ref output, allocator);
			}

			// Token: 0x0400082D RID: 2093
			private static IntPtr Pointer;

			// Token: 0x0400082E RID: 2094
			private static IntPtr DeferredCompilation;
		}
	}
}
