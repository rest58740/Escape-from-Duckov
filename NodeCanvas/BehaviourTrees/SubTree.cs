using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200012C RID: 300
	[Name("Sub Tree", 0)]
	[Description("Executes a sub Behaviour Tree. The status of the root node in the SubTree will be returned.")]
	[ParadoxNotion.Design.Icon("BT", false, "")]
	[DropReferenceType(typeof(BehaviourTree))]
	public class SubTree : BTNodeNested<BehaviourTree>
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00013FBD File Offset: 0x000121BD
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x00013FCA File Offset: 0x000121CA
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

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00013FD8 File Offset: 0x000121D8
		public override BBParameter subGraphParameter
		{
			get
			{
				return this._subTree;
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00013FE0 File Offset: 0x000121E0
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (this.subGraph == null || this.subGraph.primeNode == null)
			{
				return Status.Optional;
			}
			if (base.status == Status.Resting)
			{
				this.TryStartSubGraph(agent, null);
			}
			base.currentInstance.UpdateGraph(base.graph.deltaTime);
			if (base.currentInstance.repeat && base.currentInstance.rootStatus != Status.Running)
			{
				this.TryReadAndUnbindMappedVariables();
			}
			return base.currentInstance.rootStatus;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001405E File Offset: 0x0001225E
		protected override void OnReset()
		{
			if (base.currentInstance != null)
			{
				base.currentInstance.Stop(true);
			}
		}

		// Token: 0x04000358 RID: 856
		[SerializeField]
		[ExposeField]
		private BBParameter<BehaviourTree> _subTree;
	}
}
