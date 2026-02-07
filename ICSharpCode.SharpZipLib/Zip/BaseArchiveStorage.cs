using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000031 RID: 49
	public abstract class BaseArchiveStorage : IArchiveStorage
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x0000D7F8 File Offset: 0x0000B9F8
		protected BaseArchiveStorage(FileUpdateMode updateMode)
		{
			this.updateMode_ = updateMode;
		}

		// Token: 0x060001EA RID: 490
		public abstract Stream GetTemporaryOutput();

		// Token: 0x060001EB RID: 491
		public abstract Stream ConvertTemporaryToFinal();

		// Token: 0x060001EC RID: 492
		public abstract Stream MakeTemporaryCopy(Stream stream);

		// Token: 0x060001ED RID: 493
		public abstract Stream OpenForDirectUpdate(Stream stream);

		// Token: 0x060001EE RID: 494
		public abstract void Dispose();

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000D808 File Offset: 0x0000BA08
		public FileUpdateMode UpdateMode
		{
			get
			{
				return this.updateMode_;
			}
		}

		// Token: 0x04000191 RID: 401
		private FileUpdateMode updateMode_;
	}
}
