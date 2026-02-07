using System;
using System.IO;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000033 RID: 51
	public class MemoryArchiveStorage : BaseArchiveStorage
	{
		// Token: 0x060001F8 RID: 504 RVA: 0x0000DAE0 File Offset: 0x0000BCE0
		public MemoryArchiveStorage() : base(FileUpdateMode.Direct)
		{
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000DAEC File Offset: 0x0000BCEC
		public MemoryArchiveStorage(FileUpdateMode updateMode) : base(updateMode)
		{
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
		public MemoryStream FinalStream
		{
			get
			{
				return this.finalStream_;
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000DB00 File Offset: 0x0000BD00
		public override Stream GetTemporaryOutput()
		{
			this.temporaryStream_ = new MemoryStream();
			return this.temporaryStream_;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000DB14 File Offset: 0x0000BD14
		public override Stream ConvertTemporaryToFinal()
		{
			if (this.temporaryStream_ == null)
			{
				throw new ZipException("No temporary stream has been created");
			}
			this.finalStream_ = new MemoryStream(this.temporaryStream_.ToArray());
			return this.finalStream_;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000DB54 File Offset: 0x0000BD54
		public override Stream MakeTemporaryCopy(Stream stream)
		{
			this.temporaryStream_ = new MemoryStream();
			stream.Position = 0L;
			StreamUtils.Copy(stream, this.temporaryStream_, new byte[4096]);
			return this.temporaryStream_;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000DB88 File Offset: 0x0000BD88
		public override Stream OpenForDirectUpdate(Stream stream)
		{
			Stream stream2;
			if (stream == null || !stream.CanWrite)
			{
				stream2 = new MemoryStream();
				if (stream != null)
				{
					stream.Position = 0L;
					StreamUtils.Copy(stream, stream2, new byte[4096]);
					stream.Close();
				}
			}
			else
			{
				stream2 = stream;
			}
			return stream2;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000DBDC File Offset: 0x0000BDDC
		public override void Dispose()
		{
			if (this.temporaryStream_ != null)
			{
				this.temporaryStream_.Close();
			}
		}

		// Token: 0x04000194 RID: 404
		private MemoryStream temporaryStream_;

		// Token: 0x04000195 RID: 405
		private MemoryStream finalStream_;
	}
}
