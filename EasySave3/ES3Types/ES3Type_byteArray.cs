using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000034 RID: 52
	[Preserve]
	public class ES3Type_byteArray : ES3Type
	{
		// Token: 0x06000270 RID: 624 RVA: 0x00009777 File Offset: 0x00007977
		public ES3Type_byteArray() : base(typeof(byte[]))
		{
			this.isPrimitive = true;
			ES3Type_byteArray.Instance = this;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00009796 File Offset: 0x00007996
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((byte[])obj);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x000097A4 File Offset: 0x000079A4
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_byteArray());
		}

		// Token: 0x04000078 RID: 120
		public static ES3Type Instance;
	}
}
