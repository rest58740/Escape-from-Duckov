using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200008A RID: 138
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
		"space",
		"randomized"
	})]
	public class ES3Type_ForceOverLifetimeModule : ES3Type
	{
		// Token: 0x06000344 RID: 836 RVA: 0x000104F8 File Offset: 0x0000E6F8
		public ES3Type_ForceOverLifetimeModule() : base(typeof(ParticleSystem.ForceOverLifetimeModule))
		{
			ES3Type_ForceOverLifetimeModule.Instance = this;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00010510 File Offset: 0x0000E710
		public override void Write(object obj, ES3Writer writer)
		{
			ParticleSystem.ForceOverLifetimeModule forceOverLifetimeModule = (ParticleSystem.ForceOverLifetimeModule)obj;
			writer.WriteProperty("enabled", forceOverLifetimeModule.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("x", forceOverLifetimeModule.x, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("y", forceOverLifetimeModule.y, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("z", forceOverLifetimeModule.z, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("xMultiplier", forceOverLifetimeModule.xMultiplier, ES3Type_float.Instance);
			writer.WriteProperty("yMultiplier", forceOverLifetimeModule.yMultiplier, ES3Type_float.Instance);
			writer.WriteProperty("zMultiplier", forceOverLifetimeModule.zMultiplier, ES3Type_float.Instance);
			writer.WriteProperty("space", forceOverLifetimeModule.space);
			writer.WriteProperty("randomized", forceOverLifetimeModule.randomized, ES3Type_bool.Instance);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0001061C File Offset: 0x0000E81C
		public override object Read<T>(ES3Reader reader)
		{
			ParticleSystem.ForceOverLifetimeModule forceOverLifetimeModule = default(ParticleSystem.ForceOverLifetimeModule);
			this.ReadInto<T>(reader, forceOverLifetimeModule);
			return forceOverLifetimeModule;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00010644 File Offset: 0x0000E844
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			ParticleSystem.ForceOverLifetimeModule forceOverLifetimeModule = (ParticleSystem.ForceOverLifetimeModule)obj;
			string text;
			while ((text = reader.ReadPropertyName()) != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 1194511624U)
				{
					if (num <= 726266686U)
					{
						if (num != 49525662U)
						{
							if (num == 726266686U)
							{
								if (text == "zMultiplier")
								{
									forceOverLifetimeModule.zMultiplier = reader.Read<float>(ES3Type_float.Instance);
									continue;
								}
							}
						}
						else if (text == "enabled")
						{
							forceOverLifetimeModule.enabled = reader.Read<bool>(ES3Type_bool.Instance);
							continue;
						}
					}
					else if (num != 894689925U)
					{
						if (num == 1194511624U)
						{
							if (text == "randomized")
							{
								forceOverLifetimeModule.randomized = reader.Read<bool>(ES3Type_bool.Instance);
								continue;
							}
						}
					}
					else if (text == "space")
					{
						forceOverLifetimeModule.space = reader.Read<ParticleSystemSimulationSpace>();
						continue;
					}
				}
				else if (num <= 3709916316U)
				{
					if (num != 3281097867U)
					{
						if (num == 3709916316U)
						{
							if (text == "xMultiplier")
							{
								forceOverLifetimeModule.xMultiplier = reader.Read<float>(ES3Type_float.Instance);
								continue;
							}
						}
					}
					else if (text == "yMultiplier")
					{
						forceOverLifetimeModule.yMultiplier = reader.Read<float>(ES3Type_float.Instance);
						continue;
					}
				}
				else if (num != 4228665076U)
				{
					if (num != 4245442695U)
					{
						if (num == 4278997933U)
						{
							if (text == "z")
							{
								forceOverLifetimeModule.z = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
								continue;
							}
						}
					}
					else if (text == "x")
					{
						forceOverLifetimeModule.x = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
						continue;
					}
				}
				else if (text == "y")
				{
					forceOverLifetimeModule.y = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
					continue;
				}
				reader.Skip();
			}
		}

		// Token: 0x040000CA RID: 202
		public static ES3Type Instance;
	}
}
