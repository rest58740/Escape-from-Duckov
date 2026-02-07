using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200001E RID: 30
	public class RemoveGeometryBelowTerrain : MonoBehaviour
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00007676 File Offset: 0x00005876
		private void Start()
		{
			if (this.runOnStart)
			{
				this.Remove(base.gameObject);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000768C File Offset: 0x0000588C
		public void Remove(GameObject go)
		{
			MeshFilter[] componentsInChildren = go.GetComponentsInChildren<MeshFilter>(true);
			this.totalTriangles = 0;
			this.removeTriangles = 0;
			this.skippedObjects = 0;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.RemoveMesh(componentsInChildren[i].transform, componentsInChildren[i].mesh);
			}
			Debug.Log(string.Concat(new string[]
			{
				"Removeable ",
				this.removeTriangles.ToString(),
				" total ",
				this.totalTriangles.ToString(),
				" improvement ",
				((float)this.removeTriangles / (float)this.totalTriangles * 100f).ToString("F2")
			}));
			Debug.Log("Skipped Objects " + this.skippedObjects.ToString());
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00007760 File Offset: 0x00005960
		public void RemoveMesh(Transform t, Mesh mesh)
		{
			if (mesh == null)
			{
				return;
			}
			if (!this.IsMeshUnderTerrain(t, mesh))
			{
				this.skippedObjects++;
				return;
			}
			Vector3[] vertices = mesh.vertices;
			List<int> list = new List<int>();
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				list.AddRange(mesh.GetTriangles(i));
				int count = list.Count;
				this.RemoveTriangles(t, list, vertices);
				if (list.Count < count)
				{
					mesh.SetTriangles(list.ToArray(), i);
				}
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000077E4 File Offset: 0x000059E4
		public bool IsMeshUnderTerrain(Transform t, Mesh mesh)
		{
			Bounds bounds = mesh.bounds;
			bounds.center += t.position;
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			Vector2 vector = new Vector2(max.x - min.x, max.z - min.z);
			for (float num = 0f; num < 1f; num += 0.125f)
			{
				for (float num2 = 0f; num2 < 1f; num2 += 0.125f)
				{
					ref Vector3 ptr = new Vector3(min.x + num2 * vector.x, min.y, min.z + num * vector.y);
					float num3 = 0f;
					if (ptr.y < num3)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000078B8 File Offset: 0x00005AB8
		public void GetTerrainComponents()
		{
			this.terrainComponents = new Terrain[this.terrains.Count];
			for (int i = 0; i < this.terrains.Count; i++)
			{
				Terrain component = this.terrains[i].GetComponent<Terrain>();
				this.terrainComponents[i] = component;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000790C File Offset: 0x00005B0C
		public void GetMeshRenderersAndComponents()
		{
			this.mrs = new MeshRenderer[this.meshTerrains.Count];
			this.meshTerrainComponents = new Mesh[this.meshTerrains.Count];
			for (int i = 0; i < this.meshTerrains.Count; i++)
			{
				this.mrs[i] = this.meshTerrains[i].GetComponent<MeshRenderer>();
				MeshFilter component = this.meshTerrains[i].GetComponent<MeshFilter>();
				this.meshTerrainComponents[i] = component.sharedMesh;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00007994 File Offset: 0x00005B94
		public void CreateTerrainBounds()
		{
			this.terrainBounds = new Bounds[this.terrainComponents.Length];
			for (int i = 0; i < this.terrainBounds.Length; i++)
			{
				this.terrainBounds[i] = default(Bounds);
				this.terrainBounds[i].min = this.terrains[i].position;
				this.terrainBounds[i].max = this.terrainBounds[i].min + this.terrainComponents[i].terrainData.size;
			}
			this.meshBounds = new Bounds[this.meshTerrains.Count];
			for (int j = 0; j < this.meshTerrains.Count; j++)
			{
				this.meshBounds[j] = this.mrs[j].bounds;
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00007A7C File Offset: 0x00005C7C
		public void MakeIntersectLists(Bounds bounds)
		{
			List<Terrain> list = new List<Terrain>();
			List<Mesh> list2 = new List<Mesh>();
			List<Bounds> list3 = new List<Bounds>();
			List<Bounds> list4 = new List<Bounds>();
			Vector3[] array = new Vector3[8];
			Vector3 size = bounds.size;
			array[0] = bounds.min;
			array[1] = array[0] + new Vector3(size.x, 0f, 0f);
			array[2] = array[0] + new Vector3(0f, 0f, size.z);
			array[3] = array[0] + new Vector3(size.x, 0f, size.z);
			array[4] = array[0] + new Vector3(0f, size.y, 0f);
			array[5] = array[0] + new Vector3(size.x, size.y, 0f);
			array[6] = array[0] + new Vector3(0f, size.y, size.z);
			array[7] = array[0] + size;
			for (int i = 0; i < 8; i++)
			{
				int num = this.InterectTerrain(array[i]);
				if (num != -1)
				{
					list.Add(this.terrainArray[num]);
					list3.Add(this.terrainBounds[num]);
				}
				num = this.InterectMesh(array[i]);
				if (num != -1)
				{
					list2.Add(this.meshArray[num]);
					list4.Add(this.meshBounds[num]);
				}
			}
			this.terrainArray = list.ToArray();
			this.meshArray = list2.ToArray();
			this.terrainBoundsArray = list3.ToArray();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007C84 File Offset: 0x00005E84
		public int InterectTerrain(Vector3 pos)
		{
			for (int i = 0; i < this.terrainBounds.Length; i++)
			{
				if (this.terrainBounds[i].Contains(pos))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00007CBC File Offset: 0x00005EBC
		public int InterectMesh(Vector3 pos)
		{
			for (int i = 0; i < this.meshBounds.Length; i++)
			{
				if (this.meshBounds[i].Contains(pos))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00007CF4 File Offset: 0x00005EF4
		public float GetTerrainHeight(Vector3 pos)
		{
			int num = -1;
			for (int i = 0; i < this.terrainArray.Length; i++)
			{
				if (this.terrainBoundsArray[i].Contains(pos))
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				return this.terrainArray[num].SampleHeight(pos);
			}
			return float.PositiveInfinity;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00007D48 File Offset: 0x00005F48
		public void RemoveTriangles(Transform t, List<int> newTriangles, Vector3[] vertices)
		{
			bool[] array = new bool[vertices.Length];
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < newTriangles.Count; i += 3)
			{
				this.totalTriangles++;
				int num = newTriangles[i];
				bool flag = array[num];
				if (!flag)
				{
					vector = t.TransformPoint(vertices[num]);
					float terrainHeight = this.GetTerrainHeight(vector);
					flag = (vector.y < terrainHeight);
				}
				if (flag)
				{
					array[num] = true;
					num = newTriangles[i + 1];
					flag = array[num];
					if (!flag)
					{
						vector = t.TransformPoint(vertices[num]);
						float terrainHeight = this.GetTerrainHeight(vector);
						flag = (vector.y < terrainHeight);
					}
					if (flag)
					{
						array[num] = true;
						num = newTriangles[i + 2];
						flag = array[num];
						if (!flag)
						{
							vector = t.TransformPoint(vertices[num]);
							float terrainHeight = this.GetTerrainHeight(vector);
							flag = (vector.y < terrainHeight);
						}
						if (flag)
						{
							array[num] = true;
							this.removeTriangles++;
							newTriangles.RemoveAt(i + 2);
							newTriangles.RemoveAt(i + 1);
							newTriangles.RemoveAt(i);
							if (i + 3 < newTriangles.Count)
							{
								i -= 3;
							}
						}
					}
				}
			}
		}

		// Token: 0x040000F7 RID: 247
		private int totalTriangles;

		// Token: 0x040000F8 RID: 248
		private int removeTriangles;

		// Token: 0x040000F9 RID: 249
		private int skippedObjects;

		// Token: 0x040000FA RID: 250
		public List<Transform> terrains = new List<Transform>();

		// Token: 0x040000FB RID: 251
		public List<Transform> meshTerrains = new List<Transform>();

		// Token: 0x040000FC RID: 252
		public Bounds[] terrainBounds;

		// Token: 0x040000FD RID: 253
		public Bounds[] meshBounds;

		// Token: 0x040000FE RID: 254
		private Terrain[] terrainComponents;

		// Token: 0x040000FF RID: 255
		private Terrain[] terrainArray;

		// Token: 0x04000100 RID: 256
		private Bounds[] terrainBoundsArray;

		// Token: 0x04000101 RID: 257
		private MeshRenderer[] mrs;

		// Token: 0x04000102 RID: 258
		private Mesh[] meshTerrainComponents;

		// Token: 0x04000103 RID: 259
		private Mesh[] meshArray;

		// Token: 0x04000104 RID: 260
		public bool runOnStart;
	}
}
