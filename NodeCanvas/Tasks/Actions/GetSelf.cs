using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200006A RID: 106
	[Category("✫ Blackboard")]
	[Description("Stores the agent gameobject on the blackboard.")]
	public class GetSelf : ActionTask
	{
		// Token: 0x06000208 RID: 520 RVA: 0x00008C7A File Offset: 0x00006E7A
		protected override void OnExecute()
		{
			BBParameter<GameObject> bbparameter = this.saveAs;
			Component agent = base.agent;
			bbparameter.value = ((agent != null) ? agent.gameObject : null);
			base.EndAction(true);
		}

		// Token: 0x0400014C RID: 332
		[BlackboardOnly]
		public BBParameter<GameObject> saveAs;
	}
}
