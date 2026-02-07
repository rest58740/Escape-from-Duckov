using System;

namespace ES3Types
{
	// Token: 0x02000042 RID: 66
	public class ES3Type_floatArray : ES3ArrayType
	{
		// Token: 0x06000291 RID: 657 RVA: 0x00009F43 File Offset: 0x00008143
		public ES3Type_floatArray() : base(typeof(float[]), ES3Type_float.Instance)
		{
			ES3Type_floatArray.Instance = this;
		}

		// Token: 0x04000087 RID: 135
		public static ES3Type Instance;
	}
}
