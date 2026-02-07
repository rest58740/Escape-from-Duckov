using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x02000089 RID: 137
	public class ES3Type_FontArray : ES3ArrayType
	{
		// Token: 0x06000343 RID: 835 RVA: 0x000104DB File Offset: 0x0000E6DB
		public ES3Type_FontArray() : base(typeof(Font[]), ES3Type_Font.Instance)
		{
			ES3Type_FontArray.Instance = this;
		}

		// Token: 0x040000C9 RID: 201
		public static ES3Type Instance;
	}
}
