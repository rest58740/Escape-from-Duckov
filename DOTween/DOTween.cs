using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000009 RID: 9
	public class DOTween
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000020D1 File Offset: 0x000002D1
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000020D8 File Offset: 0x000002D8
		public static LogBehaviour logBehaviour
		{
			get
			{
				return DOTween._logBehaviour;
			}
			set
			{
				DOTween._logBehaviour = value;
				Debugger.SetLogPriority(DOTween._logBehaviour);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000020EA File Offset: 0x000002EA
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002101 File Offset: 0x00000301
		public static bool debugStoreTargetId
		{
			get
			{
				return DOTween.debugMode && DOTween.useSafeMode && DOTween._fooDebugStoreTargetId;
			}
			set
			{
				DOTween._fooDebugStoreTargetId = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002109 File Offset: 0x00000309
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002131 File Offset: 0x00000331
		internal static bool isQuitting
		{
			get
			{
				if (!DOTween._foo_isQuitting)
				{
					return false;
				}
				if (Time.frameCount >= 0 && DOTween._isQuittingFrame != Time.frameCount)
				{
					DOTween._foo_isQuitting = false;
					return false;
				}
				return true;
			}
			set
			{
				DOTween._foo_isQuitting = value;
				if (value)
				{
					DOTween._isQuittingFrame = Time.frameCount;
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002146 File Offset: 0x00000346
		public static IDOTweenInit Init(bool? recycleAllByDefault = null, bool? useSafeMode = null, LogBehaviour? logBehaviour = null)
		{
			if (DOTween.initialized)
			{
				return DOTween.instance;
			}
			if (!Application.isPlaying || DOTween.isQuitting)
			{
				return null;
			}
			return DOTween.Init(Resources.Load("DOTweenSettings") as DOTweenSettings, recycleAllByDefault, useSafeMode, logBehaviour);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000217C File Offset: 0x0000037C
		private static void AutoInit()
		{
			if (!Application.isPlaying || DOTween.isQuitting)
			{
				return;
			}
			DOTween.Init(Resources.Load("DOTweenSettings") as DOTweenSettings, null, null, null);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000021C8 File Offset: 0x000003C8
		private static IDOTweenInit Init(DOTweenSettings settings, bool? recycleAllByDefault, bool? useSafeMode, LogBehaviour? logBehaviour)
		{
			DOTween.initialized = true;
			if (recycleAllByDefault != null)
			{
				DOTween.defaultRecyclable = recycleAllByDefault.Value;
			}
			if (useSafeMode != null)
			{
				DOTween.useSafeMode = useSafeMode.Value;
			}
			if (logBehaviour != null)
			{
				DOTween.logBehaviour = logBehaviour.Value;
			}
			DOTweenComponent.Create();
			if (settings != null)
			{
				if (useSafeMode == null)
				{
					DOTween.useSafeMode = settings.useSafeMode;
				}
				if (logBehaviour == null)
				{
					DOTween.logBehaviour = settings.logBehaviour;
				}
				if (recycleAllByDefault == null)
				{
					DOTween.defaultRecyclable = settings.defaultRecyclable;
				}
				DOTween.safeModeLogBehaviour = settings.safeModeOptions.logBehaviour;
				DOTween.nestedTweenFailureBehaviour = settings.safeModeOptions.nestedTweenFailureBehaviour;
				DOTween.timeScale = settings.timeScale;
				DOTween.useSmoothDeltaTime = settings.useSmoothDeltaTime;
				DOTween.maxSmoothUnscaledTime = settings.maxSmoothUnscaledTime;
				DOTween.rewindCallbackMode = settings.rewindCallbackMode;
				DOTween.defaultRecyclable = ((recycleAllByDefault == null) ? settings.defaultRecyclable : recycleAllByDefault.Value);
				DOTween.showUnityEditorReport = settings.showUnityEditorReport;
				DOTween.drawGizmos = settings.drawGizmos;
				DOTween.defaultAutoPlay = settings.defaultAutoPlay;
				DOTween.defaultUpdateType = settings.defaultUpdateType;
				DOTween.defaultTimeScaleIndependent = settings.defaultTimeScaleIndependent;
				DOTween.defaultEaseType = settings.defaultEaseType;
				DOTween.defaultEaseOvershootOrAmplitude = settings.defaultEaseOvershootOrAmplitude;
				DOTween.defaultEasePeriod = settings.defaultEasePeriod;
				DOTween.defaultAutoKill = settings.defaultAutoKill;
				DOTween.defaultLoopType = settings.defaultLoopType;
				DOTween.debugMode = settings.debugMode;
				DOTween.debugStoreTargetId = settings.debugStoreTargetId;
			}
			if (Debugger.logPriority >= 2)
			{
				Debugger.Log(string.Concat(new string[]
				{
					"DOTween initialization (useSafeMode: ",
					DOTween.useSafeMode.ToString(),
					", recycling: ",
					DOTween.defaultRecyclable ? "ON" : "OFF",
					", logBehaviour: ",
					DOTween.logBehaviour.ToString(),
					")"
				}));
			}
			return DOTween.instance;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000023CA File Offset: 0x000005CA
		public static void SetTweensCapacity(int tweenersCapacity, int sequencesCapacity)
		{
			TweenManager.SetCapacities(tweenersCapacity, sequencesCapacity);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000023D3 File Offset: 0x000005D3
		public static void Clear(bool destroy = false)
		{
			DOTween.Clear(destroy, false);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000023DC File Offset: 0x000005DC
		internal static void Clear(bool destroy, bool isApplicationQuitting)
		{
			TweenManager.PurgeAll(isApplicationQuitting);
			PluginsManager.PurgeAll();
			if (!destroy)
			{
				return;
			}
			DOTween.initialized = false;
			DOTween.useSafeMode = false;
			DOTween.safeModeLogBehaviour = SafeModeLogBehaviour.Warning;
			DOTween.nestedTweenFailureBehaviour = NestedTweenFailureBehaviour.TryToPreserveSequence;
			DOTween.showUnityEditorReport = false;
			DOTween.drawGizmos = true;
			DOTween.timeScale = 1f;
			DOTween.useSmoothDeltaTime = false;
			DOTween.maxSmoothUnscaledTime = 0.15f;
			DOTween.rewindCallbackMode = RewindCallbackMode.FireIfPositionChanged;
			DOTween.logBehaviour = LogBehaviour.ErrorsOnly;
			DOTween.onWillLog = null;
			DOTween.defaultEaseType = Ease.OutQuad;
			DOTween.defaultEaseOvershootOrAmplitude = 1.70158f;
			DOTween.defaultEasePeriod = 0f;
			DOTween.defaultUpdateType = UpdateType.Normal;
			DOTween.defaultTimeScaleIndependent = false;
			DOTween.defaultAutoPlay = AutoPlay.All;
			DOTween.defaultLoopType = LoopType.Restart;
			DOTween.defaultAutoKill = true;
			DOTween.defaultRecyclable = false;
			DOTween.maxActiveTweenersReached = (DOTween.maxActiveSequencesReached = 0);
			DOTween.GizmosDelegates.Clear();
			DOTweenComponent.DestroyInstance();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000024A1 File Offset: 0x000006A1
		public static void ClearCachedTweens()
		{
			TweenManager.PurgePools();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000024A8 File Offset: 0x000006A8
		public static int Validate()
		{
			return TweenManager.Validate();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000024AF File Offset: 0x000006AF
		public static void ManualUpdate(float deltaTime, float unscaledDeltaTime)
		{
			DOTween.InitCheck();
			if (TweenManager.hasActiveManualTweens)
			{
				TweenManager.Update(UpdateType.Manual, deltaTime * DOTween.timeScale, unscaledDeltaTime * DOTween.timeScale);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000024D1 File Offset: 0x000006D1
		public static TweenerCore<float, float, FloatOptions> To(DOGetter<float> getter, DOSetter<float> setter, float endValue, float duration)
		{
			return DOTween.ApplyTo<float, float, FloatOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000024DD File Offset: 0x000006DD
		public static TweenerCore<double, double, NoOptions> To(DOGetter<double> getter, DOSetter<double> setter, double endValue, float duration)
		{
			return DOTween.ApplyTo<double, double, NoOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000024E9 File Offset: 0x000006E9
		public static TweenerCore<int, int, NoOptions> To(DOGetter<int> getter, DOSetter<int> setter, int endValue, float duration)
		{
			return DOTween.ApplyTo<int, int, NoOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000024F5 File Offset: 0x000006F5
		public static TweenerCore<uint, uint, UintOptions> To(DOGetter<uint> getter, DOSetter<uint> setter, uint endValue, float duration)
		{
			return DOTween.ApplyTo<uint, uint, UintOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002501 File Offset: 0x00000701
		public static TweenerCore<long, long, NoOptions> To(DOGetter<long> getter, DOSetter<long> setter, long endValue, float duration)
		{
			return DOTween.ApplyTo<long, long, NoOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000250D File Offset: 0x0000070D
		public static TweenerCore<ulong, ulong, NoOptions> To(DOGetter<ulong> getter, DOSetter<ulong> setter, ulong endValue, float duration)
		{
			return DOTween.ApplyTo<ulong, ulong, NoOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002519 File Offset: 0x00000719
		public static TweenerCore<string, string, StringOptions> To(DOGetter<string> getter, DOSetter<string> setter, string endValue, float duration)
		{
			return DOTween.ApplyTo<string, string, StringOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002525 File Offset: 0x00000725
		public static TweenerCore<Vector2, Vector2, VectorOptions> To(DOGetter<Vector2> getter, DOSetter<Vector2> setter, Vector2 endValue, float duration)
		{
			return DOTween.ApplyTo<Vector2, Vector2, VectorOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002531 File Offset: 0x00000731
		public static TweenerCore<Vector3, Vector3, VectorOptions> To(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3 endValue, float duration)
		{
			return DOTween.ApplyTo<Vector3, Vector3, VectorOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000253D File Offset: 0x0000073D
		public static TweenerCore<Vector4, Vector4, VectorOptions> To(DOGetter<Vector4> getter, DOSetter<Vector4> setter, Vector4 endValue, float duration)
		{
			return DOTween.ApplyTo<Vector4, Vector4, VectorOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002549 File Offset: 0x00000749
		public static TweenerCore<Quaternion, Vector3, QuaternionOptions> To(DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, Vector3 endValue, float duration)
		{
			return DOTween.ApplyTo<Quaternion, Vector3, QuaternionOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002555 File Offset: 0x00000755
		public static TweenerCore<Color, Color, ColorOptions> To(DOGetter<Color> getter, DOSetter<Color> setter, Color endValue, float duration)
		{
			return DOTween.ApplyTo<Color, Color, ColorOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002561 File Offset: 0x00000761
		public static TweenerCore<Rect, Rect, RectOptions> To(DOGetter<Rect> getter, DOSetter<Rect> setter, Rect endValue, float duration)
		{
			return DOTween.ApplyTo<Rect, Rect, RectOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000256D File Offset: 0x0000076D
		public static Tweener To(DOGetter<RectOffset> getter, DOSetter<RectOffset> setter, RectOffset endValue, float duration)
		{
			return DOTween.ApplyTo<RectOffset, RectOffset, NoOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002579 File Offset: 0x00000779
		public static TweenerCore<T1, T2, TPlugOptions> To<T1, T2, TPlugOptions>(ABSTweenPlugin<T1, T2, TPlugOptions> plugin, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration) where TPlugOptions : struct, IPlugOptions
		{
			return DOTween.ApplyTo<T1, T2, TPlugOptions>(getter, setter, endValue, duration, plugin);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002586 File Offset: 0x00000786
		public static TweenerCore<Vector3, Vector3, VectorOptions> ToAxis(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue, float duration, AxisConstraint axisConstraint = AxisConstraint.X)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.ApplyTo<Vector3, Vector3, VectorOptions>(getter, setter, new Vector3(endValue, endValue, endValue), duration, null);
			tweenerCore.plugOptions.axisConstraint = axisConstraint;
			return tweenerCore;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000025A6 File Offset: 0x000007A6
		public static TweenerCore<Color, Color, ColorOptions> ToAlpha(DOGetter<Color> getter, DOSetter<Color> setter, float endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ApplyTo<Color, Color, ColorOptions>(getter, setter, new Color(0f, 0f, 0f, endValue), duration, null);
			tweenerCore.SetOptions(true);
			return tweenerCore;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000025D0 File Offset: 0x000007D0
		public static Tweener To(DOSetter<float> setter, float startValue, float endValue, float duration)
		{
			return DOTween.To(() => startValue, delegate(float x)
			{
				startValue = x;
				setter(startValue);
			}, endValue, duration).NoFrom<float, float, FloatOptions>();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002618 File Offset: 0x00000818
		public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Punch(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3 direction, float duration, int vibrato = 10, float elasticity = 1f)
		{
			if (elasticity > 1f)
			{
				elasticity = 1f;
			}
			else if (elasticity < 0f)
			{
				elasticity = 0f;
			}
			float num = direction.magnitude;
			int num2 = (int)((float)vibrato * duration);
			if (num2 < 2)
			{
				num2 = 2;
			}
			float num3 = num / (float)num2;
			float[] array = new float[num2];
			float num4 = 0f;
			for (int i = 0; i < num2; i++)
			{
				float num5 = (float)(i + 1) / (float)num2;
				float num6 = duration * num5;
				num4 += num6;
				array[i] = num6;
			}
			float num7 = duration / num4;
			for (int j = 0; j < num2; j++)
			{
				array[j] *= num7;
			}
			Vector3[] array2 = new Vector3[num2];
			for (int k = 0; k < num2; k++)
			{
				if (k < num2 - 1)
				{
					if (k == 0)
					{
						array2[k] = direction;
					}
					else if (k % 2 != 0)
					{
						array2[k] = -Vector3.ClampMagnitude(direction, num * elasticity);
					}
					else
					{
						array2[k] = Vector3.ClampMagnitude(direction, num);
					}
					num -= num3;
				}
				else
				{
					array2[k] = Vector3.zero;
				}
			}
			return DOTween.ToArray(getter, setter, array2, array).NoFrom<Vector3, Vector3[], Vector3ArrayOptions>().SetSpecialStartupMode(SpecialStartupMode.SetPunch);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002748 File Offset: 0x00000948
		public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Shake(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float duration, float strength = 3f, int vibrato = 10, float randomness = 90f, bool ignoreZAxis = true, bool fadeOut = true)
		{
			return DOTween.Shake(getter, setter, duration, new Vector3(strength, strength, strength), vibrato, randomness, ignoreZAxis, false, fadeOut);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002770 File Offset: 0x00000970
		public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Shake(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			return DOTween.Shake(getter, setter, duration, strength, vibrato, randomness, false, true, fadeOut);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002790 File Offset: 0x00000990
		private static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Shake(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float duration, Vector3 strength, int vibrato, float randomness, bool ignoreZAxis, bool vectorBased, bool fadeOut)
		{
			float num = vectorBased ? strength.magnitude : strength.x;
			int num2 = (int)((float)vibrato * duration);
			if (num2 < 2)
			{
				num2 = 2;
			}
			float num3 = num / (float)num2;
			float[] array = new float[num2];
			float num4 = 0f;
			for (int i = 0; i < num2; i++)
			{
				float num5 = (float)(i + 1) / (float)num2;
				float num6 = fadeOut ? (duration * num5) : (duration / (float)num2);
				num4 += num6;
				array[i] = num6;
			}
			float num7 = duration / num4;
			for (int j = 0; j < num2; j++)
			{
				array[j] *= num7;
			}
			float num8 = Random.Range(0f, 360f);
			Vector3[] array2 = new Vector3[num2];
			for (int k = 0; k < num2; k++)
			{
				if (k < num2 - 1)
				{
					if (k > 0)
					{
						num8 = num8 - 180f + Random.Range(-randomness, randomness);
					}
					if (vectorBased)
					{
						Vector3 vector = Quaternion.AngleAxis(Random.Range(-randomness, randomness), Vector3.up) * DOTweenUtils.Vector3FromAngle(num8, num);
						vector.x = Vector3.ClampMagnitude(vector, strength.x).x;
						vector.y = Vector3.ClampMagnitude(vector, strength.y).y;
						vector.z = Vector3.ClampMagnitude(vector, strength.z).z;
						vector = vector.normalized * num;
						array2[k] = vector;
						if (fadeOut)
						{
							num -= num3;
						}
						strength = Vector3.ClampMagnitude(strength, num);
					}
					else
					{
						if (ignoreZAxis)
						{
							array2[k] = DOTweenUtils.Vector3FromAngle(num8, num);
						}
						else
						{
							Quaternion rotation = Quaternion.AngleAxis(Random.Range(-randomness, randomness), Vector3.up);
							array2[k] = rotation * DOTweenUtils.Vector3FromAngle(num8, num);
						}
						if (fadeOut)
						{
							num -= num3;
						}
					}
				}
				else
				{
					array2[k] = Vector3.zero;
				}
			}
			return DOTween.ToArray(getter, setter, array2, array).NoFrom<Vector3, Vector3[], Vector3ArrayOptions>().SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002990 File Offset: 0x00000B90
		public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> ToArray(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3[] endValues, float[] durations)
		{
			int num = durations.Length;
			if (num != endValues.Length)
			{
				Debugger.LogError("To Vector3 array tween: endValues and durations arrays must have the same length", null);
				return null;
			}
			Vector3[] array = new Vector3[num];
			float[] array2 = new float[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = endValues[i];
				array2[i] = durations[i];
			}
			float num2 = 0f;
			for (int j = 0; j < num; j++)
			{
				num2 += array2[j];
			}
			TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> tweenerCore = DOTween.ApplyTo<Vector3, Vector3[], Vector3ArrayOptions>(getter, setter, array, num2, null).NoFrom<Vector3, Vector3[], Vector3ArrayOptions>();
			tweenerCore.plugOptions.durations = array2;
			return tweenerCore;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A22 File Offset: 0x00000C22
		internal static TweenerCore<Color2, Color2, ColorOptions> To(DOGetter<Color2> getter, DOSetter<Color2> setter, Color2 endValue, float duration)
		{
			return DOTween.ApplyTo<Color2, Color2, ColorOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002A2E File Offset: 0x00000C2E
		public static Sequence Sequence()
		{
			DOTween.InitCheck();
			Sequence sequence = TweenManager.GetSequence();
			DG.Tweening.Sequence.Setup(sequence);
			return sequence;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002A40 File Offset: 0x00000C40
		public static Sequence Sequence(object target)
		{
			return DOTween.Sequence().SetTarget(target);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002A4D File Offset: 0x00000C4D
		public static int CompleteAll(bool withCallbacks = false)
		{
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.All, null, false, (float)(withCallbacks ? 1 : 0), null, null);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002A62 File Offset: 0x00000C62
		public static int Complete(object targetOrId, bool withCallbacks = false)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.TargetOrId, targetOrId, false, (float)(withCallbacks ? 1 : 0), null, null);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002A7C File Offset: 0x00000C7C
		internal static int CompleteAndReturnKilledTot()
		{
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.All, null, true, 0f, null, null);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002A8E File Offset: 0x00000C8E
		internal static int CompleteAndReturnKilledTot(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.TargetOrId, targetOrId, true, 0f, null, null);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002AA5 File Offset: 0x00000CA5
		internal static int CompleteAndReturnKilledTot(object target, object id)
		{
			if (target == null || id == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.TargetAndId, id, true, 0f, target, null);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002ABF File Offset: 0x00000CBF
		internal static int CompleteAndReturnKilledTotExceptFor(params object[] excludeTargetsOrIds)
		{
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.AllExceptTargetsOrIds, null, true, 0f, null, excludeTargetsOrIds);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002AD1 File Offset: 0x00000CD1
		public static int FlipAll()
		{
			return TweenManager.FilteredOperation(OperationType.Flip, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002AE3 File Offset: 0x00000CE3
		public static int Flip(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Flip, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002AFA File Offset: 0x00000CFA
		public static int GotoAll(float to, bool andPlay = false)
		{
			return TweenManager.FilteredOperation(OperationType.Goto, FilterType.All, null, andPlay, to, null, null);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002B08 File Offset: 0x00000D08
		public static int Goto(object targetOrId, float to, bool andPlay = false)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Goto, FilterType.TargetOrId, targetOrId, andPlay, to, null, null);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002B1B File Offset: 0x00000D1B
		public static int KillAll(bool complete = false)
		{
			return (complete ? DOTween.CompleteAndReturnKilledTot() : 0) + TweenManager.DespawnAll();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002B2E File Offset: 0x00000D2E
		public static int KillAll(bool complete, params object[] idsOrTargetsToExclude)
		{
			if (idsOrTargetsToExclude == null)
			{
				return (complete ? DOTween.CompleteAndReturnKilledTot() : 0) + TweenManager.DespawnAll();
			}
			return (complete ? DOTween.CompleteAndReturnKilledTotExceptFor(idsOrTargetsToExclude) : 0) + TweenManager.FilteredOperation(OperationType.Despawn, FilterType.AllExceptTargetsOrIds, null, false, 0f, null, idsOrTargetsToExclude);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002B62 File Offset: 0x00000D62
		public static int Kill(object targetOrId, bool complete = false)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return (complete ? DOTween.CompleteAndReturnKilledTot(targetOrId) : 0) + TweenManager.FilteredOperation(OperationType.Despawn, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002B86 File Offset: 0x00000D86
		public static int Kill(object target, object id, bool complete = false)
		{
			if (target == null || id == null)
			{
				return 0;
			}
			return (complete ? DOTween.CompleteAndReturnKilledTot(target, id) : 0) + TweenManager.FilteredOperation(OperationType.Despawn, FilterType.TargetAndId, id, false, 0f, target, null);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002BAE File Offset: 0x00000DAE
		public static int PauseAll()
		{
			return TweenManager.FilteredOperation(OperationType.Pause, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002BC0 File Offset: 0x00000DC0
		public static int Pause(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Pause, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002BD7 File Offset: 0x00000DD7
		public static int PlayAll()
		{
			return TweenManager.FilteredOperation(OperationType.Play, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002BE9 File Offset: 0x00000DE9
		public static int Play(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Play, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002C00 File Offset: 0x00000E00
		public static int Play(object target, object id)
		{
			if (target == null || id == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Play, FilterType.TargetAndId, id, false, 0f, target, null);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002C1A File Offset: 0x00000E1A
		public static int PlayBackwardsAll()
		{
			return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002C2C File Offset: 0x00000E2C
		public static int PlayBackwards(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002C43 File Offset: 0x00000E43
		public static int PlayBackwards(object target, object id)
		{
			if (target == null || id == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.TargetAndId, id, false, 0f, target, null);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002C5D File Offset: 0x00000E5D
		public static int PlayForwardAll()
		{
			return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002C6F File Offset: 0x00000E6F
		public static int PlayForward(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002C86 File Offset: 0x00000E86
		public static int PlayForward(object target, object id)
		{
			if (target == null || id == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.TargetAndId, id, false, 0f, target, null);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002CA0 File Offset: 0x00000EA0
		public static int RestartAll(bool includeDelay = true)
		{
			return TweenManager.FilteredOperation(OperationType.Restart, FilterType.All, null, includeDelay, 0f, null, null);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002CB3 File Offset: 0x00000EB3
		public static int Restart(object targetOrId, bool includeDelay = true, float changeDelayTo = -1f)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Restart, FilterType.TargetOrId, targetOrId, includeDelay, changeDelayTo, null, null);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002CC7 File Offset: 0x00000EC7
		public static int Restart(object target, object id, bool includeDelay = true, float changeDelayTo = -1f)
		{
			if (target == null || id == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Restart, FilterType.TargetAndId, id, includeDelay, changeDelayTo, target, null);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002CDE File Offset: 0x00000EDE
		public static int RewindAll(bool includeDelay = true)
		{
			return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.All, null, includeDelay, 0f, null, null);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002CF0 File Offset: 0x00000EF0
		public static int Rewind(object targetOrId, bool includeDelay = true)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.TargetOrId, targetOrId, includeDelay, 0f, null, null);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002D07 File Offset: 0x00000F07
		public static int SmoothRewindAll()
		{
			return TweenManager.FilteredOperation(OperationType.SmoothRewind, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002D1A File Offset: 0x00000F1A
		public static int SmoothRewind(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.SmoothRewind, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002D32 File Offset: 0x00000F32
		public static int TogglePauseAll()
		{
			return TweenManager.FilteredOperation(OperationType.TogglePause, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002D45 File Offset: 0x00000F45
		public static int TogglePause(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.TogglePause, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002D5D File Offset: 0x00000F5D
		public static bool IsTweening(object targetOrId, bool alsoCheckIfIsPlaying = false)
		{
			return TweenManager.FilteredOperation(OperationType.IsTweening, FilterType.TargetOrId, targetOrId, alsoCheckIfIsPlaying, 0f, null, null) > 0;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002D73 File Offset: 0x00000F73
		public static int TotalActiveTweens()
		{
			return TweenManager.totActiveTweens;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002D7A File Offset: 0x00000F7A
		public static int TotalPlayingTweens()
		{
			return TweenManager.TotalPlayingTweens();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002D81 File Offset: 0x00000F81
		public static List<Tween> PlayingTweens(List<Tween> fillableList = null)
		{
			if (fillableList != null)
			{
				fillableList.Clear();
			}
			return TweenManager.GetActiveTweens(true, fillableList);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002D93 File Offset: 0x00000F93
		public static List<Tween> PausedTweens(List<Tween> fillableList = null)
		{
			if (fillableList != null)
			{
				fillableList.Clear();
			}
			return TweenManager.GetActiveTweens(false, fillableList);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002DA5 File Offset: 0x00000FA5
		public static List<Tween> TweensById(object id, bool playingOnly = false, List<Tween> fillableList = null)
		{
			if (id == null)
			{
				return null;
			}
			if (fillableList != null)
			{
				fillableList.Clear();
			}
			return TweenManager.GetTweensById(id, playingOnly, fillableList);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002DBD File Offset: 0x00000FBD
		public static List<Tween> TweensByTarget(object target, bool playingOnly = false, List<Tween> fillableList = null)
		{
			if (fillableList != null)
			{
				fillableList.Clear();
			}
			return TweenManager.GetTweensByTarget(target, playingOnly, fillableList);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002DD0 File Offset: 0x00000FD0
		private static void InitCheck()
		{
			if (DOTween.initialized || !Application.isPlaying || DOTween.isQuitting)
			{
				return;
			}
			DOTween.AutoInit();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002DF0 File Offset: 0x00000FF0
		private static TweenerCore<T1, T2, TPlugOptions> ApplyTo<T1, T2, TPlugOptions>(DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration, ABSTweenPlugin<T1, T2, TPlugOptions> plugin = null) where TPlugOptions : struct, IPlugOptions
		{
			DOTween.InitCheck();
			TweenerCore<T1, T2, TPlugOptions> tweener = TweenManager.GetTweener<T1, T2, TPlugOptions>();
			if (!Tweener.Setup<T1, T2, TPlugOptions>(tweener, getter, setter, endValue, duration, plugin))
			{
				TweenManager.Despawn(tweener, true);
				return null;
			}
			return tweener;
		}

		// Token: 0x0400000E RID: 14
		public static readonly string Version = "1.2.632";

		// Token: 0x0400000F RID: 15
		public static bool useSafeMode = true;

		// Token: 0x04000010 RID: 16
		public static SafeModeLogBehaviour safeModeLogBehaviour = SafeModeLogBehaviour.Warning;

		// Token: 0x04000011 RID: 17
		public static NestedTweenFailureBehaviour nestedTweenFailureBehaviour = NestedTweenFailureBehaviour.TryToPreserveSequence;

		// Token: 0x04000012 RID: 18
		public static bool showUnityEditorReport = false;

		// Token: 0x04000013 RID: 19
		public static float timeScale = 1f;

		// Token: 0x04000014 RID: 20
		public static bool useSmoothDeltaTime;

		// Token: 0x04000015 RID: 21
		public static float maxSmoothUnscaledTime = 0.15f;

		// Token: 0x04000016 RID: 22
		internal static RewindCallbackMode rewindCallbackMode = RewindCallbackMode.FireIfPositionChanged;

		// Token: 0x04000017 RID: 23
		private static LogBehaviour _logBehaviour = LogBehaviour.ErrorsOnly;

		// Token: 0x04000018 RID: 24
		public static Func<LogType, object, bool> onWillLog;

		// Token: 0x04000019 RID: 25
		public static bool drawGizmos = true;

		// Token: 0x0400001A RID: 26
		public static bool debugMode = false;

		// Token: 0x0400001B RID: 27
		private static bool _fooDebugStoreTargetId = true;

		// Token: 0x0400001C RID: 28
		public static UpdateType defaultUpdateType = UpdateType.Normal;

		// Token: 0x0400001D RID: 29
		public static bool defaultTimeScaleIndependent = false;

		// Token: 0x0400001E RID: 30
		public static AutoPlay defaultAutoPlay = AutoPlay.All;

		// Token: 0x0400001F RID: 31
		public static bool defaultAutoKill = true;

		// Token: 0x04000020 RID: 32
		public static LoopType defaultLoopType = LoopType.Restart;

		// Token: 0x04000021 RID: 33
		public static bool defaultRecyclable;

		// Token: 0x04000022 RID: 34
		public static Ease defaultEaseType = Ease.OutQuad;

		// Token: 0x04000023 RID: 35
		public static float defaultEaseOvershootOrAmplitude = 1.70158f;

		// Token: 0x04000024 RID: 36
		public static float defaultEasePeriod = 0f;

		// Token: 0x04000025 RID: 37
		public static DOTweenComponent instance;

		// Token: 0x04000026 RID: 38
		private static bool _foo_isQuitting;

		// Token: 0x04000027 RID: 39
		internal static int maxActiveTweenersReached;

		// Token: 0x04000028 RID: 40
		internal static int maxActiveSequencesReached;

		// Token: 0x04000029 RID: 41
		internal static SafeModeReport safeModeReport;

		// Token: 0x0400002A RID: 42
		internal static readonly List<TweenCallback> GizmosDelegates = new List<TweenCallback>();

		// Token: 0x0400002B RID: 43
		internal static bool initialized;

		// Token: 0x0400002C RID: 44
		private static int _isQuittingFrame = -1;
	}
}
