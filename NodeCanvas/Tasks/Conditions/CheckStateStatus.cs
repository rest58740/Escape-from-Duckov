using System;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000055 RID: 85
	[Category("✫ Utility")]
	[Description("Check the parent state status. This condition is only meant to be used along with an FSM system.")]
	public class CheckStateStatus : ConditionTask
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00007C4A File Offset: 0x00005E4A
		protected override string info
		{
			get
			{
				return string.Format("State == {0}", this.status);
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00007C64 File Offset: 0x00005E64
		protected override bool OnCheck()
		{
			FSM fsm = base.ownerSystem as FSM;
			return fsm != null && fsm.currentState.status == (Status)this.status;
		}

		// Token: 0x04000100 RID: 256
		public CompactStatus status = CompactStatus.Success;
	}
}
