using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000CC RID: 204
	[Preserve]
	[ES3Properties(new string[]
	{
		"enabled",
		"x",
		"y",
		"z",
		"xMultiplier",
		"yMultiplier",
		"zMultiplier",
		"space"
	})]
	public class ES3Type_VelocityOverLifetimeModule : ES3Type
	{
		// Token: 0x06000403 RID: 1027 RVA: 0x0001A40A File Offset: 0x0001860A
		public ES3Type_VelocityOverLifetimeModule() : base(typeof(ParticleSystem.VelocityOverLifetimeModule))
		{
			ES3Type_VelocityOverLifetimeModule.Instance = this;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001A424 File Offset: 0x00018624
		public override void Write(object obj, ES3Writer writer)
		{
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = (ParticleSystem.VelocityOverLifetimeModule)obj;
			writer.WriteProperty("enabled", velocityOverLifetimeModule.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("x", velocityOverLifetimeModule.x, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("y", velocityOverLifetimeModule.y, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("z", velocityOverLifetimeModule.z, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("xMultiplier", velocityOverLifetimeModule.xMultiplier, ES3Type_float.Instance);
			writer.WriteProperty("yMultiplier", velocityOverLifetimeModule.yMultiplier, ES3Type_float.Instance);
			writer.WriteProperty("zMultiplier", velocityOverLifetimeModule.zMultiplier, ES3Type_float.Instance);
			writer.WriteProperty("space", velocityOverLifetimeModule.space);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001A514 File Offset: 0x00018714
		public override object Read<T>(ES3Reader reader)
		{
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = default(ParticleSystem.VelocityOverLifetimeModule);
			this.ReadInto<T>(reader, velocityOverLifetimeModule);
			return velocityOverLifetimeModule;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001A53C File Offset: 0x0001873C
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = (ParticleSystem.VelocityOverLifetimeModule)obj;
			string text;
			while ((text = reader.ReadPropertyName()) != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 3281097867U)
				{
					if (num <= 726266686U)
					{
						if (num != 49525662U)
						{
							if (num == 726266686U)
							{
								if (text == "zMultiplier")
								{
									velocityOverLifetimeModule.zMultiplier = reader.Read<float>(ES3Type_float.Instance);
									continue;
								}
							}
						}
						else if (text == "enabled")
						{
							velocityOverLifetimeModule.enabled = reader.Read<bool>(ES3Type_bool.Instance);
							continue;
						}
					}
					else if (num != 894689925U)
					{
						if (num == 3281097867U)
						{
							if (text == "yMultiplier")
							{
								velocityOverLifetimeModule.yMultiplier = reader.Read<float>(ES3Type_float.Instance);
								continue;
							}
						}
					}
					else if (text == "space")
					{
						velocityOverLifetimeModule.space = reader.Read<ParticleSystemSimulationSpace>();
						continue;
					}
				}
				else if (num <= 4228665076U)
				{
					if (num != 3709916316U)
					{
						if (num == 4228665076U)
						{
							if (text == "y")
							{
								velocityOverLifetimeModule.y = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
								continue;
							}
						}
					}
					else if (text == "xMultiplier")
					{
						velocityOverLifetimeModule.xMultiplier = reader.Read<float>(ES3Type_float.Instance);
						continue;
					}
				}
				else if (num != 4245442695U)
				{
					if (num == 4278997933U)
					{
						if (text == "z")
						{
							velocityOverLifetimeModule.z = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
							continue;
						}
					}
				}
				else if (text == "x")
				{
					velocityOverLifetimeModule.x = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
					continue;
				}
				reader.Skip();
			}
		}

		// Token: 0x0400010F RID: 271
		public static ES3Type Instance;
	}
}
