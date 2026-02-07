using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000046 RID: 70
	public interface IFillable
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000C50 RID: 3152
		// (set) Token: 0x06000C51 RID: 3153
		GradientFill Fill { get; set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000C52 RID: 3154
		// (set) Token: 0x06000C53 RID: 3155
		bool UseFill { get; set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000C54 RID: 3156
		// (set) Token: 0x06000C55 RID: 3157
		FillType FillType { get; set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000C56 RID: 3158
		// (set) Token: 0x06000C57 RID: 3159
		FillSpace FillSpace { get; set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000C58 RID: 3160
		// (set) Token: 0x06000C59 RID: 3161
		Vector3 FillRadialOrigin { get; set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000C5A RID: 3162
		// (set) Token: 0x06000C5B RID: 3163
		float FillRadialRadius { get; set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000C5C RID: 3164
		// (set) Token: 0x06000C5D RID: 3165
		Vector3 FillLinearStart { get; set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000C5E RID: 3166
		// (set) Token: 0x06000C5F RID: 3167
		Vector3 FillLinearEnd { get; set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000C60 RID: 3168
		// (set) Token: 0x06000C61 RID: 3169
		Color FillColorStart { get; set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000C62 RID: 3170
		// (set) Token: 0x06000C63 RID: 3171
		Color FillColorEnd { get; set; }
	}
}
