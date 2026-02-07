using System;
using System.Collections;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200006E RID: 110
	[Category("✫ Blackboard/Lists")]
	public class ClearList : ActionTask
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00008DAA File Offset: 0x00006FAA
		protected override string info
		{
			get
			{
				return string.Format("Clear List {0}", this.targetList);
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00008DBC File Offset: 0x00006FBC
		protected override void OnExecute()
		{
			this.targetList.value.Clear();
			base.EndAction(true);
		}

		// Token: 0x04000153 RID: 339
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<IList> targetList;
	}
}
