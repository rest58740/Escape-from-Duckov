using System;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x02000073 RID: 115
	public struct EventData<T> : IEventData
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0000A527 File Offset: 0x00008727
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x0000A52F File Offset: 0x0000872F
		public GameObject receiver { readonly get; private set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0000A538 File Offset: 0x00008738
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x0000A540 File Offset: 0x00008740
		public object sender { readonly get; private set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x0000A549 File Offset: 0x00008749
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x0000A551 File Offset: 0x00008751
		public T value { readonly get; private set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x0000A55A File Offset: 0x0000875A
		public object valueBoxed
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000A567 File Offset: 0x00008767
		public EventData(T value, GameObject receiver, object sender)
		{
			this.receiver = receiver;
			this.sender = sender;
			this.value = value;
		}
	}
}
