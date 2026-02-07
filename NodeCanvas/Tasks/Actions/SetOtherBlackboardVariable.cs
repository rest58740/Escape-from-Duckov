using System;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000084 RID: 132
	[Category("✫ Blackboard")]
	[Description("Use this to set a variable on any blackboard by overriding the agent")]
	public class SetOtherBlackboardVariable : ActionTask<Blackboard>
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000251 RID: 593 RVA: 0x000097C7 File Offset: 0x000079C7
		protected override string info
		{
			get
			{
				return string.Format("<b>{0}</b> = {1}", this.targetVariableName.ToString(), (this.newValue != null) ? this.newValue.ToString() : "");
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x000097F8 File Offset: 0x000079F8
		protected override void OnExecute()
		{
			base.agent.SetVariableValue(this.targetVariableName.value, this.newValue.value);
			base.EndAction();
		}

		// Token: 0x04000185 RID: 389
		[RequiredField]
		public BBParameter<string> targetVariableName;

		// Token: 0x04000186 RID: 390
		public BBObjectParameter newValue;
	}
}
