using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Pathfinding;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.Graphs.Util;
using Pathfinding.Jobs;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000004 RID: 4
[ExecuteInEditMode]
[AddComponentMenu("Pathfinding/AstarPath")]
[DisallowMultipleComponent]
[HelpURL("https://arongranberg.com/astar/documentation/stable/astarpath.html")]
public class AstarPath : VersionedMonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
	public NavGraph[] graphs
	{
		get
		{
			return this.data.graphs;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000004 RID: 4 RVA: 0x00002065 File Offset: 0x00000265
	public float maxNearestNodeDistanceSqr
	{
		get
		{
			return this.maxNearestNodeDistance * this.maxNearestNodeDistance;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000005 RID: 5 RVA: 0x00002074 File Offset: 0x00000274
	// (set) Token: 0x06000006 RID: 6 RVA: 0x0000207C File Offset: 0x0000027C
	public float lastScanTime { get; private set; }

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000007 RID: 7 RVA: 0x00002085 File Offset: 0x00000285
	// (set) Token: 0x06000008 RID: 8 RVA: 0x0000208D File Offset: 0x0000028D
	public bool isScanning { get; private set; }

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000009 RID: 9 RVA: 0x00002096 File Offset: 0x00000296
	public int NumParallelThreads
	{
		get
		{
			return this.pathProcessor.NumThreads;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600000A RID: 10 RVA: 0x000020A3 File Offset: 0x000002A3
	public bool IsUsingMultithreading
	{
		get
		{
			return this.pathProcessor.IsUsingMultithreading;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600000B RID: 11 RVA: 0x000020B0 File Offset: 0x000002B0
	public bool IsAnyGraphUpdateQueued
	{
		get
		{
			return this.graphUpdates.IsAnyGraphUpdateQueued;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600000C RID: 12 RVA: 0x000020BD File Offset: 0x000002BD
	public bool IsAnyGraphUpdateInProgress
	{
		get
		{
			return this.graphUpdates.IsAnyGraphUpdateInProgress;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600000D RID: 13 RVA: 0x000020CA File Offset: 0x000002CA
	public bool IsAnyWorkItemInProgress
	{
		get
		{
			return this.workItems.workItemsInProgress;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600000E RID: 14 RVA: 0x000020D7 File Offset: 0x000002D7
	internal bool IsInsideWorkItem
	{
		get
		{
			return this.workItems.workItemsInProgressRightNow;
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000020E4 File Offset: 0x000002E4
	private AstarPath()
	{
		this.pathReturnQueue = new PathReturnQueue(this, delegate()
		{
			if (AstarPath.OnPathsCalculated != null)
			{
				AstarPath.OnPathsCalculated();
			}
		});
		this.nodeStorage = new GlobalNodeStorage(this);
		this.hierarchicalGraph = new HierarchicalGraph(this.nodeStorage);
		this.pathProcessor = new PathProcessor(this, this.pathReturnQueue, 1, false);
		this.offMeshLinks = new OffMeshLinks(this);
		this.workItems = new WorkItemProcessor(this);
		this.graphUpdates = new GraphUpdateProcessor(this);
		this.navmeshUpdates.astar = this;
		this.data = new AstarData(this);
		this.workItems.OnGraphsUpdated += delegate()
		{
			if (AstarPath.OnGraphsUpdated != null)
			{
				try
				{
					AstarPath.OnGraphsUpdated(this);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
		};
		this.pathProcessor.OnPathPreSearch += delegate(Path path)
		{
			OnPathDelegate onPathPreSearch = AstarPath.OnPathPreSearch;
			if (onPathPreSearch != null)
			{
				onPathPreSearch(path);
			}
		};
		this.pathProcessor.OnPathPostSearch += delegate(Path path)
		{
			this.LogPathResults(path);
			OnPathDelegate onPathPostSearch = AstarPath.OnPathPostSearch;
			if (onPathPostSearch != null)
			{
				onPathPostSearch(path);
			}
		};
		this.pathProcessor.OnQueueUnblocked += delegate()
		{
			if (this.euclideanEmbedding.dirty)
			{
				this.euclideanEmbedding.RecalculateCosts();
			}
		};
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000022A8 File Offset: 0x000004A8
	public string[] GetTagNames()
	{
		if (this.tagNames == null || this.tagNames.Length != 32)
		{
			this.tagNames = new string[32];
			for (int i = 0; i < this.tagNames.Length; i++)
			{
				this.tagNames[i] = (i.ToString() ?? "");
			}
			this.tagNames[0] = "Basic Ground";
		}
		return this.tagNames;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002314 File Offset: 0x00000514
	public static void FindAstarPath()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (AstarPath.active == null)
		{
			AstarPath.active = UnityCompatibility.FindAnyObjectByType<AstarPath>();
		}
		if (AstarPath.active != null && (AstarPath.active.data.graphs == null || AstarPath.active.data.graphs.Length == 0))
		{
			AstarPath.active.data.DeserializeGraphs();
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x0000237F File Offset: 0x0000057F
	public static string[] FindTagNames()
	{
		AstarPath.FindAstarPath();
		if (!(AstarPath.active != null))
		{
			return new string[]
			{
				"There is no AstarPath component in the scene"
			};
		}
		return AstarPath.active.GetTagNames();
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000023AC File Offset: 0x000005AC
	internal ushort GetNextPathID()
	{
		if (this.nextFreePathID == 0)
		{
			this.nextFreePathID += 1;
			if (AstarPath.On65KOverflow != null)
			{
				Action on65KOverflow = AstarPath.On65KOverflow;
				AstarPath.On65KOverflow = null;
				on65KOverflow();
			}
		}
		ushort num = this.nextFreePathID;
		this.nextFreePathID = num + 1;
		return num;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000023F9 File Offset: 0x000005F9
	private void RecalculateDebugLimits()
	{
		this.debugFloor = 0f;
		this.debugRoof = 1f;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002414 File Offset: 0x00000614
	public override void DrawGizmos()
	{
		if (AstarPath.active != this || this.graphs == null)
		{
			return;
		}
		this.InitializeColors();
		if (!this.redrawScope.isValid)
		{
			this.redrawScope = DrawingManager.GetRedrawScope(base.gameObject);
		}
		if (!this.workItems.workItemsInProgress && !this.isScanning)
		{
			this.redrawScope.Rewind();
			if (this.showNavGraphs && !this.manualDebugFloorRoof)
			{
				this.RecalculateDebugLimits();
			}
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null && this.graphs[i].drawGizmos)
				{
					this.graphs[i].OnDrawGizmos(DrawingManager.instance.gizmos, this.showNavGraphs, this.redrawScope);
				}
			}
			if (this.showNavGraphs)
			{
				this.euclideanEmbedding.OnDrawGizmos();
				if (this.debugMode == GraphDebugMode.HierarchicalNode)
				{
					this.hierarchicalGraph.OnDrawGizmos(DrawingManager.instance.gizmos, this.redrawScope);
				}
				if (this.debugMode == GraphDebugMode.NavmeshBorderObstacles)
				{
					this.hierarchicalGraph.navmeshEdges.OnDrawGizmos(DrawingManager.instance.gizmos, this.redrawScope);
				}
			}
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002548 File Offset: 0x00000748
	private void OnGUI()
	{
		if (this.logPathResults == PathLog.InGame && this.inGameDebugPath != "")
		{
			GUI.Label(new Rect(5f, 5f, 400f, 600f), this.inGameDebugPath);
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002594 File Offset: 0x00000794
	private void LogPathResults(Path path)
	{
		if (this.logPathResults != PathLog.None && (path.error || this.logPathResults != PathLog.OnlyErrors))
		{
			string message = ((IPathInternals)path).DebugString(this.logPathResults);
			if (this.logPathResults == PathLog.InGame)
			{
				this.inGameDebugPath = message;
				return;
			}
			if (path.error)
			{
				UnityEngine.Debug.LogWarning(message);
				return;
			}
			UnityEngine.Debug.Log(message);
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000025F0 File Offset: 0x000007F0
	private void Update()
	{
		this.navmeshUpdates.Update();
		if (!Application.isPlaying)
		{
			return;
		}
		if (!this.isScanning)
		{
			this.PerformBlockingActions(false);
		}
		if (!this.pathProcessor.IsUsingMultithreading)
		{
			this.pathProcessor.TickNonMultithreaded();
		}
		this.pathReturnQueue.ReturnPaths(true);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002644 File Offset: 0x00000844
	private void PerformBlockingActions(bool force = false)
	{
		if (this.workItemLock.Held && this.pathProcessor.queue.allReceiversBlocked)
		{
			this.pathReturnQueue.ReturnPaths(false);
			if (this.workItems.ProcessWorkItemsForUpdate(force))
			{
				this.workItemLock.Release();
			}
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002695 File Offset: 0x00000895
	public void AddWorkItem(Action callback)
	{
		this.AddWorkItem(new AstarWorkItem(callback, null));
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000026A4 File Offset: 0x000008A4
	public void AddWorkItem(Action<IWorkItemContext> callback)
	{
		this.AddWorkItem(new AstarWorkItem(callback, null));
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000026B3 File Offset: 0x000008B3
	public void AddWorkItem(AstarWorkItem item)
	{
		this.workItems.AddWorkItem(item);
		if (!this.workItemLock.Held)
		{
			this.workItemLock = this.PausePathfindingSoon();
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000026DC File Offset: 0x000008DC
	public void QueueGraphUpdates()
	{
		if (!this.graphUpdatesWorkItemAdded)
		{
			this.graphUpdatesWorkItemAdded = true;
			AstarWorkItem workItem = this.graphUpdates.GetWorkItem();
			this.AddWorkItem(new AstarWorkItem(delegate(IWorkItemContext context)
			{
				this.graphUpdatesWorkItemAdded = false;
				this.lastGraphUpdate = Time.realtimeSinceStartup;
				workItem.initWithContext(context);
			}, workItem.updateWithContext));
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002738 File Offset: 0x00000938
	private IEnumerator DelayedGraphUpdate()
	{
		this.graphUpdateRoutineRunning = true;
		yield return new WaitForSeconds(this.graphUpdateBatchingInterval - (Time.realtimeSinceStartup - this.lastGraphUpdate));
		this.QueueGraphUpdates();
		this.graphUpdateRoutineRunning = false;
		yield break;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002747 File Offset: 0x00000947
	public void UpdateGraphs(Bounds bounds, float delay)
	{
		this.UpdateGraphs(new GraphUpdateObject(bounds), delay);
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002756 File Offset: 0x00000956
	public void UpdateGraphs(GraphUpdateObject ob, float delay)
	{
		base.StartCoroutine(this.UpdateGraphsInternal(ob, delay));
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002767 File Offset: 0x00000967
	private IEnumerator UpdateGraphsInternal(GraphUpdateObject ob, float delay)
	{
		yield return new WaitForSeconds(delay);
		this.UpdateGraphs(ob);
		yield break;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002784 File Offset: 0x00000984
	public void UpdateGraphs(Bounds bounds)
	{
		this.UpdateGraphs(new GraphUpdateObject(bounds));
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002794 File Offset: 0x00000994
	public void UpdateGraphs(GraphUpdateObject ob)
	{
		if (ob.internalStage != -1)
		{
			throw new Exception("You are trying to update graphs using the same graph update object twice. Please create a new GraphUpdateObject instead.");
		}
		ob.internalStage = -2;
		this.graphUpdates.AddToQueue(ob);
		if (this.batchGraphUpdates && Time.realtimeSinceStartup - this.lastGraphUpdate < this.graphUpdateBatchingInterval)
		{
			if (!this.graphUpdateRoutineRunning)
			{
				base.StartCoroutine(this.DelayedGraphUpdate());
				return;
			}
		}
		else
		{
			this.QueueGraphUpdates();
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002801 File Offset: 0x00000A01
	public void FlushGraphUpdates()
	{
		if (this.IsAnyGraphUpdateQueued || this.IsAnyGraphUpdateInProgress)
		{
			this.QueueGraphUpdates();
			this.FlushWorkItems();
		}
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002820 File Offset: 0x00000A20
	public void FlushWorkItems()
	{
		if (this.workItems.anyQueued || this.workItems.workItemsInProgress)
		{
			if (AstarPath.active != this)
			{
				throw new Exception("This AstarPath component is not initialized in a scene. Are you trying to add work items to a prefab or a disabled AstarPath component?");
			}
			using (this.PausePathfinding())
			{
				this.PerformBlockingActions(true);
			}
		}
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002890 File Offset: 0x00000A90
	public static int CalculateThreadCount(ThreadCount count)
	{
		if (count != ThreadCount.AutomaticLowLoad && count != ThreadCount.AutomaticHighLoad)
		{
			return (int)count;
		}
		int num = Mathf.Max(1, SystemInfo.processorCount);
		int num2 = SystemInfo.systemMemorySize;
		if (num2 <= 0)
		{
			UnityEngine.Debug.LogError("Machine reporting that is has <= 0 bytes of RAM. This is definitely not true, assuming 1 GiB");
			num2 = 1024;
		}
		if (num <= 1)
		{
			return 0;
		}
		if (num2 <= 512)
		{
			return 0;
		}
		if (count == ThreadCount.AutomaticHighLoad)
		{
			if (num2 <= 1024)
			{
				num = Math.Min(num, 2);
			}
		}
		else
		{
			num /= 2;
			num = Mathf.Max(1, num);
			if (num2 <= 1024)
			{
				num = Math.Min(num, 2);
			}
			num = Math.Min(num, 6);
		}
		return num;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x0000291C File Offset: 0x00000B1C
	private void InitializePathProcessor()
	{
		int num = AstarPath.CalculateThreadCount(this.threadCount);
		if (!Application.isPlaying)
		{
			num = 0;
		}
		int processors = Mathf.Max(num, 1);
		bool multithreaded = num > 0;
		this.pathProcessor.StopThreads();
		this.pathProcessor.SetThreadCount(processors, multithreaded);
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002963 File Offset: 0x00000B63
	private void InitializeColors()
	{
		this.colorSettings = (this.colorSettings ?? new AstarColor());
		this.colorSettings.PushToStatic();
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002988 File Offset: 0x00000B88
	private void ShutdownPathfindingThreads()
	{
		PathProcessor.GraphUpdateLock graphUpdateLock = this.PausePathfinding();
		this.navmeshUpdates.OnDisable();
		this.euclideanEmbedding.dirty = false;
		this.graphUpdates.DiscardQueued();
		this.FlushWorkItems();
		if (this.logPathResults == PathLog.Heavy)
		{
			UnityEngine.Debug.Log("Processing Possible Work Items");
		}
		this.pathProcessor.StopThreads();
		if (this.logPathResults == PathLog.Heavy)
		{
			UnityEngine.Debug.Log("Returning Paths");
		}
		this.pathReturnQueue.ReturnPaths(false);
		graphUpdateLock.Release();
		this.euclideanEmbedding.OnDisable();
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002A14 File Offset: 0x00000C14
	private void OnEnable()
	{
		if (AstarPath.active != null)
		{
			if (AstarPath.active != this && Application.isPlaying)
			{
				if (base.enabled)
				{
					UnityEngine.Debug.LogWarning("Another A* component is already in the scene. More than one A* component cannot be active at the same time. Disabling this one.", this);
				}
				base.enabled = false;
			}
			return;
		}
		AstarPath.active = this;
		base.useGUILayout = false;
		if (AstarPath.OnAwakeSettings != null)
		{
			AstarPath.OnAwakeSettings();
		}
		this.hierarchicalGraph.OnEnable();
		GraphModifier.FindAllModifiers();
		RelevantGraphSurface.FindAllGraphSurfaces();
		this.InitializeColors();
		this.navmeshUpdates.OnEnable();
		this.data.OnEnable();
		this.FlushWorkItems();
		this.euclideanEmbedding.dirty = true;
		this.InitializePathProcessor();
		if (Application.isPlaying && this.scanOnStartup && !this.hasScannedGraphAtStartup && (!this.data.cacheStartup || this.data.file_cachedStartup == null))
		{
			this.hasScannedGraphAtStartup = true;
			this.Scan(null);
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002B0C File Offset: 0x00000D0C
	private void OnDisable()
	{
		this.redrawScope.Dispose();
		if (AstarPath.active == this)
		{
			if (this.asyncScanTask != null)
			{
				UnityEngine.Debug.LogWarning("An async scan was running when the AstarPath component was disabled. Blocking until the async scan is complete.", this);
				this.BlockUntilAsyncScanComplete();
			}
			this.graphDataLock.WriteSync().Unlock();
			this.ShutdownPathfindingThreads();
			this.data.DestroyAllNodes();
			this.data.DisposeUnmanagedData();
			this.hierarchicalGraph.OnDisable();
			this.nodeStorage.OnDisable();
			this.offMeshLinks.OnDisable();
			AstarPath.active = null;
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002BA0 File Offset: 0x00000DA0
	private void OnDestroy()
	{
		if (this.logPathResults == PathLog.Heavy)
		{
			UnityEngine.Debug.Log("AstarPath Component Destroyed - Cleaning Up Pathfinding Data");
		}
		AstarPath astarPath = AstarPath.active;
		AstarPath.active = this;
		this.ShutdownPathfindingThreads();
		this.pathProcessor.Dispose();
		if (this.logPathResults == PathLog.Heavy)
		{
			UnityEngine.Debug.Log("Destroying Graphs");
		}
		if (this.data != null)
		{
			this.data.OnDestroy();
		}
		AstarPath.active = astarPath;
		if (this.logPathResults == PathLog.Heavy)
		{
			UnityEngine.Debug.Log("Cleaning up variables");
		}
		if (AstarPath.active == this)
		{
			AstarPath.OnAwakeSettings = null;
			AstarPath.OnGraphPreScan = null;
			AstarPath.OnGraphPostScan = null;
			AstarPath.OnPathPreSearch = null;
			AstarPath.OnPathPostSearch = null;
			AstarPath.OnPreScan = null;
			AstarPath.OnPostScan = null;
			AstarPath.OnLatePostScan = null;
			AstarPath.On65KOverflow = null;
			AstarPath.OnGraphsUpdated = null;
			AstarPath.active = null;
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002C69 File Offset: 0x00000E69
	public JobHandle AllocateNodes<T>(T[] result, int count, Func<T> createNode, uint variantsPerNode) where T : GraphNode
	{
		if (!this.pathProcessor.queue.allReceiversBlocked)
		{
			throw new Exception("Trying to initialize a node when it is not safe to initialize any nodes. Must be done during a graph update. See http://arongranberg.com/astar/docs/graph-updates.html#direct");
		}
		return this.nodeStorage.AllocateNodesJob<T>(result, count, createNode, variantsPerNode);
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002C98 File Offset: 0x00000E98
	internal void InitializeNode(GraphNode node)
	{
		if (!this.pathProcessor.queue.allReceiversBlocked)
		{
			throw new Exception("Trying to initialize a node when it is not safe to initialize any nodes. Must be done during a graph update. See http://arongranberg.com/astar/docs/graph-updates.html#direct");
		}
		this.nodeStorage.InitializeNode(node);
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002CC4 File Offset: 0x00000EC4
	internal void InitializeNodes(GraphNode[] nodes)
	{
		if (!this.pathProcessor.queue.allReceiversBlocked)
		{
			throw new Exception("Trying to initialize a node when it is not safe to initialize any nodes. Must be done during a graph update. See http://arongranberg.com/astar/docs/graph-updates.html#direct");
		}
		for (int i = 0; i < nodes.Length; i++)
		{
			this.nodeStorage.InitializeNode(nodes[i]);
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002D0A File Offset: 0x00000F0A
	internal void DestroyNode(GraphNode node)
	{
		this.nodeStorage.DestroyNode(node);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002D18 File Offset: 0x00000F18
	public PathProcessor.GraphUpdateLock PausePathfinding()
	{
		this.graphDataLock.WriteSync().Unlock();
		return this.pathProcessor.PausePathfinding(true);
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002D44 File Offset: 0x00000F44
	public PathProcessor.GraphUpdateLock PausePathfindingSoon()
	{
		return this.pathProcessor.PausePathfinding(false);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002D52 File Offset: 0x00000F52
	private void BlockUntilAsyncScanComplete()
	{
		while (this.asyncScanTask != null && this.asyncScanTask.MoveNext())
		{
		}
		this.asyncScanTask = null;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002D70 File Offset: 0x00000F70
	public void Scan(NavGraph graphToScan)
	{
		if (graphToScan == null)
		{
			throw new ArgumentNullException();
		}
		this.Scan(new NavGraph[]
		{
			graphToScan
		});
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002D8C File Offset: 0x00000F8C
	public void Scan(NavGraph[] graphsToScan = null)
	{
		ScanningStage scanningStage = (ScanningStage)(-1);
		if (this.asyncScanTask != null)
		{
			UnityEngine.Debug.LogWarning("An async scan was already running when a new scan was requested. Blocking until it is complete. You can check if a scan is currently in progress using the AstarPath.active.isScanning property.", this);
			this.BlockUntilAsyncScanComplete();
		}
		foreach (Progress progress in this.ScanInternal(graphsToScan, false))
		{
			if (scanningStage != progress.stage)
			{
				scanningStage = progress.stage;
			}
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002E00 File Offset: 0x00001000
	public IEnumerable<Progress> ScanAsync(NavGraph graphToScan)
	{
		if (graphToScan == null)
		{
			throw new ArgumentNullException();
		}
		return this.ScanAsync(new NavGraph[]
		{
			graphToScan
		});
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002E1C File Offset: 0x0000101C
	public IEnumerable<Progress> ScanAsync(NavGraph[] graphsToScan = null)
	{
		if (this.asyncScanTask != null)
		{
			UnityEngine.Debug.LogWarning("An async scan was already running when a new async scan was requested. Blocking until the previous one is complete. You can check if a scan is currently in progress using the AstarPath.active.isScanning property.", this);
			this.BlockUntilAsyncScanComplete();
		}
		this.asyncScanTask = this.ScanInternal(graphsToScan, true).GetEnumerator();
		try
		{
			this.asyncScanTask.MoveNext();
		}
		catch
		{
			this.asyncScanTask = null;
			throw;
		}
		return this.TickAsyncScanUntilCompletion(this.asyncScanTask);
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002E8C File Offset: 0x0000108C
	private IEnumerable<Progress> TickAsyncScanUntilCompletion(IEnumerator<Progress> task)
	{
		for (;;)
		{
			try
			{
				if (!task.MoveNext())
				{
					break;
				}
			}
			catch
			{
				if (this.asyncScanTask == task)
				{
					this.asyncScanTask = null;
				}
				throw;
			}
			yield return task.Current;
		}
		if (this.asyncScanTask == task)
		{
			this.asyncScanTask = null;
		}
		yield break;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002EA3 File Offset: 0x000010A3
	private IEnumerable<Progress> ScanInternal(NavGraph[] graphsToScan, bool async)
	{
		if (graphsToScan == null)
		{
			graphsToScan = this.graphs;
		}
		if (graphsToScan == null || graphsToScan.Length == 0)
		{
			yield break;
		}
		if (!base.enabled)
		{
			throw new InvalidOperationException("The AstarPath object must be enabled to scan graphs");
		}
		if (AstarPath.active != this)
		{
			throw new InvalidOperationException("The AstarPath object is not enabled in a scene");
		}
		this.isScanning = true;
		PathProcessor.GraphUpdateLock graphUpdateLock = this.PausePathfinding();
		this.pathReturnQueue.ReturnPaths(false);
		this.workItems.ProcessWorkItemsForScan(true);
		if (!Application.isPlaying)
		{
			this.data.FindGraphTypes();
			GraphModifier.FindAllModifiers();
		}
		yield return new Progress(0.05f, ScanningStage.PreProcessingGraphs, 0, 0);
		using (this.graphDataLock.WriteSync())
		{
			try
			{
				if (AstarPath.OnPreScan != null)
				{
					AstarPath.OnPreScan(this);
				}
				GraphModifier.TriggerEvent(GraphModifier.EventType.PreScan);
				GraphModifier.TriggerEvent(GraphModifier.EventType.PreUpdate);
			}
			catch
			{
				this.isScanning = false;
				graphUpdateLock.Release();
				throw;
			}
		}
		this.data.LockGraphStructure(false);
		Physics.SyncTransforms();
		Physics2D.SyncTransforms();
		Stopwatch watch = Stopwatch.StartNew();
		if (!async)
		{
			using (this.graphDataLock.WriteSync())
			{
				for (int i = 0; i < graphsToScan.Length; i++)
				{
					if (graphsToScan[i] != null)
					{
						((IGraphInternals)graphsToScan[i]).DestroyAllNodes();
					}
				}
			}
		}
		if (AstarPath.OnGraphPreScan != null)
		{
			using (this.graphDataLock.WriteSync())
			{
				try
				{
					for (int j = 0; j < graphsToScan.Length; j++)
					{
						if (graphsToScan[j] != null)
						{
							AstarPath.OnGraphPreScan(graphsToScan[j]);
						}
					}
				}
				catch
				{
					this.isScanning = false;
					this.data.UnlockGraphStructure();
					graphUpdateLock.Release();
					throw;
				}
			}
		}
		List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> promises = new List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>>(graphsToScan.Length);
		for (int k = 0; k < graphsToScan.Length; k++)
		{
			if (graphsToScan[k] != null)
			{
				IGraphUpdatePromise graphUpdatePromise;
				if ((graphUpdatePromise = ((IGraphInternals)graphsToScan[k]).ScanInternal(async)) == null)
				{
					(graphUpdatePromise = new AstarPath.DestroyGraphPromise()).graph = graphsToScan[k];
				}
				IGraphUpdatePromise graphUpdatePromise2 = graphUpdatePromise;
				IEnumerator<JobHandle> item = graphUpdatePromise2.Prepare();
				promises.Add(new ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>(graphUpdatePromise2, item));
			}
		}
		for (;;)
		{
			int num;
			try
			{
				num = GraphUpdateProcessor.PrepareGraphUpdatePromises(promises, async ? TimeSlice.MillisFromNow(2f) : TimeSlice.Infinite);
			}
			catch
			{
				this.isScanning = false;
				this.data.UnlockGraphStructure();
				graphUpdateLock.Release();
				throw;
			}
			if (num == -1)
			{
				break;
			}
			float num2 = 0f;
			for (int l = 0; l < promises.Count; l++)
			{
				num2 += promises[l].Item1.Progress;
			}
			num2 /= (float)promises.Count;
			yield return new Progress(Mathf.Lerp(0.1f, 0.8f, num2), ScanningStage.ScanningGraph, num, promises.Count);
		}
		yield return new Progress(0.95f, ScanningStage.FinishingScans, 0, 0);
		RWLock.LockSync lockSync4 = this.graphDataLock.WriteSync();
		AstarPath.DummyGraphUpdateContext context = new AstarPath.DummyGraphUpdateContext();
		try
		{
			GraphUpdateProcessor.ApplyGraphUpdatePromises(promises, context);
		}
		catch
		{
			this.isScanning = false;
			this.data.UnlockGraphStructure();
			graphUpdateLock.Release();
			lockSync4.Unlock();
			throw;
		}
		for (int m = 0; m < graphsToScan.Length; m++)
		{
			if (graphsToScan[m] != null)
			{
				if (AstarPath.OnGraphPostScan != null)
				{
					try
					{
						AstarPath.OnGraphPostScan(graphsToScan[m]);
					}
					catch
					{
						this.isScanning = false;
						this.data.UnlockGraphStructure();
						graphUpdateLock.Release();
						lockSync4.Unlock();
						throw;
					}
				}
				if (!(graphsToScan[m] is LinkGraph))
				{
					this.offMeshLinks.DirtyBounds(graphsToScan[m].bounds);
				}
			}
		}
		this.data.UnlockGraphStructure();
		try
		{
			if (AstarPath.OnPostScan != null)
			{
				AstarPath.OnPostScan(this);
			}
			GraphModifier.TriggerEvent(GraphModifier.EventType.PostScan);
		}
		catch
		{
			this.isScanning = false;
			graphUpdateLock.Release();
			lockSync4.Unlock();
			throw;
		}
		if (this.workItemLock.Held)
		{
			this.workItems.ProcessWorkItemsForScan(true);
			this.workItemLock.Release();
		}
		this.offMeshLinks.Refresh();
		GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdateBeforeAreaRecalculation);
		this.hierarchicalGraph.RecalculateIfNecessary();
		GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
		if (AstarPath.OnGraphsUpdated != null)
		{
			try
			{
				AstarPath.OnGraphsUpdated(this);
			}
			catch
			{
				this.isScanning = false;
				graphUpdateLock.Release();
				lockSync4.Unlock();
				throw;
			}
		}
		this.isScanning = false;
		try
		{
			if (AstarPath.OnLatePostScan != null)
			{
				AstarPath.OnLatePostScan(this);
			}
			GraphModifier.TriggerEvent(GraphModifier.EventType.LatePostScan);
		}
		catch
		{
			graphUpdateLock.Release();
			lockSync4.Unlock();
			throw;
		}
		lockSync4.Unlock();
		this.euclideanEmbedding.dirty = true;
		this.euclideanEmbedding.RecalculatePivots();
		this.FlushWorkItems();
		graphUpdateLock.Release();
		watch.Stop();
		this.lastScanTime = (float)watch.Elapsed.TotalSeconds;
		if (this.logPathResults != PathLog.None && this.logPathResults != PathLog.OnlyErrors)
		{
			UnityEngine.Debug.Log("Scanned graphs in " + (this.lastScanTime * 1000f).ToString("0") + " ms");
		}
		yield break;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002EC1 File Offset: 0x000010C1
	internal void DirtyBounds(Bounds bounds)
	{
		this.offMeshLinks.DirtyBounds(bounds);
		this.workItems.DirtyGraphs();
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002EDC File Offset: 0x000010DC
	public static void BlockUntilCalculated(Path path)
	{
		if (AstarPath.active == null)
		{
			throw new Exception("Pathfinding is not correctly initialized in this scene (yet?). AstarPath.active is null.\nDo not call this function in Awake");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (AstarPath.active.pathProcessor.queue.isClosed)
		{
			return;
		}
		if (path.PipelineState == PathState.Created)
		{
			throw new Exception("The specified path has not been started yet.");
		}
		AstarPath.waitForPathDepth++;
		if (AstarPath.waitForPathDepth == 5)
		{
			UnityEngine.Debug.LogError("You are calling the BlockUntilCalculated function recursively (maybe from a path callback). Please don't do this.");
		}
		if (path.PipelineState < PathState.ReturnQueue)
		{
			if (AstarPath.active.IsUsingMultithreading)
			{
				while (path.PipelineState < PathState.ReturnQueue)
				{
					if (AstarPath.active.pathProcessor.queue.isClosed)
					{
						AstarPath.waitForPathDepth--;
						throw new Exception("Pathfinding Threads seem to have crashed.");
					}
					Thread.Sleep(1);
					AstarPath.active.PerformBlockingActions(true);
				}
			}
			else
			{
				while (path.PipelineState < PathState.ReturnQueue)
				{
					if (AstarPath.active.pathProcessor.queue.isEmpty && path.PipelineState != PathState.Processing)
					{
						AstarPath.waitForPathDepth--;
						throw new Exception("Critical error. Path Queue is empty but the path state is '" + path.PipelineState.ToString() + "'");
					}
					AstarPath.active.pathProcessor.TickNonMultithreaded();
					AstarPath.active.PerformBlockingActions(true);
				}
			}
		}
		AstarPath.active.pathReturnQueue.ReturnPaths(false);
		AstarPath.waitForPathDepth--;
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00003050 File Offset: 0x00001250
	public static void StartPath(Path path, bool pushToFront = false, bool assumeInPlayMode = false)
	{
		AstarPath astarPath = AstarPath.active;
		if (astarPath == null)
		{
			UnityEngine.Debug.LogError("There is no AstarPath object in the scene or it has not been initialized yet");
			return;
		}
		if (path.PipelineState != PathState.Created)
		{
			throw new Exception(string.Concat(new string[]
			{
				"The path has an invalid state. Expected ",
				PathState.Created.ToString(),
				" found ",
				path.PipelineState.ToString(),
				"\nMake sure you are not requesting the same path twice"
			}));
		}
		if (astarPath.pathProcessor.queue.isClosed)
		{
			path.FailWithError("No new paths are accepted");
			return;
		}
		if (astarPath.graphs == null || astarPath.graphs.Length == 0)
		{
			UnityEngine.Debug.LogError("There are no graphs in the scene");
			path.FailWithError("There are no graphs in the scene");
			UnityEngine.Debug.LogError(path.errorLog);
			return;
		}
		path.Claim(astarPath);
		((IPathInternals)path).AdvanceState(PathState.PathQueue);
		if (pushToFront)
		{
			astarPath.pathProcessor.queue.PushFront(path);
		}
		else
		{
			astarPath.pathProcessor.queue.Push(path);
		}
		if (!assumeInPlayMode && !JobsUtility.IsExecutingJob && !Application.isPlaying)
		{
			AstarPath.BlockUntilCalculated(path);
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00003168 File Offset: 0x00001368
	public bool IsPointOnNavmesh(Vector3 position)
	{
		NNInfo nearest = this.GetNearest(position, AstarPath.NNConstraintClosestAsSeenFromAbove);
		return nearest.node != null && nearest.node.Walkable && nearest.distanceCostSqr < 0.0001f;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000031A6 File Offset: 0x000013A6
	public NNInfo GetNearest(Vector3 position)
	{
		return this.GetNearest(position, null);
	}

	// Token: 0x0600003F RID: 63 RVA: 0x000031B0 File Offset: 0x000013B0
	public unsafe NNInfo GetNearest(Vector3 position, NNConstraint constraint)
	{
		NavGraph[] graphs = this.graphs;
		float num = (constraint == null || constraint.constrainDistance) ? this.maxNearestNodeDistanceSqr : float.PositiveInfinity;
		NNInfo result = NNInfo.Empty;
		if (graphs == null || graphs.Length == 0)
		{
			return result;
		}
		if (graphs.Length == 1)
		{
			NavGraph navGraph = graphs[0];
			if (navGraph == null || (constraint != null && !constraint.SuitableGraph(0, navGraph)))
			{
				return result;
			}
			result = navGraph.GetNearest(position, constraint, num);
		}
		else
		{
			ValueTuple<float, int>* ptr = stackalloc ValueTuple<float, int>[checked(unchecked((UIntPtr)graphs.Length) * (UIntPtr)sizeof(ValueTuple<float, int>))];
			UnsafeSpan<ValueTuple<float, int>> span = new UnsafeSpan<ValueTuple<float, int>>((void*)ptr, graphs.Length);
			int length = 0;
			for (int i = 0; i < graphs.Length; i++)
			{
				NavGraph navGraph2 = graphs[i];
				if (navGraph2 != null && (constraint == null || constraint.SuitableGraph(i, navGraph2)))
				{
					float num2 = navGraph2.NearestNodeDistanceSqrLowerBound(position, constraint);
					if (num2 <= num)
					{
						*span[length++] = new ValueTuple<float, int>(num2, i);
					}
				}
			}
			span = span.Slice(0, length);
			span.Sort<ValueTuple<float, int>>();
			int num3 = 0;
			while (num3 < span.Length && span[num3].Item1 <= num)
			{
				NNInfo nearest = graphs[span[num3].Item2].GetNearest(position, constraint, num);
				if (nearest.distanceCostSqr < num)
				{
					num = nearest.distanceCostSqr;
					result = nearest;
				}
				num3++;
			}
		}
		return result;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000032F8 File Offset: 0x000014F8
	public bool Linecast(Vector3 start, Vector3 end)
	{
		IRaycastableGraph raycastableGraph = this.ClosestRaycastableGraph(start);
		return raycastableGraph == null || raycastableGraph.Linecast(start, end);
	}

	// Token: 0x06000041 RID: 65 RVA: 0x0000331C File Offset: 0x0000151C
	public bool Linecast(Vector3 start, Vector3 end, out GraphHitInfo hit)
	{
		IRaycastableGraph raycastableGraph = this.ClosestRaycastableGraph(start);
		if (raycastableGraph == null)
		{
			hit = new GraphHitInfo
			{
				origin = start,
				point = start
			};
			return true;
		}
		return raycastableGraph.Linecast(start, end, out hit, null, null);
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003360 File Offset: 0x00001560
	private IRaycastableGraph ClosestRaycastableGraph(Vector3 point)
	{
		if (this.data.graphs == null)
		{
			return null;
		}
		IRaycastableGraph result = null;
		int num = 0;
		for (int i = 0; i < this.data.graphs.Length; i++)
		{
			IRaycastableGraph raycastableGraph = this.data.graphs[i] as IRaycastableGraph;
			if (raycastableGraph != null)
			{
				result = raycastableGraph;
				num++;
			}
		}
		if (num > 1)
		{
			GraphNode node = this.GetNearest(point).node;
			result = (((node != null) ? node.Graph : null) as IRaycastableGraph);
		}
		return result;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000033D8 File Offset: 0x000015D8
	public GraphNode GetNearest(Ray ray)
	{
		if (this.graphs == null)
		{
			return null;
		}
		float minDist = float.PositiveInfinity;
		GraphNode nearestNode = null;
		Vector3 lineDirection = ray.direction;
		Vector3 lineOrigin = ray.origin;
		Action<GraphNode> <>9__0;
		for (int i = 0; i < this.graphs.Length; i++)
		{
			NavGraph navGraph = this.graphs[i];
			Action<GraphNode> action;
			if ((action = <>9__0) == null)
			{
				action = (<>9__0 = delegate(GraphNode node)
				{
					Vector3 vector = (Vector3)node.position;
					Vector3 vector2 = lineOrigin + Vector3.Dot(vector - lineOrigin, lineDirection) * lineDirection;
					float num = Mathf.Abs(vector2.x - vector.x);
					if (num * num > minDist)
					{
						return;
					}
					float num2 = Mathf.Abs(vector2.z - vector.z);
					if (num2 * num2 > minDist)
					{
						return;
					}
					float sqrMagnitude = (vector2 - vector).sqrMagnitude;
					if (sqrMagnitude < minDist)
					{
						minDist = sqrMagnitude;
						nearestNode = node;
					}
				});
			}
			navGraph.GetNodes(action);
		}
		return nearestNode;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00003468 File Offset: 0x00001668
	public GraphSnapshot Snapshot(Bounds bounds, GraphMask graphMask)
	{
		List<IGraphSnapshot> list = new List<IGraphSnapshot>();
		for (int i = 0; i < this.graphs.Length; i++)
		{
			if (this.graphs[i] != null && graphMask.Contains(i))
			{
				IGraphSnapshot graphSnapshot = this.graphs[i].Snapshot(bounds);
				if (graphSnapshot != null)
				{
					list.Add(graphSnapshot);
				}
			}
		}
		return new GraphSnapshot(list);
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000034C1 File Offset: 0x000016C1
	public RWLock.ReadLockAsync LockGraphDataForReading()
	{
		return this.graphDataLock.Read();
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000034CE File Offset: 0x000016CE
	public RWLock.WriteLockAsync LockGraphDataForWriting()
	{
		return this.graphDataLock.Write();
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000034DB File Offset: 0x000016DB
	public RWLock.LockSync LockGraphDataForWritingSync()
	{
		return this.graphDataLock.WriteSync();
	}

	// Token: 0x06000048 RID: 72 RVA: 0x000034E8 File Offset: 0x000016E8
	public NavmeshEdges.NavmeshBorderData GetNavmeshBorderData(out RWLock.CombinedReadLockAsync readLock)
	{
		return this.hierarchicalGraph.navmeshEdges.GetNavmeshEdgeData(out readLock);
	}

	// Token: 0x04000001 RID: 1
	public static readonly Version Version = new Version(5, 3, 7);

	// Token: 0x04000002 RID: 2
	public static readonly AstarPath.AstarDistribution Distribution = AstarPath.AstarDistribution.AssetStore;

	// Token: 0x04000003 RID: 3
	public static readonly string Branch = "master";

	// Token: 0x04000004 RID: 4
	[FormerlySerializedAs("astarData")]
	public AstarData data;

	// Token: 0x04000005 RID: 5
	public static AstarPath active;

	// Token: 0x04000006 RID: 6
	private bool hasScannedGraphAtStartup;

	// Token: 0x04000007 RID: 7
	public bool showNavGraphs = true;

	// Token: 0x04000008 RID: 8
	public bool showUnwalkableNodes = true;

	// Token: 0x04000009 RID: 9
	public GraphDebugMode debugMode;

	// Token: 0x0400000A RID: 10
	public float debugFloor;

	// Token: 0x0400000B RID: 11
	public float debugRoof = 20000f;

	// Token: 0x0400000C RID: 12
	public bool manualDebugFloorRoof;

	// Token: 0x0400000D RID: 13
	public bool showSearchTree;

	// Token: 0x0400000E RID: 14
	public float unwalkableNodeDebugSize = 0.3f;

	// Token: 0x0400000F RID: 15
	public PathLog logPathResults = PathLog.Normal;

	// Token: 0x04000010 RID: 16
	public float maxNearestNodeDistance = 100f;

	// Token: 0x04000011 RID: 17
	public bool scanOnStartup = true;

	// Token: 0x04000012 RID: 18
	[Obsolete("This setting has been removed. It is now always true", true)]
	public bool fullGetNearestSearch;

	// Token: 0x04000013 RID: 19
	[Obsolete("This setting has been removed. It was always a bit of a hack. Use NNConstraint.graphMask if you want to choose which graphs are searched.", true)]
	public bool prioritizeGraphs;

	// Token: 0x04000014 RID: 20
	[Obsolete("This setting has been removed. It was always a bit of a hack. Use NNConstraint.graphMask if you want to choose which graphs are searched.", true)]
	public float prioritizeGraphsLimit = 1f;

	// Token: 0x04000015 RID: 21
	public AstarColor colorSettings;

	// Token: 0x04000016 RID: 22
	[SerializeField]
	protected string[] tagNames;

	// Token: 0x04000017 RID: 23
	public Heuristic heuristic = Heuristic.Euclidean;

	// Token: 0x04000018 RID: 24
	public float heuristicScale = 1f;

	// Token: 0x04000019 RID: 25
	public ThreadCount threadCount = ThreadCount.One;

	// Token: 0x0400001A RID: 26
	public float maxFrameTime = 1f;

	// Token: 0x0400001B RID: 27
	public bool batchGraphUpdates;

	// Token: 0x0400001C RID: 28
	public float graphUpdateBatchingInterval = 0.2f;

	// Token: 0x0400001E RID: 30
	[NonSerialized]
	internal PathHandler debugPathData;

	// Token: 0x0400001F RID: 31
	[NonSerialized]
	internal ushort debugPathID;

	// Token: 0x04000020 RID: 32
	private string inGameDebugPath;

	// Token: 0x04000022 RID: 34
	public static Action OnAwakeSettings;

	// Token: 0x04000023 RID: 35
	public static OnGraphDelegate OnGraphPreScan;

	// Token: 0x04000024 RID: 36
	public static OnGraphDelegate OnGraphPostScan;

	// Token: 0x04000025 RID: 37
	public static OnPathDelegate OnPathPreSearch;

	// Token: 0x04000026 RID: 38
	public static OnPathDelegate OnPathPostSearch;

	// Token: 0x04000027 RID: 39
	public static OnScanDelegate OnPreScan;

	// Token: 0x04000028 RID: 40
	public static OnScanDelegate OnPostScan;

	// Token: 0x04000029 RID: 41
	public static OnScanDelegate OnLatePostScan;

	// Token: 0x0400002A RID: 42
	public static OnScanDelegate OnGraphsUpdated;

	// Token: 0x0400002B RID: 43
	public static Action On65KOverflow;

	// Token: 0x0400002C RID: 44
	public static Action OnPathsCalculated;

	// Token: 0x0400002D RID: 45
	private readonly GraphUpdateProcessor graphUpdates;

	// Token: 0x0400002E RID: 46
	internal readonly HierarchicalGraph hierarchicalGraph;

	// Token: 0x0400002F RID: 47
	public readonly OffMeshLinks offMeshLinks;

	// Token: 0x04000030 RID: 48
	public NavmeshUpdates navmeshUpdates = new NavmeshUpdates();

	// Token: 0x04000031 RID: 49
	private readonly WorkItemProcessor workItems;

	// Token: 0x04000032 RID: 50
	private readonly PathProcessor pathProcessor;

	// Token: 0x04000033 RID: 51
	internal GlobalNodeStorage nodeStorage;

	// Token: 0x04000034 RID: 52
	private RWLock graphDataLock = new RWLock();

	// Token: 0x04000035 RID: 53
	private bool graphUpdateRoutineRunning;

	// Token: 0x04000036 RID: 54
	private bool graphUpdatesWorkItemAdded;

	// Token: 0x04000037 RID: 55
	private float lastGraphUpdate = -9999f;

	// Token: 0x04000038 RID: 56
	private PathProcessor.GraphUpdateLock workItemLock;

	// Token: 0x04000039 RID: 57
	internal readonly PathReturnQueue pathReturnQueue;

	// Token: 0x0400003A RID: 58
	public EuclideanEmbedding euclideanEmbedding = new EuclideanEmbedding();

	// Token: 0x0400003B RID: 59
	private IEnumerator<Progress> asyncScanTask;

	// Token: 0x0400003C RID: 60
	public bool showGraphs;

	// Token: 0x0400003D RID: 61
	private ushort nextFreePathID = 1;

	// Token: 0x0400003E RID: 62
	private RedrawScope redrawScope;

	// Token: 0x0400003F RID: 63
	private static int waitForPathDepth = 0;

	// Token: 0x04000040 RID: 64
	internal static readonly NNConstraint NNConstraintClosestAsSeenFromAbove = new NNConstraint
	{
		constrainWalkability = false,
		constrainTags = false,
		constrainDistance = true,
		distanceMetric = DistanceMetric.ClosestAsSeenFromAbove()
	};

	// Token: 0x02000005 RID: 5
	public enum AstarDistribution
	{
		// Token: 0x04000042 RID: 66
		WebsiteDownload,
		// Token: 0x04000043 RID: 67
		AssetStore,
		// Token: 0x04000044 RID: 68
		PackageManager
	}

	// Token: 0x02000006 RID: 6
	private class DummyGraphUpdateContext : IGraphUpdateContext
	{
		// Token: 0x0600004D RID: 77 RVA: 0x000035CE File Offset: 0x000017CE
		public void DirtyBounds(Bounds bounds)
		{
		}
	}

	// Token: 0x02000007 RID: 7
	private class DestroyGraphPromise : IGraphUpdatePromise
	{
		// Token: 0x0600004F RID: 79 RVA: 0x000035D8 File Offset: 0x000017D8
		public IEnumerator<JobHandle> Prepare()
		{
			return null;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000035DB File Offset: 0x000017DB
		public void Apply(IGraphUpdateContext context)
		{
			this.graph.DestroyAllNodes();
		}

		// Token: 0x04000045 RID: 69
		public IGraphInternals graph;
	}
}
