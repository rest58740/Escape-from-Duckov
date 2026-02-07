using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200007B RID: 123
	[Preserve]
	[ES3Properties(new string[]
	{
		"time",
		"count",
		"minCount",
		"maxCount",
		"cycleCount",
		"repeatInterval",
		"probability"
	})]
	public class ES3Type_Burst : ES3Type
	{
		// Token: 0x06000319 RID: 793 RVA: 0x0000F373 File Offset: 0x0000D573
		public ES3Type_Burst() : base(typeof(ParticleSystem.Burst))
		{
			ES3Type_Burst.Instance = this;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000F38C File Offset: 0x0000D58C
		public override void Write(object obj, ES3Writer writer)
		{
			ParticleSystem.Burst burst = (ParticleSystem.Burst)obj;
			writer.WriteProperty("time", burst.time, ES3Type_float.Instance);
			writer.WriteProperty("count", burst.count, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("minCount", burst.minCount, ES3Type_short.Instance);
			writer.WriteProperty("maxCount", burst.maxCount, ES3Type_short.Instance);
			writer.WriteProperty("cycleCount", burst.cycleCount, ES3Type_int.Instance);
			writer.WriteProperty("repeatInterval", burst.repeatInterval, ES3Type_float.Instance);
			writer.WriteProperty("probability", burst.probability, ES3Type_float.Instance);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000F464 File Offset: 0x0000D664
		public override object Read<T>(ES3Reader reader)
		{
			ParticleSystem.Burst burst = default(ParticleSystem.Burst);
			string text;
			while ((text = reader.ReadPropertyName()) != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 1688821116U)
				{
					if (num != 967958004U)
					{
						if (num != 1564253156U)
						{
							if (num == 1688821116U)
							{
								if (text == "probability")
								{
									burst.probability = reader.Read<float>(ES3Type_float.Instance);
									continue;
								}
							}
						}
						else if (text == "time")
						{
							burst.time = reader.Read<float>(ES3Type_float.Instance);
							continue;
						}
					}
					else if (text == "count")
					{
						burst.count = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
						continue;
					}
				}
				else if (num <= 2412367502U)
				{
					if (num != 2254115328U)
					{
						if (num == 2412367502U)
						{
							if (text == "minCount")
							{
								burst.minCount = reader.Read<short>(ES3Type_short.Instance);
								continue;
							}
						}
					}
					else if (text == "maxCount")
					{
						burst.maxCount = reader.Read<short>(ES3Type_short.Instance);
						continue;
					}
				}
				else if (num != 3688237032U)
				{
					if (num == 4028083871U)
					{
						if (text == "repeatInterval")
						{
							burst.repeatInterval = reader.Read<float>(ES3Type_float.Instance);
							continue;
						}
					}
				}
				else if (text == "cycleCount")
				{
					burst.cycleCount = reader.Read<int>(ES3Type_int.Instance);
					continue;
				}
				reader.Skip();
			}
			return burst;
		}

		// Token: 0x040000BB RID: 187
		public static ES3Type Instance;
	}
}
