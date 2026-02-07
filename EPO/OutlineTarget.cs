using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace EPOOutline
{
	// Token: 0x02000017 RID: 23
	[Serializable]
	public class OutlineTarget
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00005CEF File Offset: 0x00003EEF
		public Renderer Renderer
		{
			get
			{
				return this.renderer;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00005CF7 File Offset: 0x00003EF7
		internal bool UsesCutout
		{
			get
			{
				return !string.IsNullOrEmpty(this.cutoutTextureName);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00005D08 File Offset: 0x00003F08
		internal Material SharedMaterial
		{
			get
			{
				if (this.renderer == null)
				{
					return null;
				}
				OutlineTarget.TempSharedMaterials.Clear();
				this.renderer.GetSharedMaterials(OutlineTarget.TempSharedMaterials);
				if (OutlineTarget.TempSharedMaterials.Count != 0)
				{
					return OutlineTarget.TempSharedMaterials[this.ShiftedSubmeshIndex % OutlineTarget.TempSharedMaterials.Count];
				}
				return null;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00005D68 File Offset: 0x00003F68
		internal Texture CutoutTexture
		{
			get
			{
				Material sharedMaterial = this.SharedMaterial;
				if (!(sharedMaterial == null))
				{
					return sharedMaterial.GetTexture(this.CutoutTextureId);
				}
				return null;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00005D94 File Offset: 0x00003F94
		internal bool IsValidForCutout
		{
			get
			{
				Material sharedMaterial = this.SharedMaterial;
				return this.UsesCutout && sharedMaterial != null && sharedMaterial.HasProperty(this.CutoutTextureId) && this.CutoutTexture != null;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00005DD5 File Offset: 0x00003FD5
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00005DDD File Offset: 0x00003FDD
		public int CutoutTextureIndex
		{
			get
			{
				return this.cutoutTextureIndex;
			}
			set
			{
				this.cutoutTextureIndex = value;
				if (this.cutoutTextureIndex >= 0)
				{
					return;
				}
				Debug.LogError("Trying to set cutout texture index less than zero");
				this.cutoutTextureIndex = 0;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00005E01 File Offset: 0x00004001
		internal int ShiftedSubmeshIndex
		{
			get
			{
				return this.SubmeshIndex;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00005E09 File Offset: 0x00004009
		internal int CutoutTextureId
		{
			get
			{
				if (this.cutoutTextureId == null)
				{
					this.cutoutTextureId = new int?(Shader.PropertyToID(this.cutoutTextureName));
				}
				return this.cutoutTextureId.Value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00005E39 File Offset: 0x00004039
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00005E41 File Offset: 0x00004041
		public string CutoutTextureName
		{
			get
			{
				return this.cutoutTextureName;
			}
			set
			{
				this.cutoutTextureName = value;
				this.cutoutTextureId = null;
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005E56 File Offset: 0x00004056
		public OutlineTarget()
		{
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005E88 File Offset: 0x00004088
		public OutlineTarget(Renderer renderer, int submesh = 0)
		{
			this.SubmeshIndex = submesh;
			this.renderer = renderer;
			this.CutoutThreshold = 0.5f;
			this.cutoutTextureId = null;
			this.cutoutTextureName = string.Empty;
			this.CullMode = ((renderer is SpriteRenderer) ? CullMode.Off : CullMode.Back);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005F04 File Offset: 0x00004104
		public OutlineTarget(Renderer renderer, string cutoutTextureName, float cutoutThreshold = 0.5f)
		{
			this.SubmeshIndex = 0;
			this.renderer = renderer;
			this.cutoutTextureId = new int?(Shader.PropertyToID(cutoutTextureName));
			this.CutoutThreshold = cutoutThreshold;
			this.cutoutTextureName = cutoutTextureName;
			this.CullMode = ((renderer is SpriteRenderer) ? CullMode.Off : CullMode.Back);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005F80 File Offset: 0x00004180
		public OutlineTarget(Renderer renderer, int submeshIndex, string cutoutTextureName, float cutoutThreshold = 0.5f)
		{
			this.SubmeshIndex = submeshIndex;
			this.renderer = renderer;
			this.cutoutTextureId = new int?(Shader.PropertyToID(cutoutTextureName));
			this.CutoutThreshold = cutoutThreshold;
			this.cutoutTextureName = cutoutTextureName;
			this.CullMode = ((renderer is SpriteRenderer) ? CullMode.Off : CullMode.Back);
		}

		// Token: 0x040000A3 RID: 163
		private static List<Material> TempSharedMaterials = new List<Material>();

		// Token: 0x040000A4 RID: 164
		internal bool IsVisible;

		// Token: 0x040000A5 RID: 165
		[SerializeField]
		public ColorMask CutoutMask = ColorMask.A;

		// Token: 0x040000A6 RID: 166
		[SerializeField]
		internal Renderer renderer;

		// Token: 0x040000A7 RID: 167
		[SerializeField]
		public int SubmeshIndex;

		// Token: 0x040000A8 RID: 168
		[SerializeField]
		public BoundsMode BoundsMode;

		// Token: 0x040000A9 RID: 169
		[SerializeField]
		public Bounds Bounds = new Bounds(Vector3.zero, Vector3.one);

		// Token: 0x040000AA RID: 170
		[SerializeField]
		[Range(0f, 1f)]
		public float CutoutThreshold = 0.5f;

		// Token: 0x040000AB RID: 171
		[SerializeField]
		public CullMode CullMode;

		// Token: 0x040000AC RID: 172
		[SerializeField]
		private string cutoutTextureName;

		// Token: 0x040000AD RID: 173
		[SerializeField]
		private int cutoutTextureIndex;

		// Token: 0x040000AE RID: 174
		private int? cutoutTextureId;
	}
}
