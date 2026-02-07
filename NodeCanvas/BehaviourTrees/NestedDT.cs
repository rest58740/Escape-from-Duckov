using System;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200012A RID: 298
	[Name("Sub Dialogue", 0)]
	[Description("Executes a sub Dialogue Tree. Returns Running while the sub Dialogue Tree is active. You can Finish the Dialogue Tree with the 'Finish' node and return Success or Failure.")]
	[ParadoxNotion.Design.Icon("Dialogue", false, "")]
	[DropReferenceType(typeof(DialogueTree))]
	public class NestedDT : BTNodeNested<DialogueTree>
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00013DAF File Offset: 0x00011FAF
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x00013DBC File Offset: 0x00011FBC
		public override DialogueTree subGraph
		{
			get
			{
				return this._nestedDialogueTree.value;
			}
			set
			{
				this._nestedDialogueTree.value = value;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00013DCA File Offset: 0x00011FCA
		public override BBParameter subGraphParameter
		{
			get
			{
				return this._nestedDialogueTree;
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00013DD4 File Offset: 0x00011FD4
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (this.subGraph == null || this.subGraph.primeNode == null)
			{
				return Status.Optional;
			}
			if (base.status == Status.Resting)
			{
				base.status = Status.Running;
				this.TryStartSubGraph(agent, new Action<bool>(this.OnDLGFinished));
			}
			if (base.status == Status.Running)
			{
				base.currentInstance.UpdateGraph(base.graph.deltaTime);
			}
			return base.status;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00013E47 File Offset: 0x00012047
		private void OnDLGFinished(bool success)
		{
			if (base.status == Status.Running)
			{
				base.status = (success ? Status.Success : Status.Failure);
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00013E5F File Offset: 0x0001205F
		protected override void OnReset()
		{
			if (base.currentInstance != null)
			{
				base.currentInstance.Stop(true);
			}
		}

		// Token: 0x04000354 RID: 852
		[SerializeField]
		[ExposeField]
		[Name("Sub Tree", 0)]
		private BBParameter<DialogueTree> _nestedDialogueTree;
	}
}
