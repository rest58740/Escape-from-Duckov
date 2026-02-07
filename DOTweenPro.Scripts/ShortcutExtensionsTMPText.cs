using System;
using System.Globalization;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000007 RID: 7
	public static class ShortcutExtensionsTMPText
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00003794 File Offset: 0x00001994
		public static TweenerCore<Color, Color, ColorOptions> DOColor(this TMP_Text target, Color endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000037DC File Offset: 0x000019DC
		public static TweenerCore<Color, Color, ColorOptions> DOFaceColor(this TMP_Text target, Color32 endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.faceColor, delegate(Color x)
			{
				target.faceColor = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003828 File Offset: 0x00001A28
		public static TweenerCore<Color, Color, ColorOptions> DOOutlineColor(this TMP_Text target, Color32 endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.outlineColor, delegate(Color x)
			{
				target.outlineColor = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003873 File Offset: 0x00001A73
		public static TweenerCore<Color, Color, ColorOptions> DOGlowColor(this TMP_Text target, Color endValue, float duration, bool useSharedMaterial = false)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = useSharedMaterial ? ShortcutExtensions.DOColor(target.fontSharedMaterial, endValue, "_GlowColor", duration) : ShortcutExtensions.DOColor(target.fontMaterial, endValue, "_GlowColor", duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000038A8 File Offset: 0x00001AA8
		public static TweenerCore<Color, Color, ColorOptions> DOFade(this TMP_Text target, float endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000038F0 File Offset: 0x00001AF0
		public static TweenerCore<Color, Color, ColorOptions> DOFaceFade(this TMP_Text target, float endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.faceColor, delegate(Color x)
			{
				target.faceColor = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003938 File Offset: 0x00001B38
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(this TMP_Text target, float endValue, float duration)
		{
			Transform trans = target.transform;
			Vector3 vector = new Vector3(endValue, endValue, endValue);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => trans.localScale, delegate(Vector3 x)
			{
				trans.localScale = x;
			}, vector, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Vector3, VectorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003988 File Offset: 0x00001B88
		public static TweenerCore<int, int, NoOptions> DOCounter(this TMP_Text target, int fromValue, int endValue, float duration, bool addThousandsSeparator = true, CultureInfo culture = null)
		{
			CultureInfo cInfo = (!addThousandsSeparator) ? null : (culture ?? CultureInfo.InvariantCulture);
			TweenerCore<int, int, NoOptions> tweenerCore = DOTween.To(() => fromValue, delegate(int x)
			{
				fromValue = x;
				target.text = (addThousandsSeparator ? fromValue.ToString("N0", cInfo) : fromValue.ToString());
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<int, int, NoOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000039FC File Offset: 0x00001BFC
		public static TweenerCore<float, float, FloatOptions> DOFontSize(this TMP_Text target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.fontSize, delegate(float x)
			{
				target.fontSize = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<float, float, FloatOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003A44 File Offset: 0x00001C44
		public static TweenerCore<int, int, NoOptions> DOMaxVisibleCharacters(this TMP_Text target, int endValue, float duration)
		{
			TweenerCore<int, int, NoOptions> tweenerCore = DOTween.To(() => target.maxVisibleCharacters, delegate(int x)
			{
				target.maxVisibleCharacters = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<int, int, NoOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003A8C File Offset: 0x00001C8C
		public static TweenerCore<string, string, StringOptions> DOText(this TMP_Text target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = 0, string scrambleChars = null)
		{
			TweenerCore<string, string, StringOptions> tweenerCore = DOTween.To(() => target.text, delegate(string x)
			{
				target.text = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, richTextEnabled, scrambleMode, scrambleChars), target);
			return tweenerCore;
		}
	}
}
