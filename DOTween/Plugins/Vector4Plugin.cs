using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	// Token: 0x0200002D RID: 45
	public class Vector4Plugin : ABSTweenPlugin<Vector4, Vector4, VectorOptions>
	{
		// Token: 0x06000225 RID: 549 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void Reset(TweenerCore<Vector4, Vector4, VectorOptions> t)
		{
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000C224 File Offset: 0x0000A424
		public override void SetFrom(TweenerCore<Vector4, Vector4, VectorOptions> t, bool isRelative)
		{
			Vector4 endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			Vector4 vector = t.endValue;
			AxisConstraint axisConstraint = t.plugOptions.axisConstraint;
			if (axisConstraint <= AxisConstraint.Y)
			{
				if (axisConstraint == AxisConstraint.X)
				{
					vector.x = t.startValue.x;
					goto IL_B3;
				}
				if (axisConstraint == AxisConstraint.Y)
				{
					vector.y = t.startValue.y;
					goto IL_B3;
				}
			}
			else
			{
				if (axisConstraint == AxisConstraint.Z)
				{
					vector.z = t.startValue.z;
					goto IL_B3;
				}
				if (axisConstraint == AxisConstraint.W)
				{
					vector.w = t.startValue.w;
					goto IL_B3;
				}
			}
			vector = t.startValue;
			IL_B3:
			if (t.plugOptions.snapping)
			{
				vector.x = (float)Math.Round((double)vector.x);
				vector.y = (float)Math.Round((double)vector.y);
				vector.z = (float)Math.Round((double)vector.z);
				vector.w = (float)Math.Round((double)vector.w);
			}
			t.setter(vector);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000C350 File Offset: 0x0000A550
		public override void SetFrom(TweenerCore<Vector4, Vector4, VectorOptions> t, Vector4 fromValue, bool setImmediately, bool isRelative)
		{
			if (isRelative)
			{
				Vector4 b = t.getter();
				t.endValue += b;
				fromValue += b;
			}
			t.startValue = fromValue;
			if (setImmediately)
			{
				AxisConstraint axisConstraint = t.plugOptions.axisConstraint;
				Vector4 vector;
				if (axisConstraint <= AxisConstraint.Y)
				{
					if (axisConstraint == AxisConstraint.X)
					{
						vector = t.getter();
						vector.x = fromValue.x;
						goto IL_CB;
					}
					if (axisConstraint == AxisConstraint.Y)
					{
						vector = t.getter();
						vector.y = fromValue.y;
						goto IL_CB;
					}
				}
				else
				{
					if (axisConstraint == AxisConstraint.Z)
					{
						vector = t.getter();
						vector.z = fromValue.z;
						goto IL_CB;
					}
					if (axisConstraint == AxisConstraint.W)
					{
						vector = t.getter();
						vector.w = fromValue.w;
						goto IL_CB;
					}
				}
				vector = fromValue;
				IL_CB:
				if (t.plugOptions.snapping)
				{
					vector.x = (float)Math.Round((double)vector.x);
					vector.y = (float)Math.Round((double)vector.y);
					vector.z = (float)Math.Round((double)vector.z);
					vector.w = (float)Math.Round((double)vector.w);
				}
				t.setter(vector);
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00008F27 File Offset: 0x00007127
		public override Vector4 ConvertToStartValue(TweenerCore<Vector4, Vector4, VectorOptions> t, Vector4 value)
		{
			return value;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000C491 File Offset: 0x0000A691
		public override void SetRelativeEndValue(TweenerCore<Vector4, Vector4, VectorOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000C4AC File Offset: 0x0000A6AC
		public override void SetChangeValue(TweenerCore<Vector4, Vector4, VectorOptions> t)
		{
			AxisConstraint axisConstraint = t.plugOptions.axisConstraint;
			if (axisConstraint <= AxisConstraint.Y)
			{
				if (axisConstraint == AxisConstraint.X)
				{
					t.changeValue = new Vector4(t.endValue.x - t.startValue.x, 0f, 0f, 0f);
					return;
				}
				if (axisConstraint == AxisConstraint.Y)
				{
					t.changeValue = new Vector4(0f, t.endValue.y - t.startValue.y, 0f, 0f);
					return;
				}
			}
			else
			{
				if (axisConstraint == AxisConstraint.Z)
				{
					t.changeValue = new Vector4(0f, 0f, t.endValue.z - t.startValue.z, 0f);
					return;
				}
				if (axisConstraint == AxisConstraint.W)
				{
					t.changeValue = new Vector4(0f, 0f, 0f, t.endValue.w - t.startValue.w);
					return;
				}
			}
			t.changeValue = t.endValue - t.startValue;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000C5C6 File Offset: 0x0000A7C6
		public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector4 changeValue)
		{
			return changeValue.magnitude / unitsXSecond;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000C5D4 File Offset: 0x0000A7D4
		public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector4> getter, DOSetter<Vector4> setter, float elapsed, Vector4 startValue, Vector4 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (float)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			AxisConstraint axisConstraint = options.axisConstraint;
			if (axisConstraint <= AxisConstraint.Y)
			{
				if (axisConstraint == AxisConstraint.X)
				{
					Vector4 vector = getter();
					vector.x = startValue.x + changeValue.x * num;
					if (options.snapping)
					{
						vector.x = (float)Math.Round((double)vector.x);
					}
					setter(vector);
					return;
				}
				if (axisConstraint == AxisConstraint.Y)
				{
					Vector4 vector2 = getter();
					vector2.y = startValue.y + changeValue.y * num;
					if (options.snapping)
					{
						vector2.y = (float)Math.Round((double)vector2.y);
					}
					setter(vector2);
					return;
				}
			}
			else
			{
				if (axisConstraint == AxisConstraint.Z)
				{
					Vector4 vector3 = getter();
					vector3.z = startValue.z + changeValue.z * num;
					if (options.snapping)
					{
						vector3.z = (float)Math.Round((double)vector3.z);
					}
					setter(vector3);
					return;
				}
				if (axisConstraint == AxisConstraint.W)
				{
					Vector4 vector4 = getter();
					vector4.w = startValue.w + changeValue.w * num;
					if (options.snapping)
					{
						vector4.w = (float)Math.Round((double)vector4.w);
					}
					setter(vector4);
					return;
				}
			}
			startValue.x += changeValue.x * num;
			startValue.y += changeValue.y * num;
			startValue.z += changeValue.z * num;
			startValue.w += changeValue.w * num;
			if (options.snapping)
			{
				startValue.x = (float)Math.Round((double)startValue.x);
				startValue.y = (float)Math.Round((double)startValue.y);
				startValue.z = (float)Math.Round((double)startValue.z);
				startValue.w = (float)Math.Round((double)startValue.w);
			}
			setter(startValue);
		}
	}
}
