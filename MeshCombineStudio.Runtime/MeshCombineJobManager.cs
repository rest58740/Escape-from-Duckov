using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000016 RID: 22
	[DefaultExecutionOrder(-94000000)]
	[ExecuteInEditMode]
	public class MeshCombineJobManager : MonoBehaviour
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00004288 File Offset: 0x00002488
		public static MeshCombineJobManager CreateInstance(MeshCombiner meshCombiner, GameObject instantiatePrefab)
		{
			if (MeshCombineJobManager.instance != null)
			{
				MeshCombineJobManager.instance.camGeometryCapture.computeDepthToArray = meshCombiner.computeDepthToArray;
				return MeshCombineJobManager.instance;
			}
			GameObject gameObject = new GameObject("MCS Job Manager");
			MeshCombineJobManager.instance = gameObject.AddComponent<MeshCombineJobManager>();
			MeshCombineJobManager.instance.SetJobMode(meshCombiner.jobSettings);
			gameObject.AddComponent<Camera>().enabled = false;
			MeshCombineJobManager.instance.camGeometryCapture = gameObject.AddComponent<CamGeometryCapture>();
			MeshCombineJobManager.instance.camGeometryCapture.computeDepthToArray = meshCombiner.computeDepthToArray;
			MeshCombineJobManager.instance.camGeometryCapture.Init();
			return MeshCombineJobManager.instance;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004328 File Offset: 0x00002528
		public static void ResetMeshCache()
		{
			if (MeshCombineJobManager.instance)
			{
				MeshCombineJobManager.instance.meshCacheDictionary.Clear();
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004345 File Offset: 0x00002545
		private void Awake()
		{
			MeshCombineJobManager.instance = this;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000434D File Offset: 0x0000254D
		private void OnEnable()
		{
			MeshCombineJobManager.instance = this;
			base.gameObject.hideFlags = (HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
			this.Init();
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004368 File Offset: 0x00002568
		public void Init()
		{
			this.cores = Environment.ProcessorCount;
			if (this.meshCombineJobsThreads == null || this.meshCombineJobsThreads.Length != this.cores)
			{
				this.meshCombineJobsThreads = new MeshCombineJobManager.MeshCombineJobsThread[this.cores];
				for (int i = 0; i < this.meshCombineJobsThreads.Length; i++)
				{
					this.meshCombineJobsThreads[i] = new MeshCombineJobManager.MeshCombineJobsThread(i);
				}
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000043CA File Offset: 0x000025CA
		private void OnDisable()
		{
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000043CC File Offset: 0x000025CC
		private void OnDestroy()
		{
			this.AbortJobs();
			if (MeshCombineJobManager.instance == this)
			{
				MeshCombineJobManager.instance = null;
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000043E7 File Offset: 0x000025E7
		private void Update()
		{
			if (Application.isPlaying)
			{
				this.MyUpdate();
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000043F6 File Offset: 0x000025F6
		private void MyUpdate()
		{
			this.ExecuteJobs();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004400 File Offset: 0x00002600
		public void SetJobMode(MeshCombineJobManager.JobSettings newJobSettings)
		{
			if (newJobSettings.combineMeshesPerFrame < 1)
			{
				Debug.LogError("(MeshCombineStudio) => CombineMeshesPerFrame is " + newJobSettings.combineMeshesPerFrame.ToString() + " and should be 1 or higher.");
				return;
			}
			if (newJobSettings.combineMeshesPerFrame > 128)
			{
				Debug.LogError("(MeshCombineStudio) => CombineMeshesPerFrame is " + newJobSettings.combineMeshesPerFrame.ToString() + " and should be 128 or lower.");
				return;
			}
			if (newJobSettings.customThreadAmount < 1)
			{
				Debug.LogError("(MeshCombineStudio) => customThreadAmount is " + newJobSettings.combineMeshesPerFrame.ToString() + " and should be 1 or higher.");
				return;
			}
			if (newJobSettings.customThreadAmount > this.cores)
			{
				newJobSettings.customThreadAmount = this.cores;
			}
			this.jobSettings.CopySettings(newJobSettings);
			if (this.jobSettings.useMultiThreading)
			{
				this.startThreadId = (this.jobSettings.useMainThread ? 0 : 1);
				if (this.jobSettings.threadAmountMode == MeshCombineJobManager.ThreadAmountMode.Custom)
				{
					if (this.jobSettings.customThreadAmount > this.cores - this.startThreadId)
					{
						this.jobSettings.customThreadAmount = this.cores - this.startThreadId;
					}
					this.threadAmount = this.jobSettings.customThreadAmount;
				}
				else
				{
					if (this.jobSettings.threadAmountMode == MeshCombineJobManager.ThreadAmountMode.AllThreads)
					{
						this.threadAmount = this.cores;
					}
					else
					{
						this.threadAmount = this.cores / 2;
					}
					this.threadAmount -= this.startThreadId;
				}
				this.endThreadId = this.startThreadId + this.threadAmount;
			}
			else
			{
				this.startThreadId = 0;
				this.endThreadId = 1;
				this.threadAmount = 1;
			}
			int combineMeshesPerFrame;
			if (this.jobSettings.combineJobMode == MeshCombineJobManager.CombineJobMode.CombinePerFrame)
			{
				combineMeshesPerFrame = this.jobSettings.combineMeshesPerFrame;
			}
			else
			{
				combineMeshesPerFrame = this.threadAmount;
			}
			while (this.newMeshObjectsPool.Count > combineMeshesPerFrame)
			{
				this.newMeshObjectsPool.RemoveLast();
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000045D0 File Offset: 0x000027D0
		public void AddJob(MeshCombiner meshCombiner, MeshObjectsHolder meshObjectsHolder, Transform parent, Vector3 position)
		{
			FastList<MeshObject> meshObjects = meshObjectsHolder.meshObjects;
			if (meshObjects.Count == 0)
			{
				return;
			}
			if (meshObjects.Count < 2 && meshObjects.items[0].cachedGO.mr.sharedMaterials.Length == 1 && !meshCombiner.removeTrianglesBelowSurface && !meshCombiner.removeOverlappingTriangles && !meshCombiner.removeBackFaceTriangles)
			{
				if (meshCombiner.excludeSingleMeshes)
				{
					for (int i = 0; i < meshObjects.Count; i++)
					{
						meshObjects.items[i].cachedGO.excludeCombine = true;
					}
					meshCombiner.originalDrawCalls++;
					meshCombiner.newDrawCalls++;
					return;
				}
				if (meshObjects.Count == 1 && meshObjectsHolder.lodParent.lodLevels.Length == 1)
				{
					MeshObject meshObject = meshObjects.items[0];
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(meshCombiner.instantiatePrefab, meshObject.position, meshObject.rotation, parent);
					gameObject.transform.localScale = meshObject.cachedGO.t.lossyScale;
					Mesh sharedMesh = meshObject.cachedGO.mf.sharedMesh;
					gameObject.name = "SingleMesh " + sharedMesh.name;
					CachedComponents component = gameObject.GetComponent<CachedComponents>();
					component.mf.sharedMesh = sharedMesh;
					MeshRenderer mr = component.mr;
					MeshRenderer mr2 = meshObject.cachedGO.mr;
					mr.sharedMaterials = mr2.sharedMaterials;
					mr.lightmapScaleOffset = mr2.lightmapScaleOffset;
					mr.lightmapIndex = mr2.lightmapIndex;
					meshObjectsHolder.combineCondition.WriteToGameObject(gameObject, mr2);
					if (meshCombiner.copyBakedLighting)
					{
						LightmapSettings lightmapSettings = gameObject.AddComponent<LightmapSettings>();
						lightmapSettings.mr = mr;
						lightmapSettings.lightmapIndex = mr2.lightmapIndex;
						lightmapSettings.setLightmapScaleOffset = true;
						lightmapSettings.lightmapScaleOffset = mr2.lightmapScaleOffset;
					}
					return;
				}
			}
			int num = 0;
			int num2 = 0;
			int startIndex = 0;
			int num3 = 0;
			bool firstMesh = true;
			bool intersectsSurface = false;
			Mesh y = null;
			MeshCache meshCache = null;
			int num4 = meshCombiner.useVertexOutputLimit ? meshCombiner.vertexOutputLimit : 64000;
			int j = 0;
			while (j < meshObjects.Count)
			{
				MeshObject meshObject2 = meshObjects.items[j];
				meshObject2.skip = false;
				meshCombiner.originalDrawCalls++;
				Mesh mesh = meshObject2.cachedGO.mesh;
				if (mesh != y && !this.meshCacheDictionary.TryGetValue(mesh, out meshCache))
				{
					meshCache = new MeshCache(mesh);
					this.meshCacheDictionary.Add(mesh, meshCache);
				}
				y = mesh;
				meshObject2.meshCache = meshCache;
				int vertexCount = meshCache.subMeshCache[meshObject2.subMeshIndex].vertexCount;
				int triangleCount = meshCache.subMeshCache[meshObject2.subMeshIndex].triangleCount;
				meshCombiner.originalTotalVertices += vertexCount;
				meshCombiner.originalTotalTriangles += triangleCount;
				if (num + vertexCount > num4)
				{
					MeshCombineJobManager.MeshCombineJob meshCombineJob = new MeshCombineJobManager.MeshCombineJob(meshCombiner, meshObjectsHolder, parent, position, startIndex, num3, firstMesh, intersectsSurface);
					this.EnqueueJob(meshCombiner, meshCombineJob);
					intersectsSurface = (firstMesh = false);
					num2 = (num = (num3 = 0));
					startIndex = j;
				}
				if (meshCombiner.removeOverlappingTriangles)
				{
					meshObject2.startNewTriangleIndex = num2;
					meshObject2.newTriangleCount = triangleCount;
				}
				if (!meshCombiner.removeTrianglesBelowSurface)
				{
					goto IL_37E;
				}
				int num5 = 0;
				if (!meshCombiner.noColliders)
				{
					num5 = this.MeshIntersectsSurface(meshCombiner, meshObject2.cachedGO);
				}
				meshObject2.startNewTriangleIndex = num2;
				meshObject2.newTriangleCount = triangleCount;
				if (num5 == 0)
				{
					intersectsSurface = (meshObject2.intersectsSurface = true);
					meshObject2.skip = false;
					goto IL_37E;
				}
				meshObject2.intersectsSurface = false;
				if (num5 != -1)
				{
					meshObject2.skip = false;
					goto IL_37E;
				}
				meshObject2.skip = true;
				num3++;
				IL_38E:
				j++;
				continue;
				IL_37E:
				num += vertexCount;
				num2 += triangleCount;
				num3++;
				goto IL_38E;
			}
			if (num > 0)
			{
				MeshCombineJobManager.MeshCombineJob meshCombineJob2 = new MeshCombineJobManager.MeshCombineJob(meshCombiner, meshObjectsHolder, parent, position, startIndex, num3, firstMesh, intersectsSurface);
				this.EnqueueJob(meshCombiner, meshCombineJob2);
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000499E File Offset: 0x00002B9E
		private void EnqueueJob(MeshCombiner meshCombiner, MeshCombineJobManager.MeshCombineJob meshCombineJob)
		{
			meshCombiner.meshCombineJobs.Add(meshCombineJob);
			meshCombiner.totalMeshCombineJobs++;
			this.meshCombineJobs.Enqueue(meshCombineJob);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000049C8 File Offset: 0x00002BC8
		public int MeshIntersectsSurface(MeshCombiner meshCombiner, CachedGameObject cachedGO)
		{
			MeshRenderer mr = cachedGO.mr;
			LayerMask surfaceLayerMask = meshCombiner.surfaceLayerMask;
			float maxSurfaceHeight = meshCombiner.maxSurfaceHeight;
			if (Physics.CheckBox(mr.bounds.center, mr.bounds.extents, Quaternion.identity, surfaceLayerMask))
			{
				return 0;
			}
			Vector3 min = mr.bounds.min;
			float maxDistance = meshCombiner.maxSurfaceHeight - min.y;
			this.ray.origin = new Vector3(min.x, maxSurfaceHeight, min.z);
			if (Physics.Raycast(this.ray, out this.hitInfo, maxDistance, surfaceLayerMask) && min.y < this.hitInfo.point.y)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004A90 File Offset: 0x00002C90
		public void AbortJobs()
		{
			foreach (MeshCombineJobManager.MeshCombineJob meshCombineJob in this.meshCombineJobs)
			{
				meshCombineJob.meshCombiner.ClearMeshCombineJobs(true);
			}
			this.meshCombineJobs.Clear();
			for (int i = 0; i < this.meshCombineJobsThreads.Length; i++)
			{
				MeshCombineJobManager.MeshCombineJobsThread meshCombineJobsThread = this.meshCombineJobsThreads[i];
				Queue<MeshCombineJobManager.MeshCombineJob> obj = meshCombineJobsThread.meshCombineJobs;
				lock (obj)
				{
					foreach (MeshCombineJobManager.MeshCombineJob meshCombineJob2 in meshCombineJobsThread.meshCombineJobs)
					{
						meshCombineJob2.meshCombiner.ClearMeshCombineJobs(true);
					}
					meshCombineJobsThread.meshCombineJobs.Clear();
				}
			}
			this.totalNewMeshObjects = 0;
			this.abort = true;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004B98 File Offset: 0x00002D98
		public void ExecuteJobs()
		{
			while (this.meshCombineJobs.Count > 0)
			{
				int num = 999999;
				int num2 = 0;
				for (int i = this.startThreadId; i < this.endThreadId; i++)
				{
					int count = this.meshCombineJobsThreads[i].meshCombineJobs.Count;
					if (count < num)
					{
						num2 = i;
						num = count;
						if (num == 0)
						{
							break;
						}
					}
				}
				Queue<MeshCombineJobManager.MeshCombineJob> obj = this.meshCombineJobsThreads[num2].meshCombineJobs;
				lock (obj)
				{
					MeshCombineJobManager.MeshCombineJob meshCombineJob = this.meshCombineJobs.Dequeue();
					if (!meshCombineJob.abort)
					{
						this.meshCombineJobsThreads[num2].meshCombineJobs.Enqueue(meshCombineJob);
					}
				}
			}
			try
			{
				for (;;)
				{
					bool flag2 = false;
					if (this.jobSettings.useMultiThreading)
					{
						for (int j = 1; j < this.endThreadId; j++)
						{
							MeshCombineJobManager.MeshCombineJobsThread meshCombineJobsThread = this.meshCombineJobsThreads[j];
							if (meshCombineJobsThread.meshCombineJobs.Count > 0)
							{
								flag2 = true;
								if (meshCombineJobsThread.threadState == MeshCombineJobManager.ThreadState.isFree)
								{
									if (MeshCombineJobManager.instance.jobSettings.combineJobMode == MeshCombineJobManager.CombineJobMode.CombinePerFrame && MeshCombineJobManager.instance.totalNewMeshObjects + 1 > MeshCombineJobManager.instance.jobSettings.combineMeshesPerFrame)
									{
										break;
									}
									meshCombineJobsThread.threadState = MeshCombineJobManager.ThreadState.isRunning;
									ThreadPool.QueueUserWorkItem(new WaitCallback(meshCombineJobsThread.ExecuteJobsThread));
								}
								if (meshCombineJobsThread.threadState == MeshCombineJobManager.ThreadState.hasError)
								{
									goto Block_12;
								}
							}
						}
						for (int k = 1; k < this.endThreadId; k++)
						{
							if (this.meshCombineJobsThreads[k].threadState == MeshCombineJobManager.ThreadState.isReady)
							{
								this.CombineMeshesDone(this.meshCombineJobsThreads[k]);
							}
						}
					}
					if (!this.jobSettings.useMultiThreading || this.jobSettings.useMainThread)
					{
						MeshCombineJobManager.MeshCombineJobsThread meshCombineJobsThread2 = this.meshCombineJobsThreads[0];
						if (meshCombineJobsThread2.meshCombineJobs.Count > 0)
						{
							flag2 = true;
							meshCombineJobsThread2.threadState = MeshCombineJobManager.ThreadState.isRunning;
							meshCombineJobsThread2.ExecuteJobsThread(null);
							if (meshCombineJobsThread2.threadState == MeshCombineJobManager.ThreadState.isReady)
							{
								this.CombineMeshesDone(meshCombineJobsThread2);
							}
						}
					}
					if (this.jobSettings.combineJobMode != MeshCombineJobManager.CombineJobMode.CombineAtOnce || !flag2)
					{
						goto IL_1FB;
					}
				}
				Block_12:
				this.AbortJobs();
				IL_1FB:;
			}
			catch (Exception ex)
			{
				Debug.LogError("(MeshCombineStudio) => " + ex.ToString());
				this.AbortJobs();
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004DF8 File Offset: 0x00002FF8
		public void CombineMeshesDone(MeshCombineJobManager.MeshCombineJobsThread meshCombineJobThread)
		{
			Queue<MeshCombineJobManager.NewMeshObject> newMeshObjectsDone = meshCombineJobThread.newMeshObjectsDone;
			int num = 0;
			while (newMeshObjectsDone.Count > 0)
			{
				MeshCombineJobManager.NewMeshObject newMeshObject = newMeshObjectsDone.Dequeue();
				MeshCombiner meshCombiner = newMeshObject.meshCombineJob.meshCombiner;
				if (!this.abort && !newMeshObject.meshCombineJob.abort)
				{
					meshCombiner.meshCombineJobs.Remove(newMeshObject.meshCombineJob);
					try
					{
						if (!newMeshObject.allSkipped)
						{
							newMeshObject.CreateMesh();
						}
						if (meshCombiner.meshCombineJobs.Count == 0)
						{
							if (meshCombiner.addMeshColliders)
							{
								meshCombiner.AddMeshColliders();
							}
							meshCombiner.ExecuteOnCombiningReady();
						}
					}
					catch (Exception ex)
					{
						Debug.LogError("(MeshCombineStudio) => " + ex.ToString());
						MeshCombineJobManager.instance.AbortJobs();
					}
				}
				FastList<MeshCombineJobManager.NewMeshObject> obj = this.newMeshObjectsPool;
				lock (obj)
				{
					this.newMeshObjectsPool.Add(newMeshObject);
				}
				Interlocked.Decrement(ref this.totalNewMeshObjects);
				if (this.jobSettings.combineJobMode == MeshCombineJobManager.CombineJobMode.CombinePerFrame && ++num > this.jobSettings.combineMeshesPerFrame && !this.abort)
				{
					break;
				}
			}
			meshCombineJobThread.threadState = MeshCombineJobManager.ThreadState.isFree;
			this.abort = false;
		}

		// Token: 0x0400004E RID: 78
		public static MeshCombineJobManager instance;

		// Token: 0x0400004F RID: 79
		public MeshCombineJobManager.JobSettings jobSettings = new MeshCombineJobManager.JobSettings();

		// Token: 0x04000050 RID: 80
		[NonSerialized]
		public FastList<MeshCombineJobManager.NewMeshObject> newMeshObjectsPool = new FastList<MeshCombineJobManager.NewMeshObject>();

		// Token: 0x04000051 RID: 81
		public Dictionary<Mesh, MeshCache> meshCacheDictionary = new Dictionary<Mesh, MeshCache>();

		// Token: 0x04000052 RID: 82
		[NonSerialized]
		public int totalNewMeshObjects;

		// Token: 0x04000053 RID: 83
		public Queue<MeshCombineJobManager.MeshCombineJob> meshCombineJobs = new Queue<MeshCombineJobManager.MeshCombineJob>();

		// Token: 0x04000054 RID: 84
		public MeshCombineJobManager.MeshCombineJobsThread[] meshCombineJobsThreads;

		// Token: 0x04000055 RID: 85
		public CamGeometryCapture camGeometryCapture;

		// Token: 0x04000056 RID: 86
		public int cores;

		// Token: 0x04000057 RID: 87
		public int threadAmount;

		// Token: 0x04000058 RID: 88
		public int startThreadId;

		// Token: 0x04000059 RID: 89
		public int endThreadId;

		// Token: 0x0400005A RID: 90
		public bool abort;

		// Token: 0x0400005B RID: 91
		private MeshCache.SubMeshCache tempMeshCache;

		// Token: 0x0400005C RID: 92
		private Ray ray = new Ray(Vector3.zero, Vector3.down);

		// Token: 0x0400005D RID: 93
		private RaycastHit hitInfo;

		// Token: 0x0200005C RID: 92
		[Serializable]
		public class JobSettings
		{
			// Token: 0x060001C0 RID: 448 RVA: 0x0000F588 File Offset: 0x0000D788
			public void CopySettings(MeshCombineJobManager.JobSettings source)
			{
				this.combineJobMode = source.combineJobMode;
				this.threadAmountMode = source.threadAmountMode;
				this.combineMeshesPerFrame = source.combineMeshesPerFrame;
				this.useMultiThreading = source.useMultiThreading;
				this.useMainThread = source.useMainThread;
				this.customThreadAmount = source.customThreadAmount;
			}

			// Token: 0x060001C1 RID: 449 RVA: 0x0000F5E0 File Offset: 0x0000D7E0
			public void ReportStatus()
			{
				Debug.Log("---------------------");
				Debug.Log("combineJobMode " + this.combineJobMode.ToString());
				Debug.Log("threadAmountMode " + this.threadAmountMode.ToString());
				Debug.Log("combineMeshesPerFrame " + this.combineMeshesPerFrame.ToString());
				Debug.Log("useMultiThreading " + this.useMultiThreading.ToString());
				Debug.Log("useMainThread " + this.useMainThread.ToString());
				Debug.Log("customThreadAmount " + this.customThreadAmount.ToString());
			}

			// Token: 0x04000239 RID: 569
			public MeshCombineJobManager.CombineJobMode combineJobMode;

			// Token: 0x0400023A RID: 570
			public MeshCombineJobManager.ThreadAmountMode threadAmountMode;

			// Token: 0x0400023B RID: 571
			public int combineMeshesPerFrame = 4;

			// Token: 0x0400023C RID: 572
			public bool useMultiThreading = true;

			// Token: 0x0400023D RID: 573
			public bool useMainThread = true;

			// Token: 0x0400023E RID: 574
			public int customThreadAmount = 1;

			// Token: 0x0400023F RID: 575
			public bool showStats;
		}

		// Token: 0x0200005D RID: 93
		public enum CombineJobMode
		{
			// Token: 0x04000241 RID: 577
			CombineAtOnce,
			// Token: 0x04000242 RID: 578
			CombinePerFrame
		}

		// Token: 0x0200005E RID: 94
		public enum ThreadAmountMode
		{
			// Token: 0x04000244 RID: 580
			AllThreads,
			// Token: 0x04000245 RID: 581
			HalfThreads,
			// Token: 0x04000246 RID: 582
			Custom
		}

		// Token: 0x0200005F RID: 95
		public enum ThreadState
		{
			// Token: 0x04000248 RID: 584
			isFree,
			// Token: 0x04000249 RID: 585
			isReady,
			// Token: 0x0400024A RID: 586
			isRunning,
			// Token: 0x0400024B RID: 587
			hasError
		}

		// Token: 0x02000060 RID: 96
		public class MeshCombineJobsThread
		{
			// Token: 0x060001C3 RID: 451 RVA: 0x0000F6C3 File Offset: 0x0000D8C3
			public MeshCombineJobsThread(int threadId)
			{
				this.threadId = threadId;
			}

			// Token: 0x060001C4 RID: 452 RVA: 0x0000F6E8 File Offset: 0x0000D8E8
			public void ExecuteJobsThread(object state)
			{
				MeshCombineJobManager.NewMeshObject newMeshObject = null;
				try
				{
					newMeshObject = null;
					Queue<MeshCombineJobManager.MeshCombineJob> obj = this.meshCombineJobs;
					MeshCombineJobManager.MeshCombineJob meshCombineJob;
					lock (obj)
					{
						meshCombineJob = this.meshCombineJobs.Dequeue();
					}
					Interlocked.Increment(ref MeshCombineJobManager.instance.totalNewMeshObjects);
					FastList<MeshCombineJobManager.NewMeshObject> newMeshObjectsPool = MeshCombineJobManager.instance.newMeshObjectsPool;
					lock (newMeshObjectsPool)
					{
						if (MeshCombineJobManager.instance.newMeshObjectsPool.Count == 0)
						{
							newMeshObject = new MeshCombineJobManager.NewMeshObject();
						}
						else
						{
							newMeshObject = MeshCombineJobManager.instance.newMeshObjectsPool.Dequeue();
						}
					}
					newMeshObject.newPosition = meshCombineJob.position;
					newMeshObject.Combine(meshCombineJob);
					Queue<MeshCombineJobManager.NewMeshObject> obj2 = this.newMeshObjectsDone;
					lock (obj2)
					{
						this.newMeshObjectsDone.Enqueue(newMeshObject);
					}
					this.threadState = MeshCombineJobManager.ThreadState.isReady;
				}
				catch (Exception ex)
				{
					if (newMeshObject != null)
					{
						FastList<MeshCombineJobManager.NewMeshObject> newMeshObjectsPool = MeshCombineJobManager.instance.newMeshObjectsPool;
						lock (newMeshObjectsPool)
						{
							MeshCombineJobManager.instance.newMeshObjectsPool.Add(newMeshObject);
						}
						Interlocked.Decrement(ref MeshCombineJobManager.instance.totalNewMeshObjects);
					}
					Queue<MeshCombineJobManager.MeshCombineJob> obj = this.meshCombineJobs;
					lock (obj)
					{
						this.meshCombineJobs.Clear();
					}
					Debug.LogError("(MeshCombineStudio) => Mesh Combine Studio thread error -> " + ex.ToString());
					this.threadState = MeshCombineJobManager.ThreadState.hasError;
				}
			}

			// Token: 0x0400024C RID: 588
			public int threadId;

			// Token: 0x0400024D RID: 589
			public MeshCombineJobManager.ThreadState threadState;

			// Token: 0x0400024E RID: 590
			public Queue<MeshCombineJobManager.MeshCombineJob> meshCombineJobs = new Queue<MeshCombineJobManager.MeshCombineJob>();

			// Token: 0x0400024F RID: 591
			public Queue<MeshCombineJobManager.NewMeshObject> newMeshObjectsDone = new Queue<MeshCombineJobManager.NewMeshObject>();
		}

		// Token: 0x02000061 RID: 97
		public class MeshCombineJob
		{
			// Token: 0x060001C5 RID: 453 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
			public MeshCombineJob(MeshCombiner meshCombiner, MeshObjectsHolder meshObjectsHolder, Transform parent, Vector3 position, int startIndex, int length, bool firstMesh, bool intersectsSurface)
			{
				this.meshCombiner = meshCombiner;
				this.meshObjectsHolder = meshObjectsHolder;
				this.parent = parent;
				this.position = position;
				this.startIndex = startIndex;
				this.firstMesh = firstMesh;
				this.intersectsSurface = intersectsSurface;
				this.endIndex = startIndex + length;
				meshObjectsHolder.lodParent.jobsPending++;
				this.name = this.GetHashCode().ToString();
			}

			// Token: 0x04000250 RID: 592
			public MeshCombiner meshCombiner;

			// Token: 0x04000251 RID: 593
			public MeshObjectsHolder meshObjectsHolder;

			// Token: 0x04000252 RID: 594
			public Transform parent;

			// Token: 0x04000253 RID: 595
			public Vector3 position;

			// Token: 0x04000254 RID: 596
			public int startIndex;

			// Token: 0x04000255 RID: 597
			public int endIndex;

			// Token: 0x04000256 RID: 598
			public bool firstMesh;

			// Token: 0x04000257 RID: 599
			public bool intersectsSurface;

			// Token: 0x04000258 RID: 600
			public int backFaceTrianglesRemoved;

			// Token: 0x04000259 RID: 601
			public int trianglesRemoved;

			// Token: 0x0400025A RID: 602
			public bool abort;

			// Token: 0x0400025B RID: 603
			public string name;
		}

		// Token: 0x02000062 RID: 98
		public class NewMeshObject
		{
			// Token: 0x060001C6 RID: 454 RVA: 0x0000F922 File Offset: 0x0000DB22
			public NewMeshObject()
			{
				this.newMeshCache.Init(true);
			}

			// Token: 0x060001C7 RID: 455 RVA: 0x0000F944 File Offset: 0x0000DB44
			public void Combine(MeshCombineJobManager.MeshCombineJob meshCombineJob)
			{
				BitArray bitArray = new BitArray(this.newMeshCache.triangles.Length, false);
				this.meshCombineJob = meshCombineJob;
				if (meshCombineJob.abort)
				{
					return;
				}
				int startIndex = meshCombineJob.startIndex;
				int endIndex = meshCombineJob.endIndex;
				FastList<MeshObject> meshObjects = meshCombineJob.meshObjectsHolder.meshObjects;
				this.newMeshCache.ResetHasBooleans();
				int num = 0;
				int num2 = 0;
				int num3 = endIndex - startIndex;
				MeshCombiner meshCombiner = meshCombineJob.meshCombiner;
				CombineMode combineMode = meshCombiner.combineMode;
				bool validCopyBakedLighting = meshCombiner.validCopyBakedLighting;
				bool validRebakeLighting = meshCombiner.validRebakeLighting;
				bool flag = meshCombiner.rebakeLightingMode == MeshCombiner.RebakeLightingMode.RegenarateLightmapUvs;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				float num7 = 0f;
				if (validRebakeLighting)
				{
					num4 = Mathf.CeilToInt(Mathf.Sqrt((float)num3));
					num7 = 1f / (float)num4;
				}
				this.allSkipped = true;
				for (int i = startIndex; i < endIndex; i++)
				{
					MeshObject meshObject = meshObjects.items[i];
					int subMeshIndex = meshObject.subMeshIndex;
					MeshCache.SubMeshCache subMeshCache = meshObject.meshCache.subMeshCache[subMeshIndex];
					int vertexCount = subMeshCache.vertexCount;
					this.HasArray<Vector3>(ref this.newMeshCache.hasNormals, subMeshCache.hasNormals, ref this.newMeshCache.normals, subMeshCache.normals, vertexCount, num, false, default(Vector3));
					this.HasArray<Vector4>(ref this.newMeshCache.hasTangents, subMeshCache.hasTangents, ref this.newMeshCache.tangents, subMeshCache.tangents, vertexCount, num, true, new Vector4(1f, 1f, 1f, 1f));
					this.HasArray<Vector2>(ref this.newMeshCache.hasUv, subMeshCache.hasUv, ref this.newMeshCache.uv, subMeshCache.uv, vertexCount, num, false, default(Vector2));
					this.HasArray<Vector2>(ref this.newMeshCache.hasUv2, subMeshCache.hasUv2, ref this.newMeshCache.uv2, subMeshCache.uv2, vertexCount, num, false, default(Vector2));
					this.HasArray<Vector2>(ref this.newMeshCache.hasUv3, subMeshCache.hasUv3, ref this.newMeshCache.uv3, subMeshCache.uv3, vertexCount, num, false, default(Vector2));
					this.HasArray<Vector2>(ref this.newMeshCache.hasUv4, subMeshCache.hasUv4, ref this.newMeshCache.uv4, subMeshCache.uv4, vertexCount, num, false, default(Vector2));
					this.HasArray<Color32>(ref this.newMeshCache.hasColors, subMeshCache.hasColors, ref this.newMeshCache.colors32, subMeshCache.colors32, vertexCount, num, true, new Color32(1, 1, 1, 1));
					num += vertexCount;
				}
				num = 0;
				for (int j = startIndex; j < endIndex; j++)
				{
					MeshObject meshObject2 = meshObjects.items[j];
					if (!meshObject2.skip)
					{
						bool flag2 = meshCombiner.useExcludeBackfaceRemovalTag && !string.IsNullOrEmpty(meshCombiner.excludeBackfaceRemovalTag) && meshObject2.cachedGO.go.CompareTag(meshCombiner.excludeBackfaceRemovalTag);
						this.allSkipped = false;
						MeshCache meshCache = meshObject2.meshCache;
						int subMeshIndex2 = meshObject2.subMeshIndex;
						MeshCache.SubMeshCache subMeshCache2 = meshCache.subMeshCache[subMeshIndex2];
						Vector3 scale = meshObject2.scale;
						bool flag3 = false;
						if (scale.x < 0f)
						{
							flag3 = !flag3;
						}
						if (scale.y < 0f)
						{
							flag3 = !flag3;
						}
						if (scale.z < 0f)
						{
							flag3 = !flag3;
						}
						int num8 = 1;
						if (flag3)
						{
							num8 = -1;
						}
						Vector3[] vertices = subMeshCache2.vertices;
						Vector3[] normals = subMeshCache2.normals;
						Vector4[] tangents = subMeshCache2.tangents;
						Vector2[] uv = subMeshCache2.uv;
						Vector2[] uv2 = subMeshCache2.uv2;
						Vector2[] uv3 = subMeshCache2.uv3;
						Vector2[] uv4 = subMeshCache2.uv4;
						Color32[] colors = subMeshCache2.colors32;
						int[] triangles = subMeshCache2.triangles;
						int vertexCount2 = subMeshCache2.vertexCount;
						int[] triangles2 = this.newMeshCache.triangles;
						Vector3[] vertices2 = this.newMeshCache.vertices;
						Vector3[] normals2 = this.newMeshCache.normals;
						Vector4[] tangents2 = this.newMeshCache.tangents;
						Vector2[] uv5 = this.newMeshCache.uv;
						Vector2[] uv6 = this.newMeshCache.uv2;
						Vector2[] uv7 = this.newMeshCache.uv3;
						Vector2[] uv8 = this.newMeshCache.uv4;
						Color32[] colors2 = this.newMeshCache.colors32;
						bool hasNormals = subMeshCache2.hasNormals;
						bool hasTangents = subMeshCache2.hasTangents;
						Vector3 position = meshCombineJob.position;
						Matrix4x4 mt = meshObject2.cachedGO.mt;
						Matrix4x4 mtNormals = meshObject2.cachedGO.mtNormals;
						if (combineMode == CombineMode.DynamicObjects)
						{
							Vector3 rootTLossyScale = meshObject2.cachedGO.rootTLossyScale;
							rootTLossyScale.x = 1f / rootTLossyScale.x;
							rootTLossyScale.y = 1f / rootTLossyScale.y;
							rootTLossyScale.z = 1f / rootTLossyScale.z;
							for (int k = 0; k < vertices.Length; k++)
							{
								int num9 = k + num;
								vertices2[num9] = Vector3.Scale(mt.MultiplyPoint3x4(vertices[k]) - position, rootTLossyScale);
							}
						}
						else
						{
							for (int l = 0; l < vertices.Length; l++)
							{
								int num10 = l + num;
								vertices2[num10] = mt.MultiplyPoint3x4(vertices[l]) - position;
							}
						}
						if (hasNormals)
						{
							meshCombiner.originalTotalNormalChannels++;
							for (int m = 0; m < vertices.Length; m++)
							{
								int num11 = m + num;
								normals2[num11] = mtNormals.MultiplyVector(normals[m]);
							}
						}
						if (hasTangents)
						{
							meshCombiner.originalTotalTangentChannels++;
							for (int n = 0; n < vertices.Length; n++)
							{
								int num12 = n + num;
								tangents2[num12] = mt.MultiplyVector(tangents[n]);
								tangents2[num12].w = tangents[n].w * (float)num8;
							}
						}
						if (subMeshCache2.hasUv)
						{
							meshCombiner.originalTotalUvChannels++;
							Array.Copy(uv, 0, uv5, num, vertexCount2);
						}
						if (subMeshCache2.hasUv2)
						{
							meshCombiner.originalTotalUv2Channels++;
							if (validCopyBakedLighting)
							{
								Vector4 lightmapScaleOffset = meshObject2.lightmapScaleOffset;
								Vector2 b = new Vector2(lightmapScaleOffset.z, lightmapScaleOffset.w);
								Vector2 vector = new Vector2(lightmapScaleOffset.x, lightmapScaleOffset.y);
								for (int num13 = 0; num13 < vertices.Length; num13++)
								{
									int num14 = num13 + num;
									uv6[num14] = new Vector2(uv2[num13].x * vector.x, uv2[num13].y * vector.y) + b;
								}
							}
							else if (validRebakeLighting)
							{
								if (!flag)
								{
									Vector2 b2 = new Vector2(num7 * (float)num5, num7 * (float)num6);
									for (int num15 = 0; num15 < vertices.Length; num15++)
									{
										int num16 = num15 + num;
										uv6[num16] = uv2[num15] * num7 + b2;
									}
								}
							}
							else
							{
								Array.Copy(uv2, 0, uv6, num, vertexCount2);
							}
						}
						if (subMeshCache2.hasUv3)
						{
							meshCombiner.originalTotalUv3Channels++;
							Array.Copy(uv3, 0, uv7, num, vertexCount2);
						}
						if (subMeshCache2.hasUv4)
						{
							meshCombiner.originalTotalUv4Channels++;
							Array.Copy(uv4, 0, uv8, num, vertexCount2);
						}
						if (subMeshCache2.hasColors)
						{
							meshCombiner.originalTotalColorChannels++;
							Array.Copy(colors, 0, colors2, num, vertexCount2);
						}
						else if (this.newMeshCache.hasColors)
						{
							int num17 = num + vertexCount2;
							for (int num18 = num; num18 < num17; num18++)
							{
								colors2[num18] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
							}
						}
						if (flag3)
						{
							for (int num19 = 0; num19 < triangles.Length; num19 += 3)
							{
								triangles2[num19 + num2] = triangles[num19 + 2] + num;
								triangles2[num19 + num2 + 1] = triangles[num19 + 1] + num;
								triangles2[num19 + num2 + 2] = triangles[num19] + num;
								if (flag2)
								{
									bitArray[num19 + num2] = true;
									bitArray[num19 + num2 + 1] = true;
									bitArray[num19 + num2 + 2] = true;
								}
							}
						}
						else
						{
							for (int num20 = 0; num20 < triangles.Length; num20++)
							{
								triangles2[num20 + num2] = triangles[num20] + num;
								if (flag2)
								{
									bitArray[num20 + num2] = true;
								}
							}
						}
						num += vertexCount2;
						num2 += triangles.Length;
						if (++num5 >= num4)
						{
							num5 = 0;
							num6++;
						}
					}
				}
				this.newMeshCache.vertexCount = num;
				this.newMeshCache.triangleCount = num2;
				if (meshCombiner.removeBackFaceTriangles)
				{
					this.RemoveBackFaceTriangles(bitArray);
				}
			}

			// Token: 0x060001C8 RID: 456 RVA: 0x00010248 File Offset: 0x0000E448
			private void PrintMissingArrayWarning(MeshCombiner meshCombiner, GameObject go, Mesh mesh, string text)
			{
				Debug.Log(string.Concat(new string[]
				{
					"(MeshCombineStudio) => GameObject: ",
					go.name,
					" Mesh ",
					mesh.name,
					" has missing ",
					text,
					" while the other meshes have them. Click the 'Select Meshes in Project' button to change the import settings."
				}));
				meshCombiner.selectImportSettingsMeshes.Add(mesh);
			}

			// Token: 0x060001C9 RID: 457 RVA: 0x000102AC File Offset: 0x0000E4AC
			private void HasArray<T>(ref bool hasNewArray, bool hasArray, ref T[] newArray, Array array, int vertexCount, int totalVertices, bool useDefaultValue = false, T defaultValue = default(T))
			{
				if (hasArray)
				{
					if (!hasNewArray)
					{
						if (newArray == null)
						{
							newArray = new T[65534];
							if (useDefaultValue)
							{
								this.FillArray<T>(newArray, 0, totalVertices, defaultValue);
							}
						}
						else if (useDefaultValue)
						{
							this.FillArray<T>(newArray, 0, totalVertices, defaultValue);
						}
						else
						{
							Array.Clear(newArray, 0, totalVertices);
						}
					}
					hasNewArray = true;
					return;
				}
				if (hasNewArray)
				{
					if (useDefaultValue)
					{
						this.FillArray<T>(newArray, totalVertices, vertexCount, defaultValue);
						return;
					}
					Array.Clear(newArray, totalVertices, vertexCount);
				}
			}

			// Token: 0x060001CA RID: 458 RVA: 0x00010328 File Offset: 0x0000E528
			private void FillArray<T>(T[] array, int offset, int length, T value)
			{
				length += offset;
				for (int i = offset; i < length; i++)
				{
					array[i] = value;
				}
			}

			// Token: 0x060001CB RID: 459 RVA: 0x00010350 File Offset: 0x0000E550
			public void RemoveTrianglesBelowSurface(Transform t, MeshCombineJobManager.MeshCombineJob meshCombineJob)
			{
				if (this.vertexIsBelow == null)
				{
					this.vertexIsBelow = new byte[65534];
				}
				Ray ray = MeshCombineJobManager.instance.ray;
				RaycastHit hitInfo = MeshCombineJobManager.instance.hitInfo;
				Vector3 vector = Vector3.zero;
				int layerMask = meshCombineJob.meshCombiner.surfaceLayerMask;
				float maxSurfaceHeight = meshCombineJob.meshCombiner.maxSurfaceHeight;
				Vector3[] vertices = this.newMeshCache.vertices;
				int[] triangles = this.newMeshCache.triangles;
				FastList<MeshObject> meshObjects = meshCombineJob.meshObjectsHolder.meshObjects;
				int startIndex = meshCombineJob.startIndex;
				int endIndex = meshCombineJob.endIndex;
				for (int i = startIndex; i < endIndex; i++)
				{
					MeshObject meshObject = meshObjects.items[i];
					if (meshObject.intersectsSurface)
					{
						int startNewTriangleIndex = meshObject.startNewTriangleIndex;
						int num = meshObject.newTriangleCount + startNewTriangleIndex;
						for (int j = startNewTriangleIndex; j < num; j += 3)
						{
							bool flag = false;
							for (int k = 0; k < 3; k++)
							{
								int num2 = triangles[j + k];
								if (num2 != -1)
								{
									byte b = this.vertexIsBelow[num2];
									if (b == 0)
									{
										vector = t.TransformPoint(vertices[num2]);
										ray.origin = new Vector3(vector.x, maxSurfaceHeight, vector.z);
										if (!Physics.Raycast(ray, out hitInfo, maxSurfaceHeight - vector.y, layerMask))
										{
											this.vertexIsBelow[num2] = 2;
											flag = true;
											break;
										}
										if (vector.y >= hitInfo.point.y)
										{
											this.vertexIsBelow[num2] = 2;
											break;
										}
										b = (this.vertexIsBelow[num2] = 1);
									}
									if (b != 1)
									{
										flag = true;
										break;
									}
								}
							}
							if (!flag)
							{
								meshCombineJob.trianglesRemoved += 3;
								triangles[j] = -1;
							}
						}
					}
				}
				Array.Clear(this.vertexIsBelow, 0, vertices.Length);
			}

			// Token: 0x060001CC RID: 460 RVA: 0x0001052C File Offset: 0x0000E72C
			public void RemoveBackFaceTriangles(BitArray backfaceRemovalExclusions)
			{
				int[] triangles = this.newMeshCache.triangles;
				Vector3[] normals = this.newMeshCache.normals;
				int triangleCount = this.newMeshCache.triangleCount;
				MeshCombiner meshCombiner = this.meshCombineJob.meshCombiner;
				bool flag = meshCombiner.backFaceTriangleMode == MeshCombiner.BackFaceTriangleMode.Box;
				Bounds backFaceBounds = meshCombiner.backFaceBounds;
				Vector3 min = backFaceBounds.min;
				Vector3 max = backFaceBounds.max;
				Vector3[] vertices = this.newMeshCache.vertices;
				Vector3 lhs;
				if (meshCombiner.backFaceTriangleMode == MeshCombiner.BackFaceTriangleMode.EulerAngles)
				{
					lhs = Quaternion.Euler(meshCombiner.backFaceRotation) * Vector3.forward;
				}
				else
				{
					lhs = meshCombiner.backFaceDirection;
				}
				for (int i = 0; i < triangleCount; i += 3)
				{
					Vector3 vector = Vector3.zero;
					Vector3 vector2 = Vector3.zero;
					if (!backfaceRemovalExclusions[i])
					{
						for (int j = 0; j < 3; j++)
						{
							int num = triangles[i + j];
							vector2 += vertices[num];
							vector += normals[num];
						}
						vector2 /= 3f;
						vector /= 3f;
						if (flag)
						{
							Vector3 b;
							b.x = ((vector.x > 0f) ? max.x : min.x);
							b.y = ((vector.y > 0f) ? max.y : min.y);
							b.z = ((vector.z > 0f) ? max.z : min.z);
							lhs = this.newPosition + vector2 - b;
						}
						if (Vector3.Dot(lhs, vector) >= 0f)
						{
							triangles[i] = -1;
							this.meshCombineJob.backFaceTrianglesRemoved += 3;
						}
					}
				}
			}

			// Token: 0x060001CD RID: 461 RVA: 0x00010704 File Offset: 0x0000E904
			public void WeldVertices(MeshCombineJobManager.MeshCombineJob meshCombineJob)
			{
				if (MeshCombineJobManager.NewMeshObject.weldVertices == null)
				{
					MeshCombineJobManager.NewMeshObject.weldVertices = new FastList<Vector3>(65534);
				}
				else
				{
					MeshCombineJobManager.NewMeshObject.weldVertices.FastClear();
				}
				Vector3[] vertices = this.newMeshCache.vertices;
				int vertexCount = this.newMeshCache.vertexCount;
				int[] array = new int[vertexCount];
				Dictionary<Vector3, int> dictionary = new Dictionary<Vector3, int>();
				if (meshCombineJob.meshCombiner.weldSnapVertices)
				{
					float num = meshCombineJob.meshCombiner.weldSnapSize;
					if (num < 1E-05f)
					{
						num = 1E-05f;
					}
					for (int i = 0; i < vertexCount; i++)
					{
						Vector3 vector = Mathw.SnapRound(vertices[i], num);
						int num2;
						if (dictionary.TryGetValue(vector, out num2))
						{
							array[i] = num2;
						}
						else
						{
							dictionary[vector] = (array[i] = MeshCombineJobManager.NewMeshObject.weldVertices.Count);
							MeshCombineJobManager.NewMeshObject.weldVertices.Add(vector);
						}
					}
				}
				else
				{
					for (int j = 0; j < vertexCount; j++)
					{
						Vector3 vector2 = vertices[j];
						int num2;
						if (dictionary.TryGetValue(vector2, out num2))
						{
							array[j] = num2;
						}
						else
						{
							dictionary[vector2] = (array[j] = MeshCombineJobManager.NewMeshObject.weldVertices.Count);
							MeshCombineJobManager.NewMeshObject.weldVertices.Add(vector2);
						}
					}
				}
				int[] triangles = this.newMeshCache.triangles;
				int triangleCount = this.newMeshCache.triangleCount;
				for (int k = 0; k < triangleCount; k++)
				{
					if (triangles[k] != -1)
					{
						triangles[k] = array[triangles[k]];
					}
				}
				Array.Copy(MeshCombineJobManager.NewMeshObject.weldVertices.items, this.newMeshCache.vertices, MeshCombineJobManager.NewMeshObject.weldVertices.Count);
				this.newMeshCache.vertexCount = MeshCombineJobManager.NewMeshObject.weldVertices.Count;
			}

			// Token: 0x060001CE RID: 462 RVA: 0x000108B0 File Offset: 0x0000EAB0
			private void ArrangeTriangles()
			{
				int num = this.newMeshCache.triangleCount;
				int[] triangles = this.newMeshCache.triangles;
				for (int i = 0; i < num; i += 3)
				{
					if (triangles[i] == -1)
					{
						triangles[i] = triangles[num - 3];
						triangles[i + 1] = triangles[num - 2];
						triangles[i + 2] = triangles[num - 1];
						i -= 3;
						num -= 3;
					}
				}
				this.newMeshCache.triangleCount = num;
			}

			// Token: 0x060001CF RID: 463 RVA: 0x00010918 File Offset: 0x0000EB18
			public void CreateMesh()
			{
				MeshCombiner meshCombiner = this.meshCombineJob.meshCombiner;
				if (meshCombiner.instantiatePrefab == null)
				{
					Debug.LogError("(MeshCombineStudio) => Instantiate Prefab = null");
					return;
				}
				CombineMode combineMode = meshCombiner.combineMode;
				MeshObjectsHolder meshObjectsHolder = this.meshCombineJob.meshObjectsHolder;
				if (combineMode == CombineMode.DynamicObjects)
				{
					this.meshCombineJob.parent = this.meshCombineJob.meshObjectsHolder.meshObjects.items[0].cachedGO.rootT;
				}
				else if (this.meshCombineJob.parent == null)
				{
					this.meshCombineJob.parent = this.meshCombineJob.meshCombiner.transform;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(meshCombiner.instantiatePrefab, this.newPosition, Quaternion.identity, this.meshCombineJob.parent);
				meshCombiner.data.combinedGameObjects.Add(gameObject);
				CachedComponents component = gameObject.GetComponent<CachedComponents>();
				MeshRenderer mr = component.mr;
				MeshFilter mf = component.mf;
				string name = (combineMode == CombineMode.DynamicObjects) ? "CombinedMesh" : meshObjectsHolder.mat.name;
				gameObject.name = name;
				if (this.meshCombineJob.intersectsSurface)
				{
					if (meshCombiner.noColliders)
					{
						MeshCombineJobManager.instance.camGeometryCapture.RemoveTrianglesBelowSurface(gameObject.transform, this.meshCombineJob, this.newMeshCache, ref this.vertexIsBelow);
					}
					else
					{
						this.RemoveTrianglesBelowSurface(gameObject.transform, this.meshCombineJob);
					}
				}
				if (meshCombiner.weldVertices)
				{
					this.WeldVertices(this.meshCombineJob);
				}
				if (this.meshCombineJob.trianglesRemoved > 0 || this.meshCombineJob.backFaceTrianglesRemoved > 0 || meshCombiner.weldVertices)
				{
					this.ArrangeTriangles();
					if (MeshCombineJobManager.instance.tempMeshCache == null)
					{
						MeshCombineJobManager.instance.tempMeshCache = new MeshCache.SubMeshCache();
						MeshCombineJobManager.instance.tempMeshCache.Init(false);
					}
					MeshCombineJobManager.instance.tempMeshCache.CopySubMeshCache(this.newMeshCache);
					this.newMeshCache.RebuildVertexBuffer(MeshCombineJobManager.instance.tempMeshCache, false);
				}
				int vertexCount = this.newMeshCache.vertexCount;
				int triangleCount = this.newMeshCache.triangleCount;
				if (vertexCount == 0)
				{
					Methods.Destroy(gameObject);
					return;
				}
				Mesh mesh = new Mesh();
				mesh.name = name;
				meshCombiner.newTotalVertices += vertexCount;
				meshCombiner.newTotalTriangles += triangleCount;
				MeshExtension.ApplyVertices(mesh, this.newMeshCache.vertices, vertexCount);
				MeshExtension.ApplyTriangles(mesh, this.newMeshCache.triangles, triangleCount);
				if (meshCombiner.weldVertices)
				{
					if (this.newMeshCache.hasNormals && meshCombiner.weldIncludeNormals)
					{
						mesh.RecalculateNormals();
					}
				}
				else
				{
					if (this.newMeshCache.hasNormals)
					{
						meshCombiner.newTotalNormalChannels++;
						MeshExtension.ApplyNormals(mesh, this.newMeshCache.normals, vertexCount);
					}
					if (this.newMeshCache.hasTangents)
					{
						meshCombiner.newTotalTangentChannels++;
						MeshExtension.ApplyTangents(mesh, this.newMeshCache.tangents, vertexCount);
					}
					if (this.newMeshCache.hasUv)
					{
						meshCombiner.newTotalUvChannels++;
						MeshExtension.ApplyUvs(mesh, this.newMeshCache.uv, 0, vertexCount);
					}
					if (this.newMeshCache.hasUv2)
					{
						meshCombiner.newTotalUv2Channels++;
						MeshExtension.ApplyUvs(mesh, this.newMeshCache.uv2, 1, vertexCount);
					}
					if (this.newMeshCache.hasUv3)
					{
						meshCombiner.newTotalUv3Channels++;
						MeshExtension.ApplyUvs(mesh, this.newMeshCache.uv3, 2, vertexCount);
					}
					if (this.newMeshCache.hasUv4)
					{
						meshCombiner.newTotalUv4Channels++;
						MeshExtension.ApplyUvs(mesh, this.newMeshCache.uv4, 3, vertexCount);
					}
					if (this.newMeshCache.hasColors)
					{
						meshCombiner.newTotalColorChannels++;
						MeshExtension.ApplyColors32(mesh, this.newMeshCache.colors32, vertexCount);
					}
				}
				if (meshCombiner.addMeshColliders)
				{
					bool flag = true;
					if (meshCombiner.addMeshCollidersInRange && !meshCombiner.addMeshCollidersBounds.Contains(gameObject.transform.position))
					{
						flag = false;
					}
					if (flag)
					{
						meshCombiner.addMeshCollidersList.Add(new MeshColliderAdd(gameObject, mesh));
					}
				}
				if (meshCombiner.makeMeshesUnreadable)
				{
					mesh.UploadMeshData(true);
				}
				meshCombiner.newDrawCalls++;
				mr.sharedMaterial = meshObjectsHolder.mat;
				mf.sharedMesh = mesh;
				component.garbageCollectMesh.mesh = mesh;
				meshObjectsHolder.combineCondition.WriteToGameObject(gameObject, mr);
				if (meshObjectsHolder.newCachedGOs == null)
				{
					meshObjectsHolder.newCachedGOs = new FastList<CachedGameObject>();
				}
				meshObjectsHolder.newCachedGOs.Add(new CachedGameObject(component));
				meshObjectsHolder.lodParent.lodLevels[meshObjectsHolder.lodLevel].newMeshRenderers.Add(mr);
				ObjectOctree.LODParent lodParent = meshObjectsHolder.lodParent;
				int num = lodParent.jobsPending - 1;
				lodParent.jobsPending = num;
				if (num == 0 && meshObjectsHolder.lodParent.lodLevels.Length > 1)
				{
					meshObjectsHolder.lodParent.AssignLODGroup(meshCombiner);
				}
			}

			// Token: 0x0400025C RID: 604
			public static FastList<Vector3> weldVertices;

			// Token: 0x0400025D RID: 605
			public MeshCombineJobManager.MeshCombineJob meshCombineJob;

			// Token: 0x0400025E RID: 606
			public MeshCache.SubMeshCache newMeshCache = new MeshCache.SubMeshCache();

			// Token: 0x0400025F RID: 607
			public bool allSkipped;

			// Token: 0x04000260 RID: 608
			public Vector3 newPosition;

			// Token: 0x04000261 RID: 609
			private byte[] vertexIsBelow;

			// Token: 0x04000262 RID: 610
			private const byte belowSurface = 1;

			// Token: 0x04000263 RID: 611
			private const byte aboveSurface = 2;
		}
	}
}
