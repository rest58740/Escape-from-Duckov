using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000094 RID: 148
	[Preserve]
	[ES3Properties(new string[]
	{
		"enabled",
		"mode",
		"curve",
		"curveMultiplier"
	})]
	public class ES3Type_InheritVelocityModule : ES3Type
	{
		// Token: 0x06000363 RID: 867 RVA: 0x0001134D File Offset: 0x0000F54D
		public ES3Type_InheritVelocityModule() : base(typeof(ParticleSystem.InheritVelocityModule))
		{
			ES3Type_InheritVelocityModule.Instance = this;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00011368 File Offset: 0x0000F568
		public override void Write(object obj, ES3Writer writer)
		{
			ParticleSystem.InheritVelocityModule inheritVelocityModule = (ParticleSystem.InheritVelocityModule)obj;
			writer.WriteProperty("enabled", inheritVelocityModule.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("mode", inheritVelocityModule.mode);
			writer.WriteProperty("curve", inheritVelocityModule.curve, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("curveMultiplier", inheritVelocityModule.curveMultiplier, ES3Type_float.Instance);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x000113E8 File Offset: 0x0000F5E8
		public override object Read<T>(ES3Reader reader)
		{
			ParticleSystem.InheritVelocityModule inheritVelocityModule = default(ParticleSystem.InheritVelocityModule);
			this.ReadInto<T>(reader, inheritVelocityModule);
			return inheritVelocityModule;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00011410 File Offset: 0x0000F610
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			ParticleSystem.InheritVelocityModule inheritVelocityModule = (ParticleSystem.InheritVelocityModule)obj;
			string a;
			while ((a = reader.ReadPropertyName()) != null)
			{
				if (!(a == "enabled"))
				{
					if (!(a == "mode"))
					{
						if (!(a == "curve"))
						{
							if (!(a == "curveMultiplier"))
							{
								reader.Skip();
							}
							else
							{
								inheritVelocityModule.curveMultiplier = reader.Read<float>(ES3Type_float.Instance);
							}
						}
						else
						{
							inheritVelocityModule.curve = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
						}
					}
					else
					{
						inheritVelocityModule.mode = reader.Read<ParticleSystemInheritVelocityMode>();
					}
				}
				else
				{
					inheritVelocityModule.enabled = reader.Read<bool>(ES3Type_bool.Instance);
				}
			}
		}

		// Token: 0x040000D7 RID: 215
		public static ES3Type Instance;
	}
}
