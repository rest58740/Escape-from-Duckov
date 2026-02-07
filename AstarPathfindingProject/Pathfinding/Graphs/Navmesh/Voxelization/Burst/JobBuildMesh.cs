using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001D5 RID: 469
	[BurstCompile]
	public struct JobBuildMesh : IJob
	{
		// Token: 0x06000C49 RID: 3145 RVA: 0x00049644 File Offset: 0x00047844
		private static bool Diagonal(int i, int j, int n, NativeArray<int> verts, NativeArray<int> indices)
		{
			return JobBuildMesh.InCone(i, j, n, verts, indices) && JobBuildMesh.Diagonalie(i, j, n, verts, indices);
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x00049660 File Offset: 0x00047860
		private static bool InCone(int i, int j, int n, NativeArray<int> verts, NativeArray<int> indices)
		{
			int num = (indices[i] & 268435455) * 3;
			int num2 = (indices[j] & 268435455) * 3;
			int c = (indices[JobBuildMesh.Next(i, n)] & 268435455) * 3;
			int num3 = (indices[JobBuildMesh.Prev(i, n)] & 268435455) * 3;
			if (JobBuildMesh.LeftOn(num3, num, c, verts))
			{
				return JobBuildMesh.Left(num, num2, num3, verts) && JobBuildMesh.Left(num2, num, c, verts);
			}
			return !JobBuildMesh.LeftOn(num, num2, c, verts) || !JobBuildMesh.LeftOn(num2, num, num3, verts);
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x000496F8 File Offset: 0x000478F8
		private static bool Left(int a, int b, int c, NativeArray<int> verts)
		{
			return JobBuildMesh.Area2(a, b, c, verts) < 0;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00049706 File Offset: 0x00047906
		private static bool LeftOn(int a, int b, int c, NativeArray<int> verts)
		{
			return JobBuildMesh.Area2(a, b, c, verts) <= 0;
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00049717 File Offset: 0x00047917
		private static bool Collinear(int a, int b, int c, NativeArray<int> verts)
		{
			return JobBuildMesh.Area2(a, b, c, verts) == 0;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00049728 File Offset: 0x00047928
		public static int Area2(int a, int b, int c, NativeArray<int> verts)
		{
			return (verts[b] - verts[a]) * (verts[c + 2] - verts[a + 2]) - (verts[c] - verts[a]) * (verts[b + 2] - verts[a + 2]);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00049784 File Offset: 0x00047984
		private static bool Diagonalie(int i, int j, int n, NativeArray<int> verts, NativeArray<int> indices)
		{
			int a = (indices[i] & 268435455) * 3;
			int num = (indices[j] & 268435455) * 3;
			for (int k = 0; k < n; k++)
			{
				int num2 = JobBuildMesh.Next(k, n);
				if (k != i && num2 != i && k != j && num2 != j)
				{
					int num3 = (indices[k] & 268435455) * 3;
					int num4 = (indices[num2] & 268435455) * 3;
					if (!JobBuildMesh.Vequal(a, num3, verts) && !JobBuildMesh.Vequal(num, num3, verts) && !JobBuildMesh.Vequal(a, num4, verts) && !JobBuildMesh.Vequal(num, num4, verts) && JobBuildMesh.Intersect(a, num, num3, num4, verts))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00049838 File Offset: 0x00047A38
		private static bool Xorb(bool x, bool y)
		{
			return !x ^ !y;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00049844 File Offset: 0x00047A44
		private static bool IntersectProp(int a, int b, int c, int d, NativeArray<int> verts)
		{
			return !JobBuildMesh.Collinear(a, b, c, verts) && !JobBuildMesh.Collinear(a, b, d, verts) && !JobBuildMesh.Collinear(c, d, a, verts) && !JobBuildMesh.Collinear(c, d, b, verts) && JobBuildMesh.Xorb(JobBuildMesh.Left(a, b, c, verts), JobBuildMesh.Left(a, b, d, verts)) && JobBuildMesh.Xorb(JobBuildMesh.Left(c, d, a, verts), JobBuildMesh.Left(c, d, b, verts));
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x000498BC File Offset: 0x00047ABC
		private static bool Between(int a, int b, int c, NativeArray<int> verts)
		{
			if (!JobBuildMesh.Collinear(a, b, c, verts))
			{
				return false;
			}
			if (verts[a] != verts[b])
			{
				return (verts[a] <= verts[c] && verts[c] <= verts[b]) || (verts[a] >= verts[c] && verts[c] >= verts[b]);
			}
			return (verts[a + 2] <= verts[c + 2] && verts[c + 2] <= verts[b + 2]) || (verts[a + 2] >= verts[c + 2] && verts[c + 2] >= verts[b + 2]);
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00049998 File Offset: 0x00047B98
		private static bool Intersect(int a, int b, int c, int d, NativeArray<int> verts)
		{
			return JobBuildMesh.IntersectProp(a, b, c, d, verts) || (JobBuildMesh.Between(a, b, c, verts) || JobBuildMesh.Between(a, b, d, verts) || JobBuildMesh.Between(c, d, a, verts) || JobBuildMesh.Between(c, d, b, verts));
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x000499E7 File Offset: 0x00047BE7
		private static bool Vequal(int a, int b, NativeArray<int> verts)
		{
			return verts[a] == verts[b] && verts[a + 2] == verts[b + 2];
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00049A13 File Offset: 0x00047C13
		private static int Prev(int i, int n)
		{
			if (i - 1 < 0)
			{
				return n - 1;
			}
			return i - 1;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00049A22 File Offset: 0x00047C22
		private static int Next(int i, int n)
		{
			if (i + 1 >= n)
			{
				return 0;
			}
			return i + 1;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00049A30 File Offset: 0x00047C30
		private static int AddVertex(NativeList<Int3> vertices, NativeHashMap<Int3, int> vertexMap, Int3 vertex)
		{
			int result;
			if (vertexMap.TryGetValue(vertex, out result))
			{
				return result;
			}
			vertices.AddNoResize(vertex);
			vertexMap.Add(vertex, vertices.Length - 1);
			return vertices.Length - 1;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00049A70 File Offset: 0x00047C70
		public void Execute()
		{
			int num = 3;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < this.contours.Length; i++)
			{
				if (this.contours[i].nverts >= 3)
				{
					num2 += this.contours[i].nverts;
					num3 += this.contours[i].nverts - 2;
					num4 = Math.Max(num4, this.contours[i].nverts);
				}
			}
			this.mesh.verts.Clear();
			if (num2 > this.mesh.verts.Capacity)
			{
				this.mesh.verts.SetCapacity(num2);
			}
			this.mesh.tris.ResizeUninitialized(num3 * num);
			this.mesh.areas.ResizeUninitialized(num3);
			NativeList<Int3> verts = this.mesh.verts;
			NativeList<int> tris = this.mesh.tris;
			NativeList<int> areas = this.mesh.areas;
			NativeArray<int> indices = new NativeArray<int>(num4, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<int> tris2 = new NativeArray<int>(num4 * 3, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<bool> verticesToRemove = new NativeArray<bool>(num2, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeHashMap<Int3, int> vertexMap = new NativeHashMap<Int3, int>(num2, Allocator.Temp);
			int num5 = 0;
			int num6 = 0;
			for (int j = 0; j < this.contours.Length; j++)
			{
				VoxelContour voxelContour = this.contours[j];
				if (voxelContour.nverts >= 3)
				{
					for (int k = 0; k < voxelContour.nverts; k++)
					{
						ref NativeList<int> ptr = ref this.contourVertices;
						int index = voxelContour.vertexStartIndex + k * 4 + 2;
						ptr[index] /= this.field.width;
					}
					for (int l = 0; l < voxelContour.nverts; l++)
					{
						int num7 = this.contourVertices[voxelContour.vertexStartIndex + l * 4 + 3];
						int num8 = JobBuildMesh.AddVertex(verts, vertexMap, new Int3(this.contourVertices[voxelContour.vertexStartIndex + l * 4], this.contourVertices[voxelContour.vertexStartIndex + l * 4 + 1], this.contourVertices[voxelContour.vertexStartIndex + l * 4 + 2]));
						indices[l] = num8;
						verticesToRemove[num8] = ((num7 & 65536) != 0);
					}
					int num9 = JobBuildMesh.Triangulate(voxelContour.nverts, verts.AsArray().Reinterpret<int>(12), indices, tris2);
					if (num9 < 0)
					{
						num9 = -num9;
					}
					for (int m = 0; m < num9 * 3; m++)
					{
						tris[num5] = tris2[m];
						num5++;
					}
					for (int n = 0; n < num9; n++)
					{
						areas[num6] = voxelContour.area;
						num6++;
					}
				}
			}
			this.mesh.tris.ResizeUninitialized(num5);
			this.mesh.areas.ResizeUninitialized(num6);
			this.RemoveTileBorderVertices(ref this.mesh, verticesToRemove);
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00049D98 File Offset: 0x00047F98
		private void RemoveTileBorderVertices(ref VoxelMesh mesh, NativeArray<bool> verticesToRemove)
		{
			NativeArray<byte> arr = new NativeArray<byte>(mesh.verts.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = mesh.verts.Length - 1; i >= 0; i--)
			{
				if (verticesToRemove[i] && this.CanRemoveVertex(ref mesh, i, arr.AsUnsafeSpan<byte>()))
				{
					this.RemoveVertex(ref mesh, i);
				}
			}
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00049DF4 File Offset: 0x00047FF4
		private unsafe bool CanRemoveVertex(ref VoxelMesh mesh, int vertexToRemove, UnsafeSpan<byte> vertexScratch)
		{
			int num = 0;
			for (int i = 0; i < mesh.tris.Length; i += 3)
			{
				int num2 = 0;
				for (int j = 0; j < 3; j++)
				{
					if (mesh.tris[i + j] == vertexToRemove)
					{
						num2++;
					}
				}
				if (num2 > 0)
				{
					if (num2 > 1)
					{
						throw new Exception("Degenerate triangle. This should have already been removed.");
					}
					num++;
				}
			}
			if (num <= 2)
			{
				return false;
			}
			vertexScratch.FillZeros<byte>();
			for (int k = 0; k < mesh.tris.Length; k += 3)
			{
				int l = 0;
				int num3 = 2;
				while (l < 3)
				{
					if (mesh.tris[k + l] == vertexToRemove || mesh.tris[k + num3] == vertexToRemove)
					{
						int num4 = mesh.tris[k + l];
						int num5 = mesh.tris[k + num3];
						ref byte ptr = ref vertexScratch[(num5 == vertexToRemove) ? num4 : num5];
						ptr += 1;
					}
					num3 = l++;
				}
			}
			int num6 = 0;
			int num7 = 0;
			for (int m = 0; m < vertexScratch.Length; m++)
			{
				if (*vertexScratch[m] == 1)
				{
					num6++;
				}
				else if (*vertexScratch[m] > 2)
				{
					num7++;
				}
			}
			if (num7 > 0)
			{
				Debug.LogError("Vertex has multiple shared edges. This should not happen. Navmesh must be corrupt. Trying to not make it worse.");
				return false;
			}
			return num6 <= 2;
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00049F54 File Offset: 0x00048154
		private void RemoveVertex(ref VoxelMesh mesh, int vertexToRemove)
		{
			NativeList<int> nativeList = new NativeList<int>(16, Allocator.Temp);
			int num = -1;
			int num3;
			for (int i = 0; i < mesh.tris.Length; i += 3)
			{
				int num2 = -1;
				for (int j = 0; j < 3; j++)
				{
					if (mesh.tris[i + j] == vertexToRemove)
					{
						num2 = j;
						break;
					}
				}
				if (num2 != -1)
				{
					num = mesh.areas[i / 3];
					num3 = mesh.tris[i + (num2 + 1) % 3];
					nativeList.Add(num3);
					num3 = mesh.tris[i + (num2 + 2) % 3];
					nativeList.Add(num3);
					mesh.tris[i] = mesh.tris[mesh.tris.Length - 3];
					mesh.tris[i + 1] = mesh.tris[mesh.tris.Length - 3 + 1];
					mesh.tris[i + 2] = mesh.tris[mesh.tris.Length - 3 + 2];
					mesh.tris.Length = mesh.tris.Length - 3;
					mesh.areas.RemoveAtSwapBack(i / 3);
					i -= 3;
				}
			}
			NativeList<int> nativeList2 = new NativeList<int>(nativeList.Length / 2 + 1, Allocator.Temp);
			num3 = nativeList[nativeList.Length - 2];
			nativeList2.Add(num3);
			num3 = nativeList[nativeList.Length - 1];
			nativeList2.Add(num3);
			nativeList.Length -= 2;
			while (nativeList.Length > 0)
			{
				for (int k = nativeList.Length - 2; k >= 0; k -= 2)
				{
					int num4 = nativeList[k];
					int num5 = nativeList[k + 1];
					bool flag = false;
					if (nativeList2[0] == num5)
					{
						nativeList2.InsertRange(0, 1);
						nativeList2[0] = num4;
						flag = true;
					}
					if (nativeList2[nativeList2.Length - 1] == num4)
					{
						nativeList2.AddNoResize(num5);
						flag = true;
					}
					if (flag)
					{
						nativeList[k] = nativeList[nativeList.Length - 2];
						nativeList[k + 1] = nativeList[nativeList.Length - 1];
						nativeList.Length -= 2;
					}
				}
			}
			mesh.verts.RemoveAt(vertexToRemove);
			for (int l = 0; l < mesh.tris.Length; l++)
			{
				if (mesh.tris[l] > vertexToRemove)
				{
					num3 = l;
					int num6 = mesh.tris[num3];
					mesh.tris[num3] = num6 - 1;
				}
			}
			for (int m = 0; m < nativeList2.Length; m++)
			{
				if (nativeList2[m] > vertexToRemove)
				{
					int num6 = m;
					num3 = nativeList2[num6];
					nativeList2[num6] = num3 - 1;
				}
			}
			int num7 = (nativeList2.Length - 2) * 3;
			int length = mesh.tris.Length;
			mesh.tris.Length = mesh.tris.Length + num7;
			int num8 = JobBuildMesh.Triangulate(nativeList2.Length, mesh.verts.AsArray().Reinterpret<int>(12), nativeList2.AsArray(), mesh.tris.AsArray().GetSubArray(length, num7));
			if (num8 < 0)
			{
				num8 = -num8;
			}
			mesh.tris.ResizeUninitialized(length + num8 * 3);
			mesh.areas.AddReplicate(num, num8);
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0004A310 File Offset: 0x00048510
		private static int Triangulate(int n, NativeArray<int> verts, NativeArray<int> indices, NativeArray<int> tris)
		{
			int num = 0;
			NativeArray<int> nativeArray = tris;
			int num2 = 0;
			for (int i = 0; i < n; i++)
			{
				int num3 = JobBuildMesh.Next(i, n);
				int j = JobBuildMesh.Next(num3, n);
				if (JobBuildMesh.Diagonal(i, j, n, verts, indices))
				{
					ref NativeArray<int> ptr = ref indices;
					int index = num3;
					ptr[index] |= 1073741824;
				}
			}
			while (n > 3)
			{
				int num4 = int.MaxValue;
				int num5 = -1;
				for (int k = 0; k < n; k++)
				{
					int num6 = JobBuildMesh.Next(k, n);
					if ((indices[num6] & 1073741824) != 0)
					{
						int num7 = (indices[k] & 268435455) * 3;
						int num8 = (indices[JobBuildMesh.Next(num6, n)] & 268435455) * 3;
						int num9 = verts[num8] - verts[num7];
						int num10 = verts[num8 + 2] - verts[num7 + 2];
						int num11 = num9 * num9 + num10 * num10;
						if (num11 < num4)
						{
							num4 = num11;
							num5 = k;
						}
					}
				}
				if (num5 == -1)
				{
					Debug.LogWarning("Degenerate triangles might have been generated.\nUsually this is not a problem, but if you have a static level, try to modify the graph settings slightly to avoid this edge case.");
					return -num;
				}
				int num12 = num5;
				int num13 = JobBuildMesh.Next(num12, n);
				int index2 = JobBuildMesh.Next(num13, n);
				nativeArray[num2] = (indices[num12] & 268435455);
				num2++;
				nativeArray[num2] = (indices[num13] & 268435455);
				num2++;
				nativeArray[num2] = (indices[index2] & 268435455);
				num2++;
				num++;
				n--;
				for (int l = num13; l < n; l++)
				{
					indices[l] = indices[l + 1];
				}
				if (num13 >= n)
				{
					num13 = 0;
				}
				num12 = JobBuildMesh.Prev(num13, n);
				if (JobBuildMesh.Diagonal(JobBuildMesh.Prev(num12, n), num13, n, verts, indices))
				{
					ref NativeArray<int> ptr = ref indices;
					int index = num12;
					ptr[index] |= 1073741824;
				}
				else
				{
					ref NativeArray<int> ptr = ref indices;
					int index = num12;
					ptr[index] &= 268435455;
				}
				if (JobBuildMesh.Diagonal(num12, JobBuildMesh.Next(num13, n), n, verts, indices))
				{
					ref NativeArray<int> ptr = ref indices;
					int index = num13;
					ptr[index] |= 1073741824;
				}
				else
				{
					ref NativeArray<int> ptr = ref indices;
					int index = num13;
					ptr[index] &= 268435455;
				}
			}
			nativeArray[num2] = (indices[0] & 268435455);
			num2++;
			nativeArray[num2] = (indices[1] & 268435455);
			num2++;
			nativeArray[num2] = (indices[2] & 268435455);
			num2++;
			return num + 1;
		}

		// Token: 0x04000887 RID: 2183
		public NativeList<int> contourVertices;

		// Token: 0x04000888 RID: 2184
		public NativeList<VoxelContour> contours;

		// Token: 0x04000889 RID: 2185
		public VoxelMesh mesh;

		// Token: 0x0400088A RID: 2186
		public CompactVoxelField field;
	}
}
