using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x0200007A RID: 122
	public class ES3Type_BoundsArray : ES3ArrayType
	{
		// Token: 0x06000318 RID: 792 RVA: 0x0000F356 File Offset: 0x0000D556
		public ES3Type_BoundsArray() : base(typeof(Bounds[]), ES3Type_Bounds.Instance)
		{
			ES3Type_BoundsArray.Instance = this;
		}

		// Token: 0x040000BA RID: 186
		public static ES3Type Instance;
	}
}
