using System;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x0200004A RID: 74
	public class ScanEventArgs : EventArgs
	{
		// Token: 0x06000395 RID: 917 RVA: 0x00015A10 File Offset: 0x00013C10
		public ScanEventArgs(string name)
		{
			this.name_ = name;
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00015A28 File Offset: 0x00013C28
		public string Name
		{
			get
			{
				return this.name_;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00015A30 File Offset: 0x00013C30
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00015A38 File Offset: 0x00013C38
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

		// Token: 0x04000299 RID: 665
		private string name_;

		// Token: 0x0400029A RID: 666
		private bool continueRunning_ = true;
	}
}
