using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000B1 RID: 177
	public class ES3Type_ShaderArray : ES3ArrayType
	{
		// Token: 0x060003BA RID: 954 RVA: 0x00016D5C File Offset: 0x00014F5C
		public ES3Type_ShaderArray() : base(typeof(Shader[]), ES3Type_Shader.Instance)
		{
			ES3Type_ShaderArray.Instance = this;
		}

		// Token: 0x040000F4 RID: 244
		public static ES3Type Instance;
	}
}
