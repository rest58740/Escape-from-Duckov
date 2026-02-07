using System;

namespace ES3Types
{
	// Token: 0x02000048 RID: 72
	public class ES3Type_longArray : ES3ArrayType
	{
		// Token: 0x0600029D RID: 669 RVA: 0x0000A070 File Offset: 0x00008270
		public ES3Type_longArray() : base(typeof(long[]), ES3Type_long.Instance)
		{
			ES3Type_longArray.Instance = this;
		}

		// Token: 0x0400008D RID: 141
		public static ES3Type Instance;
	}
}
