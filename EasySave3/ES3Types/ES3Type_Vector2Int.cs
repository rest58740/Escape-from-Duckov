using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000C4 RID: 196
	[Preserve]
	[ES3Properties(new string[]
	{
		"x",
		"y"
	})]
	public class ES3Type_Vector2Int : ES3Type
	{
		// Token: 0x060003F2 RID: 1010 RVA: 0x0001A089 File Offset: 0x00018289
		public ES3Type_Vector2Int() : base(typeof(Vector2Int))
		{
			ES3Type_Vector2Int.Instance = this;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001A0A4 File Offset: 0x000182A4
		public override void Write(object obj, ES3Writer writer)
		{
			Vector2Int vector2Int = (Vector2Int)obj;
			writer.WriteProperty("x", vector2Int.x, ES3Type_int.Instance);
			writer.WriteProperty("y", vector2Int.y, ES3Type_int.Instance);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001A0F0 File Offset: 0x000182F0
		public override object Read<T>(ES3Reader reader)
		{
			return new Vector2Int(reader.ReadProperty<int>(ES3Type_int.Instance), reader.ReadProperty<int>(ES3Type_int.Instance));
		}

		// Token: 0x04000107 RID: 263
		public static ES3Type Instance;
	}
}
