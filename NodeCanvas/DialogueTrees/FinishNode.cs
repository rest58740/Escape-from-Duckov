using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x02000100 RID: 256
	[Name("FINISH", 0)]
	[Category("Control")]
	[Description("End the dialogue in Success or Failure.\nNote: A Dialogue will anyway End in Succcess if it has reached a node without child connections. Thus this node is mostly useful if you want to end a Dialogue in Failure.")]
	[ParadoxNotion.Design.Icon("Halt", false, "")]
	[Color("6ebbff")]
	public class FinishNode : DTNode
	{
		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x00011488 File Offset: 0x0000F688
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0001148B File Offset: 0x0000F68B
		public override bool requireActorSelection
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001148E File Offset: 0x0000F68E
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			base.status = (Status)this.finishState;
			base.DLGTree.Stop(this.finishState == CompactStatus.Success);
			return base.status;
		}

		// Token: 0x040002E4 RID: 740
		public CompactStatus finishState = CompactStatus.Success;
	}
}
