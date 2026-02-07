using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000BF RID: 191
	[Preserve]
	[ES3Properties(new string[]
	{
		"enabled",
		"numTilesX",
		"numTilesY",
		"animation",
		"useRandomRow",
		"frameOverTime",
		"frameOverTimeMultiplier",
		"startFrame",
		"startFrameMultiplier",
		"cycleCount",
		"rowIndex",
		"uvChannelMask",
		"flipU",
		"flipV"
	})]
	public class ES3Type_TextureSheetAnimationModule : ES3Type
	{
		// Token: 0x060003E2 RID: 994 RVA: 0x0001942A File Offset: 0x0001762A
		public ES3Type_TextureSheetAnimationModule() : base(typeof(ParticleSystem.TextureSheetAnimationModule))
		{
			ES3Type_TextureSheetAnimationModule.Instance = this;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00019444 File Offset: 0x00017644
		public override void Write(object obj, ES3Writer writer)
		{
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = (ParticleSystem.TextureSheetAnimationModule)obj;
			writer.WriteProperty("enabled", textureSheetAnimationModule.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("numTilesX", textureSheetAnimationModule.numTilesX, ES3Type_int.Instance);
			writer.WriteProperty("numTilesY", textureSheetAnimationModule.numTilesY, ES3Type_int.Instance);
			writer.WriteProperty("animation", textureSheetAnimationModule.animation);
			writer.WriteProperty("useRandomRow", textureSheetAnimationModule.rowMode);
			writer.WriteProperty("frameOverTime", textureSheetAnimationModule.frameOverTime, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("frameOverTimeMultiplier", textureSheetAnimationModule.frameOverTimeMultiplier, ES3Type_float.Instance);
			writer.WriteProperty("startFrame", textureSheetAnimationModule.startFrame, ES3Type_MinMaxCurve.Instance);
			writer.WriteProperty("startFrameMultiplier", textureSheetAnimationModule.startFrameMultiplier, ES3Type_float.Instance);
			writer.WriteProperty("cycleCount", textureSheetAnimationModule.cycleCount, ES3Type_int.Instance);
			writer.WriteProperty("rowIndex", textureSheetAnimationModule.rowIndex, ES3Type_int.Instance);
			writer.WriteProperty("uvChannelMask", textureSheetAnimationModule.uvChannelMask);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001959C File Offset: 0x0001779C
		public override object Read<T>(ES3Reader reader)
		{
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = default(ParticleSystem.TextureSheetAnimationModule);
			this.ReadInto<T>(reader, textureSheetAnimationModule);
			return textureSheetAnimationModule;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000195C4 File Offset: 0x000177C4
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = (ParticleSystem.TextureSheetAnimationModule)obj;
			string text;
			while ((text = reader.ReadPropertyName()) != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 2539397777U)
				{
					if (num <= 154306809U)
					{
						if (num != 49525662U)
						{
							if (num != 57108932U)
							{
								if (num == 154306809U)
								{
									if (text == "uvChannelMask")
									{
										textureSheetAnimationModule.uvChannelMask = reader.Read<UVChannelFlags>();
										continue;
									}
								}
							}
							else if (text == "startFrame")
							{
								textureSheetAnimationModule.startFrame = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
								continue;
							}
						}
						else if (text == "enabled")
						{
							textureSheetAnimationModule.enabled = reader.Read<bool>(ES3Type_bool.Instance);
							continue;
						}
					}
					else if (num != 834825294U)
					{
						if (num != 968475780U)
						{
							if (num == 2539397777U)
							{
								if (text == "rowIndex")
								{
									textureSheetAnimationModule.rowIndex = reader.Read<int>(ES3Type_int.Instance);
									continue;
								}
							}
						}
						else if (text == "rowMode")
						{
							textureSheetAnimationModule.rowMode = reader.Read<ParticleSystemAnimationRowMode>();
							continue;
						}
					}
					else if (text == "frameOverTimeMultiplier")
					{
						textureSheetAnimationModule.frameOverTimeMultiplier = reader.Read<float>(ES3Type_float.Instance);
						continue;
					}
				}
				else if (num <= 3069737503U)
				{
					if (num != 3035729115U)
					{
						if (num != 3052959884U)
						{
							if (num == 3069737503U)
							{
								if (text == "numTilesY")
								{
									textureSheetAnimationModule.numTilesY = reader.Read<int>(ES3Type_int.Instance);
									continue;
								}
							}
						}
						else if (text == "numTilesX")
						{
							textureSheetAnimationModule.numTilesX = reader.Read<int>(ES3Type_int.Instance);
							continue;
						}
					}
					else if (text == "startFrameMultiplier")
					{
						textureSheetAnimationModule.startFrameMultiplier = reader.Read<float>(ES3Type_float.Instance);
						continue;
					}
				}
				else if (num != 3688237032U)
				{
					if (num != 3779456605U)
					{
						if (num == 4125476797U)
						{
							if (text == "frameOverTime")
							{
								textureSheetAnimationModule.frameOverTime = reader.Read<ParticleSystem.MinMaxCurve>(ES3Type_MinMaxCurve.Instance);
								continue;
							}
						}
					}
					else if (text == "animation")
					{
						textureSheetAnimationModule.animation = reader.Read<ParticleSystemAnimationType>();
						continue;
					}
				}
				else if (text == "cycleCount")
				{
					textureSheetAnimationModule.cycleCount = reader.Read<int>(ES3Type_int.Instance);
					continue;
				}
				reader.Skip();
			}
		}

		// Token: 0x04000102 RID: 258
		public static ES3Type Instance;
	}
}
