using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000BA RID: 186
	[Preserve]
	[ES3Properties(new string[]
	{
		"properties",
		"systems",
		"types"
	})]
	public class ES3Type_SubEmittersModule : ES3Type
	{
		// Token: 0x060003D3 RID: 979 RVA: 0x00018E25 File Offset: 0x00017025
		public ES3Type_SubEmittersModule() : base(typeof(ParticleSystem.SubEmittersModule))
		{
			ES3Type_SubEmittersModule.Instance = this;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00018E40 File Offset: 0x00017040
		public override void Write(object obj, ES3Writer writer)
		{
			ParticleSystem.SubEmittersModule subEmittersModule = (ParticleSystem.SubEmittersModule)obj;
			ParticleSystemSubEmitterProperties[] array = new ParticleSystemSubEmitterProperties[subEmittersModule.subEmittersCount];
			ParticleSystem[] array2 = new ParticleSystem[subEmittersModule.subEmittersCount];
			ParticleSystemSubEmitterType[] array3 = new ParticleSystemSubEmitterType[subEmittersModule.subEmittersCount];
			for (int i = 0; i < subEmittersModule.subEmittersCount; i++)
			{
				array[i] = subEmittersModule.GetSubEmitterProperties(i);
				array2[i] = subEmittersModule.GetSubEmitterSystem(i);
				array3[i] = subEmittersModule.GetSubEmitterType(i);
			}
			writer.WriteProperty("properties", array);
			writer.WriteProperty("systems", array2);
			writer.WriteProperty("types", array3);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00018EDC File Offset: 0x000170DC
		public override object Read<T>(ES3Reader reader)
		{
			ParticleSystem.SubEmittersModule subEmittersModule = default(ParticleSystem.SubEmittersModule);
			this.ReadInto<T>(reader, subEmittersModule);
			return subEmittersModule;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00018F04 File Offset: 0x00017104
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			ParticleSystem.SubEmittersModule subEmittersModule = (ParticleSystem.SubEmittersModule)obj;
			ParticleSystemSubEmitterProperties[] array = null;
			ParticleSystem[] array2 = null;
			ParticleSystemSubEmitterType[] array3 = null;
			string a;
			while ((a = reader.ReadPropertyName()) != null)
			{
				if (!(a == "enabled"))
				{
					if (!(a == "properties"))
					{
						if (!(a == "systems"))
						{
							if (!(a == "types"))
							{
								reader.Skip();
							}
							else
							{
								array3 = reader.Read<ParticleSystemSubEmitterType[]>();
							}
						}
						else
						{
							array2 = reader.Read<ParticleSystem[]>();
						}
					}
					else
					{
						array = reader.Read<ParticleSystemSubEmitterProperties[]>(new ES3ArrayType(typeof(ParticleSystemSubEmitterProperties[])));
					}
				}
				else
				{
					subEmittersModule.enabled = reader.Read<bool>(ES3Type_bool.Instance);
				}
			}
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					subEmittersModule.RemoveSubEmitter(i);
					subEmittersModule.AddSubEmitter(array2[i], array3[i], array[i]);
				}
			}
		}

		// Token: 0x040000FD RID: 253
		public static ES3Type Instance;
	}
}
