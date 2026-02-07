using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000037 RID: 55
	[Preserve]
	public class ES3Type_DateTime : ES3Type
	{
		// Token: 0x06000277 RID: 631 RVA: 0x00009817 File Offset: 0x00007A17
		public ES3Type_DateTime() : base(typeof(DateTime))
		{
			ES3Type_DateTime.Instance = this;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00009830 File Offset: 0x00007A30
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WriteProperty("ticks", ((DateTime)obj).Ticks, ES3Type_long.Instance);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00009860 File Offset: 0x00007A60
		public override object Read<T>(ES3Reader reader)
		{
			reader.ReadPropertyName();
			return new DateTime(reader.Read<long>(ES3Type_long.Instance));
		}

		// Token: 0x0400007B RID: 123
		public static ES3Type Instance;
	}
}
