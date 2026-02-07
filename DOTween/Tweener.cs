using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x0200001B RID: 27
	public abstract class Tweener : Tween
	{
		// Token: 0x06000190 RID: 400 RVA: 0x000087E4 File Offset: 0x000069E4
		internal Tweener()
		{
		}

		// Token: 0x06000191 RID: 401
		public abstract Tweener ChangeStartValue(object newStartValue, float newDuration = -1f);

		// Token: 0x06000192 RID: 402
		public abstract Tweener ChangeEndValue(object newEndValue, float newDuration = -1f, bool snapStartValue = false);

		// Token: 0x06000193 RID: 403
		public abstract Tweener ChangeEndValue(object newEndValue, bool snapStartValue);

		// Token: 0x06000194 RID: 404
		public abstract Tweener ChangeValues(object newStartValue, object newEndValue, float newDuration = -1f);

		// Token: 0x06000195 RID: 405
		internal abstract Tweener SetFrom(bool relative);

		// Token: 0x06000196 RID: 406 RVA: 0x000087F4 File Offset: 0x000069F4
		internal static bool Setup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration, ABSTweenPlugin<T1, T2, TPlugOptions> plugin = null) where TPlugOptions : struct, IPlugOptions
		{
			if (plugin != null)
			{
				t.tweenPlugin = plugin;
			}
			else
			{
				if (t.tweenPlugin == null)
				{
					t.tweenPlugin = PluginsManager.GetDefaultPlugin<T1, T2, TPlugOptions>();
				}
				if (t.tweenPlugin == null)
				{
					Debugger.LogError("No suitable plugin found for this type", null);
					return false;
				}
			}
			t.getter = getter;
			t.setter = setter;
			t.endValue = endValue;
			t.duration = duration;
			t.autoKill = DOTween.defaultAutoKill;
			t.isRecyclable = DOTween.defaultRecyclable;
			t.easeType = DOTween.defaultEaseType;
			t.easeOvershootOrAmplitude = DOTween.defaultEaseOvershootOrAmplitude;
			t.easePeriod = DOTween.defaultEasePeriod;
			t.loopType = DOTween.defaultLoopType;
			t.isPlaying = (DOTween.defaultAutoPlay == AutoPlay.All || DOTween.defaultAutoPlay == AutoPlay.AutoPlayTweeners);
			return true;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000088B0 File Offset: 0x00006AB0
		internal static float DoUpdateDelay<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, float elapsed) where TPlugOptions : struct, IPlugOptions
		{
			float delay = t.delay;
			if (elapsed > delay)
			{
				t.elapsedDelay = delay;
				t.delayComplete = true;
				return elapsed - delay;
			}
			t.elapsedDelay = elapsed;
			return 0f;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000088E8 File Offset: 0x00006AE8
		internal static bool DoStartup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
		{
			t.startupDone = true;
			if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
			{
				return false;
			}
			if (!t.hasManuallySetStartValue)
			{
				if (DOTween.useSafeMode)
				{
					try
					{
						if (t.isFrom)
						{
							t.SetFrom(t.isRelative && !t.isBlendable);
							t.isRelative = false;
						}
						else
						{
							t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
						}
						goto IL_F8;
					}
					catch (Exception ex)
					{
						if (Debugger.ShouldLogSafeModeCapturedError())
						{
							Debugger.LogSafeModeCapturedError(string.Format("Tween startup failed (NULL target/property - {0}): the tween will now be killed ► {1}", ex.TargetSite, ex.Message), t);
						}
						DOTween.safeModeReport.Add(SafeModeReport.SafeModeReportType.StartupFailure);
						return false;
					}
				}
				if (t.isFrom)
				{
					t.SetFrom(t.isRelative && !t.isBlendable);
					t.isRelative = false;
				}
				else
				{
					t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
				}
			}
			IL_F8:
			if (t.isRelative)
			{
				t.tweenPlugin.SetRelativeEndValue(t);
			}
			t.tweenPlugin.SetChangeValue(t);
			Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
			if (t.duration <= 0f)
			{
				t.easeType = Ease.INTERNAL_Zero;
			}
			return true;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00008A3C File Offset: 0x00006C3C
		internal static TweenerCore<T1, T2, TPlugOptions> DoChangeStartValue<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newStartValue, float newDuration) where TPlugOptions : struct, IPlugOptions
		{
			t.hasManuallySetStartValue = true;
			t.startValue = newStartValue;
			if (t.startupDone)
			{
				if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
				{
					return null;
				}
				t.tweenPlugin.SetChangeValue(t);
			}
			if (newDuration > 0f)
			{
				t.duration = newDuration;
				if (t.startupDone)
				{
					Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
				}
			}
			Tween.DoGoto(t, 0f, 0, UpdateMode.IgnoreOnUpdate);
			return t;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008AAC File Offset: 0x00006CAC
		internal static TweenerCore<T1, T2, TPlugOptions> DoChangeEndValue<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newEndValue, float newDuration, bool snapStartValue) where TPlugOptions : struct, IPlugOptions
		{
			t.endValue = newEndValue;
			t.isRelative = false;
			if (t.startupDone)
			{
				if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
				{
					return null;
				}
				if (snapStartValue)
				{
					if (DOTween.useSafeMode)
					{
						try
						{
							t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
							goto IL_B4;
						}
						catch (Exception ex)
						{
							if (Debugger.ShouldLogSafeModeCapturedError())
							{
								Debugger.LogSafeModeCapturedError(string.Format("Target or field is missing/null ({0}) ► {1}\n\n{2}\n\n", ex.TargetSite, ex.Message, ex.StackTrace), t);
							}
							TweenManager.Despawn(t, true);
							DOTween.safeModeReport.Add(SafeModeReport.SafeModeReportType.TargetOrFieldMissing);
							return null;
						}
					}
					t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
				}
				IL_B4:
				t.tweenPlugin.SetChangeValue(t);
			}
			if (newDuration > 0f)
			{
				t.duration = newDuration;
				if (t.startupDone)
				{
					Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
				}
			}
			Tween.DoGoto(t, 0f, 0, UpdateMode.IgnoreOnUpdate);
			return t;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008BB8 File Offset: 0x00006DB8
		internal static TweenerCore<T1, T2, TPlugOptions> DoChangeValues<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newStartValue, T2 newEndValue, float newDuration) where TPlugOptions : struct, IPlugOptions
		{
			t.hasManuallySetStartValue = true;
			t.isRelative = (t.isFrom = false);
			t.startValue = newStartValue;
			t.endValue = newEndValue;
			if (t.startupDone)
			{
				if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
				{
					return null;
				}
				t.tweenPlugin.SetChangeValue(t);
			}
			if (newDuration > 0f)
			{
				t.duration = newDuration;
				if (t.startupDone)
				{
					Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
				}
			}
			Tween.DoGoto(t, 0f, 0, UpdateMode.IgnoreOnUpdate);
			return t;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008C3C File Offset: 0x00006E3C
		private static bool DOStartupSpecials<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
		{
			bool result;
			try
			{
				switch (t.specialStartupMode)
				{
				case SpecialStartupMode.SetLookAt:
					if (!SpecialPluginsUtils.SetLookAt(t as TweenerCore<Quaternion, Vector3, QuaternionOptions>))
					{
						return false;
					}
					break;
				case SpecialStartupMode.SetShake:
					if (!SpecialPluginsUtils.SetShake(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>))
					{
						return false;
					}
					break;
				case SpecialStartupMode.SetPunch:
					if (!SpecialPluginsUtils.SetPunch(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>))
					{
						return false;
					}
					break;
				case SpecialStartupMode.SetCameraShakePosition:
					if (!SpecialPluginsUtils.SetCameraShakePosition(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>))
					{
						return false;
					}
					break;
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008CC8 File Offset: 0x00006EC8
		private static void DOStartupDurationBased<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
		{
			if (t.isSpeedBased)
			{
				t.duration = t.tweenPlugin.GetSpeedBasedDuration(t.plugOptions, t.duration, t.changeValue);
			}
			t.fullDuration = ((t.loops > -1) ? (t.duration * (float)t.loops) : float.PositiveInfinity);
		}

		// Token: 0x040000CB RID: 203
		internal bool hasManuallySetStartValue;

		// Token: 0x040000CC RID: 204
		internal bool isFromAllowed = true;
	}
}
