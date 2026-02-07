using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x0200009D RID: 157
	public class ES3Type_MaterialArray : ES3ArrayType
	{
		// Token: 0x06000381 RID: 897 RVA: 0x000137F4 File Offset: 0x000119F4
		public ES3Type_MaterialArray() : base(typeof(Material[]), ES3Type_Material.Instance)
		{
			ES3Type_MaterialArray.Instance = this;
		}

		// Token: 0x040000E0 RID: 224
		public static ES3Type Instance;
	}
}
