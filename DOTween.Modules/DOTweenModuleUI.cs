using System;
using System.Globalization;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace DG.Tweening
{
	// Token: 0x02000007 RID: 7
	public static class DOTweenModuleUI
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002ECC File Offset: 0x000010CC
		public static TweenerCore<float, float, FloatOptions> DOFade(this CanvasGroup target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.alpha, delegate(float x)
			{
				target.alpha = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<float, float, FloatOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002F14 File Offset: 0x00001114
		public static TweenerCore<Color, Color, ColorOptions> DOColor(this Graphic target, Color endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002F5C File Offset: 0x0000115C
		public static TweenerCore<Color, Color, ColorOptions> DOFade(this Graphic target, float endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002FA4 File Offset: 0x000011A4
		public static TweenerCore<Color, Color, ColorOptions> DOColor(this Image target, Color endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002FEC File Offset: 0x000011EC
		public static TweenerCore<Color, Color, ColorOptions> DOFade(this Image target, float endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003034 File Offset: 0x00001234
		public static TweenerCore<float, float, FloatOptions> DOFillAmount(this Image target, float endValue, float duration)
		{
			if (endValue > 1f)
			{
				endValue = 1f;
			}
			else if (endValue < 0f)
			{
				endValue = 0f;
			}
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.fillAmount, delegate(float x)
			{
				target.fillAmount = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<float, float, FloatOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000309C File Offset: 0x0000129C
		public static Sequence DOGradientColor(this Image target, Gradient gradient, float duration)
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

		// Token: 0x06000031 RID: 49 RVA: 0x00003154 File Offset: 0x00001354
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOFlexibleSize(this LayoutElement target, Vector2 endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => new Vector2(target.flexibleWidth, target.flexibleHeight), delegate(Vector2 x)
			{
				target.flexibleWidth = x.x;
				target.flexibleHeight = x.y;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), target);
			return tweenerCore;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000031A0 File Offset: 0x000013A0
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOMinSize(this LayoutElement target, Vector2 endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => new Vector2(target.minWidth, target.minHeight), delegate(Vector2 x)
			{
				target.minWidth = x.x;
				target.minHeight = x.y;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), target);
			return tweenerCore;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000031EC File Offset: 0x000013EC
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOPreferredSize(this LayoutElement target, Vector2 endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => new Vector2(target.preferredWidth, target.preferredHeight), delegate(Vector2 x)
			{
				target.preferredWidth = x.x;
				target.preferredHeight = x.y;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), target);
			return tweenerCore;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003238 File Offset: 0x00001438
		public static TweenerCore<Color, Color, ColorOptions> DOColor(this Outline target, Color endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.effectColor, delegate(Color x)
			{
				target.effectColor = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003280 File Offset: 0x00001480
		public static TweenerCore<Color, Color, ColorOptions> DOFade(this Outline target, float endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.effectColor, delegate(Color x)
			{
				target.effectColor = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000032C8 File Offset: 0x000014C8
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOScale(this Outline target, Vector2 endValue, float duration)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.effectDistance, delegate(Vector2 x)
			{
				target.effectDistance = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Vector2, Vector2, VectorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003310 File Offset: 0x00001510
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorPos(this RectTransform target, Vector2 endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.anchoredPosition, delegate(Vector2 x)
			{
				target.anchoredPosition = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), target);
			return tweenerCore;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000335C File Offset: 0x0000155C
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorPosX(this RectTransform target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.anchoredPosition, delegate(Vector2 x)
			{
				target.anchoredPosition = x;
			}, new Vector2(endValue, 0f), duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, 2, snapping), target);
			return tweenerCore;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000033B4 File Offset: 0x000015B4
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorPosY(this RectTransform target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.anchoredPosition, delegate(Vector2 x)
			{
				target.anchoredPosition = x;
			}, new Vector2(0f, endValue), duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, 4, snapping), target);
			return tweenerCore;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000340C File Offset: 0x0000160C
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOAnchorPos3D(this RectTransform target, Vector3 endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.anchoredPosition3D, delegate(Vector3 x)
			{
				target.anchoredPosition3D = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), target);
			return tweenerCore;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003458 File Offset: 0x00001658
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOAnchorPos3DX(this RectTransform target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.anchoredPosition3D, delegate(Vector3 x)
			{
				target.anchoredPosition3D = x;
			}, new Vector3(endValue, 0f, 0f), duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, 2, snapping), target);
			return tweenerCore;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000034B4 File Offset: 0x000016B4
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOAnchorPos3DY(this RectTransform target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.anchoredPosition3D, delegate(Vector3 x)
			{
				target.anchoredPosition3D = x;
			}, new Vector3(0f, endValue, 0f), duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, 4, snapping), target);
			return tweenerCore;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003510 File Offset: 0x00001710
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOAnchorPos3DZ(this RectTransform target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.anchoredPosition3D, delegate(Vector3 x)
			{
				target.anchoredPosition3D = x;
			}, new Vector3(0f, 0f, endValue), duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, 8, snapping), target);
			return tweenerCore;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000356C File Offset: 0x0000176C
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorMax(this RectTransform target, Vector2 endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.anchorMax, delegate(Vector2 x)
			{
				target.anchorMax = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), target);
			return tweenerCore;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000035B8 File Offset: 0x000017B8
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorMin(this RectTransform target, Vector2 endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.anchorMin, delegate(Vector2 x)
			{
				target.anchorMin = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), target);
			return tweenerCore;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003604 File Offset: 0x00001804
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOPivot(this RectTransform target, Vector2 endValue, float duration)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.pivot, delegate(Vector2 x)
			{
				target.pivot = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Vector2, Vector2, VectorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000364C File Offset: 0x0000184C
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOPivotX(this RectTransform target, float endValue, float duration)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.pivot, delegate(Vector2 x)
			{
				target.pivot = x;
			}, new Vector2(endValue, 0f), duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, 2, false), target);
			return tweenerCore;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000036A4 File Offset: 0x000018A4
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOPivotY(this RectTransform target, float endValue, float duration)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.pivot, delegate(Vector2 x)
			{
				target.pivot = x;
			}, new Vector2(0f, endValue), duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, 4, false), target);
			return tweenerCore;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000036FC File Offset: 0x000018FC
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOSizeDelta(this RectTransform target, Vector2 endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.sizeDelta, delegate(Vector2 x)
			{
				target.sizeDelta = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), target);
			return tweenerCore;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003748 File Offset: 0x00001948
		public static Tweener DOPunchAnchorPos(this RectTransform target, Vector2 punch, float duration, int vibrato = 10, float elasticity = 1f, bool snapping = false)
		{
			return TweenSettingsExtensions.SetOptions(TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>(DOTween.Punch(() => target.anchoredPosition, delegate(Vector3 x)
			{
				target.anchoredPosition = x;
			}, punch, duration, vibrato, elasticity), target), snapping);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000379C File Offset: 0x0000199C
		public static Tweener DOShakeAnchorPos(this RectTransform target, float duration, float strength = 100f, int vibrato = 10, float randomness = 90f, bool snapping = false, bool fadeOut = true)
		{
			return TweenSettingsExtensions.SetOptions(Extensions.SetSpecialStartupMode<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>(TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>(DOTween.Shake(() => target.anchoredPosition, delegate(Vector3 x)
			{
				target.anchoredPosition = x;
			}, duration, strength, vibrato, randomness, true, fadeOut), target), 2), snapping);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000037F4 File Offset: 0x000019F4
		public static Tweener DOShakeAnchorPos(this RectTransform target, float duration, Vector2 strength, int vibrato = 10, float randomness = 90f, bool snapping = false, bool fadeOut = true)
		{
			return TweenSettingsExtensions.SetOptions(Extensions.SetSpecialStartupMode<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>(TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>(DOTween.Shake(() => target.anchoredPosition, delegate(Vector3 x)
			{
				target.anchoredPosition = x;
			}, duration, strength, vibrato, randomness, fadeOut), target), 2), snapping);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003850 File Offset: 0x00001A50
		public static Sequence DOJumpAnchorPos(this RectTransform target, Vector2 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
		{
			if (numJumps < 1)
			{
				numJumps = 1;
			}
			float startPosY = 0f;
			float offsetY = -1f;
			bool offsetYSet = false;
			Sequence s = DOTween.Sequence();
			Tween tween = TweenSettingsExtensions.OnStart<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(TweenSettingsExtensions.SetRelative<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => target.anchoredPosition, delegate(Vector2 x)
			{
				target.anchoredPosition = x;
			}, new Vector2(0f, jumpPower), duration / (float)(numJumps * 2)), 4, snapping), 6)), numJumps * 2, 1), delegate()
			{
				startPosY = target.anchoredPosition.y;
			});
			TweenSettingsExtensions.SetEase<Sequence>(TweenSettingsExtensions.SetTarget<Sequence>(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(s, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => target.anchoredPosition, delegate(Vector2 x)
			{
				target.anchoredPosition = x;
			}, new Vector2(endValue.x, 0f), duration), 2, snapping), 1)), tween), target), DOTween.defaultEaseType);
			TweenSettingsExtensions.OnUpdate<Sequence>(s, delegate()
			{
				if (!offsetYSet)
				{
					offsetYSet = true;
					offsetY = (s.isRelative ? endValue.y : (endValue.y - startPosY));
				}
				Vector2 anchoredPosition = target.anchoredPosition;
				anchoredPosition.y += DOVirtual.EasedValue(0f, offsetY, TweenExtensions.ElapsedDirectionalPercentage(s), 6);
				target.anchoredPosition = anchoredPosition;
			});
			return s;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003984 File Offset: 0x00001B84
		public static Tweener DONormalizedPos(this ScrollRect target, Vector2 endValue, float duration, bool snapping = false)
		{
			return TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => new Vector2(target.horizontalNormalizedPosition, target.verticalNormalizedPosition), delegate(Vector2 x)
			{
				target.horizontalNormalizedPosition = x.x;
				target.verticalNormalizedPosition = x.y;
			}, endValue, duration), snapping), target);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000039D0 File Offset: 0x00001BD0
		public static Tweener DOHorizontalNormalizedPos(this ScrollRect target, float endValue, float duration, bool snapping = false)
		{
			return TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => target.horizontalNormalizedPosition, delegate(float x)
			{
				target.horizontalNormalizedPosition = x;
			}, endValue, duration), snapping), target);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003A1C File Offset: 0x00001C1C
		public static Tweener DOVerticalNormalizedPos(this ScrollRect target, float endValue, float duration, bool snapping = false)
		{
			return TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => target.verticalNormalizedPosition, delegate(float x)
			{
				target.verticalNormalizedPosition = x;
			}, endValue, duration), snapping), target);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003A68 File Offset: 0x00001C68
		public static TweenerCore<float, float, FloatOptions> DOValue(this Slider target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.value, delegate(float x)
			{
				target.value = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), target);
			return tweenerCore;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003AB4 File Offset: 0x00001CB4
		public static TweenerCore<Color, Color, ColorOptions> DOColor(this Text target, Color endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003AFC File Offset: 0x00001CFC
		public static TweenerCore<int, int, NoOptions> DOCounter(this Text target, int fromValue, int endValue, float duration, bool addThousandsSeparator = true, CultureInfo culture = null)
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

		// Token: 0x0600004E RID: 78 RVA: 0x00003B70 File Offset: 0x00001D70
		public static TweenerCore<Color, Color, ColorOptions> DOFade(this Text target, float endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003BB8 File Offset: 0x00001DB8
		public static TweenerCore<string, string, StringOptions> DOText(this Text target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = 0, string scrambleChars = null)
		{
			if (endValue == null)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogWarning("You can't pass a NULL string to DOText: an empty string will be used instead to avoid errors", null);
				}
				endValue = "";
			}
			TweenerCore<string, string, StringOptions> tweenerCore = DOTween.To(() => target.text, delegate(string x)
			{
				target.text = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, richTextEnabled, scrambleMode, scrambleChars), target);
			return tweenerCore;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003C28 File Offset: 0x00001E28
		public static Tweener DOBlendableColor(this Graphic target, Color endValue, float duration)
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

		// Token: 0x06000051 RID: 81 RVA: 0x00003CA4 File Offset: 0x00001EA4
		public static Tweener DOBlendableColor(this Image target, Color endValue, float duration)
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

		// Token: 0x06000052 RID: 82 RVA: 0x00003D20 File Offset: 0x00001F20
		public static Tweener DOBlendableColor(this Text target, Color endValue, float duration)
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

		// Token: 0x06000053 RID: 83 RVA: 0x00003D9C File Offset: 0x00001F9C
		public static TweenerCore<Vector2, Vector2, CircleOptions> DOShapeCircle(this RectTransform target, Vector2 center, float endValueDegrees, float duration, bool relativeCenter = false, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, CircleOptions> tweenerCore = DOTween.To<Vector2, Vector2, CircleOptions>(CirclePlugin.Get(), () => target.anchoredPosition, delegate(Vector2 x)
			{
				target.anchoredPosition = x;
			}, center, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, endValueDegrees, relativeCenter, snapping), target);
			return tweenerCore;
		}

		// Token: 0x02000027 RID: 39
		public static class Utils
		{
			// Token: 0x060000AF RID: 175 RVA: 0x00004860 File Offset: 0x00002A60
			public static Vector2 SwitchToRectTransform(RectTransform from, RectTransform to)
			{
				Vector2 b = new Vector2(from.rect.width * 0.5f + from.rect.xMin, from.rect.height * 0.5f + from.rect.yMin);
				Vector2 vector = RectTransformUtility.WorldToScreenPoint(null, from.position);
				vector += b;
				Vector2 b2;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(to, vector, null, out b2);
				Vector2 b3 = new Vector2(to.rect.width * 0.5f + to.rect.xMin, to.rect.height * 0.5f + to.rect.yMin);
				return to.anchoredPosition + b2 - b3;
			}
		}
	}
}
