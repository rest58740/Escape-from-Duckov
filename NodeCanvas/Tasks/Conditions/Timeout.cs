using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000058 RID: 88
	[Category("✫ Utility")]
	[Description("Will return true after a specific amount of time has passed and false while still counting down")]
	public class Timeout : ConditionTask
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00007D51 File Offset: 0x00005F51
		private float elapsedTime
		{
			get
			{
				return base.ownerSystem.elapsedTime - this.startTime;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00007D68 File Offset: 0x00005F68
		protected override string info
		{
			get
			{
				return string.Format("Timeout {0}/{1}", this.elapsedTime.ToString("0.00"), this.timeout.ToString());
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00007D9D File Offset: 0x00005F9D
		protected override void OnEnable()
		{
			this.startTime = base.ownerSystem.elapsedTime;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00007DB0 File Offset: 0x00005FB0
		protected override bool OnCheck()
		{
			return this.elapsedTime >= this.timeout.value;
		}

		// Token: 0x04000104 RID: 260
		public BBParameter<float> timeout = 1f;

		// Token: 0x04000105 RID: 261
		private float startTime;
	}
}
