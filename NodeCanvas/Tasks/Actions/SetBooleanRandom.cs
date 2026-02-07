using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200007E RID: 126
	[Category("✫ Blackboard")]
	[Description("Set a blackboard boolean variable at random between min and max value")]
	public class SetBooleanRandom : ActionTask
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000947C File Offset: 0x0000767C
		protected override string info
		{
			get
			{
				string text = "Set ";
				BBParameter<bool> bbparameter = this.boolVariable;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null) + " Random";
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000949F File Offset: 0x0000769F
		protected override void OnExecute()
		{
			this.boolVariable.value = (Random.Range(0, 2) != 0);
			base.EndAction();
		}

		// Token: 0x04000175 RID: 373
		[BlackboardOnly]
		public BBParameter<bool> boolVariable;
	}
}
