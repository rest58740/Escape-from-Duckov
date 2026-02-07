using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x02000087 RID: 135
	public class ES3Type_FlareArray : ES3ArrayType
	{
		// Token: 0x0600033E RID: 830 RVA: 0x000103F9 File Offset: 0x0000E5F9
		public ES3Type_FlareArray() : base(typeof(Flare[]), ES3Type_Flare.Instance)
		{
			ES3Type_FlareArray.Instance = this;
		}

		// Token: 0x040000C7 RID: 199
		public static ES3Type Instance;
	}
}
