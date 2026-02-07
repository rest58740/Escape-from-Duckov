using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000051 RID: 81
	[Category("✫ Utility")]
	[Description("Check if an event is received and return true for one frame")]
	public class CheckEvent : ConditionTask<GraphOwner>
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000798A File Offset: 0x00005B8A
		protected override string info
		{
			get
			{
				return "[" + this.eventName.ToString() + "]";
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000079A6 File Offset: 0x00005BA6
		protected override void OnEnable()
		{
			base.router.onCustomEvent += this.OnCustomEvent;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000079BF File Offset: 0x00005BBF
		protected override void OnDisable()
		{
			base.router.onCustomEvent -= this.OnCustomEvent;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000079D8 File Offset: 0x00005BD8
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000079DB File Offset: 0x00005BDB
		private void OnCustomEvent(string eventName, IEventData data)
		{
			if (eventName.Equals(this.eventName.value, 5))
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x040000F9 RID: 249
		[RequiredField]
		public BBParameter<string> eventName;
	}
}
