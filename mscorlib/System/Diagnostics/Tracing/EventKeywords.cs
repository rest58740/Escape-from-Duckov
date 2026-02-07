using System;

namespace System.Diagnostics.Tracing
{
	// Token: 0x020009EB RID: 2539
	[Flags]
	public enum EventKeywords : long
	{
		// Token: 0x04003804 RID: 14340
		None = 0L,
		// Token: 0x04003805 RID: 14341
		All = -1L,
		// Token: 0x04003806 RID: 14342
		MicrosoftTelemetry = 562949953421312L,
		// Token: 0x04003807 RID: 14343
		WdiContext = 562949953421312L,
		// Token: 0x04003808 RID: 14344
		WdiDiagnostic = 1125899906842624L,
		// Token: 0x04003809 RID: 14345
		Sqm = 2251799813685248L,
		// Token: 0x0400380A RID: 14346
		AuditFailure = 4503599627370496L,
		// Token: 0x0400380B RID: 14347
		AuditSuccess = 9007199254740992L,
		// Token: 0x0400380C RID: 14348
		CorrelationHint = 4503599627370496L,
		// Token: 0x0400380D RID: 14349
		EventLogClassic = 36028797018963968L
	}
}
