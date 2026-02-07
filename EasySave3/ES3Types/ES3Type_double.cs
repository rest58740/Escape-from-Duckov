using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200003B RID: 59
	[Preserve]
	public class ES3Type_double : ES3Type
	{
		// Token: 0x0600027F RID: 639 RVA: 0x000098FC File Offset: 0x00007AFC
		public ES3Type_double() : base(typeof(double))
		{
			this.isPrimitive = true;
			ES3Type_double.Instance = this;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000991B File Offset: 0x00007B1B
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((double)obj);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00009929 File Offset: 0x00007B29
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_double());
		}

		// Token: 0x0400007F RID: 127
		public static ES3Type Instance;
	}
}
