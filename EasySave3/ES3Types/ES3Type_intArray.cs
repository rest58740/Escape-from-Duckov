using System;

namespace ES3Types
{
	// Token: 0x02000044 RID: 68
	public class ES3Type_intArray : ES3ArrayType
	{
		// Token: 0x06000295 RID: 661 RVA: 0x00009FA4 File Offset: 0x000081A4
		public ES3Type_intArray() : base(typeof(int[]), ES3Type_int.Instance)
		{
			ES3Type_intArray.Instance = this;
		}

		// Token: 0x04000089 RID: 137
		public static ES3Type Instance;
	}
}
