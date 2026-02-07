using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000084 RID: 132
	[Preserve]
	[ES3Properties(new string[]
	{
		"enabled",
		"rateOverTime",
		"rateOverTimeMultiplier",
		"rateOverDistance",
		"rateOverDistanceMultiplier"
	})]
	public class ES3Type_EmissionModule : ES3Type
	{
		// Token: 0x06000332 RID: 818 RVA: 0x00010062 File Offset: 0x0000E262
		public ES3Type_EmissionModule() : base(typeof(ParticleSystem.EmissionModule))
		{
			ES3Type_EmissionModule.Instance = this;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0001007C File Offset: 0x0000E27C
		public override void Write(object obj, ES3Writer writer)
		{
			ParticleSystem.EmissionModule emissionModule = (ParticleSystem.EmissionModule)obj;
			writer.WriteProperty("enabled", emissionModule.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("rateOverTime", emissionModule.rateOverTime, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("rateOverTimeMultiplier", emissionModule.rateOverTimeMultiplier, ES3Type_float.Instance);
			writer.WriteProperty("rateOverDistance", emissionModule.rateOverDistance, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("rateOverDistanceMultiplier", emissionModule.rateOverDistanceMultiplier, ES3Type_float.Instance);
			ParticleSystem.Burst[] array = new ParticleSystem.Burst[emissionModule.burstCount];
			emissionModule.GetBursts(array);
			writer.WriteProperty("bursts", array, ES3Type_BurstArray.Instance);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00010144 File Offset: 0x0000E344
		public override object Read<T>(ES3Reader reader)
		{
			ParticleSystem.EmissionModule emissionModule = default(ParticleSystem.EmissionModule);
			this.ReadInto<T>(reader, emissionModule);
			return emissionModule;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0001016C File Offset: 0x0000E36C
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			ParticleSystem.EmissionModule emissionModule = (ParticleSystem.EmissionModule)obj;
			string a;
			while ((a = reader.ReadPropertyName()) != null)
			{
				if (!(a == "enabled"))
				{
					if (!(a == "rateOverTime"))
					{
						if (!(a == "rateOverTimeMultiplier"))
						{
							if (!(a == "rateOverDistance"))
							{
								if (!(a == "rateOverDistanceMultiplier"))
								{
									if (!(a == "bursts"))
									{
										reader.Skip();
									}
									else
									{
										emissionModule.SetBursts(reader.Read<ParticleSystem.Burst[]>(ES3Type_BurstArray.Instance));
									}
								}
								else
								{
									emissionModule.rateOverDistanceMultiplier = reader.Read<float>(ES3Type_float.Instance);
								}
							}
							else
							{
								emissionModule.rateOverDistance = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
							}
						}
						else
						{
							emissionModule.rateOverTimeMultiplier = reader.Read<float>(ES3Type_float.Instance);
						}
					}
					else
					{
						emissionModule.rateOverTime = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
					}
				}
				else
				{
					emissionModule.enabled = reader.Read<bool>(ES3Type_bool.Instance);
				}
			}
		}

		// Token: 0x040000C4 RID: 196
		public static ES3Type Instance;
	}
}
