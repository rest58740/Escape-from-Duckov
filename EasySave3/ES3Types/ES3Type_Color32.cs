using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000080 RID: 128
	[Preserve]
	[ES3Properties(new string[]
	{
		"r",
		"g",
		"b",
		"a"
	})]
	public class ES3Type_Color32 : ES3Type
	{
		// Token: 0x06000325 RID: 805 RVA: 0x0000FD09 File Offset: 0x0000DF09
		public ES3Type_Color32() : base(typeof(Color32))
		{
			ES3Type_Color32.Instance = this;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000FD24 File Offset: 0x0000DF24
		public override void Write(object obj, ES3Writer writer)
		{
			Color32 color = (Color32)obj;
			writer.WriteProperty("r", color.r, ES3Type_byte.Instance);
			writer.WriteProperty("g", color.g, ES3Type_byte.Instance);
			writer.WriteProperty("b", color.b, ES3Type_byte.Instance);
			writer.WriteProperty("a", color.a, ES3Type_byte.Instance);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000FDA4 File Offset: 0x0000DFA4
		public override object Read<T>(ES3Reader reader)
		{
			return new Color32(reader.ReadProperty<byte>(ES3Type_byte.Instance), reader.ReadProperty<byte>(ES3Type_byte.Instance), reader.ReadProperty<byte>(ES3Type_byte.Instance), reader.ReadProperty<byte>(ES3Type_byte.Instance));
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000FDDC File Offset: 0x0000DFDC
		public static bool Equals(Color32 a, Color32 b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		// Token: 0x040000C0 RID: 192
		public static ES3Type Instance;
	}
}
