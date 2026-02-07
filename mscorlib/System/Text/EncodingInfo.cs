using System;
using Unity;

namespace System.Text
{
	// Token: 0x020003A6 RID: 934
	[Serializable]
	public sealed class EncodingInfo
	{
		// Token: 0x06002648 RID: 9800 RVA: 0x00087B44 File Offset: 0x00085D44
		internal EncodingInfo(int codePage, string name, string displayName)
		{
			this.iCodePage = codePage;
			this.strEncodingName = name;
			this.strDisplayName = displayName;
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06002649 RID: 9801 RVA: 0x00087B61 File Offset: 0x00085D61
		public int CodePage
		{
			get
			{
				return this.iCodePage;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x0600264A RID: 9802 RVA: 0x00087B69 File Offset: 0x00085D69
		public string Name
		{
			get
			{
				return this.strEncodingName;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600264B RID: 9803 RVA: 0x00087B71 File Offset: 0x00085D71
		public string DisplayName
		{
			get
			{
				return this.strDisplayName;
			}
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x00087B79 File Offset: 0x00085D79
		public Encoding GetEncoding()
		{
			return Encoding.GetEncoding(this.iCodePage);
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x00087B88 File Offset: 0x00085D88
		public override bool Equals(object value)
		{
			EncodingInfo encodingInfo = value as EncodingInfo;
			return encodingInfo != null && this.CodePage == encodingInfo.CodePage;
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x00087BAF File Offset: 0x00085DAF
		public override int GetHashCode()
		{
			return this.CodePage;
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x000173AD File Offset: 0x000155AD
		internal EncodingInfo()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04001DC4 RID: 7620
		private int iCodePage;

		// Token: 0x04001DC5 RID: 7621
		private string strEncodingName;

		// Token: 0x04001DC6 RID: 7622
		private string strDisplayName;
	}
}
