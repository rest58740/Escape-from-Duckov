using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000020 RID: 32
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ClipperEngine
	{
		// Token: 0x060000CB RID: 203 RVA: 0x00005974 File Offset: 0x00003B74
		internal static void AddLocMin(Vertex vert, PathType polytype, bool isOpen, List<LocalMinima> minimaList)
		{
			if ((vert.flags & VertexFlags.LocalMin) != VertexFlags.None)
			{
				return;
			}
			vert.flags |= VertexFlags.LocalMin;
			LocalMinima item = new LocalMinima(vert, polytype, isOpen);
			minimaList.Add(item);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000059AB File Offset: 0x00003BAB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void EnsureCapacity<[Nullable(2)] T>(this List<T> list, int minCapacity)
		{
			if (list.Capacity < minCapacity)
			{
				list.Capacity = minCapacity;
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000059C0 File Offset: 0x00003BC0
		internal static void AddPathsToVertexList(List<List<Point64>> paths, PathType polytype, bool isOpen, List<LocalMinima> minimaList, List<Vertex> vertexList, VertexPool vertexPool)
		{
			int num = 0;
			foreach (List<Point64> list in paths)
			{
				num += list.Count;
			}
			vertexList.EnsureCapacity(vertexList.Count + num);
			foreach (List<Point64> list2 in paths)
			{
				Vertex vertex = null;
				Vertex vertex2 = null;
				foreach (Point64 point in list2)
				{
					if (vertex == null)
					{
						vertex = vertexPool.GetNew(point, VertexFlags.None, null);
						vertexList.Add(vertex);
						vertex2 = vertex;
					}
					else if (vertex2.pt != point)
					{
						Vertex vertex3 = vertexPool.GetNew(point, VertexFlags.None, vertex2);
						vertexList.Add(vertex3);
						vertex2.next = vertex3;
						vertex2 = vertex3;
					}
				}
				if (vertex2 != null && vertex2.prev != null)
				{
					if (!isOpen && vertex2.pt == vertex.pt)
					{
						vertex2 = vertex2.prev;
					}
					vertex2.next = vertex;
					vertex.prev = vertex2;
					if (isOpen || vertex2.next != vertex2)
					{
						bool flag;
						if (isOpen)
						{
							Vertex vertex3 = vertex.next;
							while (vertex3 != vertex && vertex3.pt.Y == vertex.pt.Y)
							{
								vertex3 = vertex3.next;
							}
							flag = (vertex3.pt.Y <= vertex.pt.Y);
							if (flag)
							{
								vertex.flags = VertexFlags.OpenStart;
								ClipperEngine.AddLocMin(vertex, polytype, true, minimaList);
							}
							else
							{
								vertex.flags = (VertexFlags.OpenStart | VertexFlags.LocalMax);
							}
						}
						else
						{
							vertex2 = vertex.prev;
							while (vertex2 != vertex && vertex2.pt.Y == vertex.pt.Y)
							{
								vertex2 = vertex2.prev;
							}
							if (vertex2 == vertex)
							{
								continue;
							}
							flag = (vertex2.pt.Y > vertex.pt.Y);
						}
						bool flag2 = flag;
						vertex2 = vertex;
						for (Vertex vertex3 = vertex.next; vertex3 != vertex; vertex3 = vertex3.next)
						{
							if (vertex3.pt.Y > vertex2.pt.Y && flag)
							{
								vertex2.flags |= VertexFlags.LocalMax;
								flag = false;
							}
							else if (vertex3.pt.Y < vertex2.pt.Y && !flag)
							{
								flag = true;
								ClipperEngine.AddLocMin(vertex2, polytype, isOpen, minimaList);
							}
							vertex2 = vertex3;
						}
						if (isOpen)
						{
							vertex2.flags |= VertexFlags.OpenEnd;
							if (flag)
							{
								vertex2.flags |= VertexFlags.LocalMax;
							}
							else
							{
								ClipperEngine.AddLocMin(vertex2, polytype, isOpen, minimaList);
							}
						}
						else if (flag != flag2)
						{
							if (flag2)
							{
								ClipperEngine.AddLocMin(vertex2, polytype, false, minimaList);
							}
							else
							{
								vertex2.flags |= VertexFlags.LocalMax;
							}
						}
					}
				}
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005D00 File Offset: 0x00003F00
		internal unsafe static void AddPathToVertexList([Nullable(0)] SpanCompat<Point64> path, PathType polytype, bool isOpen, List<LocalMinima> minimaList, List<Vertex> vertexList, VertexPool vertexPool)
		{
			int length = path.Length;
			vertexList.EnsureCapacity(vertexList.Count + length);
			Vertex vertex = null;
			Vertex vertex2 = null;
			for (int i = 0; i < path.Length; i++)
			{
				Point64 point = *path[i];
				if (vertex == null)
				{
					vertex = vertexPool.GetNew(point, VertexFlags.None, null);
					vertexList.Add(vertex);
					vertex2 = vertex;
				}
				else if (vertex2.pt != point)
				{
					Vertex vertex3 = vertexPool.GetNew(point, VertexFlags.None, vertex2);
					vertexList.Add(vertex3);
					vertex2.next = vertex3;
					vertex2 = vertex3;
				}
			}
			if (vertex2 == null || vertex2.prev == null)
			{
				return;
			}
			if (!isOpen && vertex2.pt == vertex.pt)
			{
				vertex2 = vertex2.prev;
			}
			vertex2.next = vertex;
			vertex.prev = vertex2;
			if (!isOpen && vertex2.next == vertex2)
			{
				return;
			}
			bool flag;
			if (isOpen)
			{
				Vertex vertex3 = vertex.next;
				while (vertex3 != vertex && vertex3.pt.Y == vertex.pt.Y)
				{
					vertex3 = vertex3.next;
				}
				flag = (vertex3.pt.Y <= vertex.pt.Y);
				if (flag)
				{
					vertex.flags = VertexFlags.OpenStart;
					ClipperEngine.AddLocMin(vertex, polytype, true, minimaList);
				}
				else
				{
					vertex.flags = (VertexFlags.OpenStart | VertexFlags.LocalMax);
				}
			}
			else
			{
				vertex2 = vertex.prev;
				while (vertex2 != vertex && vertex2.pt.Y == vertex.pt.Y)
				{
					vertex2 = vertex2.prev;
				}
				if (vertex2 == vertex)
				{
					return;
				}
				flag = (vertex2.pt.Y > vertex.pt.Y);
			}
			bool flag2 = flag;
			vertex2 = vertex;
			for (Vertex vertex3 = vertex.next; vertex3 != vertex; vertex3 = vertex3.next)
			{
				if (vertex3.pt.Y > vertex2.pt.Y && flag)
				{
					vertex2.flags |= VertexFlags.LocalMax;
					flag = false;
				}
				else if (vertex3.pt.Y < vertex2.pt.Y && !flag)
				{
					flag = true;
					ClipperEngine.AddLocMin(vertex2, polytype, isOpen, minimaList);
				}
				vertex2 = vertex3;
			}
			if (!isOpen)
			{
				if (flag != flag2)
				{
					if (flag2)
					{
						ClipperEngine.AddLocMin(vertex2, polytype, false, minimaList);
						return;
					}
					vertex2.flags |= VertexFlags.LocalMax;
				}
				return;
			}
			vertex2.flags |= VertexFlags.OpenEnd;
			if (flag)
			{
				vertex2.flags |= VertexFlags.LocalMax;
				return;
			}
			ClipperEngine.AddLocMin(vertex2, polytype, isOpen, minimaList);
		}
	}
}
