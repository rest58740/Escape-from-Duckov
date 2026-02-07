using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000433 RID: 1075
	public class SetRun : ActionTask<AICharacterController>
	{
		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x0008657E File Offset: 0x0008477E
		protected override string info
		{
			get
			{
				return string.Format("SetRun:{0}", this.run.value);
			}
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x0008659A File Offset: 0x0008479A
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x0008659D File Offset: 0x0008479D
		protected override void OnExecute()
		{
			base.agent.CharacterMainControl.SetRunInput(this.run.value);
			base.EndAction(true);
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x000865C1 File Offset: 0x000847C1
		protected override void OnStop()
		{
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x000865C3 File Offset: 0x000847C3
		protected override void OnPause()
		{
		}

		// Token: 0x04001A73 RID: 6771
		public BBParameter<bool> run;
	}
}
