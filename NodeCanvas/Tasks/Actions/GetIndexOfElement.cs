using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000070 RID: 112
	[Category("✫ Blackboard/Lists")]
	public class GetIndexOfElement<T> : ActionTask
	{
		// Token: 0x06000219 RID: 537 RVA: 0x00008ECC File Offset: 0x000070CC
		protected override void OnExecute()
		{
			this.saveIndexAs.value = this.targetList.value.IndexOf(this.targetElement.value);
			base.EndAction(true);
		}

		// Token: 0x04000156 RID: 342
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<List<T>> targetList;

		// Token: 0x04000157 RID: 343
		public BBParameter<T> targetElement;

		// Token: 0x04000158 RID: 344
		[BlackboardOnly]
		public BBParameter<int> saveIndexAs;
	}
}
