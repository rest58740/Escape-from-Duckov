using System;
using ES3Internal;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000BD RID: 189
	[Preserve]
	[ES3Properties(new string[]
	{
		"filterMode",
		"anisoLevel",
		"wrapMode",
		"mipMapBias",
		"rawTextureData"
	})]
	public class ES3Type_Texture2D : ES3UnityObjectType
	{
		// Token: 0x060003DC RID: 988 RVA: 0x000190D1 File Offset: 0x000172D1
		public ES3Type_Texture2D() : base(typeof(Texture2D))
		{
			ES3Type_Texture2D.Instance = this;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000190EC File Offset: 0x000172EC
		protected override void WriteUnityObject(object obj, ES3Writer writer)
		{
			Texture2D texture2D = (Texture2D)obj;
			if (!this.IsReadable(texture2D))
			{
				ES3Debug.LogWarning("Easy Save cannot save the pixels or properties of this Texture because it is not read/write enabled, so Easy Save will store it by reference instead. To save the pixel data, check the 'Read/Write Enabled' checkbox in the Texture's import settings. Clicking this warning will take you to the Texture, assuming it is not generated at runtime.", texture2D, 0);
				return;
			}
			writer.WriteProperty("width", texture2D.width, ES3Type_int.Instance);
			writer.WriteProperty("height", texture2D.height, ES3Type_int.Instance);
			writer.WriteProperty("format", texture2D.format);
			writer.WriteProperty("mipmapCount", texture2D.mipmapCount, ES3Type_int.Instance);
			writer.WriteProperty("filterMode", texture2D.filterMode);
			writer.WriteProperty("anisoLevel", texture2D.anisoLevel, ES3Type_int.Instance);
			writer.WriteProperty("wrapMode", texture2D.wrapMode);
			writer.WriteProperty("mipMapBias", texture2D.mipMapBias, ES3Type_float.Instance);
			writer.WriteProperty("rawTextureData", texture2D.GetRawTextureData(), ES3Type_byteArray.Instance);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000191F8 File Offset: 0x000173F8
		protected override void ReadUnityObject<T>(ES3Reader reader, object obj)
		{
			if (obj == null)
			{
				return;
			}
			if (obj.GetType() == typeof(RenderTexture))
			{
				ES3Type_RenderTexture.Instance.ReadInto<T>(reader, obj);
				return;
			}
			Texture2D texture2D = (Texture2D)obj;
			if (!this.IsReadable(texture2D))
			{
				ES3Debug.LogWarning("Easy Save cannot load the properties or pixels for this Texture because it is not read/write enabled, so it will be loaded by reference. To load the properties and pixels for this Texture, check the 'Read/Write Enabled' checkbox in its Import Settings.", texture2D, 0);
			}
			foreach (object obj2 in reader.Properties)
			{
				string a = (string)obj2;
				if (!this.IsReadable(texture2D))
				{
					reader.Skip();
				}
				else if (!(a == "filterMode"))
				{
					if (!(a == "anisoLevel"))
					{
						if (!(a == "wrapMode"))
						{
							if (!(a == "mipMapBias"))
							{
								if (a == "rawTextureData")
								{
									if (!this.IsReadable(texture2D))
									{
										ES3Debug.LogWarning("Easy Save cannot load the pixels of this Texture because it is not read/write enabled, so Easy Save will ignore the pixel data. To load the pixel data, check the 'Read/Write Enabled' checkbox in the Texture's import settings. Clicking this warning will take you to the Texture, assuming it is not generated at runtime.", texture2D, 0);
										reader.Skip();
										continue;
									}
									try
									{
										texture2D.LoadRawTextureData(reader.Read<byte[]>(ES3Type_byteArray.Instance));
										texture2D.Apply();
										continue;
									}
									catch (Exception ex)
									{
										ES3Debug.LogError("Easy Save encountered an error when trying to load this Texture, please see the end of this messasge for the error. This is most likely because the Texture format of the instance we are loading into is different to the Texture we saved.\n" + ex.ToString(), texture2D, 0);
										continue;
									}
								}
								reader.Skip();
							}
							else
							{
								texture2D.mipMapBias = reader.Read<float>(ES3Type_float.Instance);
							}
						}
						else
						{
							texture2D.wrapMode = reader.Read<TextureWrapMode>();
						}
					}
					else
					{
						texture2D.anisoLevel = reader.Read<int>(ES3Type_int.Instance);
					}
				}
				else
				{
					texture2D.filterMode = reader.Read<FilterMode>();
				}
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x000193B4 File Offset: 0x000175B4
		protected override object ReadUnityObject<T>(ES3Reader reader)
		{
			Texture2D texture2D = new Texture2D(reader.Read<int>(ES3Type_int.Instance), reader.ReadProperty<int>(ES3Type_int.Instance), reader.ReadProperty<TextureFormat>(), reader.ReadProperty<int>(ES3Type_int.Instance) > 1);
			this.ReadObject<T>(reader, texture2D);
			return texture2D;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x000193FA File Offset: 0x000175FA
		protected bool IsReadable(Texture2D instance)
		{
			return instance != null && instance.isReadable;
		}

		// Token: 0x04000100 RID: 256
		public static ES3Type Instance;
	}
}
