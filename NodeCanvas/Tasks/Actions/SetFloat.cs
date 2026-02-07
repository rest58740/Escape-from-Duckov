using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000080 RID: 128
	[Category("✫ Blackboard")]
	[Description("Set a blackboard float variable")]
	public class SetFloat : ActionTask
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00009548 File Offset: 0x00007748
		protected override string info
		{
			get
			{
				return string.Format("{0} {1} {2}{3}", new object[]
				{
					this.valueA,
					OperationTools.GetOperationString(this.Operation),
					this.valueB,
					this.perSecond ? " Per Second" : ""
				});
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000959C File Offset: 0x0000779C
		protected override void OnExecute()
		{
			this.valueA.value = OperationTools.Operate(this.valueA.value, this.valueB.value, this.Operation, this.perSecond ? Time.deltaTime : 1f);
			base.EndAction(true);
		}

		// Token: 0x04000178 RID: 376
		[BlackboardOnly]
		public BBParameter<float> valueA;

		// Token: 0x04000179 RID: 377
		public OperationMethod Operation;

		// Token: 0x0400017A RID: 378
		public BBParameter<float> valueB;

		// Token: 0x0400017B RID: 379
		public bool perSecond;
	}
}
