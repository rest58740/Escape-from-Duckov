using System;

namespace ES3Types
{
	// Token: 0x0200004E RID: 78
	public class ES3Type_StringArray : ES3ArrayType
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x0000A184 File Offset: 0x00008384
		public ES3Type_StringArray() : base(typeof(string[]), ES3Type_string.Instance)
		{
			ES3Type_StringArray.Instance = this;
		}

		// Token: 0x04000093 RID: 147
		public static ES3Type Instance;
	}
}
