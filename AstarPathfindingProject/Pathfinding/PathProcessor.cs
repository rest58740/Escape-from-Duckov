using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Pathfinding.Sync;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;

namespace Pathfinding
{
	// Token: 0x020000B4 RID: 180
	public class PathProcessor
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060005AC RID: 1452 RVA: 0x0001B508 File Offset: 0x00019708
		// (remove) Token: 0x060005AD RID: 1453 RVA: 0x0001B540 File Offset: 0x00019740
		public event Action<Path> OnPathPreSearch;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060005AE RID: 1454 RVA: 0x0001B578 File Offset: 0x00019778
		// (remove) Token: 0x060005AF RID: 1455 RVA: 0x0001B5B0 File Offset: 0x000197B0
		public event Action<Path> OnPathPostSearch;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060005B0 RID: 1456 RVA: 0x0001B5E8 File Offset: 0x000197E8
		// (remove) Token: 0x060005B1 RID: 1457 RVA: 0x0001B620 File Offset: 0x00019820
		public event Action OnQueueUnblocked;

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0001B655 File Offset: 0x00019855
		public int NumThreads
		{
			get
			{
				return this.pathHandlers.Length;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x0001B65F File Offset: 0x0001985F
		public bool IsUsingMultithreading
		{
			get
			{
				return this.multithreaded;
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001B668 File Offset: 0x00019868
		internal PathProcessor(AstarPath astar, PathReturnQueue returnQueue, int processors, bool multithreaded)
		{
			this.astar = astar;
			this.returnQueue = returnQueue;
			this.queue = new BlockableChannel<Path>();
			this.threads = null;
			this.threadCoroutine = null;
			this.pathHandlers = new PathHandler[0];
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001B6BC File Offset: 0x000198BC
		public void SetThreadCount(int processors, bool multithreaded)
		{
			if (this.threads != null || this.threadCoroutine != null || this.pathHandlers.Length != 0)
			{
				throw new Exception("Call StopThreads before setting the thread count");
			}
			if (processors < 1)
			{
				throw new ArgumentOutOfRangeException("processors");
			}
			if (!multithreaded && processors != 1)
			{
				throw new Exception("Only a single non-multithreaded processor is allowed");
			}
			this.pathHandlers = new PathHandler[processors];
			this.multithreaded = multithreaded;
			for (int i = 0; i < processors; i++)
			{
				this.pathHandlers[i] = new PathHandler(this.astar.nodeStorage, i, processors);
			}
			this.astar.nodeStorage.SetThreadCount(processors);
			this.StartThreads();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001B760 File Offset: 0x00019960
		private void StartThreads()
		{
			if (this.threads != null || this.threadCoroutine != null)
			{
				throw new Exception("Call StopThreads before starting threads");
			}
			this.queue.Reopen();
			this.astar.nodeStorage.SetThreadCount(this.pathHandlers.Length);
			if (this.multithreaded)
			{
				this.threads = new Thread[this.pathHandlers.Length];
				for (int i = 0; i < this.pathHandlers.Length; i++)
				{
					PathHandler pathHandler = this.pathHandlers[i];
					BlockableChannel<Path>.Receiver receiver = this.queue.AddReceiver();
					this.threads[i] = new Thread(delegate()
					{
						this.CalculatePathsThreaded(pathHandler, receiver);
					});
					this.threads[i].Name = "Pathfinding Thread " + i.ToString();
					this.threads[i].IsBackground = true;
					this.threads[i].Start();
				}
				return;
			}
			this.coroutineReceiver = this.queue.AddReceiver();
			this.threadCoroutine = this.CalculatePaths(this.pathHandlers[0]);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001B888 File Offset: 0x00019A88
		private int Lock(bool block)
		{
			this.queue.isBlocked = true;
			if (block)
			{
				while (!this.queue.allReceiversBlocked)
				{
					if (this.IsUsingMultithreading)
					{
						Thread.Sleep(1);
					}
					else
					{
						this.TickNonMultithreaded();
					}
				}
			}
			this.nextLockID++;
			this.locks.Add(this.nextLockID);
			return this.nextLockID;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001B8F0 File Offset: 0x00019AF0
		private void Unlock(int id)
		{
			if (!this.locks.Remove(id))
			{
				throw new ArgumentException("This lock has already been released");
			}
			if (this.locks.Count == 0)
			{
				if (this.OnQueueUnblocked != null)
				{
					this.OnQueueUnblocked();
				}
				this.queue.isBlocked = false;
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001B942 File Offset: 0x00019B42
		public PathProcessor.GraphUpdateLock PausePathfinding(bool block)
		{
			return new PathProcessor.GraphUpdateLock(this, block);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001B94C File Offset: 0x00019B4C
		public void TickNonMultithreaded()
		{
			if (this.threadCoroutine == null)
			{
				throw new InvalidOperationException("Cannot tick non-multithreaded pathfinding when no coroutine has been started");
			}
			try
			{
				if (!this.threadCoroutine.MoveNext())
				{
					this.threadCoroutine = null;
					this.coroutineReceiver.Close();
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.LogError("Unhandled exception during pathfinding. Terminating.");
				this.queue.Close();
				this.threadCoroutine = null;
				this.coroutineReceiver.Close();
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001B9CC File Offset: 0x00019BCC
		public void StopThreads()
		{
			this.queue.Close();
			if (this.threads != null)
			{
				for (int i = 0; i < this.threads.Length; i++)
				{
					if (!this.threads[i].Join(200))
					{
						Debug.LogError("Could not terminate pathfinding thread[" + i.ToString() + "] in 200ms, trying Thread.Abort");
						this.threads[i].Abort();
					}
				}
				this.threads = null;
			}
			if (this.threadCoroutine != null)
			{
				while (this.queue.numReceivers > 0)
				{
					this.TickNonMultithreaded();
				}
			}
			for (int j = 0; j < this.pathHandlers.Length; j++)
			{
				this.pathHandlers[j].Dispose();
			}
			this.pathHandlers = new PathHandler[0];
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001BA8C File Offset: 0x00019C8C
		public void Dispose()
		{
			this.StopThreads();
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001BA94 File Offset: 0x00019C94
		private void CalculatePathsThreaded(PathHandler pathHandler, BlockableChannel<Path>.Receiver receiver)
		{
			try
			{
				long num = 100000L;
				long targetTick = DateTime.UtcNow.Ticks + num;
				Path path;
				while (receiver.Receive(out path) != BlockableChannel<Path>.PopState.Closed)
				{
					IPathInternals pathInternals = path;
					pathInternals.PrepareBase(pathHandler);
					pathInternals.AdvanceState(PathState.Processing);
					if (this.OnPathPreSearch != null)
					{
						this.OnPathPreSearch(path);
					}
					long ticks = DateTime.UtcNow.Ticks;
					pathInternals.Prepare();
					pathHandler.heap.tieBreaking = (path.heuristicObjectiveInternal.hasHeuristic ? BinaryHeap.TieBreaking.HScore : BinaryHeap.TieBreaking.InsertionOrder);
					if (path.CompleteState == PathCompleteState.NotCalculated)
					{
						this.astar.debugPathData = pathInternals.PathHandler;
						this.astar.debugPathID = path.pathID;
						while (path.CompleteState == PathCompleteState.NotCalculated)
						{
							pathInternals.CalculateStep(targetTick);
							targetTick = DateTime.UtcNow.Ticks + num;
							if (this.queue.isClosed)
							{
								path.FailWithError("AstarPath object destroyed");
							}
						}
						path.duration = (float)(DateTime.UtcNow.Ticks - ticks) * 0.0001f;
					}
					pathInternals.Cleanup();
					pathHandler.heap.Clear(pathHandler.pathNodes);
					if (path.immediateCallback != null)
					{
						path.immediateCallback(path);
					}
					if (this.OnPathPostSearch != null)
					{
						this.OnPathPostSearch(path);
					}
					this.returnQueue.Enqueue(path);
					pathInternals.AdvanceState(PathState.ReturnQueue);
				}
				if (this.astar.logPathResults == PathLog.Heavy)
				{
					Debug.LogWarning("Shutting down pathfinding thread #" + pathHandler.threadID.ToString());
				}
				receiver.Close();
				return;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException)
				{
					if (this.astar.logPathResults == PathLog.Heavy)
					{
						Debug.LogWarning("Shutting down pathfinding thread #" + pathHandler.threadID.ToString());
					}
					receiver.Close();
					return;
				}
				Debug.LogException(ex);
				Debug.LogError("Unhandled exception during pathfinding. Terminating.");
				this.queue.Close();
			}
			finally
			{
				Profiler.EndThreadProfiling();
			}
			Debug.LogError("Error : This part should never be reached.");
			receiver.Close();
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001BCDC File Offset: 0x00019EDC
		private IEnumerator CalculatePaths(PathHandler pathHandler)
		{
			long maxTicks = (long)(this.astar.maxFrameTime * 10000f);
			long targetTick = DateTime.UtcNow.Ticks + maxTicks;
			for (;;)
			{
				Path p = null;
				bool blockedBefore = false;
				while (p == null)
				{
					switch (this.coroutineReceiver.ReceiveNoBlock(blockedBefore, out p))
					{
					case BlockableChannel<Path>.PopState.Wait:
						blockedBefore = true;
						yield return null;
						break;
					case BlockableChannel<Path>.PopState.Closed:
						goto IL_BD;
					}
				}
				IPathInternals ip = p;
				maxTicks = (long)(this.astar.maxFrameTime * 10000f);
				ip.PrepareBase(pathHandler);
				ip.AdvanceState(PathState.Processing);
				Action<Path> onPathPreSearch = this.OnPathPreSearch;
				if (onPathPreSearch != null)
				{
					onPathPreSearch(p);
				}
				long ticks = DateTime.UtcNow.Ticks;
				long totalTicks = 0L;
				ip.Prepare();
				pathHandler.heap.tieBreaking = (p.heuristicObjectiveInternal.hasHeuristic ? BinaryHeap.TieBreaking.HScore : BinaryHeap.TieBreaking.InsertionOrder);
				if (p.CompleteState == PathCompleteState.NotCalculated)
				{
					this.astar.debugPathData = ip.PathHandler;
					this.astar.debugPathID = p.pathID;
					while (p.CompleteState == PathCompleteState.NotCalculated)
					{
						ip.CalculateStep(targetTick);
						if (p.CompleteState != PathCompleteState.NotCalculated)
						{
							break;
						}
						totalTicks += DateTime.UtcNow.Ticks - ticks;
						yield return null;
						ticks = DateTime.UtcNow.Ticks;
						if (this.queue.isClosed)
						{
							p.FailWithError("AstarPath object destroyed");
						}
						targetTick = DateTime.UtcNow.Ticks + maxTicks;
					}
					totalTicks += DateTime.UtcNow.Ticks - ticks;
					p.duration = (float)totalTicks * 0.0001f;
				}
				ip.Cleanup();
				pathHandler.heap.Clear(pathHandler.pathNodes);
				OnPathDelegate immediateCallback = p.immediateCallback;
				if (immediateCallback != null)
				{
					immediateCallback(p);
				}
				Action<Path> onPathPostSearch = this.OnPathPostSearch;
				if (onPathPostSearch != null)
				{
					onPathPostSearch(p);
				}
				this.returnQueue.Enqueue(p);
				ip.AdvanceState(PathState.ReturnQueue);
				if (DateTime.UtcNow.Ticks > targetTick)
				{
					yield return null;
					targetTick = DateTime.UtcNow.Ticks + maxTicks;
				}
				p = null;
				ip = null;
			}
			IL_BD:
			yield break;
			yield break;
		}

		// Token: 0x040003C9 RID: 969
		internal BlockableChannel<Path> queue;

		// Token: 0x040003CA RID: 970
		private readonly AstarPath astar;

		// Token: 0x040003CB RID: 971
		private readonly PathReturnQueue returnQueue;

		// Token: 0x040003CC RID: 972
		private PathHandler[] pathHandlers;

		// Token: 0x040003CD RID: 973
		private Thread[] threads;

		// Token: 0x040003CE RID: 974
		private bool multithreaded;

		// Token: 0x040003CF RID: 975
		private IEnumerator threadCoroutine;

		// Token: 0x040003D0 RID: 976
		private BlockableChannel<Path>.Receiver coroutineReceiver;

		// Token: 0x040003D1 RID: 977
		private readonly List<int> locks = new List<int>();

		// Token: 0x040003D2 RID: 978
		private int nextLockID;

		// Token: 0x040003D3 RID: 979
		private static readonly ProfilerMarker MarkerCalculatePath = new ProfilerMarker("Calculating Path");

		// Token: 0x040003D4 RID: 980
		private static readonly ProfilerMarker MarkerPreparePath = new ProfilerMarker("Prepare Path");

		// Token: 0x020000B5 RID: 181
		public struct GraphUpdateLock : IDisposable
		{
			// Token: 0x060005C0 RID: 1472 RVA: 0x0001BD12 File Offset: 0x00019F12
			public GraphUpdateLock(PathProcessor pathProcessor, bool block)
			{
				this.pathProcessor = pathProcessor;
				this.id = pathProcessor.Lock(block);
			}

			// Token: 0x170000FE RID: 254
			// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0001BD28 File Offset: 0x00019F28
			public bool Held
			{
				get
				{
					return this.pathProcessor != null && this.pathProcessor.locks.Contains(this.id);
				}
			}

			// Token: 0x060005C2 RID: 1474 RVA: 0x0001BD4A File Offset: 0x00019F4A
			public void Release()
			{
				this.pathProcessor.Unlock(this.id);
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x0001BD5D File Offset: 0x00019F5D
			void IDisposable.Dispose()
			{
				this.Release();
			}

			// Token: 0x040003D5 RID: 981
			private PathProcessor pathProcessor;

			// Token: 0x040003D6 RID: 982
			private int id;
		}
	}
}
