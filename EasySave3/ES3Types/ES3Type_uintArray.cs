using System;

namespace ES3Types
{
	// Token: 0x02000050 RID: 80
	public class ES3Type_uintArray : ES3ArrayType
	{
		// Token: 0x060002AD RID: 685 RVA: 0x0000A1E5 File Offset: 0x000083E5
		public ES3Type_uintArray() : base(typeof(uint[]), ES3Type_uint.Instance)
		{
			ES3Type_uintArray.Instance = this;
		}

		// Token: 0x04000095 RID: 149
		public static ES3Type Instance;
	}
}
