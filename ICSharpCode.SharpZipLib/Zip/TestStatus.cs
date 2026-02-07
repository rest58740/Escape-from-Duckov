using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000021 RID: 33
	public class TestStatus
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00009AB0 File Offset: 0x00007CB0
		public TestStatus(ZipFile file)
		{
			this.file_ = file;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00009AC0 File Offset: 0x00007CC0
		public TestOperation Operation
		{
			get
			{
				return this.operation_;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00009AC8 File Offset: 0x00007CC8
		public ZipFile File
		{
			get
			{
				return this.file_;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00009AD0 File Offset: 0x00007CD0
		public ZipEntry Entry
		{
			get
			{
				return this.entry_;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00009AD8 File Offset: 0x00007CD8
		public int ErrorCount
		{
			get
			{
				return this.errorCount_;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00009AE0 File Offset: 0x00007CE0
		public long BytesTested
		{
			get
			{
				return this.bytesTested_;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00009AE8 File Offset: 0x00007CE8
		public bool EntryValid
		{
			get
			{
				return this.entryValid_;
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00009AF0 File Offset: 0x00007CF0
		internal void AddError()
		{
			this.errorCount_++;
			this.entryValid_ = false;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00009B08 File Offset: 0x00007D08
		internal void SetOperation(TestOperation operation)
		{
			this.operation_ = operation;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00009B14 File Offset: 0x00007D14
		internal void SetEntry(ZipEntry entry)
		{
			this.entry_ = entry;
			this.entryValid_ = true;
			this.bytesTested_ = 0L;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00009B2C File Offset: 0x00007D2C
		internal void SetBytesTested(long value)
		{
			this.bytesTested_ = value;
		}

		// Token: 0x04000154 RID: 340
		private ZipFile file_;

		// Token: 0x04000155 RID: 341
		private ZipEntry entry_;

		// Token: 0x04000156 RID: 342
		private bool entryValid_;

		// Token: 0x04000157 RID: 343
		private int errorCount_;

		// Token: 0x04000158 RID: 344
		private long bytesTested_;

		// Token: 0x04000159 RID: 345
		private TestOperation operation_;
	}
}
