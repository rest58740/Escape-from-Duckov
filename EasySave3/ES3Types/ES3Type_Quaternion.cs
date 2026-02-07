using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000A8 RID: 168
	[Preserve]
	[ES3Properties(new string[]
	{
		"x",
		"y",
		"z",
		"w"
	})]
	public class ES3Type_Quaternion : ES3Type
	{
		// Token: 0x0600039F RID: 927 RVA: 0x000155A5 File Offset: 0x000137A5
		public ES3Type_Quaternion() : base(typeof(Quaternion))
		{
			ES3Type_Quaternion.Instance = this;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x000155C0 File Offset: 0x000137C0
		public override void Write(object obj, ES3Writer writer)
		{
			Quaternion quaternion = (Quaternion)obj;
			writer.WriteProperty("x", quaternion.x, ES3Type_float.Instance);
			writer.WriteProperty("y", quaternion.y, ES3Type_float.Instance);
			writer.WriteProperty("z", quaternion.z, ES3Type_float.Instance);
			writer.WriteProperty("w", quaternion.w, ES3Type_float.Instance);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00015640 File Offset: 0x00013840
		public override object Read<T>(ES3Reader reader)
		{
			return new Quaternion(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}

		// Token: 0x040000EB RID: 235
		public static ES3Type Instance;
	}
}
