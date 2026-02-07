using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000C7 RID: 199
	public class ES3Type_Vector3Array : ES3ArrayType
	{
		// Token: 0x060003F9 RID: 1017 RVA: 0x0001A1DA File Offset: 0x000183DA
		public ES3Type_Vector3Array() : base(typeof(Vector3[]), ES3Type_Vector3.Instance)
		{
			ES3Type_Vector3Array.Instance = this;
		}

		// Token: 0x0400010A RID: 266
		public static ES3Type Instance;
	}
}
