using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200005B RID: 91
	public class DescriptorData
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00017184 File Offset: 0x00015384
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x0001718C File Offset: 0x0001538C
		public long CompressedSize
		{
			get
			{
				return this.compressedSize;
			}
			set
			{
				this.compressedSize = value;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00017198 File Offset: 0x00015398
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x000171A0 File Offset: 0x000153A0
		public long Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x000171AC File Offset: 0x000153AC
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x000171B4 File Offset: 0x000153B4
		public long Crc
		{
			get
			{
				return this.crc;
			}
			set
			{
				this.crc = (value & (long)((ulong)-1));
			}
		}

		// Token: 0x040002C9 RID: 713
		private long size;

		// Token: 0x040002CA RID: 714
		private long compressedSize;

		// Token: 0x040002CB RID: 715
		private long crc;
	}
}
