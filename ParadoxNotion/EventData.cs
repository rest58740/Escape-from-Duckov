using System;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x02000072 RID: 114
	public struct EventData : IEventData
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0000A4BE File Offset: 0x000086BE
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x0000A4C6 File Offset: 0x000086C6
		public GameObject receiver { readonly get; private set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0000A4CF File Offset: 0x000086CF
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x0000A4D7 File Offset: 0x000086D7
		public object sender { readonly get; private set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x0000A4E0 File Offset: 0x000086E0
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x0000A4E8 File Offset: 0x000086E8
		public object value { readonly get; private set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x0000A4F1 File Offset: 0x000086F1
		public object valueBoxed
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000A4F9 File Offset: 0x000086F9
		public EventData(object value, GameObject receiver, object sender)
		{
			this.value = value;
			this.receiver = receiver;
			this.sender = sender;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000A510 File Offset: 0x00008710
		public EventData(GameObject receiver, object sender)
		{
			this.value = null;
			this.receiver = receiver;
			this.sender = sender;
		}
	}
}
