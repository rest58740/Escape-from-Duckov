using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200005C RID: 92
	internal class EntryPatchData
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x000171C8 File Offset: 0x000153C8
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x000171D0 File Offset: 0x000153D0
		public long SizePatchOffset
		{
			get
			{
				return this.sizePatchOffset_;
			}
			set
			{
				this.sizePatchOffset_ = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x000171DC File Offset: 0x000153DC
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x000171E4 File Offset: 0x000153E4
		public long CrcPatchOffset
		{
			get
			{
				return this.crcPatchOffset_;
			}
			set
			{
				this.crcPatchOffset_ = value;
			}
		}

		// Token: 0x040002CC RID: 716
		private long sizePatchOffset_;

		// Token: 0x040002CD RID: 717
		private long crcPatchOffset_;
	}
}
