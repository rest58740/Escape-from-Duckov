using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000039 RID: 57
	[Preserve]
	public class ES3Type_decimal : ES3Type
	{
		// Token: 0x0600027B RID: 635 RVA: 0x0000989B File Offset: 0x00007A9B
		public ES3Type_decimal() : base(typeof(decimal))
		{
			this.isPrimitive = true;
			ES3Type_decimal.Instance = this;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000098BA File Offset: 0x00007ABA
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((decimal)obj);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000098C8 File Offset: 0x00007AC8
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_decimal());
		}

		// Token: 0x0400007D RID: 125
		public static ES3Type Instance;
	}
}
