using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200001A RID: 26
	[ExecuteInEditMode]
	public class MeshCombiner : MonoBehaviour
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000061 RID: 97 RVA: 0x00005118 File Offset: 0x00003318
		// (remove) Token: 0x06000062 RID: 98 RVA: 0x00005150 File Offset: 0x00003350
		public event MeshCombiner.EventMethod onCombiningStart;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000063 RID: 99 RVA: 0x00005188 File Offset: 0x00003388
		// (remove) Token: 0x06000064 RID: 100 RVA: 0x000051C0 File Offset: 0x000033C0
		public event MeshCombiner.EventMethod onCombiningAbort;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000065 RID: 101 RVA: 0x000051F8 File Offset: 0x000033F8
		// (remove) Token: 0x06000066 RID: 102 RVA: 0x00005230 File Offset: 0x00003430
		public event MeshCombiner.EventMethod onCombiningReady;

		// Token: 0x06000067 RID: 103 RVA: 0x00005268 File Offset: 0x00003468
		public void AddMeshColliders()
		{
			try
			{
				for (int i = 0; i < this.addMeshCollidersList.Count; i++)
				{
					MeshColliderAdd meshColliderAdd = this.addMeshCollidersList.items[i];
					MeshCollider meshCollider = meshColliderAdd.go.AddComponent<MeshCollider>();
					meshCollider.sharedMesh = meshColliderAdd.mesh;
					meshCollider.material = this.physicsMaterial;
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			finally
			{
				this.addMeshCollidersList.Clear();
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000052F0 File Offset: 0x000034F0
		public void ExecuteOnCombiningReady()
		{
			this.totalMeshCombineJobs = 0;
			this.ExecuteHandleObjects(false, MeshCombiner.HandleComponent.Disable, MeshCombiner.HandleComponent.Disable, true, false);
			this.stopwatch.Stop();
			this.combineTime = (float)this.stopwatch.ElapsedMilliseconds / 1000f;
			this.combinedActive = true;
			this.combined = true;
			this.isCombining = false;
			LODGroupSetup[] componentsInChildren = base.GetComponentsInChildren<LODGroupSetup>();
			if (componentsInChildren != null)
			{
				foreach (LODGroupSetup lodgroupSetup in componentsInChildren)
				{
					try
					{
						lodgroupSetup.ApplySetup();
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogError("An error occurred applying lod setup");
						UnityEngine.Debug.LogException(exception);
					}
				}
			}
			if (this.onCombiningReady != null)
			{
				this.onCombiningReady(this);
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000053A4 File Offset: 0x000035A4
		private void Awake()
		{
			this.Init();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000053AC File Offset: 0x000035AC
		private void OnEnable()
		{
			this.Init();
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000053B4 File Offset: 0x000035B4
		private void Init()
		{
			if (this.thisInstance == null)
			{
				MeshCombiner.instances.Add(this);
				this.thisInstance = this;
				if (MeshCombiner.onInit != null)
				{
					MeshCombiner.onInit(this);
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000053E8 File Offset: 0x000035E8
		private void OnDisable()
		{
			this.thisInstance = null;
			MeshCombiner.instances.Remove(this);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005400 File Offset: 0x00003600
		public void InitData()
		{
			if ((this.searchOptions.parentGOs == null || this.searchOptions.parentGOs.Length == 0) && this.searchOptions.parent)
			{
				this.searchOptions.parentGOs = new GameObject[]
				{
					this.searchOptions.parent
				};
			}
			if (this.data == null)
			{
				this.data = base.GetComponent<MeshCombinerData>();
				if (this.data == null)
				{
					this.data = base.gameObject.AddComponent<MeshCombinerData>();
					this.data.combinedGameObjects = new List<GameObject>(this.combinedGameObjects);
					this.data.foundObjects = new List<CachedGameObject>(this.foundObjects);
					this.data.foundLodObjects = new List<CachedLodGameObject>(this.foundLodObjects);
					this.data.foundLodGroups = new List<LODGroup>(this.foundLodGroups);
					this.data.foundColliders = new List<Collider>(this.foundColliders);
					this.combinedGameObjects.Clear();
					this.foundObjects.Clear();
					this.foundLodObjects.Clear();
					this.foundLodGroups.Clear();
					this.foundColliders.Clear();
				}
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00005540 File Offset: 0x00003740
		private void Start()
		{
			if (Application.isPlaying && !this.combineInRuntime)
			{
				return;
			}
			this.InitMeshCombineJobManager();
			if (MeshCombiner.instances[0] == this)
			{
				MeshCombineJobManager.instance.SetJobMode(this.jobSettings);
			}
			if (!Application.isPlaying && Application.isEditor)
			{
				return;
			}
			this.StartRuntime();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000559C File Offset: 0x0000379C
		private void OnDestroy()
		{
			this.RestoreOriginalRenderersAndLODGroups(true);
			this.thisInstance = null;
			MeshCombiner.instances.Remove(this);
			if (MeshCombiner.instances.Count == 0 && MeshCombineJobManager.instance != null)
			{
				Methods.Destroy(MeshCombineJobManager.instance.gameObject);
				MeshCombineJobManager.instance = null;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000055F4 File Offset: 0x000037F4
		public static MeshCombiner GetInstance(string name)
		{
			for (int i = 0; i < MeshCombiner.instances.Count; i++)
			{
				if (MeshCombiner.instances[i].gameObject.name == name)
				{
					return MeshCombiner.instances[i];
				}
			}
			return null;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005640 File Offset: 0x00003840
		public void CopyJobSettingsToAllInstances()
		{
			for (int i = 0; i < MeshCombiner.instances.Count; i++)
			{
				MeshCombiner.instances[i].jobSettings.CopySettings(this.jobSettings);
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000567D File Offset: 0x0000387D
		public void InitMeshCombineJobManager()
		{
			if (MeshCombineJobManager.instance == null)
			{
				MeshCombineJobManager.CreateInstance(this, this.instantiatePrefab);
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000569C File Offset: 0x0000389C
		public void CreateLodGroupsSettings()
		{
			this.lodGroupsSettings = new MeshCombiner.LODGroupSettings[8];
			for (int i = 0; i < this.lodGroupsSettings.Length; i++)
			{
				this.lodGroupsSettings[i] = new MeshCombiner.LODGroupSettings(i);
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000056D8 File Offset: 0x000038D8
		private void StartRuntime()
		{
			if (this.combineInRuntime)
			{
				if (this.combineOnStart)
				{
					this.CombineAll(true);
				}
				if (this.useCombineSwapKey && this.originalMeshRenderers == MeshCombiner.HandleComponent.Disable && this.originalLODGroups == MeshCombiner.HandleComponent.Disable)
				{
					if (SwapCombineKey.instance == null)
					{
						base.gameObject.AddComponent<SwapCombineKey>();
						return;
					}
					SwapCombineKey.instance.meshCombinerList.Add(this);
				}
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005740 File Offset: 0x00003940
		public void DestroyCombinedObjects()
		{
			this.AbortAndClearMeshCombineJobs(false);
			this.RestoreOriginalRenderersAndLODGroups(false);
			Methods.DestroyChildren(base.transform);
			List<GameObject> list = this.data.combinedGameObjects;
			for (int i = 0; i < list.Count; i++)
			{
				Methods.Destroy(list[i]);
			}
			list.Clear();
			this.data.ClearAll();
			this.combinedActive = false;
			this.combined = false;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000057B0 File Offset: 0x000039B0
		public void Reset()
		{
			this.DestroyCombinedObjects();
			this.uniqueLodObjects.Clear();
			this.uniqueFoundLodGroups.Clear();
			this.unreadableMeshes.Clear();
			this.foundCombineConditions.combineConditions.Clear();
			this.ResetOctree();
			this.hasFoundFirstObject = false;
			this.bounds.center = (this.bounds.size = Vector3.zero);
			if (this.searchOptions.useSearchBox)
			{
				this.searchOptions.GetSearchBoxBounds();
			}
			this.InitAndResetLodParentsCount();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005840 File Offset: 0x00003A40
		public void AbortAndClearMeshCombineJobs(bool executeAbortEvent = true)
		{
			foreach (MeshCombineJobManager.MeshCombineJob meshCombineJob in this.meshCombineJobs)
			{
				meshCombineJob.abort = true;
				meshCombineJob.meshCombiner.isCombining = false;
			}
			this.ClearMeshCombineJobs(executeAbortEvent);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000058A4 File Offset: 0x00003AA4
		public void ClearMeshCombineJobs(bool executeAbortEvent = true)
		{
			this.meshCombineJobs.Clear();
			this.totalMeshCombineJobs = 0;
			if (executeAbortEvent && this.onCombiningAbort != null)
			{
				this.onCombiningAbort(this);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000058D0 File Offset: 0x00003AD0
		public void AddObjects(Transform rootT, List<Transform> transforms, bool useSearchOptions, bool checkForLODGroups = true)
		{
			List<LODGroup> list = new List<LODGroup>();
			if (checkForLODGroups)
			{
				for (int i = 0; i < transforms.Count; i++)
				{
					LODGroup component = transforms[i].GetComponent<LODGroup>();
					if (component != null)
					{
						list.Add(component);
					}
				}
				if (list.Count > 0)
				{
					this.AddLodGroups(rootT, list.ToArray(), useSearchOptions);
				}
			}
			this.AddTransforms(rootT, transforms.ToArray(), useSearchOptions);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000593C File Offset: 0x00003B3C
		public void AddObjectsAutomatically(bool useSearchConditions = true)
		{
			this.InitData();
			this.Reset();
			this.AddObjectsFromSearchParent(useSearchConditions);
			if (this.combineMode == CombineMode.DynamicObjects && this.data.foundLodObjects.Count > 0)
			{
				UnityEngine.Debug.Log("(MeshCombineStudio) => Lod Groups don't work yet for dynamic objects (they only work on static objects), this feature will be added in the next update.");
				this.data.foundLodObjects.Clear();
				return;
			}
			this.AddFoundObjectsToOctree();
			if (this.octreeContainsObjects)
			{
				this.octree.SortObjects(this);
				CombineCondition.MakeFoundReport(this.foundCombineConditions);
				this.cellCount = ObjectOctree.MaxCell.maxCellCount;
			}
			if (Console.instance != null)
			{
				this.LogOctreeInfo();
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000059D8 File Offset: 0x00003BD8
		public void AddFoundObjectsToOctree()
		{
			List<CachedGameObject> list = this.data.foundObjects;
			List<CachedLodGameObject> list2 = this.data.foundLodObjects;
			if (list.Count > 0 || list2.Count > 0)
			{
				this.octreeContainsObjects = true;
				this.CalcOctreeSize(this.bounds);
				ObjectOctree.MaxCell.maxCellCount = 0;
				for (int i = 0; i < list.Count; i++)
				{
					CachedGameObject cachedGameObject = list[i];
					Vector3 position = (this.searchOptions.objectCenter == MeshCombiner.ObjectCenter.TransformPosition) ? cachedGameObject.t.position : cachedGameObject.mr.bounds.center;
					this.octree.AddObject(position, this, cachedGameObject, 0, 0, false);
				}
				for (int j = 0; j < list2.Count; j++)
				{
					CachedLodGameObject cachedLodGameObject = list2[j];
					this.octree.AddObject(cachedLodGameObject.center, this, cachedLodGameObject, cachedLodGameObject.lodCount, cachedLodGameObject.lodLevel, false);
				}
				return;
			}
			UnityEngine.Debug.Log("(MeshCombineStudio) => No matching GameObjects with chosen search options are found for combining.");
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00005AD8 File Offset: 0x00003CD8
		public void ResetOctree()
		{
			this.octreeContainsObjects = false;
			if (this.octree == null)
			{
				this.octree = new ObjectOctree.Cell();
				return;
			}
			BaseOctree.Cell[] cells = this.octree.cells;
			BaseOctree.Cell[] array = cells;
			this.octree.Reset(ref array);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00005B1C File Offset: 0x00003D1C
		public void CalcOctreeSize(Bounds bounds)
		{
			Methods.SnapBoundsAndPreserveArea(ref bounds, (float)this.cellSize, (this.combineMode == CombineMode.StaticObjects) ? this.cellOffset : Vector3.zero);
			int num;
			float num2;
			if (this.combineMode == CombineMode.StaticObjects)
			{
				num = Mathf.CeilToInt(Mathf.Log(Mathf.Max(Mathw.GetMax(bounds.size), (float)this.cellSize) / (float)this.cellSize, 2f));
				num2 = (float)((int)Mathf.Pow(2f, (float)num) * this.cellSize);
			}
			else
			{
				num2 = Mathw.GetMax(bounds.size);
				num = 0;
			}
			if (num == 0 && this.octree != null)
			{
				this.octree = new ObjectOctree.MaxCell();
			}
			else if (num > 0 && this.octree is ObjectOctree.MaxCell)
			{
				this.octree = new ObjectOctree.Cell();
			}
			this.octree.maxLevels = num;
			this.octree.bounds = new Bounds(bounds.center, new Vector3(num2, num2, num2));
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005C0C File Offset: 0x00003E0C
		public void ApplyChanges()
		{
			this.validRebakeLighting = (this.rebakeLighting && !this.validCopyBakedLighting && !Application.isPlaying && Application.isEditor);
			for (int i = 0; i < this.changedCells.Count; i++)
			{
				ObjectOctree.MaxCell maxCell = this.changedCells[i];
				maxCell.hasChanged = false;
				maxCell.ApplyChanges(this);
			}
			this.changedCells.Clear();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005C78 File Offset: 0x00003E78
		public void CombineAll(bool useSearchConditions = true)
		{
			if (this.instantiatePrefab == null)
			{
				UnityEngine.Debug.LogError("(MeshCombineStudio) => The `Custom Combined GameObject` is null. Make sure it's assigned in the 'Use Custom Combine GameObject` setting");
				return;
			}
			if (!this.combineConditionSettings.sameMaterial && this.combineConditionSettings.material == null)
			{
				UnityEngine.Debug.LogError("(MeshCombineStudio) => You need to assign an output material in 'Combine Conditions' => 'Change Materials'. Keep in mind with this setting you ignore the source materials and combine all meshes into 1 output material.");
				return;
			}
			if (this.onCombiningStart != null)
			{
				this.onCombiningStart(this);
			}
			if (this.removeBackFaceTriangles && this.backFaceTriangleMode == MeshCombiner.BackFaceTriangleMode.Transform)
			{
				if (this.backFaceT == null)
				{
					UnityEngine.Debug.LogError("(MeshCombineStudio) => You need to assign the BackFace Transform in 'Output Settings'.");
					return;
				}
				this.backFaceDirection = this.backFaceT.forward;
			}
			this.InitMeshCombineJobManager();
			this.isCombining = true;
			this.stopwatch.Reset();
			this.stopwatch.Start();
			this.addMeshCollidersList.Clear();
			this.unreadableMeshes.Clear();
			this.selectImportSettingsMeshes.Clear();
			this.AddObjectsAutomatically(useSearchConditions);
			if (!this.octreeContainsObjects)
			{
				return;
			}
			this.validRebakeLighting = (this.rebakeLighting && !this.validCopyBakedLighting && !Application.isPlaying && Application.isEditor);
			this.newTotalVertices = (this.newTotalTriangles = (this.originalTotalVertices = (this.originalTotalTriangles = (this.originalDrawCalls = (this.newDrawCalls = 0)))));
			this.originalTotalNormalChannels = (this.originalTotalTangentChannels = (this.originalTotalUvChannels = (this.originalTotalUv2Channels = (this.originalTotalUv3Channels = (this.originalTotalUv4Channels = (this.originalTotalColorChannels = 0))))));
			this.newTotalNormalChannels = (this.newTotalTangentChannels = (this.newTotalUvChannels = (this.newTotalUv2Channels = (this.newTotalUv3Channels = (this.newTotalUv4Channels = (this.newTotalColorChannels = 0))))));
			for (int i = 0; i < this.lodParentHolders.Length; i++)
			{
				MeshCombiner.LodParentHolder lodParentHolder = this.lodParentHolders[i];
				if (lodParentHolder.found)
				{
					if (lodParentHolder.go == null && this.combineMode != CombineMode.DynamicObjects)
					{
						lodParentHolder.Create(this, i);
					}
					this.octree.CombineMeshes(this, i);
				}
			}
			if (MeshCombineJobManager.instance.jobSettings.combineJobMode == MeshCombineJobManager.CombineJobMode.CombineAtOnce)
			{
				MeshCombineJobManager.instance.ExecuteJobs();
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005EAC File Offset: 0x000040AC
		private void InitAndResetLodParentsCount()
		{
			for (int i = 0; i < this.lodParentHolders.Length; i++)
			{
				if (this.lodParentHolders[i].lods == null || this.lodParentHolders[i].lods.Length != i + 1)
				{
					this.lodParentHolders[i].Init(i + 1);
				}
				else
				{
					this.lodParentHolders[i].Reset();
				}
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005F10 File Offset: 0x00004110
		public void AddObjectsFromSearchParent(bool useSearchConditions)
		{
			if (this.searchOptions.parentGOs == null || this.searchOptions.parentGOs.Length == 0)
			{
				UnityEngine.Debug.Log("(MeshCombineStudio) => You need to assign at least one Parent GameObject to 'Search Parents' in which meshes will be searched");
				return;
			}
			foreach (GameObject gameObject in this.searchOptions.parentGOs)
			{
				if (!(gameObject == null))
				{
					Transform transform = gameObject.transform;
					LODGroup[] componentsInChildren = gameObject.GetComponentsInChildren<LODGroup>(true);
					this.AddLodGroups(transform, componentsInChildren, true);
					Transform[] componentsInChildren2 = gameObject.GetComponentsInChildren<Transform>(true);
					this.AddTransforms(transform, componentsInChildren2, useSearchConditions);
				}
			}
			List<CachedGameObject> list = this.data.foundObjects;
			List<LODGroup> list2 = this.data.foundLodGroups;
			List<Collider> list3 = this.data.foundColliders;
			Dictionary<Collider, CachedGameObject> colliderLookup = this.data.colliderLookup;
			Dictionary<LODGroup, CachedGameObject> lodGroupLookup = this.data.lodGroupLookup;
			if (this.addMeshColliders)
			{
				for (int j = 0; j < list.Count; j++)
				{
					foreach (Collider collider in list[j].go.GetComponentsInChildren<Collider>(false))
					{
						if (!colliderLookup.ContainsKey(collider))
						{
							list3.Add(collider);
							colliderLookup.Add(collider, list[j]);
						}
					}
				}
				for (int l = 0; l < list2.Count; l++)
				{
					LODGroup key = list2[l];
					foreach (Collider collider2 in list2[l].gameObject.GetComponentsInChildren<Collider>(false))
					{
						if (!colliderLookup.ContainsKey(collider2))
						{
							list3.Add(collider2);
							colliderLookup.Add(collider2, lodGroupLookup[key]);
						}
					}
				}
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000060C8 File Offset: 0x000042C8
		private void CheckForFoundObjectNotOnOverlapLayerMask(GameObject go)
		{
			if (!Methods.IsLayerInLayerMask(this.overlapLayerMask, go.layer))
			{
				UnityEngine.Debug.LogError(string.Concat(new string[]
				{
					"(MeshCombineStudio) => ",
					go.name,
					" on layer ",
					LayerMask.LayerToName(go.layer),
					" is not part of the Overlap LayerMask"
				}), go);
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00006128 File Offset: 0x00004328
		private void AddLodGroups(Transform searchParentT, LODGroup[] lodGroups, bool useSearchOptions = true)
		{
			List<CachedLodGameObject> list = new List<CachedLodGameObject>();
			CachedGameObject cachedGameObject = null;
			int i = 0;
			while (i < lodGroups.Length)
			{
				LODGroup lodgroup = lodGroups[i];
				if (this.searchOptions.lodGroupSearchMode == MeshCombiner.SearchOptions.LODGroupSearchMode.LodGroup)
				{
					bool flag = this.ValidObject(searchParentT, lodgroup.transform, MeshCombiner.ObjectType.LodGroup, useSearchOptions, ref cachedGameObject) == 1;
					goto IL_58;
				}
				if (!this.searchOptions.onlyActive || lodgroup.gameObject.activeInHierarchy)
				{
					bool flag = true;
					goto IL_58;
				}
				IL_2A7:
				i++;
				continue;
				IL_58:
				LOD[] lods = lodgroup.GetLODs();
				int num = lods.Length - 1;
				if (num <= 0)
				{
					goto IL_2A7;
				}
				if (i == 0)
				{
					this.lodGroupsSettings[num].CopyFromLodGroup(lodgroup, lods);
				}
				Vector3 vector = Vector3.zero;
				int num2 = 0;
				for (int j = 0; j < lods.Length; j++)
				{
					LOD lod = lods[j];
					for (int k = 0; k < lod.renderers.Length; k++)
					{
						Renderer renderer = lod.renderers[k];
						if (renderer)
						{
							bool flag;
							if (flag)
							{
								CachedGameObject cachedGameObject2 = null;
								int num3 = this.ValidObject(searchParentT, renderer.transform, MeshCombiner.ObjectType.LodRenderer, useSearchOptions, ref cachedGameObject2);
								if (num3 == -1)
								{
									goto IL_16A;
								}
								if (num3 == -2)
								{
									list.Clear();
									goto IL_191;
								}
								if (this.removeOverlappingTriangles && this.reportFoundObjectsNotOnOverlapLayerMask)
								{
									this.CheckForFoundObjectNotOnOverlapLayerMask(cachedGameObject2.go);
								}
								list.Add(new CachedLodGameObject(cachedGameObject2, num, j));
								if (this.searchOptions.objectCenter == MeshCombiner.ObjectCenter.BoundsCenter)
								{
									vector += cachedGameObject2.mr.bounds.center;
									num2++;
								}
							}
							this.uniqueLodObjects.Add(renderer.transform);
						}
						IL_16A:;
					}
				}
				IL_191:
				if (list.Count > 0)
				{
					if (this.searchOptions.objectCenter == MeshCombiner.ObjectCenter.BoundsCenter)
					{
						vector /= (float)num2;
					}
					else
					{
						vector = lodgroup.transform.position;
					}
					List<CachedLodGameObject> list2 = this.data.foundLodObjects;
					for (int l = 0; l < list.Count; l++)
					{
						CachedLodGameObject cachedLodGameObject = list[l];
						if (l == 0)
						{
							this.data.lodGroupLookup[lodgroup] = cachedLodGameObject;
						}
						cachedLodGameObject.center = vector;
						if (!this.hasFoundFirstObject)
						{
							this.bounds.center = cachedLodGameObject.mr.bounds.center;
							this.hasFoundFirstObject = true;
						}
						this.bounds.Encapsulate(cachedLodGameObject.mr.bounds);
						list2.Add(cachedLodGameObject);
						this.lodParentHolders[num].found = true;
						this.lodParentHolders[num].lods[cachedLodGameObject.lodLevel]++;
					}
					this.uniqueFoundLodGroups.Add(lodgroup);
					list.Clear();
					goto IL_2A7;
				}
				goto IL_2A7;
			}
			this.data.foundLodGroups = new List<LODGroup>(this.uniqueFoundLodGroups);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00006400 File Offset: 0x00004600
		private void AddTransforms(Transform searchParentT, Transform[] transforms, bool useSearchConditions = true)
		{
			int count = this.uniqueLodObjects.Count;
			List<CachedGameObject> list = this.data.foundObjects;
			foreach (Transform transform in transforms)
			{
				if (count <= 0 || !this.uniqueLodObjects.Contains(transform))
				{
					CachedGameObject cachedGameObject = null;
					if (this.ValidObject(searchParentT, transform, MeshCombiner.ObjectType.Normal, useSearchConditions, ref cachedGameObject) == 1)
					{
						if (this.removeOverlappingTriangles && this.reportFoundObjectsNotOnOverlapLayerMask)
						{
							this.CheckForFoundObjectNotOnOverlapLayerMask(cachedGameObject.go);
						}
						if (!this.hasFoundFirstObject)
						{
							this.bounds.center = cachedGameObject.mr.bounds.center;
							this.hasFoundFirstObject = true;
						}
						this.bounds.Encapsulate(cachedGameObject.mr.bounds);
						list.Add(cachedGameObject);
						this.lodParentHolders[0].lods[0]++;
					}
				}
			}
			if (list.Count > 0)
			{
				this.lodParentHolders[0].found = true;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00006504 File Offset: 0x00004704
		private int ValidObject(Transform searchParentT, Transform t, MeshCombiner.ObjectType objectType, bool useSearchOptions, ref CachedGameObject cachedGameObject)
		{
			if (t == null)
			{
				return -1;
			}
			GameObject gameObject = t.gameObject;
			MeshRenderer meshRenderer = null;
			MeshFilter meshFilter = null;
			Mesh mesh = null;
			if (objectType != MeshCombiner.ObjectType.LodGroup || this.searchOptions.lodGroupSearchMode == MeshCombiner.SearchOptions.LODGroupSearchMode.LodRenderers)
			{
				meshRenderer = t.GetComponent<MeshRenderer>();
				if (meshRenderer == null || (!meshRenderer.enabled && this.searchOptions.onlyActiveMeshRenderers))
				{
					return -1;
				}
				meshFilter = t.GetComponent<MeshFilter>();
				if (meshFilter == null)
				{
					return -1;
				}
				mesh = meshFilter.sharedMesh;
				if (mesh == null)
				{
					return -1;
				}
				if (mesh.vertexCount > 65534)
				{
					return -2;
				}
			}
			if (useSearchOptions)
			{
				if (this.searchOptions.onlyActive && !gameObject.activeInHierarchy)
				{
					return -1;
				}
				if (objectType != MeshCombiner.ObjectType.LodRenderer || this.searchOptions.lodGroupSearchMode == MeshCombiner.SearchOptions.LODGroupSearchMode.LodRenderers)
				{
					if (this.searchOptions.useLayerMask)
					{
						int num = 1 << t.gameObject.layer;
						if ((this.searchOptions.layerMask.value & num) != num)
						{
							return -1;
						}
					}
					if (this.searchOptions.onlyStatic && !gameObject.isStatic)
					{
						return -1;
					}
					if (this.searchOptions.useTag && !t.CompareTag(this.searchOptions.tag))
					{
						return -1;
					}
					if (this.searchOptions.useComponentsFilter)
					{
						if (this.searchOptions.componentCondition == MeshCombiner.SearchOptions.ComponentCondition.And)
						{
							bool flag = true;
							for (int i = 0; i < this.searchOptions.componentNameList.Count; i++)
							{
								if (t.GetComponent(this.searchOptions.componentNameList[i]) == null)
								{
									flag = false;
									break;
								}
							}
							if (!flag)
							{
								return -1;
							}
						}
						else if (this.searchOptions.componentCondition == MeshCombiner.SearchOptions.ComponentCondition.Or)
						{
							bool flag2 = false;
							for (int j = 0; j < this.searchOptions.componentNameList.Count; j++)
							{
								if (t.GetComponent(this.searchOptions.componentNameList[j]) != null)
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								return -1;
							}
						}
						else
						{
							bool flag3 = true;
							for (int k = 0; k < this.searchOptions.componentNameList.Count; k++)
							{
								if (t.GetComponent(this.searchOptions.componentNameList[k]) != null)
								{
									flag3 = false;
									break;
								}
							}
							if (!flag3)
							{
								return -1;
							}
						}
					}
					if (this.searchOptions.useNameContains)
					{
						bool flag4 = false;
						for (int l = 0; l < this.searchOptions.nameContainList.Count; l++)
						{
							if (Methods.Contains(t.name, this.searchOptions.nameContainList[l]))
							{
								flag4 = true;
								break;
							}
						}
						if (!flag4)
						{
							return -1;
						}
					}
					if (this.searchOptions.useSearchBox)
					{
						if (this.searchOptions.objectCenter == MeshCombiner.ObjectCenter.BoundsCenter)
						{
							if (!this.searchOptions.searchBoxBounds.Contains(meshRenderer.bounds.center))
							{
								return -2;
							}
						}
						else if (!this.searchOptions.searchBoxBounds.Contains(t.position))
						{
							return -2;
						}
					}
				}
				if (objectType != MeshCombiner.ObjectType.LodGroup)
				{
					if (this.searchOptions.useVertexInputLimit && mesh.vertexCount > this.searchOptions.vertexInputLimit)
					{
						return -2;
					}
					if (this.useVertexOutputLimit && mesh.vertexCount > this.vertexOutputLimit)
					{
						return -2;
					}
					if (this.searchOptions.useMaxBoundsFactor && this.combineMode == CombineMode.StaticObjects && Mathw.GetMax(meshRenderer.bounds.size) > (float)this.cellSize * this.searchOptions.maxBoundsFactor)
					{
						return -2;
					}
				}
			}
			if ((objectType != MeshCombiner.ObjectType.LodGroup || this.searchOptions.lodGroupSearchMode == MeshCombiner.SearchOptions.LODGroupSearchMode.LodRenderers) && !mesh.isReadable)
			{
				if (this.unreadableMeshes.Add(mesh))
				{
					UnityEngine.Debug.LogError("(MeshCombineStudio) => Read/Write is disabled on the mesh on GameObject " + gameObject.name + " and can't be combined. Click the 'Make Meshes Readable' in the MCS Inspector to make it automatically readable in the mesh import settings.");
				}
				return -1;
			}
			if (objectType != MeshCombiner.ObjectType.LodGroup)
			{
				cachedGameObject = new CachedGameObject(searchParentT, gameObject, t, meshRenderer, meshFilter, mesh);
			}
			return 1;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000068D9 File Offset: 0x00004AD9
		public void RestoreOriginalRenderersAndLODGroups(bool onDestroy)
		{
			if (this.activeOriginal)
			{
				return;
			}
			this.ExecuteHandleObjects(true, MeshCombiner.HandleComponent.Disable, MeshCombiner.HandleComponent.Disable, true, onDestroy);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000068EF File Offset: 0x00004AEF
		public void SwapCombine()
		{
			if (!this.combined)
			{
				this.CombineAll(true);
				return;
			}
			this.combinedActive = !this.combinedActive;
			this.ExecuteHandleObjects(!this.combinedActive, this.originalMeshRenderers, this.originalLODGroups, true, false);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00006930 File Offset: 0x00004B30
		private void SetOriginalCollidersActive(bool active, bool onDestroy)
		{
			if (this.data == null && !onDestroy)
			{
				this.InitData();
			}
			if (this.data == null)
			{
				return;
			}
			List<Collider> list = this.data.foundColliders;
			for (int i = 0; i < list.Count; i++)
			{
				Collider collider = list[i];
				if (collider)
				{
					CachedGameObject cachedGameObject;
					this.data.colliderLookup.TryGetValue(collider, out cachedGameObject);
					if (cachedGameObject == null || !cachedGameObject.excludeCombine)
					{
						collider.enabled = active;
					}
					else
					{
						Methods.ListRemoveAt<Collider>(list, i--);
					}
				}
				else
				{
					Methods.ListRemoveAt<Collider>(list, i--);
				}
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000069D0 File Offset: 0x00004BD0
		private void ExecuteMeshFilter(bool active, CachedGameObject cachedGO)
		{
			if (active)
			{
				if (cachedGO.mfr)
				{
					cachedGO.mfr.RevertMeshFilter(cachedGO.mf);
					return;
				}
			}
			else
			{
				MeshFilterRevert meshFilterRevert = cachedGO.go.AddComponent<MeshFilterRevert>();
				if (meshFilterRevert.DestroyAndReferenceMeshFilter(cachedGO.mf))
				{
					cachedGO.mfr = meshFilterRevert;
					return;
				}
				Methods.Destroy(meshFilterRevert);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00006A28 File Offset: 0x00004C28
		public void ExecuteHandleObjects(bool active, MeshCombiner.HandleComponent handleOriginalObjects, MeshCombiner.HandleComponent handleOriginalLodGroups, bool includeColliders = true, bool onDestroy = false)
		{
			this.activeOriginal = active;
			Methods.SetChildrenActive(base.transform, !active);
			bool flag = !Application.isPlaying && (this.removeOriginalMeshReference || this.usedRemoveOriginalMeshRederences);
			if (!active)
			{
				this.usedRemoveOriginalMeshRederences = flag;
			}
			else
			{
				this.usedRemoveOriginalMeshRederences = false;
			}
			List<CachedGameObject> list;
			List<CachedLodGameObject> list2;
			List<LODGroup> list3;
			List<Collider> list4;
			if (onDestroy)
			{
				list = this.foundObjects;
				list2 = this.foundLodObjects;
				list3 = this.foundLodGroups;
				list4 = this.foundColliders;
			}
			else
			{
				this.InitData();
				if (this.data == null)
				{
					return;
				}
				list = this.data.foundObjects;
				list2 = this.data.foundLodObjects;
				list3 = this.data.foundLodGroups;
				list4 = this.data.foundColliders;
			}
			if (handleOriginalObjects == MeshCombiner.HandleComponent.Disable)
			{
				if (includeColliders)
				{
					this.SetOriginalCollidersActive(active, onDestroy);
				}
				for (int i = 0; i < list.Count; i++)
				{
					CachedGameObject cachedGameObject = list[i];
					if (cachedGameObject.mr && !cachedGameObject.excludeCombine)
					{
						cachedGameObject.mr.enabled = (cachedGameObject.mrEnabled && active);
						if (active)
						{
							cachedGameObject.go.hideFlags = HideFlags.None;
						}
						else if (this.useOriginalObjectsHideFlags)
						{
							cachedGameObject.go.hideFlags = this.orginalObjectsHideFlags;
						}
						if (flag)
						{
							this.ExecuteMeshFilter(active, cachedGameObject);
						}
					}
					else
					{
						Methods.ListRemoveAt<CachedGameObject>(list, i--);
					}
				}
				for (int j = 0; j < list2.Count; j++)
				{
					CachedLodGameObject cachedLodGameObject = list2[j];
					if (cachedLodGameObject.mr && !cachedLodGameObject.excludeCombine)
					{
						cachedLodGameObject.mr.enabled = (cachedLodGameObject.mrEnabled && active);
						if (flag)
						{
							this.ExecuteMeshFilter(active, cachedLodGameObject);
						}
					}
					else
					{
						Methods.ListRemoveAt<CachedLodGameObject>(list2, j--);
					}
				}
			}
			if (handleOriginalObjects == MeshCombiner.HandleComponent.Destroy)
			{
				for (int k = 0; k < list4.Count; k++)
				{
					Collider collider = list4[k];
					if (collider)
					{
						CachedGameObject cachedGameObject2;
						this.data.colliderLookup.TryGetValue(collider, out cachedGameObject2);
						if (cachedGameObject2 == null || !cachedGameObject2.excludeCombine)
						{
							UnityEngine.Object.Destroy(collider);
						}
						else
						{
							Methods.ListRemoveAt<Collider>(list4, k--);
						}
					}
					else
					{
						Methods.ListRemoveAt<Collider>(list4, k--);
					}
				}
				for (int l = 0; l < list.Count; l++)
				{
					bool flag2 = false;
					CachedGameObject cachedGameObject3 = list[l];
					if (!cachedGameObject3.excludeCombine)
					{
						if (cachedGameObject3.mf)
						{
							UnityEngine.Object.Destroy(cachedGameObject3.mf);
						}
						else
						{
							flag2 = true;
						}
						if (cachedGameObject3.mr)
						{
							UnityEngine.Object.Destroy(cachedGameObject3.mr);
						}
						else
						{
							flag2 = true;
						}
					}
					else
					{
						flag2 = true;
					}
					if (flag2)
					{
						Methods.ListRemoveAt<CachedGameObject>(list, l--);
					}
				}
				for (int m = 0; m < list2.Count; m++)
				{
					bool flag3 = false;
					CachedGameObject cachedGameObject4 = list2[m];
					if (!cachedGameObject4.excludeCombine)
					{
						if (cachedGameObject4.mf)
						{
							UnityEngine.Object.Destroy(cachedGameObject4.mf);
						}
						else
						{
							flag3 = true;
						}
						if (cachedGameObject4.mr)
						{
							UnityEngine.Object.Destroy(cachedGameObject4.mr);
						}
						else
						{
							flag3 = true;
						}
					}
					else
					{
						flag3 = true;
					}
					if (flag3)
					{
						Methods.ListRemoveAt<CachedLodGameObject>(list2, m--);
					}
				}
			}
			for (int n = 0; n < list3.Count; n++)
			{
				LODGroup lodgroup = list3[n];
				if (lodgroup)
				{
					CachedGameObject cachedGameObject5;
					this.data.lodGroupLookup.TryGetValue(lodgroup, out cachedGameObject5);
					if (cachedGameObject5 == null || !cachedGameObject5.excludeCombine)
					{
						if (active)
						{
							lodgroup.gameObject.hideFlags = HideFlags.None;
						}
						else if (this.useOriginalObjectsHideFlags)
						{
							lodgroup.gameObject.hideFlags = this.orginalObjectsHideFlags;
						}
						if (handleOriginalLodGroups == MeshCombiner.HandleComponent.Disable)
						{
							lodgroup.enabled = active;
						}
						else
						{
							UnityEngine.Object.Destroy(lodgroup);
						}
					}
					else
					{
						Methods.ListRemoveAt<LODGroup>(list3, n--);
					}
				}
				else
				{
					Methods.ListRemoveAt<LODGroup>(list3, n--);
				}
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00006E18 File Offset: 0x00005018
		private void DrawGizmosCube(Bounds bounds, Color color)
		{
			Gizmos.color = color;
			Gizmos.DrawWireCube(bounds.center, bounds.size);
			Gizmos.color = new Color(color.r, color.g, color.b, 0.5f);
			Gizmos.DrawCube(bounds.center, bounds.size);
			Gizmos.color = Color.white;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00006E7C File Offset: 0x0000507C
		private void OnDrawGizmosSelected()
		{
			if (this.addMeshColliders && this.addMeshCollidersInRange)
			{
				this.DrawGizmosCube(this.addMeshCollidersBounds, Color.green);
			}
			if (this.removeBackFaceTriangles && this.backFaceTriangleMode == MeshCombiner.BackFaceTriangleMode.Box)
			{
				this.DrawGizmosCube(this.backFaceBounds, Color.blue);
			}
			if (!this.drawGizmos)
			{
				return;
			}
			if (this.octree != null && this.octreeContainsObjects)
			{
				this.octree.Draw(this, true, !this.searchOptions.useSearchBox);
			}
			if (this.searchOptions.useSearchBox)
			{
				this.searchOptions.GetSearchBoxBounds();
				Gizmos.color = Color.green;
				Gizmos.DrawWireCube(this.searchOptions.searchBoxBounds.center, this.searchOptions.searchBoxBounds.size);
				Gizmos.color = Color.white;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00006F54 File Offset: 0x00005154
		private void LogOctreeInfo()
		{
			Console.Log("Cells " + ObjectOctree.MaxCell.maxCellCount.ToString() + " -> Found Objects: ", 0, null, null);
			MeshCombiner.LodParentHolder[] array = this.lodParentHolders;
			if (array == null || array.Length == 0)
			{
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				MeshCombiner.LodParentHolder lodParentHolder = array[i];
				if (lodParentHolder.found)
				{
					string text = "LOD Group " + (i + 1).ToString() + " |";
					int[] lods = lodParentHolder.lods;
					for (int j = 0; j < lods.Length; j++)
					{
						text = text + " " + lods[j].ToString() + " |";
					}
					Console.Log(text, 0, null, null);
				}
			}
		}

		// Token: 0x04000064 RID: 100
		public static MeshCombiner.EventMethod onInit;

		// Token: 0x04000065 RID: 101
		public static List<MeshCombiner> instances = new List<MeshCombiner>();

		// Token: 0x04000069 RID: 105
		public MeshCombineJobManager.JobSettings jobSettings = new MeshCombineJobManager.JobSettings();

		// Token: 0x0400006A RID: 106
		public MeshCombiner.LODGroupSettings[] lodGroupsSettings;

		// Token: 0x0400006B RID: 107
		public ComputeShader computeDepthToArray;

		// Token: 0x0400006C RID: 108
		public bool useCustomInstantiatePrefab;

		// Token: 0x0400006D RID: 109
		public GameObject instantiatePrefab;

		// Token: 0x0400006E RID: 110
		public bool instantiatePrefabValid;

		// Token: 0x0400006F RID: 111
		public const int maxLodCount = 8;

		// Token: 0x04000070 RID: 112
		public string saveMeshesFolder;

		// Token: 0x04000071 RID: 113
		public ObjectOctree.Cell octree;

		// Token: 0x04000072 RID: 114
		public List<ObjectOctree.MaxCell> changedCells;

		// Token: 0x04000073 RID: 115
		[NonSerialized]
		public bool octreeContainsObjects;

		// Token: 0x04000074 RID: 116
		public bool unitySettingsFoldout = true;

		// Token: 0x04000075 RID: 117
		public MeshCombiner.SearchOptions searchOptions;

		// Token: 0x04000076 RID: 118
		public bool useOriginalObjectsHideFlags;

		// Token: 0x04000077 RID: 119
		public HideFlags orginalObjectsHideFlags;

		// Token: 0x04000078 RID: 120
		public CombineConditionSettings combineConditionSettings;

		// Token: 0x04000079 RID: 121
		public bool outputSettingsFoldout = true;

		// Token: 0x0400007A RID: 122
		public CombineMode combineMode;

		// Token: 0x0400007B RID: 123
		public int cellSize = 32;

		// Token: 0x0400007C RID: 124
		public Vector3 cellOffset;

		// Token: 0x0400007D RID: 125
		public int cellCount;

		// Token: 0x0400007E RID: 126
		public bool removeOriginalMeshReference;

		// Token: 0x0400007F RID: 127
		public bool usedRemoveOriginalMeshRederences;

		// Token: 0x04000080 RID: 128
		public bool useVertexOutputLimit;

		// Token: 0x04000081 RID: 129
		public int vertexOutputLimit = 64000;

		// Token: 0x04000082 RID: 130
		public MeshCombiner.RebakeLightingMode rebakeLightingMode;

		// Token: 0x04000083 RID: 131
		public bool copyBakedLighting;

		// Token: 0x04000084 RID: 132
		public bool validCopyBakedLighting;

		// Token: 0x04000085 RID: 133
		public bool rebakeLighting;

		// Token: 0x04000086 RID: 134
		public bool validRebakeLighting;

		// Token: 0x04000087 RID: 135
		public float scaleInLightmap = 1f;

		// Token: 0x04000088 RID: 136
		public bool addMeshColliders;

		// Token: 0x04000089 RID: 137
		public PhysicMaterial physicsMaterial;

		// Token: 0x0400008A RID: 138
		public bool addMeshCollidersInRange;

		// Token: 0x0400008B RID: 139
		public Bounds addMeshCollidersBounds;

		// Token: 0x0400008C RID: 140
		public bool makeMeshesUnreadable = true;

		// Token: 0x0400008D RID: 141
		public bool excludeSingleMeshes;

		// Token: 0x0400008E RID: 142
		public bool removeTrianglesBelowSurface;

		// Token: 0x0400008F RID: 143
		public bool noColliders;

		// Token: 0x04000090 RID: 144
		public LayerMask surfaceLayerMask;

		// Token: 0x04000091 RID: 145
		public float maxSurfaceHeight = 1000f;

		// Token: 0x04000092 RID: 146
		public bool removeOverlappingTriangles;

		// Token: 0x04000093 RID: 147
		public bool removeSamePositionTriangles;

		// Token: 0x04000094 RID: 148
		public bool reportFoundObjectsNotOnOverlapLayerMask = true;

		// Token: 0x04000095 RID: 149
		public GameObject overlappingCollidersGO;

		// Token: 0x04000096 RID: 150
		public LayerMask overlapLayerMask;

		// Token: 0x04000097 RID: 151
		public int voxelizeLayer;

		// Token: 0x04000098 RID: 152
		public int lodGroupLayer;

		// Token: 0x04000099 RID: 153
		public GameObject overlappingNonCombineGO;

		// Token: 0x0400009A RID: 154
		public bool disableOverlappingNonCombineGO;

		// Token: 0x0400009B RID: 155
		public bool removeBackFaceTriangles;

		// Token: 0x0400009C RID: 156
		public MeshCombiner.BackFaceTriangleMode backFaceTriangleMode;

		// Token: 0x0400009D RID: 157
		public Transform backFaceT;

		// Token: 0x0400009E RID: 158
		public Vector3 backFaceDirection;

		// Token: 0x0400009F RID: 159
		public Vector3 backFaceRotation;

		// Token: 0x040000A0 RID: 160
		public Bounds backFaceBounds;

		// Token: 0x040000A1 RID: 161
		public bool useExcludeBackfaceRemovalTag;

		// Token: 0x040000A2 RID: 162
		public string excludeBackfaceRemovalTag;

		// Token: 0x040000A3 RID: 163
		public bool weldVertices;

		// Token: 0x040000A4 RID: 164
		public bool weldSnapVertices;

		// Token: 0x040000A5 RID: 165
		public float weldSnapSize = 0.025f;

		// Token: 0x040000A6 RID: 166
		public bool weldIncludeNormals;

		// Token: 0x040000A7 RID: 167
		public bool jobSettingsFoldout = true;

		// Token: 0x040000A8 RID: 168
		public bool runtimeSettingsFoldout = true;

		// Token: 0x040000A9 RID: 169
		public bool combineInRuntime;

		// Token: 0x040000AA RID: 170
		public bool combineOnStart = true;

		// Token: 0x040000AB RID: 171
		public bool useCombineSwapKey;

		// Token: 0x040000AC RID: 172
		public KeyCode combineSwapKey = KeyCode.Tab;

		// Token: 0x040000AD RID: 173
		public MeshCombiner.HandleComponent originalMeshRenderers;

		// Token: 0x040000AE RID: 174
		public MeshCombiner.HandleComponent originalLODGroups;

		// Token: 0x040000AF RID: 175
		public bool meshSaveSettingsFoldout = true;

		// Token: 0x040000B0 RID: 176
		public bool deleteFilesFromSaveFolder;

		// Token: 0x040000B1 RID: 177
		public Vector3 oldPosition;

		// Token: 0x040000B2 RID: 178
		public Vector3 oldScale;

		// Token: 0x040000B3 RID: 179
		public MeshCombiner.LodParentHolder[] lodParentHolders = new MeshCombiner.LodParentHolder[8];

		// Token: 0x040000B4 RID: 180
		[HideInInspector]
		public List<GameObject> combinedGameObjects = new List<GameObject>();

		// Token: 0x040000B5 RID: 181
		[HideInInspector]
		public List<CachedGameObject> foundObjects = new List<CachedGameObject>();

		// Token: 0x040000B6 RID: 182
		[HideInInspector]
		public List<CachedLodGameObject> foundLodObjects = new List<CachedLodGameObject>();

		// Token: 0x040000B7 RID: 183
		[HideInInspector]
		public List<LODGroup> foundLodGroups = new List<LODGroup>();

		// Token: 0x040000B8 RID: 184
		[HideInInspector]
		public List<Collider> foundColliders = new List<Collider>();

		// Token: 0x040000B9 RID: 185
		public HashSet<LODGroup> uniqueFoundLodGroups = new HashSet<LODGroup>();

		// Token: 0x040000BA RID: 186
		public HashSet<Mesh> unreadableMeshes = new HashSet<Mesh>();

		// Token: 0x040000BB RID: 187
		public HashSet<Mesh> selectImportSettingsMeshes = new HashSet<Mesh>();

		// Token: 0x040000BC RID: 188
		public FoundCombineConditions foundCombineConditions = new FoundCombineConditions();

		// Token: 0x040000BD RID: 189
		public HashSet<MeshCombineJobManager.MeshCombineJob> meshCombineJobs = new HashSet<MeshCombineJobManager.MeshCombineJob>();

		// Token: 0x040000BE RID: 190
		public int totalMeshCombineJobs;

		// Token: 0x040000BF RID: 191
		public int mrDisabledCount;

		// Token: 0x040000C0 RID: 192
		public bool combined;

		// Token: 0x040000C1 RID: 193
		public bool isCombining;

		// Token: 0x040000C2 RID: 194
		public bool activeOriginal = true;

		// Token: 0x040000C3 RID: 195
		public bool combinedActive;

		// Token: 0x040000C4 RID: 196
		public bool drawGizmos = true;

		// Token: 0x040000C5 RID: 197
		public bool drawMeshBounds = true;

		// Token: 0x040000C6 RID: 198
		public int originalTotalVertices;

		// Token: 0x040000C7 RID: 199
		public int originalTotalTriangles;

		// Token: 0x040000C8 RID: 200
		public int newTotalVertices;

		// Token: 0x040000C9 RID: 201
		public int newTotalTriangles;

		// Token: 0x040000CA RID: 202
		public int originalDrawCalls;

		// Token: 0x040000CB RID: 203
		public int newDrawCalls;

		// Token: 0x040000CC RID: 204
		public int originalTotalNormalChannels;

		// Token: 0x040000CD RID: 205
		public int originalTotalTangentChannels;

		// Token: 0x040000CE RID: 206
		public int originalTotalUvChannels;

		// Token: 0x040000CF RID: 207
		public int originalTotalUv2Channels;

		// Token: 0x040000D0 RID: 208
		public int originalTotalUv3Channels;

		// Token: 0x040000D1 RID: 209
		public int originalTotalUv4Channels;

		// Token: 0x040000D2 RID: 210
		public int originalTotalColorChannels;

		// Token: 0x040000D3 RID: 211
		public int newTotalNormalChannels;

		// Token: 0x040000D4 RID: 212
		public int newTotalTangentChannels;

		// Token: 0x040000D5 RID: 213
		public int newTotalUvChannels;

		// Token: 0x040000D6 RID: 214
		public int newTotalUv2Channels;

		// Token: 0x040000D7 RID: 215
		public int newTotalUv3Channels;

		// Token: 0x040000D8 RID: 216
		public int newTotalUv4Channels;

		// Token: 0x040000D9 RID: 217
		public int newTotalColorChannels;

		// Token: 0x040000DA RID: 218
		public float combineTime;

		// Token: 0x040000DB RID: 219
		[NonSerialized]
		public MeshCombinerData data;

		// Token: 0x040000DC RID: 220
		public FastList<MeshColliderAdd> addMeshCollidersList = new FastList<MeshColliderAdd>();

		// Token: 0x040000DD RID: 221
		private HashSet<Transform> uniqueLodObjects = new HashSet<Transform>();

		// Token: 0x040000DE RID: 222
		[NonSerialized]
		private MeshCombiner thisInstance;

		// Token: 0x040000DF RID: 223
		private bool hasFoundFirstObject;

		// Token: 0x040000E0 RID: 224
		private Bounds bounds;

		// Token: 0x040000E1 RID: 225
		private Stopwatch stopwatch = new Stopwatch();

		// Token: 0x02000064 RID: 100
		public enum ObjectType
		{
			// Token: 0x04000277 RID: 631
			Normal,
			// Token: 0x04000278 RID: 632
			LodGroup,
			// Token: 0x04000279 RID: 633
			LodRenderer
		}

		// Token: 0x02000065 RID: 101
		public enum HandleComponent
		{
			// Token: 0x0400027B RID: 635
			Disable,
			// Token: 0x0400027C RID: 636
			Destroy
		}

		// Token: 0x02000066 RID: 102
		public enum ObjectCenter
		{
			// Token: 0x0400027E RID: 638
			BoundsCenter,
			// Token: 0x0400027F RID: 639
			TransformPosition
		}

		// Token: 0x02000067 RID: 103
		public enum BackFaceTriangleMode
		{
			// Token: 0x04000281 RID: 641
			Transform,
			// Token: 0x04000282 RID: 642
			Box,
			// Token: 0x04000283 RID: 643
			Direction,
			// Token: 0x04000284 RID: 644
			EulerAngles
		}

		// Token: 0x02000068 RID: 104
		// (Invoke) Token: 0x060001DA RID: 474
		public delegate void EventMethod(MeshCombiner meshCombiner);

		// Token: 0x02000069 RID: 105
		public enum RebakeLightingMode
		{
			// Token: 0x04000286 RID: 646
			CopyLightmapUvs,
			// Token: 0x04000287 RID: 647
			RegenarateLightmapUvs
		}

		// Token: 0x0200006A RID: 106
		[Serializable]
		public class SearchOptions
		{
			// Token: 0x060001DD RID: 477 RVA: 0x0001145B File Offset: 0x0000F65B
			public void GetSearchBoxBounds()
			{
				this.searchBoxBounds = new Bounds(this.searchBoxPivot + new Vector3(0f, this.searchBoxSize.y * 0.5f, 0f), this.searchBoxSize);
			}

			// Token: 0x04000288 RID: 648
			public bool foldoutSearchParents = true;

			// Token: 0x04000289 RID: 649
			public bool foldoutSearchConditions = true;

			// Token: 0x0400028A RID: 650
			public GameObject parent;

			// Token: 0x0400028B RID: 651
			public GameObject[] parentGOs;

			// Token: 0x0400028C RID: 652
			public MeshCombiner.ObjectCenter objectCenter;

			// Token: 0x0400028D RID: 653
			public MeshCombiner.SearchOptions.LODGroupSearchMode lodGroupSearchMode;

			// Token: 0x0400028E RID: 654
			public bool useSearchBox;

			// Token: 0x0400028F RID: 655
			public Bounds searchBoxBounds;

			// Token: 0x04000290 RID: 656
			public bool searchBoxSquare;

			// Token: 0x04000291 RID: 657
			public Vector3 searchBoxPivot;

			// Token: 0x04000292 RID: 658
			public Vector3 searchBoxSize = new Vector3(25f, 25f, 25f);

			// Token: 0x04000293 RID: 659
			public bool useMaxBoundsFactor = true;

			// Token: 0x04000294 RID: 660
			public float maxBoundsFactor = 1.5f;

			// Token: 0x04000295 RID: 661
			public bool useVertexInputLimit = true;

			// Token: 0x04000296 RID: 662
			public int vertexInputLimit = 5000;

			// Token: 0x04000297 RID: 663
			public bool useLayerMask;

			// Token: 0x04000298 RID: 664
			public LayerMask layerMask = -1;

			// Token: 0x04000299 RID: 665
			public bool useTag;

			// Token: 0x0400029A RID: 666
			public string tag;

			// Token: 0x0400029B RID: 667
			public bool useNameContains;

			// Token: 0x0400029C RID: 668
			public List<string> nameContainList = new List<string>();

			// Token: 0x0400029D RID: 669
			public bool onlyActive = true;

			// Token: 0x0400029E RID: 670
			public bool onlyStatic = true;

			// Token: 0x0400029F RID: 671
			public bool onlyActiveMeshRenderers = true;

			// Token: 0x040002A0 RID: 672
			public bool useComponentsFilter;

			// Token: 0x040002A1 RID: 673
			public MeshCombiner.SearchOptions.ComponentCondition componentCondition;

			// Token: 0x040002A2 RID: 674
			public List<string> componentNameList = new List<string>();

			// Token: 0x02000079 RID: 121
			public enum ComponentCondition
			{
				// Token: 0x040002D2 RID: 722
				And,
				// Token: 0x040002D3 RID: 723
				Or,
				// Token: 0x040002D4 RID: 724
				Not
			}

			// Token: 0x0200007A RID: 122
			public enum LODGroupSearchMode
			{
				// Token: 0x040002D6 RID: 726
				LodGroup,
				// Token: 0x040002D7 RID: 727
				LodRenderers
			}
		}

		// Token: 0x0200006B RID: 107
		[Serializable]
		public class LODGroupSettings
		{
			// Token: 0x060001DF RID: 479 RVA: 0x00011534 File Offset: 0x0000F734
			public LODGroupSettings(int lodParentIndex)
			{
				int num = lodParentIndex + 1;
				this.lodSettings = new MeshCombiner.LODSettings[num];
				float num2 = 1f / (float)num;
				for (int i = 0; i < this.lodSettings.Length; i++)
				{
					this.lodSettings[i] = new MeshCombiner.LODSettings(1f - num2 * (float)(i + 1));
				}
			}

			// Token: 0x060001E0 RID: 480 RVA: 0x0001158C File Offset: 0x0000F78C
			public void CopyFromLodGroup(LODGroup lodGroup, LOD[] lods)
			{
				this.animateCrossFading = lodGroup.animateCrossFading;
				this.fadeMode = lodGroup.fadeMode;
				for (int i = 0; i < lods.Length; i++)
				{
					this.lodSettings[i].fadeTransitionWidth = lods[i].fadeTransitionWidth;
				}
			}

			// Token: 0x060001E1 RID: 481 RVA: 0x000115D8 File Offset: 0x0000F7D8
			public void CopyToLodGroup(LODGroup lodGroup, LOD[] lods)
			{
				lodGroup.animateCrossFading = this.animateCrossFading;
				lodGroup.fadeMode = this.fadeMode;
				for (int i = 0; i < lods.Length; i++)
				{
					lods[i].fadeTransitionWidth = this.lodSettings[i].fadeTransitionWidth;
				}
			}

			// Token: 0x040002A3 RID: 675
			public bool animateCrossFading;

			// Token: 0x040002A4 RID: 676
			public LODFadeMode fadeMode;

			// Token: 0x040002A5 RID: 677
			public MeshCombiner.LODSettings[] lodSettings;
		}

		// Token: 0x0200006C RID: 108
		[Serializable]
		public class LODSettings
		{
			// Token: 0x060001E2 RID: 482 RVA: 0x00011624 File Offset: 0x0000F824
			public LODSettings(float screenRelativeTransitionHeight)
			{
				this.screenRelativeTransitionHeight = screenRelativeTransitionHeight;
			}

			// Token: 0x040002A6 RID: 678
			public float screenRelativeTransitionHeight;

			// Token: 0x040002A7 RID: 679
			public float fadeTransitionWidth;
		}

		// Token: 0x0200006D RID: 109
		[Serializable]
		public class LodParentHolder
		{
			// Token: 0x060001E3 RID: 483 RVA: 0x00011633 File Offset: 0x0000F833
			public void Init(int lodCount)
			{
				this.lods = new int[lodCount];
			}

			// Token: 0x060001E4 RID: 484 RVA: 0x00011644 File Offset: 0x0000F844
			public void Create(MeshCombiner meshCombiner, int lodParentIndex)
			{
				if (meshCombiner.data.foundLodGroups.Count == 0)
				{
					this.go = new GameObject((meshCombiner.combineMode == CombineMode.StaticObjects) ? "Cells" : "Combine Parent");
				}
				else
				{
					this.go = new GameObject("LODGroup " + (lodParentIndex + 1).ToString());
					this.go.AddComponent<LODGroupSetup>().Init(meshCombiner, lodParentIndex);
				}
				this.t = this.go.transform;
				this.t.transform.parent = meshCombiner.transform;
			}

			// Token: 0x060001E5 RID: 485 RVA: 0x000116DD File Offset: 0x0000F8DD
			public void Reset()
			{
				this.found = false;
				Array.Clear(this.lods, 0, this.lods.Length);
			}

			// Token: 0x040002A8 RID: 680
			public GameObject go;

			// Token: 0x040002A9 RID: 681
			public Transform t;

			// Token: 0x040002AA RID: 682
			public bool found;

			// Token: 0x040002AB RID: 683
			public int[] lods;
		}
	}
}
