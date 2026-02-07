using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000014 RID: 20
	public class CombinedLODManager : MonoBehaviour
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00003F30 File Offset: 0x00002130
		private void Awake()
		{
			this.cameraMainT = Camera.main.transform;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003F42 File Offset: 0x00002142
		private void InitOctree()
		{
			this.octree = new CombinedLODManager.Cell(this.octreeCenter, this.octreeSize, this.maxLevels);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003F61 File Offset: 0x00002161
		private void Start()
		{
			if (this.search)
			{
				this.search = false;
				this.InitOctree();
				this.Search();
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003F7E File Offset: 0x0000217E
		private void Update()
		{
			if (this.octree.cellsUsed != null)
			{
				this.Lod(this.lodMode);
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003F9C File Offset: 0x0000219C
		public void UpdateLods(MeshCombiner meshCombiner, int lodAmount)
		{
			if (this.lods != null && this.lods.Length == lodAmount)
			{
				return;
			}
			this.lods = new CombinedLODManager.LOD[lodAmount];
			float[] array = new float[lodAmount];
			for (int i = 0; i < this.lods.Length; i++)
			{
				this.lods[i] = new CombinedLODManager.LOD();
				if (this.lodDistanceMode == CombinedLODManager.LodDistanceMode.Automatic)
				{
					array[i] = (float)(meshCombiner.cellSize * i);
				}
				else if (this.distances != null && i < this.distances.Length)
				{
					array[i] = this.distances[i];
				}
			}
			this.distances = array;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000402C File Offset: 0x0000222C
		public void UpdateDistances(MeshCombiner meshCombiner)
		{
			if (this.lodDistanceMode != CombinedLODManager.LodDistanceMode.Automatic)
			{
				return;
			}
			for (int i = 0; i < this.distances.Length; i++)
			{
				this.distances[i] = (float)(meshCombiner.cellSize * i);
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004068 File Offset: 0x00002268
		public void Search()
		{
			for (int i = 0; i < this.lods.Length; i++)
			{
				this.lods[i].searchParent.gameObject.SetActive(true);
				MeshRenderer[] componentsInChildren = this.lods[i].searchParent.GetComponentsInChildren<MeshRenderer>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					this.octree.AddMeshRenderer(componentsInChildren[j], componentsInChildren[j].transform.position, i, this.lods.Length);
				}
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000040E8 File Offset: 0x000022E8
		public void ResetOctree()
		{
			if (this.octree == null)
			{
				return;
			}
			this.octree.cells = null;
			this.octree.cellsUsed = null;
			for (int i = 0; i < this.lods.Length; i++)
			{
				if (this.lods[i].searchParent != null)
				{
					UnityEngine.Object.Destroy(this.lods[i].searchParent.gameObject);
				}
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004158 File Offset: 0x00002358
		public void Lod(CombinedLODManager.LodMode lodMode)
		{
			Vector3 position = this.cameraMainT.position;
			for (int i = 0; i < this.lods.Length - 1; i++)
			{
				this.lods[i].sphere.center = position;
				this.lods[i].sphere.radius = this.distances[i + 1];
			}
			if (lodMode == CombinedLODManager.LodMode.Automatic)
			{
				this.octree.AutoLodInternal(this.lods, this.lodCulled ? this.lodCullDistance : -1f);
				return;
			}
			this.octree.LodInternal(this.lods, this.showLod);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000041F6 File Offset: 0x000023F6
		private void OnDrawGizmosSelected()
		{
			if (this.drawGizmos && this.octree != null && this.octree.cells != null)
			{
				this.octree.DrawGizmos(this.lods);
			}
		}

		// Token: 0x04000040 RID: 64
		public bool drawGizmos = true;

		// Token: 0x04000041 RID: 65
		public CombinedLODManager.LOD[] lods;

		// Token: 0x04000042 RID: 66
		public float[] distances;

		// Token: 0x04000043 RID: 67
		public CombinedLODManager.LodDistanceMode lodDistanceMode;

		// Token: 0x04000044 RID: 68
		public CombinedLODManager.LodMode lodMode;

		// Token: 0x04000045 RID: 69
		public int showLod;

		// Token: 0x04000046 RID: 70
		public bool lodCulled;

		// Token: 0x04000047 RID: 71
		public float lodCullDistance = 500f;

		// Token: 0x04000048 RID: 72
		public Vector3 octreeCenter = Vector3.zero;

		// Token: 0x04000049 RID: 73
		public Vector3 octreeSize = new Vector3(256f, 256f, 256f);

		// Token: 0x0400004A RID: 74
		public int maxLevels = 4;

		// Token: 0x0400004B RID: 75
		public bool search = true;

		// Token: 0x0400004C RID: 76
		private CombinedLODManager.Cell octree;

		// Token: 0x0400004D RID: 77
		private Transform cameraMainT;

		// Token: 0x02000057 RID: 87
		public enum LodMode
		{
			// Token: 0x0400022E RID: 558
			Automatic,
			// Token: 0x0400022F RID: 559
			DebugLod
		}

		// Token: 0x02000058 RID: 88
		public enum LodDistanceMode
		{
			// Token: 0x04000231 RID: 561
			Automatic,
			// Token: 0x04000232 RID: 562
			Manual
		}

		// Token: 0x02000059 RID: 89
		[Serializable]
		public class LOD
		{
			// Token: 0x060001B5 RID: 437 RVA: 0x0000F11E File Offset: 0x0000D31E
			public LOD()
			{
			}

			// Token: 0x060001B6 RID: 438 RVA: 0x0000F126 File Offset: 0x0000D326
			public LOD(Transform searchParent)
			{
				this.searchParent = searchParent;
			}

			// Token: 0x04000233 RID: 563
			public Transform searchParent;

			// Token: 0x04000234 RID: 564
			public Sphere3 sphere;
		}

		// Token: 0x0200005A RID: 90
		public class Cell : BaseOctree.Cell
		{
			// Token: 0x060001B7 RID: 439 RVA: 0x0000F135 File Offset: 0x0000D335
			public Cell()
			{
			}

			// Token: 0x060001B8 RID: 440 RVA: 0x0000F13D File Offset: 0x0000D33D
			public Cell(Vector3 position, Vector3 size, int maxLevels) : base(position, size, maxLevels)
			{
			}

			// Token: 0x060001B9 RID: 441 RVA: 0x0000F148 File Offset: 0x0000D348
			public void AddMeshRenderer(MeshRenderer mr, Vector3 position, int lodLevel, int lodLevels)
			{
				if (base.InsideBounds(position))
				{
					this.AddMeshRendererInternal(mr, position, lodLevel, lodLevels);
				}
			}

			// Token: 0x060001BA RID: 442 RVA: 0x0000F160 File Offset: 0x0000D360
			private void AddMeshRendererInternal(MeshRenderer mr, Vector3 position, int lodLevel, int lodLevels)
			{
				if (this.level == this.maxLevels)
				{
					CombinedLODManager.MaxCell maxCell = (CombinedLODManager.MaxCell)this;
					if (maxCell.mrList == null)
					{
						maxCell.mrList = new List<MeshRenderer>[lodLevels];
					}
					List<MeshRenderer>[] mrList = maxCell.mrList;
					if (mrList[lodLevel] == null)
					{
						mrList[lodLevel] = new List<MeshRenderer>();
					}
					mrList[lodLevel].Add(mr);
					maxCell.currentLod = -1;
					return;
				}
				bool flag;
				int num = base.AddCell<CombinedLODManager.Cell, CombinedLODManager.MaxCell>(ref this.cells, position, out flag);
				this.cells[num].box = new AABB3(this.cells[num].bounds.min, this.cells[num].bounds.max);
				this.cells[num].AddMeshRendererInternal(mr, position, lodLevel, lodLevels);
			}

			// Token: 0x060001BB RID: 443 RVA: 0x0000F214 File Offset: 0x0000D414
			public void AutoLodInternal(CombinedLODManager.LOD[] lods, float lodCulledDistance)
			{
				if (this.level == this.maxLevels)
				{
					CombinedLODManager.MaxCell maxCell = (CombinedLODManager.MaxCell)this;
					if (lodCulledDistance != -1f && (this.bounds.center - lods[0].sphere.center).sqrMagnitude > lodCulledDistance * lodCulledDistance)
					{
						if (maxCell.currentLod != -1)
						{
							for (int i = 0; i < lods.Length; i++)
							{
								for (int j = 0; j < maxCell.mrList[i].Count; j++)
								{
									maxCell.mrList[i][j].enabled = false;
								}
							}
							maxCell.currentLod = -1;
						}
						return;
					}
					int k = 0;
					while (k < lods.Length)
					{
						bool flag = k >= lods.Length - 1 || Mathw.IntersectAABB3Sphere3(this.box, lods[k].sphere);
						if (flag)
						{
							if (maxCell.currentLod != k)
							{
								for (int l = 0; l < lods.Length; l++)
								{
									bool enabled = l == k;
									for (int m = 0; m < maxCell.mrList[l].Count; m++)
									{
										maxCell.mrList[l][m].enabled = enabled;
									}
								}
								maxCell.currentLod = k;
								return;
							}
							return;
						}
						else
						{
							k++;
						}
					}
					return;
				}
				else
				{
					for (int n = 0; n < 8; n++)
					{
						if (this.cellsUsed[n])
						{
							this.cells[n].AutoLodInternal(lods, lodCulledDistance);
						}
					}
				}
			}

			// Token: 0x060001BC RID: 444 RVA: 0x0000F388 File Offset: 0x0000D588
			public void LodInternal(CombinedLODManager.LOD[] lods, int lodLevel)
			{
				if (this.level == this.maxLevels)
				{
					CombinedLODManager.MaxCell maxCell = (CombinedLODManager.MaxCell)this;
					if (maxCell.currentLod != lodLevel)
					{
						for (int i = 0; i < lods.Length; i++)
						{
							bool enabled = i == lodLevel;
							for (int j = 0; j < maxCell.mrList[i].Count; j++)
							{
								maxCell.mrList[i][j].enabled = enabled;
							}
						}
						maxCell.currentLod = lodLevel;
						return;
					}
				}
				else
				{
					for (int k = 0; k < 8; k++)
					{
						if (this.cellsUsed[k])
						{
							this.cells[k].LodInternal(lods, lodLevel);
						}
					}
				}
			}

			// Token: 0x060001BD RID: 445 RVA: 0x0000F428 File Offset: 0x0000D628
			public void DrawGizmos(CombinedLODManager.LOD[] lods)
			{
				for (int i = 0; i < lods.Length; i++)
				{
					if (i == 0)
					{
						Gizmos.color = Color.red;
					}
					else if (i == 1)
					{
						Gizmos.color = Color.green;
					}
					else if (i == 2)
					{
						Gizmos.color = Color.yellow;
					}
					else if (i == 3)
					{
						Gizmos.color = Color.blue;
					}
					Gizmos.DrawWireSphere(lods[i].sphere.center, lods[i].sphere.radius);
				}
				this.DrawGizmosInternal();
			}

			// Token: 0x060001BE RID: 446 RVA: 0x0000F4A8 File Offset: 0x0000D6A8
			public void DrawGizmosInternal()
			{
				if (this.level == this.maxLevels)
				{
					CombinedLODManager.MaxCell maxCell = (CombinedLODManager.MaxCell)this;
					if (maxCell.currentLod == 0)
					{
						Gizmos.color = Color.red;
					}
					else if (maxCell.currentLod == 1)
					{
						Gizmos.color = Color.green;
					}
					else if (maxCell.currentLod == 2)
					{
						Gizmos.color = Color.yellow;
					}
					else if (maxCell.currentLod == 3)
					{
						Gizmos.color = Color.blue;
					}
					Gizmos.DrawWireCube(this.bounds.center, this.bounds.size - new Vector3(0.25f, 0.25f, 0.25f));
					Gizmos.color = Color.white;
					return;
				}
				for (int i = 0; i < 8; i++)
				{
					if (this.cellsUsed[i])
					{
						this.cells[i].DrawGizmosInternal();
					}
				}
			}

			// Token: 0x04000235 RID: 565
			public CombinedLODManager.Cell[] cells;

			// Token: 0x04000236 RID: 566
			private AABB3 box;
		}

		// Token: 0x0200005B RID: 91
		public class MaxCell : CombinedLODManager.Cell
		{
			// Token: 0x04000237 RID: 567
			public List<MeshRenderer>[] mrList;

			// Token: 0x04000238 RID: 568
			public int currentLod;
		}
	}
}
