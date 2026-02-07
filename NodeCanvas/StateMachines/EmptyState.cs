using System;
using ParadoxNotion.Design;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000E7 RID: 231
	[Description("This node has no functionality and you can use this for organization.\nIn comparison to an empty Action State, Transitions here are immediately evaluated in the same frame that this node is entered.")]
	[Color("6ebbff")]
	[Name("Pass", 98)]
	public class EmptyState : FSMState
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0000FA9D File Offset: 0x0000DC9D
		public override string name
		{
			get
			{
				return base.name.ToUpper();
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000FAAA File Offset: 0x0000DCAA
		protected override void OnEnter()
		{
			base.Finish();
			base.CheckTransitions();
		}
	}
}
