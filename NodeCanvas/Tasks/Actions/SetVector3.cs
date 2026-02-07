using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000086 RID: 134
	[Category("✫ Blackboard")]
	[Description("Set a blackboard Vector3 variable")]
	public class SetVector3 : ActionTask
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00009880 File Offset: 0x00007A80
		protected override string info
		{
			get
			{
				return string.Format("{0} {1} {2}{3}", new object[]
				{
					this.valueA,
					OperationTools.GetOperationString(this.operation),
					this.valueB,
					this.perSecond ? " Per Second" : ""
				});
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x000098D4 File Offset: 0x00007AD4
		protected override void OnExecute()
		{
			this.valueA.value = OperationTools.Operate(this.valueA.value, this.valueB.value, this.operation, this.perSecond ? Time.deltaTime : 1f);
			base.EndAction();
		}

		// Token: 0x04000189 RID: 393
		[BlackboardOnly]
		public BBParameter<Vector3> valueA;

		// Token: 0x0400018A RID: 394
		public OperationMethod operation;

		// Token: 0x0400018B RID: 395
		public BBParameter<Vector3> valueB;

		// Token: 0x0400018C RID: 396
		public bool perSecond;
	}
}
