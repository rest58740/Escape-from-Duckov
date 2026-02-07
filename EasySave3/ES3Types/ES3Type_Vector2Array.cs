using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000C3 RID: 195
	public class ES3Type_Vector2Array : ES3ArrayType
	{
		// Token: 0x060003F1 RID: 1009 RVA: 0x0001A06C File Offset: 0x0001826C
		public ES3Type_Vector2Array() : base(typeof(Vector2[]), ES3Type_Vector2.Instance)
		{
			ES3Type_Vector2Array.Instance = this;
		}

		// Token: 0x04000106 RID: 262
		public static ES3Type Instance;
	}
}
