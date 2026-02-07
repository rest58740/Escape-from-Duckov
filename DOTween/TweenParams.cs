using System;
using DG.Tweening.Core.Easing;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000017 RID: 23
	public class TweenParams
	{
		// Token: 0x06000114 RID: 276 RVA: 0x000069DB File Offset: 0x00004BDB
		public TweenParams()
		{
			this.Clear();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000069EC File Offset: 0x00004BEC
		public TweenParams Clear()
		{
			this.id = (this.target = null);
			this.updateType = DOTween.defaultUpdateType;
			this.isIndependentUpdate = DOTween.defaultTimeScaleIndependent;
			this.onStart = (this.onPlay = (this.onRewind = (this.onUpdate = (this.onStepComplete = (this.onComplete = (this.onKill = null))))));
			this.onWaypointChange = null;
			this.isRecyclable = DOTween.defaultRecyclable;
			this.isSpeedBased = false;
			this.autoKill = DOTween.defaultAutoKill;
			this.loops = 1;
			this.loopType = DOTween.defaultLoopType;
			this.delay = 0f;
			this.isRelative = false;
			this.easeType = Ease.Unset;
			this.customEase = null;
			this.easeOvershootOrAmplitude = DOTween.defaultEaseOvershootOrAmplitude;
			this.easePeriod = DOTween.defaultEasePeriod;
			return this;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00006AC9 File Offset: 0x00004CC9
		public TweenParams SetAutoKill(bool autoKillOnCompletion = true)
		{
			this.autoKill = autoKillOnCompletion;
			return this;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006AD3 File Offset: 0x00004CD3
		public TweenParams SetId(object id)
		{
			this.id = id;
			return this;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00006ADD File Offset: 0x00004CDD
		public TweenParams SetTarget(object target)
		{
			this.target = target;
			return this;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006AE7 File Offset: 0x00004CE7
		public TweenParams SetLoops(int loops, LoopType? loopType = null)
		{
			if (loops < -1)
			{
				loops = -1;
			}
			else if (loops == 0)
			{
				loops = 1;
			}
			this.loops = loops;
			if (loopType != null)
			{
				this.loopType = loopType.Value;
			}
			return this;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00006B18 File Offset: 0x00004D18
		public TweenParams SetEase(Ease ease, float? overshootOrAmplitude = null, float? period = null)
		{
			this.easeType = ease;
			this.easeOvershootOrAmplitude = ((overshootOrAmplitude != null) ? overshootOrAmplitude.Value : DOTween.defaultEaseOvershootOrAmplitude);
			this.easePeriod = ((period != null) ? period.Value : DOTween.defaultEasePeriod);
			this.customEase = null;
			return this;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00006B70 File Offset: 0x00004D70
		public TweenParams SetEase(AnimationCurve animCurve)
		{
			this.easeType = Ease.INTERNAL_Custom;
			this.customEase = new EaseFunction(new EaseCurve(animCurve).Evaluate);
			return this;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006B92 File Offset: 0x00004D92
		public TweenParams SetEase(EaseFunction customEase)
		{
			this.easeType = Ease.INTERNAL_Custom;
			this.customEase = customEase;
			return this;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006BA4 File Offset: 0x00004DA4
		public TweenParams SetRecyclable(bool recyclable = true)
		{
			this.isRecyclable = recyclable;
			return this;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00006BAE File Offset: 0x00004DAE
		public TweenParams SetUpdate(bool isIndependentUpdate)
		{
			this.updateType = DOTween.defaultUpdateType;
			this.isIndependentUpdate = isIndependentUpdate;
			return this;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006BC3 File Offset: 0x00004DC3
		public TweenParams SetUpdate(UpdateType updateType, bool isIndependentUpdate = false)
		{
			this.updateType = updateType;
			this.isIndependentUpdate = isIndependentUpdate;
			return this;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006BD4 File Offset: 0x00004DD4
		public TweenParams OnStart(TweenCallback action)
		{
			this.onStart = action;
			return this;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006BDE File Offset: 0x00004DDE
		public TweenParams OnPlay(TweenCallback action)
		{
			this.onPlay = action;
			return this;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006BE8 File Offset: 0x00004DE8
		public TweenParams OnRewind(TweenCallback action)
		{
			this.onRewind = action;
			return this;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006BF2 File Offset: 0x00004DF2
		public TweenParams OnUpdate(TweenCallback action)
		{
			this.onUpdate = action;
			return this;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006BFC File Offset: 0x00004DFC
		public TweenParams OnStepComplete(TweenCallback action)
		{
			this.onStepComplete = action;
			return this;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006C06 File Offset: 0x00004E06
		public TweenParams OnComplete(TweenCallback action)
		{
			this.onComplete = action;
			return this;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006C10 File Offset: 0x00004E10
		public TweenParams OnKill(TweenCallback action)
		{
			this.onKill = action;
			return this;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006C1A File Offset: 0x00004E1A
		public TweenParams OnWaypointChange(TweenCallback<int> action)
		{
			this.onWaypointChange = action;
			return this;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006C24 File Offset: 0x00004E24
		public TweenParams SetDelay(float delay)
		{
			this.delay = delay;
			return this;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006C2E File Offset: 0x00004E2E
		public TweenParams SetRelative(bool isRelative = true)
		{
			this.isRelative = isRelative;
			return this;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006C38 File Offset: 0x00004E38
		public TweenParams SetSpeedBased(bool isSpeedBased = true)
		{
			this.isSpeedBased = isSpeedBased;
			return this;
		}

		// Token: 0x0400007C RID: 124
		public static readonly TweenParams Params = new TweenParams();

		// Token: 0x0400007D RID: 125
		internal object id;

		// Token: 0x0400007E RID: 126
		internal object target;

		// Token: 0x0400007F RID: 127
		internal UpdateType updateType;

		// Token: 0x04000080 RID: 128
		internal bool isIndependentUpdate;

		// Token: 0x04000081 RID: 129
		internal TweenCallback onStart;

		// Token: 0x04000082 RID: 130
		internal TweenCallback onPlay;

		// Token: 0x04000083 RID: 131
		internal TweenCallback onRewind;

		// Token: 0x04000084 RID: 132
		internal TweenCallback onUpdate;

		// Token: 0x04000085 RID: 133
		internal TweenCallback onStepComplete;

		// Token: 0x04000086 RID: 134
		internal TweenCallback onComplete;

		// Token: 0x04000087 RID: 135
		internal TweenCallback onKill;

		// Token: 0x04000088 RID: 136
		internal TweenCallback<int> onWaypointChange;

		// Token: 0x04000089 RID: 137
		internal bool isRecyclable;

		// Token: 0x0400008A RID: 138
		internal bool isSpeedBased;

		// Token: 0x0400008B RID: 139
		internal bool autoKill;

		// Token: 0x0400008C RID: 140
		internal int loops;

		// Token: 0x0400008D RID: 141
		internal LoopType loopType;

		// Token: 0x0400008E RID: 142
		internal float delay;

		// Token: 0x0400008F RID: 143
		internal bool isRelative;

		// Token: 0x04000090 RID: 144
		internal Ease easeType;

		// Token: 0x04000091 RID: 145
		internal EaseFunction customEase;

		// Token: 0x04000092 RID: 146
		internal float easeOvershootOrAmplitude;

		// Token: 0x04000093 RID: 147
		internal float easePeriod;
	}
}
