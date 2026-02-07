using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000013 RID: 19
	public static class TweenExtensions
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00003139 File Offset: 0x00001339
		public static void Complete(this Tween t)
		{
			t.Complete(false);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003144 File Offset: 0x00001344
		public static void Complete(this Tween t, bool withCallbacks)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.Complete(t, true, withCallbacks ? UpdateMode.Update : UpdateMode.Goto);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000031A0 File Offset: 0x000013A0
		public static void Flip(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.Flip(t);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000031F4 File Offset: 0x000013F4
		public static void ForceInit(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.ForceInit(t, false);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003248 File Offset: 0x00001448
		public static void Goto(this Tween t, float to, bool andPlay = false)
		{
			TweenExtensions.DoGoto(t, to, andPlay, false);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003253 File Offset: 0x00001453
		public static void GotoWithCallbacks(this Tween t, float to, bool andPlay = false)
		{
			TweenExtensions.DoGoto(t, to, andPlay, true);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003260 File Offset: 0x00001460
		private static void DoGoto(Tween t, float to, bool andPlay, bool withCallbacks)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			if (to < 0f)
			{
				to = 0f;
			}
			if (!t.startupDone)
			{
				TweenManager.ForceInit(t, false);
			}
			TweenManager.Goto(t, to, andPlay, withCallbacks ? UpdateMode.Update : UpdateMode.Goto);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000032DC File Offset: 0x000014DC
		public static void Kill(this Tween t, bool complete = false)
		{
			if (!DOTween.initialized)
			{
				return;
			}
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			if (complete)
			{
				TweenManager.Complete(t, true, UpdateMode.Goto);
				if (t.autoKill && t.loops >= 0)
				{
					return;
				}
			}
			if (TweenManager.isUpdateLoop)
			{
				t.active = false;
				return;
			}
			TweenManager.Despawn(t, true);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003368 File Offset: 0x00001568
		public static void ManualUpdate(this Tween t, float deltaTime, float unscaledDeltaTime)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.Update(t, deltaTime, unscaledDeltaTime, true);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000033C0 File Offset: 0x000015C0
		public static T Pause<T>(this T t) where T : Tween
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return t;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return t;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return t;
			}
			TweenManager.Pause(t);
			return t;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000343C File Offset: 0x0000163C
		public static T Play<T>(this T t) where T : Tween
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return t;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return t;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return t;
			}
			TweenManager.Play(t);
			return t;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000034B8 File Offset: 0x000016B8
		public static void PlayBackwards(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.PlayBackwards(t);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000350C File Offset: 0x0000170C
		public static void PlayForward(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.PlayForward(t);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003560 File Offset: 0x00001760
		public static void Restart(this Tween t, bool includeDelay = true, float changeDelayTo = -1f)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.Restart(t, includeDelay, changeDelayTo);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000035B8 File Offset: 0x000017B8
		public static void Rewind(this Tween t, bool includeDelay = true)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.Rewind(t, includeDelay);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003610 File Offset: 0x00001810
		public static void SmoothRewind(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.SmoothRewind(t);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003664 File Offset: 0x00001864
		public static void TogglePause(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.TogglePause(t);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000036B8 File Offset: 0x000018B8
		public static void GotoWaypoint(this Tween t, int waypointIndex, bool andPlay = false)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = t as TweenerCore<Vector3, Path, PathOptions>;
			if (tweenerCore == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNonPathTween(t);
				}
				return;
			}
			if (!t.startupDone)
			{
				TweenManager.ForceInit(t, false);
			}
			if (waypointIndex < 0)
			{
				waypointIndex = 0;
			}
			else if (waypointIndex > tweenerCore.changeValue.wps.Length - 1)
			{
				waypointIndex = tweenerCore.changeValue.wps.Length - 1;
			}
			float num = 0f;
			for (int i = 0; i < waypointIndex + 1; i++)
			{
				num += tweenerCore.changeValue.wpLengths[i];
			}
			float num2 = num / tweenerCore.changeValue.length;
			if (t.hasLoops && t.loopType == LoopType.Yoyo && ((t.position < t.duration) ? (t.completedLoops % 2 != 0) : (t.completedLoops % 2 == 0)))
			{
				num2 = 1f - num2;
			}
			float to = (float)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops) * t.duration + num2 * t.duration;
			TweenManager.Goto(t, to, andPlay, UpdateMode.Goto);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003808 File Offset: 0x00001A08
		public static YieldInstruction WaitForCompletion(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return DOTween.instance.StartCoroutine(DOTween.instance.WaitForCompletion(t));
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003837 File Offset: 0x00001A37
		public static YieldInstruction WaitForRewind(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return DOTween.instance.StartCoroutine(DOTween.instance.WaitForRewind(t));
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003866 File Offset: 0x00001A66
		public static YieldInstruction WaitForKill(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return DOTween.instance.StartCoroutine(DOTween.instance.WaitForKill(t));
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003895 File Offset: 0x00001A95
		public static YieldInstruction WaitForElapsedLoops(this Tween t, int elapsedLoops)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return DOTween.instance.StartCoroutine(DOTween.instance.WaitForElapsedLoops(t, elapsedLoops));
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000038C5 File Offset: 0x00001AC5
		public static YieldInstruction WaitForPosition(this Tween t, float position)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return DOTween.instance.StartCoroutine(DOTween.instance.WaitForPosition(t, position));
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000038F5 File Offset: 0x00001AF5
		public static Coroutine WaitForStart(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return DOTween.instance.StartCoroutine(DOTween.instance.WaitForStart(t));
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003924 File Offset: 0x00001B24
		public static int CompletedLoops(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0;
			}
			return t.completedLoops;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003944 File Offset: 0x00001B44
		public static float Delay(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0f;
			}
			return t.delay;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003968 File Offset: 0x00001B68
		public static float ElapsedDelay(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0f;
			}
			return t.elapsedDelay;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000398C File Offset: 0x00001B8C
		public static float Duration(this Tween t, bool includeLoops = true)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0f;
			}
			if (!includeLoops)
			{
				return t.duration;
			}
			if (t.loops != -1)
			{
				return t.duration * (float)t.loops;
			}
			return float.PositiveInfinity;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000039DC File Offset: 0x00001BDC
		public static float Elapsed(this Tween t, bool includeLoops = true)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0f;
			}
			if (includeLoops)
			{
				return (float)((t.position >= t.duration) ? (t.completedLoops - 1) : t.completedLoops) * t.duration + t.position;
			}
			return t.position;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003A3C File Offset: 0x00001C3C
		public static float ElapsedPercentage(this Tween t, bool includeLoops = true)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0f;
			}
			if (!includeLoops)
			{
				return t.position / t.duration;
			}
			if (t.fullDuration <= 0f)
			{
				return 0f;
			}
			return ((float)((t.position >= t.duration) ? (t.completedLoops - 1) : t.completedLoops) * t.duration + t.position) / t.fullDuration;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003AC0 File Offset: 0x00001CC0
		public static float ElapsedDirectionalPercentage(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0f;
			}
			float num = t.position / t.duration;
			if (t.completedLoops <= 0 || !t.hasLoops || t.loopType != LoopType.Yoyo || ((t.isComplete || t.completedLoops % 2 == 0) && (!t.isComplete || t.completedLoops % 2 != 0)))
			{
				return num;
			}
			return 1f - num;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003B4A File Offset: 0x00001D4A
		public static bool IsActive(this Tween t)
		{
			return t != null && t.active;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003B57 File Offset: 0x00001D57
		public static bool IsBackwards(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return false;
			}
			return t.isBackwards;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003B77 File Offset: 0x00001D77
		public static bool IsComplete(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return false;
			}
			return t.isComplete;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003B97 File Offset: 0x00001D97
		public static bool IsInitialized(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return false;
			}
			return t.startupDone;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003BB7 File Offset: 0x00001DB7
		public static bool IsPlaying(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return false;
			}
			return t.isPlaying;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003BD7 File Offset: 0x00001DD7
		public static int Loops(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0;
			}
			return t.loops;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003BF8 File Offset: 0x00001DF8
		public static Vector3 PathGetPoint(this Tween t, float pathPercentage)
		{
			if (pathPercentage > 1f)
			{
				pathPercentage = 1f;
			}
			else if (pathPercentage < 0f)
			{
				pathPercentage = 0f;
			}
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return Vector3.zero;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return Vector3.zero;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return Vector3.zero;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = t as TweenerCore<Vector3, Path, PathOptions>;
			if (tweenerCore == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNonPathTween(t);
				}
				return Vector3.zero;
			}
			if (!tweenerCore.endValue.isFinalized)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogWarning("The path is not finalized yet", t);
				}
				return Vector3.zero;
			}
			return tweenerCore.endValue.GetPoint(pathPercentage, true);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003CC8 File Offset: 0x00001EC8
		public static Vector3[] PathGetDrawPoints(this Tween t, int subdivisionsXSegment = 10)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return null;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return null;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = t as TweenerCore<Vector3, Path, PathOptions>;
			if (tweenerCore == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNonPathTween(t);
				}
				return null;
			}
			if (!tweenerCore.endValue.isFinalized)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogWarning("The path is not finalized yet", t);
				}
				return null;
			}
			return Path.GetDrawPoints(tweenerCore.endValue, subdivisionsXSegment);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003D60 File Offset: 0x00001F60
		public static float PathLength(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return -1f;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return -1f;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return -1f;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = t as TweenerCore<Vector3, Path, PathOptions>;
			if (tweenerCore == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNonPathTween(t);
				}
				return -1f;
			}
			if (!tweenerCore.endValue.isFinalized)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogWarning("The path is not finalized yet", t);
				}
				return -1f;
			}
			return tweenerCore.endValue.length;
		}
	}
}
