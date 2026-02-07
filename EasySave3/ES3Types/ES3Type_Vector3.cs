using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000C6 RID: 198
	[Preserve]
	[ES3Properties(new string[]
	{
		"x",
		"y",
		"z"
	})]
	public class ES3Type_Vector3 : ES3Type
	{
		// Token: 0x060003F6 RID: 1014 RVA: 0x0001A12F File Offset: 0x0001832F
		public ES3Type_Vector3() : base(typeof(Vector3))
		{
			ES3Type_Vector3.Instance = this;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001A148 File Offset: 0x00018348
		public override void Write(object obj, ES3Writer writer)
		{
			Vector3 vector = (Vector3)obj;
			writer.WriteProperty("x", vector.x, ES3Type_float.Instance);
			writer.WriteProperty("y", vector.y, ES3Type_float.Instance);
			writer.WriteProperty("z", vector.z, ES3Type_float.Instance);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001A1AD File Offset: 0x000183AD
		public override object Read<T>(ES3Reader reader)
		{
			return new Vector3(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}

		// Token: 0x04000109 RID: 265
		public static ES3Type Instance;
	}
}
