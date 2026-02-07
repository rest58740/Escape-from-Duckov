using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000073 RID: 115
	[Category("✫ Blackboard/Lists")]
	public class PickListElement<T> : ActionTask
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00008FC3 File Offset: 0x000071C3
		protected override string info
		{
			get
			{
				return string.Format("{0} = {1} [{2}]", this.saveAs, this.targetList, this.index);
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00008FE4 File Offset: 0x000071E4
		protected override void OnExecute()
		{
			if (this.index.value < 0 || this.index.value >= this.targetList.value.Count)
			{
				base.EndAction(false);
				return;
			}
			this.saveAs.value = this.targetList.value[this.index.value];
			base.EndAction(true);
		}

		// Token: 0x0400015E RID: 350
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<List<T>> targetList;

		// Token: 0x0400015F RID: 351
		public BBParameter<int> index;

		// Token: 0x04000160 RID: 352
		[BlackboardOnly]
		public BBParameter<T> saveAs;
	}
}
