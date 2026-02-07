using System;

namespace DG.Tweening.Core
{
	// Token: 0x02000053 RID: 83
	internal struct SafeModeReport
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000FD36 File Offset: 0x0000DF36
		// (set) Token: 0x060002DA RID: 730 RVA: 0x0000FD3E File Offset: 0x0000DF3E
		public int totMissingTargetOrFieldErrors { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000FD47 File Offset: 0x0000DF47
		// (set) Token: 0x060002DC RID: 732 RVA: 0x0000FD4F File Offset: 0x0000DF4F
		public int totCallbackErrors { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000FD58 File Offset: 0x0000DF58
		// (set) Token: 0x060002DE RID: 734 RVA: 0x0000FD60 File Offset: 0x0000DF60
		public int totStartupErrors { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000FD69 File Offset: 0x0000DF69
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x0000FD71 File Offset: 0x0000DF71
		public int totUnsetErrors { get; private set; }

		// Token: 0x060002E1 RID: 737 RVA: 0x0000FD7C File Offset: 0x0000DF7C
		public void Add(SafeModeReport.SafeModeReportType type)
		{
			switch (type)
			{
			case SafeModeReport.SafeModeReportType.TargetOrFieldMissing:
			{
				int num = this.totMissingTargetOrFieldErrors;
				this.totMissingTargetOrFieldErrors = num + 1;
				return;
			}
			case SafeModeReport.SafeModeReportType.Callback:
			{
				int num = this.totCallbackErrors;
				this.totCallbackErrors = num + 1;
				return;
			}
			case SafeModeReport.SafeModeReportType.StartupFailure:
			{
				int num = this.totStartupErrors;
				this.totStartupErrors = num + 1;
				return;
			}
			default:
			{
				int num = this.totUnsetErrors;
				this.totUnsetErrors = num + 1;
				return;
			}
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000FDE2 File Offset: 0x0000DFE2
		public int GetTotErrors()
		{
			return this.totMissingTargetOrFieldErrors + this.totCallbackErrors + this.totStartupErrors + this.totUnsetErrors;
		}

		// Token: 0x020000C3 RID: 195
		internal enum SafeModeReportType
		{
			// Token: 0x0400027C RID: 636
			Unset,
			// Token: 0x0400027D RID: 637
			TargetOrFieldMissing,
			// Token: 0x0400027E RID: 638
			Callback,
			// Token: 0x0400027F RID: 639
			StartupFailure
		}
	}
}
