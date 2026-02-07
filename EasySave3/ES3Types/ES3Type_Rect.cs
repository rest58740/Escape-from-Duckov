using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000AA RID: 170
	[Preserve]
	[ES3Properties(new string[]
	{
		"x",
		"y",
		"width",
		"height"
	})]
	public class ES3Type_Rect : ES3Type
	{
		// Token: 0x060003A3 RID: 931 RVA: 0x00015695 File Offset: 0x00013895
		public ES3Type_Rect() : base(typeof(Rect))
		{
			ES3Type_Rect.Instance = this;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x000156B0 File Offset: 0x000138B0
		public override void Write(object obj, ES3Writer writer)
		{
			Rect rect = (Rect)obj;
			writer.WriteProperty("x", rect.x, ES3Type_float.Instance);
			writer.WriteProperty("y", rect.y, ES3Type_float.Instance);
			writer.WriteProperty("width", rect.width, ES3Type_float.Instance);
			writer.WriteProperty("height", rect.height, ES3Type_float.Instance);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00015734 File Offset: 0x00013934
		public override object Read<T>(ES3Reader reader)
		{
			return new Rect(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}

		// Token: 0x040000ED RID: 237
		public static ES3Type Instance;
	}
}
