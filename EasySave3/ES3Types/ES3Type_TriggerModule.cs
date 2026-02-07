using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000C1 RID: 193
	[Preserve]
	[ES3Properties(new string[]
	{
		"enabled",
		"inside",
		"outside",
		"enter",
		"exit",
		"radiusScale"
	})]
	public class ES3Type_TriggerModule : ES3Type
	{
		// Token: 0x060003EA RID: 1002 RVA: 0x00019E1E File Offset: 0x0001801E
		public ES3Type_TriggerModule() : base(typeof(ParticleSystem.TriggerModule))
		{
			ES3Type_TriggerModule.Instance = this;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00019E38 File Offset: 0x00018038
		public override void Write(object obj, ES3Writer writer)
		{
			ParticleSystem.TriggerModule triggerModule = (ParticleSystem.TriggerModule)obj;
			writer.WriteProperty("enabled", triggerModule.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("inside", triggerModule.inside);
			writer.WriteProperty("outside", triggerModule.outside);
			writer.WriteProperty("enter", triggerModule.enter);
			writer.WriteProperty("exit", triggerModule.exit);
			writer.WriteProperty("radiusScale", triggerModule.radiusScale, ES3Type_float.Instance);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00019EE0 File Offset: 0x000180E0
		public override object Read<T>(ES3Reader reader)
		{
			ParticleSystem.TriggerModule triggerModule = default(ParticleSystem.TriggerModule);
			this.ReadInto<T>(reader, triggerModule);
			return triggerModule;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00019F08 File Offset: 0x00018108
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			ParticleSystem.TriggerModule triggerModule = (ParticleSystem.TriggerModule)obj;
			string a;
			while ((a = reader.ReadPropertyName()) != null)
			{
				if (!(a == "enabled"))
				{
					if (!(a == "inside"))
					{
						if (!(a == "outside"))
						{
							if (!(a == "enter"))
							{
								if (!(a == "exit"))
								{
									if (!(a == "radiusScale"))
									{
										reader.Skip();
									}
									else
									{
										triggerModule.radiusScale = reader.Read<float>(ES3Type_float.Instance);
									}
								}
								else
								{
									triggerModule.exit = reader.Read<ParticleSystemOverlapAction>();
								}
							}
							else
							{
								triggerModule.enter = reader.Read<ParticleSystemOverlapAction>();
							}
						}
						else
						{
							triggerModule.outside = reader.Read<ParticleSystemOverlapAction>();
						}
					}
					else
					{
						triggerModule.inside = reader.Read<ParticleSystemOverlapAction>();
					}
				}
				else
				{
					triggerModule.enabled = reader.Read<bool>(ES3Type_bool.Instance);
				}
			}
		}

		// Token: 0x04000104 RID: 260
		public static ES3Type Instance;
	}
}
