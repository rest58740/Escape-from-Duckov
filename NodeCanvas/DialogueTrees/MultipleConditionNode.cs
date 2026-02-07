using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x02000103 RID: 259
	[ParadoxNotion.Design.Icon("Selector", false, "")]
	[Name("Multiple Task Condition", 0)]
	[Category("Branch")]
	[Description("Will continue with the first child node which condition returns true. The Dialogue Actor selected will be used for the checks")]
	[Color("b3ff7f")]
	public class MultipleConditionNode : DTNode
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x00011687 File Offset: 0x0000F887
		public override int maxOutConnections
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001168A File Offset: 0x0000F88A
		public override void OnChildConnected(int index)
		{
			if (this.conditions.Count < base.outConnections.Count)
			{
				this.conditions.Insert(index, null);
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000116B1 File Offset: 0x0000F8B1
		public override void OnChildDisconnected(int index)
		{
			this.conditions.RemoveAt(index);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x000116C0 File Offset: 0x0000F8C0
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			if (base.outConnections.Count == 0)
			{
				return base.Error("There are no connections on the Dialogue Condition Node");
			}
			for (int i = 0; i < base.outConnections.Count; i++)
			{
				if (this.conditions[i] == null || this.conditions[i].CheckOnce(base.finalActor.transform, base.graphBlackboard))
				{
					base.DLGTree.Continue(i);
					return Status.Success;
				}
			}
			base.DLGTree.Stop(false);
			return Status.Failure;
		}

		// Token: 0x040002E9 RID: 745
		[SerializeField]
		[Node.AutoSortWithChildrenConnections]
		private List<ConditionTask> conditions = new List<ConditionTask>();
	}
}
