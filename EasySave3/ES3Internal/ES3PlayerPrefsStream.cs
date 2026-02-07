using System;
using System.IO;
using UnityEngine;

namespace ES3Internal
{
	// Token: 0x020000DF RID: 223
	internal class ES3PlayerPrefsStream : MemoryStream
	{
		// Token: 0x060004C9 RID: 1225 RVA: 0x0001DB22 File Offset: 0x0001BD22
		public ES3PlayerPrefsStream(string path) : base(ES3PlayerPrefsStream.GetData(path, false))
		{
			this.path = path;
			this.append = false;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001DB3F File Offset: 0x0001BD3F
		public ES3PlayerPrefsStream(string path, int bufferSize, bool append = false) : base(bufferSize)
		{
			this.path = path;
			this.append = append;
			this.isWriteStream = true;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001DB5D File Offset: 0x0001BD5D
		private static byte[] GetData(string path, bool isWriteStream)
		{
			if (!PlayerPrefs.HasKey(path))
			{
				throw new FileNotFoundException("File \"" + path + "\" could not be found in PlayerPrefs");
			}
			return Convert.FromBase64String(PlayerPrefs.GetString(path));
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001DB88 File Offset: 0x0001BD88
		protected override void Dispose(bool disposing)
		{
			if (this.isDisposed)
			{
				return;
			}
			this.isDisposed = true;
			if (this.isWriteStream && this.Length > 0L)
			{
				if (this.append)
				{
					byte[] array = Convert.FromBase64String(PlayerPrefs.GetString(this.path));
					byte[] array2 = this.ToArray();
					byte[] array3 = new byte[array.Length + array2.Length];
					Buffer.BlockCopy(array, 0, array3, 0, array.Length);
					Buffer.BlockCopy(array2, 0, array3, array.Length, array2.Length);
					PlayerPrefs.SetString(this.path, Convert.ToBase64String(array3));
					PlayerPrefs.Save();
				}
				else
				{
					PlayerPrefs.SetString(this.path + ".tmp", Convert.ToBase64String(this.ToArray()));
				}
				PlayerPrefs.SetString("timestamp_" + this.path, DateTime.UtcNow.Ticks.ToString());
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000146 RID: 326
		private string path;

		// Token: 0x04000147 RID: 327
		private bool append;

		// Token: 0x04000148 RID: 328
		private bool isWriteStream;

		// Token: 0x04000149 RID: 329
		private bool isDisposed;
	}
}
