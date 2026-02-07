using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200006D RID: 109
	[Category("✫ Blackboard/Lists")]
	public class AddElementToList<T> : ActionTask
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00008D67 File Offset: 0x00006F67
		protected override string info
		{
			get
			{
				return string.Format("Add {0} In {1}", this.targetElement, this.targetList);
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00008D7F File Offset: 0x00006F7F
		protected override void OnExecute()
		{
			this.targetList.value.Add(this.targetElement.value);
			base.EndAction();
		}

		// Token: 0x04000151 RID: 337
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<List<T>> targetList;

		// Token: 0x04000152 RID: 338
		public BBParameter<T> targetElement;
	}
}
