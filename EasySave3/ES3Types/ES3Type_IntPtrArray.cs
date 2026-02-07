using System;

namespace ES3Types
{
	// Token: 0x02000046 RID: 70
	public class ES3Type_IntPtrArray : ES3ArrayType
	{
		// Token: 0x06000299 RID: 665 RVA: 0x0000A00F File Offset: 0x0000820F
		public ES3Type_IntPtrArray() : base(typeof(IntPtr[]), ES3Type_IntPtr.Instance)
		{
			ES3Type_IntPtrArray.Instance = this;
		}

		// Token: 0x0400008B RID: 139
		public static ES3Type Instance;
	}
}
