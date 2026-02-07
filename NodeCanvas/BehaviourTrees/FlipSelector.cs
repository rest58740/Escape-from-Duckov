using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000111 RID: 273
	[Category("Composites")]
	[Description("Works like a normal Selector, but when a child returns Success, that child will be moved to the end.\nAs a result, previously Failed children will always be checked first and recently Successful children last.")]
	[ParadoxNotion.Design.Icon("FlipSelector", false, "")]
	[Color("b3ff7f")]
	public class FlipSelector : BTComposite
	{
		// Token: 0x060005D2 RID: 1490 RVA: 0x000126E8 File Offset: 0x000108E8
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			for (int i = this.current; i < base.outConnections.Count; i++)
			{
				base.status = base.outConnections[i].Execute(agent, blackboard);
				if (base.status == Status.Running)
				{
					this.current = i;
					return Status.Running;
				}
				if (base.status == Status.Success)
				{
					this.SendToBack(i);
					return Status.Success;
				}
			}
			return Status.Failure;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00012750 File Offset: 0x00010950
		private void SendToBack(int i)
		{
			Connection connection = base.outConnections[i];
			base.outConnections.RemoveAt(i);
			base.outConnections.Add(connection);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00012782 File Offset: 0x00010982
		protected override void OnReset()
		{
			this.current = 0;
		}

		// Token: 0x0400030C RID: 780
		private int current;
	}
}
