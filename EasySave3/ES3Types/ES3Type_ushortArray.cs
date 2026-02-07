using System;

namespace ES3Types
{
	// Token: 0x02000056 RID: 86
	public class ES3Type_ushortArray : ES3ArrayType
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x0000A2FE File Offset: 0x000084FE
		public ES3Type_ushortArray() : base(typeof(ushort[]), ES3Type_ushort.Instance)
		{
			ES3Type_ushortArray.Instance = this;
		}

		// Token: 0x0400009B RID: 155
		public static ES3Type Instance;
	}
}
