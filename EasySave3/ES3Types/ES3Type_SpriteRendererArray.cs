using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000B9 RID: 185
	public class ES3Type_SpriteRendererArray : ES3ArrayType
	{
		// Token: 0x060003D2 RID: 978 RVA: 0x00018E08 File Offset: 0x00017008
		public ES3Type_SpriteRendererArray() : base(typeof(SpriteRenderer[]), ES3Type_SpriteRenderer.Instance)
		{
			ES3Type_SpriteRendererArray.Instance = this;
		}

		// Token: 0x040000FC RID: 252
		public static ES3Type Instance;
	}
}
