using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000508 RID: 1288
	public enum AceType : byte
	{
		// Token: 0x04002422 RID: 9250
		AccessAllowed,
		// Token: 0x04002423 RID: 9251
		AccessDenied,
		// Token: 0x04002424 RID: 9252
		SystemAudit,
		// Token: 0x04002425 RID: 9253
		SystemAlarm,
		// Token: 0x04002426 RID: 9254
		AccessAllowedCompound,
		// Token: 0x04002427 RID: 9255
		AccessAllowedObject,
		// Token: 0x04002428 RID: 9256
		AccessDeniedObject,
		// Token: 0x04002429 RID: 9257
		SystemAuditObject,
		// Token: 0x0400242A RID: 9258
		SystemAlarmObject,
		// Token: 0x0400242B RID: 9259
		AccessAllowedCallback,
		// Token: 0x0400242C RID: 9260
		AccessDeniedCallback,
		// Token: 0x0400242D RID: 9261
		AccessAllowedCallbackObject,
		// Token: 0x0400242E RID: 9262
		AccessDeniedCallbackObject,
		// Token: 0x0400242F RID: 9263
		SystemAuditCallback,
		// Token: 0x04002430 RID: 9264
		SystemAlarmCallback,
		// Token: 0x04002431 RID: 9265
		SystemAuditCallbackObject,
		// Token: 0x04002432 RID: 9266
		SystemAlarmCallbackObject,
		// Token: 0x04002433 RID: 9267
		MaxDefinedAceType = 16
	}
}
