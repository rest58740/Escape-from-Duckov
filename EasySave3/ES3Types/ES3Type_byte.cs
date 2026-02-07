using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000033 RID: 51
	[Preserve]
	public class ES3Type_byte : ES3Type
	{
		// Token: 0x0600026D RID: 621 RVA: 0x00009733 File Offset: 0x00007933
		public ES3Type_byte() : base(typeof(byte))
		{
			this.isPrimitive = true;
			ES3Type_byte.Instance = this;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00009752 File Offset: 0x00007952
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((byte)obj);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00009760 File Offset: 0x00007960
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_byte());
		}

		// Token: 0x04000077 RID: 119
		public static ES3Type Instance;
	}
}
