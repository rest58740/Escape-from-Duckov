using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000075 RID: 117
	internal class ShapesMaterials
	{
		// Token: 0x170001D8 RID: 472
		public Material this[ShapesBlendMode type]
		{
			get
			{
				return this.materials[(int)type];
			}
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00019FAC File Offset: 0x000181AC
		public ShapesMaterials(string shaderName, params string[] keywords)
		{
			int num = Enum.GetNames(typeof(ShapesBlendMode)).Length;
			this.materials = new Material[num];
			for (int i = 0; i < num; i++)
			{
				Material[] array = this.materials;
				int num2 = i;
				ShapesBlendMode shapesBlendMode = (ShapesBlendMode)i;
				array[num2] = ShapesMaterials.InitMaterial(shaderName, shapesBlendMode.ToString(), keywords);
			}
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0001A008 File Offset: 0x00018208
		public static string GetMaterialName(string shaderName, string blendModeSuffix, params string[] keywords)
		{
			string text = "";
			if (keywords != null && keywords.Length != 0)
			{
				text = " (" + string.Join(")(", keywords) + ")";
			}
			return shaderName + " " + blendModeSuffix + text;
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0001A04A File Offset: 0x0001824A
		public static void ApplyDefaultGlobalProperties(Material mat)
		{
			mat.SetInt_Shapes(ShapesMaterialUtils.propZTest, 4);
			mat.SetFloat(ShapesMaterialUtils.propZOffsetFactor, 0f);
			mat.SetInt_Shapes(ShapesMaterialUtils.propZOffsetUnits, 0);
			mat.SetInt_Shapes(ShapesMaterialUtils.propColorMask, 15);
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0001A084 File Offset: 0x00018284
		private static Material CreateShapesMaterial(Shader shader, HideFlags hideFlags, params string[] keywords)
		{
			Material material = new Material(shader)
			{
				hideFlags = hideFlags,
				enableInstancing = true
			};
			if (keywords != null)
			{
				foreach (string keyword in keywords)
				{
					material.EnableKeyword(keyword);
				}
			}
			ShapesMaterials.ApplyDefaultGlobalProperties(material);
			return material;
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0001A0CC File Offset: 0x000182CC
		private static Material InitMaterial(string shaderName, string blendModeSuffix, params string[] keywords)
		{
			shaderName = "Shapes/" + shaderName + " " + blendModeSuffix;
			Shader shader = Shader.Find(shaderName);
			if (shader == null)
			{
				Debug.LogError("Could not find shader " + shaderName);
				return null;
			}
			return ShapesMaterials.CreateShapesMaterial(shader, HideFlags.HideAndDontSave, keywords);
		}

		// Token: 0x0400029C RID: 668
		private const bool USE_INSTANCING = true;

		// Token: 0x0400029D RID: 669
		public const string SHAPES_SHADER_PATH_PREFIX = "Shapes/";

		// Token: 0x0400029E RID: 670
		private readonly Material[] materials;
	}
}
