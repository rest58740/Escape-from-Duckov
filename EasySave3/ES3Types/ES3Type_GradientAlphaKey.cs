using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200008E RID: 142
	[Preserve]
	[ES3Properties(new string[]
	{
		"alpha",
		"time"
	})]
	public class ES3Type_GradientAlphaKey : ES3Type
	{
		// Token: 0x06000357 RID: 855 RVA: 0x00011185 File Offset: 0x0000F385
		public ES3Type_GradientAlphaKey() : base(typeof(GradientAlphaKey))
		{
			ES3Type_GradientAlphaKey.Instance = this;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000111A0 File Offset: 0x0000F3A0
		public override void Write(object obj, ES3Writer writer)
		{
			GradientAlphaKey gradientAlphaKey = (GradientAlphaKey)obj;
			writer.WriteProperty("alpha", gradientAlphaKey.alpha, ES3Type_float.Instance);
			writer.WriteProperty("time", gradientAlphaKey.time, ES3Type_float.Instance);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000111EA File Offset: 0x0000F3EA
		public override object Read<T>(ES3Reader reader)
		{
			return new GradientAlphaKey(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}

		// Token: 0x040000D1 RID: 209
		public static ES3Type Instance;
	}
}
