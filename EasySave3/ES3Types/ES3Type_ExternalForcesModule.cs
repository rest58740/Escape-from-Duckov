using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000085 RID: 133
	[Preserve]
	[ES3Properties(new string[]
	{
		"enabled",
		"multiplier"
	})]
	public class ES3Type_ExternalForcesModule : ES3Type
	{
		// Token: 0x06000336 RID: 822 RVA: 0x00010260 File Offset: 0x0000E460
		public ES3Type_ExternalForcesModule() : base(typeof(ParticleSystem.ExternalForcesModule))
		{
			ES3Type_ExternalForcesModule.Instance = this;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00010278 File Offset: 0x0000E478
		public override void Write(object obj, ES3Writer writer)
		{
			ParticleSystem.ExternalForcesModule externalForcesModule = (ParticleSystem.ExternalForcesModule)obj;
			writer.WriteProperty("enabled", externalForcesModule.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("multiplier", externalForcesModule.multiplier, ES3Type_float.Instance);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x000102C4 File Offset: 0x0000E4C4
		public override object Read<T>(ES3Reader reader)
		{
			ParticleSystem.ExternalForcesModule externalForcesModule = default(ParticleSystem.ExternalForcesModule);
			this.ReadInto<T>(reader, externalForcesModule);
			return externalForcesModule;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000102EC File Offset: 0x0000E4EC
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			ParticleSystem.ExternalForcesModule externalForcesModule = (ParticleSystem.ExternalForcesModule)obj;
			string a;
			while ((a = reader.ReadPropertyName()) != null)
			{
				if (!(a == "enabled"))
				{
					if (!(a == "multiplier"))
					{
						reader.Skip();
					}
					else
					{
						externalForcesModule.multiplier = reader.Read<float>(ES3Type_float.Instance);
					}
				}
				else
				{
					externalForcesModule.enabled = reader.Read<bool>(ES3Type_bool.Instance);
				}
			}
		}

		// Token: 0x040000C5 RID: 197
		public static ES3Type Instance;
	}
}
