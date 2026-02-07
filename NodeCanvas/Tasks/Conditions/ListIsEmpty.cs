using System;
using System.Collections;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000018 RID: 24
	[Category("✫ Blackboard/Lists")]
	public class ListIsEmpty : ConditionTask
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002A8B File Offset: 0x00000C8B
		protected override string info
		{
			get
			{
				return string.Format("{0} Is Empty", this.targetList);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002A9D File Offset: 0x00000C9D
		protected override bool OnCheck()
		{
			return this.targetList.value.Count == 0;
		}

		// Token: 0x04000036 RID: 54
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<IList> targetList;
	}
}
