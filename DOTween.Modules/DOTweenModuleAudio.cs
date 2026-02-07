using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Audio;

namespace DG.Tweening
{
	// Token: 0x02000003 RID: 3
	public static class DOTweenModuleAudio
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public static TweenerCore<float, float, FloatOptions> DOFade(this AudioSource target, float endValue, float duration)
		{
			if (endValue < 0f)
			{
				endValue = 0f;
			}
			else if (endValue > 1f)
			{
				endValue = 1f;
			}
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.volume, delegate(float x)
			{
				target.volume = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<float, float, FloatOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002128 File Offset: 0x00000328
		public static TweenerCore<float, float, FloatOptions> DOPitch(this AudioSource target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.pitch, delegate(float x)
			{
				target.pitch = x;
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<float, float, FloatOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002170 File Offset: 0x00000370
		public static TweenerCore<float, float, FloatOptions> DOSetFloat(this AudioMixer target, string floatName, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(delegate()
			{
				float result;
				target.GetFloat(floatName, out result);
				return result;
			}, delegate(float x)
			{
				target.SetFloat(floatName, x);
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<float, float, FloatOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021BD File Offset: 0x000003BD
		public static int DOComplete(this AudioMixer target, bool withCallbacks = false)
		{
			return DOTween.Complete(target, withCallbacks);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021C6 File Offset: 0x000003C6
		public static int DOKill(this AudioMixer target, bool complete = false)
		{
			return DOTween.Kill(target, complete);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021CF File Offset: 0x000003CF
		public static int DOFlip(this AudioMixer target)
		{
			return DOTween.Flip(target);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021D7 File Offset: 0x000003D7
		public static int DOGoto(this AudioMixer target, float to, bool andPlay = false)
		{
			return DOTween.Goto(target, to, andPlay);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021E1 File Offset: 0x000003E1
		public static int DOPause(this AudioMixer target)
		{
			return DOTween.Pause(target);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021E9 File Offset: 0x000003E9
		public static int DOPlay(this AudioMixer target)
		{
			return DOTween.Play(target);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021F1 File Offset: 0x000003F1
		public static int DOPlayBackwards(this AudioMixer target)
		{
			return DOTween.PlayBackwards(target);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021F9 File Offset: 0x000003F9
		public static int DOPlayForward(this AudioMixer target)
		{
			return DOTween.PlayForward(target);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002201 File Offset: 0x00000401
		public static int DORestart(this AudioMixer target)
		{
			return DOTween.Restart(target, true, -1f);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000220F File Offset: 0x0000040F
		public static int DORewind(this AudioMixer target)
		{
			return DOTween.Rewind(target, true);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002218 File Offset: 0x00000418
		public static int DOSmoothRewind(this AudioMixer target)
		{
			return DOTween.SmoothRewind(target);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002220 File Offset: 0x00000420
		public static int DOTogglePause(this AudioMixer target)
		{
			return DOTween.TogglePause(target);
		}
	}
}
