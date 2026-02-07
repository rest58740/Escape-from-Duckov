using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000BF RID: 191
	[Category("✫ Reflected")]
	[Description("SendMessage to the agent, optionaly with an argument")]
	public class SendMessage<T> : ActionTask<Transform>
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000CD02 File Offset: 0x0000AF02
		protected override string info
		{
			get
			{
				return string.Format("Message {0}({1})", this.methodName, this.argument.ToString());
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000CD20 File Offset: 0x0000AF20
		protected override void OnExecute()
		{
			if (this.argument.isNull)
			{
				base.agent.SendMessage(this.methodName.value);
			}
			else
			{
				base.agent.SendMessage(this.methodName.value, this.argument.value);
			}
			base.EndAction();
		}

		// Token: 0x04000247 RID: 583
		[RequiredField]
		public BBParameter<string> methodName;

		// Token: 0x04000248 RID: 584
		public BBParameter<T> argument;
	}
}
