using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000076 RID: 118
	[Category("✫ Blackboard/Lists")]
	public class SetListElement<T> : ActionTask
	{
		// Token: 0x0600022A RID: 554 RVA: 0x00009124 File Offset: 0x00007324
		protected override void OnExecute()
		{
			if (this.index.value < 0 || this.index.value >= this.targetList.value.Count)
			{
				base.EndAction(false);
				return;
			}
			this.targetList.value[this.index.value] = this.newValue.value;
			base.EndAction(true);
		}

		// Token: 0x04000165 RID: 357
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<List<T>> targetList;

		// Token: 0x04000166 RID: 358
		public BBParameter<int> index;

		// Token: 0x04000167 RID: 359
		public BBParameter<T> newValue;
	}
}
