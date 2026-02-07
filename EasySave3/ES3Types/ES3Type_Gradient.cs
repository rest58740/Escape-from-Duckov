using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200008D RID: 141
	[Preserve]
	[ES3Properties(new string[]
	{
		"colorKeys",
		"alphaKeys",
		"mode"
	})]
	public class ES3Type_Gradient : ES3Type
	{
		// Token: 0x06000353 RID: 851 RVA: 0x0001109C File Offset: 0x0000F29C
		public ES3Type_Gradient() : base(typeof(Gradient))
		{
			ES3Type_Gradient.Instance = this;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000110B4 File Offset: 0x0000F2B4
		public override void Write(object obj, ES3Writer writer)
		{
			Gradient gradient = (Gradient)obj;
			writer.WriteProperty("colorKeys", gradient.colorKeys, ES3Type_GradientColorKeyArray.Instance);
			writer.WriteProperty("alphaKeys", gradient.alphaKeys, ES3Type_GradientAlphaKeyArray.Instance);
			writer.WriteProperty("mode", gradient.mode);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0001110C File Offset: 0x0000F30C
		public override object Read<T>(ES3Reader reader)
		{
			Gradient gradient = new Gradient();
			this.ReadInto<T>(reader, gradient);
			return gradient;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00011128 File Offset: 0x0000F328
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			Gradient gradient = (Gradient)obj;
			gradient.SetKeys(reader.ReadProperty<GradientColorKey[]>(ES3Type_GradientColorKeyArray.Instance), reader.ReadProperty<GradientAlphaKey[]>(ES3Type_GradientAlphaKeyArray.Instance));
			string a;
			while ((a = reader.ReadPropertyName()) != null)
			{
				if (a == "mode")
				{
					gradient.mode = reader.Read<GradientMode>();
				}
				else
				{
					reader.Skip();
				}
			}
		}

		// Token: 0x040000D0 RID: 208
		public static ES3Type Instance;
	}
}
