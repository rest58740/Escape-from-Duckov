using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000032 RID: 50
	public class DiskArchiveStorage : BaseArchiveStorage
	{
		// Token: 0x060001F0 RID: 496 RVA: 0x0000D810 File Offset: 0x0000BA10
		public DiskArchiveStorage(ZipFile file, FileUpdateMode updateMode) : base(updateMode)
		{
			if (file.Name == null)
			{
				throw new ZipException("Cant handle non file archives");
			}
			this.fileName_ = file.Name;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000D83C File Offset: 0x0000BA3C
		public DiskArchiveStorage(ZipFile file) : this(file, FileUpdateMode.Safe)
		{
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000D848 File Offset: 0x0000BA48
		public override Stream GetTemporaryOutput()
		{
			if (this.temporaryStream_ != null)
			{
				return this.temporaryStream_;
			}
			string tempFileName = Path.GetTempFileName();
			this.temporaryStream_ = File.Open(tempFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
			return this.temporaryStream_;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000D884 File Offset: 0x0000BA84
		public override Stream ConvertTemporaryToFinal()
		{
			Stream temporaryOutput = this.GetTemporaryOutput();
			if (temporaryOutput == null || !(temporaryOutput is FileStream))
			{
				throw new ZipException("No temporary stream has been created");
			}
			Stream result = null;
			string name = ((FileStream)temporaryOutput).Name;
			string tempFileName = DiskArchiveStorage.GetTempFileName(this.fileName_, false);
			bool flag = false;
			try
			{
				temporaryOutput.Close();
				File.Move(this.fileName_, tempFileName);
				File.Move(name, this.fileName_);
				flag = true;
				File.Delete(tempFileName);
				result = File.Open(this.fileName_, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			catch (Exception)
			{
				result = null;
				if (!flag)
				{
					File.Move(tempFileName, this.fileName_);
					File.Delete(tempFileName);
				}
				throw;
			}
			return result;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000D950 File Offset: 0x0000BB50
		public override Stream MakeTemporaryCopy(Stream stream)
		{
			stream.Close();
			string tempFileName = DiskArchiveStorage.GetTempFileName(this.fileName_, true);
			File.Copy(this.fileName_, tempFileName, true);
			if (this.temporaryStream_ != null)
			{
				this.temporaryStream_.Close();
			}
			this.temporaryStream_ = new FileStream(tempFileName, FileMode.Open, FileAccess.ReadWrite);
			return this.temporaryStream_;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000D9A8 File Offset: 0x0000BBA8
		public override Stream OpenForDirectUpdate(Stream stream)
		{
			Stream result;
			if (stream == null || !stream.CanWrite)
			{
				if (stream != null)
				{
					stream.Close();
				}
				result = new FileStream(this.fileName_, FileMode.Open, FileAccess.ReadWrite);
			}
			else
			{
				result = stream;
			}
			return result;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000D9E8 File Offset: 0x0000BBE8
		public override void Dispose()
		{
			if (this.temporaryStream_ != null)
			{
				this.temporaryStream_.Close();
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000DA00 File Offset: 0x0000BC00
		private static string GetTempFileName(string original, bool makeTempFile)
		{
			string text = null;
			if (original == null)
			{
				text = Path.GetTempFileName();
			}
			else
			{
				int num = 0;
				int second = DateTime.Now.Second;
				while (text == null)
				{
					num++;
					string text2 = string.Format("{0}.{1}{2}.tmp", original, second, num);
					if (!File.Exists(text2))
					{
						if (makeTempFile)
						{
							try
							{
								using (File.Create(text2))
								{
								}
								text = text2;
							}
							catch
							{
								second = DateTime.Now.Second;
							}
						}
						else
						{
							text = text2;
						}
					}
				}
			}
			return text;
		}

		// Token: 0x04000192 RID: 402
		private Stream temporaryStream_;

		// Token: 0x04000193 RID: 403
		private string fileName_;
	}
}
