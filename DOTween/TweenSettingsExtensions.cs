using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000018 RID: 24
	public static class TweenSettingsExtensions
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00006C4E File Offset: 0x00004E4E
		public static T SetAutoKill<T>(this T t) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			t.autoKill = true;
			return t;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006C81 File Offset: 0x00004E81
		public static T SetAutoKill<T>(this T t, bool autoKillOnCompletion) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			t.autoKill = autoKillOnCompletion;
			return t;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006CB4 File Offset: 0x00004EB4
		public static T SetId<T>(this T t, object objectId) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.id = objectId;
			return t;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006CDA File Offset: 0x00004EDA
		public static T SetId<T>(this T t, string stringId) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.stringId = stringId;
			return t;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006D00 File Offset: 0x00004F00
		public static T SetId<T>(this T t, int intId) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.intId = intId;
			return t;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006D28 File Offset: 0x00004F28
		public static T SetLink<T>(this T t, GameObject gameObject) where T : Tween
		{
			if (t == null || !t.active || t.isSequenced || gameObject == null)
			{
				return t;
			}
			TweenManager.AddTweenLink(t, new TweenLink(gameObject, LinkBehaviour.KillOnDestroy));
			return t;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006D78 File Offset: 0x00004F78
		public static T SetLink<T>(this T t, GameObject gameObject, LinkBehaviour behaviour) where T : Tween
		{
			if (t == null || !t.active || t.isSequenced || gameObject == null)
			{
				return t;
			}
			TweenManager.AddTweenLink(t, new TweenLink(gameObject, behaviour));
			return t;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006DC8 File Offset: 0x00004FC8
		public static T SetTarget<T>(this T t, object target) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			if (DOTween.debugStoreTargetId)
			{
				Component component = target as Component;
				t.debugTargetId = ((component != null) ? component.name : target.ToString());
			}
			t.target = target;
			return t;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006E2C File Offset: 0x0000502C
		public static T SetLoops<T>(this T t, int loops) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			if (loops < -1)
			{
				loops = -1;
			}
			else if (loops == 0)
			{
				loops = 1;
			}
			t.loops = loops;
			if (t.tweenType == TweenType.Tweener)
			{
				if (loops > -1)
				{
					t.fullDuration = t.duration * (float)loops;
				}
				else
				{
					t.fullDuration = float.PositiveInfinity;
				}
			}
			return t;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006EB8 File Offset: 0x000050B8
		public static T SetLoops<T>(this T t, int loops, LoopType loopType) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			if (loops < -1)
			{
				loops = -1;
			}
			else if (loops == 0)
			{
				loops = 1;
			}
			t.loops = loops;
			t.loopType = loopType;
			if (t.tweenType == TweenType.Tweener)
			{
				if (loops > -1)
				{
					t.fullDuration = t.duration * (float)loops;
				}
				else
				{
					t.fullDuration = float.PositiveInfinity;
				}
			}
			return t;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006F50 File Offset: 0x00005150
		public static T SetEase<T>(this T t, Ease ease) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.easeType = ease;
			if (EaseManager.IsFlashEase(ease))
			{
				t.easeOvershootOrAmplitude = (float)((int)t.easeOvershootOrAmplitude);
			}
			t.customEase = null;
			return t;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006FB0 File Offset: 0x000051B0
		public static T SetEase<T>(this T t, Ease ease, float overshoot) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.easeType = ease;
			if (EaseManager.IsFlashEase(ease))
			{
				overshoot = (float)((int)overshoot);
			}
			t.easeOvershootOrAmplitude = overshoot;
			t.customEase = null;
			return t;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00007008 File Offset: 0x00005208
		public static T SetEase<T>(this T t, Ease ease, float amplitude, float period) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.easeType = ease;
			if (EaseManager.IsFlashEase(ease))
			{
				amplitude = (float)((int)amplitude);
			}
			t.easeOvershootOrAmplitude = amplitude;
			t.easePeriod = period;
			t.customEase = null;
			return t;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000706C File Offset: 0x0000526C
		public static T SetEase<T>(this T t, AnimationCurve animCurve) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.easeType = Ease.INTERNAL_Custom;
			t.customEase = new EaseFunction(new EaseCurve(animCurve).Evaluate);
			return t;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000070BA File Offset: 0x000052BA
		public static T SetEase<T>(this T t, EaseFunction customEase) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.easeType = Ease.INTERNAL_Custom;
			t.customEase = customEase;
			return t;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000070ED File Offset: 0x000052ED
		public static T SetRecyclable<T>(this T t) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.isRecyclable = true;
			return t;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007113 File Offset: 0x00005313
		public static T SetRecyclable<T>(this T t, bool recyclable) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.isRecyclable = recyclable;
			return t;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007139 File Offset: 0x00005339
		public static T SetUpdate<T>(this T t, bool isIndependentUpdate) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			TweenManager.SetUpdateType(t, DOTween.defaultUpdateType, isIndependentUpdate);
			return t;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00007164 File Offset: 0x00005364
		public static T SetUpdate<T>(this T t, UpdateType updateType) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			TweenManager.SetUpdateType(t, updateType, DOTween.defaultTimeScaleIndependent);
			return t;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000718F File Offset: 0x0000538F
		public static T SetUpdate<T>(this T t, UpdateType updateType, bool isIndependentUpdate) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			TweenManager.SetUpdateType(t, updateType, isIndependentUpdate);
			return t;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000071B6 File Offset: 0x000053B6
		public static T SetInverted<T>(this T t) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			t.isInverted = true;
			return t;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000071E9 File Offset: 0x000053E9
		public static T SetInverted<T>(this T t, bool inverted) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			t.isInverted = inverted;
			return t;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000721C File Offset: 0x0000541C
		public static T OnStart<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onStart = action;
			return t;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00007242 File Offset: 0x00005442
		public static T OnPlay<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onPlay = action;
			return t;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00007268 File Offset: 0x00005468
		public static T OnPause<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onPause = action;
			return t;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000728E File Offset: 0x0000548E
		public static T OnRewind<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onRewind = action;
			return t;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000072B4 File Offset: 0x000054B4
		public static T OnUpdate<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onUpdate = action;
			return t;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000072DA File Offset: 0x000054DA
		public static T OnStepComplete<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onStepComplete = action;
			return t;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007300 File Offset: 0x00005500
		public static T OnComplete<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onComplete = action;
			return t;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007326 File Offset: 0x00005526
		public static T OnKill<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onKill = action;
			return t;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000734C File Offset: 0x0000554C
		public static T OnWaypointChange<T>(this T t, TweenCallback<int> action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onWaypointChange = action;
			return t;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007374 File Offset: 0x00005574
		public static T SetAs<T>(this T t, Tween asTween) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			t.timeScale = asTween.timeScale;
			t.isBackwards = asTween.isBackwards;
			TweenManager.SetUpdateType(t, asTween.updateType, asTween.isIndependentUpdate);
			t.id = asTween.id;
			t.onStart = asTween.onStart;
			t.onPlay = asTween.onPlay;
			t.onRewind = asTween.onRewind;
			t.onUpdate = asTween.onUpdate;
			t.onStepComplete = asTween.onStepComplete;
			t.onComplete = asTween.onComplete;
			t.onKill = asTween.onKill;
			t.onWaypointChange = asTween.onWaypointChange;
			t.isRecyclable = asTween.isRecyclable;
			t.isSpeedBased = asTween.isSpeedBased;
			t.autoKill = asTween.autoKill;
			t.loops = asTween.loops;
			t.loopType = asTween.loopType;
			if (t.tweenType == TweenType.Tweener)
			{
				if (t.loops > -1)
				{
					t.fullDuration = t.duration * (float)t.loops;
				}
				else
				{
					t.fullDuration = float.PositiveInfinity;
				}
			}
			t.delay = asTween.delay;
			t.delayComplete = (t.delay <= 0f);
			t.isRelative = asTween.isRelative;
			t.easeType = asTween.easeType;
			t.customEase = asTween.customEase;
			t.easeOvershootOrAmplitude = asTween.easeOvershootOrAmplitude;
			t.easePeriod = asTween.easePeriod;
			return t;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000075A4 File Offset: 0x000057A4
		public static T SetAs<T>(this T t, TweenParams tweenParams) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			TweenManager.SetUpdateType(t, tweenParams.updateType, tweenParams.isIndependentUpdate);
			t.id = tweenParams.id;
			t.onStart = tweenParams.onStart;
			t.onPlay = tweenParams.onPlay;
			t.onRewind = tweenParams.onRewind;
			t.onUpdate = tweenParams.onUpdate;
			t.onStepComplete = tweenParams.onStepComplete;
			t.onComplete = tweenParams.onComplete;
			t.onKill = tweenParams.onKill;
			t.onWaypointChange = tweenParams.onWaypointChange;
			t.isRecyclable = tweenParams.isRecyclable;
			t.isSpeedBased = tweenParams.isSpeedBased;
			t.autoKill = tweenParams.autoKill;
			t.loops = tweenParams.loops;
			t.loopType = tweenParams.loopType;
			if (t.tweenType == TweenType.Tweener)
			{
				if (t.loops > -1)
				{
					t.fullDuration = t.duration * (float)t.loops;
				}
				else
				{
					t.fullDuration = float.PositiveInfinity;
				}
			}
			t.delay = tweenParams.delay;
			t.delayComplete = (t.delay <= 0f);
			t.isRelative = tweenParams.isRelative;
			if (tweenParams.easeType == Ease.Unset)
			{
				if (t.tweenType == TweenType.Sequence)
				{
					t.easeType = Ease.Linear;
				}
				else
				{
					t.easeType = DOTween.defaultEaseType;
				}
			}
			else
			{
				t.easeType = tweenParams.easeType;
			}
			t.customEase = tweenParams.customEase;
			t.easeOvershootOrAmplitude = tweenParams.easeOvershootOrAmplitude;
			t.easePeriod = tweenParams.easePeriod;
			return t;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000077E7 File Offset: 0x000059E7
		public static Sequence Append(this Sequence s, Tween t)
		{
			if (!TweenSettingsExtensions.ValidateAddToSequence(s, t, false))
			{
				return s;
			}
			Sequence.DoInsert(s, t, s.duration);
			return s;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007804 File Offset: 0x00005A04
		public static Sequence Prepend(this Sequence s, Tween t)
		{
			if (!TweenSettingsExtensions.ValidateAddToSequence(s, t, false))
			{
				return s;
			}
			Sequence.DoPrepend(s, t);
			return s;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000781B File Offset: 0x00005A1B
		public static Sequence Join(this Sequence s, Tween t)
		{
			if (!TweenSettingsExtensions.ValidateAddToSequence(s, t, false))
			{
				return s;
			}
			Sequence.DoInsert(s, t, s.lastTweenInsertTime);
			return s;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00007838 File Offset: 0x00005A38
		public static Sequence Insert(this Sequence s, float atPosition, Tween t)
		{
			if (!TweenSettingsExtensions.ValidateAddToSequence(s, t, false))
			{
				return s;
			}
			Sequence.DoInsert(s, t, atPosition);
			return s;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00007850 File Offset: 0x00005A50
		public static Sequence AppendInterval(this Sequence s, float interval)
		{
			if (!TweenSettingsExtensions.ValidateAddToSequence(s, null, true))
			{
				return s;
			}
			Sequence.DoAppendInterval(s, interval);
			return s;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00007867 File Offset: 0x00005A67
		public static Sequence PrependInterval(this Sequence s, float interval)
		{
			if (!TweenSettingsExtensions.ValidateAddToSequence(s, null, true))
			{
				return s;
			}
			Sequence.DoPrependInterval(s, interval);
			return s;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000787E File Offset: 0x00005A7E
		public static Sequence AppendCallback(this Sequence s, TweenCallback callback)
		{
			if (!TweenSettingsExtensions.ValidateAddToSequence(s, null, true))
			{
				return s;
			}
			if (callback == null)
			{
				return s;
			}
			Sequence.DoInsertCallback(s, callback, s.duration);
			return s;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000078A0 File Offset: 0x00005AA0
		public static Sequence PrependCallback(this Sequence s, TweenCallback callback)
		{
			if (!TweenSettingsExtensions.ValidateAddToSequence(s, null, true))
			{
				return s;
			}
			if (callback == null)
			{
				return s;
			}
			Sequence.DoInsertCallback(s, callback, 0f);
			return s;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000078C1 File Offset: 0x00005AC1
		public static Sequence InsertCallback(this Sequence s, float atPosition, TweenCallback callback)
		{
			if (!TweenSettingsExtensions.ValidateAddToSequence(s, null, true))
			{
				return s;
			}
			if (callback == null)
			{
				return s;
			}
			Sequence.DoInsertCallback(s, callback, atPosition);
			return s;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000078E0 File Offset: 0x00005AE0
		private static bool ValidateAddToSequence(Sequence s, Tween t, bool ignoreTween = false)
		{
			if (s == null)
			{
				Debugger.Sequence.LogAddToNullSequence();
				return false;
			}
			if (!s.active)
			{
				Debugger.Sequence.LogAddToInactiveSequence();
				return false;
			}
			if (s.creationLocked)
			{
				Debugger.Sequence.LogAddToLockedSequence();
				return false;
			}
			if (!ignoreTween)
			{
				if (t == null)
				{
					Debugger.Sequence.LogAddNullTween();
					return false;
				}
				if (!t.active)
				{
					Debugger.Sequence.LogAddInactiveTween(t);
					return false;
				}
				if (t.isSequenced)
				{
					Debugger.Sequence.LogAddAlreadySequencedTween(t);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00007943 File Offset: 0x00005B43
		public static T From<T>(this T t) where T : Tweener
		{
			return t.From(true, false);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000794D File Offset: 0x00005B4D
		public static T From<T>(this T t, bool isRelative) where T : Tweener
		{
			return t.From(true, isRelative);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007958 File Offset: 0x00005B58
		public static T From<T>(this T t, bool setImmediately, bool isRelative) where T : Tweener
		{
			if (t == null || !t.active || t.creationLocked || !t.isFromAllowed)
			{
				return t;
			}
			t.isFrom = true;
			if (setImmediately)
			{
				t.SetFrom(isRelative && !t.isBlendable);
			}
			else
			{
				t.isRelative = isRelative;
			}
			return t;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000079D4 File Offset: 0x00005BD4
		public static TweenerCore<T1, T2, TPlugOptions> From<T1, T2, TPlugOptions>(this TweenerCore<T1, T2, TPlugOptions> t, T2 fromValue, bool setImmediately = true, bool isRelative = false) where TPlugOptions : struct, IPlugOptions
		{
			if (t == null || !t.active || t.creationLocked || !t.isFromAllowed)
			{
				return t;
			}
			t.isFrom = true;
			t.SetFrom(fromValue, setImmediately, isRelative);
			return t;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007A08 File Offset: 0x00005C08
		public static TweenerCore<Color, Color, ColorOptions> From(this TweenerCore<Color, Color, ColorOptions> t, float fromAlphaValue, bool setImmediately = true, bool isRelative = false)
		{
			if (t == null || !t.active || t.creationLocked || !t.isFromAllowed)
			{
				return t;
			}
			t.isFrom = true;
			t.SetFrom(new Color(0f, 0f, 0f, fromAlphaValue), setImmediately, isRelative);
			return t;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00007A58 File Offset: 0x00005C58
		public static TweenerCore<Vector3, Vector3, VectorOptions> From(this TweenerCore<Vector3, Vector3, VectorOptions> t, float fromValue, bool setImmediately = true, bool isRelative = false)
		{
			if (t == null || !t.active || t.creationLocked || !t.isFromAllowed)
			{
				return t;
			}
			t.isFrom = true;
			t.SetFrom(new Vector3(fromValue, fromValue, fromValue), setImmediately, isRelative);
			return t;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007A90 File Offset: 0x00005C90
		public static TweenerCore<Vector2, Vector2, CircleOptions> From(this TweenerCore<Vector2, Vector2, CircleOptions> t, float fromValueDegrees, bool setImmediately = true, bool isRelative = false)
		{
			if (t == null || !t.active || t.creationLocked || !t.isFromAllowed)
			{
				return t;
			}
			t.isFrom = true;
			t.SetFrom(new Vector2(fromValueDegrees, 0f), setImmediately, isRelative);
			return t;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007ACC File Offset: 0x00005CCC
		public static T SetDelay<T>(this T t, float delay) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			if (t.tweenType == TweenType.Sequence)
			{
				(t as Sequence).PrependInterval(delay);
			}
			else
			{
				t.delay = delay;
				t.delayComplete = (delay <= 0f);
			}
			return t;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007B44 File Offset: 0x00005D44
		public static T SetDelay<T>(this T t, float delay, bool asPrependedIntervalIfSequence) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			if (t.tweenType != TweenType.Sequence || !asPrependedIntervalIfSequence)
			{
				t.delay = delay;
				t.delayComplete = (delay <= 0f);
			}
			else
			{
				(t as Sequence).PrependInterval(delay);
			}
			return t;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00007BC0 File Offset: 0x00005DC0
		public static T SetRelative<T>(this T t) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked || t.isFrom || t.isBlendable)
			{
				return t;
			}
			t.isRelative = true;
			return t;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007C18 File Offset: 0x00005E18
		public static T SetRelative<T>(this T t, bool isRelative) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked || t.isFrom || t.isBlendable)
			{
				return t;
			}
			t.isRelative = isRelative;
			return t;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007C70 File Offset: 0x00005E70
		public static T SetSpeedBased<T>(this T t) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			t.isSpeedBased = true;
			return t;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007CA3 File Offset: 0x00005EA3
		public static T SetSpeedBased<T>(this T t, bool isSpeedBased) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			t.isSpeedBased = isSpeedBased;
			return t;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007CD6 File Offset: 0x00005ED6
		public static Tweener SetOptions(this TweenerCore<float, float, FloatOptions> t, bool snapping)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00007CF2 File Offset: 0x00005EF2
		public static Tweener SetOptions(this TweenerCore<Vector2, Vector2, VectorOptions> t, bool snapping)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007D0E File Offset: 0x00005F0E
		public static Tweener SetOptions(this TweenerCore<Vector2, Vector2, VectorOptions> t, AxisConstraint axisConstraint, bool snapping = false)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.axisConstraint = axisConstraint;
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007D36 File Offset: 0x00005F36
		public static Tweener SetOptions(this TweenerCore<Vector3, Vector3, VectorOptions> t, bool snapping)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007D52 File Offset: 0x00005F52
		public static Tweener SetOptions(this TweenerCore<Vector3, Vector3, VectorOptions> t, AxisConstraint axisConstraint, bool snapping = false)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.axisConstraint = axisConstraint;
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007D7A File Offset: 0x00005F7A
		public static Tweener SetOptions(this TweenerCore<Vector4, Vector4, VectorOptions> t, bool snapping)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007D96 File Offset: 0x00005F96
		public static Tweener SetOptions(this TweenerCore<Vector4, Vector4, VectorOptions> t, AxisConstraint axisConstraint, bool snapping = false)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.axisConstraint = axisConstraint;
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00007DBE File Offset: 0x00005FBE
		public static Tweener SetOptions(this TweenerCore<Quaternion, Vector3, QuaternionOptions> t, bool useShortest360Route = true)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.rotateMode = (useShortest360Route ? RotateMode.Fast : RotateMode.FastBeyond360);
			return t;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007DE0 File Offset: 0x00005FE0
		public static Tweener SetOptions(this TweenerCore<Color, Color, ColorOptions> t, bool alphaOnly)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.alphaOnly = alphaOnly;
			return t;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007DFC File Offset: 0x00005FFC
		public static Tweener SetOptions(this TweenerCore<Rect, Rect, RectOptions> t, bool snapping)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007E18 File Offset: 0x00006018
		public static Tweener SetOptions(this TweenerCore<string, string, StringOptions> t, bool richTextEnabled, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.richTextEnabled = richTextEnabled;
			t.plugOptions.scrambleMode = scrambleMode;
			if (!string.IsNullOrEmpty(scrambleChars))
			{
				if (scrambleChars.Length <= 1)
				{
					scrambleChars += scrambleChars;
				}
				t.plugOptions.scrambledChars = scrambleChars.ToCharArray();
				t.plugOptions.scrambledChars.ScrambleChars();
			}
			return t;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007E86 File Offset: 0x00006086
		public static Tweener SetOptions(this TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, bool snapping)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007EA2 File Offset: 0x000060A2
		public static Tweener SetOptions(this TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, AxisConstraint axisConstraint, bool snapping = false)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.axisConstraint = axisConstraint;
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007ECA File Offset: 0x000060CA
		public static Tweener SetOptions(this TweenerCore<Vector2, Vector2, CircleOptions> t, float endValueDegrees, bool relativeCenter = true, bool snapping = false)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.endValueDegrees = endValueDegrees;
			t.plugOptions.relativeCenter = relativeCenter;
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007EFE File Offset: 0x000060FE
		public static TweenerCore<Vector3, Path, PathOptions> SetOptions(this TweenerCore<Vector3, Path, PathOptions> t, AxisConstraint lockPosition, AxisConstraint lockRotation = AxisConstraint.None)
		{
			return t.SetOptions(false, lockPosition, lockRotation);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007F09 File Offset: 0x00006109
		public static TweenerCore<Vector3, Path, PathOptions> SetOptions(this TweenerCore<Vector3, Path, PathOptions> t, bool closePath, AxisConstraint lockPosition = AxisConstraint.None, AxisConstraint lockRotation = AxisConstraint.None)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.isClosedPath = closePath;
			t.plugOptions.lockPositionAxis = lockPosition;
			t.plugOptions.lockRotationAxis = lockRotation;
			return t;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007F3D File Offset: 0x0000613D
		public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, Vector3 lookAtPosition, Vector3? forwardDirection = null, Vector3? up = null)
		{
			return t.SetLookAt(OrientType.LookAtPosition, lookAtPosition, null, -1f, forwardDirection, up, false);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007F50 File Offset: 0x00006150
		public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, Vector3 lookAtPosition, bool stableZRotation)
		{
			return t.SetLookAt(OrientType.LookAtPosition, lookAtPosition, null, -1f, null, null, stableZRotation);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007F7E File Offset: 0x0000617E
		public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, Transform lookAtTransform, Vector3? forwardDirection = null, Vector3? up = null)
		{
			return t.SetLookAt(OrientType.LookAtTransform, Vector3.zero, lookAtTransform, -1f, forwardDirection, up, false);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007F98 File Offset: 0x00006198
		public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, Transform lookAtTransform, bool stableZRotation)
		{
			return t.SetLookAt(OrientType.LookAtTransform, Vector3.zero, lookAtTransform, -1f, null, null, stableZRotation);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007FCA File Offset: 0x000061CA
		public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, float lookAhead, Vector3? forwardDirection = null, Vector3? up = null)
		{
			return t.SetLookAt(OrientType.ToPath, Vector3.zero, null, lookAhead, forwardDirection, up, false);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007FE0 File Offset: 0x000061E0
		public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, float lookAhead, bool stableZRotation)
		{
			return t.SetLookAt(OrientType.ToPath, Vector3.zero, null, lookAhead, null, null, stableZRotation);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008010 File Offset: 0x00006210
		private static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, OrientType orientType, Vector3 lookAtPosition, Transform lookAtTransform, float lookAhead, Vector3? forwardDirection = null, Vector3? up = null, bool stableZRotation = false)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.orientType = orientType;
			switch (orientType)
			{
			case OrientType.ToPath:
				if (lookAhead < 0.0001f)
				{
					lookAhead = 0.0001f;
				}
				t.plugOptions.lookAhead = lookAhead;
				break;
			case OrientType.LookAtTransform:
				t.plugOptions.lookAtTransform = lookAtTransform;
				break;
			case OrientType.LookAtPosition:
				t.plugOptions.lookAtPosition = lookAtPosition;
				break;
			}
			t.plugOptions.lookAtPosition = lookAtPosition;
			t.plugOptions.stableZRotation = stableZRotation;
			t.SetPathForwardDirection(forwardDirection, up);
			return t;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000080AC File Offset: 0x000062AC
		private static void SetPathForwardDirection(this TweenerCore<Vector3, Path, PathOptions> t, Vector3? forwardDirection = null, Vector3? up = null)
		{
			if (t == null || !t.active)
			{
				return;
			}
			bool hasCustomForwardDirection;
			if (forwardDirection != null)
			{
				Vector3? vector = forwardDirection;
				Vector3 zero = Vector3.zero;
				if (vector == null || (vector != null && vector.GetValueOrDefault() != zero))
				{
					hasCustomForwardDirection = true;
					goto IL_86;
				}
			}
			if (up != null)
			{
				Vector3? vector = up;
				Vector3 zero = Vector3.zero;
				hasCustomForwardDirection = (vector == null || (vector != null && vector.GetValueOrDefault() != zero));
			}
			else
			{
				hasCustomForwardDirection = false;
			}
			IL_86:
			t.plugOptions.hasCustomForwardDirection = hasCustomForwardDirection;
			if (t.plugOptions.hasCustomForwardDirection)
			{
				Vector3? vector = forwardDirection;
				Vector3 zero = Vector3.zero;
				if (vector != null && (vector == null || vector.GetValueOrDefault() == zero))
				{
					forwardDirection = new Vector3?(Vector3.forward);
				}
				t.plugOptions.forward = Quaternion.LookRotation((forwardDirection == null) ? Vector3.forward : forwardDirection.Value, (up == null) ? Vector3.up : up.Value);
			}
		}
	}
}
