using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000123 RID: 291
	[Name("Remap", 0)]
	[Category("Decorators")]
	[Description("Remaps the child status to another status. Used to either invert the child's return status or to always return a specific status.")]
	[ParadoxNotion.Design.Icon("Remap", false, "")]
	public class Remapper : BTDecorator
	{
		// Token: 0x06000626 RID: 1574 RVA: 0x00013990 File Offset: 0x00011B90
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.decoratedConnection == null)
			{
				return Status.Optional;
			}
			base.status = base.decoratedConnection.Execute(agent, blackboard);
			Status status = base.status;
			if (status == Status.Failure)
			{
				return (Status)this.failureRemap;
			}
			if (status == Status.Success)
			{
				return (Status)this.successRemap;
			}
			return base.status;
		}

		// Token: 0x04000347 RID: 839
		public Remapper.RemapStatus successRemap = Remapper.RemapStatus.Success;

		// Token: 0x04000348 RID: 840
		public Remapper.RemapStatus failureRemap;

		// Token: 0x02000173 RID: 371
		public enum RemapStatus
		{
			// Token: 0x04000433 RID: 1075
			Failure,
			// Token: 0x04000434 RID: 1076
			Success
		}
	}
}
