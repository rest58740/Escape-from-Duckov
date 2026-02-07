using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000006 RID: 6
	public static class DOTweenModuleSprite
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002D08 File Offset: 0x00000F08
		public static TweenerCore<Color, Color, ColorOptions> DOColor(this SpriteRenderer target, Color endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002D50 File Offset: 0x00000F50
		public static TweenerCore<Color, Color, ColorOptions> DOFade(this SpriteRenderer target, float endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002D98 File Offset: 0x00000F98
		public static Sequence DOGradientColor(this SpriteRenderer target, Gradient gradient, float duration)
		{
			Sequence sequence = DOTween.Sequence();
			GradientColorKey[] colorKeys = gradient.colorKeys;
			int num = colorKeys.Length;
			for (int i = 0; i < num; i++)
			{
				GradientColorKey gradientColorKey = colorKeys[i];
				if (i == 0 && gradientColorKey.time <= 0f)
				{
					target.color = gradientColorKey.color;
				}
				else
				{
					float duration2 = (i == num - 1) ? (duration - TweenExtensions.Duration(sequence, false)) : (duration * ((i == 0) ? gradientColorKey.time : (gradientColorKey.time - colorKeys[i - 1].time)));
					TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(target.DOColor(gradientColorKey.color, duration2), 1));
				}
			}
			TweenSettingsExtensions.SetTarget<Sequence>(sequence, target);
			return sequence;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002E50 File Offset: 0x00001050
		public static Tweener DOBlendableColor(this SpriteRenderer target, Color endValue, float duration)
		{
			endValue -= target.color;
			Color to = new Color(0f, 0f, 0f, 0f);
			return TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(Extensions.Blendable<Color, Color, ColorOptions>(DOTween.To(() => to, delegate(Color x)
			{
				Color b = x - to;
				to = x;
				target.color += b;
			}, endValue, duration)), target);
		}
	}
}
