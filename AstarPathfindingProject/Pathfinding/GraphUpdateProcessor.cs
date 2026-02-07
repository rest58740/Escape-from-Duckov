using System;
using System.Collections.Generic;
using System.Threading;
using Pathfinding.Jobs;
using Pathfinding.Pooling;
using Unity.Jobs;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200009E RID: 158
	internal class GraphUpdateProcessor
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00018714 File Offset: 0x00016914
		public bool IsAnyGraphUpdateQueued
		{
			get
			{
				return this.graphUpdateQueue.Count > 0;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00018724 File Offset: 0x00016924
		public bool IsAnyGraphUpdateInProgress
		{
			get
			{
				return this.anyGraphUpdateInProgress;
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001872C File Offset: 0x0001692C
		public GraphUpdateProcessor(AstarPath astar)
		{
			this.astar = astar;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001875C File Offset: 0x0001695C
		public AstarWorkItem GetWorkItem()
		{
			return new AstarWorkItem(new Action<IWorkItemContext>(this.QueueGraphUpdatesInternal), new Func<IWorkItemContext, bool, bool>(this.ProcessGraphUpdates));
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001877B File Offset: 0x0001697B
		public void AddToQueue(GraphUpdateObject ob)
		{
			this.graphUpdateQueue.Enqueue(ob);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00018789 File Offset: 0x00016989
		public void DiscardQueued()
		{
			while (this.graphUpdateQueue.Count > 0)
			{
				this.graphUpdateQueue.Dequeue().internalStage = -3;
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x000187B0 File Offset: 0x000169B0
		private void QueueGraphUpdatesInternal(IWorkItemContext context)
		{
			while (this.graphUpdateQueue.Count > 0)
			{
				GraphUpdateObject graphUpdateObject = this.graphUpdateQueue.Dequeue();
				this.pendingGraphUpdates.Add(graphUpdateObject);
				if (graphUpdateObject.internalStage != -2)
				{
					Debug.LogError("Expected remaining graph update to be pending");
				}
			}
			foreach (object obj in this.astar.data.GetUpdateableGraphs())
			{
				IUpdatableGraph updatableGraph = (IUpdatableGraph)obj;
				NavGraph navGraph = updatableGraph as NavGraph;
				List<GraphUpdateObject> list = ListPool<GraphUpdateObject>.Claim();
				for (int i = 0; i < this.pendingGraphUpdates.Count; i++)
				{
					GraphUpdateObject graphUpdateObject2 = this.pendingGraphUpdates[i];
					if (graphUpdateObject2.nnConstraint == null || graphUpdateObject2.nnConstraint.SuitableGraph((int)navGraph.graphIndex, navGraph))
					{
						list.Add(graphUpdateObject2);
					}
				}
				if (list.Count > 0)
				{
					IGraphUpdatePromise graphUpdatePromise = updatableGraph.ScheduleGraphUpdates(list);
					if (graphUpdatePromise != null)
					{
						IEnumerator<JobHandle> item = graphUpdatePromise.Prepare();
						this.pendingPromises.Add(new ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>(graphUpdatePromise, item));
					}
					else
					{
						ListPool<GraphUpdateObject>.Release(ref list);
					}
				}
				else
				{
					ListPool<GraphUpdateObject>.Release(ref list);
				}
			}
			context.PreUpdate();
			this.anyGraphUpdateInProgress = true;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00018904 File Offset: 0x00016B04
		private bool ProcessGraphUpdates(IWorkItemContext context, bool force)
		{
			if (this.pendingPromises.Count > 0)
			{
				try
				{
					if (GraphUpdateProcessor.ProcessGraphUpdatePromises(this.pendingPromises, context, force ? TimeSlice.Infinite : TimeSlice.MillisFromNow(2f)) != -1)
					{
						return false;
					}
				}
				catch (Exception innerException)
				{
					Debug.LogError(new Exception("Error while updating graphs", innerException));
					return false;
				}
				this.pendingPromises.Clear();
			}
			this.anyGraphUpdateInProgress = false;
			for (int i = 0; i < this.pendingGraphUpdates.Count; i++)
			{
				this.pendingGraphUpdates[i].internalStage = 0;
			}
			this.pendingGraphUpdates.Clear();
			return true;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x000189B8 File Offset: 0x00016BB8
		public static int ProcessGraphUpdatePromises(List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> promises, IGraphUpdateContext context, TimeSlice timeSlice)
		{
			int num = GraphUpdateProcessor.PrepareGraphUpdatePromises(promises, timeSlice);
			if (num == -1)
			{
				GraphUpdateProcessor.ApplyGraphUpdatePromises(promises, context);
			}
			return num;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000189CC File Offset: 0x00016BCC
		public static int PrepareGraphUpdatePromises(List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> promises, TimeSlice timeSlice)
		{
			TimeSlice timeSlice2 = default(TimeSlice);
			bool flag = !timeSlice.isInfinite;
			int num;
			for (;;)
			{
				num = -1;
				bool flag2 = false;
				for (int i = 0; i < promises.Count; i++)
				{
					ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>> valueTuple = promises[i];
					IGraphUpdatePromise item = valueTuple.Item1;
					IEnumerator<JobHandle> item2 = valueTuple.Item2;
					if (item2 != null)
					{
						if (flag)
						{
							JobHandle jobHandle = item2.Current;
							if (jobHandle.IsCompleted)
							{
								flag2 = true;
								jobHandle = item2.Current;
								jobHandle.Complete();
							}
							else
							{
								if (num == -1)
								{
									num = i;
									goto IL_B3;
								}
								goto IL_B3;
							}
						}
						else
						{
							JobHandle jobHandle = item2.Current;
							jobHandle.Complete();
						}
						try
						{
							if (item2.MoveNext())
							{
								if (num == -1)
								{
									num = i;
								}
							}
							else
							{
								promises[i] = new ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>(item, null);
							}
						}
						catch
						{
							promises[i] = new ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>(null, null);
							throw;
						}
					}
					IL_B3:;
				}
				if (num == -1)
				{
					return -1;
				}
				if (flag)
				{
					if (timeSlice.expired)
					{
						break;
					}
					if (flag2)
					{
						timeSlice2 = TimeSlice.MillisFromNow(0.1f);
					}
					else
					{
						if (timeSlice2.expired)
						{
							return num;
						}
						if (!flag2)
						{
							Thread.Yield();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00018AF4 File Offset: 0x00016CF4
		public static void ApplyGraphUpdatePromises(List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> promises, IGraphUpdateContext context)
		{
			for (int i = 0; i < promises.Count; i++)
			{
				IGraphUpdatePromise item = promises[i].Item1;
				if (item != null)
				{
					try
					{
						item.Apply(context);
					}
					finally
					{
					}
				}
			}
		}

		// Token: 0x04000345 RID: 837
		private readonly AstarPath astar;

		// Token: 0x04000346 RID: 838
		private bool anyGraphUpdateInProgress;

		// Token: 0x04000347 RID: 839
		private readonly Queue<GraphUpdateObject> graphUpdateQueue = new Queue<GraphUpdateObject>();

		// Token: 0x04000348 RID: 840
		private readonly List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> pendingPromises = new List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>>();

		// Token: 0x04000349 RID: 841
		private readonly List<GraphUpdateObject> pendingGraphUpdates = new List<GraphUpdateObject>();

		// Token: 0x0400034A RID: 842
		private static readonly ProfilerMarker MarkerSleep = new ProfilerMarker(ProfilerCategory.Loading, "Sleep");

		// Token: 0x0400034B RID: 843
		private static readonly ProfilerMarker MarkerCalculate = new ProfilerMarker("Calculating Graph Update");

		// Token: 0x0400034C RID: 844
		private static readonly ProfilerMarker MarkerApply = new ProfilerMarker("Applying Graph Update");
	}
}
