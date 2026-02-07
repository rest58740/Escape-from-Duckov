using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x02000081 RID: 129
	public class ES3Type_Color32Array : ES3ArrayType
	{
		// Token: 0x06000329 RID: 809 RVA: 0x0000FE19 File Offset: 0x0000E019
		public ES3Type_Color32Array() : base(typeof(Color32[]), ES3Type_Color32.Instance)
		{
			ES3Type_Color32Array.Instance = this;
		}

		// Token: 0x040000C1 RID: 193
		public static ES3Type Instance;
	}
}
