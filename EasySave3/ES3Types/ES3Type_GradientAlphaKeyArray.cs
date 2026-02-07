using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x0200008F RID: 143
	public class ES3Type_GradientAlphaKeyArray : ES3ArrayType
	{
		// Token: 0x0600035A RID: 858 RVA: 0x0001120C File Offset: 0x0000F40C
		public ES3Type_GradientAlphaKeyArray() : base(typeof(GradientAlphaKey[]), ES3Type_GradientAlphaKey.Instance)
		{
			ES3Type_GradientAlphaKeyArray.Instance = this;
		}

		// Token: 0x040000D2 RID: 210
		public static ES3Type Instance;
	}
}
