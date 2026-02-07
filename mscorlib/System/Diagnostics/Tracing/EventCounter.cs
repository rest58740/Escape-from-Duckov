using System;

namespace System.Diagnostics.Tracing
{
	// Token: 0x020009EF RID: 2543
	public class EventCounter : DiagnosticCounter
	{
		// Token: 0x06005AB9 RID: 23225 RVA: 0x00134454 File Offset: 0x00132654
		public EventCounter(string name, EventSource eventSource) : base(name, eventSource)
		{
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void WriteMetric(float value)
		{
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void WriteMetric(double value)
		{
		}
	}
}
