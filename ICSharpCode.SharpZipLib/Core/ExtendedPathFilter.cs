using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x02000050 RID: 80
	public class ExtendedPathFilter : PathFilter
	{
		// Token: 0x060003B4 RID: 948 RVA: 0x00015EDC File Offset: 0x000140DC
		public ExtendedPathFilter(string filter, long minSize, long maxSize) : base(filter)
		{
			this.MinSize = minSize;
			this.MaxSize = maxSize;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00015F24 File Offset: 0x00014124
		public ExtendedPathFilter(string filter, DateTime minDate, DateTime maxDate) : base(filter)
		{
			this.MinDate = minDate;
			this.MaxDate = maxDate;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00015F6C File Offset: 0x0001416C
		public ExtendedPathFilter(string filter, long minSize, long maxSize, DateTime minDate, DateTime maxDate) : base(filter)
		{
			this.MinSize = minSize;
			this.MaxSize = maxSize;
			this.MinDate = minDate;
			this.MaxDate = maxDate;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00015FC4 File Offset: 0x000141C4
		public override bool IsMatch(string name)
		{
			bool flag = base.IsMatch(name);
			if (flag)
			{
				FileInfo fileInfo = new FileInfo(name);
				flag = (this.MinSize <= fileInfo.Length && this.MaxSize >= fileInfo.Length && this.MinDate <= fileInfo.LastWriteTime && this.MaxDate >= fileInfo.LastWriteTime);
			}
			return flag;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00016034 File Offset: 0x00014234
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x0001603C File Offset: 0x0001423C
		public long MinSize
		{
			get
			{
				return this.minSize_;
			}
			set
			{
				if (value < 0L || this.maxSize_ < value)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.minSize_ = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00016070 File Offset: 0x00014270
		// (set) Token: 0x060003BB RID: 955 RVA: 0x00016078 File Offset: 0x00014278
		public long MaxSize
		{
			get
			{
				return this.maxSize_;
			}
			set
			{
				if (value < 0L || this.minSize_ > value)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.maxSize_ = value;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060003BC RID: 956 RVA: 0x000160AC File Offset: 0x000142AC
		// (set) Token: 0x060003BD RID: 957 RVA: 0x000160B4 File Offset: 0x000142B4
		public DateTime MinDate
		{
			get
			{
				return this.minDate_;
			}
			set
			{
				if (value > this.maxDate_)
				{
					throw new ArgumentOutOfRangeException("value", "Exceeds MaxDate");
				}
				this.minDate_ = value;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060003BE RID: 958 RVA: 0x000160EC File Offset: 0x000142EC
		// (set) Token: 0x060003BF RID: 959 RVA: 0x000160F4 File Offset: 0x000142F4
		public DateTime MaxDate
		{
			get
			{
				return this.maxDate_;
			}
			set
			{
				if (this.minDate_ > value)
				{
					throw new ArgumentOutOfRangeException("value", "Exceeds MinDate");
				}
				this.maxDate_ = value;
			}
		}

		// Token: 0x040002AC RID: 684
		private long minSize_;

		// Token: 0x040002AD RID: 685
		private long maxSize_ = long.MaxValue;

		// Token: 0x040002AE RID: 686
		private DateTime minDate_ = DateTime.MinValue;

		// Token: 0x040002AF RID: 687
		private DateTime maxDate_ = DateTime.MaxValue;
	}
}
