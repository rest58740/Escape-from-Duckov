using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000090 RID: 144
	[Preserve]
	[ES3Properties(new string[]
	{
		"color",
		"time"
	})]
	public class ES3Type_GradientColorKey : ES3Type
	{
		// Token: 0x0600035B RID: 859 RVA: 0x00011229 File Offset: 0x0000F429
		public ES3Type_GradientColorKey() : base(typeof(GradientColorKey))
		{
			ES3Type_GradientColorKey.Instance = this;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00011244 File Offset: 0x0000F444
		public override void Write(object obj, ES3Writer writer)
		{
			GradientColorKey gradientColorKey = (GradientColorKey)obj;
			writer.WriteProperty("color", gradientColorKey.color, ES3Type_Color.Instance);
			writer.WriteProperty("time", gradientColorKey.time, ES3Type_float.Instance);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0001128E File Offset: 0x0000F48E
		public override object Read<T>(ES3Reader reader)
		{
			return new GradientColorKey(reader.ReadProperty<Color>(ES3Type_Color.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}

		// Token: 0x040000D3 RID: 211
		public static ES3Type Instance;
	}
}
