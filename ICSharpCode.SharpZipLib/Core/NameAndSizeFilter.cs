using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x02000051 RID: 81
	[Obsolete("Use ExtendedPathFilter instead")]
	public class NameAndSizeFilter : PathFilter
	{
		// Token: 0x060003C0 RID: 960 RVA: 0x0001612C File Offset: 0x0001432C
		public NameAndSizeFilter(string filter, long minSize, long maxSize) : base(filter)
		{
			this.MinSize = minSize;
			this.MaxSize = maxSize;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00016160 File Offset: 0x00014360
		public override bool IsMatch(string name)
		{
			bool flag = base.IsMatch(name);
			if (flag)
			{
				FileInfo fileInfo = new FileInfo(name);
				long length = fileInfo.Length;
				flag = (this.MinSize <= length && this.MaxSize >= length);
			}
			return flag;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x000161A8 File Offset: 0x000143A8
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x000161B0 File Offset: 0x000143B0
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

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x000161E4 File Offset: 0x000143E4
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x000161EC File Offset: 0x000143EC
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

		// Token: 0x040002B0 RID: 688
		private long minSize_;

		// Token: 0x040002B1 RID: 689
		private long maxSize_ = long.MaxValue;
	}
}
