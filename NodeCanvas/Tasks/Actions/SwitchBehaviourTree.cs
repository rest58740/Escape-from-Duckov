using System;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000D4 RID: 212
	[Category("✫ Utility")]
	[Description("Switch the entire Behaviour Tree of BehaviourTreeOwner")]
	public class SwitchBehaviourTree : ActionTask<BehaviourTreeOwner>
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0000E40E File Offset: 0x0000C60E
		protected override string info
		{
			get
			{
				return string.Format("Switch Behaviour {0}", this.behaviourTree);
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000E420 File Offset: 0x0000C620
		protected override void OnExecute()
		{
			base.agent.SwitchBehaviour(this.behaviourTree.value);
			base.EndAction();
		}

		// Token: 0x0400027A RID: 634
		[RequiredField]
		public BBParameter<BehaviourTree> behaviourTree;
	}
}
