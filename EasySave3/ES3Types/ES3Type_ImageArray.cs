using System;
using UnityEngine.UI;

namespace ES3Types
{
	// Token: 0x02000063 RID: 99
	public class ES3Type_ImageArray : ES3ArrayType
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x0000BF1C File Offset: 0x0000A11C
		public ES3Type_ImageArray() : base(typeof(Image[]), ES3Type_Image.Instance)
		{
			ES3Type_ImageArray.Instance = this;
		}

		// Token: 0x040000A2 RID: 162
		public static ES3Type Instance;
	}
}
