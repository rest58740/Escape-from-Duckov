using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000A5 RID: 165
	public class ES3Type_PhysicMaterialArray : ES3ArrayType
	{
		// Token: 0x06000399 RID: 921 RVA: 0x00015444 File Offset: 0x00013644
		public ES3Type_PhysicMaterialArray() : base(typeof(PhysicMaterial[]), ES3Type_PhysicMaterial.Instance)
		{
			ES3Type_PhysicMaterialArray.Instance = this;
		}

		// Token: 0x040000E8 RID: 232
		public static ES3Type Instance;
	}
}
