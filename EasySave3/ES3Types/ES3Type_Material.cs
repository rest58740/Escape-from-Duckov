using System;
using ES3Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200009C RID: 156
	[Preserve]
	[ES3Properties(new string[]
	{
		"shader",
		"renderQueue",
		"shaderKeywords",
		"globalIlluminationFlags",
		"properties"
	})]
	public class ES3Type_Material : ES3UnityObjectType
	{
		// Token: 0x0600037D RID: 893 RVA: 0x0001338C File Offset: 0x0001158C
		public ES3Type_Material() : base(typeof(Material))
		{
			ES3Type_Material.Instance = this;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000133A4 File Offset: 0x000115A4
		protected override void WriteUnityObject(object obj, ES3Writer writer)
		{
			Material material = (Material)obj;
			writer.WriteProperty("name", material.name);
			writer.WriteProperty("shader", material.shader);
			writer.WriteProperty("renderQueue", material.renderQueue, ES3Type_int.Instance);
			writer.WriteProperty("shaderKeywords", material.shaderKeywords);
			writer.WriteProperty("globalIlluminationFlags", material.globalIlluminationFlags);
			Shader shader = material.shader;
			if (shader != null)
			{
				for (int i = 0; i < shader.GetPropertyCount(); i++)
				{
					string propertyName = shader.GetPropertyName(i);
					switch (shader.GetPropertyType(i))
					{
					case ShaderPropertyType.Color:
						writer.WriteProperty(propertyName, material.GetColor(propertyName));
						break;
					case ShaderPropertyType.Vector:
						writer.WriteProperty(propertyName, material.GetVector(propertyName));
						break;
					case ShaderPropertyType.Float:
					case ShaderPropertyType.Range:
						writer.WriteProperty(propertyName, material.GetFloat(propertyName));
						break;
					case ShaderPropertyType.Texture:
					{
						Texture texture = material.GetTexture(propertyName);
						if (texture != null && texture.GetType() != typeof(Texture2D))
						{
							ES3Debug.LogWarning(string.Format("The texture '{0}' of Material '{1}' will not be saved as only Textures of type Texture2D can be saved at runtime, whereas '{2}' is of type '{3}'.", new object[]
							{
								propertyName,
								material.name,
								propertyName,
								texture.GetType()
							}), null, 0);
						}
						else
						{
							writer.WriteProperty(propertyName, texture);
							writer.WriteProperty(propertyName + "_TextureOffset", material.GetTextureOffset(propertyName));
							writer.WriteProperty(propertyName + "_TextureScale", material.GetTextureScale(propertyName));
						}
						break;
					}
					}
				}
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00013560 File Offset: 0x00011760
		protected override object ReadUnityObject<T>(ES3Reader reader)
		{
			Material material = new Material(Shader.Find("Diffuse"));
			this.ReadUnityObject<T>(reader, material);
			return material;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00013588 File Offset: 0x00011788
		protected override void ReadUnityObject<T>(ES3Reader reader, object obj)
		{
			Material material = (Material)obj;
			foreach (object obj2 in reader.Properties)
			{
				string text = (string)obj2;
				if (!(text == "name"))
				{
					if (!(text == "shader"))
					{
						if (!(text == "renderQueue"))
						{
							if (!(text == "shaderKeywords"))
							{
								if (!(text == "globalIlluminationFlags"))
								{
									if (!(text == "_MainTex_Scale"))
									{
										int propertyIndex;
										if (material.shader != null && material.HasProperty(text) && (propertyIndex = material.shader.FindPropertyIndex(text)) != -1)
										{
											switch (material.shader.GetPropertyType(propertyIndex))
											{
											case ShaderPropertyType.Color:
												material.SetColor(text, reader.Read<Color>());
												break;
											case ShaderPropertyType.Vector:
												material.SetColor(text, reader.Read<Vector4>());
												break;
											case ShaderPropertyType.Float:
											case ShaderPropertyType.Range:
												material.SetFloat(text, reader.Read<float>());
												break;
											case ShaderPropertyType.Texture:
												material.SetTexture(text, reader.Read<Texture>());
												break;
											}
										}
										else if (text.EndsWith("_TextureScale"))
										{
											material.SetTextureScale(text.Split(new string[]
											{
												"_TextureScale"
											}, StringSplitOptions.None)[0], reader.Read<Vector2>());
										}
										else if (text.EndsWith("_TextureOffset"))
										{
											material.SetTextureOffset(text.Split(new string[]
											{
												"_TextureOffset"
											}, StringSplitOptions.None)[0], reader.Read<Vector2>());
										}
										reader.Skip();
									}
									else
									{
										material.SetTextureScale("_MainTex", reader.Read<Vector2>());
									}
								}
								else
								{
									material.globalIlluminationFlags = reader.Read<MaterialGlobalIlluminationFlags>();
								}
							}
							else
							{
								foreach (string keyword in reader.Read<string[]>())
								{
									material.EnableKeyword(keyword);
								}
							}
						}
						else
						{
							material.renderQueue = reader.Read<int>(ES3Type_int.Instance);
						}
					}
					else
					{
						material.shader = reader.Read<Shader>(ES3Type_Shader.Instance);
					}
				}
				else
				{
					material.name = reader.Read<string>(ES3Type_string.Instance);
				}
			}
		}

		// Token: 0x040000DF RID: 223
		public static ES3Type Instance;
	}
}
