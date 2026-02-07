using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200007E RID: 126
	[Preserve]
	[ES3Properties(new string[]
	{
		"r",
		"g",
		"b",
		"a"
	})]
	public class ES3Type_Color : ES3Type
	{
		// Token: 0x06000321 RID: 801 RVA: 0x0000FC1C File Offset: 0x0000DE1C
		public ES3Type_Color() : base(typeof(Color))
		{
			ES3Type_Color.Instance = this;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000FC34 File Offset: 0x0000DE34
		public override void Write(object obj, ES3Writer writer)
		{
			Color color = (Color)obj;
			writer.WriteProperty("r", color.r, ES3Type_float.Instance);
			writer.WriteProperty("g", color.g, ES3Type_float.Instance);
			writer.WriteProperty("b", color.b, ES3Type_float.Instance);
			writer.WriteProperty("a", color.a, ES3Type_float.Instance);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000FCB4 File Offset: 0x0000DEB4
		public override object Read<T>(ES3Reader reader)
		{
			return new Color(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}

		// Token: 0x040000BE RID: 190
		public static ES3Type Instance;
	}
}
