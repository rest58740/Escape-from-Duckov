using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000C8 RID: 200
	[Preserve]
	[ES3Properties(new string[]
	{
		"x",
		"y",
		"z"
	})]
	public class ES3Type_Vector3Int : ES3Type
	{
		// Token: 0x060003FA RID: 1018 RVA: 0x0001A1F7 File Offset: 0x000183F7
		public ES3Type_Vector3Int() : base(typeof(Vector3Int))
		{
			ES3Type_Vector3Int.Instance = this;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0001A210 File Offset: 0x00018410
		public override void Write(object obj, ES3Writer writer)
		{
			Vector3Int vector3Int = (Vector3Int)obj;
			writer.WriteProperty("x", vector3Int.x, ES3Type_int.Instance);
			writer.WriteProperty("y", vector3Int.y, ES3Type_int.Instance);
			writer.WriteProperty("z", vector3Int.z, ES3Type_int.Instance);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001A278 File Offset: 0x00018478
		public override object Read<T>(ES3Reader reader)
		{
			return new Vector3Int(reader.ReadProperty<int>(ES3Type_int.Instance), reader.ReadProperty<int>(ES3Type_int.Instance), reader.ReadProperty<int>(ES3Type_int.Instance));
		}

		// Token: 0x0400010B RID: 267
		public static ES3Type Instance;
	}
}
