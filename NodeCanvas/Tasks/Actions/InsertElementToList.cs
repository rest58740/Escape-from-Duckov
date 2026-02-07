using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000072 RID: 114
	[Category("✫ Blackboard/Lists")]
	public class InsertElementToList<T> : ActionTask
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00008F47 File Offset: 0x00007147
		protected override string info
		{
			get
			{
				return string.Format("Insert {0} in {1} at {2}", this.targetElement, this.targetList, this.targetIndex);
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00008F68 File Offset: 0x00007168
		protected override void OnExecute()
		{
			int value = this.targetIndex.value;
			List<T> value2 = this.targetList.value;
			if (value < 0 || value >= value2.Count)
			{
				base.EndAction(false);
				return;
			}
			value2.Insert(value, this.targetElement.value);
			base.EndAction(true);
		}

		// Token: 0x0400015B RID: 347
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<List<T>> targetList;

		// Token: 0x0400015C RID: 348
		public BBParameter<T> targetElement;

		// Token: 0x0400015D RID: 349
		public BBParameter<int> targetIndex;
	}
}
