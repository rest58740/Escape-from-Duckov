using System;
using UnityEngine;

namespace DG.DemiLib
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class DeToggleColors
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002566 File Offset: 0x00000766
		public DeToggleColors.ColorCombination GetColors(ToggleColors offType, ToggleColors onType)
		{
			return new DeToggleColors.ColorCombination(this.GetColor2(offType), this.GetColor2(onType));
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000257C File Offset: 0x0000077C
		private DeToggleColors.Color2 GetColor2(ToggleColors type)
		{
			DeToggleColors.Color2 color = new DeToggleColors.Color2();
			switch (type)
			{
			case ToggleColors.DefaultOn:
				color.bg = this.bgOn;
				color.content = this.contentOn;
				break;
			case ToggleColors.DefaultOff:
				color.bg = this.bgOff;
				color.content = this.contentOff;
				break;
			case ToggleColors.Critical:
				color.bg = this.bgCritical;
				color.content = this.contentCritical;
				break;
			case ToggleColors.Yellow:
				color.bg = this.bgYellow;
				color.content = this.contentYellow;
				break;
			case ToggleColors.Orange:
				color.bg = this.bgOrange;
				color.content = this.contentOrange;
				break;
			case ToggleColors.Blue:
				color.bg = this.bgBlue;
				color.content = this.contentBlue;
				break;
			case ToggleColors.Cyan:
				color.bg = this.bgCyan;
				color.content = this.contentCyan;
				break;
			case ToggleColors.Purple:
				color.bg = this.bgPurple;
				color.content = this.contentPurple;
				break;
			}
			return color;
		}

		// Token: 0x04000022 RID: 34
		public DeSkinColor bgOn = new DeSkinColor(new Color(0.3158468f, 0.875f, 0.1351103f, 1f), new Color(0.2183823f, 0.7279412f, 0.09099264f, 1f));

		// Token: 0x04000023 RID: 35
		public DeSkinColor bgOff = new DeSkinColor(0.75f, 0.4f);

		// Token: 0x04000024 RID: 36
		public DeSkinColor bgCritical = new DeSkinColor(new Color(0.9411765f, 0.2388736f, 0.006920422f, 1f), new Color(1f, 0.2482758f, 0f, 1f));

		// Token: 0x04000025 RID: 37
		public DeSkinColor bgYellow = new DeSkinColor(new Color(0.93f, 0.77f, 0.04f));

		// Token: 0x04000026 RID: 38
		public DeSkinColor bgOrange = new DeSkinColor(new Color(0.98f, 0.44f, 0f));

		// Token: 0x04000027 RID: 39
		public DeSkinColor bgBlue = new DeSkinColor(new Color(0f, 0.4f, 0.91f));

		// Token: 0x04000028 RID: 40
		public DeSkinColor bgCyan = new DeSkinColor(new Color(0f, 0.79f, 1f));

		// Token: 0x04000029 RID: 41
		public DeSkinColor bgPurple = new DeSkinColor(new Color(0.67f, 0.17f, 0.87f));

		// Token: 0x0400002A RID: 42
		public DeSkinColor contentOn = new DeSkinColor(new Color(1f, 0.9686275f, 0.6980392f, 1f), new Color(0.8025267f, 1f, 0.4705882f, 1f));

		// Token: 0x0400002B RID: 43
		public DeSkinColor contentOff = new DeSkinColor(new Color(0.4117647f, 0.3360727f, 0.3360727f, 1f), new Color(0.6470588f, 0.5185986f, 0.5185986f, 1f));

		// Token: 0x0400002C RID: 44
		public DeSkinColor contentCritical = new DeSkinColor(new Color(1f, 0.84f, 0.62f), new Color(1f, 0.84f, 0.62f));

		// Token: 0x0400002D RID: 45
		public DeSkinColor contentYellow = new DeSkinColor(new Color(1f, 1f, 0.64f));

		// Token: 0x0400002E RID: 46
		public DeSkinColor contentOrange = new DeSkinColor(new Color(1f, 0.96f, 0.57f));

		// Token: 0x0400002F RID: 47
		public DeSkinColor contentBlue = new DeSkinColor(new Color(0.35f, 0.96f, 0.94f));

		// Token: 0x04000030 RID: 48
		public DeSkinColor contentCyan = new DeSkinColor(new Color(0.62f, 1f, 0.89f));

		// Token: 0x04000031 RID: 49
		public DeSkinColor contentPurple = new DeSkinColor(new Color(1f, 0.81f, 0.98f));

		// Token: 0x02000011 RID: 17
		public class ColorCombination
		{
			// Token: 0x0600002B RID: 43 RVA: 0x000030B5 File Offset: 0x000012B5
			public ColorCombination(DeToggleColors.Color2 offCombination, DeToggleColors.Color2 onCombination)
			{
				this.offCols = offCombination;
				this.onCols = onCombination;
			}

			// Token: 0x0400003C RID: 60
			public DeToggleColors.Color2 offCols;

			// Token: 0x0400003D RID: 61
			public DeToggleColors.Color2 onCols;
		}

		// Token: 0x02000012 RID: 18
		public class Color2
		{
			// Token: 0x0400003E RID: 62
			public Color bg;

			// Token: 0x0400003F RID: 63
			public Color content;
		}
	}
}
