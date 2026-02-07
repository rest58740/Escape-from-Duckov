using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000420 RID: 1056
	public class CheckNoticed : ConditionTask<AICharacterController>
	{
		// Token: 0x06002668 RID: 9832 RVA: 0x0008512D File Offset: 0x0008332D
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x00085130 File Offset: 0x00083330
		protected override void OnEnable()
		{
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x00085132 File Offset: 0x00083332
		protected override void OnDisable()
		{
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x00085134 File Offset: 0x00083334
		protected override bool OnCheck()
		{
			bool result = base.agent.isNoticing(this.noticedTimeThreshold);
			if (this.resetNotice)
			{
				base.agent.noticed = false;
			}
			return result;
		}

		// Token: 0x04001A2A RID: 6698
		public float noticedTimeThreshold = 0.2f;

		// Token: 0x04001A2B RID: 6699
		public bool resetNotice;
	}
}
