using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000053 RID: 83
	[Category("✫ Utility")]
	[Description("Check if an event is received and it's value is equal to specified value, then return true for one frame")]
	public class CheckEventValue<T> : ConditionTask<GraphOwner>
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00007AC6 File Offset: 0x00005CC6
		protected override string info
		{
			get
			{
				return string.Format("Event [{0}].value == {1}", this.eventName, this.value);
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00007ADE File Offset: 0x00005CDE
		protected override void OnEnable()
		{
			base.router.onCustomEvent += this.OnCustomEvent;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00007AF7 File Offset: 0x00005CF7
		protected override void OnDisable()
		{
			base.router.onCustomEvent -= this.OnCustomEvent;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00007B10 File Offset: 0x00005D10
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00007B13 File Offset: 0x00005D13
		private void OnCustomEvent(string eventName, IEventData msg)
		{
			if (eventName.Equals(this.eventName.value, 5) && ObjectUtils.AnyEquals(msg.valueBoxed, this.value.value))
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x040000FC RID: 252
		[RequiredField]
		public BBParameter<string> eventName;

		// Token: 0x040000FD RID: 253
		[Name("Compare Value To", 0)]
		public BBParameter<T> value;
	}
}
