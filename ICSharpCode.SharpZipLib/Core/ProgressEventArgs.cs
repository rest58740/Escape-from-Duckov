using System;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x0200004B RID: 75
	public class ProgressEventArgs : EventArgs
	{
		// Token: 0x06000399 RID: 921 RVA: 0x00015A44 File Offset: 0x00013C44
		public ProgressEventArgs(string name, long processed, long target)
		{
			this.name_ = name;
			this.processed_ = processed;
			this.target_ = target;
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600039A RID: 922 RVA: 0x00015A74 File Offset: 0x00013C74
		public string Name
		{
			get
			{
				return this.name_;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00015A7C File Offset: 0x00013C7C
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00015A84 File Offset: 0x00013C84
		public bool ContinueRunning
		{
			get
			{
				return this.continueRunning_;
			}
			set
			{
				this.continueRunning_ = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00015A90 File Offset: 0x00013C90
		public float PercentComplete
		{
			get
			{
				float result;
				if (this.target_ <= 0L)
				{
					result = 0f;
				}
				else
				{
					result = (float)this.processed_ / (float)this.target_ * 100f;
				}
				return result;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00015ACC File Offset: 0x00013CCC
		public long Processed
		{
			get
			{
				return this.processed_;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00015AD4 File Offset: 0x00013CD4
		public long Target
		{
			get
			{
				return this.target_;
			}
		}

		// Token: 0x0400029B RID: 667
		private string name_;

		// Token: 0x0400029C RID: 668
		private long processed_;

		// Token: 0x0400029D RID: 669
		private long target_;

		// Token: 0x0400029E RID: 670
		private bool continueRunning_ = true;
	}
}
