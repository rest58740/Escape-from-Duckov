using System;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

namespace ES3Types
{
	// Token: 0x02000062 RID: 98
	[Preserve]
	[ES3Properties(new string[]
	{
		"sprite",
		"overrideSprite",
		"type",
		"preserveAspect",
		"fillCenter",
		"fillMethod",
		"fillAmount",
		"fillClockwise",
		"fillOrigin",
		"alphaHitTestMinimumThreshold",
		"useSpriteMesh",
		"pixelsPerUnitMultiplier",
		"material",
		"onCullStateChanged",
		"maskable",
		"color",
		"raycastTarget",
		"useLegacyMeshGeneration",
		"useGUILayout",
		"enabled",
		"hideFlags"
	})]
	public class ES3Type_Image : ES3ComponentType
	{
		// Token: 0x060002E0 RID: 736 RVA: 0x0000B898 File Offset: 0x00009A98
		public ES3Type_Image() : base(typeof(Image))
		{
			ES3Type_Image.Instance = this;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000B8B0 File Offset: 0x00009AB0
		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			Image image = (Image)obj;
			writer.WritePropertyByRef("sprite", image.sprite);
			writer.WriteProperty("type", image.type);
			writer.WriteProperty("preserveAspect", image.preserveAspect, ES3Type_bool.Instance);
			writer.WriteProperty("fillCenter", image.fillCenter, ES3Type_bool.Instance);
			writer.WriteProperty("fillMethod", image.fillMethod);
			writer.WriteProperty("fillAmount", image.fillAmount, ES3Type_float.Instance);
			writer.WriteProperty("fillClockwise", image.fillClockwise, ES3Type_bool.Instance);
			writer.WriteProperty("fillOrigin", image.fillOrigin, ES3Type_int.Instance);
			writer.WriteProperty("useSpriteMesh", image.useSpriteMesh, ES3Type_bool.Instance);
			if (image.material.name.Contains("Default"))
			{
				writer.WriteProperty("material", null);
			}
			else
			{
				writer.WriteProperty("material", image.material);
			}
			writer.WriteProperty("onCullStateChanged", image.onCullStateChanged);
			writer.WriteProperty("maskable", image.maskable, ES3Type_bool.Instance);
			writer.WriteProperty("color", image.color, ES3Type_Color.Instance);
			writer.WriteProperty("raycastTarget", image.raycastTarget, ES3Type_bool.Instance);
			writer.WritePrivateProperty("useLegacyMeshGeneration", image);
			writer.WriteProperty("useGUILayout", image.useGUILayout, ES3Type_bool.Instance);
			writer.WriteProperty("enabled", image.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("hideFlags", image.hideFlags, ES3Type_enum.Instance);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000BA98 File Offset: 0x00009C98
		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			Image image = (Image)obj;
			foreach (object obj2 in reader.Properties)
			{
				string text = (string)obj2;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 1656530132U)
				{
					if (num <= 1031692888U)
					{
						if (num <= 465631631U)
						{
							if (num != 49525662U)
							{
								if (num == 465631631U)
								{
									if (text == "onCullStateChanged")
									{
										image.onCullStateChanged = reader.Read<MaskableGraphic.CullStateChangedEvent>();
										continue;
									}
								}
							}
							else if (text == "enabled")
							{
								image.enabled = reader.Read<bool>(ES3Type_bool.Instance);
								continue;
							}
						}
						else if (num != 886554752U)
						{
							if (num == 1031692888U)
							{
								if (text == "color")
								{
									image.color = reader.Read<Color>(ES3Type_Color.Instance);
									continue;
								}
							}
						}
						else if (text == "useLegacyMeshGeneration")
						{
							reader.SetPrivateProperty("useLegacyMeshGeneration", reader.Read<bool>(), image);
							continue;
						}
					}
					else if (num <= 1361572173U)
					{
						if (num != 1168076458U)
						{
							if (num == 1361572173U)
							{
								if (text == "type")
								{
									image.type = reader.Read<Image.Type>();
									continue;
								}
							}
						}
						else if (text == "fillOrigin")
						{
							image.fillOrigin = reader.Read<int>(ES3Type_int.Instance);
							continue;
						}
					}
					else if (num != 1487442558U)
					{
						if (num != 1570416393U)
						{
							if (num == 1656530132U)
							{
								if (text == "fillAmount")
								{
									image.fillAmount = reader.Read<float>(ES3Type_float.Instance);
									continue;
								}
							}
						}
						else if (text == "raycastTarget")
						{
							image.raycastTarget = reader.Read<bool>(ES3Type_bool.Instance);
							continue;
						}
					}
					else if (text == "fillClockwise")
					{
						image.fillClockwise = reader.Read<bool>(ES3Type_bool.Instance);
						continue;
					}
				}
				else if (num <= 2179094556U)
				{
					if (num <= 2131725860U)
					{
						if (num != 1784211997U)
						{
							if (num == 2131725860U)
							{
								if (text == "useSpriteMesh")
								{
									image.useSpriteMesh = reader.Read<bool>(ES3Type_bool.Instance);
									continue;
								}
							}
						}
						else if (text == "useGUILayout")
						{
							image.useGUILayout = reader.Read<bool>(ES3Type_bool.Instance);
							continue;
						}
					}
					else if (num != 2154374041U)
					{
						if (num == 2179094556U)
						{
							if (text == "sprite")
							{
								image.sprite = reader.Read<Sprite>(ES3Type_Sprite.Instance);
								continue;
							}
						}
					}
					else if (text == "maskable")
					{
						image.maskable = reader.Read<bool>(ES3Type_bool.Instance);
						continue;
					}
				}
				else if (num <= 2618981797U)
				{
					if (num != 2572378073U)
					{
						if (num == 2618981797U)
						{
							if (text == "preserveAspect")
							{
								image.preserveAspect = reader.Read<bool>(ES3Type_bool.Instance);
								continue;
							}
						}
					}
					else if (text == "fillMethod")
					{
						image.fillMethod = reader.Read<Image.FillMethod>();
						continue;
					}
				}
				else if (num != 3168709905U)
				{
					if (num != 3538210912U)
					{
						if (num == 3944566772U)
						{
							if (text == "hideFlags")
							{
								image.hideFlags = reader.Read<HideFlags>(ES3Type_enum.Instance);
								continue;
							}
						}
					}
					else if (text == "material")
					{
						image.material = reader.Read<Material>(ES3Type_Material.Instance);
						continue;
					}
				}
				else if (text == "fillCenter")
				{
					image.fillCenter = reader.Read<bool>(ES3Type_bool.Instance);
					continue;
				}
				reader.Skip();
			}
		}

		// Token: 0x040000A1 RID: 161
		public static ES3Type Instance;
	}
}
