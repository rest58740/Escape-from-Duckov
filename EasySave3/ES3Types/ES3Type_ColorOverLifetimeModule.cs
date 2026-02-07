using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000083 RID: 131
	[Preserve]
	[ES3Properties(new string[]
	{
		"enabled",
		"color"
	})]
	public class ES3Type_ColorOverLifetimeModule : ES3Type
	{
		// Token: 0x0600032E RID: 814 RVA: 0x0000FF6B File Offset: 0x0000E16B
		public ES3Type_ColorOverLifetimeModule() : base(typeof(ParticleSystem.ColorOverLifetimeModule))
		{
			ES3Type_ColorOverLifetimeModule.Instance = this;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000FF84 File Offset: 0x0000E184
		public override void Write(object obj, ES3Writer writer)
		{
			ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = (ParticleSystem.ColorOverLifetimeModule)obj;
			writer.WriteProperty("enabled", colorOverLifetimeModule.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("color", colorOverLifetimeModule.color, ES3Type_MinMaxGradient.Instance);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000FFD0 File Offset: 0x0000E1D0
		public override object Read<T>(ES3Reader reader)
		{
			ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = default(ParticleSystem.ColorOverLifetimeModule);
			this.ReadInto<T>(reader, colorOverLifetimeModule);
			return colorOverLifetimeModule;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000FFF8 File Offset: 0x0000E1F8
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = (ParticleSystem.ColorOverLifetimeModule)obj;
			string a;
			while ((a = reader.ReadPropertyName()) != null)
			{
				if (!(a == "enabled"))
				{
					if (!(a == "color"))
					{
						reader.Skip();
					}
					else
					{
						colorOverLifetimeModule.color = reader.Read<ParticleSystem.MinMaxGradient>(ES3Type_MinMaxGradient.Instance);
					}
				}
				else
				{
					colorOverLifetimeModule.enabled = reader.Read<bool>(ES3Type_bool.Instance);
				}
			}
		}

		// Token: 0x040000C3 RID: 195
		public static ES3Type Instance;
	}
}
