using System;
using Pathfinding.Sync;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000090 RID: 144
	internal class WorkItemProcessor : IWorkItemContext, IGraphUpdateContext
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600045F RID: 1119 RVA: 0x00017348 File Offset: 0x00015548
		// (remove) Token: 0x06000460 RID: 1120 RVA: 0x00017380 File Offset: 0x00015580
		public event Action OnGraphsUpdated;

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x000173B5 File Offset: 0x000155B5
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x000173BD File Offset: 0x000155BD
		public bool workItemsInProgressRightNow { get; private set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x000173C6 File Offset: 0x000155C6
		public bool anyQueued
		{
			get
			{
				return this.workItems.Count > 0;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x000173D6 File Offset: 0x000155D6
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x000173DE File Offset: 0x000155DE
		public bool workItemsInProgress { get; private set; }

		// Token: 0x06000466 RID: 1126 RVA: 0x000035CE File Offset: 0x000017CE
		void IWorkItemContext.QueueFloodFill()
		{
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000173E7 File Offset: 0x000155E7
		void IWorkItemContext.PreUpdate()
		{
			if (!this.preUpdateEventSent && !this.astar.isScanning)
			{
				this.preUpdateEventSent = true;
				GraphModifier.TriggerEvent(GraphModifier.EventType.PreUpdate);
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001740B File Offset: 0x0001560B
		void IWorkItemContext.SetGraphDirty(NavGraph graph)
		{
			this.astar.DirtyBounds(graph.bounds);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001741E File Offset: 0x0001561E
		void IGraphUpdateContext.DirtyBounds(Bounds bounds)
		{
			this.astar.DirtyBounds(bounds);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001742C File Offset: 0x0001562C
		internal void DirtyGraphs()
		{
			this.anyGraphsDirty = true;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00017435 File Offset: 0x00015635
		public void EnsureValidFloodFill()
		{
			this.astar.hierarchicalGraph.RecalculateIfNecessary();
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00017447 File Offset: 0x00015647
		public WorkItemProcessor(AstarPath astar)
		{
			this.astar = astar;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00017468 File Offset: 0x00015668
		public void AddWorkItem(AstarWorkItem item)
		{
			this.workItems.Enqueue(item);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00017478 File Offset: 0x00015678
		private bool ProcessWorkItems(bool force, bool sendEvents)
		{
			if (this.workItemsInProgressRightNow)
			{
				throw new Exception("Processing work items recursively. Please do not wait for other work items to be completed inside work items. If you think this is not caused by any of your scripts, this might be a bug.");
			}
			RWLock.LockSync lockSync = this.astar.LockGraphDataForWritingSync();
			this.astar.data.LockGraphStructure(true);
			Physics.SyncTransforms();
			Physics2D.SyncTransforms();
			this.workItemsInProgressRightNow = true;
			try
			{
				bool flag = false;
				bool flag2 = false;
				while (this.workItems.Count > 0)
				{
					if (!this.workItemsInProgress)
					{
						this.workItemsInProgress = true;
					}
					AstarWorkItem astarWorkItem = this.workItems[0];
					bool flag3;
					try
					{
						if (astarWorkItem.init != null)
						{
							astarWorkItem.init();
							astarWorkItem.init = null;
						}
						if (astarWorkItem.initWithContext != null)
						{
							astarWorkItem.initWithContext(this);
							astarWorkItem.initWithContext = null;
						}
						this.workItems[0] = astarWorkItem;
						if (astarWorkItem.update != null)
						{
							flag3 = astarWorkItem.update(force);
						}
						else
						{
							flag3 = (astarWorkItem.updateWithContext == null || astarWorkItem.updateWithContext(this, force));
						}
					}
					catch
					{
						this.workItems.Dequeue();
						throw;
					}
					if (!flag3)
					{
						if (force)
						{
							Debug.LogError("Misbehaving WorkItem. 'force'=true but the work item did not complete.\nIf force=true is passed to a WorkItem it should always return true.");
						}
						flag = true;
						break;
					}
					this.workItems.Dequeue();
					flag2 = true;
				}
				if (sendEvents && flag2)
				{
					if (this.anyGraphsDirty)
					{
						GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdateBeforeAreaRecalculation);
					}
					this.astar.offMeshLinks.Refresh();
					this.EnsureValidFloodFill();
					if (this.anyGraphsDirty)
					{
						GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
						if (this.OnGraphsUpdated != null)
						{
							this.OnGraphsUpdated();
						}
					}
				}
				if (flag)
				{
					return false;
				}
			}
			finally
			{
				lockSync.Unlock();
				this.astar.data.UnlockGraphStructure();
				this.workItemsInProgressRightNow = false;
			}
			this.anyGraphsDirty = false;
			this.preUpdateEventSent = false;
			this.workItemsInProgress = false;
			return true;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0001766C File Offset: 0x0001586C
		public bool ProcessWorkItemsForScan(bool force)
		{
			return this.ProcessWorkItems(force, false);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00017676 File Offset: 0x00015876
		public bool ProcessWorkItemsForUpdate(bool force)
		{
			return this.ProcessWorkItems(force, true);
		}

		// Token: 0x04000309 RID: 777
		private readonly AstarPath astar;

		// Token: 0x0400030A RID: 778
		private readonly WorkItemProcessor.IndexedQueue<AstarWorkItem> workItems = new WorkItemProcessor.IndexedQueue<AstarWorkItem>();

		// Token: 0x0400030B RID: 779
		private bool anyGraphsDirty = true;

		// Token: 0x0400030C RID: 780
		private bool preUpdateEventSent;

		// Token: 0x02000091 RID: 145
		private class IndexedQueue<T>
		{
			// Token: 0x170000C6 RID: 198
			public T this[int index]
			{
				get
				{
					if (index < 0 || index >= this.Count)
					{
						throw new IndexOutOfRangeException();
					}
					return this.buffer[(this.start + index) % this.buffer.Length];
				}
				set
				{
					if (index < 0 || index >= this.Count)
					{
						throw new IndexOutOfRangeException();
					}
					this.buffer[(this.start + index) % this.buffer.Length] = value;
				}
			}

			// Token: 0x170000C7 RID: 199
			// (get) Token: 0x06000473 RID: 1139 RVA: 0x000176E3 File Offset: 0x000158E3
			// (set) Token: 0x06000474 RID: 1140 RVA: 0x000176EB File Offset: 0x000158EB
			public int Count { get; private set; }

			// Token: 0x06000475 RID: 1141 RVA: 0x000176F4 File Offset: 0x000158F4
			public void Enqueue(T item)
			{
				if (this.Count == this.buffer.Length)
				{
					T[] array = new T[this.buffer.Length * 2];
					for (int i = 0; i < this.Count; i++)
					{
						array[i] = this[i];
					}
					this.buffer = array;
					this.start = 0;
				}
				this.buffer[(this.start + this.Count) % this.buffer.Length] = item;
				int count = this.Count;
				this.Count = count + 1;
			}

			// Token: 0x06000476 RID: 1142 RVA: 0x00017780 File Offset: 0x00015980
			public T Dequeue()
			{
				if (this.Count == 0)
				{
					throw new InvalidOperationException();
				}
				T result = this.buffer[this.start];
				this.start = (this.start + 1) % this.buffer.Length;
				int count = this.Count;
				this.Count = count - 1;
				return result;
			}

			// Token: 0x0400030E RID: 782
			private T[] buffer = new T[4];

			// Token: 0x0400030F RID: 783
			private int start;
		}
	}
}
