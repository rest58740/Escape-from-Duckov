using System;
using ES3Types;

namespace ES3Internal
{
	// Token: 0x020000D2 RID: 210
	public struct ES3Data
	{
		// Token: 0x06000419 RID: 1049 RVA: 0x0001ABA1 File Offset: 0x00018DA1
		public ES3Data(Type type, byte[] bytes)
		{
			this.type = ((type == null) ? null : ES3TypeMgr.GetOrCreateES3Type(type, true));
			this.bytes = bytes;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001ABC3 File Offset: 0x00018DC3
		public ES3Data(ES3Type type, byte[] bytes)
		{
			this.type = type;
			this.bytes = bytes;
		}

		// Token: 0x0400011D RID: 285
		public ES3Type type;

		// Token: 0x0400011E RID: 286
		public byte[] bytes;
	}
}
