using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000047 RID: 71
	public class ObjectOctree
	{
		// Token: 0x02000071 RID: 113
		public class LODParent
		{
			// Token: 0x060001F1 RID: 497 RVA: 0x00011AFC File Offset: 0x0000FCFC
			public LODParent(int lodCount)
			{
				this.lodLevels = new ObjectOctree.LODLevel[lodCount];
				for (int i = 0; i < this.lodLevels.Length; i++)
				{
					this.lodLevels[i] = new ObjectOctree.LODLevel();
				}
			}

			// Token: 0x060001F2 RID: 498 RVA: 0x00011B3C File Offset: 0x0000FD3C
			public void AssignLODGroup(MeshCombiner meshCombiner)
			{
				LOD[] array = new LOD[this.lodLevels.Length];
				int num = array.Length - 1;
				for (int i = 0; i < this.lodLevels.Length; i++)
				{
					ObjectOctree.LODLevel lodlevel = this.lodLevels[i];
					LOD[] array2 = array;
					int num2 = i;
					float screenRelativeTransitionHeight = meshCombiner.lodGroupsSettings[num].lodSettings[i].screenRelativeTransitionHeight;
					Renderer[] renderers = lodlevel.newMeshRenderers.ToArray();
					array2[num2] = new LOD(screenRelativeTransitionHeight, renderers);
				}
				this.lodGroup.SetLODs(array);
				this.lodGroup.size = (float)meshCombiner.cellSize;
				meshCombiner.lodGroupsSettings[num].CopyToLodGroup(this.lodGroup, array);
			}

			// Token: 0x060001F3 RID: 499 RVA: 0x00011BDC File Offset: 0x0000FDDC
			public void ApplyChanges(MeshCombiner meshCombiner)
			{
				for (int i = 0; i < this.lodLevels.Length; i++)
				{
					this.lodLevels[i].ApplyChanges(meshCombiner);
				}
				this.hasChanged = false;
			}

			// Token: 0x040002C0 RID: 704
			public GameObject cellGO;

			// Token: 0x040002C1 RID: 705
			public Transform cellT;

			// Token: 0x040002C2 RID: 706
			public LODGroup lodGroup;

			// Token: 0x040002C3 RID: 707
			public ObjectOctree.LODLevel[] lodLevels;

			// Token: 0x040002C4 RID: 708
			public bool hasChanged;

			// Token: 0x040002C5 RID: 709
			public int jobsPending;
		}

		// Token: 0x02000072 RID: 114
		public class LODLevel
		{
			// Token: 0x060001F4 RID: 500 RVA: 0x00011C14 File Offset: 0x0000FE14
			public void ApplyChanges(MeshCombiner meshCombiner)
			{
				for (int i = 0; i < this.changedMeshObjectsHolders.Count; i++)
				{
					this.changedMeshObjectsHolders.items[i].hasChanged = false;
				}
				this.changedMeshObjectsHolders.Clear();
			}

			// Token: 0x040002C6 RID: 710
			public FastList<CachedGameObject> cachedGOs = new FastList<CachedGameObject>();

			// Token: 0x040002C7 RID: 711
			public Dictionary<CombineCondition, MeshObjectsHolder> meshObjectsHoldersLookup;

			// Token: 0x040002C8 RID: 712
			public FastList<MeshObjectsHolder> changedMeshObjectsHolders;

			// Token: 0x040002C9 RID: 713
			public FastList<MeshRenderer> newMeshRenderers = new FastList<MeshRenderer>();

			// Token: 0x040002CA RID: 714
			public int vertCount;

			// Token: 0x040002CB RID: 715
			public int objectCount;
		}

		// Token: 0x02000073 RID: 115
		public class MaxCell : ObjectOctree.Cell
		{
			// Token: 0x060001F6 RID: 502 RVA: 0x00011C74 File Offset: 0x0000FE74
			public void ApplyChanges(MeshCombiner meshCombiner)
			{
				for (int i = 0; i < this.changedLodParents.Count; i++)
				{
					this.changedLodParents[i].ApplyChanges(meshCombiner);
				}
				this.changedLodParents.Clear();
				this.hasChanged = false;
			}

			// Token: 0x040002CC RID: 716
			public static int maxCellCount;

			// Token: 0x040002CD RID: 717
			public ObjectOctree.LODParent[] lodParents;

			// Token: 0x040002CE RID: 718
			public List<ObjectOctree.LODParent> changedLodParents;

			// Token: 0x040002CF RID: 719
			public bool hasChanged;
		}

		// Token: 0x02000074 RID: 116
		public class Cell : BaseOctree.Cell
		{
			// Token: 0x060001F8 RID: 504 RVA: 0x00011CC3 File Offset: 0x0000FEC3
			public Cell()
			{
			}

			// Token: 0x060001F9 RID: 505 RVA: 0x00011CCB File Offset: 0x0000FECB
			public Cell(Vector3 position, Vector3 size, int maxLevels) : base(position, size, maxLevels)
			{
			}

			// Token: 0x060001FA RID: 506 RVA: 0x00011CD6 File Offset: 0x0000FED6
			public ObjectOctree.MaxCell GetCell(Vector3 position)
			{
				if (!base.InsideBounds(position))
				{
					return null;
				}
				return this.GetCellInternal(position);
			}

			// Token: 0x060001FB RID: 507 RVA: 0x00011CEC File Offset: 0x0000FEEC
			private ObjectOctree.MaxCell GetCellInternal(Vector3 position)
			{
				if (this.level == this.maxLevels)
				{
					return (ObjectOctree.MaxCell)this;
				}
				ObjectOctree.Cell cell = base.GetCell<ObjectOctree.Cell>(this.cells, position);
				if (cell == null)
				{
					return null;
				}
				return cell.GetCellInternal(position);
			}

			// Token: 0x060001FC RID: 508 RVA: 0x00011D28 File Offset: 0x0000FF28
			public CachedGameObject AddObject(Vector3 position, MeshCombiner meshCombiner, CachedGameObject cachedGO, int lodParentIndex, int lodLevel, bool isChangeMode = false)
			{
				if (base.InsideBounds(position))
				{
					this.AddObjectInternal(meshCombiner, cachedGO, position, lodParentIndex, lodLevel, isChangeMode);
					return cachedGO;
				}
				return null;
			}

			// Token: 0x060001FD RID: 509 RVA: 0x00011D48 File Offset: 0x0000FF48
			private void AddObjectInternal(MeshCombiner meshCombiner, CachedGameObject cachedGO, Vector3 position, int lodParentIndex, int lodLevel, bool isChangeMode)
			{
				if (this.level == this.maxLevels)
				{
					ObjectOctree.MaxCell maxCell = (ObjectOctree.MaxCell)this;
					if (maxCell.lodParents == null)
					{
						maxCell.lodParents = new ObjectOctree.LODParent[10];
					}
					if (maxCell.lodParents[lodParentIndex] == null)
					{
						maxCell.lodParents[lodParentIndex] = new ObjectOctree.LODParent(lodParentIndex + 1);
					}
					ObjectOctree.LODParent lodparent = maxCell.lodParents[lodParentIndex];
					ObjectOctree.LODLevel lodlevel = lodparent.lodLevels[lodLevel];
					lodlevel.cachedGOs.Add(cachedGO);
					if (isChangeMode && this.SortObject(meshCombiner, lodlevel, cachedGO, false))
					{
						if (!maxCell.hasChanged)
						{
							maxCell.hasChanged = true;
							if (meshCombiner.changedCells == null)
							{
								meshCombiner.changedCells = new List<ObjectOctree.MaxCell>();
							}
							meshCombiner.changedCells.Add(maxCell);
						}
						if (!lodparent.hasChanged)
						{
							lodparent.hasChanged = true;
							maxCell.changedLodParents.Add(lodparent);
						}
					}
					lodlevel.objectCount++;
					lodlevel.vertCount += cachedGO.mesh.vertexCount;
					return;
				}
				bool flag;
				int num = base.AddCell<ObjectOctree.Cell, ObjectOctree.MaxCell>(ref this.cells, position, out flag);
				if (flag)
				{
					ObjectOctree.MaxCell.maxCellCount++;
				}
				this.cells[num].AddObjectInternal(meshCombiner, cachedGO, position, lodParentIndex, lodLevel, isChangeMode);
			}

			// Token: 0x060001FE RID: 510 RVA: 0x00011E78 File Offset: 0x00010078
			public void SortObjects(MeshCombiner meshCombiner)
			{
				if (this.level == this.maxLevels)
				{
					foreach (ObjectOctree.LODParent lodparent in ((ObjectOctree.MaxCell)this).lodParents)
					{
						if (lodparent != null)
						{
							for (int j = 0; j < lodparent.lodLevels.Length; j++)
							{
								ObjectOctree.LODLevel lodlevel = lodparent.lodLevels[j];
								if (lodlevel == null || lodlevel.cachedGOs.Count == 0)
								{
									return;
								}
								for (int k = 0; k < lodlevel.cachedGOs.Count; k++)
								{
									CachedGameObject cachedGO = lodlevel.cachedGOs.items[k];
									if (!this.SortObject(meshCombiner, lodlevel, cachedGO, false))
									{
										lodlevel.cachedGOs.RemoveAt(k--);
									}
								}
							}
						}
					}
					return;
				}
				for (int l = 0; l < 8; l++)
				{
					if (this.cellsUsed[l])
					{
						this.cells[l].SortObjects(meshCombiner);
					}
				}
			}

			// Token: 0x060001FF RID: 511 RVA: 0x00011F68 File Offset: 0x00010168
			public bool SortObject(MeshCombiner meshCombiner, ObjectOctree.LODLevel lod, CachedGameObject cachedGO, bool isChangeMode = false)
			{
				if (cachedGO.mr == null)
				{
					return false;
				}
				if (lod.meshObjectsHoldersLookup == null)
				{
					lod.meshObjectsHoldersLookup = new Dictionary<CombineCondition, MeshObjectsHolder>();
				}
				CombineConditionSettings combineConditionSettings = meshCombiner.combineConditionSettings;
				Material[] sharedMaterials = cachedGO.mr.sharedMaterials;
				int num = Mathf.Min(cachedGO.mesh.subMeshCount, sharedMaterials.Length);
				int num2 = -1;
				if (meshCombiner.combineMode == CombineMode.DynamicObjects)
				{
					num2 = cachedGO.rootInstanceId;
					if (num2 == -1)
					{
						cachedGO.GetRoot();
						num2 = cachedGO.rootInstanceId;
					}
				}
				int i = 0;
				while (i < num)
				{
					Material material;
					if (!combineConditionSettings.sameMaterial)
					{
						material = combineConditionSettings.material;
						goto IL_9A;
					}
					material = sharedMaterials[i];
					if (!(material == null))
					{
						goto IL_9A;
					}
					IL_146:
					i++;
					continue;
					IL_9A:
					CombineCondition combineCondition = default(CombineCondition);
					combineCondition.ReadFromGameObject(num2, combineConditionSettings, meshCombiner.copyBakedLighting && meshCombiner.validCopyBakedLighting, cachedGO.go, cachedGO.t, cachedGO.mr, material);
					MeshObjectsHolder meshObjectsHolder;
					if (!lod.meshObjectsHoldersLookup.TryGetValue(combineCondition, out meshObjectsHolder))
					{
						meshCombiner.foundCombineConditions.combineConditions.Add(combineCondition);
						meshObjectsHolder = new MeshObjectsHolder(ref combineCondition, material);
						lod.meshObjectsHoldersLookup.Add(combineCondition, meshObjectsHolder);
					}
					meshObjectsHolder.meshObjects.Add(new MeshObject(cachedGO, i));
					if (isChangeMode && !meshObjectsHolder.hasChanged)
					{
						meshObjectsHolder.hasChanged = true;
						lod.changedMeshObjectsHolders.Add(meshObjectsHolder);
						goto IL_146;
					}
					goto IL_146;
				}
				return true;
			}

			// Token: 0x06000200 RID: 512 RVA: 0x000120CC File Offset: 0x000102CC
			public void CombineMeshes(MeshCombiner meshCombiner, int lodParentIndex)
			{
				if (this.level != this.maxLevels)
				{
					for (int i = 0; i < 8; i++)
					{
						if (this.cellsUsed[i])
						{
							this.cells[i].CombineMeshes(meshCombiner, lodParentIndex);
						}
					}
					return;
				}
				ObjectOctree.LODParent lodparent = ((ObjectOctree.MaxCell)this).lodParents[lodParentIndex];
				if (lodparent == null)
				{
					return;
				}
				CombineMode combineMode = meshCombiner.combineMode;
				if (combineMode != CombineMode.DynamicObjects)
				{
					lodparent.cellGO = new GameObject((meshCombiner.combineMode == CombineMode.StaticObjects) ? ("Cell " + this.bounds.center.ToString()) : "Combined Objects");
					lodparent.cellT = lodparent.cellGO.transform;
					lodparent.cellT.position = this.bounds.center;
					lodparent.cellT.parent = meshCombiner.lodParentHolders[lodParentIndex].t;
				}
				if (lodParentIndex > 0)
				{
					lodparent.lodGroup = lodparent.cellGO.AddComponent<LODGroup>();
					lodparent.lodGroup.localReferencePoint = (lodparent.cellT.position = this.bounds.center);
				}
				ObjectOctree.LODLevel[] lodLevels = lodparent.lodLevels;
				for (int j = 0; j < lodLevels.Length; j++)
				{
					ObjectOctree.LODLevel lodlevel = lodparent.lodLevels[j];
					if (lodlevel == null || lodlevel.meshObjectsHoldersLookup == null)
					{
						return;
					}
					Transform transform = null;
					if (lodParentIndex > 0)
					{
						transform = new GameObject("LOD" + j.ToString()).transform;
						transform.parent = lodparent.cellT;
					}
					foreach (MeshObjectsHolder meshObjectsHolder in lodlevel.meshObjectsHoldersLookup.Values)
					{
						meshObjectsHolder.lodParent = lodparent;
						meshObjectsHolder.lodLevel = j;
						Vector3 position = (combineMode == CombineMode.DynamicObjects) ? meshObjectsHolder.meshObjects.items[0].cachedGO.rootT.position : this.bounds.center;
						MeshCombineJobManager.instance.AddJob(meshCombiner, meshObjectsHolder, (lodParentIndex > 0) ? transform : lodparent.cellT, position);
					}
				}
			}

			// Token: 0x06000201 RID: 513 RVA: 0x000122F8 File Offset: 0x000104F8
			public void Draw(MeshCombiner meshCombiner, bool onlyMaxLevel, bool drawLevel0)
			{
				if (!onlyMaxLevel || this.level == this.maxLevels || (drawLevel0 && this.level == 0))
				{
					Gizmos.DrawWireCube(this.bounds.center, this.bounds.size);
					if (this.level == this.maxLevels && meshCombiner.drawMeshBounds)
					{
						ObjectOctree.LODParent[] lodParents = ((ObjectOctree.MaxCell)this).lodParents;
						for (int i = 0; i < lodParents.Length; i++)
						{
							if (lodParents[i] != null)
							{
								ObjectOctree.LODLevel[] lodLevels = lodParents[i].lodLevels;
								Gizmos.color = (meshCombiner.activeOriginal ? Color.blue : Color.green);
								for (int j = 0; j < lodLevels.Length; j++)
								{
									for (int k = 0; k < lodLevels[j].cachedGOs.Count; k++)
									{
										if (!(lodLevels[j].cachedGOs.items[k].mr == null))
										{
											Bounds bounds = lodLevels[j].cachedGOs.items[k].mr.bounds;
											Gizmos.DrawWireCube(bounds.center, bounds.size);
										}
									}
								}
								Gizmos.color = Color.white;
							}
						}
						return;
					}
				}
				if (this.cells == null || this.cellsUsed == null)
				{
					return;
				}
				for (int l = 0; l < 8; l++)
				{
					if (this.cellsUsed[l])
					{
						this.cells[l].Draw(meshCombiner, onlyMaxLevel, drawLevel0);
					}
				}
			}

			// Token: 0x040002D0 RID: 720
			public ObjectOctree.Cell[] cells;
		}
	}
}
