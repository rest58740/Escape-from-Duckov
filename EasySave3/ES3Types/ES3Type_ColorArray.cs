using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x0200007F RID: 127
	public class ES3Type_ColorArray : ES3ArrayType
	{
		// Token: 0x06000324 RID: 804 RVA: 0x0000FCEC File Offset: 0x0000DEEC
		public ES3Type_ColorArray() : base(typeof(Color[]), ES3Type_Color.Instance)
		{
			ES3Type_ColorArray.Instance = this;
		}

		// Token: 0x040000BF RID: 191
		public static ES3Type Instance;
	}
}
