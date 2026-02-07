using System;

namespace ES3Types
{
	// Token: 0x02000052 RID: 82
	public class ES3Type_UIntPtrArray : ES3ArrayType
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x0000A23C File Offset: 0x0000843C
		public ES3Type_UIntPtrArray() : base(typeof(UIntPtr[]), ES3Type_UIntPtr.Instance)
		{
			ES3Type_UIntPtrArray.Instance = this;
		}

		// Token: 0x04000097 RID: 151
		public static ES3Type Instance;
	}
}
