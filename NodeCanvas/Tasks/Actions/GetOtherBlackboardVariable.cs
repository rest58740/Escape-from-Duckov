using System;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200006B RID: 107
	[Category("✫ Blackboard")]
	[Description("Use this to get a variable on any blackboard by overriding the agent")]
	public class GetOtherBlackboardVariable : ActionTask<Blackboard>
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00008CA8 File Offset: 0x00006EA8
		protected override string info
		{
			get
			{
				return string.Format("{0} = {1}", this.saveAs, this.targetVariableName);
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00008CC0 File Offset: 0x00006EC0
		protected override void OnExecute()
		{
			Variable variable = base.agent.GetVariable(this.targetVariableName.value, null);
			if (variable == null)
			{
				base.EndAction(false);
				return;
			}
			this.saveAs.value = variable.value;
			base.EndAction(true);
		}

		// Token: 0x0400014D RID: 333
		[RequiredField]
		public BBParameter<string> targetVariableName;

		// Token: 0x0400014E RID: 334
		[BlackboardOnly]
		public BBObjectParameter saveAs;
	}
}
