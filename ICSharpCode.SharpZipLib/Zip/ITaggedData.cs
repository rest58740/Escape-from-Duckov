using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200005E RID: 94
	public interface ITaggedData
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600041F RID: 1055
		short TagID { get; }

		// Token: 0x06000420 RID: 1056
		void SetData(byte[] data, int offset, int count);

		// Token: 0x06000421 RID: 1057
		byte[] GetData();
	}
}
