using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000052 RID: 82
	[Category("✫ Utility")]
	[Description("Check if an event is received and return true for one frame. Optionaly save the received event's value")]
	public class CheckEvent<T> : ConditionTask<GraphOwner>
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00007A00 File Offset: 0x00005C00
		protected override string info
		{
			get
			{
				return string.Format("Event [{0}]\n{1} = EventValue", this.eventName, this.saveEventValue);
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007A18 File Offset: 0x00005C18
		protected override void OnEnable()
		{
			base.router.onCustomEvent += this.OnCustomEvent;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007A31 File Offset: 0x00005C31
		protected override void OnDisable()
		{
			base.router.onCustomEvent -= this.OnCustomEvent;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007A4A File Offset: 0x00005C4A
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00007A50 File Offset: 0x00005C50
		private void OnCustomEvent(string eventName, IEventData data)
		{
			if (eventName.Equals(this.eventName.value, 5))
			{
				if (data is EventData<T>)
				{
					this.saveEventValue.value = ((EventData<T>)data).value;
				}
				else if (data.valueBoxed is T)
				{
					this.saveEventValue.value = (T)((object)data.valueBoxed);
				}
				base.YieldReturn(true);
			}
		}

		// Token: 0x040000FA RID: 250
		[RequiredField]
		public BBParameter<string> eventName;

		// Token: 0x040000FB RID: 251
		[BlackboardOnly]
		public BBParameter<T> saveEventValue;
	}
}
