using System;
using System.Diagnostics;
using UnityEngine;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000050 RID: 80
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class PreviewFieldAttribute : Attribute
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00002F66 File Offset: 0x00001166
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00002F6E File Offset: 0x0000116E
		public ObjectFieldAlignment Alignment
		{
			get
			{
				return this.alignment;
			}
			set
			{
				this.alignment = value;
				this.alignmentHasValue = true;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00002F7E File Offset: 0x0000117E
		public bool AlignmentHasValue
		{
			get
			{
				return this.alignmentHasValue;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00002F86 File Offset: 0x00001186
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00002F8E File Offset: 0x0000118E
		public string PreviewGetter
		{
			get
			{
				return this.previewGetter;
			}
			set
			{
				this.previewGetter = value;
				this.PreviewGetterHasValue = true;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00002F9E File Offset: 0x0000119E
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00002FA6 File Offset: 0x000011A6
		public bool PreviewGetterHasValue { get; private set; }

		// Token: 0x060000F9 RID: 249 RVA: 0x00002FAF File Offset: 0x000011AF
		public PreviewFieldAttribute()
		{
			this.Height = 0f;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00002FC9 File Offset: 0x000011C9
		public PreviewFieldAttribute(float height)
		{
			this.Height = height;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00002FDF File Offset: 0x000011DF
		public PreviewFieldAttribute(string previewGetter, FilterMode filterMode = FilterMode.Bilinear)
		{
			this.PreviewGetter = previewGetter;
			this.FilterMode = filterMode;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00002FFC File Offset: 0x000011FC
		public PreviewFieldAttribute(string previewGetter, float height, FilterMode filterMode = FilterMode.Bilinear)
		{
			this.PreviewGetter = previewGetter;
			this.Height = height;
			this.FilterMode = filterMode;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00003020 File Offset: 0x00001220
		public PreviewFieldAttribute(float height, ObjectFieldAlignment alignment)
		{
			this.Height = height;
			this.Alignment = alignment;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000303D File Offset: 0x0000123D
		public PreviewFieldAttribute(string previewGetter, ObjectFieldAlignment alignment, FilterMode filterMode = FilterMode.Bilinear)
		{
			this.PreviewGetter = previewGetter;
			this.Alignment = alignment;
			this.FilterMode = filterMode;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00003061 File Offset: 0x00001261
		public PreviewFieldAttribute(string previewGetter, float height, ObjectFieldAlignment alignment, FilterMode filterMode = FilterMode.Bilinear)
		{
			this.PreviewGetter = previewGetter;
			this.Height = height;
			this.Alignment = alignment;
			this.FilterMode = filterMode;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000308D File Offset: 0x0000128D
		public PreviewFieldAttribute(ObjectFieldAlignment alignment)
		{
			this.Alignment = alignment;
		}

		// Token: 0x040000D0 RID: 208
		private ObjectFieldAlignment alignment;

		// Token: 0x040000D1 RID: 209
		private bool alignmentHasValue;

		// Token: 0x040000D2 RID: 210
		private string previewGetter;

		// Token: 0x040000D3 RID: 211
		public float Height;

		// Token: 0x040000D4 RID: 212
		public FilterMode FilterMode = FilterMode.Bilinear;
	}
}
