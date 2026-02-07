using System;
using System.IO;

namespace ES3Internal
{
	// Token: 0x020000DE RID: 222
	public class ES3FileStream : FileStream
	{
		// Token: 0x060004C4 RID: 1220 RVA: 0x0001DA89 File Offset: 0x0001BC89
		public ES3FileStream(string path, ES3FileMode fileMode, int bufferSize, bool useAsync) : base(ES3FileStream.GetPath(path, fileMode), ES3FileStream.GetFileMode(fileMode), ES3FileStream.GetFileAccess(fileMode), FileShare.None, bufferSize, useAsync)
		{
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001DAA8 File Offset: 0x0001BCA8
		protected static string GetPath(string path, ES3FileMode fileMode)
		{
			string directoryPath = ES3IO.GetDirectoryPath(path, '/');
			if (fileMode != ES3FileMode.Read && directoryPath != ES3IO.persistentDataPath)
			{
				ES3IO.CreateDirectory(directoryPath);
			}
			if (fileMode != ES3FileMode.Write || fileMode == ES3FileMode.Append)
			{
				return path;
			}
			if (fileMode != ES3FileMode.Write)
			{
				return path;
			}
			return path + ".tmp";
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001DAEF File Offset: 0x0001BCEF
		protected static FileMode GetFileMode(ES3FileMode fileMode)
		{
			if (fileMode == ES3FileMode.Read)
			{
				return FileMode.Open;
			}
			if (fileMode == ES3FileMode.Write)
			{
				return FileMode.Create;
			}
			return FileMode.Append;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001DAFD File Offset: 0x0001BCFD
		protected static FileAccess GetFileAccess(ES3FileMode fileMode)
		{
			if (fileMode == ES3FileMode.Read)
			{
				return FileAccess.Read;
			}
			return FileAccess.Write;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001DB09 File Offset: 0x0001BD09
		protected override void Dispose(bool disposing)
		{
			if (this.isDisposed)
			{
				return;
			}
			this.isDisposed = true;
			base.Dispose(disposing);
		}

		// Token: 0x04000145 RID: 325
		private bool isDisposed;
	}
}
