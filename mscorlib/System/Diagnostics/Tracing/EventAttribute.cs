using System;

namespace System.Diagnostics.Tracing
{
	// Token: 0x020009EC RID: 2540
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class EventAttribute : Attribute
	{
		// Token: 0x06005A9F RID: 23199 RVA: 0x00134393 File Offset: 0x00132593
		public EventAttribute(int eventId)
		{
			this.EventId = eventId;
		}

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06005AA0 RID: 23200 RVA: 0x001343A2 File Offset: 0x001325A2
		// (set) Token: 0x06005AA1 RID: 23201 RVA: 0x001343AA File Offset: 0x001325AA
		public int EventId { get; private set; }

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x06005AA2 RID: 23202 RVA: 0x001343B3 File Offset: 0x001325B3
		// (set) Token: 0x06005AA3 RID: 23203 RVA: 0x001343BB File Offset: 0x001325BB
		public EventActivityOptions ActivityOptions { get; set; }

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x06005AA4 RID: 23204 RVA: 0x001343C4 File Offset: 0x001325C4
		// (set) Token: 0x06005AA5 RID: 23205 RVA: 0x001343CC File Offset: 0x001325CC
		public EventLevel Level { get; set; }

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x06005AA6 RID: 23206 RVA: 0x001343D5 File Offset: 0x001325D5
		// (set) Token: 0x06005AA7 RID: 23207 RVA: 0x001343DD File Offset: 0x001325DD
		public EventKeywords Keywords { get; set; }

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x06005AA8 RID: 23208 RVA: 0x001343E6 File Offset: 0x001325E6
		// (set) Token: 0x06005AA9 RID: 23209 RVA: 0x001343EE File Offset: 0x001325EE
		public EventOpcode Opcode { get; set; }

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x06005AAA RID: 23210 RVA: 0x001343F7 File Offset: 0x001325F7
		// (set) Token: 0x06005AAB RID: 23211 RVA: 0x001343FF File Offset: 0x001325FF
		public EventChannel Channel { get; set; }

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06005AAC RID: 23212 RVA: 0x00134408 File Offset: 0x00132608
		// (set) Token: 0x06005AAD RID: 23213 RVA: 0x00134410 File Offset: 0x00132610
		public string Message { get; set; }

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x06005AAE RID: 23214 RVA: 0x00134419 File Offset: 0x00132619
		// (set) Token: 0x06005AAF RID: 23215 RVA: 0x00134421 File Offset: 0x00132621
		public EventTask Task { get; set; }

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06005AB0 RID: 23216 RVA: 0x0013442A File Offset: 0x0013262A
		// (set) Token: 0x06005AB1 RID: 23217 RVA: 0x00134432 File Offset: 0x00132632
		public EventTags Tags { get; set; }

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06005AB2 RID: 23218 RVA: 0x0013443B File Offset: 0x0013263B
		// (set) Token: 0x06005AB3 RID: 23219 RVA: 0x00134443 File Offset: 0x00132643
		public byte Version { get; set; }
	}
}
