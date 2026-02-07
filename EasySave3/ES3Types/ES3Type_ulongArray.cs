using System;

namespace ES3Types
{
	// Token: 0x02000054 RID: 84
	public class ES3Type_ulongArray : ES3ArrayType
	{
		// Token: 0x060002B5 RID: 693 RVA: 0x0000A29D File Offset: 0x0000849D
		public ES3Type_ulongArray() : base(typeof(ulong[]), ES3Type_ulong.Instance)
		{
			ES3Type_ulongArray.Instance = this;
		}

		// Token: 0x04000099 RID: 153
		public static ES3Type Instance;
	}
}
