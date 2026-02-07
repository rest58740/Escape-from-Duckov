using System;

namespace NodeCanvas.Framework
{
	// Token: 0x02000006 RID: 6
	[AttributeUsage(4)]
	[Obsolete("[EventReceiver] is no longer used. Please use the '.router' property to subscribe/unsubscribe to events (in OnExecute/OnStop for actions and OnEnable/OnDisable for conditions). For custom events, use '.router.onCustomEvent'.")]
	public class EventReceiverAttribute : Attribute
	{
		// Token: 0x0600001D RID: 29 RVA: 0x000023E6 File Offset: 0x000005E6
		public EventReceiverAttribute(params string[] args)
		{
			this.eventMessages = args;
		}

		// Token: 0x0400000C RID: 12
		public readonly string[] eventMessages;
	}
}
