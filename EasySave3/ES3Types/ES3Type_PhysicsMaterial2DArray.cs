using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000A7 RID: 167
	public class ES3Type_PhysicsMaterial2DArray : ES3ArrayType
	{
		// Token: 0x0600039E RID: 926 RVA: 0x00015588 File Offset: 0x00013788
		public ES3Type_PhysicsMaterial2DArray() : base(typeof(PhysicsMaterial2D[]), ES3Type_PhysicsMaterial2D.Instance)
		{
			ES3Type_PhysicsMaterial2DArray.Instance = this;
		}

		// Token: 0x040000EA RID: 234
		public static ES3Type Instance;
	}
}
