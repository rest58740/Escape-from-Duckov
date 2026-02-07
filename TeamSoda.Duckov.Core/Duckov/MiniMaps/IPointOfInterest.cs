using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.MiniMaps
{
	// Token: 0x02000284 RID: 644
	public interface IPointOfInterest
	{
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x0004CB8B File Offset: 0x0004AD8B
		int OverrideScene
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001488 RID: 5256 RVA: 0x0004CB8E File Offset: 0x0004AD8E
		Sprite Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001489 RID: 5257 RVA: 0x0004CB91 File Offset: 0x0004AD91
		Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x0004CB98 File Offset: 0x0004AD98
		string DisplayName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x0600148B RID: 5259 RVA: 0x0004CB9B File Offset: 0x0004AD9B
		Color ShadowColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x0600148C RID: 5260 RVA: 0x0004CBA2 File Offset: 0x0004ADA2
		float ShadowDistance
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x0600148D RID: 5261 RVA: 0x0004CBA9 File Offset: 0x0004ADA9
		bool IsArea
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x0004CBAC File Offset: 0x0004ADAC
		float AreaRadius
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x0600148F RID: 5263 RVA: 0x0004CBB3 File Offset: 0x0004ADB3
		bool HideIcon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001490 RID: 5264 RVA: 0x0004CBB6 File Offset: 0x0004ADB6
		float ScaleFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0004CBBD File Offset: 0x0004ADBD
		void NotifyClicked(PointerEventData eventData)
		{
		}
	}
}
