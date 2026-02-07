using System;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000D5 RID: 213
	[Category("✫ Utility")]
	[Description("Switch the entire FSM of FSMTreeOwner")]
	public class SwitchFSM : ActionTask<FSMOwner>
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0000E446 File Offset: 0x0000C646
		protected override string info
		{
			get
			{
				return string.Format("Switch FSM {0}", this.fsm);
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000E458 File Offset: 0x0000C658
		protected override void OnExecute()
		{
			base.agent.SwitchBehaviour(this.fsm.value);
			base.EndAction();
		}

		// Token: 0x0400027B RID: 635
		[RequiredField]
		public BBParameter<FSM> fsm;
	}
}
