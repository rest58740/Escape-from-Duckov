using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x0200009F RID: 159
	public class ES3Type_Matrix4x4Array : ES3ArrayType
	{
		// Token: 0x06000385 RID: 901 RVA: 0x0001391B File Offset: 0x00011B1B
		public ES3Type_Matrix4x4Array() : base(typeof(Matrix4x4[]), ES3Type_Matrix4x4.Instance)
		{
			ES3Type_Matrix4x4Array.Instance = this;
		}

		// Token: 0x040000E2 RID: 226
		public static ES3Type Instance;
	}
}
