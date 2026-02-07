using System;
using UnityEngine;
using UnityEngine.UI;

namespace LeTai.Asset.TranslucentImage.Demo
{
	// Token: 0x02000005 RID: 5
	public class ColorSchemeManager : MonoBehaviour
	{
		// Token: 0x06000011 RID: 17 RVA: 0x0000223A File Offset: 0x0000043A
		private void Start()
		{
			this.cam = Camera.main;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002247 File Offset: 0x00000447
		public void SetLightScheme(bool on)
		{
			this.SetColorScheme(on ? ColorSchemeManager.DemoColorScheme.Light : ColorSchemeManager.DemoColorScheme.Dark);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002258 File Offset: 0x00000458
		public void SetColorScheme(ColorSchemeManager.DemoColorScheme scheme)
		{
			Color backgroundColor;
			Color color;
			Color color2;
			if (scheme != ColorSchemeManager.DemoColorScheme.Light)
			{
				if (scheme != ColorSchemeManager.DemoColorScheme.Dark)
				{
					throw new ArgumentOutOfRangeException("scheme", scheme, null);
				}
				backgroundColor = this.darkBackgroudColor;
				color = this.darkForegroudColor;
				color2 = this.darkTextColor;
			}
			else
			{
				backgroundColor = this.lightBackgroudColor;
				color = this.lightForegroudColor;
				color2 = this.lightTextColor;
			}
			this.cam.backgroundColor = backgroundColor;
			Graphic[] array = this.foregroudGraphic;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].color = color;
			}
			Text[] array2 = this.texts;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].color = color2;
			}
		}

		// Token: 0x04000008 RID: 8
		public Color lightBackgroudColor = Color.white;

		// Token: 0x04000009 RID: 9
		public Color lightForegroudColor = Color.white;

		// Token: 0x0400000A RID: 10
		public Color lightTextColor = Color.white;

		// Token: 0x0400000B RID: 11
		public Color darkBackgroudColor = Color.black;

		// Token: 0x0400000C RID: 12
		public Color darkForegroudColor = Color.black;

		// Token: 0x0400000D RID: 13
		public Color darkTextColor = Color.black;

		// Token: 0x0400000E RID: 14
		public Graphic[] foregroudGraphic;

		// Token: 0x0400000F RID: 15
		public Text[] texts;

		// Token: 0x04000010 RID: 16
		private Camera cam;

		// Token: 0x02000013 RID: 19
		public enum DemoColorScheme
		{
			// Token: 0x04000037 RID: 55
			Light,
			// Token: 0x04000038 RID: 56
			Dark
		}
	}
}
