using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000053 RID: 83
	[Preserve]
	public class ES3Type_ulong : ES3Type
	{
		// Token: 0x060002B2 RID: 690 RVA: 0x0000A259 File Offset: 0x00008459
		public ES3Type_ulong() : base(typeof(ulong))
		{
			this.isPrimitive = true;
			ES3Type_ulong.Instance = this;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000A278 File Offset: 0x00008478
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((ulong)obj);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000A286 File Offset: 0x00008486
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_ulong());
		}

		// Token: 0x04000098 RID: 152
		public static ES3Type Instance;
	}
}
