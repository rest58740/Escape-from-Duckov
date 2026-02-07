using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000CA RID: 202
	[Preserve]
	[ES3Properties(new string[]
	{
		"x",
		"y",
		"z",
		"w"
	})]
	public class ES3Type_Vector4 : ES3Type
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x0001A2C2 File Offset: 0x000184C2
		public ES3Type_Vector4() : base(typeof(Vector4))
		{
			ES3Type_Vector4.Instance = this;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001A2DC File Offset: 0x000184DC
		public override void Write(object obj, ES3Writer writer)
		{
			Vector4 vector = (Vector4)obj;
			writer.WriteProperty("x", vector.x, ES3Type_float.Instance);
			writer.WriteProperty("y", vector.y, ES3Type_float.Instance);
			writer.WriteProperty("z", vector.z, ES3Type_float.Instance);
			writer.WriteProperty("w", vector.w, ES3Type_float.Instance);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001A35C File Offset: 0x0001855C
		public override object Read<T>(ES3Reader reader)
		{
			return new Vector4(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001A394 File Offset: 0x00018594
		public static bool Equals(Vector4 a, Vector4 b)
		{
			return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z) && Mathf.Approximately(a.w, b.w);
		}

		// Token: 0x0400010D RID: 269
		public static ES3Type Instance;
	}
}
