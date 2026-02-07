using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000CF RID: 207
	[Category("✫ Utility")]
	[Description("Send a graph event. If global is true, all graph owners in scene will receive this event. Use along with the 'Check Event' Condition")]
	public class SendEvent : ActionTask<GraphOwner>
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000388 RID: 904 RVA: 0x0000DF40 File Offset: 0x0000C140
		protected override string info
		{
			get
			{
				string[] array = new string[5];
				array[0] = (this.sendGlobal ? "Global " : "");
				array[1] = "Send Event [";
				int num = 2;
				BBParameter<string> bbparameter = this.eventName;
				array[num] = ((bbparameter != null) ? bbparameter.ToString() : null);
				array[3] = "]";
				int num2 = 4;
				string text;
				if (this.delay.value <= 0f)
				{
					text = "";
				}
				else
				{
					string text2 = " after ";
					BBParameter<float> bbparameter2 = this.delay;
					text = text2 + ((bbparameter2 != null) ? bbparameter2.ToString() : null) + " sec.";
				}
				array[num2] = text;
				return string.Concat(array);
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000DFD4 File Offset: 0x0000C1D4
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= this.delay.value)
			{
				if (this.sendGlobal)
				{
					Graph.SendGlobalEvent(this.eventName.value, null, this);
				}
				else
				{
					base.agent.SendEvent(this.eventName.value, null, this);
				}
				base.EndAction();
			}
		}

		// Token: 0x04000268 RID: 616
		[RequiredField]
		public BBParameter<string> eventName;

		// Token: 0x04000269 RID: 617
		public BBParameter<float> delay;

		// Token: 0x0400026A RID: 618
		public bool sendGlobal;
	}
}
