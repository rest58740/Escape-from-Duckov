using System;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000E6 RID: 230
	[Name("Parallel Sub Behaviour Tree", -1)]
	[Description("Execute a Sub Behaviour Tree in parallel and for as long as this FSM is running.")]
	[Category("SubGraphs")]
	[Color("ff64cb")]
	public class ConcurrentSubTree : FSMNodeNested<BehaviourTree>, IUpdatable, IGraphElement
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0000FA13 File Offset: 0x0000DC13
		public override string name
		{
			get
			{
				return base.name.ToUpper();
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x0000FA20 File Offset: 0x0000DC20
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0000FA23 File Offset: 0x0000DC23
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x0000FA26 File Offset: 0x0000DC26
		public override bool allowAsPrime
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0000FA29 File Offset: 0x0000DC29
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x0000FA36 File Offset: 0x0000DC36
		public override BehaviourTree subGraph
		{
			get
			{
				return this._subTree.value;
			}
			set
			{
				this._subTree.value = value;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0000FA44 File Offset: 0x0000DC44
		public override BBParameter subGraphParameter
		{
			get
			{
				return this._subTree;
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000FA4C File Offset: 0x0000DC4C
		public override void OnGraphStarted()
		{
			if (this.subGraph == null)
			{
				return;
			}
			base.status = Status.Running;
			this.TryStartSubGraph(base.graphAgent, delegate(bool result)
			{
				base.status = (result ? Status.Success : Status.Failure);
			});
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000FA7D File Offset: 0x0000DC7D
		void IUpdatable.Update()
		{
			this.TryUpdateSubGraph();
		}

		// Token: 0x040002A0 RID: 672
		[SerializeField]
		[ExposeField]
		[Name("Parallel Tree", 0)]
		protected BBParameter<BehaviourTree> _subTree;
	}
}
