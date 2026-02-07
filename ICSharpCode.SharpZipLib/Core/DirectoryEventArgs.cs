using System;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x0200004C RID: 76
	public class DirectoryEventArgs : ScanEventArgs
	{
		// Token: 0x060003A0 RID: 928 RVA: 0x00015ADC File Offset: 0x00013CDC
		public DirectoryEventArgs(string name, bool hasMatchingFiles) : base(name)
		{
			this.hasMatchingFiles_ = hasMatchingFiles;
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00015AEC File Offset: 0x00013CEC
		public bool HasMatchingFiles
		{
			get
			{
				return this.hasMatchingFiles_;
			}
		}

		// Token: 0x0400029F RID: 671
		private bool hasMatchingFiles_;
	}
}
