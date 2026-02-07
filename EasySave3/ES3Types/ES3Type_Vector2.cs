using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000C2 RID: 194
	[Preserve]
	[ES3Properties(new string[]
	{
		"x",
		"y"
	})]
	public class ES3Type_Vector2 : ES3Type
	{
		// Token: 0x060003EE RID: 1006 RVA: 0x00019FE8 File Offset: 0x000181E8
		public ES3Type_Vector2() : base(typeof(Vector2))
		{
			ES3Type_Vector2.Instance = this;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001A000 File Offset: 0x00018200
		public override void Write(object obj, ES3Writer writer)
		{
			Vector2 vector = (Vector2)obj;
			writer.WriteProperty("x", vector.x, ES3Type_float.Instance);
			writer.WriteProperty("y", vector.y, ES3Type_float.Instance);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001A04A File Offset: 0x0001824A
		public override object Read<T>(ES3Reader reader)
		{
			return new Vector2(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}

		// Token: 0x04000105 RID: 261
		public static ES3Type Instance;
	}
}
