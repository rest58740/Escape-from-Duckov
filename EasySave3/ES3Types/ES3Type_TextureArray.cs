using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000BC RID: 188
	public class ES3Type_TextureArray : ES3ArrayType
	{
		// Token: 0x060003DB RID: 987 RVA: 0x000190B4 File Offset: 0x000172B4
		public ES3Type_TextureArray() : base(typeof(Texture[]), ES3Type_Texture.Instance)
		{
			ES3Type_TextureArray.Instance = this;
		}

		// Token: 0x040000FF RID: 255
		public static ES3Type Instance;
	}
}
