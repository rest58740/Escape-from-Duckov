using System;
using System.Collections.Generic;

namespace System.Diagnostics.Tracing
{
	// Token: 0x020009F5 RID: 2549
	public class EventListener : IDisposable
	{
		// Token: 0x06005AC6 RID: 23238 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public static int EventSourceIndex(EventSource eventSource)
		{
			return 0;
		}

		// Token: 0x06005AC7 RID: 23239 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void EnableEvents(EventSource eventSource, EventLevel level)
		{
		}

		// Token: 0x06005AC8 RID: 23240 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void EnableEvents(EventSource eventSource, EventLevel level, EventKeywords matchAnyKeyword)
		{
		}

		// Token: 0x06005AC9 RID: 23241 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void EnableEvents(EventSource eventSource, EventLevel level, EventKeywords matchAnyKeyword, IDictionary<string, string> arguments)
		{
		}

		// Token: 0x06005ACA RID: 23242 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void DisableEvents(EventSource eventSource)
		{
		}

		// Token: 0x06005ACB RID: 23243 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected internal virtual void OnEventSourceCreated(EventSource eventSource)
		{
		}

		// Token: 0x06005ACC RID: 23244 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected internal virtual void OnEventWritten(EventWrittenEventArgs eventData)
		{
		}

		// Token: 0x06005ACD RID: 23245 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public virtual void Dispose()
		{
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06005ACE RID: 23246 RVA: 0x00134460 File Offset: 0x00132660
		// (remove) Token: 0x06005ACF RID: 23247 RVA: 0x00134498 File Offset: 0x00132698
		public event EventHandler<EventSourceCreatedEventArgs> EventSourceCreated;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06005AD0 RID: 23248 RVA: 0x001344D0 File Offset: 0x001326D0
		// (remove) Token: 0x06005AD1 RID: 23249 RVA: 0x00134508 File Offset: 0x00132708
		public event EventHandler<EventWrittenEventArgs> EventWritten;
	}
}
