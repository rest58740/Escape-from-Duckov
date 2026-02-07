using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000109 RID: 265
	[GraphInfo(packageName = "NodeCanvas", docsURL = "https://nodecanvas.paradoxnotion.com/documentation/", resourcesURL = "https://nodecanvas.paradoxnotion.com/downloads/", forumsURL = "https://nodecanvas.paradoxnotion.com/forums-page/")]
	[CreateAssetMenu(menuName = "ParadoxNotion/NodeCanvas/Behaviour Tree Asset")]
	public class BehaviourTree : Graph
	{
		// Token: 0x0600058E RID: 1422 RVA: 0x00012196 File Offset: 0x00010396
		public override object OnDerivedDataSerialization()
		{
			return new BehaviourTree.DerivedSerializationData
			{
				repeat = this.repeat,
				updateInterval = this.updateInterval
			};
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x000121B5 File Offset: 0x000103B5
		public override void OnDerivedDataDeserialization(object data)
		{
			if (data is BehaviourTree.DerivedSerializationData)
			{
				this.repeat = ((BehaviourTree.DerivedSerializationData)data).repeat;
				this.updateInterval = ((BehaviourTree.DerivedSerializationData)data).updateInterval;
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000590 RID: 1424 RVA: 0x000121E4 File Offset: 0x000103E4
		// (remove) Token: 0x06000591 RID: 1425 RVA: 0x00012218 File Offset: 0x00010418
		public static event Action<BehaviourTree, Status> onRootStatusChanged;

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x0001224B File Offset: 0x0001044B
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x00012253 File Offset: 0x00010453
		public Status rootStatus
		{
			get
			{
				return this._rootStatus;
			}
			private set
			{
				if (this._rootStatus != value)
				{
					this._rootStatus = value;
					if (BehaviourTree.onRootStatusChanged != null)
					{
						BehaviourTree.onRootStatusChanged.Invoke(this, value);
					}
				}
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x00012278 File Offset: 0x00010478
		public override Type baseNodeType
		{
			get
			{
				return typeof(BTNode);
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00012284 File Offset: 0x00010484
		public override bool requiresAgent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00012287 File Offset: 0x00010487
		public override bool requiresPrimeNode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x0001228A File Offset: 0x0001048A
		public override bool isTree
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x0001228D File Offset: 0x0001048D
		public override bool allowBlackboardOverrides
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00012290 File Offset: 0x00010490
		public sealed override bool canAcceptVariableDrops
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x00012293 File Offset: 0x00010493
		public override PlanarDirection flowDirection
		{
			get
			{
				return PlanarDirection.Vertical;
			}
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00012296 File Offset: 0x00010496
		protected override void OnGraphStarted()
		{
			this.intervalCounter = this.updateInterval;
			this.rootStatus = base.primeNode.status;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x000122B8 File Offset: 0x000104B8
		protected override void OnGraphUpdate()
		{
			if (this.intervalCounter >= this.updateInterval)
			{
				this.intervalCounter = 0f;
				if (this.Tick(base.agent, base.blackboard) != Status.Running && !this.repeat)
				{
					base.Stop(this.rootStatus == Status.Success);
				}
			}
			if (this.updateInterval > 0f)
			{
				this.intervalCounter += Time.deltaTime;
			}
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001232C File Offset: 0x0001052C
		private Status Tick(Component agent, IBlackboard blackboard)
		{
			if (this.rootStatus != Status.Running)
			{
				base.primeNode.Reset(true);
			}
			return this.rootStatus = base.primeNode.Execute(agent, blackboard);
		}

		// Token: 0x04000301 RID: 769
		[NonSerialized]
		public bool repeat = true;

		// Token: 0x04000302 RID: 770
		[NonSerialized]
		public float updateInterval;

		// Token: 0x04000304 RID: 772
		private float intervalCounter;

		// Token: 0x04000305 RID: 773
		private Status _rootStatus = Status.Resting;

		// Token: 0x02000166 RID: 358
		[Serializable]
		private class DerivedSerializationData
		{
			// Token: 0x04000409 RID: 1033
			public bool repeat;

			// Token: 0x0400040A RID: 1034
			public float updateInterval;
		}
	}
}
