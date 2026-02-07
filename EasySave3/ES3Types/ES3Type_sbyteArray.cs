using System;

namespace ES3Types
{
	// Token: 0x0200004A RID: 74
	public class ES3Type_sbyteArray : ES3ArrayType
	{
		// Token: 0x060002A1 RID: 673 RVA: 0x0000A0D1 File Offset: 0x000082D1
		public ES3Type_sbyteArray() : base(typeof(sbyte[]), ES3Type_sbyte.Instance)
		{
			ES3Type_sbyteArray.Instance = this;
		}

		// Token: 0x0400008F RID: 143
		public static ES3Type Instance;
	}
}
