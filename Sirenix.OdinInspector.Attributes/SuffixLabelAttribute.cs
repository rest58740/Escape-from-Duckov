using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000068 RID: 104
	[AttributeUsage(32767, AllowMultiple = true, Inherited = false)]
	[Conditional("UNITY_EDITOR")]
	public sealed class SuffixLabelAttribute : Attribute
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000383F File Offset: 0x00001A3F
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00003847 File Offset: 0x00001A47
		public SdfIconType Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				this.icon = value;
				this.HasDefinedIcon = true;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00003857 File Offset: 0x00001A57
		// (set) Token: 0x06000162 RID: 354 RVA: 0x0000385F File Offset: 0x00001A5F
		public bool HasDefinedIcon { get; private set; }

		// Token: 0x06000163 RID: 355 RVA: 0x00003868 File Offset: 0x00001A68
		public SuffixLabelAttribute(string label, bool overlay = false)
		{
			this.Label = label;
			this.Overlay = overlay;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000387E File Offset: 0x00001A7E
		public SuffixLabelAttribute(string label, SdfIconType icon, bool overlay = false)
		{
			this.Label = label;
			this.Icon = icon;
			this.Overlay = overlay;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000389B File Offset: 0x00001A9B
		public SuffixLabelAttribute(SdfIconType icon)
		{
			this.Icon = icon;
		}

		// Token: 0x0400010E RID: 270
		public string Label;

		// Token: 0x0400010F RID: 271
		public bool Overlay;

		// Token: 0x04000110 RID: 272
		public string IconColor;

		// Token: 0x04000112 RID: 274
		private SdfIconType icon;
	}
}
