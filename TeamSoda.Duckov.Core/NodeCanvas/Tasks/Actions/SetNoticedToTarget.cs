using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000432 RID: 1074
	public class SetNoticedToTarget : ActionTask<AICharacterController>
	{
		// Token: 0x060026CF RID: 9935 RVA: 0x00086549 File Offset: 0x00084749
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060026D0 RID: 9936 RVA: 0x0008654C File Offset: 0x0008474C
		protected override string info
		{
			get
			{
				return "set noticed to";
			}
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x00086553 File Offset: 0x00084753
		protected override void OnExecute()
		{
			base.agent.SetNoticedToTarget(this.target.value);
			base.EndAction(true);
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x00086572 File Offset: 0x00084772
		protected override void OnStop()
		{
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x00086574 File Offset: 0x00084774
		protected override void OnPause()
		{
		}

		// Token: 0x04001A72 RID: 6770
		public BBParameter<DamageReceiver> target;
	}
}
