using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000028 RID: 40
	public class QuaternionPlugin : ABSTweenPlugin<Quaternion, Vector3, QuaternionOptions>
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void Reset(TweenerCore<Quaternion, Vector3, QuaternionOptions> t)
		{
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000AB8C File Offset: 0x00008D8C
		public override void SetFrom(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, bool isRelative)
		{
			Vector3 endValue = t.endValue;
			t.endValue = t.getter().eulerAngles;
			if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative)
			{
				t.startValue = endValue;
			}
			else if (t.plugOptions.rotateMode == RotateMode.FastBeyond360)
			{
				t.startValue = t.endValue + endValue;
			}
			else
			{
				Quaternion quaternion = t.getter();
				if (t.plugOptions.rotateMode == RotateMode.WorldAxisAdd)
				{
					t.startValue = (quaternion * Quaternion.Inverse(quaternion) * Quaternion.Euler(endValue) * quaternion).eulerAngles;
				}
				else
				{
					t.startValue = (quaternion * Quaternion.Euler(endValue)).eulerAngles;
				}
				t.endValue = -endValue;
			}
			t.setter(Quaternion.Euler(t.startValue));
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000AC80 File Offset: 0x00008E80
		public override void SetFrom(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, Vector3 fromValue, bool setImmediately, bool isRelative)
		{
			if (isRelative)
			{
				Vector3 eulerAngles = t.getter().eulerAngles;
				t.endValue += eulerAngles;
				fromValue += eulerAngles;
			}
			t.startValue = fromValue;
			if (setImmediately)
			{
				t.setter(Quaternion.Euler(fromValue));
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000ACDB File Offset: 0x00008EDB
		public override Vector3 ConvertToStartValue(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, Quaternion value)
		{
			return value.eulerAngles;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000ACE4 File Offset: 0x00008EE4
		public override void SetRelativeEndValue(TweenerCore<Quaternion, Vector3, QuaternionOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000AD00 File Offset: 0x00008F00
		public override void SetChangeValue(TweenerCore<Quaternion, Vector3, QuaternionOptions> t)
		{
			if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative)
			{
				Vector3 endValue = t.endValue;
				if (endValue.x > 360f)
				{
					endValue.x %= 360f;
				}
				if (endValue.y > 360f)
				{
					endValue.y %= 360f;
				}
				if (endValue.z > 360f)
				{
					endValue.z %= 360f;
				}
				Vector3 vector = endValue - t.startValue;
				float num = (vector.x > 0f) ? vector.x : (-vector.x);
				if (num > 180f)
				{
					vector.x = ((vector.x > 0f) ? (-(360f - num)) : (360f - num));
				}
				num = ((vector.y > 0f) ? vector.y : (-vector.y));
				if (num > 180f)
				{
					vector.y = ((vector.y > 0f) ? (-(360f - num)) : (360f - num));
				}
				num = ((vector.z > 0f) ? vector.z : (-vector.z));
				if (num > 180f)
				{
					vector.z = ((vector.z > 0f) ? (-(360f - num)) : (360f - num));
				}
				t.changeValue = vector;
				return;
			}
			if (t.plugOptions.rotateMode == RotateMode.FastBeyond360 || t.isRelative)
			{
				t.changeValue = t.endValue - t.startValue;
				return;
			}
			t.changeValue = t.endValue;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000AEBC File Offset: 0x000090BC
		public override float GetSpeedBasedDuration(QuaternionOptions options, float unitsXSecond, Vector3 changeValue)
		{
			return changeValue.magnitude / unitsXSecond;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000AEC8 File Offset: 0x000090C8
		public override void EvaluateAndApply(QuaternionOptions options, Tween t, bool isRelative, DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (options.dynamicLookAt)
			{
				TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = (TweenerCore<Quaternion, Vector3, QuaternionOptions>)t;
				tweenerCore.endValue = options.dynamicLookAtWorldPosition;
				SpecialPluginsUtils.SetLookAt(tweenerCore);
				this.SetChangeValue(tweenerCore);
				changeValue = tweenerCore.changeValue;
			}
			Vector3 vector = startValue;
			if (t.loopType == LoopType.Incremental)
			{
				vector += changeValue * (float)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				vector += changeValue * (float)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (float)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			RotateMode rotateMode = options.rotateMode;
			if (rotateMode - RotateMode.WorldAxisAdd > 1)
			{
				vector.x += changeValue.x * num;
				vector.y += changeValue.y * num;
				vector.z += changeValue.z * num;
				setter(Quaternion.Euler(vector));
				return;
			}
			Quaternion quaternion = Quaternion.Euler(startValue);
			vector.x = changeValue.x * num;
			vector.y = changeValue.y * num;
			vector.z = changeValue.z * num;
			if (options.rotateMode == RotateMode.WorldAxisAdd)
			{
				setter(quaternion * Quaternion.Inverse(quaternion) * Quaternion.Euler(vector) * quaternion);
				return;
			}
			setter(quaternion * Quaternion.Euler(vector));
		}
	}
}
