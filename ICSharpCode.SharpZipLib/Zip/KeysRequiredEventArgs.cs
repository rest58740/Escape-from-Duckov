using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200001E RID: 30
	public class KeysRequiredEventArgs : EventArgs
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00009A6C File Offset: 0x00007C6C
		public KeysRequiredEventArgs(string name)
		{
			this.fileName = name;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00009A7C File Offset: 0x00007C7C
		public KeysRequiredEventArgs(string name, byte[] keyValue)
		{
			this.fileName = name;
			this.key = keyValue;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00009A94 File Offset: 0x00007C94
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00009A9C File Offset: 0x00007C9C
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00009AA4 File Offset: 0x00007CA4
		public byte[] Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		// Token: 0x04000148 RID: 328
		private string fileName;

		// Token: 0x04000149 RID: 329
		private byte[] key;
	}
}
