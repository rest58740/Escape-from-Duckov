using System;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x0200004D RID: 77
	public class ScanFailureEventArgs : EventArgs
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x00015AF4 File Offset: 0x00013CF4
		public ScanFailureEventArgs(string name, Exception e)
		{
			this.name_ = name;
			this.exception_ = e;
			this.continueRunning_ = true;
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00015B14 File Offset: 0x00013D14
		public string Name
		{
			get
			{
				return this.name_;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00015B1C File Offset: 0x00013D1C
		public Exception Exception
		{
			get
			{
				return this.exception_;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00015B24 File Offset: 0x00013D24
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00015B2C File Offset: 0x00013D2C
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

		// Token: 0x040002A0 RID: 672
		private string name_;

		// Token: 0x040002A1 RID: 673
		private Exception exception_;

		// Token: 0x040002A2 RID: 674
		private bool continueRunning_;
	}
}
