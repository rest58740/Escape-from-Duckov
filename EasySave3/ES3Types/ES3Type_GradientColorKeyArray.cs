using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x02000091 RID: 145
	public class ES3Type_GradientColorKeyArray : ES3ArrayType
	{
		// Token: 0x0600035E RID: 862 RVA: 0x000112B0 File Offset: 0x0000F4B0
		public ES3Type_GradientColorKeyArray() : base(typeof(GradientColorKey[]), ES3Type_GradientColorKey.Instance)
		{
			ES3Type_GradientColorKeyArray.Instance = this;
		}

		// Token: 0x040000D4 RID: 212
		public static ES3Type Instance;
	}
}
