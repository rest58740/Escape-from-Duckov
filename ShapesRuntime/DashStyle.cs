using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200004D RID: 77
	[Serializable]
	public struct DashStyle
	{
		// Token: 0x06000C6B RID: 3179 RVA: 0x00018B3F File Offset: 0x00016D3F
		private float GetNet(float v, float thickness)
		{
			if (this.space != DashSpace.Relative)
			{
				return v;
			}
			return thickness * v;
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00018B4F File Offset: 0x00016D4F
		internal float GetNetAbsoluteSize(bool dashed, float thickness)
		{
			if (!dashed)
			{
				return 0f;
			}
			return this.GetNet(this.size, thickness);
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00018B67 File Offset: 0x00016D67
		internal float GetNetAbsoluteSpacing(bool dashed, float thickness)
		{
			if (!dashed)
			{
				return 0f;
			}
			return this.GetNet(this.spacing, thickness);
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x00018B7F File Offset: 0x00016D7F
		// (set) Token: 0x06000C6F RID: 3183 RVA: 0x00018B87 File Offset: 0x00016D87
		public float UniformSize
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
				this.spacing = ((this.space == DashSpace.FixedCount) ? 0.5f : this.size);
			}
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00018BB0 File Offset: 0x00016DB0
		public static DashStyle RelativeDashes(DashType type, float size, float spacing, DashSnapping snap = DashSnapping.Off, float offset = 0f, float shapeModifier = 1f)
		{
			return new DashStyle
			{
				space = DashSpace.Relative,
				type = type,
				size = size,
				spacing = spacing,
				snap = snap,
				offset = offset,
				shapeModifier = shapeModifier
			};
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00018C00 File Offset: 0x00016E00
		public static DashStyle FixedDashCount(DashType type, float count, float spacingFraction = 0.5f, DashSnapping snap = DashSnapping.Off, float offset = 0f, float shapeModifier = 1f)
		{
			return new DashStyle
			{
				space = DashSpace.FixedCount,
				type = type,
				size = count,
				spacing = spacingFraction,
				snap = snap,
				offset = offset,
				shapeModifier = shapeModifier
			};
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00018C54 File Offset: 0x00016E54
		public static DashStyle MeterDashes(DashType type, float size, float spacing, DashSnapping snap = DashSnapping.Off, float offset = 0f, float shapeModifier = 1f)
		{
			return new DashStyle
			{
				space = DashSpace.Meters,
				type = type,
				size = size,
				spacing = spacing,
				snap = snap,
				offset = offset,
				shapeModifier = shapeModifier
			};
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x00018CA4 File Offset: 0x00016EA4
		// (set) Token: 0x06000C74 RID: 3188 RVA: 0x00018CAB File Offset: 0x00016EAB
		[Obsolete("Deprecated, please use defaultDashStyle instead (lowercase first letter~)")]
		public static DashStyle DefaultDashStyle
		{
			get
			{
				return DashStyle.defaultDashStyle;
			}
			set
			{
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x00018CAD File Offset: 0x00016EAD
		// (set) Token: 0x06000C76 RID: 3190 RVA: 0x00018CB4 File Offset: 0x00016EB4
		[Obsolete("Deprecated, please use defaultDashStyleRing instead (lowercase first letter~)")]
		public static DashStyle DefaultDashStyleRing
		{
			get
			{
				return DashStyle.defaultDashStyleRing;
			}
			set
			{
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x00018CB6 File Offset: 0x00016EB6
		// (set) Token: 0x06000C78 RID: 3192 RVA: 0x00018CBD File Offset: 0x00016EBD
		[Obsolete("Deprecated, please use defaultDashStyleLine instead (lowercase first letter~)")]
		public static DashStyle DefaultDashStyleLine
		{
			get
			{
				return DashStyle.defaultDashStyleLine;
			}
			set
			{
			}
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00018CC0 File Offset: 0x00016EC0
		[Obsolete("Deprecated, please use <c>DashStyle.RelativeDashes/FixedCountDashes/MeterDashes</c> instead", true)]
		public DashStyle(float size)
		{
			this.type = DashStyle.defaultDashStyle.type;
			this.space = DashStyle.defaultDashStyle.space;
			this.snap = DashStyle.defaultDashStyle.snap;
			this.size = size;
			this.spacing = size;
			this.offset = DashStyle.defaultDashStyle.offset;
			this.shapeModifier = DashStyle.defaultDashStyle.shapeModifier;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00018D2C File Offset: 0x00016F2C
		[Obsolete("Deprecated, please use <c>DashStyle.RelativeDashes/FixedCountDashes/MeterDashes</c> instead", true)]
		public DashStyle(float size, DashType type)
		{
			this.type = type;
			this.space = DashStyle.defaultDashStyle.space;
			this.snap = DashStyle.defaultDashStyle.snap;
			this.size = size;
			this.spacing = size;
			this.offset = DashStyle.defaultDashStyle.offset;
			this.shapeModifier = DashStyle.defaultDashStyle.shapeModifier;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00018D90 File Offset: 0x00016F90
		[Obsolete("Deprecated, please use <c>DashStyle.RelativeDashes/FixedCountDashes/MeterDashes</c> instead", true)]
		public DashStyle(float size, float spacing, DashType type)
		{
			this.type = type;
			this.space = DashStyle.defaultDashStyle.space;
			this.snap = DashStyle.defaultDashStyle.snap;
			this.size = size;
			this.spacing = spacing;
			this.offset = DashStyle.defaultDashStyle.offset;
			this.shapeModifier = DashStyle.defaultDashStyle.shapeModifier;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00018DF4 File Offset: 0x00016FF4
		[Obsolete("Deprecated, please use <c>DashStyle.RelativeDashes/FixedCountDashes/MeterDashes</c> instead", true)]
		public DashStyle(float size, float spacing)
		{
			this.type = DashStyle.defaultDashStyle.type;
			this.space = DashStyle.defaultDashStyle.space;
			this.snap = DashStyle.defaultDashStyle.snap;
			this.size = size;
			this.spacing = spacing;
			this.offset = DashStyle.defaultDashStyle.offset;
			this.shapeModifier = DashStyle.defaultDashStyle.shapeModifier;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00018E60 File Offset: 0x00017060
		[Obsolete("Deprecated, please use <c>DashStyle.RelativeDashes/FixedCountDashes/MeterDashes</c> instead", true)]
		public DashStyle(float size, float spacing, float offset)
		{
			this.type = DashStyle.defaultDashStyle.type;
			this.space = DashStyle.defaultDashStyle.space;
			this.snap = DashStyle.defaultDashStyle.snap;
			this.size = size;
			this.spacing = spacing;
			this.offset = offset;
			this.shapeModifier = DashStyle.defaultDashStyle.shapeModifier;
		}

		// Token: 0x040001CA RID: 458
		public static readonly DashStyle defaultDashStyle = new DashStyle
		{
			type = DashType.Basic,
			space = DashSpace.Relative,
			snap = DashSnapping.Off,
			size = 4f,
			offset = 0f,
			spacing = 4f,
			shapeModifier = 1f
		};

		// Token: 0x040001CB RID: 459
		public static readonly DashStyle defaultDashStyleRing = new DashStyle
		{
			type = DashType.Basic,
			space = DashSpace.FixedCount,
			snap = DashSnapping.Tiling,
			size = 16f,
			offset = 0f,
			spacing = 0.5f,
			shapeModifier = 1f
		};

		// Token: 0x040001CC RID: 460
		public static readonly DashStyle defaultDashStyleLine = new DashStyle
		{
			type = DashType.Basic,
			space = DashSpace.Relative,
			snap = DashSnapping.EndToEnd,
			size = 4f,
			offset = 0f,
			spacing = 4f,
			shapeModifier = 1f
		};

		// Token: 0x040001CD RID: 461
		public DashType type;

		// Token: 0x040001CE RID: 462
		public DashSpace space;

		// Token: 0x040001CF RID: 463
		public DashSnapping snap;

		// Token: 0x040001D0 RID: 464
		public float size;

		// Token: 0x040001D1 RID: 465
		public float spacing;

		// Token: 0x040001D2 RID: 466
		public float offset;

		// Token: 0x040001D3 RID: 467
		[Range(-1f, 1f)]
		public float shapeModifier;
	}
}
