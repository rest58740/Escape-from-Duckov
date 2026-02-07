using System;

namespace ES3Types
{
	// Token: 0x02000036 RID: 54
	public class ES3Type_charArray : ES3ArrayType
	{
		// Token: 0x06000276 RID: 630 RVA: 0x000097FA File Offset: 0x000079FA
		public ES3Type_charArray() : base(typeof(char[]), ES3Type_char.Instance)
		{
			ES3Type_charArray.Instance = this;
		}

		// Token: 0x0400007A RID: 122
		public static ES3Type Instance;
	}
}
