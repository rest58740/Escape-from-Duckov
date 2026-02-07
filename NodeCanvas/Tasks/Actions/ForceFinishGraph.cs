using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000CC RID: 204
	[Category("✫ Utility")]
	[Description("Force Finish the current graph this Task is assigned to.")]
	public class ForceFinishGraph : ActionTask
	{
		// Token: 0x0600037E RID: 894 RVA: 0x0000DDB8 File Offset: 0x0000BFB8
		protected override void OnExecute()
		{
			Graph graph = base.ownerSystem as Graph;
			if (graph != null)
			{
				graph.Stop(this.finishStatus == CompactStatus.Success);
			}
			base.EndAction(this.finishStatus == CompactStatus.Success);
		}

		// Token: 0x04000265 RID: 613
		public CompactStatus finishStatus = CompactStatus.Success;
	}
}
