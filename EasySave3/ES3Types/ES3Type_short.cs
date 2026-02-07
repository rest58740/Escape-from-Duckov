using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200004B RID: 75
	[Preserve]
	public class ES3Type_short : ES3Type
	{
		// Token: 0x060002A2 RID: 674 RVA: 0x0000A0EE File Offset: 0x000082EE
		public ES3Type_short() : base(typeof(short))
		{
			this.isPrimitive = true;
			ES3Type_short.Instance = this;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000A10D File Offset: 0x0000830D
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((short)obj);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000A11B File Offset: 0x0000831B
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_short());
		}

		// Token: 0x04000090 RID: 144
		public static ES3Type Instance;
	}
}
