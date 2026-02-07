using System;
using System.IO;
using UnityEngine;

namespace ES3Internal
{
	// Token: 0x020000E0 RID: 224
	internal class ES3ResourcesStream : MemoryStream
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x0001DC6E File Offset: 0x0001BE6E
		public bool Exists
		{
			get
			{
				return this.Length > 0L;
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001DC7A File Offset: 0x0001BE7A
		public ES3ResourcesStream(string path) : base(ES3ResourcesStream.GetData(path))
		{
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001DC88 File Offset: 0x0001BE88
		private static byte[] GetData(string path)
		{
			TextAsset textAsset = Resources.Load(path) as TextAsset;
			if (textAsset == null)
			{
				return new byte[0];
			}
			return textAsset.bytes;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001DCB7 File Offset: 0x0001BEB7
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}
