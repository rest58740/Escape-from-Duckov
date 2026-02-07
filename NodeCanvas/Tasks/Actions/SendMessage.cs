using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000BE RID: 190
	[Category("✫ Reflected")]
	[Description("SendMessage to the agent, optionaly with an argument")]
	public class SendMessage : ActionTask<Transform>
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000CCCA File Offset: 0x0000AECA
		protected override string info
		{
			get
			{
				return string.Format("Message {0}()", this.methodName);
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000CCDC File Offset: 0x0000AEDC
		protected override void OnExecute()
		{
			base.agent.SendMessage(this.methodName.value);
			base.EndAction();
		}

		// Token: 0x04000246 RID: 582
		[RequiredField]
		public BBParameter<string> methodName;
	}
}
