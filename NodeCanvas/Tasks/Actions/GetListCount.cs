using System;
using System.Collections;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000071 RID: 113
	[Category("✫ Blackboard/Lists")]
	public class GetListCount : ActionTask
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00008F03 File Offset: 0x00007103
		protected override string info
		{
			get
			{
				return string.Format("{0} = {1}.Count", this.saveAs, this.targetList);
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00008F1B File Offset: 0x0000711B
		protected override void OnExecute()
		{
			this.saveAs.value = this.targetList.value.Count;
			base.EndAction(true);
		}

		// Token: 0x04000159 RID: 345
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<IList> targetList;

		// Token: 0x0400015A RID: 346
		[BlackboardOnly]
		public BBParameter<int> saveAs;
	}
}
