using System;

namespace FMOD
{
	// Token: 0x0200003A RID: 58
	public struct TAG
	{
		// Token: 0x040001AD RID: 429
		public TAGTYPE type;

		// Token: 0x040001AE RID: 430
		public TAGDATATYPE datatype;

		// Token: 0x040001AF RID: 431
		public StringWrapper name;

		// Token: 0x040001B0 RID: 432
		public IntPtr data;

		// Token: 0x040001B1 RID: 433
		public uint datalen;

		// Token: 0x040001B2 RID: 434
		public bool updated;
	}
}
