using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000A9 RID: 169
	[BurstCompile]
	public abstract class Path : IPathInternals
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00019E29 File Offset: 0x00018029
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x00019E31 File Offset: 0x00018031
		public PathState PipelineState { get; private set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00019E3A File Offset: 0x0001803A
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x00019E44 File Offset: 0x00018044
		public PathCompleteState CompleteState
		{
			get
			{
				return this.completeState;
			}
			protected set
			{
				lock (this)
				{
					if (this.completeState != PathCompleteState.Error)
					{
						this.completeState = value;
					}
				}
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00019E8C File Offset: 0x0001808C
		public bool error
		{
			get
			{
				return this.CompleteState == PathCompleteState.Error;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x00019E97 File Offset: 0x00018097
		// (set) Token: 0x06000530 RID: 1328 RVA: 0x00019E9F File Offset: 0x0001809F
		public string errorLog { get; private set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00019EA8 File Offset: 0x000180A8
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x00019EB0 File Offset: 0x000180B0
		public int searchedNodes { get; protected set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x00019EB9 File Offset: 0x000180B9
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x00019EC1 File Offset: 0x000180C1
		bool IPathInternals.Pooled { get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00019ECA File Offset: 0x000180CA
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x00019ED2 File Offset: 0x000180D2
		public ushort pathID { get; private set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x00019EDB File Offset: 0x000180DB
		internal ref HeuristicObjective heuristicObjectiveInternal
		{
			get
			{
				return ref this.heuristicObjective;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00019EE3 File Offset: 0x000180E3
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x00019EFA File Offset: 0x000180FA
		public int[] tagPenalties
		{
			get
			{
				if (this.internalTagPenalties != Path.ZeroTagPenalties)
				{
					return this.internalTagPenalties;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this.internalTagPenalties = Path.ZeroTagPenalties;
					return;
				}
				if (value.Length != 32)
				{
					throw new ArgumentException("tagPenalties must have a length of 32");
				}
				this.internalTagPenalties = value;
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00019F24 File Offset: 0x00018124
		public void UseSettings(PathRequestSettings settings)
		{
			this.nnConstraint.graphMask = settings.graphMask;
			this.traversalProvider = settings.traversalProvider;
			this.enabledTags = settings.traversableTags;
			this.tagPenalties = settings.tagPenalties;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00019F5C File Offset: 0x0001815C
		public float GetTotalLength()
		{
			if (this.vectorPath == null)
			{
				return float.PositiveInfinity;
			}
			float num = 0f;
			for (int i = 0; i < this.vectorPath.Count - 1; i++)
			{
				num += Vector3.Distance(this.vectorPath[i], this.vectorPath[i + 1]);
			}
			return num;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00019FB8 File Offset: 0x000181B8
		public IEnumerator WaitForPath()
		{
			if (this.PipelineState == PathState.Created)
			{
				throw new InvalidOperationException("This path has not been started yet");
			}
			while (this.PipelineState != PathState.Returned)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00019FC7 File Offset: 0x000181C7
		public void BlockUntilCalculated()
		{
			AstarPath.BlockUntilCalculated(this);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00019FD0 File Offset: 0x000181D0
		public unsafe bool ShouldConsiderPathNode(uint pathNodeIndex)
		{
			PathNode pathNode = *this.pathHandler.pathNodes[pathNodeIndex];
			return pathNode.pathID != this.pathID || pathNode.heapIndex != ushort.MaxValue;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001A014 File Offset: 0x00018214
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SkipOverNode(uint pathNodeIndex, uint parentNodeIndex, uint fractionAlongEdge, uint hScore, uint gScore)
		{
			ref PathNode ptr = ref this.pathHandler.pathNodes[pathNodeIndex];
			ptr.pathID = this.pathID;
			ptr.heapIndex = ushort.MaxValue;
			ptr.parentIndex = parentNodeIndex;
			ptr.fractionAlongEdge = fractionAlongEdge;
			this.OnVisitNode(pathNodeIndex, hScore, gScore);
			this.pathHandler.LogVisitedNode(pathNodeIndex, hScore, gScore);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001A074 File Offset: 0x00018274
		public void OpenCandidateConnectionsToEndNode(Int3 position, uint parentPathNode, uint parentNodeIndex, uint parentG)
		{
			if (this.pathHandler.pathNodes[parentNodeIndex].flag1)
			{
				uint num = 0U;
				while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
				{
					uint num2 = this.pathHandler.temporaryNodeStartIndex + num;
					ref TemporaryNode temporaryNode = ref this.pathHandler.GetTemporaryNode(num2);
					if (temporaryNode.type == TemporaryNodeType.End && temporaryNode.associatedNode == parentNodeIndex)
					{
						uint costMagnitude = (uint)(position - temporaryNode.position).costMagnitude;
						this.OpenCandidateConnection(parentPathNode, num2, parentG, costMagnitude, 0U, temporaryNode.position);
					}
					num += 1U;
				}
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001A108 File Offset: 0x00018308
		public void OpenCandidateConnection(uint parentPathNode, uint targetPathNode, uint parentG, uint connectionCost, uint fractionAlongEdge, Int3 targetNodePosition)
		{
			if (!this.ShouldConsiderPathNode(targetPathNode))
			{
				return;
			}
			uint num;
			uint targetNodeIndex;
			if (this.pathHandler.IsTemporaryNode(targetPathNode))
			{
				num = 0U;
				targetNodeIndex = 0U;
			}
			else
			{
				GraphNode node = this.pathHandler.GetNode(targetPathNode);
				num = this.GetTraversalCost(node);
				targetNodeIndex = node.NodeIndex;
			}
			uint candidateG = parentG + connectionCost + num;
			Path.OpenCandidateParams openCandidateParams = new Path.OpenCandidateParams
			{
				pathID = this.pathID,
				parentPathNode = parentPathNode,
				targetPathNode = targetPathNode,
				targetNodeIndex = targetNodeIndex,
				candidateG = candidateG,
				fractionAlongEdge = fractionAlongEdge,
				targetNodePosition = (int3)targetNodePosition,
				pathNodes = this.pathHandler.pathNodes
			};
			Path.OpenCandidateConnectionBurst(ref openCandidateParams, ref this.pathHandler.heap, ref this.heuristicObjective);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001A1D2 File Offset: 0x000183D2
		[BurstCompile]
		public static void OpenCandidateConnectionBurst(ref Path.OpenCandidateParams pars, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective)
		{
			Path.OpenCandidateConnectionBurst_00000500$BurstDirectCall.Invoke(ref pars, ref heap, ref heuristicObjective);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001A1DC File Offset: 0x000183DC
		public uint GetTagPenalty(int tag)
		{
			return (uint)this.internalTagPenalties[tag];
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001A1E6 File Offset: 0x000183E6
		public bool CanTraverse(GraphNode node)
		{
			if (this.traversalProvider != null)
			{
				return this.traversalProvider.CanTraverse(this, node);
			}
			return node.Walkable && (this.enabledTags >> (int)node.Tag & 1) != 0;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001A21D File Offset: 0x0001841D
		public bool CanTraverse(GraphNode from, GraphNode to)
		{
			if (this.traversalProvider != null)
			{
				return this.traversalProvider.CanTraverse(this, from, to);
			}
			return to.Walkable && (this.enabledTags >> (int)to.Tag & 1) != 0;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001A255 File Offset: 0x00018455
		public uint GetTraversalCost(GraphNode node)
		{
			if (this.traversalProvider != null)
			{
				return this.traversalProvider.GetTraversalCost(this, node);
			}
			return this.GetTagPenalty((int)node.Tag) + node.Penalty;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001A280 File Offset: 0x00018480
		public bool IsDone()
		{
			return this.PipelineState > PathState.Processing;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001A28C File Offset: 0x0001848C
		void IPathInternals.AdvanceState(PathState s)
		{
			lock (this)
			{
				this.PipelineState = (PathState)Math.Max((int)this.PipelineState, (int)s);
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001A2D4 File Offset: 0x000184D4
		public void FailWithError(string msg)
		{
			this.Error();
			if (this.errorLog != "")
			{
				this.errorLog = this.errorLog + "\n" + msg;
				return;
			}
			this.errorLog = msg;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001A30D File Offset: 0x0001850D
		public void Error()
		{
			this.CompleteState = PathCompleteState.Error;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001A318 File Offset: 0x00018518
		private void ErrorCheck()
		{
			if (!this.hasBeenReset)
			{
				this.FailWithError("Please use the static Construct function for creating paths, do not use the normal constructors.");
			}
			if (((IPathInternals)this).Pooled)
			{
				this.FailWithError("The path is currently in a path pool. Are you sending the path for calculation twice?");
			}
			if (this.pathHandler == null)
			{
				this.FailWithError("Field pathHandler is not set. Please report this bug.");
			}
			if (this.PipelineState > PathState.Processing)
			{
				this.FailWithError("This path has already been processed. Do not request a path with the same path object twice.");
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001A374 File Offset: 0x00018574
		protected virtual void OnEnterPool()
		{
			if (this.vectorPath != null)
			{
				ListPool<Vector3>.Release(ref this.vectorPath);
			}
			if (this.path != null)
			{
				ListPool<GraphNode>.Release(ref this.path);
			}
			this.callback = null;
			this.immediateCallback = null;
			this.traversalProvider = null;
			this.pathHandler = null;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001A3C4 File Offset: 0x000185C4
		protected virtual void Reset()
		{
			if (AstarPath.active == null)
			{
				throw new NullReferenceException("No AstarPath object found in the scene. Make sure there is one or do not create paths in Awake");
			}
			this.hasBeenReset = true;
			this.PipelineState = PathState.Created;
			this.releasedNotSilent = false;
			this.pathHandler = null;
			this.callback = null;
			this.immediateCallback = null;
			this.errorLog = "";
			this.completeState = PathCompleteState.NotCalculated;
			this.path = ListPool<GraphNode>.Claim();
			this.vectorPath = ListPool<Vector3>.Claim();
			this.duration = 0f;
			this.searchedNodes = 0;
			this.nnConstraint = PathNNConstraint.Walkable;
			this.heuristic = AstarPath.active.heuristic;
			this.heuristicScale = AstarPath.active.heuristicScale;
			this.enabledTags = -1;
			this.tagPenalties = null;
			this.pathID = AstarPath.active.GetNextPathID();
			this.hTargetNode = null;
			this.traversalProvider = null;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001A4A0 File Offset: 0x000186A0
		public void Claim(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			for (int i = 0; i < this.claimed.Count; i++)
			{
				if (this.claimed[i] == o)
				{
					throw new ArgumentException("You have already claimed the path with that object (" + ((o != null) ? o.ToString() : null) + "). Are you claiming the path with the same object twice?");
				}
			}
			this.claimed.Add(o);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001A510 File Offset: 0x00018710
		public void Release(object o, bool silent = false)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			for (int i = 0; i < this.claimed.Count; i++)
			{
				if (this.claimed[i] == o)
				{
					this.claimed.RemoveAt(i);
					if (!silent)
					{
						this.releasedNotSilent = true;
					}
					if (this.claimed.Count == 0 && this.releasedNotSilent)
					{
						PathPool.Pool(this);
					}
					return;
				}
			}
			if (this.claimed.Count == 0)
			{
				throw new ArgumentException("You are releasing a path which is not claimed at all (most likely it has been pooled already). Are you releasing the path with the same object (" + ((o != null) ? o.ToString() : null) + ") twice?\nCheck out the documentation on path pooling for help.");
			}
			throw new ArgumentException("You are releasing a path which has not been claimed with this object (" + ((o != null) ? o.ToString() : null) + "). Are you releasing the path with the same object twice?\nCheck out the documentation on path pooling for help.");
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001A5D0 File Offset: 0x000187D0
		protected virtual void Trace(uint fromPathNodeIndex)
		{
			this.Trace(fromPathNodeIndex, true);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001A5DC File Offset: 0x000187DC
		protected void Trace(uint fromPathNodeIndex, bool reverse)
		{
			uint num = fromPathNodeIndex;
			int num2 = 0;
			UnsafeSpan<PathNode> pathNodes = this.pathHandler.pathNodes;
			while (num != 0U)
			{
				num = pathNodes[num].parentIndex;
				num2++;
				if (num2 > 16384)
				{
					Debug.LogWarning("Infinite loop? >16384 node path. Remove this message if you really have that long paths (Path.cs, Trace method)");
					break;
				}
			}
			if (this.path.Capacity < num2)
			{
				this.path.Capacity = num2;
			}
			num = fromPathNodeIndex;
			GraphNode graphNode = null;
			for (int i = 0; i < num2; i++)
			{
				GraphNode node;
				if (this.pathHandler.IsTemporaryNode(num))
				{
					node = this.pathHandler.GetNode(this.pathHandler.GetTemporaryNode(num).associatedNode);
				}
				else
				{
					node = this.pathHandler.GetNode(num);
				}
				if (node != graphNode)
				{
					this.path.Add(node);
					graphNode = node;
				}
				num = pathNodes[num].parentIndex;
			}
			if (reverse)
			{
				this.path.Reverse();
			}
			num2 = this.path.Count;
			if (this.vectorPath.Capacity < num2)
			{
				this.vectorPath.Capacity = num2;
			}
			for (int j = 0; j < num2; j++)
			{
				this.vectorPath.Add((Vector3)this.path[j].position);
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001A718 File Offset: 0x00018918
		protected void DebugStringPrefix(PathLog logMode, StringBuilder text)
		{
			text.Append(this.error ? "Path Failed : " : "Path Completed : ");
			text.Append("Computation Time ");
			text.Append(this.duration.ToString((logMode == PathLog.Heavy) ? "0.000 ms " : "0.00 ms "));
			text.Append("Searched Nodes ").Append(this.searchedNodes);
			if (!this.error)
			{
				text.Append(" Path Length ");
				text.Append((this.path == null) ? "Null" : this.path.Count.ToString());
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001A7C4 File Offset: 0x000189C4
		protected void DebugStringSuffix(PathLog logMode, StringBuilder text)
		{
			if (this.error)
			{
				text.Append("\nError: ").Append(this.errorLog);
			}
			if (logMode == PathLog.Heavy && !AstarPath.active.IsUsingMultithreading)
			{
				text.Append("\nCallback references ");
				if (this.callback != null)
				{
					text.Append(this.callback.Target.GetType().FullName).AppendLine();
				}
				else
				{
					text.AppendLine("NULL");
				}
			}
			text.Append("\nPath Number ").Append(this.pathID).Append(" (unique id)");
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001A864 File Offset: 0x00018A64
		protected virtual string DebugString(PathLog logMode)
		{
			if (logMode == PathLog.None || (!this.error && logMode == PathLog.OnlyErrors))
			{
				return "";
			}
			StringBuilder debugStringBuilder = this.pathHandler.DebugStringBuilder;
			debugStringBuilder.Length = 0;
			this.DebugStringPrefix(logMode, debugStringBuilder);
			this.DebugStringSuffix(logMode, debugStringBuilder);
			return debugStringBuilder.ToString();
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001A8AF File Offset: 0x00018AAF
		protected virtual void ReturnPath()
		{
			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001A8C8 File Offset: 0x00018AC8
		private void InitializeNNConstraint()
		{
			this.nnConstraint.tags = this.enabledTags;
			if (this.traversalProvider != null)
			{
				this.pathHandler.constraintWrapper.Set(this, this.nnConstraint, this.traversalProvider);
				return;
			}
			this.pathHandler.constraintWrapper.Reset();
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001A91C File Offset: 0x00018B1C
		protected NNInfo GetNearest(Vector3 point)
		{
			return AstarPath.active.GetNearest(point, this.pathHandler.constraintWrapper.isSet ? this.pathHandler.constraintWrapper : this.nnConstraint);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001A950 File Offset: 0x00018B50
		protected void PrepareBase(PathHandler pathHandler)
		{
			this.pathHandler = pathHandler;
			pathHandler.InitializeForPath(this);
			this.InitializeNNConstraint();
			if (this.internalTagPenalties == null || this.internalTagPenalties.Length != 32)
			{
				this.internalTagPenalties = Path.ZeroTagPenalties;
			}
			try
			{
				this.ErrorCheck();
			}
			catch (Exception ex)
			{
				this.FailWithError(ex.Message);
			}
		}

		// Token: 0x06000559 RID: 1369
		protected abstract void Prepare();

		// Token: 0x0600055A RID: 1370 RVA: 0x0001A9B8 File Offset: 0x00018BB8
		protected virtual void Cleanup()
		{
			UnsafeSpan<PathNode> pathNodes = this.pathHandler.pathNodes;
			uint num = 0U;
			while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
			{
				uint nodeIndex = this.pathHandler.temporaryNodeStartIndex + num;
				ref TemporaryNode temporaryNode = ref this.pathHandler.GetTemporaryNode(nodeIndex);
				GraphNode node = this.pathHandler.GetNode(temporaryNode.associatedNode);
				uint num2 = 0U;
				while ((ulong)num2 < (ulong)((long)node.PathNodeVariants))
				{
					pathNodes[temporaryNode.associatedNode + num2].flag1 = false;
					pathNodes[temporaryNode.associatedNode + num2].flag2 = false;
					num2 += 1U;
				}
				num += 1U;
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001AA60 File Offset: 0x00018C60
		protected int3 FirstTemporaryEndNode()
		{
			uint num = 0U;
			while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
			{
				uint nodeIndex = this.pathHandler.temporaryNodeStartIndex + num;
				ref TemporaryNode temporaryNode = ref this.pathHandler.GetTemporaryNode(nodeIndex);
				if (temporaryNode.type == TemporaryNodeType.End)
				{
					return (int3)temporaryNode.position;
				}
				num += 1U;
			}
			throw new InvalidOperationException("There are no end nodes in the path");
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001AAC0 File Offset: 0x00018CC0
		protected void TemporaryEndNodesBoundingBox(out int3 mn, out int3 mx)
		{
			mn = int.MaxValue;
			mx = int.MinValue;
			uint num = 0U;
			while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
			{
				uint nodeIndex = this.pathHandler.temporaryNodeStartIndex + num;
				ref TemporaryNode temporaryNode = ref this.pathHandler.GetTemporaryNode(nodeIndex);
				if (temporaryNode.type == TemporaryNodeType.End)
				{
					mn = math.min(mn, (int3)temporaryNode.position);
					mx = math.max(mx, (int3)temporaryNode.position);
				}
				num += 1U;
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001AB64 File Offset: 0x00018D64
		protected void MarkNodesAdjacentToTemporaryEndNodes()
		{
			UnsafeSpan<PathNode> pathNodes = this.pathHandler.pathNodes;
			uint num = 0U;
			while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
			{
				uint nodeIndex = this.pathHandler.temporaryNodeStartIndex + num;
				ref TemporaryNode temporaryNode = ref this.pathHandler.GetTemporaryNode(nodeIndex);
				if (temporaryNode.type == TemporaryNodeType.End)
				{
					GraphNode node = this.pathHandler.GetNode(temporaryNode.associatedNode);
					uint num2 = 0U;
					while ((ulong)num2 < (ulong)((long)node.PathNodeVariants))
					{
						pathNodes[temporaryNode.associatedNode + num2].flag1 = true;
						num2 += 1U;
					}
				}
				num += 1U;
			}
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001ABFC File Offset: 0x00018DFC
		protected void AddStartNodesToHeap()
		{
			UnsafeSpan<PathNode> pathNodes = this.pathHandler.pathNodes;
			uint num = 0U;
			while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
			{
				uint num2 = this.pathHandler.temporaryNodeStartIndex + num;
				if (this.pathHandler.GetTemporaryNode(num2).type == TemporaryNodeType.Start)
				{
					this.pathHandler.heap.Add(pathNodes, num2, 0U, 0U);
				}
				num += 1U;
			}
		}

		// Token: 0x0600055F RID: 1375
		protected abstract void OnHeapExhausted();

		// Token: 0x06000560 RID: 1376
		protected abstract void OnFoundEndNode(uint pathNode, uint hScore, uint gScore);

		// Token: 0x06000561 RID: 1377 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void OnVisitNode(uint pathNode, uint hScore, uint gScore)
		{
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001AC64 File Offset: 0x00018E64
		protected unsafe virtual void CalculateStep(long targetTick)
		{
			int num = 0;
			uint temporaryNodeStartIndex = this.pathHandler.temporaryNodeStartIndex;
			while (this.CompleteState == PathCompleteState.NotCalculated)
			{
				int searchedNodes = this.searchedNodes;
				this.searchedNodes = searchedNodes + 1;
				if (this.pathHandler.heap.isEmpty)
				{
					this.OnHeapExhausted();
					return;
				}
				uint num3;
				uint num4;
				uint num2 = this.pathHandler.heap.Remove(this.pathHandler.pathNodes, out num3, out num4);
				if (num2 >= temporaryNodeStartIndex)
				{
					TemporaryNode temporaryNode = *this.pathHandler.GetTemporaryNode(num2);
					if (temporaryNode.type == TemporaryNodeType.Start)
					{
						this.pathHandler.GetNode(temporaryNode.associatedNode).OpenAtPoint(this, num2, temporaryNode.position, num3);
					}
					else if (temporaryNode.type == TemporaryNodeType.End)
					{
						this.pathHandler.LogVisitedNode(temporaryNode.associatedNode, num4, num3);
						this.OnFoundEndNode(num2, num4, num3);
						if (this.CompleteState == PathCompleteState.Complete)
						{
							return;
						}
					}
				}
				else
				{
					this.pathHandler.LogVisitedNode(num2, num4, num3);
					this.OnVisitNode(num2, num4, num3);
					this.pathHandler.GetNode(num2).Open(this, num2, num3);
				}
				if (num > 500)
				{
					if (DateTime.UtcNow.Ticks >= targetTick)
					{
						return;
					}
					num = 0;
					if (this.searchedNodes > 1000000)
					{
						throw new Exception("Probable infinite loop. Over 1,000,000 nodes searched");
					}
				}
				num++;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0001ADB5 File Offset: 0x00018FB5
		PathHandler IPathInternals.PathHandler
		{
			get
			{
				return this.pathHandler;
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001ADBD File Offset: 0x00018FBD
		void IPathInternals.OnEnterPool()
		{
			this.OnEnterPool();
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001ADC5 File Offset: 0x00018FC5
		void IPathInternals.Reset()
		{
			this.Reset();
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001ADCD File Offset: 0x00018FCD
		void IPathInternals.ReturnPath()
		{
			this.ReturnPath();
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001ADD5 File Offset: 0x00018FD5
		void IPathInternals.PrepareBase(PathHandler handler)
		{
			this.PrepareBase(handler);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001ADDE File Offset: 0x00018FDE
		void IPathInternals.Prepare()
		{
			this.Prepare();
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001ADE6 File Offset: 0x00018FE6
		void IPathInternals.Cleanup()
		{
			this.Cleanup();
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001ADEE File Offset: 0x00018FEE
		void IPathInternals.CalculateStep(long targetTick)
		{
			this.CalculateStep(targetTick);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001ADF7 File Offset: 0x00018FF7
		string IPathInternals.DebugString(PathLog logMode)
		{
			return this.DebugString(logMode);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001AE5C File Offset: 0x0001905C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void OpenCandidateConnectionBurst$BurstManaged(ref Path.OpenCandidateParams pars, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective)
		{
			ushort pathID = pars.pathID;
			uint parentPathNode = pars.parentPathNode;
			uint targetPathNode = pars.targetPathNode;
			uint candidateG = pars.candidateG;
			uint fractionAlongEdge = pars.fractionAlongEdge;
			int3 targetNodePosition = pars.targetNodePosition;
			UnsafeSpan<PathNode> pathNodes = pars.pathNodes;
			ref PathNode ptr = ref pathNodes[targetPathNode];
			if (ptr.pathID != pathID)
			{
				ptr.fractionAlongEdge = fractionAlongEdge;
				ptr.pathID = pathID;
				ptr.parentIndex = parentPathNode;
				uint h = (uint)heuristicObjective.Calculate(targetNodePosition, pars.targetNodeIndex);
				heap.Add(pathNodes, targetPathNode, candidateG, h);
				return;
			}
			uint f = heap.GetF((int)ptr.heapIndex);
			uint h2 = heap.GetH((int)ptr.heapIndex);
			uint num;
			if (ptr.fractionAlongEdge != fractionAlongEdge)
			{
				num = (uint)heuristicObjective.Calculate(targetNodePosition, pars.targetNodeIndex);
			}
			else
			{
				num = h2;
			}
			if (candidateG + num < f)
			{
				ptr.fractionAlongEdge = fractionAlongEdge;
				ptr.parentIndex = parentPathNode;
				heap.Add(pathNodes, targetPathNode, candidateG, num);
			}
		}

		// Token: 0x0400037D RID: 893
		protected PathHandler pathHandler;

		// Token: 0x0400037E RID: 894
		public OnPathDelegate callback;

		// Token: 0x0400037F RID: 895
		public OnPathDelegate immediateCallback;

		// Token: 0x04000381 RID: 897
		public ITraversalProvider traversalProvider;

		// Token: 0x04000382 RID: 898
		protected PathCompleteState completeState;

		// Token: 0x04000384 RID: 900
		public List<GraphNode> path;

		// Token: 0x04000385 RID: 901
		public List<Vector3> vectorPath;

		// Token: 0x04000386 RID: 902
		public float duration;

		// Token: 0x04000389 RID: 905
		protected bool hasBeenReset;

		// Token: 0x0400038A RID: 906
		public NNConstraint nnConstraint = PathNNConstraint.Walkable;

		// Token: 0x0400038B RID: 907
		public Heuristic heuristic;

		// Token: 0x0400038C RID: 908
		public float heuristicScale = 1f;

		// Token: 0x0400038E RID: 910
		protected GraphNode hTargetNode;

		// Token: 0x0400038F RID: 911
		protected HeuristicObjective heuristicObjective;

		// Token: 0x04000390 RID: 912
		public int enabledTags = -1;

		// Token: 0x04000391 RID: 913
		internal static readonly int[] ZeroTagPenalties = new int[32];

		// Token: 0x04000392 RID: 914
		protected int[] internalTagPenalties;

		// Token: 0x04000393 RID: 915
		public static readonly ProfilerMarker MarkerOpenCandidateConnectionsToEnd = new ProfilerMarker("OpenCandidateConnectionsToEnd");

		// Token: 0x04000394 RID: 916
		public static readonly ProfilerMarker MarkerTrace = new ProfilerMarker("Trace");

		// Token: 0x04000395 RID: 917
		private List<object> claimed = new List<object>();

		// Token: 0x04000396 RID: 918
		private bool releasedNotSilent;

		// Token: 0x020000AA RID: 170
		public struct OpenCandidateParams
		{
			// Token: 0x04000397 RID: 919
			public UnsafeSpan<PathNode> pathNodes;

			// Token: 0x04000398 RID: 920
			public uint parentPathNode;

			// Token: 0x04000399 RID: 921
			public uint targetPathNode;

			// Token: 0x0400039A RID: 922
			public uint targetNodeIndex;

			// Token: 0x0400039B RID: 923
			public uint candidateG;

			// Token: 0x0400039C RID: 924
			public uint fractionAlongEdge;

			// Token: 0x0400039D RID: 925
			public int3 targetNodePosition;

			// Token: 0x0400039E RID: 926
			public ushort pathID;
		}

		// Token: 0x020000AC RID: 172
		// (Invoke) Token: 0x06000576 RID: 1398
		internal delegate void OpenCandidateConnectionBurst_00000500$PostfixBurstDelegate(ref Path.OpenCandidateParams pars, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective);

		// Token: 0x020000AD RID: 173
		internal static class OpenCandidateConnectionBurst_00000500$BurstDirectCall
		{
			// Token: 0x06000579 RID: 1401 RVA: 0x0001AFC3 File Offset: 0x000191C3
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Path.OpenCandidateConnectionBurst_00000500$BurstDirectCall.Pointer == 0)
				{
					Path.OpenCandidateConnectionBurst_00000500$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Path.OpenCandidateConnectionBurst_00000500$BurstDirectCall.DeferredCompilation, methodof(Path.OpenCandidateConnectionBurst$BurstManaged(Path.OpenCandidateParams*, BinaryHeap*, HeuristicObjective*)).MethodHandle, typeof(Path.OpenCandidateConnectionBurst_00000500$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Path.OpenCandidateConnectionBurst_00000500$BurstDirectCall.Pointer;
			}

			// Token: 0x0600057A RID: 1402 RVA: 0x0001AFF0 File Offset: 0x000191F0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Path.OpenCandidateConnectionBurst_00000500$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x0600057B RID: 1403 RVA: 0x0001B008 File Offset: 0x00019208
			public unsafe static void Constructor()
			{
				Path.OpenCandidateConnectionBurst_00000500$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Path.OpenCandidateConnectionBurst(Path.OpenCandidateParams*, BinaryHeap*, HeuristicObjective*)).MethodHandle);
			}

			// Token: 0x0600057C RID: 1404 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x0600057D RID: 1405 RVA: 0x0001B019 File Offset: 0x00019219
			// Note: this type is marked as 'beforefieldinit'.
			static OpenCandidateConnectionBurst_00000500$BurstDirectCall()
			{
				Path.OpenCandidateConnectionBurst_00000500$BurstDirectCall.Constructor();
			}

			// Token: 0x0600057E RID: 1406 RVA: 0x0001B020 File Offset: 0x00019220
			public static void Invoke(ref Path.OpenCandidateParams pars, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Path.OpenCandidateConnectionBurst_00000500$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Path/OpenCandidateParams&,Pathfinding.BinaryHeap&,Pathfinding.HeuristicObjective&), ref pars, ref heap, ref heuristicObjective, functionPointer);
						return;
					}
				}
				Path.OpenCandidateConnectionBurst$BurstManaged(ref pars, ref heap, ref heuristicObjective);
			}

			// Token: 0x040003A2 RID: 930
			private static IntPtr Pointer;

			// Token: 0x040003A3 RID: 931
			private static IntPtr DeferredCompilation;
		}
	}
}
