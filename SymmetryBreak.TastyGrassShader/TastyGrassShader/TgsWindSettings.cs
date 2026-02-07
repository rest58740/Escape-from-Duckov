using System;
using UnityEngine;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x02000023 RID: 35
	[CreateAssetMenu(menuName = "Symmetry Break Studio/Tasty Grass Shader/Wind Settings")]
	public class TgsWindSettings : ScriptableObject
	{
		// Token: 0x06000099 RID: 153 RVA: 0x000065F2 File Offset: 0x000047F2
		public void ApplyToMaterialPropertyBlock(MaterialPropertyBlock materialPropertyBlock)
		{
			materialPropertyBlock.SetVector(TgsWindSettings.WindParams, new Vector4(this.direction * 0.017453292f, this.strength, this.patchSize, this.speed));
		}

		// Token: 0x04000144 RID: 324
		private static readonly int WindDirection = Shader.PropertyToID("_WindDirection");

		// Token: 0x04000145 RID: 325
		private static readonly int WindStrength = Shader.PropertyToID("_WindStrength");

		// Token: 0x04000146 RID: 326
		private static readonly int WindPatchSize = Shader.PropertyToID("_WindPatchSize");

		// Token: 0x04000147 RID: 327
		private static readonly int WindSpeed = Shader.PropertyToID("_WindSpeed");

		// Token: 0x04000148 RID: 328
		private static readonly int WindParams = Shader.PropertyToID("_WindParams");

		// Token: 0x04000149 RID: 329
		[Tooltip("The direction of the wind in degrees.")]
		[Range(0f, 360f)]
		public float direction;

		// Token: 0x0400014A RID: 330
		[Tooltip("The strength of the wind.")]
		[Range(0f, 20f)]
		public float strength = 0.5f;

		// Token: 0x0400014B RID: 331
		[Tooltip("The size of a wind patch. Smaller values may create more believable settings.")]
		[Range(0f, 0.5f)]
		public float patchSize = 0.05f;

		// Token: 0x0400014C RID: 332
		[Tooltip("The speed of how fast the wind patches move.")]
		[Range(0f, 100f)]
		public float speed = 20f;

		// Token: 0x02000024 RID: 36
		public struct TgsWindSettingsGPU
		{
			// Token: 0x0600009C RID: 156 RVA: 0x000066A4 File Offset: 0x000048A4
			public TgsWindSettingsGPU(TgsWindSettings windSettings)
			{
				this.direction = windSettings.direction;
				this.strength = windSettings.strength;
				this.patchSize = windSettings.patchSize;
				this.speed = windSettings.speed;
			}

			// Token: 0x0400014D RID: 333
			public float direction;

			// Token: 0x0400014E RID: 334
			public float strength;

			// Token: 0x0400014F RID: 335
			public float patchSize;

			// Token: 0x04000150 RID: 336
			public float speed;
		}
	}
}
