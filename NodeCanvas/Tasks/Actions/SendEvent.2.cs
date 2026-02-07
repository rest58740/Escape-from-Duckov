using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000D0 RID: 208
	[Category("✫ Utility")]
	[Description("Send a graph event with T value. If global is true, all graph owners in scene will receive this event. Use along with the 'Check Event' Condition")]
	public class SendEvent<T> : ActionTask<GraphOwner>
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600038B RID: 907 RVA: 0x0000E038 File Offset: 0x0000C238
		protected override string info
		{
			get
			{
				string text = "{0} Event [{1}] ({2}){3}";
				object[] array = new object[4];
				array[0] = (this.sendGlobal ? "Global " : "");
				array[1] = this.eventName;
				array[2] = this.eventValue;
				int num = 3;
				object obj;
				if (this.delay.value <= 0f)
				{
					obj = "";
				}
				else
				{
					string text2 = " after ";
					BBParameter<float> bbparameter = this.delay;
					obj = text2 + ((bbparameter != null) ? bbparameter.ToString() : null) + " sec.";
				}
				array[num] = obj;
				return string.Format(text, array);
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000E0BC File Offset: 0x0000C2BC
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= this.delay.value)
			{
				if (this.sendGlobal)
				{
					Graph.SendGlobalEvent<T>(this.eventName.value, this.eventValue.value, this);
				}
				else
				{
					base.agent.SendEvent<T>(this.eventName.value, this.eventValue.value, this);
				}
				base.EndAction();
			}
		}

		// Token: 0x0400026B RID: 619
		[RequiredField]
		public BBParameter<string> eventName;

		// Token: 0x0400026C RID: 620
		public BBParameter<T> eventValue;

		// Token: 0x0400026D RID: 621
		public BBParameter<float> delay;

		// Token: 0x0400026E RID: 622
		public bool sendGlobal;
	}
}
