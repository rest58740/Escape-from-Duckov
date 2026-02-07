using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000118 RID: 280
	[Name("Step Sequencer", 0)]
	[Category("Composites")]
	[Description("In comparison to a normal Sequencer which executes all its children until one fails, Step Sequencer executes its children one-by-one per Step Sequencer execution. The executed child status is returned regardless of Success or Failure.")]
	[ParadoxNotion.Design.Icon("StepIterator", false, "")]
	[Color("bf7fff")]
	public class StepIterator : BTComposite
	{
		// Token: 0x060005F3 RID: 1523 RVA: 0x00012F86 File Offset: 0x00011186
		public override void OnGraphStarted()
		{
			this.current = 0;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00012F8F File Offset: 0x0001118F
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			this.current %= base.outConnections.Count;
			return base.outConnections[this.current].Execute(agent, blackboard);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00012FC1 File Offset: 0x000111C1
		protected override void OnReset()
		{
			this.current++;
		}

		// Token: 0x04000323 RID: 803
		private int current;
	}
}
