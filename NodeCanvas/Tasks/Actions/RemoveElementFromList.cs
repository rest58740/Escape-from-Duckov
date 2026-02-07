using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000075 RID: 117
	[Category("✫ Blackboard/Lists")]
	[Description("Remove an element from the target list")]
	public class RemoveElementFromList<T> : ActionTask
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000227 RID: 551 RVA: 0x000090DC File Offset: 0x000072DC
		protected override string info
		{
			get
			{
				return string.Format("Remove {0} From {1}", this.targetElement, this.targetList);
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000090F4 File Offset: 0x000072F4
		protected override void OnExecute()
		{
			this.targetList.value.Remove(this.targetElement.value);
			base.EndAction(true);
		}

		// Token: 0x04000163 RID: 355
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<List<T>> targetList;

		// Token: 0x04000164 RID: 356
		public BBParameter<T> targetElement;
	}
}
