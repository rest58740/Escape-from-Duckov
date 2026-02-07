using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000082 RID: 130
	[Preserve]
	[ES3Properties(new string[]
	{
		"enabled",
		"color",
		"range"
	})]
	public class ES3Type_ColorBySpeedModule : ES3Type
	{
		// Token: 0x0600032A RID: 810 RVA: 0x0000FE36 File Offset: 0x0000E036
		public ES3Type_ColorBySpeedModule() : base(typeof(ParticleSystem.ColorBySpeedModule))
		{
			ES3Type_ColorBySpeedModule.Instance = this;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000FE50 File Offset: 0x0000E050
		public override void Write(object obj, ES3Writer writer)
		{
			ParticleSystem.ColorBySpeedModule colorBySpeedModule = (ParticleSystem.ColorBySpeedModule)obj;
			writer.WriteProperty("enabled", colorBySpeedModule.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("color", colorBySpeedModule.color, ES3Type_MinMaxGradient.Instance);
			writer.WriteProperty("range", colorBySpeedModule.range, ES3Type_Vector2.Instance);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000FEB8 File Offset: 0x0000E0B8
		public override object Read<T>(ES3Reader reader)
		{
			ParticleSystem.ColorBySpeedModule colorBySpeedModule = default(ParticleSystem.ColorBySpeedModule);
			this.ReadInto<T>(reader, colorBySpeedModule);
			return colorBySpeedModule;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000FEE0 File Offset: 0x0000E0E0
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			ParticleSystem.ColorBySpeedModule colorBySpeedModule = (ParticleSystem.ColorBySpeedModule)obj;
			string a;
			while ((a = reader.ReadPropertyName()) != null)
			{
				if (!(a == "enabled"))
				{
					if (!(a == "color"))
					{
						if (!(a == "range"))
						{
							reader.Skip();
						}
						else
						{
							colorBySpeedModule.range = reader.Read<Vector2>(ES3Type_Vector2.Instance);
						}
					}
					else
					{
						colorBySpeedModule.color = reader.Read<ParticleSystem.MinMaxGradient>(ES3Type_MinMaxGradient.Instance);
					}
				}
				else
				{
					colorBySpeedModule.enabled = reader.Read<bool>(ES3Type_bool.Instance);
				}
			}
		}

		// Token: 0x040000C2 RID: 194
		public static ES3Type Instance;
	}
}
