using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200004D RID: 77
	[Preserve]
	public class ES3Type_string : ES3Type
	{
		// Token: 0x060002A6 RID: 678 RVA: 0x0000A14F File Offset: 0x0000834F
		public ES3Type_string() : base(typeof(string))
		{
			this.isPrimitive = true;
			ES3Type_string.Instance = this;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000A16E File Offset: 0x0000836E
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((string)obj);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000A17C File Offset: 0x0000837C
		public override object Read<T>(ES3Reader reader)
		{
			return reader.Read_string();
		}

		// Token: 0x04000092 RID: 146
		public static ES3Type Instance;
	}
}
