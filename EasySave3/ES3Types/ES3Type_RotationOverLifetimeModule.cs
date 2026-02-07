using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000AF RID: 175
	[Preserve]
	[ES3Properties(new string[]
	{
		"enabled",
		"x",
		"xMultiplier",
		"y",
		"yMultiplier",
		"z",
		"zMultiplier",
		"separateAxes"
	})]
	public class ES3Type_RotationOverLifetimeModule : ES3Type
	{
		// Token: 0x060003B2 RID: 946 RVA: 0x000168F2 File Offset: 0x00014AF2
		public ES3Type_RotationOverLifetimeModule() : base(typeof(ParticleSystem.RotationOverLifetimeModule))
		{
			ES3Type_RotationOverLifetimeModule.Instance = this;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001690C File Offset: 0x00014B0C
		public override void Write(object obj, ES3Writer writer)
		{
			ParticleSystem.RotationOverLifetimeModule rotationOverLifetimeModule = (ParticleSystem.RotationOverLifetimeModule)obj;
			writer.WriteProperty("enabled", rotationOverLifetimeModule.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("x", rotationOverLifetimeModule.x, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("xMultiplier", rotationOverLifetimeModule.xMultiplier, ES3Type_float.Instance);
			writer.WriteProperty("y", rotationOverLifetimeModule.y, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("yMultiplier", rotationOverLifetimeModule.yMultiplier, ES3Type_float.Instance);
			writer.WriteProperty("z", rotationOverLifetimeModule.z, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("zMultiplier", rotationOverLifetimeModule.zMultiplier, ES3Type_float.Instance);
			writer.WriteProperty("separateAxes", rotationOverLifetimeModule.separateAxes, ES3Type_bool.Instance);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00016A00 File Offset: 0x00014C00
		public override object Read<T>(ES3Reader reader)
		{
			ParticleSystem.RotationOverLifetimeModule rotationOverLifetimeModule = default(ParticleSystem.RotationOverLifetimeModule);
			this.ReadInto<T>(reader, rotationOverLifetimeModule);
			return rotationOverLifetimeModule;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00016A28 File Offset: 0x00014C28
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			ParticleSystem.RotationOverLifetimeModule rotationOverLifetimeModule = (ParticleSystem.RotationOverLifetimeModule)obj;
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
									rotationOverLifetimeModule.zMultiplier = reader.Read<float>(ES3Type_float.Instance);
									continue;
								}
							}
						}
						else if (text == "enabled")
						{
							rotationOverLifetimeModule.enabled = reader.Read<bool>(ES3Type_bool.Instance);
							continue;
						}
					}
					else if (num != 1479031685U)
					{
						if (num == 3281097867U)
						{
							if (text == "yMultiplier")
							{
								rotationOverLifetimeModule.yMultiplier = reader.Read<float>(ES3Type_float.Instance);
								continue;
							}
						}
					}
					else if (text == "separateAxes")
					{
						rotationOverLifetimeModule.separateAxes = reader.Read<bool>(ES3Type_bool.Instance);
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
								rotationOverLifetimeModule.y = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
								continue;
							}
						}
					}
					else if (text == "xMultiplier")
					{
						rotationOverLifetimeModule.xMultiplier = reader.Read<float>(ES3Type_float.Instance);
						continue;
					}
				}
				else if (num != 4245442695U)
				{
					if (num == 4278997933U)
					{
						if (text == "z")
						{
							rotationOverLifetimeModule.z = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
							continue;
						}
					}
				}
				else if (text == "x")
				{
					rotationOverLifetimeModule.x = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
					continue;
				}
				reader.Skip();
			}
		}

		// Token: 0x040000F2 RID: 242
		public static ES3Type Instance;
	}
}
